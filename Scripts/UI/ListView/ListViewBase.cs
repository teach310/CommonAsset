using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using Unity.Linq;
using System.Linq;
using UniRx.Toolkit;

namespace Common.UI
{
    public enum Layout
    {
        Vertical,
        Horizontal
    }


    /// <summary>
    /// Inspectable ReactiveProperty. 大きさは定数
    /// </summary>
    [Serializable]
    public class LayoutReactiveProperty : ReactiveProperty<Layout>
    {
        public LayoutReactiveProperty()
            : base()
        {

        }

        public LayoutReactiveProperty(Layout initialValue)
            : base(initialValue)
        {

        }
    }

    public class ListItemPool<T> : ObjectPool<ListViewItem<T>>
    {

        ListViewItem<T> prototype;
        Transform parent;
        Subject<ListViewItem<T>> onCreateItem = new Subject<ListViewItem<T>>();
        public IObservable<ListViewItem<T>> OnCreateItem
        {
            get { return onCreateItem; }
        }

        public ListItemPool(Transform parent, ListViewItem<T> prototype)
        {
            this.parent = parent;
            this.prototype = prototype;
        }

        protected override ListViewItem<T> CreateInstance()
        {
            var go = UnityEngine.Object.Instantiate(prototype.gameObject, parent) as GameObject;
            var item = go.GetComponent<ListViewItem<T>>();
            onCreateItem.OnNext(item);
            return item;
        }
    }

    // Scroll座標 ScrollPos  [0 -> ContentSize - DisplaySize] を正規化
    // List座標 ListItemの位置を表すための座標
    [RequireComponent(typeof(ScrollRect))]
    public abstract class ListViewBase<T> : ListViewEdInterface
    {
        public GameObject prototype;
        RectTransform protoRectTransform;
        protected RectTransform ProtoRectTransform{
            get{ 
                if(protoRectTransform == null)
                    protoRectTransform = prototype.GetComponent<RectTransform>();
                return protoRectTransform;
            }
        }


        public LayoutReactiveProperty layoutRp;
        protected ScrollRect scrollRect;
        public float baffer = 50f;
        // 全てのデータ
        List<T> dataList;
        bool isInitialized = false;

        protected readonly List<ListViewItem<T>> items = new List<ListViewItem<T>>();
        ListItemPool<T> pool;
        int firstActiveItemIndex;
        int lastActiveItemIndex;

        Subject<ListViewItem<T>> onShowItem = new Subject<ListViewItem<T>>();
        public IObservable<ListViewItem<T>> OnShowItem{
            get{return onShowItem;}
        }

        Subject<ListViewItem<T>> onHideItem = new Subject<ListViewItem<T>>();
        public IObservable<ListViewItem<T>> OnHideItem{
            get{return onHideItem;}
        }


        // 表示するデータ オーバーライドしてフィルターをかける
        protected List<T> DisplayedDataList
        {
            get { return dataList; }
        }

        // 表示する長さ　= scrollRectの範囲
        protected float DisplaySize
        {
            get
            {
                return layoutRp.Value == Layout.Horizontal ?
                               rectTransform.rect.width :
                               rectTransform.rect.height;
            }
        }

        protected float ContentSize
        {
            get
            {
                return layoutRp.Value == Layout.Horizontal ?
                               scrollRect.content.sizeDelta.x :
                               scrollRect.content.sizeDelta.y;
            }
            set
            {
                scrollRect.content.sizeDelta = layoutRp.Value == Layout.Horizontal
                    ? new Vector2(value, 0f)
                    : new Vector2(0f, value);
            }
        }

        // 現在のScroll位置を取得
        protected virtual float ScrollPos
        {
            get
            {
                return 1.0f - (layoutRp.Value == Layout.Horizontal
                               ? scrollRect.horizontalNormalizedPosition
                               : scrollRect.verticalNormalizedPosition);
            }
        }

        public struct ItemCoord
        {
            float listCoord;
            public float ListCoord { get { return listCoord; } }

            // Scroll座標における表示する範囲
            float rangeMin;
            public float RangeMin { get { return rangeMin; } }
            float rangeMax;
            public float RangeMax { get { return rangeMax; } }

            public ItemCoord(float c, float rMin, float rMax)
            {
                this.listCoord = c;
                this.rangeMin = rMin;
                this.rangeMax = rMax;
            }
        }
        protected List<ItemCoord> itemCoordList = new List<ItemCoord>();

        void Reset()
        {
            string contentName = "Content";

            // 構築
            if (this.gameObject.Children().Any(x => x.name == contentName))
            {
                return;
            }
            var go = new GameObject(contentName, typeof(RectTransform), typeof(RaycastDetector));
            var content = go.GetComponent<RectTransform>();
            content.SetParent(this.transform);
            content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.anchoredPosition = Vector2.zero;
            this.GetComponent<ScrollRect>().content = content;
        }

        protected virtual void InitializeIfNeeded()
        {
            if (isInitialized)
                return;
            scrollRect = this.GetComponent<ScrollRect>();
            layoutRp.Subscribe(OnLayoutChanged);


            scrollRect.OnValueChangedAsObservable()
                      .Select(pos => layoutRp.Value == Layout.Horizontal ? pos.x : pos.y)
                      .Pairwise()
                      .Subscribe(x =>
                      {
                          var val = x.Current - x.Previous;
                          if (val < 0)
                          {
                              OnScrollUp(-val);
                          }
                          else
                          {
                              OnScrollDown(val);
                          }
                      });

            pool = new ListItemPool<T>(scrollRect.content, prototype.GetComponent<ListViewItem<T>>());
            pool.AddTo(this);

            pool.OnCreateItem
                .Subscribe(x => items.Add(x));

            pool.OnCreateItem
                .Select(x => x.GetComponent<RectTransform>())
                .Subscribe(SetItemLayout);

            isInitialized = true;
        }

        public void SetData(List<T> dataList)
        {
            InitializeIfNeeded();
            this.dataList = dataList;
            ResetContent();
        }

        // 初期化して更新
        public override void ResetContent()
        {
            Debug.LogError("ResetContent");
            // スクロールする長さを設定
            ContentSize = CalcContentSize();
            SetItemCoord();
            ResetAllItems();
        }

        // 全探索で確実にアイテムを更新
        public void ResetAllItems(){
            ActivateItemsIfInRange();
            InactivateItemsIfOutOfRange();
            ResetTerminateItemIndex();
        }

        // 方向を考慮した，軽めの更新
        protected virtual void UpdateContent(bool isPositive)
        {
            ActivateItemsIfInRange(isPositive); // 範囲内に入ったアイテムを表示
            InactivateItemsIfOutOfRange(); // 範囲外のアイテムを非表示
            ResetTerminateItemIndex();
        }

        protected virtual void UpdateItemForIndex(ListViewItem<T> item, int index)
        {
            item.DataIndex = index;
            item.UpdateData(DisplayedDataList[index]);
            item.GetComponent<RectTransform>().anchoredPosition = GetItemPos(layoutRp.Value, itemCoordList[index].ListCoord);
        }

        // listCoordから位置を算出
        protected abstract Vector2 GetItemPos(Layout layout, float listCoord);

        // ContentのSizeを計算
        protected abstract float CalcContentSize();

        // 表示する領域をセット
        protected abstract void SetItemCoord();

        bool IsInRange(int index)
        {
            if(ContentSize < DisplaySize)
                return true;
            var currentPos = Mathf.Clamp01(ScrollPos);
            return itemCoordList[index].RangeMin <= currentPos && currentPos <= itemCoordList[index].RangeMax;
        }

        bool IsLessThanRangeMax(int index)
        {
            return Mathf.Clamp01(ScrollPos) <= itemCoordList[index].RangeMax;
        }

        bool IsMoreThanRangeMin(int index)
        {
            return itemCoordList[index].RangeMin <= Mathf.Clamp01(ScrollPos);
        }

        //表示されているアイテムの先頭とラストを保存
        void ResetTerminateItemIndex()
        {
            if (items.All(x => !x.gameObject.activeSelf))
                return;
            firstActiveItemIndex = items.Where(x => x.gameObject.activeSelf).Min(x => x.DataIndex);
            lastActiveItemIndex = items.Where(x => x.gameObject.activeSelf).Max(x => x.DataIndex);
        }

        // 全探索 重い
        void ActivateItemsIfInRange()
        {
            for (int i = 0; i < DisplayedDataList.Count; i++)
            {
                if (IsInRange(i))
                {
                    if (items.Where(x => x.gameObject.activeSelf).All(x => x.DataIndex != i))
                    {
                        var item = pool.Rent();
                        onShowItem.OnNext(item);
                        UpdateItemForIndex(item, i);
                    }
                }
            }
        }

        // アクティベート軽め
        void ActivateItemsIfInRange(bool isPositive)
        {
            if (isPositive)
            {
                for (int i = lastActiveItemIndex + 1; i < DisplayedDataList.Count; i++)
                {
                    if (IsInRange(i))
                    {
                        var item = pool.Rent();
                        onShowItem.OnNext(item);
                        UpdateItemForIndex(item, i);
                        lastActiveItemIndex = i;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                for (int i = firstActiveItemIndex - 1; i >= 0; i--)
                {
                    if (IsInRange(i))
                    {
                        var item = pool.Rent();
                        onShowItem.OnNext(item);
                        UpdateItemForIndex(item, i);
                        firstActiveItemIndex = i;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        void InactivateItemsIfOutOfRange()
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!IsInRange(item.DataIndex) && item.gameObject.activeSelf)
                {
                    pool.Return(item);
                    onHideItem.OnNext(item);
                }
            }
        }

        protected virtual void SetItemLayout(RectTransform rt)
        {
            switch (layoutRp.Value)
            {
                case Layout.Horizontal:
                    rt.pivot = new Vector2(0f, 0.5f);
                    rt.anchorMin = new Vector2(0f, 0.5f);
                    rt.anchorMax = new Vector2(0f, 0.5f);
                    break;
                case Layout.Vertical:
                    rt.pivot = new Vector2(0.5f, 1f);
                    rt.anchorMin = new Vector2(0.5f, 1f);
                    rt.anchorMax = new Vector2(0.5f, 1f);
                    break;
            }
        }

        protected float ConvertPosToScrollPos(float pos)
        {
            if (Mathf.Approximately(ContentSize, DisplaySize))
                return 0;

            return Mathf.Clamp01(pos / (ContentSize - DisplaySize));
        }

        #region Listener
        // Index0に近づく方向
        protected virtual void OnScrollUp(float delta)
        {
            if (delta > ConvertPosToScrollPos(DisplaySize))
            {
                ResetContent();
                return;
            }
            UpdateContent(true);
        }

        protected virtual void OnScrollDown(float delta)
        {
            if (delta > ConvertPosToScrollPos(DisplaySize))
            {
                ResetContent();
                return;
            }
            UpdateContent(false);
        }

        void OnLayoutChanged(Layout layout)
        {
            switch (layout)
            {
                case Layout.Horizontal:
                    scrollRect.horizontal = true;
                    scrollRect.vertical = false;
                    // 左から右
                    scrollRect.content.pivot = new Vector2(0f, 0.5f);
                    scrollRect.content.anchorMin = new Vector2(0f, 0f);
                    scrollRect.content.anchorMax = new Vector2(0f, 1f);
                    break;
                case Layout.Vertical:
                    scrollRect.horizontal = false;
                    scrollRect.vertical = true;
                    // 上から下
                    scrollRect.content.pivot = new Vector2(0.5f, 1f);
                    scrollRect.content.anchorMin = new Vector2(0f, 1f);
                    scrollRect.content.anchorMax = new Vector2(1f, 1f);
                    break;
            }

            foreach (var item in items)
            {
                SetItemLayout(item.GetComponent<RectTransform>());
            }
        }
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common.UI
{
    /// <summary>
    /// List座標 [0 -> ContentSize]
    /// </summary>
    public abstract class ListView<T> : ListViewBase<T>
    {
        public float space;
        public float paddingTop = 0f;
        public float paddingBottom = 0f;

        // listCoordから2次元座標を取得
        protected override Vector2 GetItemPos(Layout layout, float listCoord){
            var ret = Vector2.zero;
                switch(layout){
                case Layout.Horizontal :
                    ret = new Vector2(-listCoord, 0f);
                    break;
                case Layout.Vertical:
                    ret = new Vector2(0f, -listCoord);
                    break;
            }
            return ret;
        }

        // アイテムの長さの合計値
        protected override float CalcContentSize()
        {
            float estimatedSize = 0f;
            estimatedSize += paddingTop;
            for (int i = 0; i < DisplayedDataList.Count; i++)
            {
                estimatedSize += GetSize(DisplayedDataList[i]) + space;
            }
            estimatedSize -= space; // 最後はスペースいらない
            estimatedSize += paddingBottom;
            return estimatedSize;
        }

        // アイテムの高さ
        protected virtual float GetSize(T model)
        {
            var rect = prototype.GetComponent<RectTransform>().rect;
            return layoutRp.Value == Layout.Horizontal ? rect.width : rect.height;
        }

        protected override void SetItemCoord()
        {
            float estimatedSize = 0f;
            itemCoordList.Clear();
            estimatedSize += paddingTop;
            for (int i = 0; i < DisplayedDataList.Count; i++)
            {
                float listCoord = estimatedSize;

                // 最初のDisplaySize分は全て表示するため引く
                float min = ConvertPosToScrollPos(Mathf.Max(0, estimatedSize - baffer - DisplaySize));
                estimatedSize += GetSize(DisplayedDataList[i]);

                float max = ConvertPosToScrollPos(estimatedSize + baffer);
                itemCoordList.Add(new ItemCoord(listCoord, min, max));
                estimatedSize += space;
            }
        }
    }
}
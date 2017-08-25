using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI
{
    public class TGrid<T> : ListViewBase<T>
    {
        public Vector2 space;
        public float paddingTop = 0f;
        public float paddingBottom = 0f;
        public int constraintCount = 5;


        protected override Vector2 GetItemPos(Layout layout, float listCoord)
        {
            var ret = Vector2.zero;
            var index = (int)listCoord;
            int x = 0,y = 0;
            switch (layout)
            {
                case Layout.Horizontal:
                    x = index / constraintCount;
                    y = index % constraintCount;
                    ret = new Vector2(x * ProtoRectTransform.rect.width, -1 * y * ProtoRectTransform.rect.height);
                    break;
                case Layout.Vertical:
                    x = index % constraintCount;
                    y = index / constraintCount;
                    ret = new Vector2(x * ProtoRectTransform.rect.width, -1 * y * ProtoRectTransform.rect.height);
                    break;
            }

            // Spaceとpadding分を足す
            ret += new Vector2(x * space.x, -1 * y * space.y);
            ret += layoutRp.Value == Layout.Horizontal ? new Vector2(paddingTop, 0) : new Vector2(0, -paddingTop);
            return ret;

        }

        protected override float CalcContentSize()
        {
            float estimatedSize = 0f;
            estimatedSize += paddingTop;
            var currentSpace = (layoutRp.Value == Layout.Horizontal ? space.x : space.y);
            for (int i = 0; i < (DisplayedDataList.Count / constraintCount); i++)
            {
                estimatedSize += GetSize(DisplayedDataList[i]) + currentSpace;
            }
            estimatedSize -= currentSpace;
            estimatedSize += paddingBottom;
            return estimatedSize;

        }

        // アイテムの高さ
        protected virtual float GetSize(T model)
        {
            return layoutRp.Value == Layout.Horizontal ? ProtoRectTransform.rect.width : ProtoRectTransform.rect.height;
        }

        protected override void SetItemCoord()
        {
            float estimatedSize = 0f;
            itemCoordList.Clear();
            estimatedSize += paddingTop;
            var currentSpace = (layoutRp.Value == Layout.Horizontal ? space.x : space.y);
            for (int i = 0; i < DisplayedDataList.Count; i++)
            {
                float listCoord = (float)i;
                float min = ConvertPosToScrollPos(Mathf.Max(0, estimatedSize - baffer - DisplaySize));
                float max = ConvertPosToScrollPos(estimatedSize + GetSize(DisplayedDataList[i]) + baffer);
                itemCoordList.Add(new ItemCoord(listCoord, min, max));
                if((i % constraintCount) == constraintCount - 1)
                    estimatedSize += GetSize(DisplayedDataList[i]) + currentSpace;
            }
        }

        protected override void SetItemLayout(RectTransform rt)
        {
            rt.pivot = new Vector2(0f, 1f);
            // 左上
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
        }
    }
}
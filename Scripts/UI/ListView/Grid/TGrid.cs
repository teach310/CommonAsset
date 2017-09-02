using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI
{
    // Verticalのみの対応
    public class TGrid<T> : ListViewBase<T>
    {
        // 横は自動で計算
        public float space;
        float vertSpace;
        Vector2? space2 = null;
        Vector2 Space{
            get
            {
                if (space2 == null)
                {
                    // TODO Horizontal
                    vertSpace = (rectTransform.rect.width - (ProtoRectTransform.rect.width * constraintCount))
                        / (constraintCount -1);
                    space2 = new Vector2(vertSpace, space);
                }
                return space2.Value;
            }
        }


        public float paddingTop = 0f;
        public float paddingBottom = 0f;
        public int constraintCount = 5;


        protected override Vector2 GetItemPos(Layout layout, float listCoord)
        {
            var ret = Vector2.zero;
            var index = (int)listCoord;
            int x = 0, y = 0;
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
            ret += new Vector2(x * Space.x, -1 * y * Space.y);
            var unitVec = layoutRp.Value == Layout.Horizontal ? new Vector2(1, 0) : new Vector2(0, -1);
            ret += unitVec * (paddingTop + CalcEstimatedInsertedSpace(index));
            return ret;

        }

        protected override float CalcContentSize()
        {
            float estimatedSize = 0f;
            estimatedSize += paddingTop;
            var currentSpace = (layoutRp.Value == Layout.Horizontal ? Space.x : Space.y);
            for (int i = 0; i <= (DisplayedDataList.Count / constraintCount); i++)
            {
                estimatedSize += InsertSpace(i);
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
            var currentSpace = (layoutRp.Value == Layout.Horizontal ? Space.x : Space.y);
            for (int i = 0; i < DisplayedDataList.Count; i++)
            {
                estimatedSize += InsertSpace(i);
                float listCoord = (float)i;
                float min = ConvertPosToScrollPos(Mathf.Max(0, estimatedSize - baffer - DisplaySize));
                float max = ConvertPosToScrollPos(estimatedSize + GetSize(DisplayedDataList[i]) + baffer);
                itemCoordList.Add(new ItemCoord(listCoord, min, max));
                if ((i % constraintCount) == constraintCount - 1)
                    estimatedSize += GetSize(DisplayedDataList[i]) + currentSpace;
            }
        }

        protected virtual float CalcEstimatedInsertedSpace(int end, int start = 0)
        {
            float ret = 0f;
            for (int i = start; i <= end; i++)
            {
                ret += InsertSpace(i);
            }
            return ret;
        }

        protected virtual float InsertSpace(int i)
        {
            return 0f;
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
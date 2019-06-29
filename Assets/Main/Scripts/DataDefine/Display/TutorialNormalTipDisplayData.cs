using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class TutorialNormalTipDisplayData
    {
        public TutorialTipTextCategory TextCategory;

        public TutorialTipArrowDir ArrowDir;

        public float ArrowOffset;

        public string TextKey;

        /// <summary>
        /// Widget path relative to the root node of the form. If null or empty, arrow won't display and text category will be considered as <see cref="TutorialTipTextCategory.FixedPosition"/>.
        /// </summary>
        public string WidgetPath;

        public Vector2 TextOffsetRatio;

        public Vector2 TextOffsetPixels;

        public UIWidget.Pivot TextBgPivot = UIWidget.Pivot.TopLeft;
    }
}

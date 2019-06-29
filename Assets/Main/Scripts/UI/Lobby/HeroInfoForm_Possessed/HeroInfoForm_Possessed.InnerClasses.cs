using System;
using UnityEngine;
using GameFramework;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Possessed
    {
        public enum TabType
        {
            StarLevel = 0,
            Strengthen = 1,
            NewGear = 2,
            Skill = 3,
        }

        [Serializable]
        private class AttributeItem
        {
            [SerializeField]
            public UILabel AttributeNow = null;

            [SerializeField]
            public UILabel AttributeNext = null;

            [SerializeField]
            public UILabel AttributeName = null;
        }

        [Serializable]
        private class NewGearQualityMaterialItem
        {
            [SerializeField]
            public GeneralItemView ItemView = null;

            [SerializeField]
            public UILabel CountLabel = null;

            public void SetVisib(bool visible)
            {
                ItemView.gameObject.SetActive(visible);
                CountLabel.gameObject.SetActive(visible);
            }
        }
    }
}

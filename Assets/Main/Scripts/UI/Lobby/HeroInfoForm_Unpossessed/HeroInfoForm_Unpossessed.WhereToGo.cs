using UnityEngine;
using System;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Unpossessed
    {
        [Serializable]
        private class HeroWhereToGo
        {
            [SerializeField]
            public UILabel DetailDescription = null;

            [SerializeField]
            public ScrollViewCache WhereToGoItems = null;
        }

        [SerializeField]
        private HeroWhereToGo m_HeroWhereToGo = null;

        private void RefreshHeroWhereToGo()
        {
            string detailDescription = GameEntry.Localization.GetString(HeroData.DetailDescription);
            m_HeroWhereToGo.DetailDescription.text = GameEntry.StringReplacement.GetString(detailDescription);

            var WhereToGetIds = HeroData.WhereToGetIds;
            for (int i = 0; i < WhereToGetIds.Length; i++)
            {
                var whereToGetLogic = GameEntry.WhereToGet.GetLogic(WhereToGetIds[i]);
                if (whereToGetLogic == null)
                {
                    continue;
                }
                var item = m_HeroWhereToGo.WhereToGoItems.GetOrCreateItem(i);
                item.RefreshData(whereToGetLogic);
            }

            m_HeroWhereToGo.WhereToGoItems.RecycleItemsAtAndAfter(WhereToGetIds.Length);
            m_HeroWhereToGo.WhereToGoItems.ResetPosition();
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<WhereToGetDisplayItem>
        {

        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class SweepResultItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_ItemTitleLabel = null;

        [SerializeField]
        private List<GeneralItemView> m_ShowItems = null;

        [SerializeField]
        private UILabel m_ObtainedCoinLabel = null;

        [SerializeField]
        private UILabel m_ObtainedExperienceLabel = null;

        [SerializeField]
        private GameObject m_TitleObject = null;

        public void SetItemData(List<PBItemInfo> items, int sweepTime, int obtainedCoinCount, int obtainedExperienceCount, bool isAutoSweep)
        {
            if (!isAutoSweep && sweepTime == 1)
                m_TitleObject.SetActive(false);
            else
                m_TitleObject.SetActive(true);

            m_ItemTitleLabel.text = GameEntry.Localization.GetString("UI_TITLE_NAME_CLEANOUTTIMES", sweepTime);

            m_ObtainedCoinLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", Mathf.Max(0, obtainedCoinCount));
            m_ObtainedExperienceLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", Mathf.Max(0, obtainedExperienceCount));

            for (int i = 0; i < m_ShowItems.Count; i++)
            {
                if(i < items.Count)
                {
                    m_ShowItems[i].InitGeneralItem(items[i].Type, items[i].Count);
                    m_ShowItems[i].gameObject.SetActive(true);
                }
                else
                {
                    m_ShowItems[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
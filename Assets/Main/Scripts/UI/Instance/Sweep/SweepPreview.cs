using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class SweepPreview : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_TitleLabel = null;

        [SerializeField]
        private List<GeneralItemView> m_ShowItems = null;

        [SerializeField]
        private GameObject m_WannaGetRoot = null;

        [SerializeField]
        private GeneralItemView m_WannaGetItem = null;

        [SerializeField]
        private UILabel m_WannaGetItemInfoLabel = null;

        [SerializeField]
        private UIToggle m_IsAutoSweepToggle = null;

        [SerializeField]
        private UILabel m_AutoSweepTimeLabel = null;

        public bool IsAutoSweep
        {
            get { return m_IsAutoSweepToggle.value; }
        }

        public void Initialize()
        {
            gameObject.SetActive(false);
            m_IsAutoSweepToggle.onChange.Clear();
            m_IsAutoSweepToggle.onChange.Add(new EventDelegate( ()=>
            {
                GameEntry.Setting.SetBool(Constant.Setting.AutoCleanOut,m_IsAutoSweepToggle.value);
            }));
        }

        public void Show(SweepDisplayData displayData)
        {
            m_IsAutoSweepToggle.Set(GameEntry.Setting.GetBool(Constant.Setting.AutoCleanOut, true), false);

            m_TitleLabel.text = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_MAY_FALL");
            m_AutoSweepTimeLabel.text = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_SETTLEMENT_NUMBER_OF_TIMES", displayData.MaxSweepCount);

            if (displayData.SweepEntranceType == SweepDisplayData.ShowType.InstanceAuto)
            {
                m_WannaGetRoot.SetActive(false);
            }
            else if(displayData.SweepEntranceType == SweepDisplayData.ShowType.WhereToGet)
            {
                m_WannaGetItem.InitGeneralItem(displayData.WannaGetItemId);

                var item = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>().GetDataRow(displayData.WannaGetItemId);
                string name = ColorUtility.AddColorToString(ColorUtility.GetColorForQuality(item.Quality), GameEntry.Localization.GetString(item.Name));
                string count = ColorUtility.AddColorToString(Constant.Quality.Red, Mathf.Max(0, displayData.WannaGetItemCount).ToString());
                m_WannaGetItemInfoLabel.text = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_UNDONE", name, count);

                m_WannaGetRoot.SetActive(true);
            }

            var levelConfig = GameEntry.Data.InstanceGroups.GetLevelById(displayData.LevelId).LevelConfig;
            var drops = levelConfig.PossibleDrops;

            for (int i = 0; i < m_ShowItems.Count; i++)
            {
                if (i < drops.Length)
                {
                    m_ShowItems[i].gameObject.SetActive(true);
                    m_ShowItems[i].InitGeneralItem(drops[i].ItemId);
                }
                else
                {
                    m_ShowItems[i].gameObject.SetActive(false);
                }
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
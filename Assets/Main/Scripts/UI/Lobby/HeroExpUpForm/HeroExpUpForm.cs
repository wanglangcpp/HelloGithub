using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroExpUpForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_ProgressLabel = null;

        [SerializeField]
        private UIProgressBar m_Progress = null;

        [SerializeField]
        private HeroExpUpItem[] m_ExpItems = null;

        private LobbyHeroData m_HeroData = null;

        private int m_OldHeroLevel = 0;

        private int m_OldHeroExp = 0;

        private UILevelUpProgressController m_ProgressController = null;

        private const string m_EffectKey = "EffectLevelNumberUp";

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (!ParseUserData(userData))
            {
                return;
            }
            GameEntry.Event.Subscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
            GameEntry.Event.Subscribe(EventId.ItemDataChanged, OnItemDataChanged);
            m_ProgressController = m_Progress.GetComponent<UILevelUpProgressController>();
            m_ProgressController.OnLevelUp += ExpLevelUpReturn;
            RefreshItemDisplays();
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) base.OnClose(userData);

            GameEntry.Event.Unsubscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
            GameEntry.Event.Unsubscribe(EventId.ItemDataChanged, OnItemDataChanged);
            m_ProgressController.OnLevelUp -= ExpLevelUpReturn;
            base.OnClose(userData);
        }

        private void OnItemDataChanged(object sender, GameEventArgs e)
        {
        }

        private void OnLobbyHeroDataChanged(object sender, GameEventArgs e)
        {
            RefreshItemDisplays();
        }

        private bool ParseUserData(object userData)
        {
            var data = userData as HeroExpUpDisplayData;
            if (data == null)
            {
                Log.Error("User data is invalid.");
                return false;
            }

            m_HeroData = data.ExpHeroData;
            m_OldHeroLevel = 0;
            m_OldHeroExp = 0;
            return true;
        }

        private void RefreshItemDisplays()
        {
            var dt = GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows();
            var expItems = new List<DRItem>();

            if (m_OldHeroLevel > 0)
            {
                var baseDataTable = GameEntry.DataTable.GetDataTable<DRHeroBase>();
                var expList = new List<KeyValuePair<int, int>>();
                for (int level = m_OldHeroLevel; level <= m_HeroData.Level; ++level)
                {
                    DRHeroBase dataRow = baseDataTable.GetDataRow(level);
                    if (dataRow == null)
                    {
                        break;
                    }
                    expList.Add(new KeyValuePair<int, int>(dataRow.Id, dataRow.LevelUpExp));
                }
                m_ProgressController.Init(
                            m_OldHeroLevel, m_OldHeroExp,
                            m_HeroData.Level, m_HeroData.Exp,
                            expList, "UI_TEXT_USERLEVELNUMBER");

                m_ProgressController.Play();
            }
            else
            {
                m_LevelLabel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", m_HeroData.Level);
                m_ProgressLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", m_HeroData.Exp, m_HeroData.LevelUpExp);
                m_Progress.value = m_HeroData.ExpProgress;
            }

            for (int i = 0; i < dt.Length; ++i)
            {
                var item = dt[i];

                if (item.Type != (int)ItemType.HeroExpItem)
                {
                    continue;
                }

                expItems.Add(item);
            }
            expItems.Sort(CompareItems);

            for (int i = 0; i < m_ExpItems.Length; i++)
            {
                if (i < expItems.Count)
                {
                    m_ExpItems[i].gameObject.SetActive(true);
                    m_ExpItems[i].RefreshData(expItems[i], m_HeroData.Type, UseItem);
                }
                else
                {
                    m_ExpItems[i].gameObject.SetActive(false);
                }
            }

            m_OldHeroLevel = m_HeroData.Level;
            m_OldHeroExp = m_HeroData.Exp;
        }

        private int CompareItems(DRItem a, DRItem b)
        {
            if (a.Quality == b.Quality)
            {
                return a.Id.CompareTo(b.Id);
            }
            return a.Quality.CompareTo(b.Quality);
        }

        private void UseItem(GameObject obj)
        {

        }

        private void ExpLevelUpReturn(int level)
        {
            m_EffectsController.ShowEffect(m_EffectKey);
        }

        public void OnClickExpUpAll()
        {

        }

        private int CompareItems(ItemData a, ItemData b)
        {
            var dt = GameEntry.DataTable.GetDataTable<DRItem>();
            var drA = dt[a.Key];
            var drB = dt[b.Key];
            if (drA.Quality == drB.Quality)
            {
                return drA.Id.CompareTo(drB.Id);
            }
            return drA.Quality.CompareTo(drB.Quality);
        }
    }
}

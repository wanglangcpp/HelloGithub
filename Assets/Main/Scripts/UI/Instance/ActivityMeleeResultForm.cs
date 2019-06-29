using UnityEngine;
using System.Collections;
using Genesis.GameClient;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ActivityMeleeResultForm : NGUIForm
    {
        [SerializeField]
        private MeleeItem[] m_MeleeItems = null;

        [SerializeField]
        private UILabel m_MeRank = null;

        private MimicMeleeInstanceLogic m_MeleeInstanceLogic = null;

        private List<KeyValuePair<int, int>> m_CampRanks = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_MeleeInstanceLogic = GameEntry.SceneLogic.BaseInstanceLogic as MimicMeleeInstanceLogic;
            if (m_MeleeInstanceLogic == null)
            {
                return;
            }
            RefreshMeleeRank();
        }

        private void RefreshMeleeRank()
        {
            m_CampRanks = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < MimicMeleeInstanceLogic.MimicPlayerCamps.Length; i++)
            {
                int campType = (int)MimicMeleeInstanceLogic.MimicPlayerCamps[i];
                int campRank = m_MeleeInstanceLogic.GetScoreForCamp(MimicMeleeInstanceLogic.MimicPlayerCamps[i]);
                m_CampRanks.Add(new KeyValuePair<int, int>(campType, campRank));
            }

            m_CampRanks.Sort(CompareRank);
            for (int i = 0; i < m_CampRanks.Count; i++)
            {
                string campName = GameEntry.Localization.GetString(MimicMeleeInstanceLogic.CampToName[(CampType)m_CampRanks[i].Key]);
                string campRank = GameEntry.Localization.GetString("UI_TEXT_TOTAL_POINTS", m_CampRanks[i].Value);
                m_MeleeItems[i].RefreshData(campName, campRank, (CampType)m_CampRanks[i].Key);
            }
            m_MeRank.text = GameEntry.Localization.GetString("UI_TEXT_MY_SCORES", m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeScore);
        }

        private int CompareRank(KeyValuePair<int, int> x, KeyValuePair<int, int> y)
        {
            if (x.Value == y.Value)
            {
                return x.Key.CompareTo(y.Key);
            }

            return y.Value.CompareTo(x.Value);
        }

        public void OnClickScreen()
        {
            CloseSelf();
            GameEntry.SceneLogic.GoBackToLobby();
        }

        [Serializable]
        private class MeleeItem
        {
            public UILabel CampName = null;
            public UILabel CampRank = null;
            public GameObject OtherCampBg = null;
            public GameObject MeCampBg = null;

            public void RefreshData(string campName, string campRank, CampType campType)
            {
                CampName.text = campName;
                CampRank.text = campRank;
                MeCampBg.SetActive(campType == CampType.Player);
                OtherCampBg.SetActive(campType != CampType.Player);
            }
        }
    }
}
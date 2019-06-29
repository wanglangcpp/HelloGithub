using UnityEngine;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public partial class BattleForm
    {
        [SerializeField]
        private UILabel m_MeleeLevel = null;

        [SerializeField]
        private GameObject m_MeleeExpProgressObj = null;

        [SerializeField]
        private GameObject m_MeleeRankListObj = null;

        [SerializeField]
        private UIProgressBar m_ExpProgressBar = null;

        [SerializeField]
        private UILevelUpProgressController m_LevelUpProgress = null;

        [SerializeField]
        private UILabel m_ExpProgressBarText = null;

        [SerializeField]
        private MeleeRank[] m_MeleeRanks = null;

        [SerializeField]
        private MeleeRank m_MySelfRank = null;

        [SerializeField]
        private GameObject m_SystemMessagePrefab = null;

        [SerializeField]
        private float m_SystemMessageY = 200;

        [SerializeField]
        private GameObject m_MiniMapTemplate = null;

        [SerializeField]
        private GameObject m_MiniMapRoot = null;

        [SerializeField]
        private GameObject m_MeleeResurrectionPrefab = null;

        [SerializeField]
        private GameObject m_HeroInBattleRoot = null;

        private MimicMeleeInstanceLogic m_MeleeInstanceLogic = null;

        private int m_OldMeleeLevel = 0;

        private int m_OldMeleeExp = 0;

        private int m_UISpriteDepth = 10;

        private IDataTable<DRMimicMeleeBase> m_BaseDataTable = null;

        private List<KeyValuePair<int, int>> m_LastExpList = null;

        private List<KeyValuePair<int, int>> m_CampRanks = null;

        private Stack<GameObject> m_SystemMessageGOPool = new Stack<GameObject>(8);
        private LinkedList<GameObject> m_SystemMessageGOInUse = new LinkedList<GameObject>();
        private GameObject m_MiniMapGO = null;
        private GameObject m_MeleeResurrectionObj = null;

        private void InitMelee()
        {
            m_MeleeInstanceLogic = GameEntry.SceneLogic.BaseInstanceLogic as MimicMeleeInstanceLogic;
            m_HeroInBattle.PortraitMeleeObj.SetActive(m_MeleeInstanceLogic != null);
            m_MeleeExpProgressObj.SetActive(m_MeleeInstanceLogic != null);
            m_MeleeRankListObj.SetActive(m_MeleeInstanceLogic != null);
            m_MeleeRankListObj.SetActive(m_MeleeInstanceLogic != null);
            if (m_MeleeInstanceLogic == null)
                return;
            m_HeroInBattleRoot.SetActive(m_MeleeInstanceLogic == null);
            m_DropCoinsRoot.SetActive(m_MeleeInstanceLogic == null);
            m_HeroInBattle.PortraitObj.SetActive(m_MeleeInstanceLogic == null);
            m_HeroInBattle.PortraitInObj.SetActive(m_MeleeInstanceLogic == null);
            m_HeroInBattle.PortraitOutObj.SetActive(m_MeleeInstanceLogic == null);
            //m_HeroInBattle.PortraitMeleeObj.SetActive(m_MeleeInstanceLogic != null);

            if (m_MeleeInstanceLogic == null)
            {
                return;
            }

            m_MiniMapGO = NGUITools.AddChild(m_MiniMapRoot, m_MiniMapTemplate);

            if (m_MeleeInstanceLogic.MeHeroCharacter == null)
            {
                return;
            }

            int meleeLevel = m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeLevel;
            m_MeleeLevel.text = meleeLevel.ToString();
            m_ExpProgressBarText.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERINFO_LEVEL", meleeLevel);
            m_OldMeleeLevel = meleeLevel;
            m_OldMeleeExp = m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeExpAtCurrentLevel;
            m_ExpProgressBar.value = 0;
            m_BaseDataTable = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>();
            RefreshMeleeRank();
            m_MySelfRank.RefreshRank(GameEntry.Localization.GetString("UI_TEXT_MELEE_MY_INTEGRAL"), m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeScore);
        }

        private void UpdateMeleeInfo()
        {
            if (m_MeleeInstanceLogic == null)
            {
                return;
            }

            int newMeleeLevel = m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeLevel;
            int newMeleeExp = m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeExpAtCurrentLevel;

            if (newMeleeExp != m_OldMeleeExp || newMeleeLevel != m_OldMeleeLevel)
            {
                RefreshMeleeExpProgress(newMeleeLevel, newMeleeExp);
                m_MeleeLevel.text = m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeLevel.ToString();
            }
        }

        private void UpdateSystemMessages()
        {
            if (m_MeleeInstanceLogic == null)
            {
                return;
            }

            for (var cur = m_SystemMessageGOInUse.First; cur != null; /* 空的增量语句 */)
            {
                if (!cur.Value.activeSelf)
                {
                    var next = cur.Next;
                    m_SystemMessageGOPool.Push(cur.Value);
                    m_SystemMessageGOInUse.Remove(cur);
                    cur = next;
                }
                else
                {
                    cur = cur.Next;
                }
            }
        }

        private void RefreshMeleeExpProgress(int newMeleeLevel, int newMeleeExp)
        {
            var expList = new List<KeyValuePair<int, int>>();
            for (int level = m_OldMeleeLevel; level <= newMeleeLevel; ++level)
            {
                DRMimicMeleeBase dataRow = m_BaseDataTable.GetDataRow(level);
                if (dataRow == null)
                {
                    break;
                }
                expList.Add(new KeyValuePair<int, int>(dataRow.Id, dataRow.LevelUpExp));
            }
            if (!m_LevelUpProgress.IsPlaying)
            {
                m_LevelUpProgress.Init(m_OldMeleeLevel, m_OldMeleeExp, newMeleeLevel, newMeleeExp, expList, "UI_TEXT_PLAYERINFO_LEVEL");
                m_LevelUpProgress.Play();
                m_OldMeleeLevel = newMeleeLevel;
                m_OldMeleeExp = newMeleeExp;
                m_LastExpList = expList;
            }
            else
            {
                m_OldMeleeLevel = newMeleeLevel;
                m_OldMeleeExp = newMeleeExp;
                for (int i = 0; m_LastExpList != null && i < m_LastExpList.Count; i++)
                {
                    expList.Remove(m_LastExpList[i]);
                }
                m_LevelUpProgress.RefreshProgress(m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeLevel, m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeExpAtCurrentLevel, expList);
            }
        }

        private void OnMimicMeleeChanged(object sender, GameEventArgs e)
        {
            RefreshMeleeRank();
            m_MySelfRank.RefreshRank(GameEntry.Localization.GetString("UI_TEXT_MELEE_MY_INTEGRAL"), m_MeleeInstanceLogic.MeHeroCharacter.Data.MeleeScore);
            var sysTemMessage = e as OnMimicMeleeChangedEventArgs;
            if (!string.IsNullOrEmpty(sysTemMessage.KillerName) && !string.IsNullOrEmpty(sysTemMessage.VictimName))
            {
                string autoScrollLabelText = GameEntry.Localization.GetString("UI_TEXT_MELEE_KILL_PLAYER", sysTemMessage.KillerName, sysTemMessage.VictimName);
                CreateSystemMessage(autoScrollLabelText);
                if (sysTemMessage.VictimIsMe)
                {
                    CreateMeleeResurrection(sysTemMessage.KillerName, m_MeleeInstanceLogic.ReviveWaitTime);
                }
            }
        }

        private void CreateSystemMessage(string content)
        {
            GameObject systemMessageObj;
            if (m_SystemMessageGOPool.Count <= 0)
            {
                systemMessageObj = NGUITools.AddChild(m_NormalRoot, m_SystemMessagePrefab);
            }
            else
            {
                systemMessageObj = m_SystemMessageGOPool.Pop();
            }

            systemMessageObj.transform.localPosition = new Vector3(0, m_SystemMessageY, 0);
            var systemMessage = systemMessageObj.GetComponent<SystemMessage>();
            systemMessage.Refresh(content);
            m_UISpriteDepth++;
            systemMessage.SetDepth(m_UISpriteDepth);
            systemMessageObj.SetActive(true);
            m_SystemMessageGOInUse.AddLast(systemMessageObj);
        }

        private void CreateMeleeResurrection(string killerName, float reviveWaitTime)
        {
            if (m_MeleeResurrectionObj != null)
            {
                Destroy(m_MeleeResurrectionObj);
            }
            m_MeleeResurrectionObj = NGUITools.AddChild(m_NormalRoot, m_MeleeResurrectionPrefab);
            var meleeResurrection = m_MeleeResurrectionObj.GetComponent<MeleeResurrection>();
            meleeResurrection.ShowUI(killerName, reviveWaitTime);
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
                m_MeleeRanks[i].RefreshRank(campName, m_CampRanks[i].Value);
            }
        }

        private int CompareRank(KeyValuePair<int, int> x, KeyValuePair<int, int> y)
        {
            if (x.Value == y.Value)
            {
                return x.Key.CompareTo(y.Key);
            }

            return y.Value.CompareTo(x.Value);
        }

        private void ClearSystemMessages()
        {
            for (var cur = m_SystemMessageGOInUse.First; cur != null; cur = cur.Next)
            {
                Destroy(cur.Value);
            }
            m_SystemMessageGOInUse.Clear();

            while (m_SystemMessageGOPool.Count > 0)
            {
                var go = m_SystemMessageGOPool.Pop();
                Destroy(go);
            }
        }
    }
}
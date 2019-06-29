using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CleanOutHeroStateChange : MonoBehaviour
    {
        private enum SubState
        {
            Showing,
            Player,
            Heroes,
            End,
        }

        [SerializeField]
        private Animation m_Animation = null;

        [SerializeField]
        private HeroItem[] m_HeroItem = null;

        [SerializeField]
        private HeroItem m_PlayerItem = null;

        private const string InwardAnimName = "InstanceResultSubPanelInward";

        private const string LevelUpMarkClipName = "InstanceResultLevelUpMark";

        private SubState m_SubState;

        private bool m_HasPlayPlayerLevelUp = false;

        /// <summary>
        /// 扫荡界面先不需要hero显示
        /// </summary>
        public void InitHeroStateChange()
        {
            gameObject.SetActive(true);
            InitPlayer();
            // InitHeros();
            m_Animation.Rewind(InwardAnimName);
            m_Animation.Play(InwardAnimName);
            m_SubState = SubState.Showing;
        }

        public void CloseHeroStateChange()
        {
            gameObject.SetActive(false);
        }

        private void InitPlayer()
        {
            var playerData = GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer;
            m_PlayerItem.m_Name.text = playerData.Name;

            var expList = new List<KeyValuePair<int, int>>();
            var dataTable = GameEntry.DataTable.GetDataTable<DRPlayer>();
            for (int level = playerData.OldLevel; level <= playerData.NewLevel; ++level)
            {
                DRPlayer dataRow = dataTable.GetDataRow(level);
                if (dataRow == null)
                {
                    playerData.NewLevel = level - 1;
                    break;
                }
                expList.Add(new KeyValuePair<int, int>(dataRow.Id, dataRow.LevelUpExp));
            }

            m_PlayerItem.m_LevelUpController.Init(
                playerData.OldLevel, playerData.OldExp,
                playerData.NewLevel, playerData.NewExp,
                expList);
        }

        private void InitHeros()
        {
            var heroDatas = GameEntry.Data.CleanOuts.HeroResultData.Heroes;
            var baseDataTable = GameEntry.DataTable.GetDataTable<DRHeroBase>();

            for (int i = 0; i < m_HeroItem.Length; ++i)
            {
                var heroDisplay = m_HeroItem[i];
                if (i < heroDatas.Count)
                {
                    heroDisplay.m_Root.SetActive(true);
                    var heroData = heroDatas[i];
                    var expList = new List<KeyValuePair<int, int>>();
                    for (int level = heroData.OldLevel; level <= heroData.NewLevel; ++level)
                    {
                        DRHeroBase dataRow = baseDataTable.GetDataRow(level);
                        if (dataRow == null)
                        {
                            heroData.NewLevel = level - 1;
                            break;
                        }
                        expList.Add(new KeyValuePair<int, int>(dataRow.Id, dataRow.LevelUpExp));
                    }
                    heroDisplay.m_Icon.spriteName = heroData.PortraitSpriteName;
                    heroDisplay.m_Name.text = heroData.Name;

                    heroDisplay.m_LevelUpController.Init(
                            heroData.OldLevel, heroData.OldExp,
                            heroData.NewLevel, heroData.NewExp,
                            expList);
                    heroDisplay.m_LevelUp.SetActive(false);
                }
                else
                {
                    heroDisplay.m_Root.SetActive(false);
                }
            }

        }

        private void Update()
        {
            switch (m_SubState)
            {
                case SubState.Showing:
                    UpdateShowingState();
                    break;
                case SubState.Player:
                    UpdatePlayerState();
                    break;
                case SubState.Heroes:
                    UpdateHeroesState();
                    break;
                case SubState.End:
                    UpdateEndState();
                    break;
                default:
                    break;
            }
        }

        private void UpdateShowingState()
        {
            if (m_Animation[InwardAnimName].normalizedTime < .99f)
            {
                return;
            }
            m_SubState = SubState.Player;
            m_PlayerItem.m_LevelUpController.Play();
        }

        private void UpdatePlayerState()
        {
            if (m_PlayerItem.m_LevelUpController.IsPlaying)
            {
                return;
            }

            if (!m_HasPlayPlayerLevelUp)
            {
                var playerData = GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer;
                bool HasNeedPlayPlayerLevelUp = (playerData.NewLevel > playerData.OldLevel);
                if (HasNeedPlayPlayerLevelUp)
                {
                    m_HasPlayPlayerLevelUp = true;
                    GameEntry.UI.OpenUIForm(UIFormId.PlayerLevelUpForm, new PlayerLevelUpDisplayData { PlayerLevelUpReturn = PlayerLevelUpReturn });
                }
                else
                {
                    PlayerLevelUpReturn();
                }
            }
            else
            {
                PlayerLevelUpReturn();
            }
        }

        private void PlayerLevelUpReturn()
        {
            m_HasPlayPlayerLevelUp = false;
            m_SubState = SubState.Heroes;
            for (int i = 0; i < m_HeroItem.Length; ++i)
            {
                var heroDisplay = m_HeroItem[i];
                if (!heroDisplay.m_Root.activeSelf)
                {
                    continue;
                }

                heroDisplay.m_LevelUpController.Play();
                var heroData = GameEntry.Data.CleanOuts.HeroResultData.Heroes[i];
                if (heroData.HasLevelUp)
                {
                    heroDisplay.m_LevelUp.gameObject.SetActive(true);
                    heroDisplay.m_LevelUpAnimation.Play(LevelUpMarkClipName);
                }
            }
        }

        private void UpdateHeroesState()
        {
            m_SubState = SubState.End;
            return;
            //             for (int i = 0; i < m_HeroItem.Length; ++i)
            //             {
            //                 var heroDisplay = m_HeroItem[i];
            //                 if (heroDisplay.m_Root.gameObject.activeSelf && heroDisplay.m_LevelUpController.IsPlaying)
            //                 {
            //                     return;
            //                 }
            //             }
            //
            //             m_SubState = SubState.End;
        }

        private void UpdateEndState()
        {
            return;
        }
    }
}

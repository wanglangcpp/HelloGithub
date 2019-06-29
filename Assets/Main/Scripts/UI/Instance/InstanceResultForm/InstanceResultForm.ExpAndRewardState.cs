using GameFramework.DataTable;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class ExpAndRewardState : StateBase
        {
            private const string LevelUpMarkClipName = "InstanceResultLevelUpMark";

            private enum SubState
            {
                Showing,
                Player,
                Heroes,
                Reward,
                End,
            }

            private SubState m_SubState;

            private float m_EffectiveTime;

            private bool m_HasPlayPlayerLevelUp = false;

            private bool m_HasCalledUpdatePlayerState = false;

            private bool m_StopAnim = false;

            private bool m_HasGoBackToLobby = false;

            private int m_NextInstanceId = 0;

            private float m_ChangeRewardTime = 0.8f;

            private IFsm<InstanceResultForm> m_Fsm = null;

            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                m_StopAnim = false;

                if (m_HasGoBackToLobby)
                {
                    return;
                }

                m_HasGoBackToLobby = true;
                GameEntry.SceneLogic.GoBackToLobby();
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {
                if (m_StopAnim)
                {
                    return;
                }

                switch (m_SubState)
                {
                    case SubState.Player:
                        m_StopAnim = false;
                        break;
                    case SubState.Heroes:
                        m_StopAnim = true;
                        for (int i = 0; i < fsm.Owner.m_Heroes.Length; i++)
                        {
                            var heroDisplay = fsm.Owner.m_Heroes[i];
                            heroDisplay.LevelUpProgress.Stop();
                        }
                        break;
                    default:
                        break;
                }
            }

            public ExpAndRewardState(Type nextStateType, Transform currentSubPanel, Transform lastSubPanel)
                : base(nextStateType, currentSubPanel, lastSubPanel)
            {

            }

            protected override void OnInit(IFsm<InstanceResultForm> fsm)
            {
                base.OnInit(fsm);
                InitPlayer(fsm);
                InitHeroes(fsm);
                InitReward(fsm);
                m_Fsm = fsm;
            }

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                base.OnEnter(fsm);
                m_EffectiveTime = 0f;
                m_SubState = SubState.Showing;
            }

            protected override void OnUpdate(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                switch (m_SubState)
                {
                    case SubState.Showing:
                        UpdateShowingState(fsm, elapseSeconds, realElapseSeconds);
                        break;
                    case SubState.Player:
                        UpdatePlayerState(fsm, elapseSeconds, realElapseSeconds);
                        break;
                    case SubState.Heroes:
                        UpdateHeroesState(fsm, elapseSeconds, realElapseSeconds);
                        break;
                    case SubState.Reward:
                        UpdateRewardState(fsm, elapseSeconds, realElapseSeconds);
                        break;
                    case SubState.End:
                        UpdateEndState(fsm, elapseSeconds, realElapseSeconds);
                        break;
                    default:
                        break;
                }
            }

            private void InitPlayer(IFsm<InstanceResultForm> fsm)
            {
                var playerData = fsm.Owner.m_Data.ItsPlayer;
                int pulsExp = UIUtility.CalculateDeltaExp(playerData.OldLevel, playerData.OldExp, playerData.NewLevel, playerData.NewExp);
                fsm.Owner.m_PlayerExpPlus.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", pulsExp);
            }

            private void InitHeroes(IFsm<InstanceResultForm> fsm)
            {
                var heroDatas = fsm.Owner.m_Data.Heroes;
                var playerData = fsm.Owner.m_Data.ItsPlayer;
                var baseDataTable = GameEntry.DataTable.GetDataTable<DRHeroBase>();

                for (int i = 0; i < fsm.Owner.m_Heroes.Length; ++i)
                {
                    var heroDisplay = fsm.Owner.m_Heroes[i];
                    if (i < heroDatas.Count)
                    {
                        heroDisplay.Root.gameObject.SetActive(true);

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

                        heroDisplay.Portrait.spriteName = heroData.PortraitSpriteName;
                        heroDisplay.Profession.spriteName = UIUtility.GetElementSpriteName(heroData.ElementId);

                        heroDisplay.ExpPlus.gameObject.SetActive(true);

                        if (heroData.NewLevel == playerData.NewLevel)
                        {
                            heroDisplay.ExpPlus.text = GameEntry.Localization.GetString("UI_TEXT_FULL_EXP");
                            fsm.Owner.m_HeroLevelMax.gameObject.SetActive(true);
                        }
                        else
                        {
                            int exp = 0;

                            for (int j = 0; j < expList.Count - 1; j++)
                            {
                                exp += expList[j].Value;
                            }

                            int pulsExp = 0;

                            if (heroData.NewLevel > heroData.OldLevel)
                            {
                                pulsExp = exp + heroData.NewExp - heroData.OldExp;
                            }
                            else
                            {
                                pulsExp = heroData.NewExp - heroData.OldExp;
                            }
                            heroDisplay.ExpPlus.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", pulsExp);
                            fsm.Owner.m_HeroLevelMax.gameObject.SetActive(false);
                        }

                        heroDisplay.LevelUpProgress.Init(
                            heroData.OldLevel, heroData.OldExp,
                            heroData.NewLevel, heroData.NewExp,
                            expList, "UI_TEXT_USERLEVELNUMBER");

                        heroDisplay.LevelUpMark.gameObject.SetActive(false);
                    }
                    else
                    {
                        heroDisplay.Root.gameObject.SetActive(false);
                    }
                }
            }


            private void InitReward(IFsm<InstanceResultForm> fsm)
            {
                m_HasGoBackToLobby = false;
                m_HasGoBackToLobby = false;
                m_NextInstanceId = 0;

                var playerData = fsm.Owner.m_Data.ItsPlayer;

                if (playerData.CoinEarned <= 0)
                {
                    fsm.Owner.m_CoinEarned.text = string.Empty;
                    fsm.Owner.m_CoinParent.gameObject.SetActive(false);
                }
                else
                {
                    fsm.Owner.m_CoinParent.gameObject.SetActive(true);
                    fsm.Owner.m_CoinEarned.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", playerData.CoinEarned);
                }

                for (int i = 0; i < fsm.Owner.m_ItemEarned.Length; i++)
                {
                    var itemEarned = fsm.Owner.m_ItemEarned[i];
                    if (i >= GameEntry.Data.InstanceGoods.InstanceItemData.Count)
                    {
                        itemEarned.gameObject.SetActive(false);
                        continue;
                    }

                    itemEarned.gameObject.SetActive(true);
                    var itemData = GameEntry.Data.InstanceGoods.InstanceItemData[i];
                    itemEarned.InitGeneralItem(itemData.Type, itemData.Count);
                }
            }

            private void UpdateShowingState(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (CachedAnimation[InwardClipName].normalizedTime < .99f)
                {
                    return;
                }

                m_SubState = SubState.Player;
            }

            private void UpdatePlayerState(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (m_HasCalledUpdatePlayerState)
                {
                    return;
                }

                m_HasCalledUpdatePlayerState = true;

                if (!m_HasPlayPlayerLevelUp)
                {
                    var playerData = fsm.Owner.m_Data.ItsPlayer;
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
                m_HasCalledUpdatePlayerState = false;
                m_HasPlayPlayerLevelUp = false;
                m_SubState = SubState.Heroes;

                for (int i = 0; i < m_Fsm.Owner.m_Heroes.Length; ++i)
                {
                    var heroDisplay = m_Fsm.Owner.m_Heroes[i];
                    if (!heroDisplay.Root.gameObject.activeSelf)
                    {
                        continue;
                    }

                    heroDisplay.LevelUpProgress.Play();
                    var heroData = m_Fsm.Owner.m_Data.Heroes[i];

                    if (heroData.HasLevelUp)
                    {
                        heroDisplay.LevelUpMark.gameObject.SetActive(true);
                        heroDisplay.LevelUpMarkAnimation.Play(LevelUpMarkClipName);
                    }
                }
            }

            private void UpdateHeroesState(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                for (int i = 0; i < fsm.Owner.m_Heroes.Length; ++i)
                {
                    var heroDisplay = fsm.Owner.m_Heroes[i];
                    if (heroDisplay.Root.gameObject.activeSelf && heroDisplay.LevelUpProgress.IsPlaying)
                    {
                        return;
                    }
                    if (heroDisplay.LevelUpMarkAnimation.isPlaying)
                    {
                        return;
                    }
                }

                for (int i = 0; i < fsm.Owner.m_Heroes.Length; ++i)
                {
                    var heroDisplay = fsm.Owner.m_Heroes[i];
                    if (heroDisplay.Root.gameObject.activeSelf)
                    {
                        var heroData = m_Fsm.Owner.m_Data.Heroes[i];
                        if (heroData.HasLevelUp)
                        {
                            m_Fsm.Owner.m_EffectsController.ShowEffect(string.Format("EffectHeroLevelUp{0}", (i + 1).ToString()));
                        }
                    }
                }
                m_EffectiveTime += realElapseSeconds;
                if (m_EffectiveTime > m_ChangeRewardTime)
                {
                    m_SubState = SubState.Reward;
                }
            }

            private void UpdateRewardState(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (CachedAnimation[InwardClipName].normalizedTime >= .99f && !fsm.Owner.m_ReturnLobbyButton.gameObject.activeSelf)
                {
                    CachedAnimation.Stop();
                    fsm.Owner.m_ReturnLobbyButton.gameObject.SetActive(true);
                    fsm.Owner.m_ReturnChapterBtn.gameObject.SetActive(true);

                    if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.SinglePlayer)
                    {
                        int currentInstanceId = GameEntry.SceneLogic.SinglePlayerInstanceLogic.InstanceId;
                        IDataTable<DRInstance> DRInstance = GameEntry.DataTable.GetDataTable<DRInstance>();
                        DRInstance[] drs = DRInstance.GetAllDataRows();
                        for (int i = 0; i < drs.Length; i++)
                        {
                            if (drs[i].PrerequisiteInstanceId == currentInstanceId)
                            {
                                m_NextInstanceId = drs[i].Id;
                                break;
                            }
                        }
                        for (int i = 0; i < fsm.Owner.m_Heroes.Length; ++i)
                        {
                            var heroDisplay = fsm.Owner.m_Heroes[i];
                            if (heroDisplay.Root.gameObject.activeSelf)
                            {
                                var heroData = m_Fsm.Owner.m_Data.Heroes[i];
                                if (heroData.HasLevelUp)
                                {
                                    heroDisplay.LevelUpMark.gameObject.SetActive(false);
                                }
                                heroDisplay.ExpPlus.gameObject.SetActive(false);
                            }
                        }
                    }
                    m_SubState = SubState.End;
                }
            }

            private void UpdateEndState(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (m_HasPlayPlayerLevelUp)
                {
                    return;
                }

                m_EffectiveTime += realElapseSeconds;

                if (m_EffectiveTime > fsm.Owner.m_ExpSubPanelDuration)
                {
                    //GoToNextStep(fsm);
                }
            }

            public override void GotoNextInstance(IFsm<InstanceResultForm> m_Fsm)
            {
                base.GotoNextInstance(m_Fsm);

                if (m_NextInstanceId <= 0)
                {
                    return;
                }

                GameEntry.LobbyLogic.EnterInstance(m_NextInstanceId);
            }
        }
    }
}

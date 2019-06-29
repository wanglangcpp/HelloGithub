using System;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BattleForm
    {
        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.NpcDeadDropIcons, OnNpcDeadDropCoins);
            GameEntry.Event.Subscribe(EventId.BuildingDeadDropIcons, OnBuildingDeadDropCoins);
            GameEntry.Event.Subscribe(EventId.SwitchHeroStart, OnSwitchHeroStart);
            GameEntry.Event.Subscribe(EventId.SwitchHeroComplete, OnSwitchHeroComplete);
            GameEntry.Event.Subscribe(EventId.EnterAltSkill, OnEnterAltSkill);
            GameEntry.Event.Subscribe(EventId.LeaveAltSkill, OnLeaveAltSkill);
            GameEntry.Event.Subscribe(EventId.InstancePropagandaBegin, OnPropagandaBegin);
            GameEntry.Event.Subscribe(EventId.InstancePropagandaEnd, OnPropagandaEnd);
            GameEntry.Event.Subscribe(EventId.ShowBoss, OnShowBoss);
            GameEntry.Event.Subscribe(EventId.AutoFightStateChanged, OnAutoFightStateChanged);
            GameEntry.Event.Subscribe(EventId.InstanceParamChanged, OnInstanceParamChanged);
            GameEntry.Event.Subscribe(EventId.CharacterDead, OnCharacterDead);
            GameEntry.Event.Subscribe(EventId.BuildingDead, OnBuildingDead);
            GameEntry.Event.Subscribe(EventId.SkillContinualTapUpdateInput, OnSkillContinualTapUpdateInput);
            GameEntry.Event.Subscribe(EventId.MyHeroPerformSkillStart, OnMyHeroPerformSkillStart);
            GameEntry.Event.Subscribe(EventId.MyHeroPerformSkillEnd, OnMyHeroPerformSkillEnd);
            GameEntry.Event.Subscribe(EventId.InstanceStartTimeChanged, OnInstanceStartTimeChanged);
            GameEntry.Event.Subscribe(EventId.CameraAnimAboutToStart, OnCameraAnimAboutToStart);
            GameEntry.Event.Subscribe(EventId.CameraAnimAboutToStop, OnCameraAnimAboutToStop);
            GameEntry.Event.Subscribe(EventId.PortagingAnimation, OnCharacterPortaging);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkClosed, OnNetworkClosed);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkConnected, OnNetworkConnected);
            GameEntry.Event.Subscribe(EventId.PerformChargeSkill, OnPerformChargeSkill);
        }

        private void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.NpcDeadDropIcons, OnNpcDeadDropCoins);
            GameEntry.Event.Unsubscribe(EventId.BuildingDeadDropIcons, OnBuildingDeadDropCoins);
            GameEntry.Event.Unsubscribe(EventId.SwitchHeroStart, OnSwitchHeroStart);
            GameEntry.Event.Unsubscribe(EventId.SwitchHeroComplete, OnSwitchHeroComplete);
            GameEntry.Event.Unsubscribe(EventId.EnterAltSkill, OnEnterAltSkill);
            GameEntry.Event.Unsubscribe(EventId.LeaveAltSkill, OnLeaveAltSkill);
            GameEntry.Event.Unsubscribe(EventId.InstancePropagandaBegin, OnPropagandaBegin);
            GameEntry.Event.Unsubscribe(EventId.InstancePropagandaEnd, OnPropagandaEnd);
            GameEntry.Event.Unsubscribe(EventId.ShowBoss, OnShowBoss);
            GameEntry.Event.Unsubscribe(EventId.AutoFightStateChanged, OnAutoFightStateChanged);
            GameEntry.Event.Unsubscribe(EventId.InstanceParamChanged, OnInstanceParamChanged);
            GameEntry.Event.Unsubscribe(EventId.CharacterDead, OnCharacterDead);
            GameEntry.Event.Unsubscribe(EventId.BuildingDead, OnBuildingDead);
            GameEntry.Event.Unsubscribe(EventId.SkillContinualTapUpdateInput, OnSkillContinualTapUpdateInput);
            GameEntry.Event.Unsubscribe(EventId.MyHeroPerformSkillStart, OnMyHeroPerformSkillStart);
            GameEntry.Event.Unsubscribe(EventId.MyHeroPerformSkillEnd, OnMyHeroPerformSkillEnd);
            GameEntry.Event.Unsubscribe(EventId.InstanceStartTimeChanged, OnInstanceStartTimeChanged);
            GameEntry.Event.Unsubscribe(EventId.CameraAnimAboutToStart, OnCameraAnimAboutToStart);
            GameEntry.Event.Unsubscribe(EventId.CameraAnimAboutToStop, OnCameraAnimAboutToStop);
            GameEntry.Event.Unsubscribe(EventId.PortagingAnimation, OnCharacterPortaging);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkClosed, OnNetworkClosed);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkConnected, OnNetworkConnected);
            GameEntry.Event.Unsubscribe(EventId.PerformChargeSkill, OnPerformChargeSkill);
        }

        private void OnMyHeroPerformSkillEnd(object sender, GameEventArgs e)
        {
            var ne = e as MyHeroPerformSkillEndEventArgs;
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic == null || instanceLogic.MeHeroCharacter == null || instanceLogic.MyHeroesData.CurrentHeroData.Id != ne.EntityId)
            {
                return;
            }

            m_SkillContinualTapDisplayer.Hide();
        }

        private void OnMyHeroPerformSkillStart(object sender, GameEventArgs e)
        {
            var ne = e as MyHeroPerformSkillStartEventArgs;
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic == null || instanceLogic.MyHeroesData.CurrentHeroData.Id != ne.EntityId)
            {
                return;
            }

            if (instanceLogic.MyHeroesData.CurrentHeroData.AltSkill.Enabled)
            {
                return;
            }

            int index = ne.SkillIndex;
            int skillId = -1;
            bool isInCombo = false;
            bool isContinualTap = false;
            bool isCharge = false;

            var meHeroCharacter = GameEntry.Entity.GetGameEntity(ne.EntityId) as MeHeroCharacter;
            if (meHeroCharacter == null || index < 0)
            {
                return;
            }

            meHeroCharacter.CheckNormalSkillAvailability(index, out skillId, out isInCombo, out isContinualTap, out isCharge);

            if (skillId <= 0 || !isContinualTap)
            {
                return;
            }

            m_SkillContinualTapDisplayer.Show(index, skillId);
        }

        private void OnSkillContinualTapUpdateInput(object sender, GameEventArgs e)
        {
            var ne = e as SkillContinualTapUpdateInputEventArgs;

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic == null || instanceLogic.MeHeroCharacter == null || instanceLogic.MyHeroesData.CurrentHeroData.Id != ne.OwnerEntityId)
            {
                return;
            }

            if (!ne.Success)
            {
                m_SkillContinualTapDisplayer.SetFailure();
            }
            else
            {
                m_SkillContinualTapDisplayer.IncrementProgress();
            }
        }

        private void OnBuildingDead(object sender, GameEventArgs e)
        {
            var ne = e as BuildingDeadEventArgs;
            if (ne.BuildingData.Category == NpcCategory.Boss)
            {
                m_Pause.isEnabled = false;
            }
        }

        private void OnCharacterDead(object sender, GameEventArgs e)
        {
            var ne = e as CharacterDeadEventArgs;
            var npcData = ne.CharacterData as NpcCharacterData;
            if (npcData != null && npcData.Category == NpcCategory.Boss)
            {
                m_Pause.isEnabled = false;
            }
        }

        private void OnInstanceParamChanged(object sender, GameEventArgs e)
        {
            ShowOrHideRequest();
        }

        protected override void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            // Empty.
        }

        private void OnAutoFightStateChanged(object sender, GameEventArgs e)
        {
            m_Auto.Set(GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled, false);
        }

        private void OnSwitchHeroStart(object sender, GameEventArgs e)
        {
            m_SkillContinualTapDisplayer.Hide();
        }

        private void OnSwitchHeroComplete(object sender, GameEventArgs e)
        {
            var ne = e as SwitchHeroCompleteEventArgs;
            if (!ne.IsMe && !m_PvpInfo.Panel.activeSelf)
            {
                return;
            }

            RefreshSkillButtons();
            RefreshBattleHero(ne.IsMe);
            ShowHeroIcons();
        }

        private void OnEnterAltSkill(object sender, GameEventArgs e)
        {
            RefreshSkillButtons();
        }

        private void OnLeaveAltSkill(object sender, GameEventArgs e)
        {
            RefreshSkillButtons();
        }

        private void OnInstanceStartTimeChanged(object sender, GameEventArgs e)
        {
            UpdateInstanceStartTime();
        }

        private void OnShowBoss(object sender, GameEventArgs e)
        {
            ShowBossEventArgs ne = e as ShowBossEventArgs;
            if (m_BossData != null)
            {
                Log.Warning("Boss bar is already exist.");
            }

            m_BossData = ne.BossData;
            m_BossCharacterData = m_BossData as NpcCharacterData;
            m_BossHPBarCount = ne.BossHPBarCount;
            m_BossName.text = m_BossData.Name;
            if (!(m_BossData is BuildingData))
            {
                m_BossHPPanel.SetActive(true);
                m_BossElement.InitElementView(m_BossData.ElementId);
            }

            m_BossSteadyProgressBar.gameObject.SetActive(m_BossCharacterData != null && m_BossCharacterData.Steady.IsSteadying);
            m_BossSteadyRecoverProgressBar.gameObject.SetActive(m_BossCharacterData != null && !m_BossCharacterData.Steady.IsSteadying);
        }

        private void OnPropagandaBegin(object sender, GameEventArgs e)
        {
            if (m_PropagandaInstanceLogic == null)
            {
                return;
            }

            var ne = e as InstancePropagandaBeginEventArgs;
            if (ne == null)
            {
                Log.Warning("[BattleForm OnPropagandaBegin] Unexpected event args: " + e);
                return;
            }

            if (ne.Data == null)
            {
                Log.Warning("[BattleForm OnPropagandaBegin] Data is invalid.");
                return;
            }

            m_PropagandaGroup.SetActive(true);
            m_PropagandaGroup.GetComponent<Animation>()[PropagandaAnimationInName].time = 0;
            m_PropagandaGroup.GetComponent<Animation>().Play(PropagandaAnimationInName);

            m_PropagandaContent.text = ne.Data.Text;

            var npcId = m_PropagandaInstanceLogic.GetNpcIdFromIndex(ne.Data.OwnerNpcIndex);

            var dtNpc = GameEntry.DataTable.GetDataTable<DRNpc>();
            DRNpc rowNpc = dtNpc.GetDataRow(npcId);
            if (rowNpc != null)
            {
                m_PropagatorName.text = GameEntry.Localization.GetString(rowNpc.Name);
                SetPortraitIcon(rowNpc.IconId, m_PropagatorPortrait);
            }
            else
            {
                m_PropagatorName.text = string.Empty;
            }
        }

        private void OnPropagandaEnd(object sender, GameEventArgs e)
        {
            if (m_PropagandaInstanceLogic == null)
            {
                return;
            }

            var ne = e as InstancePropagandaEndEventArgs;
            if (ne == null)
            {
                Log.Warning("[BattleForm OnPropagandaBegin] Unexpected event args: " + e);
                return;
            }

            m_PropagandaGroup.GetComponent<Animation>()[PropagandaAnimationOutName].time = 0;
            m_PropagandaGroup.GetComponent<Animation>().Play(PropagandaAnimationOutName);
        }

        private void OnNpcDeadDropCoins(object sender, GameEventArgs e)
        {
            NpcDeadDropIconsEventArgs msg = e as NpcDeadDropIconsEventArgs;
            m_DropCoinsLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", msg.DropCoins);
        }

        private void OnBuildingDeadDropCoins(object sender, GameEventArgs e)
        {
            var buildingMsg = e as BuildingDeadDropIconsEventArgs;
            if (buildingMsg != null)
            {
                m_DropCoinsLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", buildingMsg.DropCoins);
            }
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.NetworkClosedEventArgs;

            if (ne.NetworkChannel.Name != Constant.Network.RoomNetworkChannelName)
            {
                return;
            }

            HasOpenShield = true;
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.NetworkConnectedEventArgs;

            if (ne.NetworkChannel.Name != Constant.Network.RoomNetworkChannelName)
            {
                return;
            }

            HasOpenShield = false;
        }

        private void OnCameraAnimAboutToStart(object sender, GameEventArgs e)
        {
            if (m_MovieMaskGoingIn || m_MovieMaskGoingOut)
            {
                return;
            }

            StartCoroutine(PlayMovieMaskInAnimAndWait());
        }

        private void OnCameraAnimAboutToStop(object sender, GameEventArgs e)
        {
            if (m_MovieMaskGoingIn || m_MovieMaskGoingOut)
            {
                return;
            }

            StartCoroutine(PlayMovieMaskOutAnimAndWait());
        }

        private void OnCharacterPortaging(object sender, GameEventArgs e)
        {
            if (m_MovieMaskPlaying)
                return;

            StartCoroutine(PlayMovieMaskInAndOut());
        }

        private void OnPerformChargeSkill(object sender, GameEventArgs e)
        {
            var chargeSkillData = e as PerformChargeSkillEventArgs;

            if (chargeSkillData.HandleType == PerformChargeSkillEventArgs.OperateType.Show)
            {
                var dtSkillChargeTime = GameEntry.DataTable.GetDataTable<DRSkillChargeTime>();
                var drSkillChargeTime = dtSkillChargeTime.GetDataRow(chargeSkillData.SkillId);

                if (drSkillChargeTime == null)
                {
                    Log.Warning("Can not find charge skill configuration which id is '{0}'", chargeSkillData.SkillId);
                    return;
                }

                m_ChargeSkillProgressBar.Show(drSkillChargeTime);
            }
            else if (chargeSkillData.HandleType == PerformChargeSkillEventArgs.OperateType.Hide)
            {
                m_PerformingChargeSkillId = int.MinValue;
                m_ChargeSkillProgressBar.Hide();
                GameEntry.SceneLogic.MeHeroCharacter.Motion.OnShownChargeSkill();
            }
            else
            {
                m_ChargeSkillProgressBar.Hide();
            }
        }

    }
}

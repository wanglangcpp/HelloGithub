using GameFramework;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BasePvpInstanceLogic
    {
        private class Runner : AbstractInstanceRunner
        {
            public override void Init(BaseInstanceLogic instanceLogic)
            {
                base.Init(instanceLogic);

                GameEntry.Event.Subscribe(EventId.MyHeroMovingUpdate, OnMyHeroMovingUpdate);
                GameEntry.Event.Subscribe(EventId.OtherHeroMovingUpdate, OnOtherHeroMovingUpdate);
                GameEntry.Event.Subscribe(EventId.MyHeroPerformSkillStart, OnMyHeroPerformSkillStart);
                GameEntry.Event.Subscribe(EventId.MyHeroPerformSkillEnd, OnMyHeroPerformSkillEnd);
                GameEntry.Event.Subscribe(EventId.UpdateSkillRushing, OnUpdateSkillRushing);
                GameEntry.Event.Subscribe(EventId.EntitySkillRushingFromNetwork, OnEntitySkillRushingFromNetwork);
                GameEntry.Event.Subscribe(EventId.EntitySkillFFFromNetwork, OnEntitySkillFastForwardFromNetwork);
                GameEntry.Event.Subscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                GameEntry.Event.Subscribe(EventId.ApplicationFocus, OnApplicationFocus);
                GameEntry.Event.Subscribe(EventId.GetRoomBattleStatus, OnGetBattleStatus);

                var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                pvp.m_InstanceTimer = new InstanceTimer(pvp.DRInstance.TimerType, pvp.DRInstance.TimerDuration, pvp.DRInstance.TimerAlert, pvp.InstanceStartTime);
            }

            

            public override void Shutdown(bool isExternalShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.MyHeroMovingUpdate, OnMyHeroMovingUpdate);
                    GameEntry.Event.Unsubscribe(EventId.OtherHeroMovingUpdate, OnOtherHeroMovingUpdate);
                    GameEntry.Event.Unsubscribe(EventId.MyHeroPerformSkillStart, OnMyHeroPerformSkillStart);
                    GameEntry.Event.Unsubscribe(EventId.MyHeroPerformSkillEnd, OnMyHeroPerformSkillEnd);
                    GameEntry.Event.Unsubscribe(EventId.UpdateSkillRushing, OnUpdateSkillRushing);
                    GameEntry.Event.Unsubscribe(EventId.EntitySkillRushingFromNetwork, OnEntitySkillRushingFromNetwork);
                    GameEntry.Event.Unsubscribe(EventId.EntitySkillFFFromNetwork, OnEntitySkillFastForwardFromNetwork);
                    GameEntry.Event.Unsubscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                    GameEntry.Event.Unsubscribe(EventId.ApplicationFocus, OnApplicationFocus);
                    GameEntry.Event.Unsubscribe(EventId.GetRoomBattleStatus, OnGetBattleStatus);
                }

                base.Shutdown(isExternalShutdown);
            }

            private void OnUpdateSkillRushing(object sender, GameEventArgs e)
            {
                var ne = e as UpdateSkillRushingEventArgs;
                GameEntry.RoomLogic.UpdateSkillRushing(ne.EntityId, ne.SkillId, ne.Position, ne.Rotation, ne.HasJustStarted);
            }

            private void OnMyHeroMovingUpdate(object sender, GameEventArgs e)
            {
                var ne = e as MyHeroMovingUpdateEventArgs;
                GameEntry.RoomLogic.UpdateMoving(ne.EntityId, ne.Position.ToVector2(), ne.Rotation, ne.ForceSendPacket);
            }

            private void OnMyHeroPerformSkillStart(object sender, GameEventArgs e)
            {
                var ne = e as MyHeroPerformSkillStartEventArgs;
                GameEntry.RoomLogic.PerformSkillStart(ne.EntityId, ne.Position.ToVector2(), ne.Rotation, ne.SkillId);
            }

            private void OnMyHeroPerformSkillEnd(object sender, GameEventArgs e)
            {
                var ne = e as MyHeroPerformSkillEndEventArgs;
                GameEntry.RoomLogic.PerformSkillEnd(ne.EntityId, ne.Position.ToVector2(), ne.Rotation, ne.SkillId, ne.Reason);
            }

            private void OnOtherHeroMovingUpdate(object sender, GameEventArgs e)
            {
                var ne = e as OtherHeroMovingUpdateEventArgs;
                var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                var heroCharacter = pvp.m_OtherPlayer.GetCurrentHeroCharacter();
                if (heroCharacter == null || heroCharacter.Id != ne.EntityId || heroCharacter.Motion == null)
                {
                    return;
                }

                heroCharacter.Data.Position = ne.Position;
                heroCharacter.Data.Rotation = ne.Rotation;
                var position = AIUtility.SamplePosition(ne.Position);
                heroCharacter.Motion.StartMove(position);
            }

            private void OnEntitySkillRushingFromNetwork(object sender, GameEventArgs e)
            {
                var ne = e as EntitySkillRushingFromNetworkEventArgs;
                var packet = ne.Packet;
                var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                var heroCharacter = pvp.m_OtherPlayer.GetCurrentHeroCharacter();
                if (heroCharacter == null || heroCharacter.Id != packet.EntityId)
                {
                    return;
                }

                heroCharacter.Data.Rotation = packet.Transform.Rotation;
                var res = heroCharacter.Motion.TryUpdateSkillRushing(packet.SkillId, new Vector2(packet.Transform.PositionX, packet.Transform.PositionY), packet.Transform.Rotation);
                GameEntry.RoomLogic.AddLog("OnEntitySkillRushingFromNetwork", "SkillId: {0}, Position: {1}, Rotation: {2}, Result: {3}",
                    packet.SkillId.ToString(), new Vector2(packet.Transform.PositionX, packet.Transform.PositionY).ToString(), packet.Transform.Rotation.ToString(), res.ToString());
            }

            private void OnEntitySkillFastForwardFromNetwork(object sender, GameEventArgs e)
            {
                var ne = e as EntitySkillFFFromNetworkEventArgs;
                var packet = ne.Packet;
                var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                var oppHeroDatas = pvp.m_OppHeroesData.GetHeroes();

                bool res = false;
                for (int i = 0; i < oppHeroDatas.Length; ++i)
                {
                    if (packet.EntityId == oppHeroDatas[i].Id)
                    {
                        var hero = GameEntry.Entity.GetGameEntity(packet.EntityId) as HeroCharacter;
                        res = TryFastForwardHeroSkill(packet, hero);
                        break;
                    }
                }

                GameEntry.RoomLogic.AddLog("OnEntitySkillFastForwardFromNetwork", "EntityId: {0}, Result: {1}", packet.EntityId.ToString(), res.ToString());
            }

            private void OnGetRoomStatus(object sender, GameEventArgs e)
            {
                var msg = e as GetRoomStatusEventArgs;
                if (msg.RoomStatus == (int)ServerErrorCode.RoomStatusError || msg.RoomStatus == (int)RoomStateType.Finish)
                {
                    (m_InstanceLogic as BasePvpInstanceLogic).EnforceGoBackToLobby();
                    return;
                }
            }
            private void OnApplicationFocus(object sender, GameEventArgs e)
            {
                ApplicationFocusEventArgs focusArgs = e as ApplicationFocusEventArgs;
                if (focusArgs == null)
                {
                    return;
                }
                if (!focusArgs.State)
                {
                    var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                    pvp.m_Me.GetCurrentHeroCharacter().Motion.BreakSkills();
                }
                if (focusArgs.BackgroundTime >= 30f)
                {
                    GameEntry.Restart();
                }
                else
                {
                    GameEntry.RoomLogic.GetRoomBattleInfo();
                }
            }
            private void OnGetBattleStatus(object sender, GameEventArgs e)
            {
                RCGetRoomBattleStatus battleStatus = (e as GetRoomBattleStatusEventArgs).BattleStatus;
                var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                pvp.SetRoomBattleStatus(battleStatus);
                var dataRow = pvp.DRInstance;
                pvp.InstanceStartTime = Time.time - (dataRow.TimerDuration - battleStatus.LeftTime);
                pvp.m_InstanceTimer.StopTimer();
                pvp.m_InstanceTimer = new InstanceTimer(pvp.DRInstance.TimerType, pvp.DRInstance.TimerDuration, pvp.DRInstance.TimerAlert, pvp.InstanceStartTime);
                GameEntry.Data.Room.HasReconnected = false;
                //GameEntry.RoomLogic.ClaimRoomReconnectReady();
            }
            private static bool TryFastForwardHeroSkill(RCPushEntityPerformSkillFF packet, HeroCharacter hero)
            {
                if (hero == null || hero.Motion == null)
                {
                    return false;
                }

                hero.CachedTransform.localPosition = new Vector3(packet.Transform.PositionX, 0f, packet.Transform.PositionY);
                hero.CachedTransform.localEulerAngles = new Vector3(0f, packet.Transform.Rotation, 0f);
                return hero.Motion.TryFastForwardSkill(packet.SkillId, packet.TargetTime);
            }

            public override void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                FireShouldGoToResult();
            }

            public override void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                FireShouldGoToResult();
            }

            public override void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }
        }
    }
}

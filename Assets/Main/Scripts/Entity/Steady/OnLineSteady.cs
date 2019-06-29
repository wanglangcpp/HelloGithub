using System;
using GameFramework.Event;
using GameFramework.Network;

namespace Genesis.GameClient
{
    public class OnLineSteady : BaseSteady
    {
        public override float Steady
        {
            set
            {
                if (value >= MaxSteady && m_Steady < MaxSteady)
                {
                    m_Steady = MaxSteady;
                }
                else if (value <= 0 && m_Steady > 0)
                {
                    m_Steady = 0;
                }
                else
                {
                    m_Steady = value;
                }
            }
            get
            {
                return m_Steady;
            }
        }

        public override void OnShow()
        {
            base.OnShow();

            GameEntry.Event.Subscribe(EventId.PushEntitySteadyChanged, OnSteadyStatusChanged);
            GameEntry.Event.Subscribe(EventId.SinglePvpOtherPlayerDisconnect, OnPlayerConnected);
        }



        public override void OnHide()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.PushEntitySteadyChanged, OnSteadyStatusChanged);
                GameEntry.Event.Unsubscribe(EventId.SinglePvpOtherPlayerDisconnect, OnPlayerConnected);
            }
            base.OnHide();
        }

        private void OnSteadyStatusChanged(object sender, GameEventArgs eventArgs)
        {
            var e = eventArgs as PushEntitySteadyChangedEventArgs;

            if (Owner == null || Owner.Id != e.SteadyData.ChangedEntityId)
            {
                return;
            }
            //自己不处理自己发送的消息
            if (e.SteadyData.PlayerId == GameEntry.Data.Player.Id)
            {
                return;
            }
            //掉线后由其他玩家计算
            if (GameEntry.SceneLogic.SinglePvpInstanceLogic.OppHeroesData.IsBelongToPlayer(m_Owner.Id) && GameEntry.Data.PvpArenaOpponent.DisConnected)
            {
                return;
            }
            //这个消息是给对面玩家推送的
            if (e.SteadyData.SteadyStatus)
            {
                RecoverSteady(false);
            }
            else
            {
                BreakSteady(false);
            }
            if (e.SteadyData.SteadyValue > 0)
            {
                m_Steady = e.SteadyData.SteadyValue;
            }
            m_IsSteadying = e.SteadyData.SteadyStatus;
        }
        public override void UpdateSteady(float elapseSeconds = 0f)
        {

            if (m_Owner == null)
            {
                return;
            }

            if (MaxSteady <= 0f)
            {
                return;
            }

            if (GameEntry.SceneLogic.BaseInstanceLogic.IsRunning == false)
            {
                return;
            }

            if (IsSteadying)
            {
                if (Steady <= 0f)
                {
                    BreakSteady();
                }
            }
            else
            {
                Steady += SteadyRecoverSpeed * elapseSeconds;
                if (Steady >= MaxSteady)
                {
                    RecoverSteady();
                }
            }
        }
        private void RecoverSteady(bool send=true)
        {
            Steady = MaxSteady;
            IsSteadying = true;
            m_Owner.AddBuff(m_Owner.SteadyBuffId, m_Owner.Data, OnlineBuffPool.GetNextSerialId(), null);
            GameEntry.Event.Fire(this, new SteadyRecoveredEventArgs(m_Owner));
            if (!send)
            {
                return;
            }
            if (GameEntry.SceneLogic.SinglePvpInstanceLogic.OppHeroesData.IsBelongToPlayer(m_Owner.Id) && GameEntry.Data.PvpArenaOpponent.DisConnected)
            {
                //属于对面玩家且对面玩家掉线
                //向服务器更新霸体值
                CRPushEntitySteadyChanged request = new CRPushEntitySteadyChanged();
                request.PlayerId = GameEntry.Data.Player.Id;
                request.TargetPlayerId = GameEntry.Data.PvpArenaOpponent.Player.Id;
                request.ChangedEntityId = m_Owner.Id;
                request.SteadyStatus = true;
                GameEntry.Network.Send(request);
                return;
            }

            if (GameEntry.SceneLogic.MeHeroCharacter.Id == m_Owner.Id)
            {
                CRPushEntitySteadyChanged request = new CRPushEntitySteadyChanged();
                request.PlayerId = GameEntry.Data.Player.Id;
                request.TargetPlayerId = GameEntry.Data.Player.Id;
                request.ChangedEntityId = m_Owner.Id;
                request.SteadyStatus = true;
                GameEntry.Network.Send(request);
            }
        }

        private void BreakSteady(bool send = true)
        {
            Steady = 0f;
            IsSteadying = false;
            m_Owner.RemoveBuffById(m_Owner.SteadyBuffId);
            GameEntry.Event.Fire(this, new SteadyBreakedEventArgs(m_Owner));
            if (!send)
            {
                return;
            }
            if (GameEntry.SceneLogic.SinglePvpInstanceLogic.OppHeroesData.IsBelongToPlayer(m_Owner.Id) && GameEntry.Data.PvpArenaOpponent.DisConnected)
            {
                //属于对面玩家且对面玩家掉线
                //向服务器更新霸体值
                CRPushEntitySteadyChanged request = new CRPushEntitySteadyChanged();
                request.PlayerId = GameEntry.Data.Player.Id;
                request.TargetPlayerId = GameEntry.Data.PvpArenaOpponent.Player.Id;
                request.ChangedEntityId = m_Owner.Id;
                request.SteadyStatus = false;
                GameEntry.Network.Send(request);
                return;
            }

            if (GameEntry.SceneLogic.MeHeroCharacter.Id == m_Owner.Id)
            {
                CRPushEntitySteadyChanged request = new CRPushEntitySteadyChanged();
                request.PlayerId = GameEntry.Data.Player.Id;
                request.TargetPlayerId = GameEntry.Data.Player.Id;
                request.ChangedEntityId = m_Owner.Id;
                request.SteadyStatus = false;
                GameEntry.Network.Send(request);
            }
        }

        private void OnPlayerConnected(object sender, GameEventArgs e)
        {
            //掉线是true
            SinglePvpOtherPlayerDisconnectEventArgs playerConnect = e as SinglePvpOtherPlayerDisconnectEventArgs;
            if (playerConnect.PlayerId == GameEntry.Data.Player.Id)
            {
                return;
            }
            if (playerConnect.ConnectState==true)
            {
                return;
            }
            CRPushEntitySteadyChanged request = new CRPushEntitySteadyChanged();
            request.PlayerId = GameEntry.Data.Player.Id;
            request.ChangedEntityId = Owner.Id;
            request.TargetPlayerId = playerConnect.PlayerId;
            request.SteadyStatus = IsSteadying;
            request.SteadyValue = (int)Steady;
            GameEntry.Network.Send(request);
            //CRPushEntitySteadyChanged requestMe = new CRPushEntitySteadyChanged();
            //requestMe.PlayerId = GameEntry.Data.Player.Id;
            //requestMe.TargetPlayerId = playerConnect.PlayerId;
            //requestMe.SteadyStatus = IsSteadying;
            //requestMe.SteadyValue = (int)Steady;
            //GameEntry.Network.Send(requestMe);

        }
    }
}

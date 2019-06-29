using GameFramework.Event;
using GameFramework.Network;
using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家体力组件。
    /// </summary>
    public class PlayerEnergyComponent : MonoBehaviour
    {
        private enum TickInterval
        {
            EverySecond,
            EveryFrame,
        }

        [SerializeField]
        private TickInterval m_TickInterval = TickInterval.EverySecond;

        public int Energy
        {
            get
            {
                return GameEntry.Data.Player.Energy;
            }
        }

        public DateTime NextEnergyRecoveryTime
        {
            get
            {
                return GameEntry.Data.Player.NextEnergyRecoveryTime;
            }
        }

        public TimeSpan EnergyRecoveryLeftTime
        {
            get
            {
                if (GameEntry.Time.LobbyServerTimeState != TimeStateType.Set)
                {
                    return TimeSpan.Zero;
                }

                if (NextEnergyRecoveryTime < GameEntry.Time.LobbyServerUtcTime)
                {
                    return TimeSpan.Zero;
                }

                return NextEnergyRecoveryTime - GameEntry.Time.LobbyServerUtcTime;
            }
        }

        private bool m_RequestingPlayerEnergy = false;
        INetworkChannel m_CachedLobbyChannel;
        private INetworkChannel CachedLobbyChannel
        {
            get
            {
                if(null == m_CachedLobbyChannel)
                    m_CachedLobbyChannel = GameEntry.Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName);
                return m_CachedLobbyChannel;
            }
        }

        #region MonoBehaviour

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.GotPlayerEnergy, OnGotPlayerEnergy);
        }

        private void OnEnable()
        {
            StartCoroutine(CheckEnergyRecoveryCo());
            RequestPlayerEnergyIfPossible();
        }

        //private void OnDisable()
        //{
        //    StopCoroutine("CheckEnergyRecoveryCo");
        //}

        #endregion MonoBehaviour

        private IEnumerator CheckEnergyRecoveryCo()
        {
            while (true)
            {
                if (m_TickInterval == TickInterval.EveryFrame)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(10f);
                }
                if (EnergyRecoveryLeftTime > TimeSpan.Zero ||
                    null == GameEntry.Data ||
                    string.IsNullOrEmpty(GameEntry.Data.Player.Name) ||
                    GameEntry.Data.Player.Energy >= 120 )//|| !TimeComponent.IsInvalidTime(NextEnergyRecoveryTime)
                {
                    continue;
                }
                RequestPlayerEnergyIfPossible();
            }
        }

        private void RequestPlayerEnergyIfPossible()
        {
            if (null == GameEntry.Procedure ||
                null == GameEntry.Procedure.CurrentProcedure ||
                !(GameEntry.Procedure.CurrentProcedure is ProcedureMain) ||
                m_RequestingPlayerEnergy)
            {
                return;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                if (CachedLobbyChannel == null || !CachedLobbyChannel.Connected)
                {
                    return;
                }

                m_RequestingPlayerEnergy = true;
                GameEntry.Network.Send(new CLGetPlayerEnergy());
                return;
            }

            // Offline mode.
            m_RequestingPlayerEnergy = true;
            var response = new LCGetPlayerEnergy
            {
                PlayerInfo = new PBPlayerInfo
                {
                    Id = GameEntry.Data.Player.Id,
                    Energy = Mathf.Min(120, GameEntry.Data.Player.Energy + 1),
                    NextEnergyRecoveryTime = GameEntry.Data.Player.Energy < 120 ? GameEntry.Time.LobbyServerUtcTime.AddMinutes(1.0).Ticks : 0L,
                }
        };

            LCGetPlayerEnergyHandler.Handle(this, response);
        }

        private void OnGotPlayerEnergy(object sender, GameEventArgs e)
        {
            // TODO: Add new ServerErrorCode and set m_RequestingPlayerEnergy to false when the error occurs, if needed.
            m_RequestingPlayerEnergy = false;
        }
    }
}

using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class TimeComponent : MonoBehaviour
    {
        public readonly static DateTime InvalidDateTime = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public readonly static long InvalidDateTimeTicks = InvalidDateTime.Ticks;

        [SerializeField]
        private float m_ThresholdSeconds = 1f;

        private DateTime m_LastUpdateTime = DateTime.MinValue;

        private TimeStateType m_LobbyServerTimeState = TimeStateType.Unset;
        private TimeSpan m_LobbyServerTimeError = TimeSpan.Zero;

        private TimeStateType m_RoomServerTimeState = TimeStateType.Unset;
        private TimeSpan m_RoomServerTimeError = TimeSpan.Zero;

        public TimeStateType LobbyServerTimeState
        {
            get
            {
                return m_LobbyServerTimeState;
            }
        }

        public DateTime LobbyServerTime
        {
            get
            {
                //CheckLobbyTimeStateIfNeedWarn();
                return DateTime.Now + m_LobbyServerTimeError;
            }
        }

        public DateTime LobbyServerUtcTime
        {
            get
            {
                //CheckLobbyTimeStateIfNeedWarn();
                return DateTime.UtcNow + m_LobbyServerTimeError;
            }
        }

        public TimeStateType RoomServerTimeState
        {
            get
            {
                return m_RoomServerTimeState;
            }
        }

        public DateTime RoomServerTime
        {
            get
            {
                //CheckRoomTimeStateIfNeedWarn();
                return DateTime.Now + m_RoomServerTimeError;
            }
        }

        public DateTime RoomServerUtcTime
        {
            get
            {
                //CheckRoomTimeStateIfNeedWarn();
                return DateTime.UtcNow + m_RoomServerTimeError;
            }
        }

        public DateTime ClientTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public DateTime ClientUtcTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        private void Update()
        {
            CheckTimeIsDirty();
            TrySyncLobbyServerTime();
            TrySyncRoomServerTime();
        }

        public void ResetLobbyServerTime()
        {
            m_LobbyServerTimeState = TimeStateType.Unset;
            m_LobbyServerTimeError = TimeSpan.Zero;
        }

        public void ResetRoomServerTime()
        {
            m_RoomServerTimeState = TimeStateType.Unset;
            m_RoomServerTimeError = TimeSpan.Zero;
        }

        public void ReceiveLobbyServerTime(DateTime serverUtcTime)
        {
            m_LobbyServerTimeState = TimeStateType.Set;
            m_LobbyServerTimeError = serverUtcTime - DateTime.UtcNow;
        }

        public void ReceiveRoomServerTime(DateTime serverUtcTime)
        {
            m_RoomServerTimeState = TimeStateType.Set;
            m_RoomServerTimeError = serverUtcTime - DateTime.UtcNow;
        }

        public static bool IsInvalidTime(DateTime dateTime)
        {
            return dateTime <= InvalidDateTime;
        }

        public static bool IsInvalidTime(long dateTimeTicks)
        {
            return dateTimeTicks <= InvalidDateTimeTicks;
        }

        private void CheckTimeIsDirty()
        {
            if ((DateTime.UtcNow - m_LastUpdateTime).TotalSeconds >= m_ThresholdSeconds)
            {
                if (m_LobbyServerTimeState == TimeStateType.Set)
                {
                    m_LobbyServerTimeState = TimeStateType.Dirty;
                }

                if (m_RoomServerTimeState == TimeStateType.Set)
                {
                    m_RoomServerTimeState = TimeStateType.Dirty;
                }
            }

            m_LastUpdateTime = DateTime.UtcNow;
        }

        private void TrySyncLobbyServerTime()
        {
            if (m_LobbyServerTimeState == TimeStateType.Set || m_LobbyServerTimeState == TimeStateType.BeingSet)
            {
                return;
            }

            if (GameEntry.Data.Player.Id <= 0)
            {
                // 未登录
                return;
            }

            m_RoomServerTimeState = TimeStateType.BeingSet;
            CLHeartBeat pbHeartBeat = new CLHeartBeat();
            pbHeartBeat.ClientTime = DateTime.UtcNow.Ticks;
            GameEntry.Network.Send(pbHeartBeat);
        }

        private void TrySyncRoomServerTime()
        {
            if (m_RoomServerTimeState == TimeStateType.Set || m_RoomServerTimeState == TimeStateType.BeingSet)
            {
                return;
            }

            if (GameEntry.Data.Room.Id <= 0 || GameEntry.Data.Room.HasReconnected)
            {
                // 未登录
                return;
            }

            m_RoomServerTimeState = TimeStateType.BeingSet;
            CRHeartBeat pbHeartBeat = new CRHeartBeat();
            pbHeartBeat.ClientTime = DateTime.UtcNow.Ticks;
            GameEntry.Network.Send(pbHeartBeat);
        }

        private void CheckLobbyTimeStateIfNeedWarn()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            if (m_LobbyServerTimeState == TimeStateType.Set)
            {
                return;
            }

            Log.Warning("You should not get lobby server time before sync, current state is '{0}'.", m_LobbyServerTimeState.ToString());
        }

        private void CheckRoomTimeStateIfNeedWarn()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            if (m_RoomServerTimeState == TimeStateType.Set)
            {
                return;
            }

            Log.Warning("You should not get room server time before sync, current state is '{0}'.", m_RoomServerTimeState.ToString());
        }
        #region 转移后台逻辑
        private float timeGoBackground = -1f;
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                if (timeGoBackground > 0f)
                {
                    float timeGap = Time.realtimeSinceStartup - timeGoBackground;
                    timeGoBackground = 0f;
                    GameEntry.Event.Fire(EventId.ApplicationFocus, new ApplicationFocusEventArgs(true, timeGap));
                }
            }
            else
            {
                GameEntry.Event.Fire(EventId.ApplicationFocus, new ApplicationFocusEventArgs(false, 0f));
                timeGoBackground = Time.realtimeSinceStartup;
            }
        }
        
        #endregion
    }
    public class ApplicationFocusEventArgs : GameFramework.Event.GameEventArgs
    {
        /// <summary>
        /// True进前台 False进后台
        /// </summary>
        public bool State { private set; get; }
        /// <summary>
        /// 在后台持续的事件
        /// </summary>
        public float BackgroundTime { private set; get; }
        public ApplicationFocusEventArgs(bool state,float backgroundTime)
        {
            State = state;
            BackgroundTime = backgroundTime;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ApplicationFocus;
            }
        }
    }
}

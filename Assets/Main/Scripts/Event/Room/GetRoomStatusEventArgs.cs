using System;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 断线重连获取Room状态信息。
    /// </summary>
    public class GetRoomStatusEventArgs : GameEventArgs
    {
        public GetRoomStatusEventArgs(int status)
        {
            RoomStatus = status;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GetRoomStatus;
            }
        }

        public int RoomStatus
        {
            get;
            set;
        }

        public RCEndSinglePVP EndData { get; private set; }
    }
}

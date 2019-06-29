using System;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class DailyLoginAcrossDayEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.DailyLoginAcrossDay;
            }
        }
    }
}

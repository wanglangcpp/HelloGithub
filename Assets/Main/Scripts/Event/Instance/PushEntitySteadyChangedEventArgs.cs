using System;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class PushEntitySteadyChangedEventArgs : GameEventArgs
    {

        public PushEntitySteadyChangedEventArgs(RCPushEntitySteadyChanged steadyData)
        {
            SteadyData = steadyData;
        }


        public override int Id
        {
            get
            {
                return (int)EventId.PushEntitySteadyChanged;
            }
        }

        public RCPushEntitySteadyChanged SteadyData
        {
            get;
            set;
        }

        
    }
}

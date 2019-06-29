using UnityEngine;
using System.Collections;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SevenDayLoginEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.SevenDayLoginDataChange;
            }
        }

        public SevenDayLoginEventArgs()
        {

        }
    }
}


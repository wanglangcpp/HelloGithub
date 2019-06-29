using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class InstanceForBossEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.ChallengeBossDataChange;
            }
        }
        public InstanceForBossEventArgs()
        {

        }
    }
}


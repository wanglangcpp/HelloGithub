using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{

    public class OpenInstanceGroupChestEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.OpenInstanceGroupChest;
            }
        }

        public RewardCollectionHelper RewardShowHelper
        {
            get;
            set;
        }

        public int ChapterId
        {
            get;
            set;
        }

        public int ChestIndex
        {
            get;
            set;
        }
    }
}

using UnityEngine;
using System.Collections;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class LoadTitleSuccessEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.LoadTitleSuccess;
            }
        }
        public LoadTitleSuccessEventArgs()
        {

        }
    }
}


using GameFramework.Event;
using Genesis.GameClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.GameClient
{
    class OpenFunctionAnimationEndEventArgs : GameEventArgs
    {
        public NGUIForm form;

        public OpenFunctionAnimationEndEventArgs(NGUIForm lobby)
        {
            form = lobby;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.OpenFunctionAnimationEnd;
            }
        }
    }
}

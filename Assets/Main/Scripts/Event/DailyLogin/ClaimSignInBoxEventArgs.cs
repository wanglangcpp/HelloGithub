using UnityEngine;
using System.Collections;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 领取签到宝箱成功事件
    /// </summary>
    public class ClaimSignInBoxEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.ClaimSignInBoxSuccess;
            }
        }

        public ClaimSignInBoxEventArgs()
        {

        }
    }
}


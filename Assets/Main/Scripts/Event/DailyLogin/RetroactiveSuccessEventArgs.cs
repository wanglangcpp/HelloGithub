using UnityEngine;
using System.Collections;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 补签成功事件
    /// </summary>
    public class RetroactiveSuccessEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.RetroactiveSuccess;
            }
        }

        public RetroactiveSuccessEventArgs(int id)
        {
            DayId = id;
        }
        /// <summary>
        /// 补签天的ID
        /// </summary>
        public int DayId;
    }
}


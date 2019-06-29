using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class StromTowerInfoEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.StromTowerDataChange;
            }
        }
        /// <summary>
        /// 当前所在层
        /// </summary>
        public int CurrentLayer { get; private set; }
        /// <summary>
        /// 已挑战次数
        /// </summary>
        public int ChallengeCount { get; private set; }
        /// <summary>
        /// 历史挑战最高层
        /// </summary>
        public int RecordMaxLayer { get; private set; }
        /// <summary>
        /// 宝箱状态
        /// </summary>
        public int BoxStatus { get; private set; }

        public StromTowerInfoEventArgs(PBInstanceForTowerInfo data)
        {
            CurrentLayer = data.CurLayerNum;
            ChallengeCount = data.ChallengeNum;
            RecordMaxLayer = data.MaxLayerNum;
            BoxStatus = data.Chest;
        }
    }
}


using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 展示获得的物品信息
    /// </summary>
    public class ShowGetItemsInfoEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.ReceiveAndShowItems;
            }
        }

        public ShowGetItemsInfoEventArgs(RewardCollectionHelper reward)
        {
            m_Rewards = reward;
        }

        private RewardCollectionHelper m_Rewards =new RewardCollectionHelper();
        public RewardCollectionHelper Rewards { get { return m_Rewards; } }

    }

}

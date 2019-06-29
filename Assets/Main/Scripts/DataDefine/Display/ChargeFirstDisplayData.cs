using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Genesis.GameClient
{
    /// <summary>
    /// 首冲奖励配置表
    /// </summary>
    public class ChargeItemsDisPlayData : UIFormBaseUserData
    {
        public List<Reward> Rewards
        {
            get;
            private set;
        }
        public int GiftId { get { return m_giftId; } }
        private int m_giftId;

        public ChargeItemsDisPlayData(List<Reward> rewad,int giftId)
        {
            Rewards = rewad;
            m_giftId = giftId;
        }

        public class Reward
        {
            public Reward(int id, int count)
            {

                Id = id;
                Count = count;
            }

            //道具编号
            public int Id { get; private set; }

            //道具数量
            public int Count { get; private set; }
        }

    }
}

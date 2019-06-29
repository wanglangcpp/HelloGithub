﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCGetItemStatusHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 2700); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCGetItemStatus);
        }

        public static void Handle(object sender, LCGetItemStatus response)
        {
            ChargeStatus GiftStatus = new ChargeStatus();
            //*****************这里添加所有购买过的钻石
            GiftStatus.FirstChargeItems.AddRange(response.firstCharges);
            //status.HasGetGifts.AddRange(response.hasGetGifts);
            //status.NoGetGifts.AddRange(response.noGetGifts);
            //status.DailyRestricts.AddRange(response.dailyRestricts);
            foreach (var item in response.giftInfo)
            {
                int giftType = GameEntry.DataTable.GetDataTable<DRGfitBag>().GetDataRow(item.GiftId).Type;
                GiftType m_GiftType = (GiftType)giftType;//礼包的类型
                int stasus = -1;//状态默认为未购买

                //if (m_GiftType == GiftType.ChargeFirst)
                //{
                //    if (GiftStatus.FirstChargeItems.Count != 0)
                //    {
                //        stasus = GetGiftStatus(m_GiftType, item.BuyTime, item.ClaimTime);
                //    }
                //}
                //else
                //{
                stasus = GetGiftStatus(m_GiftType, item.BuyTime, item.ClaimTime);
                if (m_GiftType == GiftType.MonthCard)
                {
                    System.TimeSpan gapTime = System.DateTime.Now - new System.DateTime(item.BuyTime, System.DateTimeKind.Utc);
                    double RemainingTime = gapTime.TotalDays;//计算月卡持续时间
                    if (RemainingTime < 30)
                    {
                        //************这里添加了月卡及其持续的时间
                        GiftStatus.MonthCardTime.Add(item.GiftId, (int)RemainingTime);
                    }

                }
                //}
                //******************在这里添加所有礼包的状态
                GiftStatus.GiftStatus.Add(item.GiftId, stasus);
            }
            GameEntry.Data.ChargeStatusData.UpdateData(GiftStatus);
            GameEntry.Event.Fire(sender, new GetChargeItemStatusEventArgs());
        }

        /// <summary>
        /// 获取礼包的状态
        /// </summary>
        /// <param name="giftType">礼包的类型</param>
        /// <param name="buyTime">购买时间</param>
        /// <param name="claimTime">领取时间</param>
        /// <returns>返回礼包的状态（-1未购买，0未领取，1已领取）</returns>
        private static int GetGiftStatus(GiftType giftType, long buyTime, long claimTime)
        {
            if (giftType == GiftType.ChargeFirst)
            {
                if (claimTime >= buyTime)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (giftType == GiftType.MonthCard)
                {
                    System.TimeSpan gapTime = System.DateTime.Now - new System.DateTime(buyTime, System.DateTimeKind.Utc);
                    double RemainingTime = gapTime.TotalDays;//计算月卡持续时间
                    if (RemainingTime > 30)
                    {
                        return -1;
                    }
                    else
                    {
                        System.DateTime m_ClaimTime = new System.DateTime(claimTime, System.DateTimeKind.Utc);
                        System.DateTime m_NowTime = System.DateTime.Now;
                        if (m_NowTime.Year > m_ClaimTime.Year
                            || m_NowTime.Month > m_ClaimTime.Month
                            || m_NowTime.Day > m_ClaimTime.Day)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                else
                {
                    if (claimTime < buyTime)
                    {
                        return 0;
                    }
                    else
                    {
                        System.DateTime m_BuyTime = new System.DateTime(buyTime, System.DateTimeKind.Utc);
                        System.DateTime m_NowTime = System.DateTime.Now;
                        if (m_NowTime.Year > m_BuyTime.Year
                            || m_NowTime.Month > m_BuyTime.Month
                            || m_NowTime.Day > m_BuyTime.Day)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
        }
    }
}

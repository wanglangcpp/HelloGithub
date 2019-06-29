using UnityEngine;
using GameFramework;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    public class TalkingDataMgr
    {
        private bool isDevelop = false;

        private TDGAAccount account;
        private bool isInited = false;
        private string mOrderId;//订单号
        private Dictionary<string, ChargeInfo> chargeDict;
        private PayInfos payInfo;
        private string mAccountId;

        public void InitSDK(string channelCode)
        {
            if (isDevelop) return;

            if (isInited) return;
            isInited = true;
            Log.Debug("TalkingData init begin ");
            TalkingDataGA.OnStart("5FDA3C19EFD04614B7940273AA64DD12", channelCode);
            Log.Debug("TalkingData init completed,channelCode:" + channelCode);
        }

        //设置唯一账户
        public void SetAccount(string accountId)
        {
            if (isDevelop) return;
            Log.Debug("TalkingData SetAccount:" + accountId);
            account = TDGAAccount.SetAccount(accountId);
            mAccountId = accountId;
            if (IsGuest(accountId))
            {
                account.SetAccountType(AccountType.ANONYMOUS);
            }
        }
        //设置账户类型
        public void SetAccountName(String playerID)
        {
            if (isDevelop) return;
            if (string.IsNullOrEmpty(mAccountId))
            {
                Log.Debug("TalkingData SetAccountName by null SetAccount");
            }
            else
            {
                Log.Debug("TalkingData SetAccountName:" + playerID);
                account.SetAccountName(playerID);
                account.SetAccountType(AccountType.REGISTERED);
            }
        }

        //设置级别
        public void SetLevel(int level)
        {
            if (isDevelop) return;
            Log.Debug("TalkingData SetLevel:" + level);
            account.SetLevel(level);
        }
        //设置性别
        public void SetGender(Gender type)
        {
            if (isDevelop) return;
            Log.Debug("TalkingData SetGender:" + type);
            account.SetGender(type);
        }
        //设置年龄,范围为 0 - 120
        public void SetAge(int age)
        {
            if (isDevelop) return;
            Log.Debug("TalkingData SetAge:" + age);
            account.SetAge(age);
        }
        //设置区服
        public void SetGameServer(string gameServer)
        {
            if (isDevelop) return;
            Log.Debug("TalkingData SetGameServer:" + gameServer);
            account.SetGameServer(gameServer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单号：最多64个字符</param>
        /// <param name="iapId">充值包 ID</param>
        /// <param name="currencyAmount">现金金额</param>
        /// <param name="currencyType">币种：人民币 CNY；美元 USD，ISO4217 中规范</param>
        /// <param name="virtualCurrencyAmount">虚拟币金额，例如多少钻石</param>
        /// <param name="paymentType">支付的途径</param>
        public void OnChargeRequest(PayInfos pPayInfo)
        {
            if (isDevelop) return;
            payInfo = pPayInfo;
            if (chargeDict == null) chargeDict = InitChargeDict();
            double virtualCurrencyAmount = 0;
            if (chargeDict.ContainsKey(pPayInfo.itemId))
            {
                bool isFirstCharge = GameEntry.OpenFunction.IsFirstCharge();
                ChargeInfo data = chargeDict[pPayInfo.itemId];
                if (isFirstCharge)
                {
                    virtualCurrencyAmount = data.GainCount * 2;
                }
                else
                {
                    virtualCurrencyAmount = (1 + data.Extra / 100) * data.GainCount;
                }
            }
            TDGAVirtualCurrency.OnChargeRequest(pPayInfo.orderId, pPayInfo.itemId, pPayInfo.price, "CNY", virtualCurrencyAmount, "WeChat|Alipay");
        }

        //充值成功
        public void OnChargeResult(bool isSuccess)
        {
            if (isDevelop) return;
            Log.Debug("OnChargeResult:" + isSuccess);
            if (isSuccess)
            {
                TDGAVirtualCurrency.OnChargeSuccess(payInfo.orderId);
            }
            payInfo = null;
        }
        //
        /// <summary>
        /// 可能会在任务奖励、登录奖励、成就奖励等环节免费发放给玩家虚拟币
        /// </summary>
        /// <param name="virtualCurrencyAmount">虚拟币金额</param>
        /// <param name="reason">赠送虚拟币原因/类型（最多100种原因），赠予虚拟币.格式：32 个字符内的中文、空格、英文、数字。不要带有任何开发中的转义字符，如斜杠</param>
        public void OnReward(double virtualCurrencyAmount, string reason)
        {
            if (isDevelop) return;
            TDGAVirtualCurrency.OnReward(virtualCurrencyAmount, reason);
        }

        //记录付费点，例如用钻石买什么道具
        public void OnPurchase(string item, int itemNumber, double priceInVirtualCurrency)
        {
            if (isDevelop) return;
            TDGAItem.OnPurchase(item, itemNumber, priceInVirtualCurrency);
        }

        //消耗物品或服务等
        public void OnUse(string item, int itemNumber)
        {
            if (isDevelop) return;
            TDGAItem.OnUse(item, itemNumber);
        }

        private Dictionary<string, ChargeInfo> InitChargeDict()
        {
            ChargeInfo[] chargeArrays = GameEntry.Data.ChargeTable.ToArray();
            chargeDict = new Dictionary<string, ChargeInfo>();
            for (int i = 0; i < chargeArrays.Length; i++)
            {
                ChargeInfo data = chargeArrays[i];
                string id = Convert.ToString(data.Id);
                if (!chargeDict.ContainsKey(id) && data.Type != 1)
                {
                    chargeDict.Add(id, data);
                }
            }
            return chargeDict;
        }

        private bool IsGuest(string accountId)
        {
            return accountId.StartsWith("yx_");
        }
    }


    //1）示例1： 如一个游戏内UID为10000的匿名游戏玩家以匿名（快速登录）方式在国服2区进行游戏，并在游戏中由1级升至2级，之后玩家使用QQ号5830000在游戏中进行显性注册，并设定为18岁男玩家的整个过程。调用如下：
    ////游戏玩家以匿名（快速登录）方式在国服2区进行游戏时，做如下调用
    //TDGAAccount account = TDGAAccount.SetAccount("10000");
    //account.SetAccountType(AccountType.ANONYMOUS);
    //account.SetLevel(1);
    //account.SetGameServer("国服2");
    //玩家升级时，做如下调用
    //account.SetLevel(2);
    ////玩家显性注册成功时，做如下调用
    //account.SetAccountName("5830000@qq.com");
    //account.SetAccountType(AccountType.QQ);
    //account.SetAge(18);
    //account.SetGender(Gender.MALE);


}

using UnityEngine;
using System.Collections;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DRTask : IDataRow
    {
        /// <summary>
        /// 最大奖励数量
        /// </summary>
        public const int RewardCount = 4;
        /// <summary>
        /// 最大完成条件数量
        /// </summary>
        public const int ConditionCount = 4;
        /// <summary>
        /// 任务编号
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 后置任务编号
        /// </summary>
        public string AfterTask { get; private set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; private set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; private set; }
        /// <summary>
        /// 完成条件类型
        /// </summary>
        public int ComType { get; private set; }

        public struct Item { public int IconId; public int Count; }
        /// <summary>
        /// 奖励
        /// </summary>
        public List<Item> Rewards { get { return m_Rewards; } }
        private List<Item> m_Rewards = new List<Item>();

        /// <summary>
        /// 活跃度
        /// </summary>
        public int Activeness { get; private set; }

        /// <summary>
        /// 跳转Id
        /// </summary>
        public int WhereToGet { get; private set; }

        /// <summary>
        /// 完成条件
        /// </summary>
        public List<int> Conditions { get { return m_Conditions; } }
        private List<int> m_Conditions = new List<int>();

        /// <summary>
        /// 任务图标
        /// </summary>
        public int IconId { get; private set; }

        /// <summary>
        /// 任务要求等级
        /// </summary>
        public int Level { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            AfterTask = text[index++];
            Name = text[index++];
            Desc = text[index++];
            TaskType = int.Parse(text[index++]);
            ComType = int.Parse(text[index++]);

            for (int i = 0; i < RewardCount; i++)
            {
                Item item = new Item { IconId = int.Parse(text[index++]), Count = int.Parse(text[index++]) };
                if (item.IconId != -1 && item.Count != -1)
                {
                    m_Rewards.Add(item);
                }
            }

            Activeness = int.Parse(text[index++]);
            WhereToGet = int.Parse(text[index++]);

            for (int i = 0; i < ConditionCount; i++)
            {
                //if (int.Parse(text[index]) != -1)
                //{
                m_Conditions.Add(int.Parse(text[index++]));
                //}
            }

            IconId = int.Parse(text[index++]);
            Level = int.Parse(text[index++]);
        }
    }

    public enum ComType
    {
        /// <summary>
        /// 对话
        /// </summary>
        Dialogue = 1,
        /// <summary>
        /// 打怪
        /// </summary>
        Fight,
        /// <summary>
        /// 通关
        /// </summary>
        PassCustom,
        /// <summary>
        /// 装备强化
        /// </summary>
        EquipmentUp,
        /// <summary>
        /// 英雄升星
        /// </summary>
        HeroUpStar,
        /// <summary>
        /// 英雄进阶
        /// </summary>
        HeroAdvanced,
        /// <summary>
        /// 穿戴徽章
        /// </summary>
        WearBadge,
        /// <summary>
        /// 星图激活
        /// </summary>
        StarMap,
        /// <summary>
        /// 收集英雄
        /// </summary>
        CollectionHreo,
        /// <summary>
        /// 收集某个星级的英雄
        /// </summary>
        CollectionStarHreo,
        /// <summary>
        /// 好友数量
        /// </summary>
        FriendCount,
        /// <summary>
        /// 赠送体力
        /// </summary>
        GivePower,
        /// <summary>
        /// 参加PVP
        /// </summary>
        PVP,
        /// <summary>
        /// 参加副本
        /// </summary>
        JingYanFuBen,
        /// <summary>
        /// 抽奖
        /// </summary>
        GameDraw,
        /// <summary>
        /// 购买体力
        /// </summary>
        BuyPower,
        /// <summary>
        /// 购买金币
        /// </summary>
        BuyGold,
        /// <summary>
        /// 达到某个等级
        /// </summary>
        LevelUp,
        /// <summary>
        /// 购买商城道具
        /// </summary>
        BuyShopItem,
        /// <summary>
        /// 参加排位赛
        /// </summary>
        RankMatch,
        /// <summary>
        /// 每日登陆
        /// </summary>
        LogIn,
        /// <summary>
        /// 累计在线
        /// </summary>
        LogInTime,
        /// <summary>
        /// 扫荡或通关关卡
        /// </summary>
        PassCustomCount,
        /// <summary>
        /// pvp获胜
        /// </summary>
        PvpWin,
        /// <summary>
        /// 任意副本
        /// </summary>
        AnyFuBen,
        /// <summary>
        /// 金币副本
        /// </summary>
        CoinFuBen,
        /// <summary>
        /// Boss副本
        /// </summary>
        BossFuBen,
        /// <summary>
        /// 爬塔副本
        /// </summary>
        ToWerFuBen,
    }
}


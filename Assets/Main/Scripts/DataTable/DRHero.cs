using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄配置表。
    /// </summary>
    public class DRHero : IDataRow
    {
        private const int WhereToGetMaxCount = 10;

        /// <summary>
        /// 英雄编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 英雄名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 英雄描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 英雄职业。
        /// </summary>
        public int Profession
        {
            get;
            private set;
        }

        /// <summary>
        /// 模型编号（对应 Character 表编号）。
        /// </summary>
        public int CharacterId
        {
            get;
            private set;
        }

        /// <summary>
        /// 模型缩放比例。
        /// </summary>
        public float Scale
        {
            get;
            private set;
        }

        /// <summary>
        /// 头像编号。
        /// </summary>
        public int IconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 默认武器套装编号。
        /// </summary>
        public int DefaultWeaponSuiteId
        {
            get;
            private set;
        }

        /// <summary>
        /// 默认战力。
        /// </summary>
        public int DefaultMight
        {
            get;
            private set;
        }

        /// <summary>
        /// 默认总品阶。
        /// </summary>
        public int DefaultTotalQualityLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 默认星级。
        /// </summary>
        public int DefaultStarLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 霸体值。
        /// </summary>
        public float Steady
        {
            get;
            private set;
        }

        /// <summary>
        /// 霸体值恢复速度。
        /// </summary>
        public float SteadyRecoverSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 霸体Buff编号。
        /// </summary>
        public int SteadyBuffId
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡后等待自动切换其他英雄的时间。
        /// </summary>
        public float DeadDurationBeforeAutoSwitch
        {
            get;
            private set;
        }

        /// <summary>
        /// 躲避优先级。
        /// </summary>
        public int AvoidancePriority
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始冷却时间。
        /// </summary>
        public float CDWhenStart
        {
            get;
            private set;
        }

        /// <summary>
        /// 被切换后冷却时间。
        /// </summary>
        public float CDAfterChangeHero
        {
            get;
            private set;
        }

        /// <summary>
        /// 切换技能组编号。
        /// </summary>
        public int SwitchSkillGroupId
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能组编号。
        /// </summary>
        public int[] SkillGroupIds
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能组编号。
        /// </summary>
        /// <param name="index">技能组索引。</param>
        /// <returns>技能组编号。</returns>
        public int GetSkillGroupId(int index)
        {
            return SkillGroupIds[index];
        }

        /// <summary>
        /// 移动速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害浮动比率
        /// </summary>
        public float DamageRandomRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 英雄升星所需道具编号。
        /// </summary>
        public int StarLevelUpItemId
        {
            get;
            private set;
        }

        /// <summary>
        /// 星级系数。
        /// </summary>
        public float[] StarFactors
        {
            get;
            private set;
        }

        /// <summary>
        /// 克制属性编号。
        /// </summary>
        public int ElementId
        {
            get;
            private set;
        }

        /// <summary>
        /// 星级系数。
        /// </summary>
        /// <param name="starLevel">星级。</param>
        /// <returns>星级系数。</returns>
        public float GetStarFactor(int starLevel)
        {
            return StarFactors[starLevel - 1];
        }

        /// <summary>
        /// 升级所需道具个数。
        /// </summary>
        public int[] StarLevelUpItemCounts
        {
            get;
            private set;
        }

        /// <summary>
        /// 升级所需道具个数。
        /// </summary>
        /// <param name="starLevel">星级。</param>
        /// <returns>升级所需道具个数。</returns>
        public int GetStarLevelUpItemCount(int starLevel)
        {
            if (starLevel >= Constant.HeroStarLevelCount)
            {
                return 0;
            }
            return StarLevelUpItemCounts[starLevel];
        }

        /// <summary>
        /// 生命倍率。
        /// </summary>
        public float MaxHPFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理攻击倍率。
        /// </summary>
        public float PhysicalAttackFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理防御倍率。
        /// </summary>
        public float PhysicalDefenseFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术攻击倍率。
        /// </summary>
        public float MagicAttackFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术防御倍率。
        /// </summary>
        public float MagicDefenseFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// AI 行为。
        /// </summary>
        public string AIBehavior
        {
            get;
            private set;
        }

        /// <summary>
        /// 怒气上涨率。
        /// </summary>
        public float AngerIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础暴击率。
        /// </summary>
        public float CriticalHitProb
        {
            get;
            private set;
        }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        public float CriticalHitRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能特点描述
        /// </summary>
        public string FightCharacteristic
        {
            get;
            private set;
        }

        /// <summary>
        /// 大头像贴图编号。
        /// </summary>
        public int ProtraitTextureId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取途经编号。
        /// </summary>
        public int[] WhereToGetIds
        {
            get;
            private set;
        }

        /// <summary>
        /// 合成英雄碎片数量。
        /// </summary>
        public int PiecesPerHero
        {
            get;
            private set;
        }

        /// <summary>
        /// 待战回血。
        /// </summary>
        public float RecoverHPBase
        {
            get;
            private set;
        }

        /// <summary>
        /// 英雄详细描述。
        /// </summary>
        public string DetailDescription
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            Profession = int.Parse(text[index++]);
            CharacterId = int.Parse(text[index++]);
            Scale = float.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            DefaultWeaponSuiteId = int.Parse(text[index++]);
            DefaultMight = int.Parse(text[index++]);
            DefaultTotalQualityLevel = int.Parse(text[index++]);
            DefaultStarLevel = int.Parse(text[index++]);
            Steady = float.Parse(text[index++]);
            SteadyRecoverSpeed = float.Parse(text[index++]);
            SteadyBuffId = int.Parse(text[index++]);
            DeadDurationBeforeAutoSwitch = float.Parse(text[index++]);
            AvoidancePriority = int.Parse(text[index++]);
            CDWhenStart = float.Parse(text[index++]);
            CDAfterChangeHero = float.Parse(text[index++]);
            SwitchSkillGroupId = int.Parse(text[index++]);
            SkillGroupIds = new int[Constant.SkillGroupCount];
            for (int i = 0; i < Constant.SkillGroupCount; i++)
            {
                SkillGroupIds[i] = int.Parse(text[index++]);
            }
            Speed = float.Parse(text[index++]);
            DamageRandomRate = float.Parse(text[index++]);
            StarLevelUpItemId = int.Parse(text[index++]);
            StarFactors = new float[Constant.HeroStarLevelCount];
            StarLevelUpItemCounts = new int[Constant.HeroStarLevelCount];
            for (int i = 0; i < Constant.HeroStarLevelCount; i++)
            {
                StarFactors[i] = float.Parse(text[index++]);
                StarLevelUpItemCounts[i] = int.Parse(text[index++]);
            }
            MaxHPFactor = float.Parse(text[index++]);
            PhysicalAttackFactor = float.Parse(text[index++]);
            PhysicalDefenseFactor = float.Parse(text[index++]);
            MagicAttackFactor = float.Parse(text[index++]);
            MagicDefenseFactor = float.Parse(text[index++]);
            AIBehavior = text[index++];
            AngerIncreaseRate = float.Parse(text[index++]);
            CriticalHitProb = float.Parse(text[index++]);
            CriticalHitRate = float.Parse(text[index++]);
            FightCharacteristic = text[index++];
            ElementId = int.Parse(text[index++]);
            ProtraitTextureId = int.Parse(text[index++]);

            var whereToGetIds = new List<int>();
            for (int i = 0; i < WhereToGetMaxCount; i++)
            {
                var whereToGetId = int.Parse(text[index++]);
                if (whereToGetId > 0)
                {
                    whereToGetIds.Add(whereToGetId);
                }
            }

            WhereToGetIds = whereToGetIds.ToArray();
            PiecesPerHero = int.Parse(text[index++]);
            RecoverHPBase = float.Parse(text[index++]);
            DetailDescription = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRHero>();
        }
    }
}

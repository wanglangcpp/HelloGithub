using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// NPC配置表。
    /// </summary>
    public class DRNpc : IDataRow
    {
        /// <summary>
        /// NPC编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// NPC名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// NPC类型。
        /// </summary>
        public NpcCategory Category
        {
            get;
            private set;
        }

        /// <summary>
        /// NPC阵营。
        /// </summary>
        public CampType CampType
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
        /// 是否显示名字。
        /// </summary>
        public bool ShowName
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否持续显示血条。
        /// </summary>
        public bool ShowHPBar
        {
            get;
            private set;
        }

        /// <summary>
        /// 血条数量。
        /// </summary>
        public int HPBarCount
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
        /// 死亡后模型保持时间（秒）。
        /// </summary>
        public float KeepTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死持续时间（秒）。
        /// </summary>
        public float FakeKeepTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死回血时间（秒）。
        /// </summary>
        public float FakeRecoverTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡击飞概率。
        /// </summary>
        public float BlownAwayRatio
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡击飞时间。
        /// </summary>
        public float BlownAwayDuration
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡击飞最小距离。
        /// </summary>
        public float BlownAwayMinDistance
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡击飞最大距离。
        /// </summary>
        public float BlownAwayMaxDistance
        {
            get;
            private set;
        }

        /// <summary>
        /// 追击停止距离（米）。
        /// </summary>
        public float StoppingDistance
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
        /// AI行为。
        /// </summary>
        public string AIBehavior
        {
            get;
            private set;
        }

        /// <summary>
        /// 掉落规则编号。
        /// </summary>
        public int DropRuleId
        {
            get;
            private set;
        }

        /// <summary>
        /// 宝箱掉落编号。
        /// </summary>
        public int ChestId
        {
            get;
            private set;
        }

        /// <summary>
        /// NPC等级。
        /// </summary>
        public int Level
        {
            get;
            private set;
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
        /// 受击是否转向。
        /// </summary>
        public bool WillTurnOnHit
        {
            get;
            private set;
        }

        /// <summary>
        /// 受击转向冷却时间
        /// </summary>
        public float TurnOnHitCD
        {
            get;
            private set;
        }

        /// <summary>
        /// 死后下沉开始时间。
        /// </summary>
        public float DeathSinkStartTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡怪的沉降速度。
        /// </summary>
        public float DeathSinkSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡变色编号。
        /// </summary>
        public int DeathChangeColorId
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡特效。
        /// </summary>
        public string DeathEffect
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
        /// 出场时摄像机震动编号
        /// </summary>
        public int CameraShakingIndexOnEnter
        {
            get;
            private set;
        }

        /// <summary>
        /// 最低血量。
        /// </summary>
        public int MinHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否显示提示箭头。
        /// </summary>
        public bool ShowArrowPrompt
        {
            get;
            private set;
        }

        /// <summary>
        /// 金币掉落特效ID。
        /// </summary>
        public int DropCoinsEffect
        {
            get;
            private set;
        }

        /// <summary>
        /// 金币掉落数量。
        /// </summary>
        public int DropCoinsCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 出生动画。
        /// </summary>
        public string BornEnteringAnimationAlias
        {
            get;
            private set;
        }

        /// <summary>
        /// 出生ColorChanger。
        /// </summary>
        public int BornColorChangerId
        {
            get;
            private set;
        }

        /// <summary>
        /// 出生ColorChanger持续时间。
        /// </summary>
        public float BornColorChangerDuration
        {
            get;
            private set;
        }

        /// <summary>
        /// 出生特效。
        /// </summary>
        public string BornEffect
        {
            get;
            private set;
        }

        /// <summary>
        /// 出生特效延迟时间。
        /// </summary>
        public float BornEffectDelay
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否为假英雄。
        /// </summary>
        public bool IsFakeHero
        {
            get;
            private set;
        }

        /// <summary>
        /// 乱斗被杀时提供的经验值。
        /// </summary>
        public int MeleeExp
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
            Category = (NpcCategory)(int.Parse(text[index++]));
            CampType = (CampType)(int.Parse(text[index++]));
            CharacterId = int.Parse(text[index++]);
            Scale = float.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            ShowName = bool.Parse(text[index++]);
            ShowHPBar = bool.Parse(text[index++]);
            HPBarCount = int.Parse(text[index++]);
            Steady = float.Parse(text[index++]);
            SteadyRecoverSpeed = float.Parse(text[index++]);
            SteadyBuffId = int.Parse(text[index++]);
            KeepTime = float.Parse(text[index++]);
            FakeKeepTime = float.Parse(text[index++]);
            FakeRecoverTime = float.Parse(text[index++]);
            BlownAwayRatio = float.Parse(text[index++]);
            BlownAwayDuration = float.Parse(text[index++]);
            BlownAwayMinDistance = float.Parse(text[index++]);
            BlownAwayMaxDistance = float.Parse(text[index++]);
            StoppingDistance = float.Parse(text[index++]);
            AvoidancePriority = int.Parse(text[index++]);
            AIBehavior = text[index++];
            DropRuleId = int.Parse(text[index++]);
            ChestId = int.Parse(text[index++]);
            Level = int.Parse(text[index++]);
            Speed = float.Parse(text[index++]);
            DamageRandomRate = float.Parse(text[index++]);
            MaxHPFactor = float.Parse(text[index++]);
            PhysicalAttackFactor = float.Parse(text[index++]);
            PhysicalDefenseFactor = float.Parse(text[index++]);
            MagicAttackFactor = float.Parse(text[index++]);
            MagicDefenseFactor = float.Parse(text[index++]);
            WillTurnOnHit = bool.Parse(text[index++]);
            TurnOnHitCD = float.Parse(text[index++]);
            DeathSinkStartTime = float.Parse(text[index++]);
            DeathSinkSpeed = float.Parse(text[index++]);
            DeathChangeColorId = int.Parse(text[index++]);
            DeathEffect = text[index++];
            ElementId = int.Parse(text[index++]);
            CameraShakingIndexOnEnter = int.Parse(text[index++]);
            MinHP = int.Parse(text[index++]);
            ShowArrowPrompt = bool.Parse(text[index++]);
            DropCoinsEffect = int.Parse(text[index++]);
            DropCoinsCount = int.Parse(text[index++]);
            BornEnteringAnimationAlias = text[index++];
            BornColorChangerId = int.Parse(text[index++]);
            BornColorChangerDuration = float.Parse(text[index++]);
            BornEffect = text[index++];
            BornEffectDelay = float.Parse(text[index++]);
            IsFakeHero = bool.Parse(text[index++]);
            MeleeExp = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNpc>();
        }
    }
}

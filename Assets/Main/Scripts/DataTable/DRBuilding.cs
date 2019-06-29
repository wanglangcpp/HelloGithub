using GameFramework.DataTable;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 建筑物配置表。
    /// </summary>
    public class DRBuilding : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 名称。
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
        /// 阵营。
        /// </summary>
        public CampType CampType
        {
            get;
            private set;
        }

        /// <summary>
        /// 模型编号。
        /// </summary>
        public int BuildingModelId
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
        /// 是否显示名字。
        /// </summary>
        public bool ShowName
        {
            get;
            private set;
        }

        /// <summary>
        /// 血条显示规则。
        /// </summary>
        public HPBarDisplayRule HPBarDisplayRule
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
        /// 死亡后模型保持时间（秒）。
        /// </summary>
        public float KeepTime
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
        /// 克制属性编号。
        /// </summary>
        public int ElementId
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否显示伤害数字。
        /// </summary>
        public bool ShowHudText
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否接受攻击。
        /// </summary>
        public bool AcceptImpact
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否能被 AI 选作目标。
        /// </summary>
        public bool CanBeSelectedAsTargetByAI
        {
            get;
            private set;
        }

        /// <summary>
        /// 子弹发射器的角速度上限。
        /// </summary>
        public float ShooterMaxAngularSpeed
        {
            get;
            private set;
        }

        private BuildingDropInfo[] m_DropInfos = null;

        private const int DeathEffectCount = 2;

        /// <summary>
        /// 掉落金币时的特效编号。
        /// </summary>
        public int DropCoinsEffectId
        {
            get;
            private set;
        }

        /// <summary>1
        /// 获取掉落数据。
        /// </summary>
        /// <returns></returns>
        public IList<BuildingDropInfo> GetDropInfos()
        {
            return m_DropInfos;
        }

        private EffectInfo[] m_DeathEffectInfos = null;

        public IList<EffectInfo> GetDeathEffectInfos()
        {
            return m_DeathEffectInfos;
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
            BuildingModelId = int.Parse(text[index++]);
            Scale = float.Parse(text[index++]);
            ShowName = bool.Parse(text[index++]);
            HPBarDisplayRule = (HPBarDisplayRule)int.Parse(text[index++]);
            HPBarCount = int.Parse(text[index++]);
            KeepTime = float.Parse(text[index++]);
            AIBehavior = text[index++];
            DropRuleId = int.Parse(text[index++]);
            ChestId = int.Parse(text[index++]);
            Level = int.Parse(text[index++]);
            DamageRandomRate = float.Parse(text[index++]);
            MaxHPFactor = float.Parse(text[index++]);
            PhysicalAttackFactor = float.Parse(text[index++]);
            PhysicalDefenseFactor = float.Parse(text[index++]);
            MagicAttackFactor = float.Parse(text[index++]);
            MagicDefenseFactor = float.Parse(text[index++]);
            ElementId = int.Parse(text[index++]);
            ShowHudText = bool.Parse(text[index++]);
            AcceptImpact = bool.Parse(text[index++]);
            CanBeSelectedAsTargetByAI = bool.Parse(text[index++]);
            ShooterMaxAngularSpeed = float.Parse(text[index++]);
            ParseDropInfo(text, ref index);
            DropCoinsEffectId = int.Parse(text[index++]);
            ParseDeathEffects(text, ref index);
        }

        private void ParseDropInfo(string[] text, ref int index)
        {
            string originalText = text[index++];

            if (string.IsNullOrEmpty(originalText))
            {
                m_DropInfos = new BuildingDropInfo[0];
                return;
            }

            string[] items = originalText.Split(';');
            var dropInfos = new List<BuildingDropInfo>();
            for (int i = 0; i < items.Length; ++i)
            {
                var item = items[i];
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                var itemWithoutPrefix = item.Substring(1);
                var itemSplits = itemWithoutPrefix.Split(':');

                var dropInfo = new BuildingDropInfo();
                dropInfo.Weight = int.Parse(itemSplits[1]);

                switch (item[0])
                {
                    case 'b':
                        dropInfo.Type = BuildingDropType.Buff;
                        dropInfo.Params = new int[1];
                        dropInfo.Params[0] = int.Parse(itemSplits[0]);
                        break;
                    case 'c':
                        dropInfo.Type = BuildingDropType.Coin;
                        dropInfo.Params = new int[1];
                        dropInfo.Params[0] = int.Parse(itemSplits[0]);
                        break;
                    default:
                        throw new FormatException(string.Format("Drop info '{0}' has wrong format.", originalText));
                }

                dropInfos.Add(dropInfo);
                //GameFramework.Log.Info("DropInfo: type={0}, weight={1}, param={2}", dropInfo.Type, dropInfo.Weight, dropInfo.Params[0]);
            }

            m_DropInfos = dropInfos.ToArray();
        }

        private void ParseDeathEffects(string[] text, ref int index)
        {
            var deathEffectInfos = new List<EffectInfo>();
            for (int i = 0; i < DeathEffectCount; ++i)
            {
                var resourcePath = text[index++];
                var transformPath = text[index++];

                if (string.IsNullOrEmpty(resourcePath))
                {
                    continue;
                }

                deathEffectInfos.Add(new EffectInfo(resourcePath, transformPath));
            }

            m_DeathEffectInfos = deathEffectInfos.ToArray();
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBuilding>();
        }

        public class EffectInfo
        {
            public string ResourcePath { get; private set; }
            public string TargetTransformPath { get; private set; }

            public EffectInfo(string resourcePath, string targetTransformPath)
            {
                ResourcePath = resourcePath;
                TargetTransformPath = targetTransformPath;
            }
        }
    }
}

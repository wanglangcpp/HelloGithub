using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 建筑物动画配置表。
    /// </summary>
    public class DRBuildingAnimation : IDataRow
    {
        /// <summary>
        /// 建筑物编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 入场动画。
        /// </summary>
        public string Entering
        {
            get;
            private set;
        }

        /// <summary>
        /// 发射器入场动画。
        /// </summary>
        public string EnteringShooter
        {
            get;
            private set;
        }

        /// <summary>
        /// 默认动画。
        /// </summary>
        public string Default
        {
            get;
            private set;
        }

        /// <summary>
        /// 发射器默认动画。
        /// </summary>
        public string DefaultShooter
        {
            get;
            private set;
        }

        /// <summary>
        /// 将死动画。
        /// </summary>
        public string Dying
        {
            get;
            private set;
        }

        /// <summary>
        /// 发射器将死动画。
        /// </summary>
        public string DyingShooter
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡动画。
        /// </summary>
        public string Death
        {
            get;
            private set;
        }

        /// <summary>
        /// 发射器死亡动画。
        /// </summary>
        public string DeathShooter
        {
            get;
            private set;
        }

        #region Skill animation names

        public string Skill1 { get; private set; }
        public string Skill2 { get; private set; }
        public string Skill3 { get; private set; }
        public string Skill4 { get; private set; }
        public string Skill5 { get; private set; }


        public string Skill1Shooter { get; private set; }
        public string Skill2Shooter { get; private set; }
        public string Skill3Shooter { get; private set; }
        public string Skill4Shooter { get; private set; }
        public string Skill5Shooter { get; private set; }

        #endregion Skill animation names

        public string GetAnimationName(string animationAliasName)
        {
            switch (animationAliasName)
            {
                case "Entering":
                    return Entering;
                case "Default":
                    return Default;
                case "Dying":
                    return Dying;
                case "Death":
                    return Death;
                case "Skill1":
                    return Skill1;
                case "Skill2":
                    return Skill2;
                case "Skill3":
                    return Skill3;
                case "Skill4":
                    return Skill4;
                case "Skill5":
                    return Skill5;
                default:
                    return null;
            }
        }

        public string GetShooterAnimationName(string animationAliasName)
        {
            switch (animationAliasName)
            {
                case "Entering":
                    return EnteringShooter;
                case "Default":
                    return DefaultShooter;
                case "Dying":
                    return DyingShooter;
                case "Death":
                    return DeathShooter;
                case "Skill1":
                    return Skill1Shooter;
                case "Skill2":
                    return Skill2Shooter;
                case "Skill3":
                    return Skill3Shooter;
                case "Skill4":
                    return Skill4Shooter;
                case "Skill5":
                    return Skill5Shooter;
                default:
                    return null;
            }
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;

            // 本体动画。
            Entering = text[index++];
            Default = text[index++];
            Dying = text[index++];
            Death = text[index++];
            Skill1 = text[index++];
            Skill2 = text[index++];
            Skill3 = text[index++];
            Skill4 = text[index++];
            Skill5 = text[index++];

            // 子弹发射器动画。
            EnteringShooter = text[index++];
            DefaultShooter = text[index++];
            DyingShooter = text[index++];
            DeathShooter = text[index++];
            Skill1Shooter = text[index++];
            Skill2Shooter = text[index++];
            Skill3Shooter = text[index++];
            Skill4Shooter = text[index++];
            Skill5Shooter = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBuildingAnimation>();
        }
    }
}

using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色动作表。
    /// </summary>
    public class DRAnimation : IDataRow
    {
        /// <summary>
        /// 模型编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 休闲动作名称。
        /// </summary>
        public string Idle
        {
            get;
            private set;
        }

        /// <summary>
        /// 站立动作名称。
        /// </summary>
        public string Stand
        {
            get;
            private set;
        }

        /// <summary>
        /// 跑步动作名称。
        /// </summary>
        public string Run
        {
            get;
            private set;
        }

        /// <summary>
        /// 受击动作名称。
        /// </summary>
        public string Hit
        {
            get;
            private set;
        }

        /// <summary>
        /// 重受击动作名称。
        /// </summary>
        public string HitStrong
        {
            get;
            private set;
        }

        /// <summary>
        /// 浮空受击动作名称。
        /// </summary>
        public string HitAir
        {
            get;
            private set;
        }

        /// <summary>
        /// 浮空动作名称。
        /// </summary>
        public string Float
        {
            get;
            private set;
        }

        /// <summary>
        /// 下落动作名称。
        /// </summary>
        public string Falling
        {
            get;
            private set;
        }

        /// <summary>
        /// 倒地动作名称。
        /// </summary>
        public string FallDown
        {
            get;
            private set;
        }

        /// <summary>
        /// 起身动作名称。
        /// </summary>
        public string StandUp
        {
            get;
            private set;
        }

        /// <summary>
        /// 眩晕动作名称。
        /// </summary>
        public string Stun
        {
            get;
            private set;
        }

        /// <summary>
        /// 眩晕动作名称。
        /// </summary>
        public string Rotate
        {
            get;
            private set;
        }

        /// <summary>
        /// 击飞动作名称。
        /// </summary>
        public string BlownAway
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡动作名称。
        /// </summary>
        public string Dying
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡后动作名称。
        /// </summary>
        public string Dead
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死动作名称。
        /// </summary>
        public string FakeDying
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死后动作名称。
        /// </summary>
        public string FakeDead
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死后起身动作名称。
        /// </summary>
        public string StandUpAfterFakeDeath
        {
            get;
            private set;
        }

        /// <summary>
        /// 出场动作名称1。
        /// </summary>
        public string Entering1
        {
            get;
            private set;
        }

        /// <summary>
        /// 出场动作名称2。
        /// </summary>
        public string Entering2
        {
            get;
            private set;
        }

        /// <summary>
        /// 距离辅助攻击动作名称。
        /// </summary>
        public string DistanceAssist
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示模型的出场动作名称。
        /// </summary>
        public string ModelShowDebut
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示模型的站立动作名称。
        /// </summary>
        public string ModelShowStand
        {
            get;
            private set;
        }

        /// <summary>
        /// 与展示模型互动的动作名称一。
        /// </summary>
        public string ModelInteraction1
        {
            get;
            private set;
        }

        /// <summary>
        /// 与展示模型互动的动作名称二。
        /// </summary>
        public string ModelInteraction2
        {
            get;
            private set;
        }

        /// <summary>
        /// 与展示模型互动的动作名称三。
        /// </summary>
        public string ModelInteraction3
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第一段动作名称。
        /// </summary>
        public string Attack1
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第二段动作名称。
        /// </summary>
        public string Attack2
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第三段动作名称。
        /// </summary>
        public string Attack3
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第四段动作名称。
        /// </summary>
        public string Attack4
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第五段动作名称。
        /// </summary>
        public string Attack5
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能一动作名称。
        /// </summary>
        public string Skill1
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能二动作名称。
        /// </summary>
        public string Skill2
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能三动作名称。
        /// </summary>
        public string Skill3
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能四动作名称。
        /// </summary>
        public string Skill4
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能五动作名称。
        /// </summary>
        public string Skill5
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能六动作名称。
        /// </summary>
        public string Skill6
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能七动作名称。
        /// </summary>
        public string Skill7
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能八动作名称。
        /// </summary>
        public string Skill8
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能九动作名称。
        /// </summary>
        public string Skill9
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十动作名称。
        /// </summary>
        public string Skill10
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十一动作名称。
        /// </summary>
        public string Skill11
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十二动作名称。
        /// </summary>
        public string Skill12
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十三动作名称。
        /// </summary>
        public string Skill13
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十四动作名称。
        /// </summary>
        public string Skill14
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十五动作名称。
        /// </summary>
        public string Skill15
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十六动作名称。
        /// </summary>
        public string Skill16
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十七动作名称。
        /// </summary>
        public string Skill17
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十八动作名称。
        /// </summary>
        public string Skill18
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十九动作名称。
        /// </summary>
        public string Skill19
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能二十动作名称。
        /// </summary>
        public string Skill20
        {
            get;
            private set;
        }

        /// <summary>
        /// 左徘徊动画名称。
        /// </summary>
        public string LingeringLeft
        {
            get;
            private set;
        }

        /// <summary>
        /// 右徘徊动画名称。
        /// </summary>
        public string LingeringRight
        {
            get;
            private set;
        }

        /// <summary>
        /// 空中强力打击重击弹地。
        /// </summary>
        public string HitAirStrong
        {
            get;
            private set;
        }

        /// <summary>
        /// 击飞第二段。
        /// </summary>
        public string BlownAwayFalling
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示获取模型的出场动作名称。
        /// </summary>
        public string ReceiveModelShowDebut
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示获取模型的站立动作名称。
        /// </summary>
        public string ReceiveModelShowStand
        {
            get;
            private set;
        }

        public string GetAnimationName(string animationAliasName)
        {
            // 为了性能，先不用反射。
            switch (animationAliasName)
            {
                case "Idle":
                    return Idle;
                case "Stand":
                    return Stand;
                case "Run":
                    return Run;
                case "Hit":
                    return Hit;
                case "HitStrong":
                    return HitStrong;
                case "HitAir":
                    return HitAir;
                case "Float":
                    return Float;
                case "Falling":
                    return Falling;
                case "FallDown":
                    return FallDown;
                case "StandUp":
                    return StandUp;
                case "Stun":
                    return Stun;
                case "Rotate":
                    return Rotate;
                case "BlownAway":
                    return BlownAway;
                case "Dying":
                    return Dying;
                case "Dead":
                    return Dead;
                case "FakeDying":
                    return FakeDying;
                case "FakeDead":
                    return FakeDead;
                case "StandUpAfterFakeDeath":
                    return StandUpAfterFakeDeath;
                case "Entering1":
                    return Entering1;
                case "Entering2":
                    return Entering2;
                case "DistanceAssist":
                    return DistanceAssist;
                case "ModelShowDebut":
                    return ModelShowDebut;
                case "ModelShowStand":
                    return ModelShowStand;
                case "ModelInteraction1":
                    return ModelInteraction1;
                case "ModelInteraction2":
                    return ModelInteraction2;
                case "ModelInteraction3":
                    return ModelInteraction3;
                case "Attack1":
                    return Attack1;
                case "Attack2":
                    return Attack2;
                case "Attack3":
                    return Attack3;
                case "Attack4":
                    return Attack4;
                case "Attack5":
                    return Attack5;
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
                case "Skill6":
                    return Skill6;
                case "Skill7":
                    return Skill7;
                case "Skill8":
                    return Skill8;
                case "Skill9":
                    return Skill9;
                case "Skill10":
                    return Skill10;
                case "Skill11":
                    return Skill11;
                case "Skill12":
                    return Skill12;
                case "Skill13":
                    return Skill13;
                case "Skill14":
                    return Skill14;
                case "Skill15":
                    return Skill15;
                case "Skill16":
                    return Skill16;
                case "Skill17":
                    return Skill17;
                case "Skill18":
                    return Skill18;
                case "Skill19":
                    return Skill19;
                case "Skill20":
                    return Skill20;
                case "LingeringLeft":
                    return LingeringLeft;
                case "LingeringRight":
                    return LingeringRight;
                case "HitAirStrong":
                    return HitAirStrong;
                case "BlownAwayFalling":
                    return BlownAwayFalling;
                case "ReceiveModelShowDebut":
                    return ReceiveModelShowDebut;
                case "ReceiveModelShowStand":
                    return ReceiveModelShowStand;
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
            Idle = text[index++];
            Stand = text[index++];
            Run = text[index++];
            Hit = text[index++];
            HitStrong = text[index++];
            HitAir = text[index++];
            HitAirStrong = text[index++];
            Float = text[index++];
            Falling = text[index++];
            FallDown = text[index++];
            StandUp = text[index++];
            Stun = text[index++];
            Rotate = text[index++];
            BlownAway = text[index++];
            BlownAwayFalling = text[index++];
            Dying = text[index++];
            Dead = text[index++];
            FakeDying = text[index++];
            FakeDead = text[index++];
            StandUpAfterFakeDeath = text[index++];
            Entering1 = text[index++];
            Entering2 = text[index++];
            DistanceAssist = text[index++];
            ModelShowDebut = text[index++];
            ModelShowStand = text[index++];
            ModelInteraction1 = text[index++];
            ModelInteraction2 = text[index++];
            ModelInteraction3 = text[index++];
            Attack1 = text[index++];
            Attack2 = text[index++];
            Attack3 = text[index++];
            Attack4 = text[index++];
            Attack5 = text[index++];
            Skill1 = text[index++];
            Skill2 = text[index++];
            Skill3 = text[index++];
            Skill4 = text[index++];
            Skill5 = text[index++];
            Skill6 = text[index++];
            Skill7 = text[index++];
            Skill8 = text[index++];
            Skill9 = text[index++];
            Skill10 = text[index++];
            Skill11 = text[index++];
            Skill12 = text[index++];
            Skill13 = text[index++];
            Skill14 = text[index++];
            Skill15 = text[index++];
            Skill16 = text[index++];
            Skill17 = text[index++];
            Skill18 = text[index++];
            Skill19 = text[index++];
            Skill20 = text[index++];
            LingeringLeft = text[index++];
            LingeringRight = text[index++];
            ReceiveModelShowDebut = text[index++];
            ReceiveModelShowStand = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAnimation>();
        }
    }
}

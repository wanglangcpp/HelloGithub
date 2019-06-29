using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色动画的CrossFade融合时间表。
    /// </summary>
    public class DRAnimationCrossFade : IDataRow
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
        /// 休闲动作融合时间。
        /// </summary>
        public float Idle
        {
            get;
            private set;
        }

        /// <summary>
        /// 站立动作融合时间。
        /// </summary>
        public float Stand
        {
            get;
            private set;
        }

        /// <summary>
        /// 跑步动作融合时间。
        /// </summary>
        public float Run
        {
            get;
            private set;
        }

        /// <summary>
        /// 受击动作融合时间。
        /// </summary>
        public float Hit
        {
            get;
            private set;
        }

        /// <summary>
        /// 重受击动作融合时间。
        /// </summary>
        public float HitStrong
        {
            get;
            private set;
        }

        /// <summary>
        /// 浮空受击动作融合时间。
        /// </summary>
        public float HitAir
        {
            get;
            private set;
        }

        /// <summary>
        /// 浮空动作融合时间。
        /// </summary>
        public float Float
        {
            get;
            private set;
        }

        /// <summary>
        /// 下落动作融合时间。
        /// </summary>
        public float Falling
        {
            get;
            private set;
        }

        /// <summary>
        /// 倒地动作融合时间。
        /// </summary>
        public float FallDown
        {
            get;
            private set;
        }

        /// <summary>
        /// 起身动作融合时间。
        /// </summary>
        public float StandUp
        {
            get;
            private set;
        }

        /// <summary>
        /// 眩晕动作融合时间。
        /// </summary>
        public float Stun
        {
            get;
            private set;
        }

        /// <summary>
        /// 眩晕动作融合时间。
        /// </summary>
        public float Rotate
        {
            get;
            private set;
        }

        /// <summary>
        /// 击飞动作融合时间。
        /// </summary>
        public float BlownAway
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡动作融合时间。
        /// </summary>
        public float Dying
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡后动作融合时间。
        /// </summary>
        public float Dead
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死动作融合时间。
        /// </summary>
        public float FakeDying
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死后动作融合时间。
        /// </summary>
        public float FakeDead
        {
            get;
            private set;
        }

        /// <summary>
        /// 假死后起身动作融合时间。
        /// </summary>
        public float StandUpAfterFakeDeath
        {
            get;
            private set;
        }

        /// <summary>
        /// 出场动作融合时间。
        /// </summary>
        public float Entering
        {
            get;
            private set;
        }

        /// <summary>
        /// 距离辅助攻击动作融合时间。
        /// </summary>
        public float DistanceAssist
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示模型的出场动作融合时间。
        /// </summary>
        public float ModelShowDebut
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示模型的站立动作融合时间。
        /// </summary>
        public float ModelShowStand
        {
            get;
            private set;
        }

        /// <summary>
        /// 与展示模型互动的动作融合时间一。
        /// </summary>
        public float ModelInteraction1
        {
            get;
            private set;
        }

        /// <summary>
        /// 与展示模型互动的动作融合时间二。
        /// </summary>
        public float ModelInteraction2
        {
            get;
            private set;
        }

        /// <summary>
        /// 与展示模型互动的动作融合时间三。
        /// </summary>
        public float ModelInteraction3
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第一段动作融合时间。
        /// </summary>
        public float Attack1
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第二段动作融合时间。
        /// </summary>
        public float Attack2
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第三段动作融合时间。
        /// </summary>
        public float Attack3
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第四段动作融合时间。
        /// </summary>
        public float Attack4
        {
            get;
            private set;
        }

        /// <summary>
        /// 连续攻击第五段动作融合时间。
        /// </summary>
        public float Attack5
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能一动作融合时间。
        /// </summary>
        public float Skill1
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能二动作融合时间。
        /// </summary>
        public float Skill2
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能三动作融合时间。
        /// </summary>
        public float Skill3
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能四动作融合时间。
        /// </summary>
        public float Skill4
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能五动作融合时间。
        /// </summary>
        public float Skill5
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能六动作融合时间。
        /// </summary>
        public float Skill6
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能七动作融合时间。
        /// </summary>
        public float Skill7
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能八动作融合时间。
        /// </summary>
        public float Skill8
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能九动作融合时间。
        /// </summary>
        public float Skill9
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十动作融合时间。
        /// </summary>
        public float Skill10
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十一动作融合时间。
        /// </summary>
        public float Skill11
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十二动作融合时间。
        /// </summary>
        public float Skill12
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十三动作融合时间。
        /// </summary>
        public float Skill13
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十四动作融合时间。
        /// </summary>
        public float Skill14
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十五动作融合时间。
        /// </summary>
        public float Skill15
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十六动作融合时间。
        /// </summary>
        public float Skill16
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十七动作融合时间。
        /// </summary>
        public float Skill17
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十八动作融合时间。
        /// </summary>
        public float Skill18
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能十九动作融合时间。
        /// </summary>
        public float Skill19
        {
            get;
            private set;
        }

        /// <summary>
        /// 固定技能二十动作融合时间。
        /// </summary>
        public float Skill20
        {
            get;
            private set;
        }

        /// <summary>
        /// 左徘徊动画融合时间。
        /// </summary>
        public float LingeringLeft
        {
            get;
            private set;
        }

        /// <summary>
        /// 右徘徊动画融合时间。
        /// </summary>
        public float LingeringRight
        {
            get;
            private set;
        }

        /// <summary>
        /// 空中强力打击重击弹地融合时间。
        /// </summary>
        public float HitAirStrong
        {
            get;
            private set;
        }

        /// <summary>
        /// 击飞第二段融合时间。
        /// </summary>
        public float BlownAwayFalling
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示获取模型的出场动作融合时间。
        /// </summary>
        public float ReceiveModelShowDebut
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示获取模型的站立动作融合时间。
        /// </summary>
        public float ReceiveModelShowStand
        {
            get;
            private set;
        }

        public float GetAnimationCrossFade(string animationAliasName)
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
                case "Entering":
                    return Entering;
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
                    return 0.0f;
            }
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Idle = float.Parse(text[index++]);
            Stand = float.Parse(text[index++]);
            Run = float.Parse(text[index++]);
            Hit = float.Parse(text[index++]);
            HitStrong = float.Parse(text[index++]);
            HitAir = float.Parse(text[index++]);
            HitAirStrong = float.Parse(text[index++]);
            Float = float.Parse(text[index++]);
            Falling = float.Parse(text[index++]);
            FallDown = float.Parse(text[index++]);
            StandUp = float.Parse(text[index++]);
            Stun = float.Parse(text[index++]);
            Rotate = float.Parse(text[index++]);
            BlownAway = float.Parse(text[index++]);
            BlownAwayFalling = float.Parse(text[index++]);
            Dying = float.Parse(text[index++]);
            Dead = float.Parse(text[index++]);
            FakeDying = float.Parse(text[index++]);
            FakeDead = float.Parse(text[index++]);
            StandUpAfterFakeDeath = float.Parse(text[index++]);
            Entering = float.Parse(text[index++]);
            DistanceAssist = float.Parse(text[index++]);
            ModelShowDebut = float.Parse(text[index++]);
            ModelShowStand = float.Parse(text[index++]);
            ModelInteraction1 = float.Parse(text[index++]);
            ModelInteraction2 = float.Parse(text[index++]);
            ModelInteraction3 = float.Parse(text[index++]);
            Attack1 = float.Parse(text[index++]);
            Attack2 = float.Parse(text[index++]);
            Attack3 = float.Parse(text[index++]);
            Attack4 = float.Parse(text[index++]);
            Attack5 = float.Parse(text[index++]);
            Skill1 = float.Parse(text[index++]);
            Skill2 = float.Parse(text[index++]);
            Skill3 = float.Parse(text[index++]);
            Skill4 = float.Parse(text[index++]);
            Skill5 = float.Parse(text[index++]);
            Skill6 = float.Parse(text[index++]);
            Skill7 = float.Parse(text[index++]);
            Skill8 = float.Parse(text[index++]);
            Skill9 = float.Parse(text[index++]);
            Skill10 = float.Parse(text[index++]);
            Skill11 = float.Parse(text[index++]);
            Skill12 = float.Parse(text[index++]);
            Skill13 = float.Parse(text[index++]);
            Skill14 = float.Parse(text[index++]);
            Skill15 = float.Parse(text[index++]);
            Skill16 = float.Parse(text[index++]);
            Skill17 = float.Parse(text[index++]);
            Skill18 = float.Parse(text[index++]);
            Skill19 = float.Parse(text[index++]);
            Skill20 = float.Parse(text[index++]);
            LingeringLeft = float.Parse(text[index++]);
            LingeringRight = float.Parse(text[index++]);
            ReceiveModelShowDebut = float.Parse(text[index++]);
            ReceiveModelShowStand = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAnimationCrossFade>();
        }
    }
}

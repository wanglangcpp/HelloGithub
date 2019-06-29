using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 教程配置数据。
    /// </summary>
    public class TutorialConfig : ScriptableObject
    {
        [SerializeField]
        private DemoHero[] m_DemoHeroes = null;

        [SerializeField]
        private int m_DemoInstanceId = 1;

        /// <summary>
        /// 获取展示副本编号。
        /// </summary>
        public int DemoInstanceId { get { return m_DemoInstanceId; } }

        [SerializeField]
        private Rect m_JoystickTipRect = new Rect(150f, 100f, 220f, 220f);

        public Rect JoystickTipRect { get { return m_JoystickTipRect; } }

        [SerializeField]
        private int m_DefaultDepthAllowance = 1;

        /// <summary>
        /// 获取 UI 深度裕量默认值。
        /// </summary>
        public int DefaultDepthAllowance { get { return m_DefaultDepthAllowance; } }

        /// <summary>
        /// 获取展示副本使用的英雄数据。
        /// </summary>
        /// <returns>展示副本使用的英雄数据。</returns>
        public IList<DemoHero> GetDemoHeroes()
        {
            return m_DemoHeroes;
        }

        [Serializable]
        public class DemoHero
        {
            [SerializeField]
            private int m_Type = 0;

            public int Type { get { return m_Type; } }

            [SerializeField]
            private int m_Level = 1;

            [SerializeField]
            private int m_MaxHP = 100;

            [SerializeField]
            private int m_HP = 100;

            [SerializeField]
            private int m_PhysicalAttack = 1;

            [SerializeField]
            private int m_PhysicalDefense = 1;

            [SerializeField]
            private float m_CriticalHitProb = 0;

            [SerializeField]
            private float m_CriticalHitRate = 0;

            [SerializeField]
            private int m_Speed = 5;

            public int Level { get { return m_Level; } }

            public int MaxHP { get { return m_MaxHP; } }

            public int Hp { get { return m_HP; } }

            public int PhysicalAttack { get { return m_PhysicalAttack; } }

            public int PhysicalDefense { get { return m_PhysicalDefense; } }

            public float CriticalHitProb { get { return m_CriticalHitProb; } }

            public float CriticalHitRate { get { return m_CriticalHitRate; } }

            public int Speed { get { return m_Speed; } }

            public static explicit operator LobbyHeroData(DemoHero demoHero)
            {
                LobbyHeroData ret = new LobbyHeroData();

                ret.SkillLevels.AddRange(Constant.DefaultHeroSkillLevels);

                var pb = new PBLobbyHeroInfo
                {
                    Type = demoHero.Type,
                    ConsciousnessLevel = 1,
                    ElevationLevel = 1,
                    Exp = 0,
                    Level = demoHero.Level,
                    StarLevel = 1,
                };

                pb.HasSkillBadgesInfos = true;
                for (int i = 0; i < ret.SkillLevels.Count; i++)
                {
                    var badges = new PBHeroSkillBadgesInfo();
                    badges.SpecificBadgeId = -1;
                    badges.HasGenericBadgeIds = true;

                    for (int j = 0; j < Constant.Hero.MaxBadgeSlotCountPerSkill; j++)
                    {
                        badges.GenericBadgeIds.Add(-1);
                    }
                    pb.SkillBadgesInfos.Add(badges);
                }

                ret.UpdateData(pb);
                return ret;
            }
        }
    }
}

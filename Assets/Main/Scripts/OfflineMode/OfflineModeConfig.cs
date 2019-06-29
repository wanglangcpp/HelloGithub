using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class OfflineModeConfig : ScriptableObject
    {
        [SerializeField]
        private int m_OfflineSceneId = 1;

        [SerializeField]
        private string m_PlayerName = "Offline Mode";

        [SerializeField]
        private int m_PlayerLevel = 1;

        [SerializeField]
        private int m_HeroLevel = 1;

        [SerializeField]
        private int m_HeroStarLevel = 1;

        [SerializeField]
        private int m_HeroConsciousnessLevel = 1;

        [SerializeField]
        private int m_HeroElevationLevel = 1;

        [SerializeField]
        private List<int> m_SkillLevels = null;

        [SerializeField]
        private List<int> m_BattleHeroTeamIds = null;

        [SerializeField]
        private List<int> m_PreObtainedHeroTypeIds = null;

        [SerializeField]
        private List<ItemData> m_ItemDatas = null;

        [SerializeField]
        private List<NewGearData> m_NewGearDatas = null;

        [SerializeField]
        private HeroAttributes m_HeroAttributes = null;

        /// <summary>
        /// 获取或设置离线模式使用的场景。
        /// </summary>
        public int OfflineSceneId
        {
            get
            {
                return m_OfflineSceneId;
            }
            set
            {
                m_OfflineSceneId = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }
        }

        public int PlayerLevel
        {
            get
            {
                return m_PlayerLevel;
            }
        }

        public int HeroLevel
        {
            get
            {
                return m_HeroLevel;
            }
        }

        public int HeroStarLevel
        {
            get
            {
                return m_HeroStarLevel;
            }
        }

        public int HeroConsciousnessLevel
        {
            get
            {
                return m_HeroConsciousnessLevel;
            }
        }

        public int HeroElevationLevel
        {
            get
            {
                return m_HeroElevationLevel;
            }
        }

        public IList<int> SkillLevels
        {
            get
            {
                return m_SkillLevels;
            }
        }

        public IList<int> BattleHeroIds
        {
            get
            {
                return m_BattleHeroTeamIds;
            }
        }

        public IList<int> PreObtainedHeroTypeIds
        {
            get
            {
                return m_PreObtainedHeroTypeIds;
            }
        }

        public IList<ItemData> ItemDatas
        {
            get
            {
                return m_ItemDatas;
            }
        }


        public IList<NewGearData> NewGearDatas
        {
            get
            {
                return m_NewGearDatas;
            }
        }

        public HeroAttributes DefaultHeroAttributes
        {
            get
            {
                return m_HeroAttributes;
            }
        }

        [Serializable]
        public class ItemData
        {
            public int Type;
            public int Count;
        }

        [Serializable]
        public class HeroAttributes
        {
            public int MaxHP = 1000;
            public float Speed = 5f;
            public int PhysicalAttack = 100;
            public float CriticalHitProb = 0f;
            public float CriticalHitRate = 0f;
            public float PhysicalAtkReflectRate = 0f;
            public int PhysicalDefense = 0;
            public float AntiCriticalHitProb = 0f;
            public float DamageReductionRate = 0f;
            public float RecoverHP = 0f;

            public int Might = 1000;

            public static implicit operator PBLobbyHeroInfo(HeroAttributes heroAttributes)
            {
                return new PBLobbyHeroInfo
                {
                    HeroAttribute = new PBHeroAttribute
                    {
                        AntiCriticalHitProb = heroAttributes.AntiCriticalHitProb,
                        Attack = heroAttributes.PhysicalAttack,
                        AttackReflectRate = heroAttributes.PhysicalAtkReflectRate,
                        CriticalHitProb = heroAttributes.CriticalHitProb,
                        CriticalHitRate = heroAttributes.CriticalHitRate,
                        DamageReductionRate = heroAttributes.DamageReductionRate,
                        Defense = heroAttributes.PhysicalDefense,
                        MaxHP = heroAttributes.MaxHP,
                        RecoverHP = heroAttributes.RecoverHP,
                        Speed = heroAttributes.Speed,
                    },

                    Might = heroAttributes.Might,
                };
            }
        }
    }
}

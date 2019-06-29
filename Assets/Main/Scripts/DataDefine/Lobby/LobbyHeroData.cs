using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class LobbyHeroData : BaseLobbyHeroData, IGenericData<LobbyHeroData, PBLobbyHeroInfo>
    {
        [SerializeField]
        private NewGearsData m_NewGears = new NewGearsData();

        [SerializeField]
        private float m_AngerIncreaseRate;

        private bool m_CanStarLevelUp;

        public NewGearsData NewGears { get { return m_NewGears; } }

        public int Key
        {
            get
            {
                return Type;
            }
        }

        [SerializeField]
        private bool[] m_QualityItemSlotStates = null;

        public IList<bool> GetQualityItemSlotStates()
        {
            return m_QualityItemSlotStates;
        }

        #region Basic attributes

        /// <summary>
        /// 英雄切换后的冷却时间缩减百分比。
        /// </summary>
        public override float ReducedHeroSwitchCD
        {
            get
            {
                var ret = base.ReducedHeroSwitchCD;
                return Mathf.Clamp(ret, 0f, Constant.Hero.HeroSwitchCoolDownRateMaxVal);
            }
        }

        /// <summary>
        /// 怒气上涨速度，单位是每秒上涨百分比
        /// </summary>
        public float AngerIncreaseRate
        {
            get
            {
                m_AngerIncreaseRate = 0f;
                if (HeroDataRow != null)
                {
                    m_AngerIncreaseRate = HeroDataRow.AngerIncreaseRate;
                }

                for (int i = 0; i < GameEntry.Data.EpigraphSlots.Data.Count; ++i)
                {
                    m_AngerIncreaseRate += GameEntry.Data.EpigraphSlots.Data[i].AngerIncreaseRate;
                }

                return m_AngerIncreaseRate;
            }
        }

        #endregion Basic attributes

        /// <summary>
        /// 升阶所需道具（碎片）数量。
        /// </summary>
        public int StarLevelUpItemCount
        {
            get
            {
                return HeroDataRow.GetStarLevelUpItemCount(StarLevel);
            }
        }

        public bool CanStarLevelUp {
            get { return m_CanStarLevelUp; }
            set { m_CanStarLevelUp = value; }
        }

        /// <summary>
        /// 获取当前品阶下的QualityItem属性值。
        /// </summary>
        /// <param name="attrType">属性类型。</param>
        /// <returns>属性值。</returns>
        public float GetQualityItemAttribute(AttributeType attrType)
        {
            float ret = 0;
            var dtQualityItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
            for (int i = 0; m_HeroQualityLevelRow != null && m_QualityItemSlotStates != null && i < m_QualityItemSlotStates.Length; i++)
            {
                if (!m_QualityItemSlotStates[i])
                {
                    continue;
                }
                int item = m_HeroQualityLevelRow.GetQualityItemIds()[i];
                var itemData = dtQualityItem.GetDataRow(item);
                if (itemData == null)
                {
                    continue;
                }
                var attrs = itemData.GetAttrIds();
                for (int j = 0; j < attrs.Count; j++)
                {
                    if (attrs[j] == (int)attrType)
                    {
                        ret += itemData.GetAttrVals()[j];
                    }
                }
            }
            return ret;
        }

        public void UpdateData(PBLobbyHeroInfo data)
        {
            if (data.Type <= 0)
            {
                Log.Warning("Hero type is invalid.");
            }

            Type = data.Type;

            if (data.HasLevel)
            {
                Level = data.Level;
            }

            if (data.HasExp)
            {
                Exp = data.Exp;
            }

            if (data.HasStarLevel)
            {
                StarLevel = data.StarLevel;
            }

            if (data.SkillLevels.Count > 0)
            {
                SkillLevels.Clear();
                for (int i = 0; i < data.SkillLevels.Count; i++)
                {
                    SkillLevels.Add(data.SkillLevels[i]);
                }
            }

            if (data.SkillExps.Count > 0)
            {
                SkillExps.Clear();
                for (int i = 0; i < data.SkillExps.Count; i++)
                {
                    SkillExps.Add(data.SkillExps[i]);
                }
            }

            if (data.NewGearInfos.Count > 0)
            {
                m_NewGears.ClearAndAddData(data.NewGearInfos);
            }

            if (data.HasMight)
            {
                Might = data.Might;
            }

            if (data.HasTotalQualityLevel)
            {
                TotalQualityLevel = data.TotalQualityLevel;
            }

            if (data.QualityItemSlots.Count > 0)
            {
                m_QualityItemSlotStates = data.QualityItemSlots.ToArray();
            }

            if (data.HasSkillBadgesInfos)
            {
                m_SkillBadges.Clear();
                for (int i = 0; i < data.SkillBadgesInfos.Count; i++)
                {
                    SkillBadgesData badgesData = new SkillBadgesData();
                    badgesData.UpdateData(data.SkillBadgesInfos[i]);
                    m_SkillBadges.Add(badgesData);
                }
            }

            var heroAttributes = data.HeroAttribute;
            if (heroAttributes != null)
            {
                UpdateAttributes(heroAttributes);
            }
        }

        private void UpdateAttributes(PBHeroAttribute heroAttributes)
        {
            MaxHP = heroAttributes.MaxHP;
            PhysicalAttack = heroAttributes.Attack;
            PhysicalDefense = heroAttributes.Defense;
            CriticalHitProb = heroAttributes.CriticalHitProb;
            CriticalHitRate = heroAttributes.CriticalHitRate;
            PhysicalAtkReflectRate = heroAttributes.AttackReflectRate;
            DamageReductionRate = heroAttributes.DamageReductionRate;
            AntiCriticalHitProb = heroAttributes.AntiCriticalHitProb;
            RecoverHP = heroAttributes.RecoverHP;
            Speed = heroAttributes.Speed;
        }
    }
}

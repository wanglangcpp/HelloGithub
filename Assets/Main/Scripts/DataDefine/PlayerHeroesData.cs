using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 参加副本的一个玩家带领的英雄数据
    /// </summary>
    [Serializable]
    public class PlayerHeroesData
    {
        [SerializeField]
        private List<HeroData> m_Heroes = new List<HeroData>();

        private List<HPRecoverer> m_HPRecoverers = new List<HPRecoverer>();

        public HeroData CurrentHeroData { get; private set; }
        public int CurrentHeroIndex { get; private set; }

        public HeroData LastHeroData { get; private set; }
        public int LastHeroIndex { get; private set; }

        public PlayerHeroesData()
        {
            CurrentHeroData = null;
            CurrentHeroIndex = -1;
            LastHeroData = null;
            LastHeroIndex = -1;
        }

        public HeroData GetHero(int index)
        {
            return m_Heroes[index];
        }

        public HeroData[] GetHeroes()
        {
            return m_Heroes.ToArray();
        }

        public void Add(HeroData hero)
        {
            m_Heroes.Add(hero);
            m_HPRecoverers.Add(new HPRecoverer(hero));
        }

        public bool AnyIsAlive
        {
            get
            {
                for (int i = 0; i < m_Heroes.Count; ++i)
                {
                    var hero = m_Heroes[i];
                    if (!hero.IsDead)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public bool IsBelongToPlayer(int entityId)
        {
            foreach (var hero in m_Heroes)
            {
                if (hero.Id==entityId)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 切换玩家当前使用的英雄。此处仅判定英雄序号是否合法。
        /// </summary>
        /// <param name="index">要切换到的英雄的序号</param>
        public void SwitchHero(int index)
        {
            if (index < 0 || index >= m_Heroes.Count)
            {
                Log.Warning("We have {0} hero(es). Index {1} is invalid.", m_Heroes.Count, index);
                return;
            }

            LastHeroIndex = CurrentHeroIndex;
            LastHeroData = CurrentHeroData;

            CurrentHeroIndex = index;
            CurrentHeroData = m_Heroes[index];
            m_HPRecoverers[index].Reset();
        }

        /// <summary>
        /// 更新被换下的英雄的冷却时间。
        /// </summary>
        /// <param name="elapseSeconds">流逝的秒数。</param>
        /// <param name="realElapseSeconds">实际流逝的秒数。</param>
        public void UpdateCDsAfterSwitch(float elapseSeconds, float realElapseSeconds)
        {
            for (int i = 0; i < m_Heroes.Count; ++i)
            {
                m_Heroes[i].SwitchSkillCD.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 更新待战回血。
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public void UpdateHPRecovery(float elapseSeconds, float realElapseSeconds)
        {
            for (int i = 0; i < m_Heroes.Count; ++i)
            {
                if (i == CurrentHeroIndex)
                {
                    continue;
                }

                if (m_Heroes[i].IsDead)
                {
                    continue;
                }

                m_HPRecoverers[i].Update(elapseSeconds, realElapseSeconds);
            }
        }

        public void Clear()
        {
            m_Heroes.Clear();
            m_HPRecoverers.Clear();
        }

        [SerializeField]
        private class HPRecoverer
        {
            private HeroData m_HeroData;
            private float m_HelperFloatNumber;

            public HPRecoverer(HeroData heroData)
            {
                m_HeroData = heroData;
            }

            public void Reset()
            {
                m_HelperFloatNumber = 0f;
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_HeroData.HP >= m_HeroData.MaxHP)
                {
                    if (m_HelperFloatNumber > 0f)
                    {
                        Reset();
                    }

                    return;
                }

                m_HelperFloatNumber += m_HeroData.RecoverHP * elapseSeconds;

                if (m_HelperFloatNumber >= 1f)
                {
                    m_HeroData.HP += Mathf.FloorToInt(m_HelperFloatNumber);
                    m_HelperFloatNumber -= Mathf.Floor(m_HelperFloatNumber);
                }
            }
        }
    }
}

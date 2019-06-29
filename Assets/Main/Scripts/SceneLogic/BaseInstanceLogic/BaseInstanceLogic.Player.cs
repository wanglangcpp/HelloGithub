using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        protected abstract class Player
        {
            protected List<HeroData> m_PreparingHeroDatas = new List<HeroData>();
            protected PlayerHeroesData m_HeroesData = null;
            protected BaseInstanceLogic m_InstanceLogic = null;
            protected readonly IDictionary<CampType, List<ITargetable>> m_CampTargetableObjects;

            private List<BuffData> m_BuffsToTransferOnHeroSwitch = new List<BuffData>();
            private float m_CommonCoolDownEndTime = -1f;

            protected int m_Id = 0;

            public int Id
            {
                get
                {
                    return m_Id;
                }
            }

            public Player(int id, PlayerHeroesData heroesData, IDictionary<CampType, List<ITargetable>> campTargetableObjects, BaseInstanceLogic instanceLogic)
            {
                m_Id = id;
                m_HeroesData = heroesData;
                m_CampTargetableObjects = campTargetableObjects;
                m_InstanceLogic = instanceLogic;
            }

            public virtual void Shutdown()
            {

            }

            public bool IsDuringCommonCoolDown
            {
                get
                {
                    if (Time.time <= m_CommonCoolDownEndTime)
                    {
                        return true;
                    }

                    m_CommonCoolDownEndTime = -1f;
                    return false;
                }
            }

            public void StartCommonCoolDown(float coolDownTime)
            {
                m_CommonCoolDownEndTime = Time.time + coolDownTime;
            }

            public void StopCommonCoolDown()
            {
                m_CommonCoolDownEndTime = -1f;
            }

            public void FastForwardCommonCoolDown(float amount)
            {
                m_CommonCoolDownEndTime -= amount;

                if (m_CommonCoolDownEndTime <= 0f)
                {
                    StopCommonCoolDown();
                }
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (!m_InstanceLogic.IsRunning)
                {
                    return;
                }

                m_HeroesData.UpdateCDsAfterSwitch(elapseSeconds, realElapseSeconds);
                m_HeroesData.UpdateHPRecovery(elapseSeconds, realElapseSeconds);
            }

            public void OnHeroCharacterShown(HeroCharacter character)
            {
                var heroData = character.Data;

                if (heroData == m_HeroesData.CurrentHeroData)
                {
                    m_CampTargetableObjects[character.Data.Camp].Add(character);

                    if (IsSwitchingHero)
                    {
                        IsSwitchingHero = false;
                        StartCDAfterHeroSwitch();
                        OnSwitchHeroComplete();
                    }
                }
            }

            public void OnHeroWeaponsShown(HeroData heroData)
            {
                if (heroData != m_HeroesData.CurrentHeroData)
                {
                    GameEntry.Entity.HideEntity(heroData.Id);
                }

                if (m_PreparingHeroDatas.Count > 0)
                {
                    m_PreparingHeroDatas.Remove(heroData);

                    if (m_PreparingHeroDatas.Count <= 0)
                    {
                        NotifyHeroesReady();
                    }
                }
            }

            public bool AnyHeroHasDied { get; protected set; }

            public bool IsSwitchingHero { get; protected set; }

            public bool PrepareAndShowHero()
            {
                m_PreparingHeroDatas.Clear();
                //m_PreparingHeroDatas.Add(m_HeroesData.CurrentHeroData);
                //ShowHero(m_HeroesData.CurrentHeroData);

                var heroes = m_HeroesData.GetHeroes();
                for (int i = 0; i < heroes.Length; i++)
                {
                    m_PreparingHeroDatas.Add(heroes[i]);
                }

                for (int i = 0; i < heroes.Length; i++)
                {
                    ShowHero(heroes[i]);
                }

                return true;
            }

            public bool HeroIsCoolingDown(int index)
            {
                if (index == m_HeroesData.CurrentHeroIndex || m_HeroesData.GetHeroes().Length <= index || m_HeroesData.GetHeroes()[index].IsDead)
                {
                    return false;
                }

                var heroes = m_HeroesData.GetHeroes();
                return !heroes[index].SwitchSkillCD.IsReady;
            }

            public void ApplySwitchHero(int index)
            {
                if (IsSwitchingHero)
                {
                    return;
                }

                if (index == m_HeroesData.CurrentHeroIndex || m_HeroesData.GetHeroes().Length <= index || m_HeroesData.GetHeroes()[index].IsDead)
                {
                    return;
                }

                HeroCharacter heroCharacter = GetCurrentHeroCharacter();
                if (heroCharacter == null)
                {
                    Log.Warning("HeroCharacter not found.");
                    return;
                }

                OnWillSwitchHero();
                IsSwitchingHero = true;

                var localPosition = heroCharacter.CachedTransform.localPosition;
                var localRotation = heroCharacter.CachedTransform.localRotation;

                m_HeroesData.SwitchHero(index);
                var newCurrentHeroData = m_HeroesData.CurrentHeroData;
                newCurrentHeroData.Position = localPosition.ToVector2();
                newCurrentHeroData.Rotation = localRotation.eulerAngles.y;

                //联网切换英雄因为存在切换英雄的技能问题，关闭切换英雄的技能释放
                if (newCurrentHeroData.ModifierType == CharacterDataModifierType.Offline)
                    newCurrentHeroData.DebutOnShow = true;

                m_CampTargetableObjects[heroCharacter.Data.Camp].Remove(heroCharacter);

                heroCharacter.Motion.BreakSkills();

                for (int i = 0; i < heroCharacter.Data.SkillCD.Length; ++i)
                {
                    heroCharacter.Data.GetSkillCoolDownTime(i).SetReady();
                }

                m_BuffsToTransferOnHeroSwitch.Clear();
                for (int i = 0; i < heroCharacter.Data.Buffs.Count; ++i)
                {
                    var buffData = heroCharacter.Data.Buffs[i];
                    if (buffData.TransferOnHeroSwitch)
                    {
                        m_BuffsToTransferOnHeroSwitch.Add(buffData);
                    }
                }

                GameEntry.Entity.HideEntity(heroCharacter.Id);
                ShowHero(newCurrentHeroData);
            }

            public void TryAutoSwitchHero()
            {
                if (IsSwitchingHero)
                {
                    Log.Warning("Oops! Another hero switching procedure is ongoing.");
                    return;
                }

                int currentHeroIndex = m_HeroesData.CurrentHeroIndex;
                var heroArray = m_HeroesData.GetHeroes();

                float smallestRemainingCD = float.PositiveInfinity;
                int smallestRemainingCDIndex = -1;

                for (int i = 0; i < heroArray.Length; ++i)
                {
                    if (i == currentHeroIndex)
                    {
                        continue;
                    }

                    var hero = heroArray[i];
                    if (hero.IsDead)
                    {
                        continue;
                    }

                    if (hero.SwitchSkillCD.RemainingCoolDownSeconds >= smallestRemainingCD)
                    {
                        continue;
                    }

                    smallestRemainingCDIndex = i;
                    smallestRemainingCD = hero.SwitchSkillCD.RemainingCoolDownSeconds;
                }

                if (smallestRemainingCDIndex < 0)
                {
                    return;
                }

                if (m_InstanceLogic is BasePvpInstanceLogic)
                {
                    return;
                }
                RequestSwitchHero(smallestRemainingCDIndex, true);
            }

            public virtual void RequestSwitchHero(int index, bool ignoreCD = false)
            {
                if ((!ignoreCD && HeroIsCoolingDown(index)) || !GetCurrentHeroCharacter().CanSwitchHero)
                {
                    return;
                }

                ApplySwitchHero(index);
            }

            public void ApplyGoDie(HeroData heroData)
            {
                heroData.HP = 0;
                var rawEntity = GameEntry.Entity.GetEntity(heroData.Id);
                if (rawEntity == null)
                {
                    GameEntry.Event.FireNow(this, new CharacterDeadEventArgs(heroData, null, null, ImpactSourceType.Unknown));
                    return;
                }

                var character = rawEntity.Logic as Character;
                if (character != null)
                {
                    character.Motion.PerformGoDie();
                    character.IsDying = false;
                    return;
                }

                var building = rawEntity.Logic as Building;
                if (building != null)
                {
                    building.Motion.PerformGoDie();
                    building.IsDying = false;
                    return;
                }
            }

            public void CheckHeroesDeath(int entityId)
            {
                var heroes = m_HeroesData.GetHeroes();
                for (int i = 0; i < heroes.Length; ++i)
                {
                    if (entityId == heroes[i].Id)
                    {
                        AnyHeroHasDied = true;
                    }
                }
            }

            protected abstract void ShowHero(HeroData heroData);

            protected virtual void OnSwitchHeroComplete()
            {
                var heroCharacter = GetCurrentHeroCharacter();
                heroCharacter.Data.DodgeEnergy = heroCharacter.Data.MaxDodgeEnergy;
                for (int i = 0; i < m_BuffsToTransferOnHeroSwitch.Count; ++i)
                {
                    heroCharacter.AddTransferredBuff(m_BuffsToTransferOnHeroSwitch[i]);
                }

                m_BuffsToTransferOnHeroSwitch.Clear();
            }

            protected abstract void NotifyHeroesReady();

            public abstract HeroCharacter GetCurrentHeroCharacter();

            protected abstract void OnWillSwitchHero();

            protected void StartCDAfterHeroSwitch()
            {
                if (m_HeroesData.LastHeroData == null || m_HeroesData.LastHeroData.IsDead)
                {
                    return;
                }

                var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
                DRHero heroRow = heroDataTable.GetDataRow(m_HeroesData.LastHeroData.HeroId);
                if (heroRow == null)
                {
                    return;
                }

                m_HeroesData.LastHeroData.SwitchSkillCD.Reset(Mathf.Max(0f, heroRow.CDAfterChangeHero - m_HeroesData.LastHeroData.ReducedHeroSwitchCD), false);
            }
        }
    }
}

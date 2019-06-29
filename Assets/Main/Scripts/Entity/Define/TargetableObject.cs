using GameFramework;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 可作为目标的实体。
    /// </summary>
    public abstract class TargetableObject : Entity, ITargetable
    {
        [SerializeField]
        private TargetableObjectData m_TargetableObjectData = null;

        public new TargetableObjectData Data
        {
            get
            {
                return m_TargetableObjectData;
            }
        }

        public CapsuleCollider ImpactCollider
        {
            get;
            protected set;
        }

        public CampType Camp
        {
            get
            {
                return Data.Camp;
            }
        }

        public new TargetableObjectMotion Motion
        {
            get
            {
                return base.Motion as TargetableObjectMotion;
            }
            set
            {
                base.Motion = value;
            }
        }

        public abstract float ModelHeight
        {
            get;
        }

        public abstract bool NeedShowHPBarOnDamage
        {
            get;
        }

        /// <summary>
        /// 是否达到了死亡后的保持时间。
        /// </summary>
        public abstract bool DeadKeepTimeIsReached
        {
            get;
        }

        /// <summary>
        /// 是否死亡。
        /// </summary>
        public bool IsDead
        {
            get
            {
                return Data.IsDead;
            }
        }

        /// <summary>
        /// 是否假死。
        /// </summary>
        public bool IsFakeDead
        {
            get
            {
                return !Data.IsDead && Data.HP <= 0;
            }
        }
        /// <summary>
        /// 在多人联机中，触发死亡事件，正在通知其他客户端，非联网环境不要使用
        /// </summary>
        public bool IsDying { get; set; }
        /// <summary>
        /// 是否正在进场。
        /// </summary>
        public virtual bool IsEntering
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取克制元素编号。
        /// </summary>
        public int ElementId
        {
            get
            {
                return m_TargetableObjectData.ElementId;
            }
        }

        private ColorChanger m_CachedColorChanger = null;

        public ColorChanger ColorChanger
        {
            get
            {
                return m_CachedColorChanger;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            CachedTransform.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
            m_CachedColorChanger = GetComponent<ColorChanger>();

            ImpactCollider = gameObject.GetOrAddComponent<CapsuleCollider>();
            ImpactCollider.enabled = false;
            IsDying = false;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_TargetableObjectData = userData as TargetableObjectData;

            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }

            ResetColorChangerAndRecoverRenderers();

            ImpactCollider.enabled = true;

            if (GameEntry.IsAvailable)
            {
                GameEntry.SceneLogic.BaseInstanceLogic.OnTargetableShow(this);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Data.BuffPool.Update(Time.time);

            if (GameEntry.IsAvailable)
            {
                GameEntry.SceneLogic.BaseInstanceLogic.OnTargetableUpdate(this);
            }
        }

        protected override void OnHide(object userData)
        {
            ClearBuffs();
            Data.BuffPool = null;
            ResetColorChangerAndRecoverRenderers();

            if (GameEntry.IsAvailable)
            {
                GameEntry.SceneLogic.BaseInstanceLogic.OnTargetableHide(this);
            }

            base.OnHide(userData);
            ImpactCollider.enabled = false;
        }

        public void TryGoDie()
        {
            if (GameEntry.IsAvailable && IsDying == false)
            {
                IsDying = true;
                //客户端先行表现死亡
                Motion.PerformGoDie();

                GameEntry.SceneLogic.BaseInstanceLogic.TryGoDie(this);
            }
        }

        public int StartColorChange(int colorChangeId)
        {
            return StartColorChange(colorChangeId, float.PositiveInfinity);
        }

        public int StartColorChange(int colorChangeId, float duration)
        {
            if (m_CachedColorChanger == null)
            {
                return -1;
            }

            return m_CachedColorChanger.StartColorChange(colorChangeId, duration);
        }

        public void StopColorChange(int serialId)
        {
            if (m_CachedColorChanger == null)
            {
                return;
            }

            m_CachedColorChanger.StopColorChange(serialId);
        }

        public void ResetColorChangerAndRecoverRenderers()
        {
            if (m_CachedColorChanger == null)
            {
                return;
            }

            m_CachedColorChanger.ShowAllRenderers();
            m_CachedColorChanger.Reset();
        }

        #region Buff related

        public void AddBuff(int buffId, EntityData ownerData, long buffSerialId, SkillBadgesData ownerSkillBadges)
        {
            AddBuff(buffId, ownerData, buffSerialId, ownerSkillBadges, Time.time);
        }

        public void AddBuff(int buffId, EntityData ownerData, long buffSerialId, SkillBadgesData ownerSkillBadges, float startTime)
        {
            if (IsDead)
            {
                return;
            }

            Data.BuffPool.Add(buffId, ownerData, buffSerialId, startTime, ownerSkillBadges == null ? null : new Dictionary<string, object>
            {
                { Constant.Buff.UserData.OwnerSkillBadgesKey, ownerSkillBadges },
            });
        }

        public void AddTransferredBuff(BuffData buffData)
        {
            if (IsDead)
            {
                return;
            }

            Data.BuffPool.AddTransferred(buffData);
        }

        public void RemoveBuffByType(BuffType buffType)
        {
            Data.BuffPool.RemoveByType(buffType);
        }

        public BuffData GetBuffByType(BuffType buffType)
        {
            return Data.BuffPool.GetByType(buffType);
        }

        public void RemoveBuffById(int buffId)
        {
            RemoveBuffByIds(new int[] { buffId });
        }

        public void RemoveBuffByIds(IList<int> buffIds)
        {
            Data.BuffPool.RemoveByIds(buffIds);
        }

        public void ClearBuffs()
        {
            Data.BuffPool.Clear();
        }

        public bool HasNumHarmFreeBuff
        {
            get
            {
                for (int i = 0; i < Data.BuffPool.Buffs.Length; ++i)
                {
                    var buff = Data.BuffPool.Buffs[i];
                    if (buff.BuffType == BuffType.StateAndNumHarmFree)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion Buff related
    }
}

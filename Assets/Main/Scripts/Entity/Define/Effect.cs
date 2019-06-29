using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 特效实体。
    /// </summary>
    public class Effect : Entity
    {
        [SerializeField]
        private EffectData m_EffectData = null;

        private float? m_KeepTime = null;
        private float m_StartTime = 0f;

        public new EffectData Data
        {
            get
            {
                return m_EffectData;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            EffectTime effectTime = GetComponent<EffectTime>();
            if (effectTime == null)
            {
                Log.Warning("EffectTime component in '{0}' is not exist.", Name);
                return;
            }

            if (effectTime.IsForever)
            {
                m_KeepTime = null;
            }
            else
            {
                m_KeepTime = effectTime.Duration;
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_EffectData = userData as EffectData;

            m_StartTime = Time.time;

            if (HasOwner)
            {
                var targetOwner = Owner as ITargetable;
                if (targetOwner != null && (targetOwner.IsDead && !m_EffectData.CanAttachToDeadOwner || !targetOwner.IsAvailable))
                {
                    GameEntry.Entity.HideEntity(Entity);
                }
                else
                {
                    GameEntry.Entity.AttachEntity(Entity, Owner.Entity, Data.TargetParentPath);
                }
            }
            else
            {
                CachedTransform.localPosition = m_EffectData.NeedSamplePosition ? AIUtility.SamplePosition(m_EffectData.Position) : m_EffectData.Position.ToVector3();
                CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_EffectData.Rotation, 0f));
            }
        }

        protected override void OnHide(object userData)
        {
            base.OnHide(userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_KeepTime == null)
            {
                return;
            }

            if (Time.time >= m_StartTime + m_KeepTime.Value)
            {
                GameEntry.Entity.HideEntity(this.Entity, Data.UserData);
            }
        }
    }
}

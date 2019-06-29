using UnityEngine;

namespace Genesis.GameClient
{
    public class EntitySphereColliderTriggerImpactTimeLineAction : EntityColliderTriggerImpactTimeLineActionBase
    {
        private EntitySphereColliderTriggerImpactTimeLineActionData m_Data;

        public EntitySphereColliderTriggerImpactTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntitySphereColliderTriggerImpactTimeLineActionData;
        }

        public Vector3 Offset
        {
            get
            {
                return m_Data.Offset ?? Vector3.zero;
            }
        }

        public float Radius
        {
            get
            {
                return m_Data.Radius ?? 1f;
            }
        }

        public override int[] ImpactIds
        {
            get
            {
                return m_Data.ImpactIds;
            }
        }

        public override bool AffectFriendly
        {
            get
            {
                return m_Data.AffectFriendly;
            }
        }

        public override bool AffectNeutral
        {
            get
            {
                return m_Data.AffectNeutral;
            }
        }

        public override bool AffectHostile
        {
            get
            {
                return m_Data.AffectHostile;
            }
        }

        public override bool AcceptRepeatedImpact
        {
            get
            {
                return m_Data.AcceptRepeatedImpact;
            }
        }

        public override float? RepeatedImpactIntervalTime
        {
            get
            {
                return m_Data.RepeatedImpactIntervalTime;
            }
        }

        public override string HitEffectResourceName
        {
            get
            {
                return m_Data.HitEffectResourceName;
            }
        }

        public override int HitEffectAttachPointId
        {
            get
            {
                return m_Data.HitEffectAttachPointId;
            }
        }

        public override int[] HitSoundIds
        {
            get
            {
                return m_Data.HitSoundIds;
            }
        }

        public override bool ShouldBroadcastSound
        {
            get
            {
                return m_Data.ShouldBroadcastSound;
            }
        }

        public override float HitSoundMinInterval
        {
            get
            {
                return m_Data.HitSoundMinInterval;
            }
        }

        public override int[] BuffIds
        {
            get
            {
                return m_Data.BuffIds;
            }
        }

        public override EntityColliderTriggerImpactTimeLineActionDataAbstract.BuffConditionalImpactGroup[] BuffConditionalImpactGroups
        {
            get
            {
                return m_Data.BuffConditionalImpactGroups;
            }
        }

        public override int ColorChangeId
        {
            get
            {
                return m_Data.ColorChangeId;
            }
        }

        public override float ColorChangeDuration
        {
            get
            {
                return m_Data.ColorChangeDuration;
            }
        }

        public override string LineEffectResourceName
        {
            get
            {
                return m_Data.LineEffectResourceName;
            }
        }

        public override string LineEffectMySideAttachPoint
        {
            get
            {
                return m_Data.LineEffectMySideAttachPoint;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            base.OnStart(timeLineInstance);

            ColliderTrigger colliderTrigger = CreateColliderTrigger(timeLineInstance, "Sphere");
            colliderTrigger.CachedTransform.localPosition = Offset * timeLineInstance.Owner.Data.Scale;
            colliderTrigger.CachedTransform.localScale = Vector3.one * Radius * timeLineInstance.Owner.Data.Scale;
        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {
            Vector3 position = timeLineInstance.Owner.CachedTransform.TransformPoint(Offset);
            Color cachedColor = Gizmos.color;
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawSphere(position, Radius * timeLineInstance.Owner.Data.Scale);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, Radius * timeLineInstance.Owner.Data.Scale);
            Gizmos.color = cachedColor;
        }
    }
}

using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityBoxColliderTriggerImpactTimeLineAction : EntityColliderTriggerImpactTimeLineActionBase
    {
        private EntityBoxColliderTriggerImpactTimeLineActionData m_Data;

        public EntityBoxColliderTriggerImpactTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityBoxColliderTriggerImpactTimeLineActionData;
        }

        public Vector3 Offset
        {
            get
            {
                return m_Data.Offset ?? Vector3.zero;
            }
        }

        public float Direction
        {
            get
            {
                return m_Data.Direction ?? 0f;
            }
        }

        public Vector3 Size
        {
            get
            {
                return m_Data.Size ?? Vector3.one;
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

            ColliderTrigger colliderTrigger = CreateColliderTrigger(timeLineInstance, "Box");
            colliderTrigger.CachedTransform.localPosition = Offset * timeLineInstance.Owner.Data.Scale;
            colliderTrigger.CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, Direction, 0f));
            colliderTrigger.CachedTransform.localScale = Size * timeLineInstance.Owner.Data.Scale;
        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {
            Vector3 position = timeLineInstance.Owner.CachedTransform.TransformPoint(Offset);
            Quaternion rotation = timeLineInstance.Owner.CachedTransform.localRotation * Quaternion.Euler(new Vector3(0f, Direction, 0f));
            Vector3 scale = Size * timeLineInstance.Owner.Data.Scale;
            Matrix4x4 cachedMatrix = Gizmos.matrix;
            Color cachedColor = Gizmos.color;
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, scale);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = cachedColor;
            Gizmos.matrix = cachedMatrix;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntitySectorColliderTriggerImpactTimeLineAction : EntityColliderTriggerImpactTimeLineActionBase
    {
        private EntitySectorColliderTriggerImpactTimeLineActionData m_Data;

        private IDictionary<string, Mesh> m_CachedMesh = null;

        public EntitySectorColliderTriggerImpactTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntitySectorColliderTriggerImpactTimeLineActionData;
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

        public int Angle
        {
            get
            {
                return m_Data.Angle.HasValue ? (m_Data.Angle.Value % 360 + 15) / 30 * 30 : 0;
            }
        }

        public float Radius
        {
            get
            {
                return m_Data.Radius ?? 1f;
            }
        }

        public float Height
        {
            get
            {
                return m_Data.Height ?? 1f;
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

            ColliderTrigger colliderTrigger = CreateColliderTrigger(timeLineInstance, string.Format("Sector{0}", Angle.ToString()));
            colliderTrigger.CachedTransform.localPosition = Offset * timeLineInstance.Owner.Data.Scale;
            colliderTrigger.CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, Direction, 0f));
            colliderTrigger.CachedTransform.localScale = new Vector3(Radius, Height, Radius) * timeLineInstance.Owner.Data.Scale;

#if UNITY_EDITOR
            if (m_CachedMesh == null)
            {
                m_CachedMesh = new Dictionary<string, Mesh>();
                if (!m_CachedMesh.ContainsKey(Angle.ToString()))
                {
                    m_CachedMesh[Angle.ToString()] = colliderTrigger.GetComponent<MeshCollider>().sharedMesh;
                }
            }
#endif
        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {
            Mesh cachedMesh = null;
            if (m_CachedMesh == null || !m_CachedMesh.TryGetValue(Angle.ToString(), out cachedMesh))
            {
                return;
            }

            Vector3 position = timeLineInstance.Owner.CachedTransform.TransformPoint(Offset);
            Quaternion rotation = timeLineInstance.Owner.CachedTransform.localRotation * Quaternion.Euler(new Vector3(0f, Direction, 0f));
            Vector3 scale = new Vector3(Radius, Height, Radius) * timeLineInstance.Owner.Data.Scale;
            Color cachedColor = Gizmos.color;
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawMesh(cachedMesh, position, rotation, scale);
            Gizmos.color = Color.green;
            Gizmos.DrawWireMesh(cachedMesh, position, rotation, scale);
            Gizmos.color = cachedColor;
        }
    }
}

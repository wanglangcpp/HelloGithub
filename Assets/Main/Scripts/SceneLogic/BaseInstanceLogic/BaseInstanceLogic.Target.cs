using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        protected TargetManager m_Target = null;

        public TargetManager Target
        {
            get
            {
                return m_Target;
            }
        }

        public class TargetManager
        {
            private readonly IDictionary<int, TargetInfo> m_TargetInfos;

            public TargetManager()
            {
                m_TargetInfos = new Dictionary<int, TargetInfo>();
            }

            public Vector2 CalcTargetPosition(TargetableObject sourceEntity, float radius)
            {
                if (sourceEntity == null)
                {
                    Log.Warning("Source entity is invalid.");
                    return Vector2.zero;
                }

                ICanHaveTarget source = sourceEntity as ICanHaveTarget;
                if (source == null || !source.HasTarget)
                {
                    Log.Warning("Source entity do not have target.");
                    return Vector2.zero;
                }

                TargetableObject targetEntity = source.Target as TargetableObject;

                TargetInfo targetInfo = null;
                if (!m_TargetInfos.TryGetValue(targetEntity.Id, out targetInfo))
                {
                    targetInfo = new TargetInfo(targetEntity);
                    m_TargetInfos.Add(targetEntity.Id, targetInfo);
                }
                else if (targetInfo.TargetEntity == null)
                {
                    return sourceEntity.CachedTransform.position.ToVector2();
                }

                return targetInfo.GetTargetPosition(sourceEntity, radius);
            }

            public bool RemoveOccupier(TargetableObject sourceEntity)
            {
                if (sourceEntity == null)
                {
                    Log.Warning("Source entity is invalid.");
                    return false;
                }

                ICanHaveTarget source = sourceEntity as ICanHaveTarget;
                if (source == null || !source.HasTarget)
                {
                    Log.Warning("Source entity do not have target.");
                    return false;
                }

                TargetableObject targetEntity = source.Target as TargetableObject;

                TargetInfo targetInfo = null;
                if (!m_TargetInfos.TryGetValue(targetEntity.Id, out targetInfo))
                {
                    return false;
                }

                return targetInfo.RemoveOccupier(sourceEntity);
            }

            private class TargetInfo
            {
                private const int IndexCount = 12;
                private const float DegPerIndex = 360f / IndexCount;

                private TargetableObject m_TargetEntity;
                private int m_TargetEntityId;

                public TargetableObject TargetEntity
                {
                    get
                    {
                        if (m_TargetEntity == null)
                        {
                            m_TargetEntity = GameEntry.Entity.GetEntity(m_TargetEntityId).Logic as TargetableObject;
                        }

                        return m_TargetEntity;
                    }
                }

                private readonly IList<TargetableObject> m_OccupiedEntities;

                public TargetInfo(TargetableObject targetEntity)
                {
                    m_TargetEntityId = targetEntity.Id;
                    m_TargetEntity = targetEntity;
                    m_OccupiedEntities = new List<TargetableObject>(IndexCount);
                    for (int i = 0; i < IndexCount; i++)
                    {
                        m_OccupiedEntities.Add(null);
                    }
                }

                public Vector2 GetTargetPosition(TargetableObject sourceEntity, float radius)
                {
                    Vector2 targetPosition = TargetEntity.CachedTransform.position.ToVector2();
                    Vector2 sourcePosition = sourceEntity.CachedTransform.position.ToVector2();
                    Vector2 delta = sourcePosition - targetPosition;

                    radius += TargetEntity.ImpactCollider.radius;

                    float distance = delta.magnitude;
                    if (distance <= radius * 0.8f)
                    {
                        return sourcePosition;
                    }
                    else if (distance > radius * 3)
                    {
                        RemoveOccupier(sourceEntity);
                    }

                    int index = m_OccupiedEntities.IndexOf(sourceEntity);
                    if (index < 0)
                    {
                        float theta = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                        if (theta < 0f)
                        {
                            theta += 360f;
                        }

                        index = Mathf.RoundToInt(theta / DegPerIndex) % IndexCount;

                        for (int i = 0; i < (IndexCount + 1) / 2; i++)
                        {
                            int next = (index + i) % IndexCount;
                            if (m_OccupiedEntities[next] == null || m_OccupiedEntities[next].IsDead)
                            {
                                index = next;
                                break;
                            }

                            int prev = (index - i + IndexCount) % IndexCount;
                            if (prev == next)
                            {
                                continue;
                            }
                            if (m_OccupiedEntities[prev] == null || m_OccupiedEntities[prev].IsDead)
                            {
                                index = prev;
                                break;
                            }
                        }

                        if (m_OccupiedEntities[index] == null || m_OccupiedEntities[index].IsDead)
                        {
                            m_OccupiedEntities[index] = sourceEntity;
                        }
                    }

                    float realDeg = DegPerIndex * index;
                    return targetPosition + new Vector2(Mathf.Cos(realDeg), Mathf.Sin(realDeg)) * radius;
                }

                public bool RemoveOccupier(TargetableObject sourceEntity)
                {
                    int index = m_OccupiedEntities.IndexOf(sourceEntity);
                    if (index >= 0)
                    {
                        m_OccupiedEntities[index] = null;
                        return true;
                    }

                    return false;
                }
            }
        }
    }
}

using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class EntityColliderTriggerImpactTimeLineActionBase
    {
        private abstract class StrategyBase
        {
            protected ITimeLineInstance<Entity> m_TimeLineInstance;
            protected EntityColliderTriggerImpactTimeLineActionBase m_TimeLineAction;
            protected ImpactData m_ImpactData;

            internal virtual void Init(ITimeLineInstance<Entity> timeLineInstance, EntityColliderTriggerImpactTimeLineActionBase timeLineAction)
            {
                m_TimeLineInstance = timeLineInstance;
                m_TimeLineAction = timeLineAction;

                // Initialize m_ImpactData in subclasses.
            }

            internal virtual void Shutdown()
            {
                m_ImpactData = null;
                m_TimeLineAction = null;
                m_TimeLineInstance = null;
            }

            internal abstract void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds);

            internal abstract void OnTriggerEnter(ITimeLineInstance<Entity> timeLineInstance, ICampable campedOwner, ITargetable targetableTarget, Entity entityTarget);

            internal abstract void OnTriggerExit(ITimeLineInstance<Entity> timeLineInstance, Entity entity);

            internal abstract void HideAllLineEffects(ITimeLineInstance<Entity> timeLineInstance);

            internal abstract void OnShowLineEffectFailure(object sender, SimpleLineEffectData simpleLineED);

            internal abstract void OnShowLineEffectSuccess(object sender, SimpleLineEffectData simpleLineED);

            internal virtual void OnStart(ITimeLineInstance<Entity> timeLineInstance)
            {
                // Do nothing.
            }

            protected static void HideEffectEntity(int effectEntityId)
            {
                if (!GameEntry.Entity.IsLoadingEntity(effectEntityId) && !GameEntry.Entity.HasEntity(effectEntityId))
                {
                    return;
                }

                GameEntry.Entity.HideEntity(effectEntityId);
            }
        }

        private class StrategyNormal : StrategyBase
        {
            private ImpactData_Normal ImpactData
            {
                get
                {
                    return m_ImpactData as ImpactData_Normal;
                }
            }

            internal override void Init(ITimeLineInstance<Entity> timeLineInstance, EntityColliderTriggerImpactTimeLineActionBase timeLineAction)
            {
                base.Init(timeLineInstance, timeLineAction);
                m_ImpactData = new ImpactData_Normal();
            }

            internal override void OnTriggerEnter(ITimeLineInstance<Entity> timeLineInstance, ICampable campedOwner, ITargetable targetableTarget, Entity entityTarget)
            {
                m_ImpactData.EnteredEntityIds.Add(entityTarget.Id);

                if (m_TimeLineAction.RepeatedImpactIntervalTime == null)
                {
                    m_TimeLineAction.DoImpact(timeLineInstance, targetableTarget, m_ImpactData);
                }

                ShowLineEffectIfNeeded(timeLineInstance, campedOwner, entityTarget as TargetableObject);
            }

            internal override void OnTriggerExit(ITimeLineInstance<Entity> timeLineInstance, Entity entity)
            {
                m_ImpactData.EnteredEntityIds.Remove(entity.Id);
                HideLineEffect(timeLineInstance, entity);
            }

            internal override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
            {
                if (m_TimeLineAction.RepeatedImpactIntervalTime == null || Time.time - m_ImpactData.LastImpactTime < m_TimeLineAction.RepeatedImpactIntervalTime)
                {
                    return;
                }

                m_ImpactData.LastImpactTime = Time.time;
                foreach (int entityId in m_ImpactData.EnteredEntityIds)
                {
                    UnityGameFramework.Runtime.Entity targetEntity = GameEntry.Entity.GetEntity(entityId);
                    if (targetEntity == null)
                    {
                        continue;
                    }

                    m_TimeLineAction.DoImpact(timeLineInstance, targetEntity.Logic as ITargetable, m_ImpactData);
                }
            }

            private void ShowLineEffectIfNeeded(ITimeLineInstance<Entity> timeLineInstance, ICampable campedOwner, Entity entity)
            {
                if (string.IsNullOrEmpty(m_TimeLineAction.LineEffectResourceName))
                {
                    return;
                }

                var target = entity as ITargetable;
                if (target == null)
                {
                    return;
                }

                var targetCharacter = entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                if (!m_TimeLineAction.WillCauseImpact(campedOwner, target))
                {
                    return;
                }

                if (ImpactData.TargetEntityIdsToLineEffectIdsAboutToShow.ContainsKey(entity.Id) || ImpactData.TargetEntityIdsToLineEffectIds.ContainsKey(entity.Id))
                {
                    return;
                }

                var effectEntityId = GameEntry.Entity.GetSerialId();
                var effectData = new SimpleLineEffectData(effectEntityId, m_TimeLineAction.m_Data.LineEffectMySideAttachPoint, m_TimeLineAction.m_Data.LineEffectResourceName, timeLineInstance.Owner.Id);
                effectData.EndingEntityId = entity.Id;
                IDataTable<DRCharacter> dt = GameEntry.DataTable.GetDataTable<DRCharacter>();
                string hitPointPath = dt[targetCharacter.Data.CharacterId].GetHitPoint(m_TimeLineAction.HitEffectAttachPointId);
                effectData.EndingTransformPath = hitPointPath;
                ImpactData.TargetEntityIdsToLineEffectIdsAboutToShow.Add(entity.Id, effectEntityId);
                GameEntry.Entity.ShowEffect(effectData);
            }

            private void HideLineEffect(ITimeLineInstance<Entity> timeLineInstance, Entity entity)
            {
                var aboutToShow = ImpactData.TargetEntityIdsToLineEffectIdsAboutToShow;

                if (aboutToShow.ContainsKey(entity.Id))
                {
                    int effectEntityId = aboutToShow[entity.Id];
                    aboutToShow.Remove(entity.Id);
                    HideEffectEntity(effectEntityId);
                    return;
                }

                var showing = ImpactData.TargetEntityIdsToLineEffectIds;

                if (showing.ContainsKey(entity.Id))
                {
                    int effectEntityId = showing[entity.Id];
                    showing.Remove(entity.Id);
                    HideEffectEntity(effectEntityId);
                    return;
                }
            }

            internal override void HideAllLineEffects(ITimeLineInstance<Entity> timeLineInstance)
            {
                var aboutToShow = ImpactData.TargetEntityIdsToLineEffectIdsAboutToShow;

                foreach (var kvPair in aboutToShow)
                {
                    HideEffectEntity(kvPair.Value);
                }

                aboutToShow.Clear();
                var showing = ImpactData.TargetEntityIdsToLineEffectIds;

                foreach (var kvPair in showing)
                {
                    HideEffectEntity(kvPair.Value);
                }

                showing.Clear();
            }

            internal override void OnShowLineEffectFailure(object sender, SimpleLineEffectData simpleLineED)
            {
                var aboutToShow = ImpactData.TargetEntityIdsToLineEffectIdsAboutToShow;
                int expectedEntityId;
                if (!aboutToShow.TryGetValue(simpleLineED.EndingEntityId, out expectedEntityId) || expectedEntityId != simpleLineED.Id)
                {
                    return;
                }

                aboutToShow.Remove(simpleLineED.EndingEntityId);
            }

            internal override void OnShowLineEffectSuccess(object sender, SimpleLineEffectData simpleLineED)
            {
                var aboutToShow = ImpactData.TargetEntityIdsToLineEffectIdsAboutToShow;
                int expectedEntityId;
                if (!aboutToShow.TryGetValue(simpleLineED.EndingEntityId, out expectedEntityId) || expectedEntityId != simpleLineED.Id)
                {
                    return;
                }

                aboutToShow.Remove(simpleLineED.EndingEntityId);

                var showing = ImpactData.TargetEntityIdsToLineEffectIds;

                if (showing.ContainsKey(simpleLineED.EndingEntityId))
                {
                    HideEffectEntity(showing[simpleLineED.EndingEntityId]);
                    showing.Remove(simpleLineED.EndingEntityId);
                }

                showing.Add(simpleLineED.EndingEntityId, simpleLineED.Id);
            }
        }

        private class StrategyLightningChain : StrategyBase
        {
            private ImpactData_StrategyLightningChain ImpactData
            {
                get
                {
                    return m_ImpactData as ImpactData_StrategyLightningChain;
                }
            }

            private bool m_ShouldReceiveOnTriggerEnter;
            private int m_TransferCount;
            private float m_TransferDeltaTime;
            private float m_LastTransferTime = float.PositiveInfinity;
            private TargetableObject m_LastTarget = null;
            private CampType[] m_CampsForImpacts;

            private EntityColliderTriggerImpactTimeLineActionDataAbstract TimeLineActionData
            {
                get
                {
                    return m_TimeLineAction.m_Data;
                }
            }

            private TargetableObject Me
            {
                get
                {
                    return m_TimeLineInstance.Owner as TargetableObject;
                }
            }

            internal override void Init(ITimeLineInstance<Entity> timeLineInstance, EntityColliderTriggerImpactTimeLineActionBase timeLineAction)
            {
                base.Init(timeLineInstance, timeLineAction);
                m_ImpactData = new ImpactData_StrategyLightningChain();
                m_TransferCount = TimeLineActionData.ChainIntervalCount;
                m_TransferDeltaTime = TimeLineActionData.Duration / TimeLineActionData.ChainIntervalCount;
            }

            internal override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
            {
                base.OnStart(timeLineInstance);

                var camps = new List<CampType>();
                var me = timeLineInstance.Owner as ICampable;

                if (TimeLineActionData.AffectFriendly)
                {
                    camps.AddRange(AIUtility.GetCamps(me.Camp, RelationType.Friendly));
                }

                if (TimeLineActionData.AffectHostile)
                {
                    camps.AddRange(AIUtility.GetCamps(me.Camp, RelationType.Hostile));
                }

                if (TimeLineActionData.AffectNeutral)
                {
                    camps.AddRange(AIUtility.GetCamps(me.Camp, RelationType.Neutral));
                }

                m_CampsForImpacts = camps.ToArray();

                m_ShouldReceiveOnTriggerEnter = true;
            }

            internal override void OnTriggerEnter(ITimeLineInstance<Entity> timeLineInstance, ICampable campedOwner, ITargetable targetableTarget, Entity entityTarget)
            {
                if (!m_ShouldReceiveOnTriggerEnter)
                {
                    return;
                }

                if (entityTarget == null)
                {
                    return;
                }

                ImpactData.EnteredEntityIds.Add(entityTarget.Id);
            }

            internal override void OnTriggerExit(ITimeLineInstance<Entity> timeLineInstance, Entity entity)
            {
                // Do nothing.
            }

            internal override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
            {
                if (m_ShouldReceiveOnTriggerEnter)
                {
                    ImpactFirstTarget();
                    m_ShouldReceiveOnTriggerEnter = false;
                    return;
                }

                if (Time.time - m_LastTransferTime > m_TransferDeltaTime && m_TransferCount > 0)
                {
                    ImpactNextTarget();
                }
            }

            internal override void HideAllLineEffects(ITimeLineInstance<Entity> timeLineInstance)
            {
                var aboutToShow = ImpactData.AboutToShowEffectIds;

                foreach (var kvPair in aboutToShow)
                {
                    HideEffectEntity(kvPair.Value);
                }

                aboutToShow.Clear();
                var showing = ImpactData.ShowingEffectIds;

                foreach (var kvPair in showing)
                {
                    HideEffectEntity(kvPair.Value);
                }

                showing.Clear();
            }

            internal override void OnShowLineEffectFailure(object sender, SimpleLineEffectData simpleLineED)
            {
                var aboutToShow = ImpactData.AboutToShowEffectIds;
                var key = new ImpactData_StrategyLightningChain.Key { FromId = simpleLineED.OwnerId, ToId = simpleLineED.EndingEntityId };
                int expectedEntityId;
                if (!aboutToShow.TryGetValue(key, out expectedEntityId) || expectedEntityId != simpleLineED.Id)
                {
                    return;
                }

                aboutToShow.Remove(key);
            }

            internal override void OnShowLineEffectSuccess(object sender, SimpleLineEffectData simpleLineED)
            {
                var aboutToShow = ImpactData.AboutToShowEffectIds;
                var key = new ImpactData_StrategyLightningChain.Key { FromId = simpleLineED.OwnerId, ToId = simpleLineED.EndingEntityId };
                int expectedEntityId;
                if (!aboutToShow.TryGetValue(key, out expectedEntityId) || expectedEntityId != simpleLineED.Id)
                {
                    return;
                }

                aboutToShow.Remove(key);

                var showing = ImpactData.ShowingEffectIds;

                if (showing.ContainsKey(key))
                {
                    Log.Info("Trying to hide remaining line effect.");
                    HideEffectEntity(showing[key]);
                    showing.Remove(key);
                }

                showing.Add(key, simpleLineED.Id);
            }

            private bool ImpactFirstTarget()
            {
                TargetableObject nearest = null;
                float smallestDistance = float.PositiveInfinity;
                var me = m_TimeLineInstance.Owner as TargetableObject;

                foreach (int entityId in ImpactData.EnteredEntityIds)
                {
                    var entity = GameEntry.Entity.GetGameEntity(entityId);
                    if (entity == null)
                    {
                        continue;
                    }

                    var targetableTarget = entity as TargetableObject;
                    if (targetableTarget == null)
                    {
                        continue;
                    }

                    if (!m_TimeLineAction.WillCauseImpact(me, targetableTarget))
                    {
                        continue;
                    }

                    var distance = Vector3.Distance(targetableTarget.CachedTransform.localPosition, me.CachedTransform.localPosition);
                    if (distance < smallestDistance)
                    {
                        nearest = targetableTarget;
                        smallestDistance = distance;
                    }
                }

                if (nearest == null)
                {
                    return false;
                }

                ShowLineEffectAndDoImpact(me, nearest);
                return true;
            }

            private void ShowLineEffect(TargetableObject from, TargetableObject to)
            {
                IDataTable<DRCharacter> dt = GameEntry.DataTable.GetDataTable<DRCharacter>();

                string fromAttachPath = string.Empty;
                Character fromCharacter = from as Character;
                if (from.Id == Me.Id)
                {
                    fromAttachPath = TimeLineActionData.LineEffectMySideAttachPoint;
                }
                else if (fromCharacter != null)
                {
                    fromAttachPath = dt[fromCharacter.Data.CharacterId].GetHitPoint(m_TimeLineAction.HitEffectAttachPointId);
                }
                else
                {
                    // TODO: Do buildings need hit points?
                }

                string toAttachPath = string.Empty;
                Character toCharacter = to as Character;

                if (toCharacter != null)
                {
                    toAttachPath = dt[toCharacter.Data.CharacterId].GetHitPoint(m_TimeLineAction.HitEffectAttachPointId);
                }

                int effectEntityId = GameEntry.Entity.GetSerialId();
                var effectData = new SimpleLineEffectData(effectEntityId, fromAttachPath, m_TimeLineAction.m_Data.LineEffectResourceName, from.Id);
                effectData.EndingEntityId = to.Id;
                effectData.EndingTransformPath = toAttachPath;
                effectData.CanAttachToDeadOwner = true;

                var key = new ImpactData_StrategyLightningChain.Key { FromId = from.Id, ToId = to.Id };
                if (ImpactData.AboutToShowEffectIds.ContainsKey(key))
                {
                    return;
                }

                if (ImpactData.ShowingEffectIds.ContainsKey(key))
                {
                    HideLineEffect(from, to);
                }

                ImpactData.AboutToShowEffectIds.Add(key, effectEntityId);
                GameEntry.Entity.ShowEffect(effectData);
            }

            private void HideLineEffect(TargetableObject from, TargetableObject to)
            {
                var key = new ImpactData_StrategyLightningChain.Key { FromId = from.Id, ToId = to.Id };
                int effectEntityId;

                if (ImpactData.AboutToShowEffectIds.TryGetValue(key, out effectEntityId))
                {
                    ImpactData.AboutToShowEffectIds.Remove(key);
                    HideEffectEntity(effectEntityId);
                    return;
                }

                if (ImpactData.ShowingEffectIds.TryGetValue(key, out effectEntityId))
                {
                    ImpactData.ShowingEffectIds.Remove(key);
                    HideEffectEntity(effectEntityId);
                    return;
                }
            }

            private void ImpactNextTarget()
            {
                if (m_LastTarget == null)
                {
                    return;
                }

                var me = m_TimeLineInstance.Owner as TargetableObject;
                List<TargetableObject> candidates = new List<TargetableObject>();

                for (int i = 0; i < m_CampsForImpacts.Length; ++i)
                {
                    var objects = GameEntry.SceneLogic.BaseInstanceLogic.GetCampTargetableObjects(m_CampsForImpacts[i]);
                    SelectCandidateTargets(me, candidates, objects);
                }

                // TODO: Optimize if needed. We only need the smallest candidate here.
                candidates.Sort(CompareCandidateTargets);

                TargetableObject nearest = null;
                if (candidates.Count > 0)
                {
                    nearest = candidates[0];
                }

                if (nearest == null)
                {
                    nearest = m_LastTarget;
                }

                ShowLineEffectAndDoImpact(m_LastTarget, nearest);
            }

            private void SelectCandidateTargets(TargetableObject me, List<TargetableObject> candidates, ITargetable[] objects)
            {
                for (int j = 0; j < objects.Length; ++j)
                {
                    if (!AIUtility.TargetCanBeSelected(objects[j]))
                    {
                        continue;
                    }

                    var targetableObject = objects[j] as TargetableObject;
                    if (targetableObject == null || targetableObject == me)
                    {
                        continue;
                    }

                    if (Vector3.Distance(m_LastTarget.CachedTransform.localPosition, targetableObject.CachedTransform.localPosition) > TimeLineActionData.ChainTransferRadius)
                    {
                        continue;
                    }

                    candidates.Add(targetableObject);
                }
            }

            private void ShowLineEffectAndDoImpact(TargetableObject from, TargetableObject to)
            {
                m_TransferCount--;
                m_LastTransferTime = Time.time;
                ShowLineEffect(from, to);
                m_TimeLineAction.DoImpact(m_TimeLineInstance, to, m_ImpactData);

                if (to == null || !to.IsAvailable)
                {
                    m_LastTarget = null;
                }
                else
                {
                    m_LastTarget = to;
                }
            }

            private int CompareCandidateTargets(TargetableObject x, TargetableObject y)
            {
                if (x.Id == y.Id)
                {
                    return 0;
                }

                var xHasBeenImpacted = ImpactData.ImpactedEntityIds.Contains(x.Id);
                var yHasBeenImpacted = ImpactData.ImpactedEntityIds.Contains(y.Id);

                if (!xHasBeenImpacted && yHasBeenImpacted)
                {
                    return -1;
                }

                if (xHasBeenImpacted && !yHasBeenImpacted)
                {
                    return 1;
                }

                if (m_LastTarget != null)
                {
                    if (x.Id == m_LastTarget.Id)
                    {
                        return 1;
                    }

                    if (y.Id == m_LastTarget.Id)
                    {
                        return -1;
                    }
                }

                var lastTargetLocalPosition = m_LastTarget.CachedTransform.localPosition;
                return Vector3.Distance(y.CachedTransform.localPosition, lastTargetLocalPosition).CompareTo(Vector3.Distance(x.CachedTransform.localPosition, lastTargetLocalPosition));
            }
        }

        private class ImpactData
        {
            public HashSet<int> EnteredEntityIds;
            public HashSet<int> ImpactedEntityIds;
            public float LastImpactTime;
            public float LastSoundTime;

            public ImpactData()
            {
                EnteredEntityIds = new HashSet<int>();
                ImpactedEntityIds = new HashSet<int>();

                LastImpactTime = 0f;
                LastSoundTime = 0f;
            }
        }

        private class ImpactData_Normal : ImpactData
        {
            public Dictionary<int, int> TargetEntityIdsToLineEffectIdsAboutToShow;
            public Dictionary<int, int> TargetEntityIdsToLineEffectIds;

            public ImpactData_Normal() : base()
            {
                TargetEntityIdsToLineEffectIds = new Dictionary<int, int>();
                TargetEntityIdsToLineEffectIdsAboutToShow = new Dictionary<int, int>();
            }
        }

        private class ImpactData_StrategyLightningChain : ImpactData
        {
            public Dictionary<Key, int> AboutToShowEffectIds;
            public Dictionary<Key, int> ShowingEffectIds;

            public ImpactData_StrategyLightningChain() : base()
            {
                AboutToShowEffectIds = new Dictionary<Key, int>();
                ShowingEffectIds = new Dictionary<Key, int>();
            }

            public class Key
            {
                public int FromId;
                public int ToId;

                public override int GetHashCode()
                {
                    return FromId.GetHashCode() + (ToId << 16).GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    var another = obj as Key;
                    if (null == another)
                    {
                        return false;
                    }

                    return FromId == another.FromId && ToId == another.ToId;
                }
            }
        }

        private StrategyBase CreateStrategy(EntityColliderTriggerImpactTimeLineActionDataAbstract.Strategy dataStrategy)
        {
            switch (dataStrategy)
            {
                case EntityColliderTriggerImpactTimeLineActionDataAbstract.Strategy.LightningChain:
                    return new StrategyLightningChain();
                case EntityColliderTriggerImpactTimeLineActionDataAbstract.Strategy.Normal:
                default:
                    return new StrategyNormal();
            }
        }
    }
}

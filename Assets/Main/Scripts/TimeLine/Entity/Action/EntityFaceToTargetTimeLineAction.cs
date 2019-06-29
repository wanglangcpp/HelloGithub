using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityFaceToTargetTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityFaceToTargetTimeLineActionData m_Data;

        private NpcCharacter m_NpcCharacter = null;
        private Building m_Building = null;
        private ICanHaveTarget m_CanHaveTargetOwner = null;
        private Transform m_TransformToRotate = null;
        private Transform m_RootTransform = null;

        public EntityFaceToTargetTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityFaceToTargetTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            m_CanHaveTargetOwner = timeLineInstance.Owner as ICanHaveTarget;

            m_NpcCharacter = m_CanHaveTargetOwner as NpcCharacter;
            m_Building = m_CanHaveTargetOwner as Building;
            if (m_NpcCharacter == null && m_Building == null)
            {
                Log.Warning("'{0}' is not a npc character.", timeLineInstance.Owner.Name);
                return;
            }

            if (m_NpcCharacter != null)
            {
                m_RootTransform = m_TransformToRotate = m_NpcCharacter.CachedTransform;
            }
            else if (m_Building != null)
            {
                m_TransformToRotate = m_Building.ShooterTransform;
                m_RootTransform = m_Building.CachedTransform;
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            DoFaceToTarget(elapseSeconds);
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {

        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        private void DoFaceToTarget(float elapseSeconds)
        {
            if (!m_CanHaveTargetOwner.HasTarget)
            {
                return;
            }

            Entity target = m_CanHaveTargetOwner.Target as Entity;
            if (target == null)
            {
                return;
            }

            Vector3 dir = target.CachedTransform.localPosition - m_RootTransform.localPosition;
            float angleToRotate = Vector3.Angle(m_TransformToRotate.forward, dir);

            if (angleToRotate == 0f)
            {
                return;
            }

            float rotationAmount = m_Data.AngularSpeed * elapseSeconds;
            if (m_Data.AngularSpeed <= 0f || rotationAmount >= angleToRotate)
            {
                m_TransformToRotate.LookAt2D(target.CachedTransform.localPosition.ToVector2());
            }
            else
            {
                var from = Quaternion.LookRotation(m_TransformToRotate.forward, m_TransformToRotate.up);
                var to = Quaternion.LookRotation(dir, m_TransformToRotate.up);
                var targetThisFrame = Quaternion.Lerp(from, to, rotationAmount / angleToRotate);
                m_TransformToRotate.rotation = Quaternion.Euler(0f, targetThisFrame.eulerAngles.y, 0f);
            }
        }
    }
}

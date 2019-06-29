using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityTransformTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityTransformTimeLineActionData m_Data;

        private bool m_StoppedByContact = false;
        private Vector3 m_CachedPosition;

        public EntityTransformTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityTransformTimeLineActionData;
        }

        public Vector2 Offset
        {
            get
            {
                return m_Data.Offset ?? Vector2.zero;
            }
        }

        private void TranslateByProportion(ITimeLineInstance<Entity> timeLineInstance, float proportion)
        {
            var characterOwner = (timeLineInstance.Owner as Character);
            var myNavMeshAgent = characterOwner.NavAgent;
            Vector3 forward = timeLineInstance.Owner.CachedTransform.TransformDirection(Vector3.forward);
            forward.y = 0f;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0f, -forward.x);
            Vector3 destPosition = Offset.x * right + Offset.y * forward;
            myNavMeshAgent.nextPosition = m_CachedPosition + Vector3.Lerp(Vector3.zero, destPosition, proportion);
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var characterOwner = timeLineInstance.Owner as Character;
            if (characterOwner == null)
            {
                Log.Error("The owner has to be a character, while you're using a '{0}'", timeLineInstance.Owner.GetType().Name);
                return;
            }

            var myHeroCharacter = characterOwner as MeHeroCharacter;
            if (myHeroCharacter != null && m_Data.AcceptDirInput && !GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled && !GameEntry.Data.Room.InRoom)
            {
                RotateByDirInput(myHeroCharacter);
            }

            m_CachedPosition = timeLineInstance.Owner.CachedTransform.localPosition;
            if (Duration <= 0f)
            {
                TranslateByProportion(timeLineInstance, 1f);
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            m_StoppedByContact = false;
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            if (m_StoppedByContact)
            {
                return;
            }

            var characterOwner = (timeLineInstance.Owner as Character);
            var myNavMeshAgent = characterOwner.NavAgent;
            if (m_Data.ContactCheckAllowance != null && GameEntry.SceneLogic.BaseInstanceLogic.ContactChecker.HasContact(myNavMeshAgent, m_Data.ContactCheckAllowance.Value, m_Data.ContactCheckAngle))
            {
                m_StoppedByContact = true;
                return;
            }

            TranslateByProportion(timeLineInstance, Duration > 0f ? (timeLineInstance.CurrentTime - StartTime) / Duration : 1f);

            if (characterOwner.Motion != null)
            {
                characterOwner.Motion.UpdateGradualPosSync(Duration, elapseSeconds);
            }
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            m_StoppedByContact = false;
        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        private void RotateByDirInput(MeHeroCharacter myHeroCharacter)
        {
            var pointToLookAt = myHeroCharacter.CachedTransform.position + myHeroCharacter.MoveTargetDirection;
            myHeroCharacter.CachedTransform.LookAt2D(pointToLookAt.ToVector2());
        }
    }
}

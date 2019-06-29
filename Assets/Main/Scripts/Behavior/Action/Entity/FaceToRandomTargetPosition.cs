using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class FaceToRandomTargetPosition : Action
    {
        private Entity m_Self;
        private ITargetPositionPool m_TargetPositionPool;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
            m_TargetPositionPool = m_Self as ITargetPositionPool;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                Log.Warning("Not in an instance");
                return TaskStatus.Failure;
            }

            if (m_Self == null || m_TargetPositionPool == null)
            {
                Log.Warning("Self is not valid.");
                return TaskStatus.Failure;
            }

            var targetPos = m_TargetPositionPool.SelectTargetPosition();

            var character = m_Self as Character;
            if (character != null)
            {
                m_Self.CachedTransform.LookAt2D(targetPos.ToVector2());
                return TaskStatus.Success;
            }

            var building = m_Self as Building;
            if (building != null)
            {
                building.ShooterTransform.LookAt2D(targetPos.ToVector2());
                return TaskStatus.Success;
            }

            Log.Warning("Self type '{0}' is not supported. ", m_Self.GetType());
            return TaskStatus.Failure;
        }
    }
}

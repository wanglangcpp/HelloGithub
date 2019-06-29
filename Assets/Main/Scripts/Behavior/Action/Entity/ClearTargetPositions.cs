using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class ClearTargetPositions : Action
    {
        private Entity m_Self = null;
        private ITargetPositionPool m_TargetPositionPool = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
            m_TargetPositionPool = m_Self as ITargetPositionPool;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null || m_TargetPositionPool == null)
            {
                Log.Warning("Self is not valid.");
                return TaskStatus.Failure;
            }

            m_TargetPositionPool.ClearTargetPositions();
            return TaskStatus.Success;
        }
    }
}

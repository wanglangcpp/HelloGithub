using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class FaceToPosition : Action
    {
        [SerializeField]
        private Vector2 m_TargetPosition = Vector2.zero;

        private Entity m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                Log.Warning("Not in an instance");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                Log.Warning("Self is not valid.");
                return TaskStatus.Failure;
            }

            var character = m_Self as Character;
            if (character != null)
            {
                m_Self.CachedTransform.LookAt2D(m_TargetPosition);
                return TaskStatus.Success;
            }

            var building = m_Self as Building;
            if (building != null)
            {
                building.ShooterTransform.LookAt2D(m_TargetPosition);
                return TaskStatus.Success;
            }

            Log.Warning("Self type '{0}' is not supported. ", m_Self.GetType());
            return TaskStatus.Failure;
        }
    }
}

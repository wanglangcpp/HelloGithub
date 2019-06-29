using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    internal class AddRotation : Action
    {
        [SerializeField]
        private float m_DeltaRotation = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            Entity selfEntity = Owner.GetComponent<Entity>();

            if (selfEntity == null)
            {
                return TaskStatus.Failure;
            }

            var oldRotation = selfEntity.CachedTransform.localRotation.eulerAngles;
            selfEntity.CachedTransform.localRotation = Quaternion.Euler(oldRotation.x, oldRotation.y + m_DeltaRotation, oldRotation.z);

            return TaskStatus.Success;
        }
    }
}

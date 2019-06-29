using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ResetCameraParams : Action
    {
        [SerializeField]
        private float m_Duration = 0f;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.IsAvailable || !GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            var cameraController = GameEntry.SceneLogic.BaseInstanceLogic.CameraController;
            if (!cameraController)
            {
                return TaskStatus.Failure;
            }

            cameraController.ResetParams(m_Duration);
            return TaskStatus.Success;
        }
    }
}

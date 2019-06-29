using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ChangeCameraParams : Action
    {
        [SerializeField]
        private bool m_DontChangeCameraOffset = false;

        [SerializeField]
        private Vector3 m_CameraOffset = Vector3.zero;

        [SerializeField]
        private bool m_DontChangeCenterOffset = false;

        [SerializeField]
        private float m_CenterOffset = 0f;

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

            cameraController.ChangeParams(m_DontChangeCameraOffset ? (Vector3?)null : m_CameraOffset,
                m_DontChangeCenterOffset ? (float?)null : m_CenterOffset,
                m_Duration);
            return TaskStatus.Success;
        }
    }
}

using BehaviorDesigner.Runtime.Tasks;
using GameFramework.Event;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    internal class StartCameraAnim : Action
    {
        [SerializeField]
        private string m_AnimName = string.Empty;

        [SerializeField]
        private float m_AboutToStart = 0.4f;

        [SerializeField]
        private float m_AboutToStop = 0.4f;

        private CameraController m_CameraController = null;

        private bool m_ShouldFail = false;
        private bool m_ShouldSucceed = false;

        public override void OnStart()
        {
            m_CameraController = GameEntry.SceneLogic.BaseInstanceLogic.CameraController;

            GameEntry.Event.Subscribe(EventId.CameraAnimCancel, OnCameraAnimCancel);
            GameEntry.Event.Subscribe(EventId.CameraAnimLoadFailure, OnCameraAnimLoadFailure);
            GameEntry.Event.Subscribe(EventId.CameraAnimStartToPlay, OnCameraAnimStartToPlay);

            if (m_CameraController != null)
            {
                m_ShouldFail = string.IsNullOrEmpty(m_AnimName) || !m_CameraController.StartAnimation(m_AnimName, m_AboutToStart, m_AboutToStop);
            }
        }

        public override void OnEnd()
        {
            GameEntry.Event.Unsubscribe(EventId.CameraAnimCancel, OnCameraAnimCancel);
            GameEntry.Event.Unsubscribe(EventId.CameraAnimLoadFailure, OnCameraAnimLoadFailure);
            GameEntry.Event.Unsubscribe(EventId.CameraAnimStartToPlay, OnCameraAnimStartToPlay);
        }

        private void OnCameraAnimStartToPlay(object sender, GameEventArgs e)
        {
            m_ShouldSucceed = true;
        }

        private void OnCameraAnimLoadFailure(object sender, GameEventArgs e)
        {
            m_ShouldFail = true;
        }

        private void OnCameraAnimCancel(object sender, GameEventArgs e)
        {
            m_ShouldFail = true;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_CameraController == null)
            {
                Log.Warning("Camera controller cannot be found.");
                return TaskStatus.Failure;
            }

            if (m_ShouldFail)
            {
                return TaskStatus.Failure;
            }

            if (m_ShouldSucceed)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
    }
}

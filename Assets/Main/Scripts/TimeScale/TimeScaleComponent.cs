using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间缩放组件。
    /// </summary>
    public partial class TimeScaleComponent : MonoBehaviour
    {
        [SerializeField]
        private InstanceTimeScaleTask m_InstanceTimeScaleTask = new InstanceTimeScaleTask();

        public void PauseGame()
        {
            GameEntry.Base.PauseGame();
        }

        public void ResumeGame()
        {
            GameEntry.Base.ResumeGame();
        }

        public void SetInstanceTimeScaleTask()
        {
            if (GameEntry.Base.IsGamePaused)
            {
                Debug.LogWarning("Can't Set Instance Time Scale because of GamePause in TimeScaleComponent !");
                return;
            }

            m_InstanceTimeScaleTask.InitInstanceTask();
        }

        private void Update()
        {
            if (m_InstanceTimeScaleTask.StartTasking && !GameEntry.Base.IsGamePaused)
            {
                m_InstanceTimeScaleTask.OnUpdate();
            }
        }

        public void ResetInstanceTimeScaleTask()
        {
            m_InstanceTimeScaleTask.Reset();
        }

        public void ResetNormalGameSpeed()
        {
            ResetInstanceTimeScaleTask();
            GameEntry.Base.ResetNormalGameSpeed();
        }
    }
}

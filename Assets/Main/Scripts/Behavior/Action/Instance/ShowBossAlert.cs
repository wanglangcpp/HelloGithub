using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowBossAlert : Action
    {
        [SerializeField]
        private float m_KeepTime = 3f;
        [SerializeField]
        private string m_BossNameKey = "";

        private bool m_Showing = false;
        public override void OnStart()
        {

        }
        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (m_Showing)
            {
                if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsShowingBossAlert())
                {
                    return TaskStatus.Running;
                }
                else
                {
                    m_Showing = false;
                    return TaskStatus.Success;
                }
            }
            else
            {
                if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ShowBossAlert(m_KeepTime, m_BossNameKey))
                {
                    m_Showing = true;
                    return TaskStatus.Running;
                }

                return TaskStatus.Failure;
            }
        }

        
    }
}

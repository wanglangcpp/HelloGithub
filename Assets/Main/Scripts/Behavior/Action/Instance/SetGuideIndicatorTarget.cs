using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    /// <summary>
    /// 设置向导指示标志的目标点。
    /// </summary>
    [TaskCategory("Game/Instance")]
    [TaskDescription("Set the target position of the guide indicator.")]
    public class SetGuideIndicatorTarget : Action
    {
        [SerializeField]
        private Vector2 m_TargetPosition = Vector2.zero;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.GuideIndicatorTarget = m_TargetPosition;
            return TaskStatus.Success;
        }
    }
}

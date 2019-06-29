using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class CheckCurrentGuidePointGroup : Conditional
    {
        [SerializeField]
        private int m_TargetGuidePointGroupKey = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            var guidePointSet = GameEntry.SceneLogic.BaseInstanceLogic.GuidePointSet;
            if (guidePointSet == null)
            {
                return TaskStatus.Failure;
            }

            return guidePointSet.ActiveGroupKey == m_TargetGuidePointGroupKey ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}

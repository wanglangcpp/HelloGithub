using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class HideOrShowNameBoard : Action
    {
        [SerializeField]
        private bool m_IsShowNameBoard = true;

        private TargetableObject m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is null.");
                return TaskStatus.Failure;
            }

            var nameBoard = GameEntry.Impact.GetNameBoard(m_Self);
            if (nameBoard == null)
            {
                GameFramework.Log.Warning("nameBoard is null.");
                return TaskStatus.Failure;
            }

            nameBoard.NameBoard.gameObject.SetActive(m_IsShowNameBoard);

            return TaskStatus.Success;
        }
    }
}
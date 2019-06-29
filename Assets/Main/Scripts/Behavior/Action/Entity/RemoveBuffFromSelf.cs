using BehaviorDesigner.Runtime.Tasks;
using GameFramework;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class RemoveBuffFromSelf : Action
    {
        private enum Mode
        {
            BuffType,
            BuffId,
        }

        [SerializeField]
        private Mode m_Mode = Mode.BuffType;

        [SerializeField]
        private BuffType[] m_BuffTypes = null;

        [SerializeField]
        private int[] m_BuffIds = null;

        private TargetableObject m_Self = null;
        private GameFrameworkAction m_RemoveBuffDelegate = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();

            if (m_BuffTypes == null)
            {
                m_BuffTypes = new BuffType[0];
            }

            if (m_BuffIds == null)
            {
                m_BuffIds = new int[0];
            }

            switch (m_Mode)
            {
                case Mode.BuffId:
                    m_RemoveBuffDelegate = RemoveBuffByIds;
                    break;
                case Mode.BuffType:
                default:
                    m_RemoveBuffDelegate = RemoveBuffByTypes;
                    break;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            m_RemoveBuffDelegate();

            return TaskStatus.Success;
        }

        private void RemoveBuffByTypes()
        {
            for (int i = 0; i < m_BuffTypes.Length; ++i)
            {
                m_Self.RemoveBuffByType(m_BuffTypes[i]);
            }
        }

        private void RemoveBuffByIds()
        {
            for (int i = 0; i < m_BuffIds.Length; ++i)
            {
                m_Self.RemoveBuffById(m_BuffIds[i]);
            }
        }
    }
}

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class SendRequest : Action
    {
        [SerializeField]
        private string[] m_Keys = null;

        [SerializeField]
        private string[] m_Values = null;

        public override TaskStatus OnUpdate()
        {
            if (m_Keys == null || m_Values == null || m_Keys.Length != m_Values.Length)
            {
                return TaskStatus.Failure;
            }

            var requestData = new Dictionary<string, string>();
            for (int i = 0; i < m_Keys.Length; ++i)
            {
                requestData.Add(m_Keys[i] ?? string.Empty, m_Values[i] ?? string.Empty);
            }

            GameEntry.LobbyLogic.OperationActivityRequest(requestData);
            return TaskStatus.Success;
        }
    }
}

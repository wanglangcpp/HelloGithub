using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class AddTargetPositions : Action
    {
        [SerializeField]
        private Vector2[] m_Positions = null;

        private Entity m_Self = null;
        private ITargetPositionPool m_TargetPositionPool = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
            m_TargetPositionPool = m_Self as ITargetPositionPool;
            if (m_Positions == null)
            {
                m_Positions = new Vector2[0];
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null || m_TargetPositionPool == null)
            {
                Log.Warning("Self is not valid.");
                return TaskStatus.Failure;
            }

            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < m_Positions.Length; ++i)
            {
                positions.Add(m_Positions[i].ToVector3());
            }

            m_TargetPositionPool.AddTargetPositions(positions);
            return TaskStatus.Success;
        }
    }
}

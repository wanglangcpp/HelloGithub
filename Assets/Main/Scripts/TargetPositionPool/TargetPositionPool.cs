using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Genesis.GameClient
{
    [Serializable]
    public class TargetPositionPool : ITargetPositionPool
    {
        [SerializeField]
        private List<Vector3> m_CandidateTargetPositions = new List<Vector3>();

        public void AddTargetPositions(IList<Vector3> positions)
        {
            m_CandidateTargetPositions.AddRange(positions);
        }

        public void ClearTargetPositions()
        {
            m_CandidateTargetPositions.Clear();
        }

        public Vector3 SelectTargetPosition()
        {
            if (m_CandidateTargetPositions.Count <= 0)
            {
                Log.Warning("No target position has been set.");
                return Vector3.zero;
            }

            return m_CandidateTargetPositions[Random.Range(0, m_CandidateTargetPositions.Count)];
        }
    }
}

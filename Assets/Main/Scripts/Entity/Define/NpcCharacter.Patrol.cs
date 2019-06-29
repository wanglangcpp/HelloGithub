using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class NpcCharacter
    {
        [SerializeField]
        private PatrolData m_PatrolData = null;

        [System.Serializable]
        private class PatrolData
        {
            [SerializeField]
            private List<Vector2> m_TargetPoints = new List<Vector2>();

            [SerializeField]
            private int m_CurrentIndex = 0;

            [SerializeField]
            private int m_PatrolTimes = -1;

            public int CurrentIndex
            {
                get
                {
                    return m_CurrentIndex;
                }
            }

            public int PatrolTimes
            {
                get
                {
                    return m_PatrolTimes;
                }

                set
                {
                    m_PatrolTimes = value >= -1 ? value : -1;
                }
            }

            public PatrolData()
            {
                m_PatrolTimes = -1;
            }

            public IList<Vector2> GetTargetPoints()
            {
                return m_TargetPoints;
            }

            public void AddTargetPoints(IList<Vector2> targetPoints)
            {
                m_TargetPoints.AddRange(targetPoints);
            }

            public void ClearTargetPoints()
            {
                m_TargetPoints.Clear();
            }

            public void ResetIndex()
            {
                m_CurrentIndex = 0;
            }

            public void IncrementIndex()
            {
                if (m_TargetPoints.Count <= 0)
                {
                    throw new System.InvalidOperationException("No target point can be found.");
                }

                m_CurrentIndex++;
                if (m_CurrentIndex < m_TargetPoints.Count)
                {
                    return;
                }

                m_CurrentIndex -= m_TargetPoints.Count;
                if (m_PatrolTimes > 0)
                {
                    m_PatrolTimes--;
                }
            }
        }

        public int PatrolPointIndex
        {
            get
            {
                return m_PatrolData.CurrentIndex;
            }
        }

        public IList<Vector2> GetPatrolPoints()
        {
            return m_PatrolData.GetTargetPoints();
        }

        public void AddPatrolPoints(IList<Vector2> patrolPoints)
        {
            m_PatrolData.AddTargetPoints(patrolPoints);
        }

        public void ClearPatrolPoints()
        {
            m_PatrolData.ClearTargetPoints();
        }

        public void ResetPatrolIndex()
        {
            m_PatrolData.ResetIndex();
        }

        public void IncrementPatrolIndex()
        {
            m_PatrolData.IncrementIndex();
        }

        public int PatrolTimes
        {
            get
            {
                return m_PatrolData.PatrolTimes;
            }

            set
            {
                m_PatrolData.PatrolTimes = value;
            }
        }
    }
}

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class KillNpc : Action
    {
        [SerializeField]
        private IntRange[] m_NpcIndexRanges = null;

        [SerializeField]
        private bool m_ForbiddenFurtherUse = true;

        public override void OnStart()
        {
            if (m_NpcIndexRanges == null)
            {
                m_NpcIndexRanges = new IntRange[0];
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            HashSet<int> toKill = new HashSet<int>();
            for (int i = 0; i < m_NpcIndexRanges.Length; i++)
            {
                var range = m_NpcIndexRanges[i];
                for (int index = range.MinValue; index <= range.MaxValue; ++index)
                {
                    toKill.Add(index);
                }
            }

            int[] toKillArray = new int[toKill.Count];
            toKill.CopyTo(toKillArray);
            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.KillNpcs(toKillArray, m_ForbiddenFurtherUse);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_NpcIndexRanges = null;
        }
    }
}

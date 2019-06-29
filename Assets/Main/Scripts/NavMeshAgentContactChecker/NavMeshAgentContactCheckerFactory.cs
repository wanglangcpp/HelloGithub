using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="UnityEngine.NavMeshAgent"/> 接触检测器工厂。
    /// </summary>
    public static class NavMeshAgentContactCheckerFactory
    {
        /// <summary>
        /// 创建接触检测器对象。
        /// </summary>
        /// <returns></returns>
        public static INavMeshAgentContactChecker Create()
        {
            return new NavMeshAgentContactChecker_BruteForce();
        }

        /// <summary>
        /// <see cref="UnityEngine.NavMeshAgent"/> 接触检测器的简单实现。
        /// </summary>
        private class NavMeshAgentContactChecker_BruteForce : INavMeshAgentContactChecker
        {
            private class AgentWrapper
            {
                private NavMeshAgent m_Agent;
                private Transform m_CachedTransform;

                internal NavMeshAgent Agent
                {
                    get
                    {
                        return m_Agent;
                    }
                }

                internal Transform CachedTransform
                {
                    get
                    {
                        return m_CachedTransform;
                    }
                }

                internal AgentWrapper(NavMeshAgent agent)
                {
                    if (agent == null)
                    {
                        Log.Error("Oops, you're trying to use a null agent to create an AgentWrapper.");
                        return;
                    }

                    m_Agent = agent;
                    m_CachedTransform = agent.transform;
                }

                public override bool Equals(object obj)
                {
                    var other = obj as AgentWrapper;
                    return Equals(other);
                }

                public bool Equals(AgentWrapper other)
                {
                    if (other == null)
                    {
                        return false;
                    }

                    return other.m_CachedTransform == m_CachedTransform && other.m_Agent == m_Agent;
                }

                public override int GetHashCode()
                {
                    return base.GetHashCode();
                }
            }

            private LinkedList<AgentWrapper> m_Agents = new LinkedList<AgentWrapper>();

            public void Clear()
            {
                m_Agents.Clear();
            }

            public bool HasContact(NavMeshAgent me, float allowance, float angle)
            {
                if (me == null)
                {
                    return false;
                }

                var myTransform = me.transform;

                for (var current = m_Agents.First; current != null; current = current.Next)
                {
                    var currentVal = current.Value;
                    var currentAgent = currentVal.Agent;
                    var currentTransform = currentVal.CachedTransform;

                    if (currentAgent == null || !currentAgent.enabled || currentAgent == me)
                    {
                        continue;
                    }

                    var myRealRadius = me.radius * myTransform.lossyScale.x;
                    var otherRealRadius = currentAgent.radius * currentTransform.lossyScale.x;
                    var myRealHeight = me.height * myTransform.lossyScale.y;
                    var otherRealHeight = currentAgent.height * currentTransform.lossyScale.y;

                    // 距离足够近，高差足够小，角度不太大。
                    if (Vector2.Distance(myTransform.position.ToVector2(), currentTransform.position.ToVector2()) <= myRealRadius + otherRealRadius + allowance
                        && Mathf.Abs(myTransform.position.y - currentTransform.position.y) < myRealHeight + otherRealHeight
                        && Vector3.Angle(myTransform.forward, currentTransform.position - myTransform.position) <= angle)
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Register(NavMeshAgent agent)
            {
                if (agent == null)
                {
                    Log.Warning("You cannot register a null agent.");
                }

                m_Agents.AddLast(new AgentWrapper(agent));
            }

            public bool Unregister(NavMeshAgent agent)
            {
                return m_Agents.Remove(new AgentWrapper(agent));
            }
        }
    }
}

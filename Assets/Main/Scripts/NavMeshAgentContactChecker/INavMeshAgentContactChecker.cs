using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="UnityEngine.NavMeshAgent"/> 接触检测器。
    /// </summary>
    public interface INavMeshAgentContactChecker
    {
        /// <summary>
        /// 清除所有 <see cref="UnityEngine.NavMeshAgent"/>
        /// </summary>
        void Clear();

        /// <summary>
        /// 注册代理对象。
        /// </summary>
        /// <param name="agent">导航网格代理。</param>
        void Register(NavMeshAgent agent);

        /// <summary>
        /// 注销代理对象。
        /// </summary>
        /// <param name="agent">待注销对象。</param>
        /// <returns>是否注销成功。</returns>
        bool Unregister(NavMeshAgent agent);

        /// <summary>
        /// 检查是否有接触。
        /// </summary>
        /// <param name="me">待检查对象。</param>
        /// <param name="allowance">距离裕量。</param>
        /// <param name="angle">有效夹角。在 0 - 180 之间。</param>
        /// <returns></returns>
        bool HasContact(NavMeshAgent me, float allowance, float angle);
    }
}

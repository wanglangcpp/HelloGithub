using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 数据表代理组件。
    /// </summary>
    /// <remarks>使 Lua 脚本能访问到需要的数据表。</remarks>
    public class DataTableProxyComponent : MonoBehaviour
    {
        private DataTableProxy_OperationActivity m_OperationActivity = null;
        public DataTableProxy_OperationActivity OperationActivity
        {
            get
            {
                return m_OperationActivity;
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_OperationActivity = new DataTableProxy_OperationActivity();
        }

        #endregion MonoBehaviour
    }
}

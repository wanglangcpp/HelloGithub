using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 来自 http://uwa4d.com 的优化诊断工具组件。
    /// </summary>
    public class UwaComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_TargetGO = null;

        public GameObject TargetGO
        {
            get
            {
                return m_TargetGO;
            }
        }
    }
}

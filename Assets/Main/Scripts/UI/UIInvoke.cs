using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class UIInvoke : MonoBehaviour
    {
        [SerializeField]
        private string m_MethodName = null;

        [SerializeField]
        private List<string> m_Params = null;

        public string MethodName
        {
            get
            {
                return m_MethodName;
            }
        }

        public string[] GetParams()
        {
            return m_Params.ToArray();
        }
    }
}

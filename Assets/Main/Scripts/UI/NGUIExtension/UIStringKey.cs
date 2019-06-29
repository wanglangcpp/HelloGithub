using UnityEngine;

namespace Genesis.GameClient
{
    public class UIStringKey : MonoBehaviour
    {
        [SerializeField]
        private string m_Key = null;

        public string Key
        {
            get
            {
                return m_Key ?? string.Empty;
            }
            set
            {
                m_Key = value;
            }
        }
    }
}

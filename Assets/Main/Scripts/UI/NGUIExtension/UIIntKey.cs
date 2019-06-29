using UnityEngine;

namespace Genesis.GameClient
{
    public class UIIntKey : MonoBehaviour
    {
        [SerializeField]
        private int m_Key = 0;

        public int Key
        {
            get
            {
                return m_Key;
            }
            set
            {
                m_Key = value;
            }
        }
    }
}

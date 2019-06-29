using UnityEngine;

namespace Genesis.GameClient
{
    public class MonsterPositionArrow : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_ArrowIcon = null;
        
        public UISprite ArrowIcon
        {
            get
            {
                return m_ArrowIcon;
            }
        } 
    }
}

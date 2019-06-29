using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class SystemMessage : MonoBehaviour
    {
        [SerializeField]
        private Animation m_Anim = null;

        [SerializeField]
        private UISprite m_MessageBg = null;

        [SerializeField]
        private UILabel m_MessageText = null;

        private void OnEnable()
        {
            m_Anim.Play();
        }

        public void Refresh(string content)
        {
            m_MessageText.text = content;
        }

        public void SetDepth(int depth)
        {
            m_MessageBg.depth = depth;
            m_MessageText.depth = depth + 1;
        }

        private void Update()
        {
            if (!m_Anim.isPlaying)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
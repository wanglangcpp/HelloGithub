using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        [Serializable]
        private class Request
        {
            [SerializeField]
            public UISprite m_SmallStar = null;

            [SerializeField]
            public UILabel m_Desc = null;

            [SerializeField]
            public UILabel m_UnDesc = null;

            private bool m_HasComplete = false;

            public GameObject GetSmallStar()
            {
                return m_SmallStar.gameObject;
            }

            public GameObject GetUDesc()
            {
                return m_UnDesc.gameObject;
            }

            public GameObject GetDesc()
            {
                return m_Desc.gameObject;
            }

            public void SetComplete(bool complete)
            {
                m_HasComplete = complete;
                m_SmallStar.gameObject.SetActive(complete);
                m_SmallStar.GetComponent<UISprite>().alpha = 0.03f;
            }

            public void SetIncompleteDesc(string text)
            {
                m_UnDesc.gameObject.SetActive(true);
                m_UnDesc.text = text;
            }

            public void SetDesc(string text)
            {
                if (m_HasComplete)
                {
                    m_Desc.gameObject.SetActive(true);
                    m_Desc.text = text;
                }
                else
                {
                    m_Desc.gameObject.SetActive(false);
                }
            }
        }
    }
}

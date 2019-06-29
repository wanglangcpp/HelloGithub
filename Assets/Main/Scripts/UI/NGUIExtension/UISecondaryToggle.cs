using UnityEngine;

namespace Genesis.GameClient
{
    public class UISecondaryToggle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ToggleChildrenParent = null;

        [SerializeField]
        private UIToggle[] m_ToggleChildren = null;

        [SerializeField]
        private float m_AnimDuration = 0.2f;

        private bool m_Folding = true;
        private Vector3 m_RetractingVec3 = new Vector3(1, 0.01f, 1);
        private Vector3 m_UnfoldingVec3 = new Vector3(1, 1, 1);

        private void Start()
        {
            m_Folding = true;
            TweenScale tweenScale = m_ToggleChildrenParent.GetComponent<TweenScale>();
            if (tweenScale == null)
            {
                tweenScale = m_ToggleChildrenParent.gameObject.AddComponent<TweenScale>();
            }
            tweenScale.duration = m_AnimDuration;
            tweenScale.from = m_RetractingVec3;
            tweenScale.to = m_UnfoldingVec3;
            tweenScale.AddOnFinished(OnTweenFinished);

            m_ToggleChildrenParent.SetActive(false);
        }

        public void OnValueChanged(bool value)
        {
            m_Folding = !value;
            TweenScale tweenScale = m_ToggleChildrenParent.GetComponent<TweenScale>();
            m_ToggleChildrenParent.SetActive(true);

            if (value)
            {
                m_ToggleChildren[0].Set(true);
                for (int i = 1; i < m_ToggleChildren.Length; i++)
                {
                    m_ToggleChildren[i].Set(false);
                }
                tweenScale.PlayForward();
            }
            else
            {
                for (int i = 0; i < m_ToggleChildren.Length; i++)
                {
                    m_ToggleChildren[i].Set(false);
                }
                tweenScale.PlayReverse();
            }
        }

        private void OnTweenFinished()
        {
            if (m_Folding)
            {
                m_ToggleChildrenParent.SetActive(false);
            }
        }
    }
}

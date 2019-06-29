using UnityEngine;

namespace Genesis.GameClient
{
    public class UIMessageAutoScrollLabel : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Root = null;

        [SerializeField]
        private Transform m_TargetTrans = null;

        [SerializeField]
        private float m_Speed = 500.0f;

        [SerializeField]
        private UILabel m_Label = null;

        [SerializeField]
        private UIPanel m_Panel = null;

        private float m_InitLabelTransformX = 0;
        private float m_Offset = 5.0f;
        private float m_PanelWith = 0;
        private bool m_IsInit = false;

        private void OnEnable()
        {
            if (!m_IsInit)
            {
                Init();
                m_IsInit = true;
            }

            OnRefresh();
        }

        private void Init()
        {
            m_PanelWith = m_Panel.width;
            m_InitLabelTransformX = m_TargetTrans.localPosition.x;
        }

        private void OnRefresh()
        {
            SystemChatData data = GameEntry.Data.Chat.ChatSystemBroadCastMsg;
            if (data == null)
            {
                m_Root.gameObject.SetActive(false);
                return;
            }

            m_Label.text = data.Message;
            TweenPosition tweenRo = gameObject.AddComponent<TweenPosition>();
            tweenRo.from = new Vector3(m_InitLabelTransformX, m_TargetTrans.localPosition.y, m_TargetTrans.localPosition.z);
            tweenRo.to = new Vector3(m_InitLabelTransformX - m_Label.width - m_Offset - m_PanelWith, m_TargetTrans.localPosition.y, m_TargetTrans.localPosition.z);
            tweenRo.duration = (m_Label.width + m_Offset + m_PanelWith) / (m_Speed > 0f ? m_Speed : 500f);
            tweenRo.SetOnFinished(TweenFinished);
            tweenRo.PlayForward();
        }

        public void TweenFinished()
        {
            TweenPosition tweenRo = gameObject.AddComponent<TweenPosition>();
            Destroy(tweenRo);
            OnRefresh();
        }
    }
}

using UnityEngine;

namespace Genesis.GameClient
{
    public class MeridianTipsAdapt : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ScrollView = null;

        [SerializeField]
        private GameObject m_RewardBg = null;

        [SerializeField]
        private GameObject m_Container = null;

        [SerializeField]
        private UIPanel m_RewardPanel = null;

        private Transform m_StarTrans = null;

        private static readonly float m_Offset = 30.0f;
        private static readonly float m_OffsetDownY = -30.0f;
        private static readonly float m_OffsetRightX = -15.0f;
        private static readonly float m_OffsetRightY = -15.0f;
        private static readonly float m_OffsetLeftX = 15.0f;
        private static readonly float m_OffsetLefttY = -15.0f;

        private void Start()
        {
        }

        public void ResetPosition(Transform starTrans)
        {
            m_StarTrans = starTrans;
            AdaptTips();
        }

        private void AdaptTips()
        {
            Vector4 VecDis = CalculateBorderDistance();
            if ((VecDis.x > m_RewardPanel.height + m_Offset) && (VecDis.z > m_RewardPanel.width / 2.0f) && (VecDis.w > m_RewardPanel.width / 2.0f))
            {
                transform.localPosition = new Vector3(m_StarTrans.localPosition.x, m_StarTrans.localPosition.y - m_RewardPanel.height / 2.0f - m_Offset, 0f);
                m_RewardBg.transform.localRotation = Quaternion.Euler(0f, 0f, 180.0f);
                m_Container.transform.localPosition = new Vector3(0f, m_OffsetDownY, 0f);
                return;
            }

            if ((VecDis.y > m_RewardPanel.height + m_Offset) && (VecDis.z > m_RewardPanel.width / 2.0f) && (VecDis.w > m_RewardPanel.width / 2.0f))
            {
                transform.localPosition = new Vector3(m_StarTrans.localPosition.x, m_StarTrans.localPosition.y + m_RewardPanel.height / 2.0f + m_Offset, 0f);
                m_RewardBg.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                m_Container.transform.localPosition = new Vector3(0f, 0f, 0f);
                return;
            }

            if (VecDis.z >= VecDis.w)
            {
                transform.localPosition = new Vector3(m_StarTrans.localPosition.x - m_RewardPanel.width / 2.0f - m_Offset, m_StarTrans.localPosition.y, 0f);
                m_RewardBg.transform.localRotation = Quaternion.Euler(0f, 0f, 90.0f);
                m_Container.transform.localPosition = new Vector3(m_OffsetRightX, m_OffsetRightY, 0f);
                return;
            }
            else
            {
                transform.localPosition = new Vector3(m_StarTrans.localPosition.x + m_RewardPanel.width / 2.0f + m_Offset, m_StarTrans.localPosition.y, 0f);
                m_RewardBg.transform.localRotation = Quaternion.Euler(0f, 0f, -90.0f);
                m_Container.transform.localPosition = new Vector3(m_OffsetLeftX, m_OffsetLefttY, 0f);
            }
        }

        private Vector4 CalculateBorderDistance()
        {
            Vector4 VecDistance = Vector4.zero;
            Vector3 pos = new Vector3(m_StarTrans.localPosition.x + m_ScrollView.transform.localPosition.x, m_StarTrans.localPosition.y + m_ScrollView.transform.localPosition.y, 0);
            float width = m_ScrollView.GetComponent<UIPanel>().width / 2;
            float height = m_ScrollView.GetComponent<UIPanel>().height / 2;
            float leftDis = width + pos.x;
            float rightDis = width - pos.x;
            float topDis = height + pos.y;
            float downDis = height - pos.y;
            VecDistance = new Vector4(topDis, downDis, leftDis, rightDis);

            return VecDistance;
        }
    }
}

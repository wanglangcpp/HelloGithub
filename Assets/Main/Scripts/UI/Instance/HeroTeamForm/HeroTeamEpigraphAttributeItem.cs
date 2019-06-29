using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroTeamEpigraphAttributeItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_NameLabel = null;

        [SerializeField]
        private UILabel m_Count = null;

        [SerializeField]
        private GameObject m_NewObj = null;

        public void Init(EpigraphData epData, bool isNew)
        {
            m_NewObj.SetActive(isNew);
            m_Count.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", epData.DTAttributeValue);
            if (epData.DTAttributeType > 0 && Constant.AttributeName.AttributeNameDics.Length > epData.DTAttributeType)
            {
                m_NameLabel.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[epData.DTAttributeType]);
            }
        }
    }
}

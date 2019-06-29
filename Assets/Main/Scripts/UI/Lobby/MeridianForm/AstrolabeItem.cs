using UnityEngine;

namespace Genesis.GameClient
{
    public class AstrolabeItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_DisIcon = null;

        [SerializeField]
        private UISprite m_Icon = null;

        [SerializeField]
        private GameObject m_LightCircle = null;

        [SerializeField]
        private GameObject m_FinishedLine = null;

        public bool MiddleCollider
        {
            set { gameObject.GetComponent<BoxCollider>().enabled = value; }
        }

        private string m_DisAstrolabeNameTail = "_dis";

        private readonly string[] AstrolabeIconName = new string[]
        {
            "icon_arise",
            "icon_taurus",
            "icon_gemini",
            "icon_cancer",
            "icon_leo",
            "icon_virgo",
            "icon_libra",
            "icon_scorpio",
            "icon_sagittarius",
            "icon_capricorn",
            "icon_aquarius",
            "icon_pisces"
        };

        public void InitItem(bool active, bool inMiddle, int index, Color color)
        {
            gameObject.GetComponent<BoxCollider>().enabled = inMiddle;
            m_Icon.spriteName = AstrolabeIconName[index];
            m_Icon.gameObject.SetActive(active);
            m_DisIcon.gameObject.SetActive(!active);
            m_FinishedLine.SetActive(active);
            m_LightCircle.SetActive(active || inMiddle);
            if (!active)
            {
                m_DisIcon.spriteName = AstrolabeIconName[index] + m_DisAstrolabeNameTail;
            }
            m_LightCircle.GetComponent<UISprite>().color = color;
            m_FinishedLine.GetComponent<UISprite>().color = color;
        }
    }
}

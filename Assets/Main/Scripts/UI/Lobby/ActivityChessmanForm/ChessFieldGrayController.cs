using UnityEngine;

namespace Genesis.GameClient
{
    public class ChessFieldGrayController : ChessFieldBaseController
    {
        [SerializeField]
        private NormalChessField m_Data = null;

        [SerializeField]
        private UIButton m_NormalButton = null;

        [SerializeField]
        private UIButton m_FreeButton = null;

        public override void RefreshData(ChessField data)
        {
            m_Data = data as NormalChessField;

            if (m_Data.IsOpened)
            {
                m_NormalButton.gameObject.SetActive(false);
                m_FreeButton.gameObject.SetActive(false);
            }
            else
            {
                m_NormalButton.gameObject.SetActive(!m_Data.IsFree);
                m_FreeButton.gameObject.SetActive(m_Data.IsFree);
            }
        }

        public void PlayOpenEffect()
        {
            if (m_EffectController != null)
            {
                m_EffectController.ShowEffect("Effect Bomb");
            }
        }
    }
}

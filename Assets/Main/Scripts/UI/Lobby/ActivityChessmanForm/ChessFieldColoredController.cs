using UnityEngine;

namespace Genesis.GameClient
{
    public class ChessFieldColoredController : ChessFieldBaseController
    {
        [SerializeField]
        private BattleChessField m_Data = null;

        [SerializeField]
        private UIButton m_NormalButton = null;

        [SerializeField]
        private UISprite m_SpriteAfterWinBattle = null;

        [SerializeField]
        private UILabel m_Number = null;

        [SerializeField]
        private UISprite m_NumberGreaterThanZero = null;

        public override void RefreshData(ChessField data)
        {
            m_Data = data as BattleChessField;

            if (!data.IsOpened)
            {
                m_NormalButton.gameObject.SetActive(true);
                m_SpriteAfterWinBattle.gameObject.SetActive(false);
            }
            else
            {
                m_NormalButton.gameObject.SetActive(false);
                m_SpriteAfterWinBattle.gameObject.SetActive(true);
                m_Number.text = m_Data.RemainingCount.ToString();
                m_NumberGreaterThanZero.gameObject.SetActive(m_Data.RemainingCount > 0);
            }
        }
    }
}

using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class MeridianForm
    {
        [SerializeField]
        private UILabel m_Progress = null;

        [SerializeField]
        private UILabel m_CurrentMeridianAstrolabe = null;

        [SerializeField]
        private UILabel m_CostCoin = null;

        [SerializeField]
        private UILabel m_PlayerMeriDianCount = null;

        [SerializeField]
        private UILabel m_AllAttackAstrolabe = null;

        [SerializeField]
        private UILabel m_AllDefenseAstrolabe = null;

        [SerializeField]
        private UILabel m_AllBloodAstrolabe = null;

        [SerializeField]
        private GameObject m_RewardsBg = null;

        [SerializeField]
        private GameObject m_AstrolabeBg = null;

        //private readonly string[] MeridianTitleName = new string[]
        //{
        //    "UI_TITLE_NAME_ARIES",
        //    "UI_TITLE_NAME_TAURUS",
        //    "UI_TITLE_NAME_GEMINI",
        //    "UI_TITLE_NAME_CANCER",
        //    "UI_TITLE_NAME_LEO",
        //    "UI_TITLE_NAME_VIRGO",
        //    "UI_TITLE_NAME_LIBRA",
        //    "UI_TITLE_NAME_SCORPIO",
        //    "UI_TITLE_NAME_SAGITTARIUS",
        //    "UI_TITLE_NAME_CAPRICORN",
        //    "UI_TITLE_NAME_AQUARIUS",
        //    "UI_TITLE_NAME_PISCES"
        //};

        private void RefreshReward()
        {
            int unlockStarCount = GameEntry.Data.Meridian.MeridianProgress;
            m_AllAttackAstrolabe.text = GameEntry.Data.Meridian.PhysicalAttack.ToString();
            m_AllDefenseAstrolabe.text = GameEntry.Data.Meridian.PhysicalDefense.ToString();
            m_AllBloodAstrolabe.text = GameEntry.Data.Meridian.MaxHP.ToString();
            if (unlockStarCount == 360)
            {
                m_RewardsBg.SetActive(false);
                m_AstrolabeBg.SetActive(false);
                return;
            }
            var dtMeridian = GameEntry.DataTable.GetDataTable<DRMeridian>();
            m_Progress.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", unlockStarCount % Constant.MaxMeridianAstrolabe, Constant.MaxMeridianAstrolabe);

            var drMeridian = dtMeridian.GetDataRow((unlockStarCount + 1));
            if (drMeridian == null)
            {
                Log.Error("RefreshReward Cannot find Meridian '{0}'.", (unlockStarCount + 1));
                return;
            }

            int AstrolabeNum = 0;
            int activateStar = GameEntry.Data.Meridian.MeridianProgress;
            int attributeType = (int)GameEntry.Data.Meridian.GetMeridianAttributeValue(activateStar, out AstrolabeNum);
            string AstrolabeType = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[attributeType]);
            m_CurrentMeridianAstrolabe.text = GameEntry.Localization.GetString("UI_TEXT_MERIDIAN_HERO_ATTRIBUTES_INCREASE", AstrolabeType, AstrolabeNum);
            int playerCoin = GameEntry.Data.Player.Coin;
            if (playerCoin >= drMeridian.CostCoin)
            {
                m_CostCoin.color = Color.white;
            }
            else
            {
                m_CostCoin.color = Color.red;
            }
            m_CostCoin.text = drMeridian.CostCoin.ToString();
            m_PlayerMeriDianCount.text = "X" + GameEntry.Data.Player.MeridianEnergy;
        }
    }
}

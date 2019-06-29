using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 翻翻棋对战副本准备界面。
    /// </summary>
    public class PvpActivityChessmanForm : BasePvpPrepareForm
    {
        [SerializeField]
        private Color m_UninteractableColor = Color.white;

        // Called by NGUI via reflection.
        public override void OnClickFightButton()
        {
            var chessBattleMe = GameEntry.Data.ChessBattleMe;

            if (!chessBattleMe.AnyHeroInTeamIsAlive)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_TEAM_DEAD"),
                    ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_TEAM"),
                    OnClickConfirm = o => { OnClickChangeHeroButton(); },
                });
                return;
            }

            if (!chessBattleMe.CaptainIsAlive)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_CAPTAIN_DEAD"),
                    ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_TEAM"),
                    OnClickConfirm = o => { OnClickChangeHeroButton(); },
                });
                return;
            }

            if (chessBattleMe.AnyHeroInTeamIsDead)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_DEAD_HERO_IN_TEAM"),
                    OnClickConfirm = o => { EnterBattle(); },
                });
                return;
            }

            EnterBattle();
        }

        // Called by NGUI via reflection.
        public override void OnClickChangeHeroButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.ChessBattle, });
        }

        protected override void RefreshMyData()
        {
            base.RefreshMyData();

            var chessBattleData = GameEntry.Data.ChessBattleMe;
            for (int i = 0; i < m_MyHeroes.Length; ++i)
            {
                var heroDisplay = m_MyHeroes[i];

                var heroTeamTypes = chessBattleData.HeroTeam.HeroType;
                if (i < heroTeamTypes.Count && heroTeamTypes[i] > 0)
                {
                    heroDisplay.Root.SetActive(true);
                    var heroTypeId = heroTeamTypes[i];
                    var lobbyHeroData = GameEntry.Data.LobbyHeros.GetData(heroTypeId);
                    var chessHeroStatus = chessBattleData.HeroesStatus.GetData(heroTypeId);
                    heroDisplay.Portrait.spriteName = UIUtility.GetHeroPortraitSpriteName(heroTypeId);
                    heroDisplay.CurHP.fillAmount = (chessHeroStatus == null ? 1f : (lobbyHeroData.MaxHP > 0 ? (float)chessHeroStatus.CurHP / lobbyHeroData.MaxHP : 0f));
                    heroDisplay.HeroName.text = lobbyHeroData.Name;
                    heroDisplay.SetColor(chessBattleData.HeroIsAlive(heroTypeId) ? Color.white : m_UninteractableColor);
                }
                else
                {
                    heroDisplay.HeroName.text = string.Empty;
                    heroDisplay.Root.SetActive(false);
                }
            }
        }

        protected override IList<int> GetMyHeroTeam()
        {
            return GameEntry.Data.ChessBattleMe.HeroTeam.HeroType;
        }

        protected override void RefreshOppData()
        {
            var chessBattleData = GameEntry.Data.ChessBattleEnemy;
            m_Opponent.Name.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", chessBattleData.Player.Name, chessBattleData.Player.Level);
            m_Opponent.Might.text = GameEntry.Data.ChessBattleEnemy.Player.TeamMight.ToString();
            m_OppPlayerId = chessBattleData.Player.Id;
            m_Opponent.Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(chessBattleData.Player.PortraitType));

            var heroesData = chessBattleData.HeroesData;

            int j = 0;
            for (int i = 0; i < heroesData.Data.Count; ++i)
            {
                var heroDisplay = m_OppHeroes[j];

                var lobbyHeroData = heroesData.Data[i];
                var chessHeroStatus = chessBattleData.HeroesStatus.GetData(lobbyHeroData.Type);

                if (chessHeroStatus != null && chessHeroStatus.CurHP <= 0)
                {
                    continue;
                }

                heroDisplay.Root.SetActive(true);
                heroDisplay.Portrait.spriteName = UIUtility.GetHeroPortraitSpriteName(lobbyHeroData.Type);
                heroDisplay.CurHP.fillAmount = (chessHeroStatus == null ? 1f : (lobbyHeroData.MaxHP > 0 ? (float)chessHeroStatus.CurHP / lobbyHeroData.MaxHP : 0f));
                heroDisplay.HeroName.text = lobbyHeroData.Name;

                m_OppHeroTeam.Add(lobbyHeroData.Type);
                ++j;
            }

            for (/* Empty initializer */; j < m_OppHeroes.Length; ++j)
            {
                var heroDisplay = m_OppHeroes[j];
                heroDisplay.HeroName.text = string.Empty;
                heroDisplay.Root.SetActive(false);
            }
        }

        protected override void EnterBattle()
        {
            var chessFieldIndex = GameEntry.Data.ChessBattleEnemy.ChessFieldIndex;
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenChessBoard, true);
            GameEntry.LobbyLogic.EnterChessBattle(chessFieldIndex);
        }
    }
}

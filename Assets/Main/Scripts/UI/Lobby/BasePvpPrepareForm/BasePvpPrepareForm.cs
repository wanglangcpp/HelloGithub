using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// PVP 副本准备界面的基类。
    /// </summary>
    public abstract class BasePvpPrepareForm : NGUIForm
    {
        [SerializeField]
        protected Player m_Me = null;

        [SerializeField]
        protected Player m_Opponent = null;

        [SerializeField]
        protected Hero[] m_MyHeroes = null;

        [SerializeField]
        protected Hero[] m_OppHeroes = null;

        protected List<int> m_OppHeroTeam = new List<int>();
        protected int m_OppPlayerId = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_OppHeroTeam.Clear();
            RefreshOppData();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            RefreshMyData();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        // Called by NGUI via reflection.
        public abstract void OnClickChangeHeroButton();

        // Called by NGUI via reflection.
        public virtual void OnClickFightButton()
        {
            EnterBattle();
        }

        // Called by NGUI via reflection.
        public void OnClickViewOppButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerInfoForm, new PlayerInfoDisplayData { PlayerId = m_OppPlayerId, HeroTypes = new List<int>(m_OppHeroTeam) });
        }

        protected virtual void RefreshMyData()
        {
            m_Me.Name.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", GameEntry.Data.Player.Name, GameEntry.Data.Player.Level);
            m_Me.Might.text = GetMight().ToString();
            m_Me.Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId());
        }

        private int GetMight()
        {
            int might = 0;
            var heroTeam = GetMyHeroTeam();
            for (int i = 0; i < heroTeam.Count; i++)
            {
                var hero = GameEntry.Data.LobbyHeros.GetData(heroTeam[i]);
                if (hero != null)
                {
                    might += hero.Might;
                }
            }
            return might;
        }

        protected virtual IList<int> GetMyHeroTeam()
        {
            return GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType;
        }

        protected abstract void RefreshOppData();

        protected abstract void EnterBattle();

        [Serializable]
        protected class Player
        {
            [SerializeField]
            public UISprite Portrait = null;

            [SerializeField]
            public UILabel Name = null;

            [SerializeField]
            public UILabel Might = null;
        }

        [Serializable]
        protected class Hero
        {
            [SerializeField]
            public GameObject Root = null;

            [SerializeField]
            public UISprite Portrait = null;

            [SerializeField]
            public UITexture PortraitTexture = null;

            [SerializeField]
            public UISprite CurHP = null;

            [SerializeField]
            public UILabel HeroName = null;

            [SerializeField]
            public UILabel HeroLevel = null;

            [SerializeField]
            public UISprite Element = null;

            private UIWidget[] m_CachedWidgets = null;

            private UIWidget[] CachedWidgets
            {
                get
                {
                    if (m_CachedWidgets == null)
                    {
                        m_CachedWidgets = Root.GetComponentsInChildren<UIWidget>(true);
                    }

                    return m_CachedWidgets;
                }
            }

            public void SetColor(Color c)
            {
                for (int i = 0; i < CachedWidgets.Length; ++i)
                {
                    CachedWidgets[i].color = c;
                }
            }
        }
    }
}

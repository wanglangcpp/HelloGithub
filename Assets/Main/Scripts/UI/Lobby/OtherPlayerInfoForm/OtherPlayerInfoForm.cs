using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家详细信息界面(新)
    /// </summary>
    public partial class OtherPlayerInfoForm : NGUIForm
    {
        [SerializeField]
        private ScrollViewCache m_HeroItems = null;

        [SerializeField]
        private UISprite m_PlayerIcon = null;

        [SerializeField]
        private UILabel m_PlayerVip = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_PlayerName = null;

        [SerializeField]
        private UILabel m_PlayerMight = null;

        [SerializeField]
        private UILabel m_PlayerId = null;

        [SerializeField]
        protected Transform m_PlatformRoot = null;

        [SerializeField]
        protected UIWidget m_HeroRotationRegion = null;

        [SerializeField]
        protected float m_HeroRotationSpeedFactor = 1f;

        [SerializeField]
        protected Transform m_PlatformModel = null;

        [SerializeField]
        protected Camera m_SecondaryCamera = null;

        [SerializeField]
        private UISprite m_HeroElement = null;

        [SerializeField]
        private GameObject m_HeroFight = null;

        [SerializeField]
        private UISprite[] m_HeroStars = null;

        [SerializeField]
        private UILabel m_HeroName = null;

        private FakeCharacter m_Character = null;
        private PlayerData m_PlayerData = null;
        private List<LobbyHeroData> m_Heroes = new List<LobbyHeroData>();
        private List<int> m_HeroTeam = new List<int>();
        private bool m_SwitchingHero = false;
        private bool m_IsRotatingHero = false;
        private Vector3 m_PlatformDefaultPosition;
        private Quaternion m_PlatformModelDefaultRotation;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_PlatformDefaultPosition = m_PlatformRoot.localPosition;
            m_PlatformModelDefaultRotation = m_PlatformModel.localRotation;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            InitUserData(userData);
            RefreshData();
            GameEntry.Impact.IncreaseHidingNameBoardCount();
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_SecondaryCamera.enabled = true;
            GameEntry.Event.Subscribe(EventId.SwipeStart, OnSwipeStart);
            GameEntry.Event.Subscribe(EventId.SwipeEnd, OnSwipeEnd);
            GameEntry.Event.Subscribe(EventId.Swipe, OnSwipe);
            m_PlatformRoot.localPosition = m_PlatformDefaultPosition;
            m_PlatformModel.localRotation = m_PlatformModelDefaultRotation;
            m_IsRotatingHero = false;
        }

        protected override void OnPause()
        {
            if (!GameEntry.IsAvailable) return;

            GameEntry.Event.Unsubscribe(EventId.SwipeStart, OnSwipeStart);
            GameEntry.Event.Unsubscribe(EventId.SwipeEnd, OnSwipeEnd);
            GameEntry.Event.Unsubscribe(EventId.Swipe, OnSwipe);
            m_SecondaryCamera.enabled = false;
            base.OnPause();
        }

        private void InitUserData(object userData)
        {
            var data = userData as OtherPlayerInfoDisplayData;
            if (data == null)
            {
                Log.Error("User data is invalid.");
                return;
            }
            m_PlayerData = data.PlayerData;
            m_Heroes = data.Heroes;
            m_HeroTeam = data.HeroTeam;
            m_Heroes.Sort(HeroComparer);
        }

        private int HeroComparer(LobbyHeroData a, LobbyHeroData b)
        {
            int aIndex = -1;
            int bIndex = -1;

            for (int i = 0; i < m_HeroTeam.Count; i++)
            {
                if (m_HeroTeam[i] == a.Type)
                {
                    aIndex = i;
                }

                if (m_HeroTeam[i] == b.Type)
                {
                    bIndex = i;
                }
            }

            if (aIndex >= 0 && bIndex < 0)
            {
                return -1;
            }

            if (bIndex >= 0 && aIndex < 0)
            {
                return 1;
            }

            if (aIndex < 0 && bIndex < 0)
            {
                if (b.StarLevel != a.StarLevel)
                {
                    return b.StarLevel.CompareTo(a.StarLevel);
                }

                if (b.Quality != a.Quality)
                {
                    return b.Quality.CompareTo(a.Quality);
                }

                if (b.Level != a.Level)
                {
                    return b.Level.CompareTo(a.Level);
                }

                if (b.Might != a.Might)
                {
                    return b.Might.CompareTo(a.Might);
                }
                return a.Type.CompareTo(b.Type);
            }

            return aIndex.CompareTo(bIndex);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            GameEntry.Impact.DecreaseHidingNameBoardCount();
            m_HeroItems.DestroyAllItems();
            ClearFakeCharacter();
        }

        private void RefreshData()
        {
            m_PlayerIcon.LoadAsync(UIUtility.GetPlayerPortraitIconId(m_PlayerData.PortraitType));
            m_PlayerVip.text = m_PlayerData.VipLevel.ToString();
            m_PlayerName.text = m_PlayerData.Name;
            m_LevelLabel.text = m_PlayerData.Level.ToString();
            m_PlayerMight.text = m_PlayerData.TeamMight.ToString();
            m_PlayerId.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERID", m_PlayerData.DisplayId);
            RefreshHeroItems();
            SelectHero(0);
        }

        public void RefreshHeroItems()
        {
            StartCoroutine(RefreshHeroItemsCo());
        }

        private IEnumerator RefreshHeroItemsCo()
        {
            yield return null;

            for (int i = 0; i < m_Heroes.Count; i++)
            {
                var item = m_HeroItems.GetOrCreateItem(i);
                item.InitHeroItem(m_Heroes[i], m_HeroTeam.Contains(m_Heroes[i].Type), i, SelectHero);
            }
            m_HeroItems.ResetPosition();
        }

        private void SelectHero(int index)
        {
            if (m_SwitchingHero)
            {
                return;
            }
            m_HeroElement.spriteName = UIUtility.GetElementSpriteName(m_Heroes[index].ElementId);
            m_HeroFight.SetActive(m_HeroTeam.Contains(m_Heroes[index].Type));
            UIUtility.SetStarLevel(m_HeroStars, m_Heroes[index].StarLevel);
            m_HeroName.text = m_Heroes[index].Name;

            for (int i = 0; i < m_HeroItems.Count; i++)
            {
                var item = m_HeroItems.GetOrCreateItem(i);
                item.IsSelect = index == i;
            }

            StartCoroutine(SelectHeroCo(index));
        }

        private IEnumerator SelectHeroCo(int index)
        {
            if (index >= m_Heroes.Count || index < 0)
            {
                Log.Warning("Error Hero Index.");
                yield break;
            }
            ClearFakeCharacter();
            ShowFakeCharacter(m_Heroes[index]);

            m_SwitchingHero = true;
            while (m_Character == null || !m_Character.IsAvailable)
            {
                yield return null;
            }
            m_SwitchingHero = false;
        }

        protected void ShowFakeCharacter(LobbyHeroData heroData)
        {
            FakeCharacter.Show(heroData.CharacterId, heroData.Type, m_PlatformRoot, null, 0f, FakeCharacterData.ActionOnShow.Debut);
        }

        protected void ClearFakeCharacter()
        {
            if (m_Character != null && m_Character.IsAvailable)
            {
                GameEntry.Entity.HideEntity(m_Character.Entity);
                m_Character = null;
            }
        }

        public void OnClickHeroInteractor()
        {
            if (m_SwitchingHero || m_Character == null || !m_Character.IsAvailable)
            {
                return;
            }

            m_Character.Interact();
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<OtherHeroIconItem>
        {

        }
    }
}

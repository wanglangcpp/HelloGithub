using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class HeroInfoBaseForm : NGUIForm
    {
        [Serializable]
        private class HeroAttributes
        {
            [SerializeField]
            public UILabel MaxHpLabel = null;

            [SerializeField]
            public UILabel AttackLabel = null;

            [SerializeField]
            public UILabel DefenseLabel = null;

            [SerializeField]
            public UILabel CriticalHitProbLabel = null;

            [SerializeField]
            public UILabel CriticalHitRateLabel = null;

            [SerializeField]
            public UILabel SpeedLabel = null;

            [SerializeField]
            public UILabel CDAfterChangeHeroLabel = null;

            [SerializeField]
            public UILabel RecoverHPLabel = null;
        }

        [SerializeField]
        protected Camera m_SecondaryCamera = null;

        [SerializeField]
        protected UIWidget m_HeroRotationRegion = null;

        [SerializeField]
        protected float m_HeroRotationSpeedFactor = 1f;

        [SerializeField]
        protected Transform m_PlatformModel = null;

        [SerializeField]
        protected Transform m_PlatformRoot = null;

        [SerializeField]
        protected float m_PlatformWidth = 0f;

        [SerializeField]
        private HeroAttributes m_HeroAttributes = null;

        protected bool m_SwitchingHero = false;
        protected Vector3 m_PlatformDefaultPosition;
        protected Quaternion m_PlatformModelDefaultRotation;
        protected bool m_IsRotatingHero = false;
        protected FakeCharacter m_Character = null;
        private BaseLobbyHeroData m_HeroData = null;

        virtual protected BaseLobbyHeroData HeroData
        {
            set
            {
                m_HeroData = value;
            }
            get
            {
                return m_HeroData;
            }
        }

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

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) return;
            ClearFakeCharacter();
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);

            GameEntry.Impact.DecreaseHidingNameBoardCount();
            base.OnClose(userData);
        }

        virtual protected void ShowFakeCharacter()
        {
            var heroData = HeroData;
            FakeCharacter.Show(heroData.CharacterId, heroData.Type, m_PlatformRoot, null, 0f, FakeCharacterData.ActionOnShow.Debut);
        }

        virtual protected void ClearFakeCharacter()
        {
            if (m_Character != null && m_Character.IsAvailable)
            {
                GameEntry.Entity.HideEntity(m_Character.Entity);
                m_Character = null;
            }
        }

        virtual public void OnClickHeroInteractor()
        {
            GameEntry.DisplayModel.OnHeroInteractor();
            if (m_SwitchingHero || m_Character == null || !m_Character.IsAvailable)
            {
                return;
            }

            m_Character.Interact();
        }

        protected void RefreshHeroAttribute()
        {
            var heroData = HeroData;
            m_HeroAttributes.MaxHpLabel.text = heroData.MaxHP.ToString();
            m_HeroAttributes.AttackLabel.text = heroData.PhysicalAttack.ToString();
            m_HeroAttributes.DefenseLabel.text = heroData.PhysicalDefense.ToString();
            m_HeroAttributes.CriticalHitProbLabel.text = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", heroData.CriticalHitProb);
            m_HeroAttributes.CriticalHitRateLabel.text = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", heroData.CriticalHitRate);
            m_HeroAttributes.SpeedLabel.text = UIUtility.GetAttributeValueStr(AttributeType.Speed, heroData.Speed);
            m_HeroAttributes.CDAfterChangeHeroLabel.text = GameEntry.Localization.GetString("UI_TEXT_ATTRIBUTE_SECOND", heroData.CDAfterChangeHero);
            m_HeroAttributes.RecoverHPLabel.text = UIUtility.GetAttributeValueStr(AttributeType.RecoverHP, heroData.RecoverHP);
        }
    }
}

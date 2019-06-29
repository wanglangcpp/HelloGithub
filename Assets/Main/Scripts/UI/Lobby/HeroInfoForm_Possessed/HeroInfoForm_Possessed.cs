using System.Collections.Generic;
using GameFramework;
using System.Collections;
using UnityEngine;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 已激活英雄详情界面。
    /// </summary>
    public partial class HeroInfoForm_Possessed : HeroInfoBaseForm
    {
        [SerializeField]
        private UILabel m_HeroName = null;

        [SerializeField]
        private UILabel m_HeroGs = null;

        [SerializeField]
        private UISprite m_Element = null;

        [SerializeField]
        private UISprite[] m_Stars = null;

        [SerializeField]
        private UIToggle[] m_Tabs = null;

        [SerializeField]
        private GameObject[] m_Reminder = null;

        [SerializeField]
        private float m_SwitchOneWayTime = .15f;

        [SerializeField]
        private UIButton m_PrevHeroButton = null;

        [SerializeField]
        private UIButton m_NextHeroButton = null;

        [SerializeField]
        private UILabel m_DatailDescription = null;

        [SerializeField]
        private GameObject m_DetailPanel = null;

        [SerializeField]
        private GameObject m_OtherPanel = null;

        [SerializeField]
        private UITexture m_ModelTexture = null;

        private Dictionary<TabType, GameFrameworkFunc<bool>> m_RefreshFuncs = new Dictionary<TabType, GameFrameworkFunc<bool>>();
        private Dictionary<TabType, GameFrameworkFunc<bool>> m_CloseFuncs = new Dictionary<TabType, GameFrameworkFunc<bool>>();
        private int m_IndexInAllHeroes = -1;
        private List<BaseLobbyHeroData> m_AllHeroes = null;
        private float m_PlatformLeftMostPosX;
        private float m_PlatformRightMostPosX;

        protected override BaseLobbyHeroData HeroData
        {
            get
            {
                return m_AllHeroes[m_IndexInAllHeroes];
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.IncreaseHeroQualityLevel, OnIncreaseHeroQualityLevel);
            GameEntry.Event.Subscribe(EventId.NewGearStrengthen, OnStrengthenNewGearLevel);
            GameEntry.Event.Subscribe(EventId.NewGearQualityLevelUp, OnNewGearQualityUp);
            GameEntry.Event.Subscribe(EventId.HerostarLevelUp, OnStarLevelUpSuccess);
            GameEntry.Event.Subscribe(EventId.ItemDataChanged, OnItemDataChanged);
            GameEntry.Event.Subscribe(EventId.HeroQualityItemDataChange, OnHeroQualityItemDataChange);
            GameEntry.Event.Subscribe(EventId.HeroSkillDataChanged, OnHeroSkillDataChanged);
            GameEntry.Event.Subscribe(EventId.LobbyHeroDataChanged, OnHeroDataChanged);
            var userDataDict = userData as HeroInfoDisplayData;
            m_IndexInAllHeroes = userDataDict.IndexInAllHeroes;
            m_AllHeroes = userDataDict.AllHeroes;
            InitPlatformBothSidePos();
            GameEntry.Impact.IncreaseHidingNameBoardCount();
            InitRefreshFuncData();
            ShowCanOpenTabs();
            RefreshData();
            m_DetailPanel.SetActive(false);
            m_OtherPanel.SetActive(true);
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);

            if (m_Tabs[0].value)
            {
                //ShowExperienceItemEffect();
            }
            else if (m_Tabs[1].value)
            {
                //ShowUpgradeStarEffect();
            }
            else if (m_Tabs[2].value)
            {
                // 这个Tab签没有一直显示的特效
            }
            else if (m_Tabs[3].value)
            {
                //ShowBadgeEffect();
            }

        }

        protected override void OnClose(object userData)
        {
            GameEntry.DisplayModel.HideDisplayModel();
            if (!GameEntry.IsAvailable) return;

            GameEntry.Impact.DecreaseHidingNameBoardCount();
            GameEntry.Event.Unsubscribe(EventId.IncreaseHeroQualityLevel, OnIncreaseHeroQualityLevel);
            GameEntry.Event.Unsubscribe(EventId.NewGearStrengthen, OnStrengthenNewGearLevel);
            GameEntry.Event.Unsubscribe(EventId.NewGearQualityLevelUp, OnNewGearQualityUp);
            GameEntry.Event.Unsubscribe(EventId.HerostarLevelUp, OnStarLevelUpSuccess);
            GameEntry.Event.Unsubscribe(EventId.ItemDataChanged, OnItemDataChanged);
            GameEntry.Event.Unsubscribe(EventId.HeroQualityItemDataChange, OnHeroQualityItemDataChange);
            GameEntry.Event.Unsubscribe(EventId.HeroSkillDataChanged, OnHeroSkillDataChanged);
            GameEntry.Event.Unsubscribe(EventId.LobbyHeroDataChanged, OnHeroDataChanged);
            m_RefreshFuncs.Clear();
            m_CloseFuncs.Clear();
            base.OnClose(userData);
        }

        private void OnHeroDataChanged(object sender, GameEventArgs e)
        {
            RefreshHeroAttribute();
            RefreshHeroData();
            Log.Debug("HeroInfoForm_Possessed.OnHeroDataChanged");
            GameEntry.NoviceGuide.CheckNoviceGuide(this);
        }

        private void OnItemDataChanged(object sender, GameEventArgs e)
        {
            RefreshStarLevelUp();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateHeroStrengthenAnim();
        }

        private void RefreshData()
        {
            StartCoroutine(RefreshDataCo());
            RefreshHero();
        }

        private void RefreshHero()
        {
            RefreshHeroAttribute();
            RefreshHeroData();
            for (int i = 0; i < m_Tabs.Length; i++)
            {
                if (m_Tabs[i].value)
                {
                    TabValueChanged(true, i);
                    break;
                }
            }
            ShowFakeCharacter();
        }

        private void ShowCanOpenTabs()
        {
            for (int i = 0; i < m_Tabs.Length; i++)
            {
                bool isOpen = GameEntry.OpenFunction.CheckHeroToggle((TabType)i);
                if (!isOpen)
                {
                    m_Tabs[i].GetComponent<BoxCollider>().enabled = false;
                    m_Tabs[i].gameObject.transform.Find("Btn Dis/Icon").gameObject.GetComponent<UISprite>().color = new Color(208f / 255f, 98f / 255f, 98 / 255f, 255 / 255f);
                    m_Tabs[i].gameObject.transform.Find("Btn Dis/Btn Text Dis").gameObject.GetComponent<UILabel>().color = new Color(127f / 255f, 123f / 255f, 123f / 255f, 255 / 255f);
                }
            }
        }

        private void RefreshHeroData()
        {
            var heroData = HeroData;
            m_HeroName.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", heroData.Name, heroData.Level);
            m_HeroName.color = Constant.Quality.Colors[(int)heroData.Quality];
            m_HeroGs.text = GameEntry.Localization.GetString("UI_TEXT_HERO_GS_TEXT", heroData.Might);
            m_Element.spriteName = UIUtility.GetElementSpriteName(heroData.ElementId);
            int starLevel = heroData.StarLevel;
            UIUtility.SetStarLevel(m_Stars, starLevel);
            UIUtility.SetStarLevel(m_RightStars, starLevel);
            m_DatailDescription.text = GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(heroData.DetailDescription));
        }

        private void InitRefreshFuncData()
        {
            m_RefreshFuncs.Add(TabType.StarLevel, RefreshStarLevel);
            m_RefreshFuncs.Add(TabType.Strengthen, RefreshStrengthen);
            m_RefreshFuncs.Add(TabType.NewGear, RefreshNewGear);
            m_RefreshFuncs.Add(TabType.Skill, RefreshSkill);
            m_CloseFuncs.Add(TabType.StarLevel, CloseStarLevel);
            m_CloseFuncs.Add(TabType.Strengthen, CloseStrengthen);
            m_CloseFuncs.Add(TabType.NewGear, CloseNewGear);
            m_CloseFuncs.Add(TabType.Skill, CloseSkill);
        }

        private IEnumerator RefreshDataCo()
        {
            RefreshPrevNextButtons();
            //m_SwitchingHero = true;
            //while (m_Character == null || !m_Character.IsAvailable)
            //{
            yield return null;
            //}
            //m_SwitchingHero = false;
        }

        public void TabValueChanged(bool value, int type)
        {
            //value = !GameEntry.OpenFunction.CheckHeroToggle((TabType)type);
            m_Tabs[type].value = value;

            if (!value)
            {
                if (!m_CloseFuncs[(TabType)type]())
                {
                    Log.Warning("CloseFuncs TabValue Function Error,Tab Type is {0}.", ((TabType)type).ToString());
                    return;
                }
                return;
            }

            if (!m_RefreshFuncs[(TabType)type]())
            {
                Log.Warning("Refresh TabValue Function Error,Tab Type is {0}.", ((TabType)type).ToString());
                return;
            }
        }

        // Called by NGUI via reflection
        public void OnClickNextHeroBtn()
        {
            if (m_SwitchingHero)
            {
                return;
            }
            ShowNextHero();
        }

        // Called by NGUI via reflection
        public void OnClickPrevHeroBtn()
        {
            if (m_SwitchingHero)
            {
                return;
            }
            ShowPrevHero();
        }

        private void InitPlatformBothSidePos()
        {
            var secondaryCameraTransform = m_SecondaryCamera.transform;
            var platformDepth = m_SecondaryCamera.WorldToViewportPoint(m_PlatformRoot.position).z;
            var left = m_SecondaryCamera.ViewportToWorldPoint(new Vector3(0, 0, platformDepth));
            var right = m_SecondaryCamera.ViewportToWorldPoint(new Vector3(1, 0, platformDepth));
            var deltaX = m_PlatformWidth * m_PlatformRoot.localScale.x;
            m_PlatformLeftMostPosX = secondaryCameraTransform.InverseTransformPoint(left).x - deltaX;
            m_PlatformRightMostPosX = secondaryCameraTransform.InverseTransformPoint(right).x + deltaX;
        }

        private void ShowNextHero()
        {
            StartCoroutine(SwitchHeroCo(m_IndexInAllHeroes + 1, m_PlatformLeftMostPosX, m_PlatformRightMostPosX));
        }

        private void ShowPrevHero()
        {
            StartCoroutine(SwitchHeroCo(m_IndexInAllHeroes - 1, m_PlatformRightMostPosX, m_PlatformLeftMostPosX));
        }

        private IEnumerator SwitchHeroCo(int index, float firstWayDestX, float otherSideX)
        {
            if (GameEntry.DisplayModel.IsLoading)
            {
                yield break;
            }
            if (m_SwitchingHero)
            {
                yield break;
            }

            if (index < 0 || index >= m_AllHeroes.Count)
            {
                yield break;
            }

            //m_SwitchingHero = true;

            var speed = (firstWayDestX - m_PlatformDefaultPosition.x) / (m_SwitchOneWayTime > 0f ? m_SwitchOneWayTime : 60f);

            while ((firstWayDestX - m_PlatformRoot.localPosition.x) * speed > 0)
            {
                m_PlatformRoot.AddLocalPositionX(speed * Time.unscaledDeltaTime);
                yield return null;
            }

            ClearFakeCharacter();
            m_IndexInAllHeroes = index;
            RefreshPrevNextButtons();
            m_PlatformRoot.SetLocalPositionX(otherSideX);
            m_PlatformModel.localRotation = m_PlatformModelDefaultRotation;
            RefreshHero();

            //while (m_Character == null || !m_Character.IsAvailable)
            //{
            //    yield return null;
            //}

            //while ((m_PlatformDefaultPosition.x - m_PlatformRoot.localPosition.x) * speed > 0)
            //{
            //    m_PlatformRoot.AddLocalPositionX(speed * Time.unscaledDeltaTime);
            //    if ((m_PlatformDefaultPosition.x - m_PlatformRoot.localPosition.x) * speed < 0)
            //    {
            //        m_PlatformRoot.SetLocalPositionX(m_PlatformDefaultPosition.x);
            //    }

            //    yield return null;
            //}

            //m_Character.Debut();
            m_SwitchingHero = false;
        }

        private void RefreshPrevNextButtons()
        {
            m_PrevHeroButton.gameObject.SetActive(true);
            m_NextHeroButton.gameObject.SetActive(true);
            m_PrevHeroButton.isEnabled = m_IndexInAllHeroes > 0;
            m_NextHeroButton.isEnabled = m_IndexInAllHeroes < m_AllHeroes.Count - 1;
        }

        public void OnClickOpenDetailButton()
        {
            m_DetailPanel.SetActive(true);
            m_OtherPanel.SetActive(false);
        }

        public void OnClickCloseDetailButton()
        {
            m_DetailPanel.SetActive(false);
            m_OtherPanel.SetActive(true);
        }

        public override void OnClickBackButton()
        {
            base.OnClickBackButton();
        }

        override protected void ShowFakeCharacter()
        {
            GameEntry.DisplayModel.DisplayModel(3, HeroData.Type, m_ModelTexture);
        }

        override protected void ClearFakeCharacter()
        {
            base.ClearFakeCharacter();
        }

        override public void OnClickHeroInteractor()
        {
            base.OnClickHeroInteractor();
        }
    }
}

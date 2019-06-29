using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BattleForm : NGUIForm
    {
        [SerializeField]
        private GameObject m_NormalRoot = null;

        [SerializeField]
        private List<Color> m_BossHPBarColors = null;

        [SerializeField]
        private UILabel m_TimeText = null;

        [SerializeField]
        private UILabel m_LastTimeText = null;

        [SerializeField]
        private GameObject m_TimeCommonBg = null;

        [SerializeField]
        private GameObject m_TimeLastBg = null;

        [SerializeField]
        private GameObject m_TimerRoot = null;

        [SerializeField]
        private GameObject m_DropItemPanel = null;

        [SerializeField]
        private UILabel m_DropItemText = null;

        [SerializeField]
        private UIButton m_Pause = null;

        [SerializeField]
        private GameObject m_PausePanel = null;

        [SerializeField]
        private UIButton m_RequestButton = null;

        [SerializeField]
        private UILabel[] m_RequestDescription = null;

        [SerializeField]
        private GameObject[] m_RequestStars = null;

        [SerializeField]
        private UIToggle m_Auto = null;

        [SerializeField]
        private GameObject m_BossHPPanel = null;

        [SerializeField]
        private GoodsView m_BossElement = null;

        [SerializeField]
        private UIProgressBar m_BossHPProgressBar = null;

        [SerializeField]
        private GameObject m_BeforeBossHPBehindBar = null;

        [SerializeField]
        private UIProgressBar m_BossSteadyProgressBar = null;

        [SerializeField]
        private UIProgressBar m_BossSteadyRecoverProgressBar = null;

        [SerializeField]
        private UISprite m_BeforeNormalBossIcon = null;

        [SerializeField]
        private UISprite m_BeforeRedBossIcon = null;

        [SerializeField]
        private UISprite m_BehindNormalBossIcon = null;

        [SerializeField]
        private UISprite m_BehindRedBossIcon = null;

        [SerializeField]
        private UISprite m_AnimNormalBossIcon = null;

        [SerializeField]
        private UISprite m_AnimRedBossIcon = null;

        [SerializeField]
        private UILabel m_BossName = null;

        [SerializeField]
        private UILabel m_BossHPTimes = null;

        [SerializeField]
        private Animation m_BossHpAnim = null;

        [SerializeField]
        private GameObject m_BossAlertPanel = null;

        [SerializeField]
        private UILabel m_BossAlertName = null;

        [SerializeField]
        private Animation m_BossAlertAnimation = null;

        [SerializeField]
        private GameObject m_SkillControllerPanel = null;

        [SerializeField]
        private SkillButton[] m_SkillButtons = null;

        private bool[] m_SkillButtonsCDComplete = null;

        [SerializeField]
        private UIToggle m_MusicIsUnmuted = null;

        [SerializeField]
        private UIToggle m_SoundIsUnmuted = null;

#pragma warning disable 0414

        [SerializeField]
        private GameObject m_AngerAndHerosPanel = null;

#pragma warning restore 0414

        [SerializeField]
        private HeroButton[] m_HeroButtons = null;

        private bool[] m_HeroButtonsCDComplete = null;

        [SerializeField]
        private GameObject m_PropagandaGroup = null;

        [SerializeField]
        private UISprite m_PropagatorPortrait = null;

        [SerializeField]
        private UILabel m_PropagatorName = null;

        [SerializeField]
        private UILabel m_PropagandaContent = null;

        [SerializeField]
        private HeroInBattle m_HeroInBattle = null;

        [SerializeField]
        private ContinualTapSkillNormalDisplay m_ContinualTapSkillNormalDisplay = null;

        [SerializeField]
        private GameObject m_ContinualTapSkillItemTempate = null;

        [SerializeField]
        private float m_ContinualTapSkillItemOffset = 100f;

        [SerializeField]
        private Transform m_ChargeSkillProgressBarRoot = null;

        [SerializeField]
        private GameObject m_ChargeSkillProgressBarTemplete = null;

        private ForceStorage m_ChargeSkillProgressBar = null;

        [SerializeField]
        private DodgeButton m_DodgeButton = null;

        [SerializeField]
        private WaitingForStart m_WaitingForStart = null;

        [SerializeField]
        private UILabel m_DropCoinsLabel = null;

        [SerializeField]
        private GameObject m_DropCoinsRoot = null;

        [SerializeField]
        private GameObject m_Shield = null;

        [SerializeField]
        private float m_ChargeSkillProgressBarPositionX = 0;

        [SerializeField]
        private float m_ChargeSkillProgressBarPositionY = 0;

        private const string PropagandaAnimationInName = "NpcPropagandaIn";
        private const string PropagandaAnimationOutName = "NpcPropagandaOut";
        private const string DisplayRequestSettingKey = "BattleForm.DisplayRequest";
        private const int BossAnimFrameCount = 4;
        private const string InstanceMovieInAnimName = "InstanceMovieIn";
        private const string InstanceMovieOutAnimName = "InstanceMovieOut";
        private TargetableObjectData m_BossData = null;
        private CharacterData m_BossCharacterData = null;
        private int m_BossHPBarCount = 0;
        private IDropGoodsInstance m_DropGoodsInstanceLogic = null;
        private IPropagandaInstance m_PropagandaInstanceLogic = null;
        private IDataTable<DRHero> m_DTHero = null;
        private IDataTable<DRSkillGroup> m_DTSkillGroup = null;
        private IDataTable<DRSkill> m_DTSkill = null;
        private IDataTable<DRIcon> m_DTIcon = null;
        private int m_LastHPBarLeftCount = -1;
        private bool m_SwitchFirstHero = true;
        private bool m_BossHpAnimStart = false;
        private bool m_BossHPAnimEnd = false;
        private SkillContinualTapDisplayer m_SkillContinualTapDisplayer = null;
        private Ping m_Ping = null;
        private bool m_CachedAutoFightIsEnabled = false;

        private int m_PerformingChargeSkillId = int.MinValue;

        private bool m_HasSkipOrAbandonUI = false;
        
        protected override bool BackButtonIsAvailable
        {
            get
            {
                return false;
            }
        }

        public bool HasOpenShield
        {
            set
            {
                m_Shield.SetActive(value);
            }
        }

        private MeHeroCharacter MyHeroCharacter
        {
            get
            {
                return GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter;
            }
        }

        public void WaitBossHPAnim()
        {
            TimerUtility.WaitFrames(BossAnimFrameCount, OnBossHPAnimEnd);
        }

        public bool ShowBossHPPanelIfValid()
        {
            if (m_BossData != null && m_BossHPBarCount > 0)
            {
                m_BossHPPanel.SetActive(true);
                m_BossElement.InitElementView(m_BossData.ElementId);
                return true;
            }

            return false;
        }

        public void HideBossHPPanel()
        {
            m_BossHPPanel.SetActive(false);
        }

        public bool IsShowingBossAlert()
        {
            return m_BossAlertPanel.activeSelf;
        }

        public bool ShowBossAlert(float keepTime,string bossNameKey)
        {
            if (IsShowingBossAlert())
            {
                return false;
            }

            m_BossAlertPanel.SetActive(true);
            m_BossAlertAnimation.Play();
            m_BossAlertName.text = GameEntry.Localization.GetRawString( bossNameKey);
            //UIUtility.ReplaceDictionaryTextForLabels(m_BossAlertName.gameObject);
            StartCoroutine(HideBossAlert(keepTime));

            return true;
        }

        // Called by NGUI via reflection.
        public void OnChatClick()
        {
            //  m_ChatPanel.SetActive(!m_ChatPanel.activeSelf);
            //  m_ChatText.enabled = m_ChatPanel.activeSelf;
        }

        // Called by NGUI via reflection.
        public void OnPauseClick()
        {
            // TODO: Use separate buttons when UI resource is available.
            if (GameEntry.SceneLogic.BaseInstanceLogic is BasePvpInstanceLogic)
            {
                OnAbandonClick();
                return;
            }

            // TODO: Use separate buttons when UI resource is available.
            if (GameEntry.SceneLogic.BaseInstanceLogic is DemoInstanceLogic)
            {
                OnSkipClick();
                return;
            }

            m_PausePanel.SetActive(true);
            m_MusicIsUnmuted.value = !GameEntry.Sound.MusicIsMuted();
            m_SoundIsUnmuted.value = !GameEntry.Sound.SoundIsMuted();
            GameEntry.TimeScale.PauseGame();
            GameEntry.Input.JoystickActivated = false;
        }

        // Called by NGUI via reflection.
        public void OnResumeClick()
        {
            m_PausePanel.SetActive(false);
            GameEntry.Input.JoystickActivated = true;
            GameEntry.TimeScale.ResumeGame();
        }

        // Called by NGUI via reflection.
        public void OnAbandonClick()
        {
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic.HasResult || m_HasSkipOrAbandonUI)
            {
                return;
            }

            m_HasSkipOrAbandonUI = true;
            var abandonOrSkipDialogDisplayData = new DialogDisplayData
            {
                Message = GameEntry.Localization.GetString("UI_TEXT_BATTLE_NOTICE_GIVEUPCONFIRM"),
                Mode = 2,
                OnClickConfirm = OnAbandonConfirm,
                OnClickCancel = delegate (object o) { m_HasSkipOrAbandonUI = false; },
                ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_CONFIRM"),
                CancelText = GameEntry.Localization.GetString("UI_BUTTON_CANCEL"),
            };

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, abandonOrSkipDialogDisplayData);
        }

        // Called by NGUI via reflection.
        public void OnSkipClick()
        {
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic.HasResult || m_HasSkipOrAbandonUI)
            {
                return;
            }

            m_HasSkipOrAbandonUI = true;
            var abandonOrSkipDialogDisplayData = new DialogDisplayData
            {
                Message = GameEntry.Localization.GetString("UI_TEXT_SKIP_DEMO_INSTANCE_CONFIRM"),
                Mode = 2,
                OnClickConfirm = OnSkipConfirm,
                OnClickCancel = delegate (object o) { m_HasSkipOrAbandonUI = false; },
                ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_CONFIRM"),
                CancelText = GameEntry.Localization.GetString("UI_BUTTON_CANCEL"),
            };

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, abandonOrSkipDialogDisplayData);
        }

        // Called by NGUI via reflection.
        public void OnRequestClick()
        {
            m_RequestButton.GetComponent<BattleRequestSwitch>().SwitchRequest();
            GameEntry.Setting.SetBool(DisplayRequestSettingKey, m_RequestButton.gameObject.activeSelf);
            GameEntry.Setting.Save();

            for (int i = 0; i < Constant.InstanceRequestCount; i++)
            {
                m_RequestDescription[i].enabled = m_RequestButton.gameObject.activeSelf;
            }
        }

        // Called by NGUI via reflection.
        public void OnSkillClick(UIIntKey intKey)
        {
            if (!GameEntry.SceneLogic.BaseInstanceLogic.IsRunning)
            {
                return;
            }

            if (!GameEntry.Input.PerformSkill(intKey.Key, PerformSkillType.Click))
            {
                return;
            }

            if (m_SkillContinualTapDisplayer.IsShowing && m_SkillContinualTapDisplayer.SkillIndex == intKey.Key)
            {
                m_SkillContinualTapDisplayer.PlayTapAnimation();
            }

            PlaySkillAnimation(true, intKey.Key);
        }

        // Called by NGUI via reflection.
        public void OnHeroClick(UIIntKey intKey)
        {
            if (!GameEntry.SceneLogic.BaseInstanceLogic.IsRunning)
            {
                return;
            }
            var myHeroesData = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData;
            var entity = GameEntry.Entity.GetEntity(myHeroesData.CurrentHeroData.Id);
            if (entity == null)
            {
                return;
            }
            int index = intKey.Key;
            var heroCharacter = entity.Logic as HeroCharacter;
            if (index >= myHeroesData.CurrentHeroIndex)
            {
                index++;
            }
            if (!heroCharacter.CanSwitchHero)
            {
                return;
            }
            GameEntry.Input.SwitchHero(index);
        }

        // Called by NGUI via reflection.
        public void OnAutoFightSwitch(bool value)
        {
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;

            if (value)
            {
                if (!instanceLogic.CanEnableAutoFight)
                {
                    m_Auto.Set(false, false);
                }
                else
                {
                    instanceLogic.EnableAutoFight();
                }
            }
            else
            {
                instanceLogic.DisableAutoFight();
            }
        }

        // Called by NGUI via reflection.
        public void OnMusicIsUnmutedChanged(bool unmuted)
        {
            GameEntry.Sound.MuteMusic(!unmuted);
        }

        // Called by NGUI via reflection.
        public void OnSoundIsUnmutedChanged(bool unmuted)
        {
            GameEntry.Sound.MuteSound(!unmuted);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DTHero = GameEntry.DataTable.GetDataTable<DRHero>();
            m_DTSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            m_DTSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            m_DTIcon = GameEntry.DataTable.GetDataTable<DRIcon>();
            GameEntry.Event.Subscribe(EventId.OnMimicMeleeChanged, OnMimicMeleeChanged);

            if (m_BossHPBarColors.Count <= 0)
            {
                m_BossHPBarColors.Add(Color.red);
            }

            // Initialize charge skill progress bar
            var chargeSkillProgressBar = Instantiate(m_ChargeSkillProgressBarTemplete);
            chargeSkillProgressBar.transform.SetParent(m_ChargeSkillProgressBarRoot);
            chargeSkillProgressBar.SetActive(false);
            chargeSkillProgressBar.transform.localScale = Vector3.one;
            chargeSkillProgressBar.transform.localPosition = new Vector3(m_ChargeSkillProgressBarPositionX, m_ChargeSkillProgressBarPositionY, 0);
            m_ChargeSkillProgressBar = chargeSkillProgressBar.GetComponent<ForceStorage>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_CachedAutoFightIsEnabled = false;
            m_NormalRoot.SetActive(true);
            m_MovieMask.Root.SetActive(false);

            m_DropGoodsInstanceLogic = GameEntry.SceneLogic.BaseInstanceLogic as IDropGoodsInstance;
            m_PropagandaInstanceLogic = GameEntry.SceneLogic.BaseInstanceLogic as IPropagandaInstance;
            HasOpenShield = false;
            m_WaitingForStart.Reset();

            for (int i = 0; i < m_SkillButtons.Length; i++)
            {
                m_SkillButtons[i].OnOpen(this);
            }

            m_LastHPBarLeftCount = -1;
            m_Pause.isEnabled = true;
            m_TimeText.gameObject.SetActive(true);
            m_LastTimeText.gameObject.SetActive(false);
            m_TimeCommonBg.SetActive(true);
            m_TimeLastBg.SetActive(false);
            m_PausePanel.SetActive(false);
            m_BossHPPanel.SetActive(m_BossData != null && m_BossHPBarCount > 0);
            m_BossSteadyProgressBar.gameObject.SetActive(m_BossCharacterData != null && m_BossCharacterData.Steady.IsSteadying);
            m_BossSteadyRecoverProgressBar.gameObject.SetActive(m_BossCharacterData != null && !m_BossCharacterData.Steady.IsSteadying);
            m_BossAlertPanel.SetActive(false);
            m_SkillControllerPanel.SetActive(true);
            m_DropCoinsRoot.SetActive((GameEntry.SceneLogic.BaseInstanceLogic as BaseSinglePlayerInstanceLogic) != null);
            m_DropCoinsLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", 0);
            m_HeroInBattle.PortraitObj.SetActive(true);
            InitPvpPanel();
            InitMelee();

            if (m_BossData != null)
            {
                m_BossElement.InitElementView(m_BossData.ElementId);
            }

            ShowOrHideRequest();

            for (int i = 0; i < m_SkillButtons.Length; i++)
            {
                PlaySkillAnimation(false, i);
            }
            bool autoFight = GameEntry.Setting.GetBool("AUTO_FIGHT", false);
            autoFight = autoFight ? GameEntry.SceneLogic.InstanceLogicType != InstanceLogicType.MimicMelee && GameEntry.SceneLogic.InstanceLogicType != InstanceLogicType.SinglePvp : false;
            m_Auto.gameObject.SetActive(autoFight);
            m_Auto.value = false;

            ShowHeroIcons();

            m_SkillButtonsCDComplete = new bool[m_SkillButtons.Length];
            RefreshSkillButtons();
            CheckPropaganda();

            m_HeroButtonsCDComplete = new bool[m_HeroButtons.Length];
            for (int i = 0; i < m_HeroButtonsCDComplete.Length; ++i)
            {
                m_HeroButtonsCDComplete[i] = true;
            }

            m_SkillContinualTapDisplayer = new SkillContinualTapDisplayer(this);
            m_SkillContinualTapDisplayer.Hide();

            SubscribeEvents();

            GameEntry.Input.ShouldEnableJoystick += ShouldEnableJoystick;

            HideWidgetsOnOpen();
        }

        private void HideWidgetsOnOpen()
        {
            m_ChatButton.gameObject.SetActive(false);
            m_DropItemPanel.SetActive(false);
            m_RequestButton.gameObject.SetActive(false);
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) base.OnClose(userData);
            m_HasSkipOrAbandonUI = false;

            for (int i = 0; i < m_SkillButtons.Length; i++)
            {
                m_SkillButtons[i].OnClose();
            }

            m_SkillContinualTapDisplayer.ShutDown();
            m_SkillContinualTapDisplayer = null;
            if (m_Ping != null)
            {
                m_Ping.Shutdown();
                m_Ping = null;
            }

            GameEntry.Input.ShouldEnableJoystick -= ShouldEnableJoystick;

            UnsubscribeEvents();

            m_Pause.isEnabled = true;
            m_BossHPPanel.SetActive(false);
            m_BossData = null;
            m_BossCharacterData = null;
            m_BossHPBarCount = 0;

            m_DropGoodsInstanceLogic = null;
            m_PropagandaInstanceLogic = null;

            m_SkillButtonsCDComplete = null;
            m_HeroButtonsCDComplete = null;

            ClearSystemMessages();

            GameEntry.Event.Unsubscribe(EventId.OnMimicMeleeChanged, OnMimicMeleeChanged);

            if (m_MiniMapGO != null)
            {
                Destroy(m_MiniMapGO);
            }

            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_WaitingForStart.TimerAnim.gameObject.activeSelf)
            {
                UpdateInstanceStartTime();
            }

            MeHeroCharacter meHeroCharacter = GameEntry.SceneLogic.MeHeroCharacter;
            if (meHeroCharacter == null || meHeroCharacter.Motion == null)
            {
                return;
            }

            var meData = meHeroCharacter.Data;
            var meHeroes = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.GetHeroes();

            bool isDuringCommonCoolDown = GameEntry.SceneLogic.BaseInstanceLogic.IsDuringCommonCoolDown;

            UpdateInstanceTimer();
            UpdateDropGoods();
            UpdateRequests();
            UpdateSkillCDs(meData, isDuringCommonCoolDown);
            UpdateHeroSwitchCDs(meHeroes, isDuringCommonCoolDown);
            UpdateBossHPPanel();
            UpdateHeroSteadyBars(meHeroes);
            UpdateDodgeButton(meData);
            UpdatePvpInfo();
            UpdateMeleeInfo();
            UpdateSystemMessages();

            m_SkillContinualTapDisplayer.Update(elapseSeconds, realElapseSeconds);
        }

        private void OnSkillPress(UIIntKey intKey)
        {
            if (!GameEntry.SceneLogic.BaseInstanceLogic.IsRunning)
            {
                return;
            }

            // 这个是蓄力技能的缓存，蓄力技的发和结束必须是对应的，当发一个蓄力技能的时候，不允许再次触发
            if (m_PerformingChargeSkillId != int.MinValue)
            {
                return;
            }

            var skillGroup = GameEntry.SceneLogic.MeHeroCharacter.Data.GetCachedSkillGroupDataRow(intKey.Key);
            if (skillGroup.IsCharge)
            {
                int skillId = skillGroup.SkillId;
                m_PerformingChargeSkillId = skillId;
            }

            if (!GameEntry.Input.PerformSkill(intKey.Key, PerformSkillType.Press))
            {
                return;
            }
        }

        private void OnSkillRelease(UIIntKey intKey)
        {
            if (!GameEntry.SceneLogic.BaseInstanceLogic.IsRunning)
            {
                return;
            }

            // 当蓄力技能ID为无效值（int.MinValue 自己规定的无效值）的时候，说明之前没有释放过蓄力技，那么Release没必要再触发
            if (m_PerformingChargeSkillId == int.MinValue)
                return;

            if (!GameEntry.Input.PerformSkill(intKey.Key, PerformSkillType.Release))
            {
                return;
            }
        }

        private void PlaySkillAnimation(bool hasPlaySkill, int index)
        {
            if (m_SkillButtons[index].Root == null) return;

            m_SkillButtons[index].Animation.gameObject.SetActive(hasPlaySkill);
            if (hasPlaySkill)
            {
                m_SkillButtons[index].Animation.Play();
            }
        }

        private void OnBossHPAnimEnd(object userData)
        {
            m_BossHPAnimEnd = true;
        }

        private void UpdateInstanceTimer()
        {
            if (GameEntry.SceneLogic.BaseInstanceLogic.Timer != null)
            {
                if (GameEntry.SceneLogic.BaseInstanceLogic.Timer.IsAlert)
                {
                    if (!m_LastTimeText.isActiveAndEnabled)
                    {
                        m_TimeText.gameObject.SetActive(false);
                        m_LastTimeText.gameObject.SetActive(true);
                        m_TimeCommonBg.SetActive(false);
                        m_TimeLastBg.SetActive(true);
                    }
                    if (!m_LastTimeText.text.Equals(GameEntry.SceneLogic.BaseInstanceLogic.Timer.FormattedTimeString))
                    {
                        m_LastTimeText.text = GameEntry.SceneLogic.BaseInstanceLogic.Timer.FormattedTimeString;
                        m_LastTimeText.GetComponent<Animation>().Rewind();
                        m_LastTimeText.GetComponent<Animation>().Play();
                    }
                }
                else
                {
                    m_TimeText.text = GameEntry.SceneLogic.BaseInstanceLogic.Timer.FormattedTimeString;
                }

                if (!m_TimerRoot.activeSelf)
                {
                    m_TimerRoot.SetActive(true);
                }
            }
            else
            {
                if (m_TimerRoot.activeSelf)
                {
                    m_TimerRoot.SetActive(false);
                }
            }
        }

        private bool CheckContinualTapSkill(int skillIndex)
        {
            if (MyHeroCharacter == null)
            {
                return false;
            }
            return MyHeroCharacter.CheckContinualTapSkill(skillIndex);
        }

        private bool CoolDownMaskShouldBeEnabled(int skillIndex, HeroData meData, SkillButton skillButton, bool isDuringCommonCoolDown, bool disabledAltSkill, bool continualTapSkillFlag)
        {
            if (meData.IsDead || disabledAltSkill)
            {
                return true;
            }

            if (MyHeroCharacter == null)
            {
                return true;
            }

            if (MyHeroCharacter.Motion.PerformSkillIndex == skillIndex && continualTapSkillFlag)
            {
                return false;
            }

            return skillButton.CoolDownTime.enabled || isDuringCommonCoolDown;
        }

        private void UpdateSkillCDs(HeroData meData, bool isDuringCommonCoolDown)
        {
            if (meData == null)
            {
                return;
            }

            for (int i = 0; i < Constant.TotalSkillGroupCount; i++)
            {
                var skillButton = m_SkillButtons[i];
                if (skillButton.Root == null)
                {
                    continue;
                }

                bool disabledAltSkill = false;
                CoolDownTime coolDownTime = null;
                coolDownTime = GetSkillCDObject(meData, i, ref disabledAltSkill);
                var continualTapSkillFlag = CheckContinualTapSkill(i);
                UpdateSkillCDTimeLabel(meData, i, skillButton, coolDownTime, continualTapSkillFlag);
                var coolDownMaskShouldBeEnabled = CoolDownMaskShouldBeEnabled(i, meData, skillButton, isDuringCommonCoolDown, disabledAltSkill, continualTapSkillFlag);
                skillButton.CoolDownMask.enabled = coolDownMaskShouldBeEnabled;
                skillButton.Button.isEnabled = !skillButton.CoolDownMask.enabled;
                skillButton.CDProgress.fillAmount = coolDownTime.RemainingCoolDownTimeRatio;
                UpdateSkillCDComplete(i, coolDownTime);
                UpdateSkillCDProgress(skillButton, coolDownTime);
            }
        }

        private void UpdateSkillCDProgress(SkillButton skillButton, CoolDownTime coolDownTime)
        {
            if (!skillButton.CoolDownTime.enabled)
            {
                skillButton.CDDecal.gameObject.SetActive(false);
                skillButton.CDProgress.enabled = false;
            }
            else
            {
                skillButton.CDDecal.gameObject.SetActive(true);
                skillButton.CDProgress.enabled = true;
                skillButton.CDDecal.Value = coolDownTime.RemainingCoolDownTimeRatio;
            }
        }

        private void UpdateSkillCDComplete(int i, CoolDownTime coolDownTime)
        {
            if (coolDownTime.RemainingCoolDownSeconds <= 0f && !m_SkillButtonsCDComplete[i])
            {
                m_SkillButtonsCDComplete[i] = true;

                var effectKey = string.Format("CDComplete{0}", i.ToString());
                if (m_EffectsController.HasEffect(effectKey))
                {
                    m_EffectsController.ShowEffect(effectKey);
                }
            }
            else if (coolDownTime.RemainingCoolDownSeconds > 0f && m_SkillButtonsCDComplete[i])
            {
                m_SkillButtonsCDComplete[i] = false;
            }
        }

        private void UpdateSkillCDTimeLabel(HeroData meData, int i, SkillButton skillButton, CoolDownTime coolDownTime, bool continualTapSkillFlag)
        {
            if (i == Constant.DodgeSkillIndex)
            {
                skillButton.CoolDownTime.enabled = meData.DodgeEnergy < meData.CostPerDodge;
                skillButton.CoolDownTime.text = string.Empty;
            }
            else
            {
                var currentlyPerforming = ((MyHeroCharacter == null || MyHeroCharacter.Motion == null) ? false : MyHeroCharacter.Motion.PerformSkillIndex == i);
                skillButton.CoolDownTime.enabled = !coolDownTime.IsReady && !(continualTapSkillFlag && currentlyPerforming) && !coolDownTime.IsPaused;
                skillButton.CoolDownTime.text = coolDownTime.IsPaused ? string.Empty
                    : GameEntry.Localization.GetString("UI_TEXT_TIME_SECOND", Mathf.CeilToInt(coolDownTime.RemainingCoolDownSeconds));
            }
        }

        private static CoolDownTime GetSkillCDObject(HeroData meData, int i, ref bool disabledAltSkill)
        {
            CoolDownTime coolDownTime;
            if (meData.AltSkill.Enabled)
            {
                int skillGroupId = meData.AltSkill.DRAltSkill.GetAltSkillGroupId(i);
                if (skillGroupId < 0)
                {
                    coolDownTime = meData.GetSkillCoolDownTime(i);
                }
                else
                {
                    coolDownTime = meData.AltSkill.GetSkillCoolDownTime(i);
                }

                disabledAltSkill = !meData.AltSkill.DRAltSkill.GetAltSkillGroupEnabled(i);
            }
            else
            {
                coolDownTime = meData.GetSkillCoolDownTime(i);
            }

            return coolDownTime;
        }

        private void UpdateHeroSwitchCDs(HeroData[] meHeroes, bool isDuringCommonCoolDown)
        {
            int currentIndex = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.CurrentHeroIndex;
            m_HeroInBattle.HpBar.value = meHeroes[currentIndex].HPRatio;
            for (int i = 0, j = 0; i < Constant.MaxBattleHeroCount; i++)
            {
                if (i == currentIndex || j >= m_HeroButtons.Length)
                {
                    continue;
                }

                if (i >= meHeroes.Length)
                {
                    m_HeroButtons[j].Button.gameObject.SetActive(false);
                    j++;
                    continue;
                }

                m_HeroButtons[j].Button.gameObject.SetActive(true);
                m_HeroButtons[j].HPBar.fillAmount = meHeroes[i].HPRatio;
                CoolDownTime coolDownTime = meHeroes[i].SwitchSkillCD;
                m_HeroButtons[j].CoolDownTime.enabled = !coolDownTime.IsReady;
                m_HeroButtons[j].CoolDownTime.text = GameEntry.Localization.GetString("UI_TEXT_TIME_SECOND", Mathf.CeilToInt(coolDownTime.RemainingCoolDownSeconds));
                m_HeroButtons[j].CoolDownMask.enabled = meHeroes[i].IsDead || m_HeroButtons[j].CoolDownTime.enabled || isDuringCommonCoolDown;
                m_HeroButtons[j].Button.enabled = !m_HeroButtons[j].CoolDownMask.enabled;
                m_HeroButtons[j].SkillReady.enabled = false;
                m_HeroButtons[j].CDProgress.fillAmount = coolDownTime.RemainingCoolDownTimeRatio;

                if (coolDownTime.RemainingCoolDownSeconds <= 0f && !m_HeroButtonsCDComplete[j])
                {
                    m_HeroButtonsCDComplete[j] = true;
                    m_EffectsController.ShowEffect(string.Format("CDCompleteHero{0}", j.ToString()));
                }
                else if (coolDownTime.RemainingCoolDownSeconds > 0f && m_HeroButtonsCDComplete[j])
                {
                    m_HeroButtonsCDComplete[j] = false;
                }

                if (coolDownTime.RemainingCoolDownTimeRatio == 0)
                {
                    m_HeroButtons[j].CDDecal.gameObject.SetActive(false);
                }
                else
                {
                    m_HeroButtons[j].CDDecal.gameObject.SetActive(true);
                    m_HeroButtons[j].CDDecal.Value = coolDownTime.RemainingCoolDownTimeRatio;
                }
                j++;
            }
        }

        private void UpdateBossHPPanel()
        {
            if (m_BossData == null || m_BossHPBarCount <= 0)
            {
                return;
            }

            if (m_BossData.HP <= 0)
            {
                m_BossHPPanel.SetActive(false);
                m_BossData = null;
                m_BossCharacterData = null;
                m_BossHPBarCount = 0;
                return;
            }

            float hpPerBar = (float)m_BossData.MaxHP / (m_BossHPBarCount > 0 ? m_BossHPBarCount : 1);
            int currentHPBarLeftCount = (int)((m_BossData.HP - 1) / hpPerBar);
            int currentHPBarLeftValue = m_BossData.HP - (int)(hpPerBar * currentHPBarLeftCount);

            if (m_LastHPBarLeftCount != currentHPBarLeftCount && !m_BossHpAnimStart)
            {
                m_AnimNormalBossIcon.transform.parent.gameObject.SetActive(true);
                m_AnimNormalBossIcon.gameObject.SetActive(currentHPBarLeftCount != 0);
                m_AnimRedBossIcon.gameObject.SetActive(currentHPBarLeftCount == 0);
                m_BossHPProgressBar.value = 0;
                m_BossHpAnimStart = true;
                m_BossHpAnim[m_BossHpAnim.clip.name].time = 0;
                m_BossHpAnim.Play(m_BossHpAnim.clip.name);
                WaitBossHPAnim();
            }
            else if (m_LastHPBarLeftCount != currentHPBarLeftCount && m_BossHpAnimStart && m_BossHPAnimEnd)
            {
                m_AnimNormalBossIcon.transform.parent.gameObject.SetActive(false);
                m_BossHpAnimStart = false;
                m_BossHPAnimEnd = false;
                if (currentHPBarLeftCount >= 1)
                {
                    m_LastHPBarLeftCount = currentHPBarLeftCount;
                    m_BeforeBossHPBehindBar.SetActive(true);
                    m_BossHPTimes.text = GameEntry.Localization.GetString("UI_TEXT_TIMESNUMBER", (currentHPBarLeftCount + 1).ToString());
                    m_BehindRedBossIcon.transform.parent.gameObject.SetActive(true);
                    m_BehindNormalBossIcon.gameObject.SetActive(currentHPBarLeftCount != 1);
                    m_BehindRedBossIcon.gameObject.SetActive(currentHPBarLeftCount == 1);
                    m_BeforeRedBossIcon.gameObject.SetActive(false);
                    m_BeforeNormalBossIcon.enabled = true;
                    m_BossHPProgressBar.foregroundWidget = m_BeforeNormalBossIcon;
                    if (currentHPBarLeftCount > 2)
                    {
                        // 这里面boss颜色写死固定在m_BossHPBarColors里，只有3种颜色由策划配
                        m_BehindNormalBossIcon.color = m_BossHPBarColors[1];
                        m_BeforeNormalBossIcon.color = m_BossHPBarColors[0];
                    }
                    else if (currentHPBarLeftCount == 2)
                    {
                        m_BehindNormalBossIcon.color = m_BossHPBarColors[0];
                    }
                    else if (currentHPBarLeftCount == 1)
                    {
                        m_BeforeNormalBossIcon.color = m_BossHPBarColors[2];
                    }
                }
                else
                {
                    m_BehindRedBossIcon.transform.parent.gameObject.SetActive(false);
                    m_LastHPBarLeftCount = currentHPBarLeftCount;
                    m_BeforeBossHPBehindBar.SetActive(false);
                    m_BeforeRedBossIcon.gameObject.SetActive(true);
                    m_BeforeNormalBossIcon.enabled = false;
                    m_BossHPProgressBar.foregroundWidget = m_BeforeRedBossIcon;
                    m_BossHPTimes.text = string.Empty;
                }
            }

            if (!m_BossHpAnimStart && !m_BossHPAnimEnd)
            {
                m_BossHPProgressBar.value = currentHPBarLeftValue / hpPerBar;
            }

            if (m_BossCharacterData != null)
            {
                m_BossSteadyProgressBar.value = m_BossSteadyRecoverProgressBar.value = m_BossCharacterData.Steady.SteadyRatio;
                m_BossSteadyProgressBar.gameObject.SetActive(m_BossCharacterData.Steady.IsSteadying);
                m_BossSteadyRecoverProgressBar.gameObject.SetActive(!m_BossCharacterData.Steady.IsSteadying);
            }

            // 保留以备方案改回。
            // if (currentHPBarLeftCount >= 1)
            // {
            //     m_BossHPProgressBar.backgroundWidget.color = m_BossHPBarColors[(currentHPBarLeftCount - 1) % m_BossHPBarColors.Count];
            //     m_BossHPProgressBar.foregroundWidget.color = m_BossHPBarColors[currentHPBarLeftCount % m_BossHPBarColors.Count];
            //     m_BossHPTimes.text = GameEntry.Localization.GetString("UI_TEXT_TIMESNUMBER", (currentHPBarLeftCount + 1).ToString());
            // }
            // else
            // {
            //     m_BossHPProgressBar.backgroundWidget.color = Color.black;
            //     m_BossHPProgressBar.foregroundWidget.color = m_BossHPBarColors[0];
            //     m_BossHPTimes.text = string.Empty;
            // }
        }

        private void UpdateHeroSteadyBars(HeroData[] heroDatas)
        {
            int currentIndex = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.CurrentHeroIndex;

            HeroData currentHeroData = heroDatas[currentIndex];
            m_HeroInBattle.SteadyBar.value = m_HeroInBattle.SteadyRecoverBar.value = currentHeroData.Steady.SteadyRatio;
            m_HeroInBattle.SteadyBar.gameObject.SetActive(currentHeroData.Steady.IsSteadying);
            m_HeroInBattle.SteadyRecoverBar.gameObject.SetActive(!currentHeroData.Steady.IsSteadying);

            for (int i = 0, j = 0; j < Constant.MaxBattleHeroCount && j < heroDatas.Length; j++)
            {
                if (j == currentIndex)
                {
                    continue;
                }
                m_HeroButtons[i].SteadyBar.fillAmount = m_HeroButtons[i].SteadyRecoverBar.fillAmount = heroDatas[j].Steady.SteadyRatio;
                m_HeroButtons[i].SteadyBar.gameObject.SetActive(!heroDatas[j].IsDead && heroDatas[j].Steady.IsSteadying);
                m_HeroButtons[i].SteadyRecoverBar.gameObject.SetActive(!heroDatas[j].IsDead && !heroDatas[j].Steady.IsSteadying);
                i++;
            }
        }

        private void UpdateDropGoods()
        {
            if (m_DropGoodsInstanceLogic == null)
            {
                return;
            }

            m_DropItemText.text = GameEntry.Localization.GetString("UI_TEXT_TIMESNUMBER", m_DropGoodsInstanceLogic.DropGoodsCount.ToString());
        }

        private void UpdateRequests()
        {
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (!instanceLogic.HasRequests)
            {
                return;
            }

            for (int i = 0; i < Constant.InstanceRequestCount; i++)
            {
                bool requestComplete = instanceLogic.IsRequestComplete(i);
                m_RequestDescription[i].color = requestComplete ? (Color)(new Color32(255, 209, 67, 255)) : (Color)(new Color32(202, 240, 239, 255));
                m_RequestStars[i].SetActive(requestComplete);
            }
        }

        private void UpdateDodgeButton(HeroData meHeroData)
        {
            m_DodgeButton.EnergyBar.fillAmount = meHeroData.DodgeEnergyRatio;
        }

        private IEnumerator HideBossAlert(float keepTime)
        {
            yield return new WaitForSeconds(keepTime);
            Log.Info("[BattleForm HideBossAlert] Time is up.");
            m_BossAlertAnimation.Stop();
            m_BossAlertPanel.SetActive(false);
        }

        private void OnSkipConfirm(object userData)
        {
            m_HasSkipOrAbandonUI = false;
            GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceSuccess(InstanceSuccessReason.Unknown, GoToCreatePlayer);
        }

        private void GoToCreatePlayer()
        {
            var mainProcedure = GameEntry.Procedure.CurrentProcedure as ProcedureMain;

            if (mainProcedure == null)
            {
                return;
            }

            mainProcedure.GoToCreatePlayer();
        }

        private void OnAbandonConfirm(object userData)
        {
            m_HasSkipOrAbandonUI = false;

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic.HasResult)
            {
                return;
            }
            if (instanceLogic is BasePvpInstanceLogic)
            {
                (instanceLogic as BasePvpInstanceLogic).RequestGiveUpPvp();
                HidePvpPanel();
                return;
            }
            instanceLogic.SetInstanceFailure(InstanceFailureReason.AbandonedByUser, false, delegate () { GameEntry.SceneLogic.GoBackToLobby(); });
        }

        private void RefreshSkillButtons()
        {
            if (GameEntry.SceneLogic.MeHeroCharacter == null)
            {
                return;
            }

            for (int i = 0; i < m_SkillButtonsCDComplete.Length; ++i)
            {
                m_SkillButtonsCDComplete[i] = true;
            }

            for (int skillIndex = 0; skillIndex < Constant.TotalSkillGroupCount; skillIndex++)
            {
                if (GameEntry.SceneLogic.MeHeroCharacter.Data.AltSkill.Enabled)
                {
                    if (skillIndex < Constant.SkillGroupCount)
                    {
                        RefreshAltSkillButton(skillIndex);
                    }
                }
                else
                {
                    RefreshNormalSkillButton(skillIndex);
                }
            }
        }

        private void RefreshSwitchSkillButton(bool keepEffect)
        {
            var switchSkillButton = m_SkillButtons[Constant.SwitchSkillIndex];
            if (MyHeroCharacter == null)
            {
                switchSkillButton.Root.gameObject.SetActive(false);
                return;
            }

            var switchSkillBadges = MyHeroCharacter.Data.SkillsBadges[Constant.SwitchSkillIndex];
            if (switchSkillBadges == null)
            {
                switchSkillButton.Root.gameObject.SetActive(false);
                return;
            }

            var badgeId = switchSkillBadges.SpecificBadge.BadgeId;
            DRSpecificSkillBadge drBadge = badgeId <= 0 ? null : GameEntry.DataTable.GetDataTable<DRSpecificSkillBadge>().GetDataRow(badgeId);
            if (drBadge == null)
            {
                switchSkillButton.Root.gameObject.SetActive(false);
                return;
            }

            switchSkillButton.Root.gameObject.SetActive(drBadge.UseSwitchSkillDirectly);
        }

        private void RefreshNormalSkillButton(int skillIndex, bool keepEffect = false)
        {
            if (MyHeroCharacter == null)
            {
                return;
            }

            HeroData data = MyHeroCharacter.Data;

            if (m_SkillButtons[skillIndex].Root == null)
            {
                return;
            }

            if (skillIndex == Constant.SwitchSkillIndex)
            {
                RefreshSwitchSkillButton(keepEffect);
            }

            if (!keepEffect)
            {
                m_SkillButtons[skillIndex].Effect.gameObject.SetActive(false);
            }

            if (m_SkillButtons[skillIndex].IsLock)
            {
                m_SkillButtons[skillIndex].IsLock = false;
                for (int i = 0; i < m_SkillButtons[skillIndex].LockAnimation.Length; i++)
                {
                    m_SkillButtons[skillIndex].LockAnimation[i].PlayMode = UISpriteAnimationEx.Mode.Backward;
                    m_SkillButtons[skillIndex].LockAnimation[i].Reset();
                }
            }

            DRHero dataRowHero = m_DTHero.GetDataRow(data.HeroId);
            if (dataRowHero == null)
            {
                Log.Warning("Can not load hero '{0}' from data table.", data.HeroId.ToString());
                return;
            }

            DRSkillGroup dataRowSkillGroup = MyHeroCharacter.Data.GetCachedSkillGroupDataRow(skillIndex);
            if (dataRowSkillGroup == null)
            {
                Log.Warning("Can not load skill at index '{0}' from data table.", skillIndex.ToString());
                return;
            }

            int skillId = dataRowSkillGroup.SkillId;
            DRSkill dataRowSkill = m_DTSkill.GetDataRow(skillId);
            if (dataRowSkill == null)
            {
                Log.Warning("Can not load skill '{0}' from data table.", skillId.ToString());
                return;
            }

            int iconId = dataRowSkill.IconId;
            DRIcon dataRowIcon = m_DTIcon.GetDataRow(iconId);
            if (dataRowIcon == null)
            {
                Log.Warning("Can not load icon '{0}' from data table.", iconId.ToString());
                return;
            }

            m_SkillButtons[skillIndex].InitLevelLock(data.GetSkillIsLevelLocked(skillIndex));
            m_SkillButtons[skillIndex].InitSkillCategoryIcon((SkillCategory)dataRowSkillGroup.SkillCategory);
            m_SkillButtons[skillIndex].Icon.LoadAsync(dataRowIcon.AtlasName, dataRowIcon.SpriteName,
                (sprite, spriteName, userData) => { m_SkillButtons[skillIndex].Button.normalSprite = spriteName; });
        }

        private void RefreshAltSkillButton(int skillIndex)
        {
            HeroData data = GameEntry.SceneLogic.MeHeroCharacter.Data;

            if (data.AltSkill.DRAltSkill.GetAltSkillEffect(skillIndex))
            {
                m_SkillButtons[skillIndex].Effect.gameObject.SetActive(true);
                m_SkillButtons[skillIndex].Effect.Play();
            }
            else
            {
                m_SkillButtons[skillIndex].Effect.gameObject.SetActive(false);
            }

            int skillGroupId = data.AltSkill.DRAltSkill.GetAltSkillGroupId(skillIndex);
            if (skillGroupId < 0)
            {
                RefreshNormalSkillButton(skillIndex, true);
                return;
            }

            if (m_SkillButtons[skillIndex].IsLock)
            {
                if (data.AltSkill.DRAltSkill.GetAltSkillGroupEnabled(skillIndex))
                {
                    m_SkillButtons[skillIndex].IsLock = false;
                    for (int i = 0; i < m_SkillButtons[skillIndex].LockAnimation.Length; i++)
                    {
                        m_SkillButtons[skillIndex].LockAnimation[i].PlayMode = UISpriteAnimationEx.Mode.Backward;
                        m_SkillButtons[skillIndex].LockAnimation[i].Reset();
                    }
                }
            }
            else
            {
                if (!data.AltSkill.DRAltSkill.GetAltSkillGroupEnabled(skillIndex))
                {
                    m_SkillButtons[skillIndex].IsLock = true;
                    for (int i = 0; i < m_SkillButtons[skillIndex].LockAnimation.Length; i++)
                    {
                        m_SkillButtons[skillIndex].LockAnimation[i].PlayMode = UISpriteAnimationEx.Mode.Forward;
                        m_SkillButtons[skillIndex].LockAnimation[i].Reset();
                    }
                }
            }

            DRSkillGroup dataRowSkillGroup = m_DTSkillGroup.GetDataRow(skillGroupId);
            if (dataRowSkillGroup == null)
            {
                Log.Warning("Can not load skill group '{0}' from data table.", skillGroupId.ToString());
                return;
            }

            int skillId = dataRowSkillGroup.SkillId;
            DRSkill dataRowSkill = m_DTSkill.GetDataRow(skillId);
            if (dataRowSkill == null)
            {
                Log.Warning("Can not load skill '{0}' from data table.", skillId.ToString());
                return;
            }

            int iconId = dataRowSkill.IconId;
            DRIcon dataRowIcon = m_DTIcon.GetDataRow(iconId);
            if (dataRowIcon == null)
            {
                Log.Warning("Can not load icon '{0}' from data table.", iconId.ToString());
                return;
            }

            m_SkillButtons[skillIndex].Icon.LoadAsync(dataRowIcon.AtlasName, dataRowIcon.SpriteName,
                (sprite, spriteName, userData) => { m_SkillButtons[skillIndex].Button.normalSprite = spriteName; });
        }

        private void ShowOrHideRequest()
        {
            //var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            //if (instanceLogic.HasRequests)
            //{
            //    m_RequestButton.gameObject.SetActive(true);

            //    for (int i = 0; i < Constant.InstanceRequestCount; i++)
            //    {
            //        m_RequestDescription[i].text = instanceLogic.GetRequest(i);
            //    }
            //}
            //else
            //{
            //    m_RequestButton.gameObject.SetActive(false);
            //}
        }

        private void ShowHeroIcons()
        {
            var meHeroesData = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData;
            var heroes = meHeroesData.GetHeroes();
            int currentIndex = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.CurrentHeroIndex;
            for (int i = 0, j = 0; i < heroes.Length; ++i)
            {
                if (i == currentIndex)
                {
                    continue;
                }
                var heroData = heroes[i];
                DRHero heroRow = m_DTHero.GetDataRow(heroData.HeroId);
                if (heroRow == null)
                {
                    return;
                }

                var heroIconId = heroRow.IconId;
                DRIcon heroIconRow = m_DTIcon.GetDataRow(heroIconId);
                if (heroIconRow == null)
                {
                    return;
                }

                SetPortraitIcon(heroIconId, m_HeroButtons[j].Portrait);
                m_HeroButtons[j].ElementIcon.InitElementView(heroRow.ElementId);
                j++;
            }
            m_SwitchFirstHero = true;
            RefreshBattleHero();
        }

        private void SetPortraitIcon(int iconId, UISprite targetSprite)
        {
            DRIcon row = m_DTIcon.GetDataRow(iconId);
            if (row == null)
            {
                return;
            }

            targetSprite.spriteName = row.SpriteName;
        }

        private void UpdateInstanceStartTime()
        {
            var instanceStartTime = GameEntry.SceneLogic.BaseInstanceLogic.InstanceStartTime;
            var timeLeft = instanceStartTime - Time.time;
            m_WaitingForStart.SetNumber(timeLeft);
            m_WaitingForStart.ResetIfComplete();
        }

        private void RefreshBattleHero(bool isMe = true)
        {
            if (!m_PvpInfo.Panel.activeSelf)
            {
                var myHeroesData = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData;
                int currentIndex = myHeroesData.CurrentHeroIndex;
                var meHeroes = myHeroesData.GetHeroes();

                DRHero heroRow = m_DTHero.GetDataRow(meHeroes[currentIndex].HeroId);
                if (heroRow == null)
                {
                    return;
                }
                RefreshBattleHeroInfo(m_HeroInBattle, heroRow);
                RefreshHeroButtonVisible(currentIndex);
            }
            else
            {
                if (isMe)
                {
                    RefreshPvpHeroBattle();
                }
                else
                {
                    RefreshPvpOppHeroBattle();
                }
                UpdatePvpInfo();
            }
        }

        private void RefreshHeroButtonVisible(int currentIndex)
        {
            for (int i = 0, j = 0; i < Constant.MaxBattleHeroCount; i++)
            {
                if (i == currentIndex)
                {
                    continue;
                }
                m_HeroButtons[j].HPBar.gameObject.SetActive(true);
                m_HeroButtons[j].SteadyPanel.gameObject.SetActive(true);
                m_HeroButtons[j].CanBattleObj.SetActive(false);
                j++;
            }
        }

        private void RefreshBattleHeroInfo(HeroInBattle heroInbattle, DRHero heroRow)
        {
            heroInbattle.PortraitOutObj.SetActive(!m_SwitchFirstHero);
            heroInbattle.PortraitInObj.SetActive(!m_SwitchFirstHero);
            if (!m_SwitchFirstHero)
            {
                heroInbattle.IconOut.spriteName = heroInbattle.Icon.spriteName;
                heroInbattle.ElementIconOut.InitElementView(heroRow.ElementId);
                if (heroInbattle.IconMelee)
                {
                    heroInbattle.IconMelee.spriteName = heroInbattle.Icon.spriteName;
                    heroInbattle.ElementIconMelee.InitElementView(heroRow.ElementId);
                }
                SetPortraitIcon(heroRow.IconId, heroInbattle.IconIn);
                if (isActiveAndEnabled)
                {
                    StartCoroutine(AnimationUtility.PlayAnimation(heroInbattle.SwitchAnimation, PlayPortraitAnimationComplete, heroInbattle));
                }
                else
                {
                    PlayPortraitAnimationComplete(heroInbattle);
                }
                SetPortraitIcon(heroRow.IconId, heroInbattle.Icon);
                heroInbattle.ElementIcon.InitElementView(heroRow.ElementId);
                heroInbattle.Icon.gameObject.SetActive(false);
            }
            else
            {
                m_SwitchFirstHero = false;
                SetPortraitIcon(heroRow.IconId, heroInbattle.Icon);
                if (heroInbattle.IconMelee)
                {
                    heroInbattle.IconMelee.spriteName = heroInbattle.Icon.spriteName;
                }
                heroInbattle.ElementIcon.InitElementView(heroRow.ElementId);
            }
        }

        private void PlayPortraitAnimationComplete(object data)
        {
            var heroInBattle = data as HeroInBattle;
            if (heroInBattle == null)
            {
                return;
            }
            heroInBattle.PortraitOutObj.SetActive(false);
            heroInBattle.PortraitInObj.SetActive(false);
            heroInBattle.Icon.gameObject.SetActive(true);
        }

        private void CheckPropaganda()
        {
            if (m_PropagandaInstanceLogic == null)
            {
                m_PropagandaGroup.SetActive(false);
                return;
            }

            var currentPropagandaData = m_PropagandaInstanceLogic.CurrentPropagandaData;
            if (currentPropagandaData == null)
            {
                m_PropagandaGroup.SetActive(false);
                return;
            }

            OnPropagandaBegin(null, new InstancePropagandaBeginEventArgs(currentPropagandaData));
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                //当返回时播放摄像机动画，进行恢复
                if (!ShouldEnableJoystick())
                {
                    //GameEntry.TimeScale.ResumeGame();
                    OnResumeClick();
                }
                return;
            }

            if (!GameEntry.IsAvailable)
            {
                return;
            }

            if (GameEntry.SceneLogic.BaseInstanceLogic == null)
            {
                return;
            }

            if (!GameEntry.SceneLogic.BaseInstanceLogic.IsRunning)
            {
                return;
            }


            OnPauseClick();
        }


        private bool ShouldEnableJoystick()
        {
            return m_NormalRoot.activeInHierarchy;
        }
        public void SetAutoFightBtn(bool isAutoFightShow)
        {
            m_Auto.gameObject.SetActive(isAutoFightShow);
        }

        private void Awake()
        {
            UICamera.useButtonClickGapTime = false;
        }
        private void OnDestroy()
        {
            UICamera.useButtonClickGapTime = true;
        }
    }
}

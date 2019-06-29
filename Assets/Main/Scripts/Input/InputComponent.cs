using GameFramework;
using GameFramework.Event;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 输入组件。
    /// </summary>
    public class InputComponent : MonoBehaviour
    {
        [SerializeField]
        private EasyTouch m_Touch = null;

        [SerializeField]
        private EasyJoystick m_Joystick = null;

        [SerializeField]
        private bool[] m_SkillPressed = null;

        [SerializeField]
        private ThirdPersonController m_ThirdPersonController = new ThirdPersonController();

        private bool m_SkillActivated = false;

        public event GameFrameworkAction<Vector2> OnTouchBegan;

        public event GameFrameworkAction<float, float> OnPlayerTryingToMove;

        public event GameFrameworkAction<int, PerformSkillType> OnPlayerTryingToPerformSkill;

        public event GameFrameworkFunc<bool> ShouldEnableJoystick;

        public bool TouchEnabled
        {
            get
            {
                return m_Touch.enable;
            }
            set
            {
                m_Touch.enable = value;
            }
        }

        public bool JoystickEnabled
        {
            get
            {
                return m_Joystick.enable;
            }
            set
            {
                if (!value && m_Joystick.enable)
                {
                    On_JoystickMoveEnd(null);
                }

                m_Joystick.enable = value;
            }
        }

        public bool JoystickActivated
        {
            get
            {
                return m_Joystick.isActivated;
            }
            set
            {
                m_Joystick.isActivated = value;
            }
        }

        public bool JoystickVisible
        {
            get
            {
                return m_Joystick.visible;
            }
            set
            {
                m_Joystick.visible = value;
            }
        }

        public bool SkillActivated
        {
            get
            {
                return m_SkillActivated;
            }
            set
            {
                m_SkillActivated = value;
                if (UICamera.eventHandler != null)
                {
                    UICamera.eventHandler.allowMultiTouch = value;
                }
                if (!value)
                {
                    for (int i = 0; i < m_SkillPressed.Length; i++)
                    {
                        m_SkillPressed[i] = false;
                    }
                }
            }
        }

        public MeHeroCharacter MeHeroCharacter
        {
            get
            {
                return m_ThirdPersonController.MeHeroCharacter;
            }
            set
            {
                m_ThirdPersonController.MeHeroCharacter = value;
            }
        }

        public bool PerformSkill(int skillIndex, PerformSkillType performType)
        {
            if (MeHeroCharacter == null || MeHeroCharacter.Data.GetSkillIsLevelLocked(skillIndex))
            {
                return false;
            }

            if (OnPlayerTryingToPerformSkill != null)
            {
                OnPlayerTryingToPerformSkill(skillIndex, performType);
            }

            if (performType == PerformSkillType.Press)
            {
                m_SkillPressed[skillIndex] = true;
            }
            else if (performType == PerformSkillType.Release)
            {
                m_SkillPressed[skillIndex] = false;
            }

            return MeHeroCharacter.PerformSkillOnIndex(skillIndex, performType) != null;
        }

        public void SwitchHero(int index)
        {
            if (MeHeroCharacter == null)
            {
                return;
            }

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic is NonInstanceLogic)
            {
                Log.Warning("InstanceLogic is invalid.");
                return;
            }

            instanceLogic.RequestSwitchHero(index);
        }

        public bool IsSkillPressed(int skillIndex)
        {
            return m_SkillPressed[skillIndex];
        }

        public void RefreshJoystick()
        {
            OnRefreshJoystick(null, null);
        }

        private void Start()
        {
            if (m_Touch == null)
            {
                Log.Error("Easy touch is invalid.");
                return;
            }

            if (m_Joystick == null)
            {
                Log.Error("Easy joystick is invalid.");
                return;
            }

            m_SkillPressed = new bool[Constant.TotalSkillGroupCount];

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnRefreshJoystick);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnRefreshJoystick);

            Log.Info("Input component has been initialized.");
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnRefreshJoystick);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnRefreshJoystick);
            }
        }

        private void OnEnable()
        {
            EnableEasyTouchEvents();
            EnableEasyJoystickEvents();
        }

        private void OnDisable()
        {
            DisableEasyTouchEvents();
            DisableEasyJoystickEvents();
        }

        private void Update()
        {
            m_ThirdPersonController.Update();

            if (m_ThirdPersonController.IsTryingToMove && OnPlayerTryingToMove != null)
            {
                OnPlayerTryingToMove(m_ThirdPersonController.HorizontalRaw, m_ThirdPersonController.VerticalRaw);
            }

            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                if (touches[i].phase != TouchPhase.Began)
                {
                    continue;
                }

                if (OnTouchBegan != null)
                {
                    OnTouchBegan(touches[i].position);
                }
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            for (int i = 0; i < 3; i++)
            {
                if (!Input.GetMouseButtonDown(i))
                {
                    continue;
                }

                if (OnTouchBegan != null)
                {
                    OnTouchBegan(Input.mousePosition);
                }
            }

            if (m_Joystick == null)
            {
                return;
            }

            if (JoystickEnabled && JoystickActivated)
            {
                if (m_Joystick.JoystickAxis.sqrMagnitude <= 0f)
                {
                    m_ThirdPersonController.HorizontalRaw = Input.GetAxisRaw("Horizontal");
                    m_ThirdPersonController.VerticalRaw = Input.GetAxisRaw("Vertical");
                }

                if (m_SkillActivated)
                {
                    for (int i = 0; i < Constant.SkillGroupCount; i++)
                    {
                        if (Input.GetButtonDown(string.Format("Skill{0}", i.ToString())))
                        {
                            PerformSkill(i, PerformSkillType.Click);
                            PerformSkill(i, PerformSkillType.Press);
                        }
                        if (Input.GetButtonUp(string.Format("Skill{0}", i.ToString())))
                        {
                            PerformSkill(i, PerformSkillType.Release);
                        }
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.R))
            {
                GameEntry.Restart();
            }
#endif
        }

        private void EnableEasyTouchEvents()
        {
            EasyTouch.On_Cancel += On_Cancel;
            EasyTouch.On_Cancel2Fingers += On_Cancel2Fingers;
            EasyTouch.On_TouchStart += On_TouchStart;
            EasyTouch.On_TouchDown += On_TouchDown;
            EasyTouch.On_TouchUp += On_TouchUp;
            EasyTouch.On_SimpleTap += On_SimpleTap;
            EasyTouch.On_DoubleTap += On_DoubleTap;
            EasyTouch.On_LongTapStart += On_LongTapStart;
            EasyTouch.On_LongTap += On_LongTap;
            EasyTouch.On_LongTapEnd += On_LongTapEnd;
            EasyTouch.On_DragStart += On_DragStart;
            EasyTouch.On_Drag += On_Drag;
            EasyTouch.On_DragEnd += On_DragEnd;
            EasyTouch.On_SwipeStart += On_SwipeStart;
            EasyTouch.On_Swipe += On_Swipe;
            EasyTouch.On_SwipeEnd += On_SwipeEnd;
            EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
            EasyTouch.On_TouchDown2Fingers += On_TouchDown2Fingers;
            EasyTouch.On_TouchUp2Fingers += On_TouchUp2Fingers;
            EasyTouch.On_SimpleTap2Fingers += On_SimpleTap2Fingers;
            EasyTouch.On_DoubleTap2Fingers += On_DoubleTap2Fingers;
            EasyTouch.On_LongTapStart2Fingers += On_LongTapStart2Fingers;
            EasyTouch.On_LongTap2Fingers += On_LongTap2Fingers;
            EasyTouch.On_LongTapEnd2Fingers += On_LongTapEnd2Fingers;
            EasyTouch.On_Twist += On_Twist;
            EasyTouch.On_TwistEnd += On_TwistEnd;
            EasyTouch.On_PinchIn += On_PinchIn;
            EasyTouch.On_PinchOut += On_PinchOut;
            EasyTouch.On_PinchEnd += On_PinchEnd;
            EasyTouch.On_DragStart2Fingers += On_DragStart2Fingers;
            EasyTouch.On_Drag2Fingers += On_Drag2Fingers;
            EasyTouch.On_DragEnd2Fingers += On_DragEnd2Fingers;
            EasyTouch.On_SwipeStart2Fingers += On_SwipeStart2Fingers;
            EasyTouch.On_Swipe2Fingers += On_Swipe2Fingers;
            EasyTouch.On_SwipeEnd2Fingers += On_SwipeEnd2Fingers;
            EasyTouch.On_EasyTouchIsReady += On_EasyTouchIsReady;
        }

        private void DisableEasyTouchEvents()
        {
            EasyTouch.On_Cancel -= On_Cancel;
            EasyTouch.On_Cancel2Fingers -= On_Cancel2Fingers;
            EasyTouch.On_TouchStart -= On_TouchStart;
            EasyTouch.On_TouchDown -= On_TouchDown;
            EasyTouch.On_TouchUp -= On_TouchUp;
            EasyTouch.On_SimpleTap -= On_SimpleTap;
            EasyTouch.On_DoubleTap -= On_DoubleTap;
            EasyTouch.On_LongTapStart -= On_LongTapStart;
            EasyTouch.On_LongTap -= On_LongTap;
            EasyTouch.On_LongTapEnd -= On_LongTapEnd;
            EasyTouch.On_DragStart -= On_DragStart;
            EasyTouch.On_Drag -= On_Drag;
            EasyTouch.On_DragEnd -= On_DragEnd;
            EasyTouch.On_SwipeStart -= On_SwipeStart;
            EasyTouch.On_Swipe -= On_Swipe;
            EasyTouch.On_SwipeEnd -= On_SwipeEnd;
            EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
            EasyTouch.On_TouchDown2Fingers -= On_TouchDown2Fingers;
            EasyTouch.On_TouchUp2Fingers -= On_TouchUp2Fingers;
            EasyTouch.On_SimpleTap2Fingers -= On_SimpleTap2Fingers;
            EasyTouch.On_DoubleTap2Fingers -= On_DoubleTap2Fingers;
            EasyTouch.On_LongTapStart2Fingers -= On_LongTapStart2Fingers;
            EasyTouch.On_LongTap2Fingers -= On_LongTap2Fingers;
            EasyTouch.On_LongTapEnd2Fingers -= On_LongTapEnd2Fingers;
            EasyTouch.On_Twist -= On_Twist;
            EasyTouch.On_TwistEnd -= On_TwistEnd;
            EasyTouch.On_PinchIn -= On_PinchIn;
            EasyTouch.On_PinchOut -= On_PinchOut;
            EasyTouch.On_PinchEnd -= On_PinchEnd;
            EasyTouch.On_DragStart2Fingers -= On_DragStart2Fingers;
            EasyTouch.On_Drag2Fingers -= On_Drag2Fingers;
            EasyTouch.On_DragEnd2Fingers -= On_DragEnd2Fingers;
            EasyTouch.On_SwipeStart2Fingers -= On_SwipeStart2Fingers;
            EasyTouch.On_Swipe2Fingers -= On_Swipe2Fingers;
            EasyTouch.On_SwipeEnd2Fingers -= On_SwipeEnd2Fingers;
            EasyTouch.On_EasyTouchIsReady -= On_EasyTouchIsReady;
        }

        private void EnableEasyJoystickEvents()
        {
            EasyJoystick.On_JoystickTouchStart += On_JoystickTouchStart;
            EasyJoystick.On_JoystickMoveStart += On_JoystickMoveStart;
            EasyJoystick.On_JoystickMove += On_JoystickMove;
            EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
            EasyJoystick.On_JoystickTouchUp += On_JoystickTouchUp;
            EasyJoystick.On_JoystickTap += On_JoystickTap;
            EasyJoystick.On_JoystickDoubleTap += On_JoystickDoubleTap;
        }

        private void DisableEasyJoystickEvents()
        {
            EasyJoystick.On_JoystickTouchStart -= On_JoystickTouchStart;
            EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
            EasyJoystick.On_JoystickMove -= On_JoystickMove;
            EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
            EasyJoystick.On_JoystickTouchUp -= On_JoystickTouchUp;
            EasyJoystick.On_JoystickTap -= On_JoystickTap;
            EasyJoystick.On_JoystickDoubleTap -= On_JoystickDoubleTap;
        }

        private void On_Cancel(Gesture gesture)
        {

        }

        private void On_Cancel2Fingers(Gesture gesture)
        {

        }

        private void On_TouchStart(Gesture gesture)
        {

        }

        private void On_TouchDown(Gesture gesture)
        {

        }

        private void On_TouchUp(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new TouchUpEventArgs(gesture.position));
        }

        private void On_SimpleTap(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new SimpleTapEventArgs(gesture.position));
        }

        private void On_DoubleTap(Gesture gesture)
        {

        }

        private void On_LongTapStart(Gesture gesture)
        {

        }

        private void On_LongTap(Gesture gesture)
        {

        }

        private void On_LongTapEnd(Gesture gesture)
        {

        }

        private void On_DragStart(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new DragStartEventArgs(gesture.startPosition, gesture.swipeVector, gesture.deltaPosition));
        }

        private void On_Drag(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new DragEventArgs(gesture.startPosition, gesture.swipeVector, gesture.deltaPosition));
        }

        private void On_DragEnd(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new DragEndEventArgs(gesture.startPosition, gesture.swipeVector, gesture.deltaPosition));
        }

        private void On_SwipeStart(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new SwipeStartEventArgs(gesture.startPosition, gesture.swipeVector, gesture.deltaPosition));
        }

        private void On_Swipe(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new SwipeEventArgs(gesture.startPosition, gesture.swipeVector, gesture.deltaPosition));
        }

        private void On_SwipeEnd(Gesture gesture)
        {
            GameEntry.Event.Fire(this, new SwipeEndEventArgs(gesture.startPosition, gesture.swipeVector, gesture.deltaPosition));
        }

        private void On_TouchStart2Fingers(Gesture gesture)
        {

        }

        private void On_TouchDown2Fingers(Gesture gesture)
        {

        }

        private void On_TouchUp2Fingers(Gesture gesture)
        {

        }

        private void On_SimpleTap2Fingers(Gesture gesture)
        {

        }

        private void On_DoubleTap2Fingers(Gesture gesture)
        {

        }

        private void On_LongTapStart2Fingers(Gesture gesture)
        {

        }

        private void On_LongTap2Fingers(Gesture gesture)
        {

        }

        private void On_LongTapEnd2Fingers(Gesture gesture)
        {

        }

        private void On_Twist(Gesture gesture)
        {

        }

        private void On_TwistEnd(Gesture gesture)
        {

        }

        private void On_PinchIn(Gesture gesture)
        {

        }

        private void On_PinchOut(Gesture gesture)
        {

        }

        private void On_PinchEnd(Gesture gesture)
        {

        }

        private void On_DragStart2Fingers(Gesture gesture)
        {

        }

        private void On_Drag2Fingers(Gesture gesture)
        {

        }

        private void On_DragEnd2Fingers(Gesture gesture)
        {

        }

        private void On_SwipeStart2Fingers(Gesture gesture)
        {

        }

        private void On_Swipe2Fingers(Gesture gesture)
        {

        }

        private void On_SwipeEnd2Fingers(Gesture gesture)
        {

        }

        private void On_EasyTouchIsReady()
        {
            Log.Info("Easy touch is ready.");
        }

        private void On_JoystickDoubleTap(MovingJoystick move)
        {
            //Log.Debug("Joystick double tap.");
        }

        private void On_JoystickTap(MovingJoystick move)
        {
            //Log.Debug("Joystick tap.");
        }

        private void On_JoystickTouchUp(MovingJoystick move)
        {
            //Log.Debug("Joystick touch up.");
        }

        private void On_JoystickMoveEnd(MovingJoystick move)
        {
            //Log.Debug("Joystick move end.");
            m_ThirdPersonController.HorizontalRaw = m_ThirdPersonController.VerticalRaw = 0f;
        }

        private void On_JoystickMove(MovingJoystick move)
        {
            //Log.Debug("Joystick move.");

            m_ThirdPersonController.HorizontalRaw = move.joystickAxis.x;
            m_ThirdPersonController.VerticalRaw = move.joystickAxis.y;
        }

        private void On_JoystickMoveStart(MovingJoystick move)
        {
            //Log.Debug("Joystick move start.");
        }

        private void On_JoystickTouchStart(MovingJoystick move)
        {
            //Log.Debug("Joystick touch start.");
        }

        private void OnRefreshJoystick(object sender, GameEventArgs e)
        {
            IUIGroup defaultUIGroup = GameEntry.UI.GetUIGroup("Default");

            if (defaultUIGroup == null || defaultUIGroup.CurrentUIForm == null)
            {
                JoystickEnabled = false;
                return;
            }

            UIFormLogic currentUIForm = (defaultUIGroup.CurrentUIForm as UIForm).Logic as UIFormLogic;
            if (currentUIForm == null)
            {
                JoystickEnabled = false;
                return;
            }

            m_Touch.enabledNGuiMode = currentUIForm is MainForm;

            var uiGroups = GameEntry.UI.GetAllUIGroups();
            for (int i = 0; i < uiGroups.Length; ++i)
            {
                var uiGroup = uiGroups[i];
                if (uiGroup != defaultUIGroup && uiGroup.CurrentUIForm != null)
                {
                    JoystickEnabled = false;
                    return;
                }
            }

            JoystickEnabled = currentUIForm is MainForm || currentUIForm is BattleForm;

            if (ShouldEnableJoystick == null)
            {
                return;
            }

            var invocationList = ShouldEnableJoystick.GetInvocationList();
            for (int i = 0; i < invocationList.Length && JoystickEnabled; ++i)
            {
                JoystickEnabled &= (invocationList[i] as GameFrameworkFunc<bool>).Invoke();
            }
        }
    }
}

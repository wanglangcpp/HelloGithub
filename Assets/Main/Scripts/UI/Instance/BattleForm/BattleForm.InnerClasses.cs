using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BattleForm
    {
        [Serializable]
        private class WaitingForStart
        {
            public Animation StartAnim = null;
            public Animation TimerAnim = null;
            public UILabel[] TimerNumbers = null;

            private int m_Number = -1;

            public void SetNumber(float floatNum)
            {
                int number = Mathf.CeilToInt(floatNum);

                if (number == m_Number)
                {
                    return;
                }

                if (number <= 0 && m_Number > 0)
                {
                    StartAnim.gameObject.SetActive(true);
                    StartAnim.Play();
                }

                m_Number = number;
                if (m_Number <= 0)
                {
                    TimerAnim.gameObject.SetActive(false);
                    return;
                }

                TimerAnim.gameObject.SetActive(true);
                for (int i = 0; i < TimerNumbers.Length; ++i)
                {
                    TimerNumbers[i].text = m_Number.ToString();
                }

                TimerAnim.Rewind();
                TimerAnim.Play();
            }

            public void Reset()
            {
                StartAnim.gameObject.SetActive(false);
                TimerAnim.gameObject.SetActive(false);
                m_Number = -1;
            }

            public void ResetIfComplete()
            {
                if (m_Number <= 0 && !StartAnim.isPlaying)
                {
                    Reset();
                }
            }
        }

        [Serializable]
        private class HeroInBattle
        {
            public UISprite Icon = null;
            public UIProgressBar HpBar = null;
            public UIProgressBar SteadyBar = null;
            public UIProgressBar SteadyRecoverBar = null;
            public Animation SwitchAnimation = null;
            public GameObject PortraitObj = null;
            public GameObject PortraitOutObj = null;
            public GameObject PortraitInObj = null;
            public GameObject PortraitMeleeObj = null;
            public UISprite IconOut = null;
            public UISprite IconIn = null;
            public UISprite IconMelee = null;
            public GameObject Root = null;
            public GoodsView ElementIcon = null;
            public GoodsView ElementIconOut = null;
            public GoodsView ElementIconIn = null;
            public GoodsView ElementIconMelee = null;
        }

        [Serializable]
        private class SkillButton
        {
            private BattleForm m_BattleForm = null;

            public Transform Root = null;
            public UIButton Button = null;
            public UIIntKey Key = null;
            public UISprite Icon = null;
            public UISprite CoolDownMask = null;
            public UILabel CoolDownTime = null;
            public Animation Animation = null;
            public Animation Effect = null;
            public UISpriteAnimationEx[] LockAnimation = null;
            [HideInInspector]
            public bool IsLock = true;
            public UISprite CDProgress = null;
            public UIProgressCircle CDDecal = null;
            public UISprite LevelLock = null;
            public UISprite SkillCategoryIcon = null;

            public void OnOpen(BattleForm battleForm)
            {
                if (Root == null) return;
                m_BattleForm = battleForm;
                UIEventListener.Get(Button.gameObject).onPress += OnSkillPress;
            }

            public void OnClose()
            {
                if (Root == null) return;
                m_BattleForm = null;
                UIEventListener.Get(Button.gameObject).onPress -= OnSkillPress;
            }

            public void OnSkillPress(GameObject go, bool pressed)
            {
                if (Root == null) return;

                var skillIndex = Key.Key;
                if (m_BattleForm.MyHeroCharacter == null)
                {
                    return;
                }

                var heroData = m_BattleForm.MyHeroCharacter.Data;
                if (heroData.GetSkillIsLevelLocked(skillIndex))
                {
                    return;
                }

                if (pressed)
                {
                    m_BattleForm.OnSkillPress(Key);
                }
                else
                {
                    m_BattleForm.OnSkillRelease(Key);
                }
            }

            public void InitLevelLock(bool shouldLock)
            {
                if (Root == null)
                {
                    return;
                }

                Button.isEnabled = !shouldLock;
                if (LevelLock != null)
                {
                    LevelLock.gameObject.SetActive(shouldLock);
                }
            }

            public void InitSkillCategoryIcon(SkillCategory skillCategory)
            {
                if (SkillCategoryIcon != null)
                {
                    SkillCategoryIcon.gameObject.SetActive(skillCategory > SkillCategory.Undefined);
                    if (skillCategory == SkillCategory.ContinualTapSkill)
                    {
                        SkillCategoryIcon.spriteName = SkillCategory.ContinualTapSkill.ToString();
                    }
                    else if (skillCategory == SkillCategory.ChargeSkill)
                    {
                        SkillCategoryIcon.spriteName = SkillCategory.ChargeSkill.ToString();
                    }
                    else
                    {
                        SkillCategoryIcon.spriteName = SkillCategory.SwitchSkill.ToString();
                    }
                }
            }
        }

        [Serializable]
        private class HeroButton
        {
            public UIButton Button = null;
            public UISprite Portrait = null;
            public UISprite HPBar = null;
            public GameObject SteadyPanel = null;
            public UISprite SteadyBar = null;
            public UISprite SteadyRecoverBar = null;
            public UISprite CoolDownMask = null;
            public UILabel CoolDownTime = null;
            public UISprite SkillReady = null;
            public GameObject CanBattleObj = null;
            public UISprite CDProgress = null;
            public UIProgressCircle CDDecal = null;
            public GoodsView ElementIcon = null;
        }

        [Serializable]
        private class DodgeButton
        {
            public UISprite EnergyBar = null;
        }

        [Serializable]
        private class MeleeRank
        {
            public UILabel CampName = null;
            public UILabel CampRank = null;

            public void RefreshRank(string name, int rank)
            {
                CampName.text = name;
                CampRank.text = rank.ToString();
            }
        }

        [Serializable]
        private class PvpInfo
        {
            public GameObject Panel = null;
            public NetworkStatus Status = null;
            public UILabel OppoName = null;
            public PvpPlayerInBattle PlayerInBattle = null;
        }

        [Serializable]
        private class PvpPlayerInBattle
        {
            public GameObject Root = null;
            public HeroInBattle OpponentInBattle = null;
            public HeroInBattle SelfInBattle = null;
            public PvpBgHeroInfo[] OpponentBgHeros = null;
        }

        [Serializable]
        private class PvpBgHeroInfo
        {
            public UISprite HeroIcon = null;
            public UISprite HpBar = null;
            public GameObject DeathBar = null;
            public GameObject Root = null;
            public int HeroId = 0;
        }

        [Serializable]
        private class NetworkStatus
        {
            public Color GoodColor = Color.green;
            public Color NormalColor = Color.yellow;
            public Color BadColor = Color.red;
            public float PingInterval = 3;
            public int GoodDelay = 20;
            public int BadDelay = 100;
            public UISprite Wifi = null;
            public UISprite Cellular = null;
            public UISprite NotAvailable = null;
            public UILabel Delay = null;
        }

        [Serializable]
        private class ContinualTapSkillNormalDisplay
        {
            public GameObject Root = null;
            public Animation BgAnimation = null;
            public GameObject Decal = null;
            public UISprite Progress = null;
        }

        private class SkillContinualTapDisplayer
        {
            internal int SkillIndex { get; private set; }

            private BattleForm m_Owner;
            private ContinualTapSkillItem[] m_SkillItems = new ContinualTapSkillItem[0];
            private bool m_Success = true;
            private int m_SkillButtonEffectId = -1;
            private DRSkillContinualTap m_DRSkillContinualTap = null;
            private float m_LastTime = 0f;
            private float m_Time = 0f;
            private int m_CurrentIndex = 0;

            internal bool IsShowing
            {
                get
                {
                    return NormalDisplay.Root.activeSelf;
                }
            }

            private UIEffectsController EffectsController
            {
                get
                {
                    return m_Owner.m_EffectsController;
                }
            }

            private ContinualTapSkillNormalDisplay NormalDisplay
            {
                get
                {
                    return m_Owner.m_ContinualTapSkillNormalDisplay;
                }
            }

            private GameObject ItemTemplate
            {
                get
                {
                    return m_Owner.m_ContinualTapSkillItemTempate;
                }
            }

            private float ItemOffset
            {
                get
                {
                    return m_Owner.m_ContinualTapSkillItemOffset;
                }
            }

            internal SkillContinualTapDisplayer(BattleForm owner)
            {
                m_Owner = owner;
            }

            internal void Show(int skillIndex, int skillId)
            {
                if (IsShowing)
                {
                    return;
                }

                var dt = GameEntry.DataTable.GetDataTable<DRSkillContinualTap>();
                m_DRSkillContinualTap = dt.GetDataRow(skillId);
                if (m_DRSkillContinualTap == null)
                {
                    return;
                }

                SkillIndex = skillIndex;
                ClearSkillButtonEffect();
                NormalDisplay.Root.SetActive(true);
                NormalDisplay.Progress.fillAmount = 0f;
                m_SkillButtonEffectId = EffectsController.ShowEffect(string.Format("EffectSkill{0}ContinualTap", skillIndex));
                m_LastTime = m_Time = 0f;
                m_CurrentIndex = 0;
                m_Success = true;
                InitSkillItems();
            }

            internal void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_DRSkillContinualTap == null)
                {
                    return;
                }

                if (!m_Success)
                {
                    Hide();
                    return;
                }

                m_LastTime = m_Time;
                m_Time += elapseSeconds;

                if (m_Time < m_DRSkillContinualTap.ContinualTapStartTime)
                {
                    NormalDisplay.Progress.fillAmount = 0f;
                }
                else if (m_Time > m_DRSkillContinualTap.ContinualTapStartTime + m_DRSkillContinualTap.ContinualTapDuration)
                {
                    NormalDisplay.Progress.fillAmount = 1f;
                }
                else
                {
                    NormalDisplay.Progress.fillAmount = m_DRSkillContinualTap.ContinualTapDuration <= 0f ? 1f : (m_Time - m_DRSkillContinualTap.ContinualTapStartTime) / m_DRSkillContinualTap.ContinualTapDuration;
                }

                if (m_LastTime < m_DRSkillContinualTap.ContinualTapStartTime + m_DRSkillContinualTap.ContinualTapDuration &&
                    m_Time >= m_DRSkillContinualTap.ContinualTapStartTime + m_DRSkillContinualTap.ContinualTapDuration)
                {
                    // Highlight the first item again in the end.
                    if (m_SkillItems.Length > 0)
                    {
                        m_SkillItems[0].ShowForeground();
                    }
                }
            }

            internal void PlayTapAnimation()
            {
                if (!m_Success)
                {
                    return;
                }

                NormalDisplay.BgAnimation.Play();
            }

            internal void Hide()
            {
                if (!IsShowing)
                {
                    return;
                }

                NormalDisplay.Root.SetActive(false);
                m_DRSkillContinualTap = null;
                ClearSkillButtonEffect();
                ClearSkillItems();

                if (m_Success)
                {
                    EffectsController.ShowEffect("EffectSkillContinualTapComplete");
                }
            }

            internal void SetFailure()
            {
                m_Success = false;
            }

            internal void ShutDown()
            {
                m_DRSkillContinualTap = null;
                ClearSkillButtonEffect();
                ClearSkillItems();
                m_Owner = null;
            }

            internal void IncrementProgress()
            {
                if (m_CurrentIndex < m_SkillItems.Length)
                {
                    m_SkillItems[m_CurrentIndex++].ShowForeground();
                }
            }

            private void ClearSkillButtonEffect()
            {
                if (m_SkillButtonEffectId >= 0)
                {
                    EffectsController.DestroyEffect(m_SkillButtonEffectId);
                    m_SkillButtonEffectId = -1;
                }
            }

            private void ClearSkillItems()
            {
                for (int i = 0; i < m_SkillItems.Length; ++i)
                {
                    Destroy(m_SkillItems[i].gameObject);
                }

                m_SkillItems = new ContinualTapSkillItem[0];
            }

            private void InitSkillItems()
            {
                m_SkillItems = new ContinualTapSkillItem[m_DRSkillContinualTap.IntervalCount];

                for (int i = 0; i < m_SkillItems.Length; ++i)
                {
                    var go = NGUITools.AddChild(NormalDisplay.Root.gameObject, ItemTemplate);
                    m_SkillItems[i] = go.GetComponent<ContinualTapSkillItem>();

                    var transform = go.transform;
                    var radians = Mathf.PI * 2 / m_SkillItems.Length * i;
                    var x = Mathf.Sin(radians) * ItemOffset;
                    var y = Mathf.Cos(radians) * ItemOffset;
                    transform.localPosition = new Vector3(x, y, transform.localPosition.z);
                }

                // Show the first item foreground at the beginning.
                IncrementProgress();
            }
        }
    }
}

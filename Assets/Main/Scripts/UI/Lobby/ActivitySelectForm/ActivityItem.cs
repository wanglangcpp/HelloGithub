using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivityItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_RemainingPlayCountLbl = null;

        [SerializeField]
        private UILabel m_UnlockLevelLbl = null;

        [SerializeField]
        private UITexture m_ContentTexture = null;

        [SerializeField]
        private UILabel m_DescLbl = null;

        [SerializeField]
        private UILabel m_NameLbl = null;

        [SerializeField]
        private UIButton m_SelfButton = null;

        [SerializeField]
        private Color m_LockColor = new Color32(255, 0, 0, 255);

        [SerializeField]
        private UIWidget[] m_ChangeColorWidgets = null;

        //[SerializeField]
        //private UIEffectsController m_Effect = null;

        private DRActivity m_CachedDRActivity = null;
        private int m_RemainingPlayCount = 0;

        private const int LockedTextureId = 20000;

        private ActivitySelectForm m_Form = null;

        private ActivitySelectForm Form
        {
            get
            {
                if (m_Form == null)
                {
                    m_Form = GetComponentInParent<ActivitySelectForm>();
                }

                return m_Form;
            }
        }

        public delegate void GetPlayCountDelegate(out int remainingPlayCount, out int freePlayCount);

        private float m_TimeCounter = 60f;
        private readonly float RefreshTimeInterval = 60;

        private void Start()
        {
            //m_Effect.Resume();
        }

        public void RefreshData(DRActivity drActivity, GetPlayCountDelegate getPlayCountDelegate)
        {
            if (getPlayCountDelegate == null)
            {
                Log.Error("getPlayCountDelegate cannot be null.");
                return;
            }

            int remainingPlayCount, freePlayCount;
            getPlayCountDelegate(out remainingPlayCount, out freePlayCount);




            if (GameEntry.Data.Player.Level < drActivity.UnlockLevel)
            {
                m_RemainingPlayCountLbl.text = string.Empty;
                m_UnlockLevelLbl.text = GameEntry.Localization.GetString("UI_TEXT_ACTIVITY_OPEN_LEVEL", drActivity.UnlockLevel.ToString());
                m_DescLbl.text = string.Empty;
                //m_ContentTexture.LoadAsync(LockedTextureId);
                //m_ContentTexture.LoadAsync(drActivity.TextureId);
                SetColor(m_LockColor);
                m_SelfButton.isEnabled = false;
            }
            else
            {
                if (remainingPlayCount > 0)
                {
                    m_RemainingPlayCountLbl.text = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_NUMBER", remainingPlayCount);
                }
                else
                {
                    m_RemainingPlayCountLbl.text = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_RUN_OUT");
                }


                m_UnlockLevelLbl.text = string.Empty;
                m_DescLbl.text = GameEntry.Localization.GetString(drActivity.Description);
                //m_ContentTexture.LoadAsync(drActivity.TextureId);
                m_SelfButton.isEnabled = true;
            }
            m_ContentTexture.LoadAsync(drActivity.TextureId);
            m_RemainingPlayCount = remainingPlayCount;
            m_NameLbl.text = GameEntry.Localization.GetString(drActivity.Name);
            m_CachedDRActivity = drActivity;
        }

        private void SetColor(Color color)
        {
            for (int i = 0; i < m_ChangeColorWidgets.Length; i++)
            {
                m_ChangeColorWidgets[i].color = color;
            }
        }

        // Called by NGUI via reflection.
        public void OnClickSelf()
        {
            if (m_RemainingPlayCount <= 0 && !m_CachedDRActivity.AllowOpenOnNoRemainingPlayCount)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_RUN_OUT"));
                return;
            }

            if (!IsActivityActive)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString(m_CachedDRActivity.ActiveTimeDesc));
                return;
            }

            Form.SelectActivity(m_CachedDRActivity.Id);
        }

        private void Update()
        {
            m_TimeCounter += Time.deltaTime;
            if (m_TimeCounter < RefreshTimeInterval || m_CachedDRActivity == null)
                return;

            if (IsActivityActive && m_CachedDRActivity.UnlockLevel <= GameEntry.Data.Player.Level)
            {
                //m_Effect.ShowEffect("EffectActivityItem1");

                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    //                     if (GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer))
                    //                     {
                    //                         m_Effect.ShowEffect("EffectActivityItemClick");
                    //                     }
                }
            }
            else
            {
                //m_Effect.DestroyEffect("EffectActivityItem1");
            }
            m_TimeCounter = 0;
        }

        private void OnDisable()
        {
            //m_Effect.DestroyEffect("EffectActivityItem1");
            m_TimeCounter = RefreshTimeInterval;
        }

        private bool IsActivityActive
        {
            get
            {
                int periodCount = m_CachedDRActivity.StartTimes.Length;
                int dayOfWeekToday = (int)(GameEntry.Time.LobbyServerUtcTime.Date.DayOfWeek);
                bool activeToday = m_CachedDRActivity.ActiveOnWeekDays[dayOfWeekToday];

                // 全天开放。
                if (periodCount <= 0)
                {
                    return activeToday;
                }

                for (int i = 0; i < periodCount; i++)
                {
                    var startTime = m_CachedDRActivity.StartTimes[i];
                    var endTime = m_CachedDRActivity.EndTimes[i];

                    if (startTime <= endTime) // 未跨天。
                    {
                        var realStartTime = GameEntry.Time.LobbyServerUtcTime.Date.Add(startTime);
                        var realEndTime = GameEntry.Time.LobbyServerUtcTime.Date.Add(endTime);
                        if (activeToday && realStartTime <= GameEntry.Time.LobbyServerUtcTime && GameEntry.Time.LobbyServerUtcTime <= realEndTime)
                        {
                            return true;
                        }
                    }
                    else // 跨天。
                    {
                        int dayOfWeekPrevDay = (int)(GameEntry.Time.LobbyServerUtcTime.Date.DayOfWeek) - 1;
                        if (dayOfWeekPrevDay < 0) dayOfWeekPrevDay += 7;
                        bool activePrevDay = m_CachedDRActivity.ActiveOnWeekDays[dayOfWeekPrevDay];

                        if (GameEntry.Time.LobbyServerUtcTime.TimeOfDay >= startTime) // 仍在当天。
                        {
                            if (activeToday) return true;
                        }

                        if (GameEntry.Time.LobbyServerUtcTime.TimeOfDay <= endTime) // 已进入第二天。
                        {
                            if (activePrevDay) return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}

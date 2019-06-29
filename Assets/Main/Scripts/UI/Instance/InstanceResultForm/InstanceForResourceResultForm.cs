using System;
using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class InstanceForResourceResultForm : NGUIForm
    {
        [SerializeField]
        private float m_MinDisplayTime = .3f;

        [SerializeField]
        private UILabel m_CoinLabel = null;

        [SerializeField]
        private UILabel m_TitleLabel = null;

        [SerializeField]
        private UISprite m_CoinIcon = null;

        private bool m_OnPostOpenCalled = false;
        private float m_CurrentDisplayTime = 0f;
        private InstanceForResourceResultData m_DisplayData = null;

        public void OnClickWholeScreenBtn()
        {
            if (m_CurrentDisplayTime <= m_MinDisplayTime)
            {
                return;
            }

            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySelectForm, true);
            GameEntry.SceneLogic.GoBackToLobby(false);
        }

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            m_OnPostOpenCalled = false;
            base.OnOpen(userData);
            m_DisplayData = userData as InstanceForResourceResultData;
            if (m_DisplayData == null)
            {
                Log.Error("Display data for InstanceForResourceResultForm is invalid.");
                return;
            }

            var instanceForResourceId = m_DisplayData.InstanceForResourceId;
            var drInstance = GameEntry.DataTable.GetDataTable<DRInstanceForResource>().GetDataRow(instanceForResourceId);
            string activityName = GetActivityName(drInstance.InstanceResourceType);
            m_TitleLabel.text = GameEntry.Localization.GetString("UI_TEXT_REWARD_FROM_ACTIVITY", activityName);
            m_CoinLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", m_DisplayData.CoinObtained);
            switch (drInstance.InstanceResourceType)
            {
                case InstanceForResourceType.Coin:
                    m_CoinIcon.spriteName = "icon_gold";
                    break;
                case InstanceForResourceType.Exp:
                    m_CoinIcon.spriteName = "icon_exp";
                    break;
                default:
                    break;
            }
            
        }

        private string GetActivityName(InstanceForResourceType instanceResourceType)
        {
            ActivityType activityType = (instanceResourceType == InstanceForResourceType.Coin ? ActivityType.InstanceForResource_Coin : ActivityType.InstanceForResource_Exp);
            var drActivity = GameEntry.DataTable.GetDataTable<DRActivity>().GetDataRow((int)activityType);
            if (drActivity == null)
            {
                return string.Empty;
            }

            return GameEntry.Localization.GetString(drActivity.Name);
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            m_OnPostOpenCalled = true;
            m_CurrentDisplayTime = 0f;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_OnPostOpenCalled)
            {
                m_CurrentDisplayTime += realElapseSeconds;
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            m_OnPostOpenCalled = false;
        }

        #endregion NGUIForm
    }
}

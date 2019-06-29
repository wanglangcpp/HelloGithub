using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝活动界面。
    /// </summary>
    public class ActivityCosmosForm : NGUIForm
    {
        [SerializeField]
        private InstanceItem[] m_InstanceItems = null;

        [SerializeField]
        private UILabel m_ChallengeCountLabel = null;

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ResetDisplay();
            GameEntry.Event.Subscribe(EventId.CosmosCrackDataChanged, OnCosmosCrackDataChanged);
            GameEntry.LobbyLogic.CosmosCrack.RequestData();
        }

        protected override void OnClose(object userData)
        {
            ResetDisplay();

            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.CosmosCrackDataChanged, OnCosmosCrackDataChanged);
            }

            base.OnClose(userData);
        }

        #endregion NGUIForm

        // Called by NGUI via reflection.
        public void OnClickExchangeButton()
        {
            // TODO: Implement.
        }

        // Called by NGUI via reflection.
        public void OnClickChallengeButton(int index)
        {
            if (RemainingChallengeCount < 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_RUN_OUT") });
                return;
            }

            var data = GameEntry.Data.CosmosCrackData;
            var instanceDatas = data.GetInstanceDatas();

            if (index >= instanceDatas.Count)
            {
                Log.Warning("Instance on index '{0}' can not be found.", index);
                return;
            }

            var instanceData = instanceDatas[index];
            var rawRewardDatas = new List<PBItemInfo>();
            var rewardDatas = instanceData.GetRewards();

            for (int i = 0; i < rewardDatas.Count; ++i)
            {
                var rewardData = rewardDatas[i];

                rawRewardDatas.Add(new PBItemInfo
                {
                    Type = rewardData.GeneralItemId,
                    Count = rewardData.GeneralItemCount,
                });
            }

            GameEntry.Data.InstanceGoods.ClearAndAddData(rawRewardDatas);
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenCosmosCrack, true);
            GameEntry.LobbyLogic.CosmosCrack.RequestEnterInstance(instanceData.InstanceId);
        }

        private void ResetDisplay()
        {
            for (int i = 0; i < m_InstanceItems.Length; ++i)
            {
                var instanceItem = m_InstanceItems[i];
                instanceItem.ResetDisplay();
            }
        }

        private int RemainingChallengeCount
        {
            get
            {
                var data = GameEntry.Data.CosmosCrackData;
                int totalChallengeCount = 10; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.CosmosCrackRoundLimit, 10);
                return totalChallengeCount - data.UsedRoundCount;
            }
        }

        private void RefreshDisplay()
        {
            m_ChallengeCountLabel.text = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_NUMBER", RemainingChallengeCount.ToString());

            var data = GameEntry.Data.CosmosCrackData;
            var instanceDatas = data.GetInstanceDatas();

            for (int i = 0; i < m_InstanceItems.Length; ++i)
            {
                var instanceItem = m_InstanceItems[i];
                if (i < instanceDatas.Count)
                {
                    instanceItem.RefreshDisplay(instanceDatas[i]);
                }
                else
                {
                    instanceItem.RefreshDisplay(null);
                }
            }
        }

        private void OnCosmosCrackDataChanged(object sender, GameEventArgs e)
        {
            RefreshDisplay();
        }

        [Serializable]
        private class InstanceItem
        {
            public Transform Self = null;
            public UILabel Name = null;
            public UILabel Desc = null;
            public UITexture Texture = null;
            public Transform[] RewardLevels = null;
            public GoodsView[] RewardIcons = null;

            internal void ResetDisplay()
            {
                Name.text = string.Empty;
                Desc.text = string.Empty;

                for (int i = 0; i < RewardLevels.Length; ++i)
                {
                    RewardLevels[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < RewardIcons.Length; ++i)
                {
                    RewardIcons[i].gameObject.SetActive(false);
                }

                Self.gameObject.SetActive(true);
            }

            internal void RefreshDisplay(CosmosCrackInstanceData instanceData)
            {
                if (instanceData == null)
                {
                    Self.gameObject.SetActive(false);
                    return;
                }

                int instanceTypeId = instanceData.InstanceId;
                var dtInstance = GameEntry.DataTable.GetDataTable<DRInstanceCosmosCrack>();
                DRInstanceCosmosCrack drInstance = dtInstance.GetDataRow(instanceTypeId);
                if (drInstance == null)
                {
                    Log.Error("Instance '{0}' not found.", instanceTypeId);
                    return;
                }

                Name.text = GameEntry.Localization.GetString(drInstance.Name);
                Desc.text = GameEntry.Localization.GetString(drInstance.Description);
                // TODO: Texture.mainTexture.LoadAsync(...);

                for (int i = 0; i < RewardLevels.Length; ++i)
                {
                    RewardLevels[i].gameObject.SetActive(i == instanceData.RewardLevel - 1);
                }

                var rewardDatas = instanceData.GetRewards();
                for (int i = 0; i < RewardIcons.Length; ++i)
                {
                    var rewardIcon = RewardIcons[i];
                    if (i < rewardDatas.Count)
                    {
                        rewardIcon.gameObject.SetActive(true);
                        //var rewardData = rewardDatas[i];
                        //rewardIcon.InitGoodsView(rewardData.GeneralItemId);
                    }
                    else
                    {
                        rewardIcon.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}

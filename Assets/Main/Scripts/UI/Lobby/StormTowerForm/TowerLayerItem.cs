using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class TowerLayerItem : MonoBehaviour
    {
        /// <summary>
        /// 战斗图标
        /// </summary>
        [SerializeField]
        private GameObject m_IconFight = null;
        /// <summary>
        /// 位置图标
        /// </summary>
        [SerializeField]
        private GameObject m_IconPostion = null;
        /// <summary>
        /// 宝箱
        /// </summary
        [SerializeField]
        private GameObject m_BtnBox = null;
        /// <summary>
        /// 已领取
        /// </summary>
        [SerializeField]
        private UISprite m_IconReveived = null;
        /// <summary>
        /// 好友头像
        /// </summary>
        [SerializeField]
        private UISprite m_IconPlaye = null;
        /// <summary>
        /// 楼梯
        /// </summary>
        [SerializeField]
        private GameObject m_Stairs = null;
        /// <summary>
        /// 当前层
        /// </summary>
        [SerializeField]
        private UILabel m_CurrentLay = null;

        private DRInstanceForTower drTowerLayer = null;

        private PBInstanceForTowerInfo stromTowerData;

        /// <summary>
        /// 宝箱状态
        /// </summary>
        private int BoxStatus;
        private bool IsCurrentPos = false;

        void Awake()
        {
            UIEventListener.Get(m_BtnBox).onClick = OnClickBoxButton;
        }
        /// <summary>
        /// 刷新当前楼层信息
        /// </summary>
        /// <param name="drInstance">层数</param>
        /// <param name="playerIcon">挑战至该层的好友信息</param>
        public void RefreshLayerData(DRInstanceForTower drInstance, int playerIcon = 0)
        {
            stromTowerData = GameEntry.Data.StromTowerData.StromTowerInfo;
            drTowerLayer = drInstance;
            if (stromTowerData != null)
            {
                //IsCurrentPos = stromTowerData.CurLayerNum > 0 ?
                //    stromTowerData.CurLayerNum == drTowerLayer.Id : 1 == drTowerLayer.Id;
                IsCurrentPos = stromTowerData.CurLayerNum + 1 == drTowerLayer.Id;
                if (drTowerLayer.IsHaveBox)
                {
                    SetBoxStatus(drInstance.Id, stromTowerData.Chest);
                }
            }

            if (playerIcon == 0)
                m_IconPlaye.gameObject.SetActive(false);
            else
                m_IconPlaye.LoadAsync(playerIcon);
            m_Stairs.gameObject.SetActive((drTowerLayer.OpenInstance != -1));
            m_CurrentLay.text = GameEntry.Localization.GetString("UI_TEXT_TOWER_LEVEL", drTowerLayer.Id);
            m_BtnBox.SetActive(drTowerLayer.IsHaveBox);

            m_IconPostion.gameObject.SetActive(IsCurrentPos);
            m_IconFight.gameObject.SetActive(IsCurrentPos);

        }
        /// <summary>
        /// 设置宝箱状态(0-不可领取，1-可以领取，2-已领取)
        /// </summary>
        /// <param name="boxStatus"></param>
        public void SetBoxStatus(int layerId, int boxStatus)
        {
            if (!drTowerLayer.IsHaveBox)
                return;
            int thisStatus = 0;
            bool canClaim = (boxStatus & (1 << (layerId / 5))) == 0;
            thisStatus = canClaim ? layerId <= stromTowerData.MaxLayerNum ? 1 : 0 : 2;
            //if (canClaim)
            //{
            //    if (layerId <= stromTowerData.MaxLayerNum)
            //        thisStatus = 1;
            //    else
            //        thisStatus = 0;
            //}
            //else
            //    thisStatus = 2;

            if (thisStatus == 0)
                m_IconReveived.gameObject.SetActive(false);
            else if (thisStatus == 1)
            {
                //TODO 播放可领取特效
                m_IconReveived.gameObject.SetActive(false);
            }
            else
                m_IconReveived.gameObject.SetActive(true);

            BoxStatus = thisStatus;
        }

        /// <summary>
        /// 点击宝箱
        /// </summary>
        private void OnClickBoxButton(GameObject go)
        {
            Log.Debug("点击宝箱...当前层数为{0}", drTowerLayer.Id);
            if (BoxStatus == 1)
            {
                //领取奖励
                CLClaimRewardsForTower response = new CLClaimRewardsForTower();
                response.state = 2;
                response.LayerNum = drTowerLayer.Id;
                GameEntry.Network.Send(response);
                //stromTowerData.BoxStatus << (drTowerLayer.Id / 5) = 1;
                //SetBoxStatus();
            }
            else
            {
                //查看奖励
                List<ChestRewardDisplayData.Reward> rewards = new List<ChestRewardDisplayData.Reward>();
                var rewardTypes = drTowerLayer.BoxItems;
                if (rewardTypes.Count > 0)
                {
                    for (int i = 0; i < rewardTypes.Count; i++)
                        rewards.Add(new ChestRewardDisplayData.Reward(rewardTypes[i].ItemIcon, rewardTypes[i].ItemCount));
                }
                GameEntry.UI.OpenUIForm(UIFormId.ChestRewardSubForm, new ChestRewardDisplayData(rewards, GameEntry.Localization.GetString("UI_TEXT_PROMPT"), null));
                return;
            }
        }
    }
}


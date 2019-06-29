using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class StormTowerForm
    {
        [Serializable]
        private class ClaimReward
        {
            public GameObject m_SelfWindow = null;
            [SerializeField]
            private GameObject m_Mask = null;
            [SerializeField]
            private List<GeneralItemView> Items = null;
            [SerializeField]
            private GameObject m_BtnClaim = null;
            [SerializeField]
            private GameObject m_BtnClaimAgain = null;

            public void RefreshReward(List<Item> rewards)
            {
                if (rewards.Count == 0)
                {
                    GameFramework.Log.Debug("目前没有奖励，请先挑战副本...");
                    return;
                }
                m_SelfWindow.SetActive(true);
                UIEventListener.Get(m_BtnClaim).onClick = OnClickClaim;
                UIEventListener.Get(m_BtnClaimAgain).onClick = OnClickClaimAgain;
                UIEventListener.Get(m_Mask).onClick = OnCliakBackGround;
                for (int i = 0; i < Items.Count; i++)
                {
                    if (i >= rewards.Count)
                    {
                        Items[i].gameObject.SetActive(false);
                        continue;
                    }
                    Items[i].InitItem(rewards[i].ItemIcon, rewards[i].ItemCount);
                }
            }
            private void OnClickClaim(GameObject go)
            {
                CLClaimRewardsForTower response = new CLClaimRewardsForTower();
                response.state = 1;
                response.LayerNum =
                    GameEntry.Data.StromTowerData.StromTowerInfo.CurLayerNum > 0 ? GameEntry.Data.StromTowerData.StromTowerInfo.CurLayerNum : 1;
                GameEntry.Network.Send(response);
                OnCliakBackGround(m_SelfWindow);
            }
            private void OnClickClaimAgain(GameObject go)
            {

            }
            public void OnCliakBackGround(GameObject go)
            {
                m_SelfWindow.SetActive(false);
                UIEventListener.Get(m_BtnClaim).onClick = null;
                UIEventListener.Get(m_BtnClaimAgain).onClick = null;
                UIEventListener.Get(m_Mask).onClick = null;
            }
        }
    }
}


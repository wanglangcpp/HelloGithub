using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class StoryRewardState : StateBase
        {
            private List<Animation> m_AnimList = new List<Animation>();

            private int m_CurrentAnimIndex = 0;

            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                GameEntry.SceneLogic.GoBackToLobby();
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {

            }

            protected override void OnInit(IFsm<InstanceResultForm> fsm)
            {
                base.OnInit(fsm);
                fsm.Owner.m_StoryRewardsPanel.gameObject.SetActive(false);
            }

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                if (!GameEntry.SceneLogic.IsInstance)
                {
                    return;
                }

                m_CurrentAnimIndex = 0;
                m_AnimList.Clear();
                fsm.Owner.m_StoryRewardsPanel.gameObject.SetActive(true);
                fsm.Owner.m_RewardBgAnimation.gameObject.SetActive(true);
                fsm.Owner.m_ReturnLobbyButton.gameObject.SetActive(true);

                for (int i = 0; i < fsm.Owner.m_StoryBigStars.Length; i++)
                {
                    m_AnimList.Add(fsm.Owner.m_StoryBigStars[i].GetComponent<Animation>());
                    fsm.Owner.m_StoryBigStars[i].gameObject.SetActive(false);
                }
                fsm.Owner.m_StoryBigStars[m_CurrentAnimIndex].gameObject.SetActive(true);
                m_AnimList[m_CurrentAnimIndex].Play();
                var storyItems = fsm.Owner.m_StoryItemEarned;
                for (int i = 0; i < storyItems.Length; i++)
                {
                    storyItems[i].gameObject.SetActive(false);
                }
            }

            protected override void OnUpdate(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (PlayRequestAnimation(fsm, elapseSeconds))
                {
                    return;
                }
            }

            private bool PlayRequestAnimation(IFsm<InstanceResultForm> fsm, float elapseSeconds)
            {
                if (m_CurrentAnimIndex > m_AnimList.Count)
                {
                    return true;
                }

                if (!m_AnimList[m_CurrentAnimIndex].isPlaying)
                {
                    m_CurrentAnimIndex++;
                    if (m_CurrentAnimIndex == m_AnimList.Count)
                    {
                        m_CurrentAnimIndex++;
                        ShowItems(fsm);
                        return true;
                    }
                    fsm.Owner.m_StoryBigStars[m_CurrentAnimIndex].gameObject.SetActive(true);
                    m_AnimList[m_CurrentAnimIndex].Play();
                }

                return false;
            }

            private void ShowItems(IFsm<InstanceResultForm> fsm)
            {
                var storyItems = fsm.Owner.m_StoryItemEarned;

                for (int i = 0, j = 0; i < storyItems.Length; i++)
                {
                    var itemEarned = storyItems[i];
                    if (i >= GameEntry.Data.InstanceGoods.GoodsCount)
                    {
                        itemEarned.gameObject.SetActive(false);
                        continue;
                    }

                    if (i < GameEntry.Data.InstanceGoods.InstanceItemData.Count)
                    {
                        itemEarned.gameObject.SetActive(true);
                        itemEarned.InitGeneralItem(GameEntry.Data.InstanceGoods.InstanceItemData[i].Type);
                    }
                    else
                    {
                        SoulData soulData = GameEntry.Data.InstanceGoods.InstanceSoulData[j];
                        int count = GameEntry.Data.InstanceGoods.GetSameSoulCount(soulData.Type);
                        itemEarned.gameObject.SetActive(true);
                        itemEarned.InitGeneralItem(soulData.Type, count);
                    }
                }
            }

            protected override void OnLeave(IFsm<InstanceResultForm> fsm, bool isShutdown)
            {
            }
        }
    }
}

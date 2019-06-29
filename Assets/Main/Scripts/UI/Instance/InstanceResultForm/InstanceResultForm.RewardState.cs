using GameFramework.DataTable;
using GameFramework.Fsm;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class RewardState : StateBase
        {
            private bool m_HasGoBackToLobby = false;
            private int m_NextInstanceId = 0;

            public RewardState(Type nextStateType, Transform currentSubPanel, Transform lastSubPanel)
                : base(nextStateType, currentSubPanel, lastSubPanel)
            {

            }

            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                if (m_HasGoBackToLobby)
                {
                    return;
                }

                m_HasGoBackToLobby = true;
                GameEntry.SceneLogic.GoBackToLobby();
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {

            }

            protected override void OnInit(IFsm<InstanceResultForm> fsm)
            {
                base.OnInit(fsm);
                m_HasGoBackToLobby = false;
                m_NextInstanceId = 0;

                var playerData = fsm.Owner.m_Data.ItsPlayer;

                if (playerData.CoinEarned <= 0)
                {
                    fsm.Owner.m_CoinEarned.text = string.Empty;
                    fsm.Owner.m_CoinParent.gameObject.SetActive(false);
                }
                else
                {
                    fsm.Owner.m_CoinParent.gameObject.SetActive(true);
                    fsm.Owner.m_CoinEarned.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", playerData.CoinEarned);
                }

                for (int i = 0; i < fsm.Owner.m_ItemEarned.Length; i++)
                {
                    var itemEarned = fsm.Owner.m_ItemEarned[i];
                    if (i >= GameEntry.Data.InstanceGoods.InstanceItemData.Count)
                    {
                        itemEarned.gameObject.SetActive(false);
                        continue;
                    }

                    itemEarned.gameObject.SetActive(true);
                    var itemData = GameEntry.Data.InstanceGoods.InstanceItemData[i];
                    itemEarned.InitGeneralItem(itemData.Type, itemData.Count);
                }
            }

            protected override void OnUpdate(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (CachedAnimation[InwardClipName].normalizedTime >= .99f && !fsm.Owner.m_ReturnLobbyButton.gameObject.activeSelf)
                {
                    CachedAnimation.Stop();
                    fsm.Owner.m_ReturnLobbyButton.gameObject.SetActive(true);

                    if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.SinglePlayer)
                    {
                        int currentInstanceId = GameEntry.SceneLogic.SinglePlayerInstanceLogic.InstanceId;
                        IDataTable<DRInstance> DRInstance = GameEntry.DataTable.GetDataTable<DRInstance>();
                        DRInstance[] drs = DRInstance.GetAllDataRows();
                        for (int i = 0; i < drs.Length; i++)
                        {
                            if (drs[i].PrerequisiteInstanceId == currentInstanceId)
                            {
                                m_NextInstanceId = drs[i].Id;
                                break;
                            }
                        }
                    }
                }
            }

            public override void GotoNextInstance(IFsm<InstanceResultForm> m_Fsm)
            {
                base.GotoNextInstance(m_Fsm);

                if (m_NextInstanceId <= 0)
                {
                    return;
                }

                GameEntry.LobbyLogic.EnterInstance(m_NextInstanceId);
            }
        }
    }
}

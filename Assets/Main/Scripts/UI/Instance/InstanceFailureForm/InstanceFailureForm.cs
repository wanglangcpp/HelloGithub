using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class InstanceFailureForm : NGUIForm
    {
        [SerializeField]
        private GameObject m_TimeOutPanel = null;

        [SerializeField]
        private GameObject m_RevivalPanel = null;

        [SerializeField]
        private GameObject m_PromotionPanel = null;

        [SerializeField]
        private GameObject m_NpcDeadPanel = null;

        [SerializeField]
        private UILabel m_MoneyLabel = null;

        [SerializeField]
        private UILabel m_DiamondLabel = null;

        [SerializeField]
        private GoodsView[] m_RewardItem = null;

        [SerializeField]
        private Animation m_RevivalAnimation = null;

#pragma warning disable 0414

        [SerializeField]
        private Animation m_PromotionAnimation = null;

#pragma warning restore 0414

        private bool m_SwitchPromotionPanel = false;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(EventId.ReviveHeroes, OnReviveHeroes);

            GameEntry.TimeScale.PauseGame();
            var myUserData = userData as InstanceFailureData;
            bool canRevive = myUserData.FailureReason != InstanceFailureReason.TimeOut;
            if (myUserData.FailureReason == InstanceFailureReason.Escort)
            {
                m_NpcDeadPanel.SetActive(true);
                m_TimeOutPanel.SetActive(false);
                m_PromotionPanel.SetActive(false);
            }
            else
            {
                m_NpcDeadPanel.SetActive(false);
                m_TimeOutPanel.SetActive(!canRevive);
                m_PromotionPanel.SetActive(canRevive);
            }
            m_RevivalPanel.SetActive(false);
            m_SwitchPromotionPanel = false;
            // TODO ： 取消复活panel显示。以后会加上先不删
            //             if (canRevive)
            //             {
            //                 OnInitRevivePanel();
            //             }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!m_SwitchPromotionPanel)
            {
                return;
            }

            if (m_RevivalAnimation.isPlaying)
            {
                m_RevivalAnimation[m_RevivalAnimation.clip.name].time += realElapseSeconds;
                float time = m_RevivalAnimation[m_RevivalAnimation.clip.name].time;
                if (time > m_RevivalAnimation.clip.length)
                {
                    m_RevivalAnimation.Stop();
                }
            }
            else
            {
                m_SwitchPromotionPanel = false;
                m_RevivalPanel.SetActive(false);
                m_PromotionPanel.SetActive(true);
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);

            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.ReviveHeroes, OnReviveHeroes);
        }

        public void OnClickBack()
        {
            if (m_SwitchPromotionPanel)
            {
                return;
            }
            m_RevivalAnimation[m_RevivalAnimation.clip.name].time = 0;
            m_RevivalAnimation[m_RevivalAnimation.clip.name].speed = 1.0f;
            m_RevivalAnimation.Play();
            m_SwitchPromotionPanel = true;
        }

        public void OnClickInstanceItem()
        {
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenInstanceSelectForm, true);
            Abandon(false);
        }

        public void OnClickHeroItem()
        {
            int indexInAllHeroes = -1;
            int heroType = GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter.Data.HeroId;
            List<BaseLobbyHeroData> baseHeroData = new List<BaseLobbyHeroData>();
            var lobbyHeroes = GameEntry.Data.LobbyHeros.Data;

            lobbyHeroes.Sort(Comparer.CompareHeroes);
            for (int i = 0; i < lobbyHeroes.Count; i++)
            {
                baseHeroData.Add(lobbyHeroes[i]);
                if (lobbyHeroes[i].Type == heroType)
                {
                    indexInAllHeroes = i;
                }
            }

            if (indexInAllHeroes < 0)
            {
                Log.Error("Oops, hero type '{0}' not found.", heroType);
                return;
            }

            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenHeroInfoForm, new HeroInfoDisplayData
            {
                Scenario = HeroInfoScenario.Mine,
                IndexInAllHeroes = indexInAllHeroes,
                AllHeroes = baseHeroData,
                ShouldOpenImmediately = true,
            });
            Abandon(false);
        }

        public void OnClickMeridianItem()
        {
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenMeridianForm, true);
            Abandon(false);
        }

        public void OnClickReturn()
        {
            Abandon(true);
        }

        public void OnClickRevive()
        {
            GameEntry.LobbyLogic.ReviveHeroes();
        }

        private void OnReviveHeroes(object sender, GameEventArgs e)
        {
            CloseSelf();
        }

        private void OnInitRevivePanel()
        {
            int instanceId = GameEntry.SceneLogic.BaseInstanceLogic.InstanceId;
            IDataTable<DRInstance> dtInstance = GameEntry.DataTable.GetDataTable<DRInstance>();
            DRInstance dataRow = dtInstance.GetDataRow(instanceId);
            if (dataRow == null)
            {
                Log.Warning("Can not find instance '{0}'.", instanceId.ToString());
                return;
            }

            m_MoneyLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", dataRow.Coin);
            // TODO: Use real cost once available.
            m_DiamondLabel.text = "0";
            int dropsCount = (GameEntry.SceneLogic.BaseInstanceLogic as IDropGoodsInstance).DropGoodsCount;

            for (int i = 0; i < m_RewardItem.Length; i++)
            {
                if (i < dropsCount)
                {
                    m_RewardItem[i].gameObject.SetActive(true);
                    //m_RewardItem[i].InitGoodsView(GameEntry.Data.InstanceGoods.InstanceItemData[i].Type, GameEntry.Data.InstanceGoods.InstanceItemData[i].Count);
                }
                else
                {
                    m_RewardItem[i].gameObject.SetActive(false);
                }
            }
        }

        private void Abandon(bool autoHideLoading)
        {
            if (GameEntry.SceneLogic.BaseInstanceLogic.HasResult)
            {
                GameEntry.SceneLogic.GoBackToLobby(autoHideLoading);
                return;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceFailure(InstanceFailureReason.AbandonedByUser, false,
                delegate () { GameEntry.SceneLogic.GoBackToLobby(autoHideLoading); });
        }
    }
}

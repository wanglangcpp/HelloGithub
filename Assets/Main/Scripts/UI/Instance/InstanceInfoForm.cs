using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class InstanceInfoForm : NGUIForm
    {
        [Serializable]
        private class RewardItem
        {
            [SerializeField]
            private GeneralItemView m_Item = null;

            [SerializeField]
            private UIButton m_Button = null;

#pragma warning disable 0414
            private int m_ItemId;
#pragma warning restore 0414

            public void SetActive(bool active)
            {
                m_Item.gameObject.SetActive(active);
            }

            public void SetIcon(int itemId)
            {
                m_ItemId = itemId;
                m_Item.InitGeneralItem(itemId);
                m_Item.ResetOnClickDelegate();
            }

            private void OnLoadIconSuccess(UISprite sprite, string spriteName, object userData)
            {
                m_Button.normalSprite = spriteName;
            }
        }

        [SerializeField]
        private int m_InstanceId = -1;

        [SerializeField]
        private UILabel m_Desc = null;

        [SerializeField]
        private UILabel[] m_Requests = null;

        [SerializeField]
        private UILabel m_ExpReward = null;

        [SerializeField]
        private UILabel m_CoinReward = null;

        [SerializeField]
        private RewardItem[] m_Rewards = null;

        [SerializeField]
        private UIButton m_CleanOutBtn = null;

        [SerializeField]
        private UIButton m_CleanOutTimesBtn = null;

        [SerializeField]
        private GameObject m_ElementBoss = null;

        [SerializeField]
        private GoodsView m_ElementBossView = null;

        [SerializeField]
        private GoodsView[] m_ElementMonsterViews = null;

        [SerializeField]
        private UILabel m_CanCleanOutMaxTimes = null;

        [SerializeField]
        private HeroView[] m_HeroTeamViews = null;

        [SerializeField]
        private GameObject[] m_NoHeroTeamViews = null;

        [SerializeField]
        private UILabel m_CostEnergy = null;

        [SerializeField]
        private UITexture m_Background = null;

        [SerializeField]
        private UILabel m_RecommendMightLabel = null;

        [SerializeField]
        private UISprite m_BossIcon = null;

        [SerializeField]
        private UISprite m_ElementIcon = null;

        [SerializeField]
        private UILabel m_CurrentMightLabel = null;

        private string m_InstanceNpcsDTName = string.Empty;

        private const int MaxCleanOutTimes = 10;
        private int m_Exp;
        private int m_Coin;

        // Called by NGUI via reflection
        public void OnClickTeamChangeButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData
            {
                InstanceId = m_InstanceId,
                Scenario = HeroTeamDisplayScenario.Lobby,
                InstanceLogicType = InstanceLogicType.SinglePlayer,
            });
        }

        // Called by NGUI via reflection
        public void OnClickEnterButton()
        {
            GameEntry.LobbyLogic.EnterInstance(m_InstanceId);
        }

        // Called by NGUI via reflection
        public void OnClickCleanOut()
        {
            if (m_InstanceId == -1)
            {
                return;
            }

            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            CleanOut(1);
        }

        // Called by NGUI via reflection
        public void OnClickCleanOutMoreTimes()
        {
            var levelData = GameEntry.Data.InstanceGroups.GetLevelById(m_InstanceId);

            // 副本得到的星数小于3则提示是否打该副本
            if (levelData.StarCount < 3)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CLEANOUT_CONDITION_NOTE") });
                return;
            }

            // 体力是否满足条件
            int sweepOnceCostEnergy = 6; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.EnergyPerInstance, 6);
            int needEnergy = sweepOnceCostEnergy * 10;

            if (!UIUtility.CheckEnergy(needEnergy))
                return;

            SweepDisplayData displayData = new SweepDisplayData();
            displayData.SetFromInstanceData(m_InstanceId);

            GameEntry.UI.OpenUIForm(UIFormId.CleanOutResultForm, displayData);
            //if (m_InstanceId == -1)
            //{
            //    return;
            //}

            //if (GameEntry.OfflineMode.OfflineModeEnabled)
            //{
            //    return;
            //}

            //CleanOut(m_MaxCleanOutTimes);
        }

        // Called by NGUI via reflection
        public void OnClickExpIcon()
        {
            GameEntry.UI.OpenUIForm(UIFormId.GeneralItemInfoForm, new GeneralItemInfoDisplayData { TypeId = (int)FakeItemExceptCurrencyType.PlayerExp, Qty = m_Exp });
        }

        // Called by NGUI via reflection
        public void OnClickCoinIcon()
        {
            GameEntry.UI.OpenUIForm(UIFormId.GeneralItemInfoForm, new GeneralItemInfoDisplayData { TypeId = (int)CurrencyType.Coin, Qty = m_Coin });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
            //GameEntry.Event.Subscribe(EventId.CleanOutInstance, OnCleanOutInstance);
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            SetInstanceId(userData);
            RefreshData();
            RefreshElementData();
            SetCleanOutData();
            //GameEntry.NoviceGuide.CheckNoviceGuide(transform, UIFormId.InstanceInfoForm);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RefreshTeamData();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
            //GameEntry.Event.Unsubscribe(EventId.CleanOutInstance, OnCleanOutInstance);
            GameEntry.Event.Unsubscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            base.OnClose(userData);
        }

        private void SetCleanOutData()
        {
            var power = GameEntry.Data.Player.Energy;
            int usePower = 6; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.EnergyPerInstance, 6);
            //if (power < usePower)
            //{
            //    m_CanCleanOutOnce.text = GameEntry.Localization.GetString("UI_TITLE_NAME_CLEANOUTTIMES", 0);
            //    m_CanCleanOutMaxTimes.text = GameEntry.Localization.GetString("UI_TITLE_NAME_CLEANOUTTIMES", 0);
            //}
            //else
            //{
            //    m_CanCleanOutOnce.text = GameEntry.Localization.GetString("UI_TITLE_NAME_CLEANOUTTIMES", 1);
            //    m_MaxCleanOutTimes = power / usePower > MaxCleanOutTimes ? MaxCleanOutTimes : power / usePower;
            //    m_CanCleanOutMaxTimes.text = GameEntry.Localization.GetString("UI_TITLE_NAME_CLEANOUTTIMES", m_MaxCleanOutTimes);
            //}

            m_CanCleanOutMaxTimes.text = GameEntry.Localization.GetString("UI_TEXT_SETTLEMENT");

            m_CleanOutTimesBtn.isEnabled =/* GameEntry.Data.InstanceProgresses.HasKey(m_InstanceId) &&*/ !(power < usePower);
            m_CleanOutBtn.isEnabled = /*GameEntry.Data.InstanceProgresses.HasKey(m_InstanceId) && */!(power < usePower);
            m_CleanOutBtn.gameObject.SetActive(false);
        }

        private void SetInstanceId(object userData)
        {
            var myUserData = userData as InstanceInfoDisplayData;
            if (myUserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            m_InstanceId = myUserData.InstanceId;

            if (m_InstanceId <= 0)
            {
                Log.Error("Instance ID is invalid.");
                return;
            }
        }

        private void RefreshData()
        {
            var instanceDataTable = GameEntry.DataTable.GetDataTable<DRInstance>();

            DRInstance row = instanceDataTable.GetDataRow(m_InstanceId);
            if (row == null)
            {
                Log.Error("Instance '{0}' not found.", m_InstanceId);
                return;
            }

            GetComponent<UITitle>().SetTitle(row.Name);
            m_Desc.text = GameEntry.Localization.GetString(row.Description);

            for (int i = 0; i < m_Requests.Length; ++i)
            {
                m_Requests[i].text = GameEntry.Localization.GetString(row.RequestDescriptions[i]);
            }

            m_Exp = row.PlayerExp;
            m_ExpReward.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", row.PlayerExp);

            m_Coin = row.Coin;
            m_CoinReward.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", row.Coin);

            var possibleDrops = row.PossibleDrops;
            for (int i = 0; i < m_Rewards.Length; ++i)
            {
                if (i < possibleDrops.Length)
                {
                    m_Rewards[i].SetActive(true);
                    m_Rewards[i].SetIcon(possibleDrops[i].ItemId);
                }
                else
                {
                    m_Rewards[i].SetActive(false);
                }
            }

            m_CostEnergy.text = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Instance.CostEnergy, 6).ToString();

            var levelData = GameEntry.Data.InstanceGroups.GetLevelById(m_InstanceId);
            if (levelData.NpcInfo != null)
            {
                var icons = GameEntry.DataTable.GetDataTable<DRIcon>();
                DRIcon iconRow = icons.GetDataRow(levelData.NpcInfo.IconId);
                if (iconRow != null)
                    m_BossIcon.spriteName = iconRow.SpriteName;
                else
                    m_BossIcon.gameObject.SetActive(false);

                m_ElementIcon.spriteName = UIUtility.GetElementSpriteName(levelData.NpcInfo.ElementId);
                m_ElementIcon.gameObject.SetActive(true);
                m_BossIcon.gameObject.SetActive(true);

                m_ElementBoss.SetActive(false);
            }
            else
            {
                m_ElementBoss.SetActive(true);

                m_ElementIcon.gameObject.SetActive(false);
                m_BossIcon.gameObject.SetActive(false);
            }
            m_RecommendMightLabel.text = levelData.LevelConfig.RecommendMight.ToString();
            RefreshTeamData();

            if (row.TextureId > 0)
            {
                m_Background.LoadAsync(row.TextureId);
            }
        }

        private void RefreshTeamData()
        {
            var heroTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default);
            for (int i = 0; i < m_HeroTeamViews.Length; i++)
            {
                if (i < heroTeam.HeroType.Count && heroTeam.HeroType[i] > 0)
                {
                    m_NoHeroTeamViews[i].SetActive(false);
                    m_HeroTeamViews[i].gameObject.SetActive(true);
                    m_HeroTeamViews[i].InitHeroView(heroTeam.HeroType[i]);
                }
                else
                {
                    m_HeroTeamViews[i].gameObject.SetActive(false);
                    m_NoHeroTeamViews[i].SetActive(true);
                }
            }

            m_CurrentMightLabel.text = GameEntry.Localization.GetString("UI_FIGHTING_CAPACITY_RANKS", GameEntry.Data.Player.TeamMight);
        }

        private void RefreshElementData()
        {
            string instanceNpcStr;
            if (m_InstanceId == -1)
            {
                return;
            }
            IDataTable<DRInstance> dtInstanceTabele = GameEntry.DataTable.GetDataTable<DRInstance>();
            DRInstance drInstance = dtInstanceTabele.GetDataRow(m_InstanceId);
            if (drInstance == null)
            {
                Log.Warning("Can not find instance '{0}'.", m_InstanceId.ToString());
                return;
            }
            instanceNpcStr = drInstance.InstanceNpcs;
            m_InstanceNpcsDTName = instanceNpcStr;

            string[] splitNames = instanceNpcStr.Split('_');
            if (splitNames.Length != 2)
            {
                Log.Warning("Instance NPC is invalid.");
                return;
            }

            IDataTable<DRInstanceNpcs> instanceNpcs = GameEntry.DataTable.GetDataTable<DRInstanceNpcs>(splitNames[1]);

            if (instanceNpcs != null)
            {
                RefreshElementLast();
            }
            else
            {
                GameEntry.DataTable.LoadDataTable(m_InstanceNpcsDTName);
            }
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            RefreshElementLast();
        }

        private void RefreshElementLast()
        {
            string[] splitNames = m_InstanceNpcsDTName.Split('_');
            IDataTable<DRInstanceNpcs> instanceNpcs = GameEntry.DataTable.GetDataTable<DRInstanceNpcs>(splitNames[1]);

            if (instanceNpcs == null)
            {
                Log.Warning("Can not load instance NPCs data table '{0}'.", splitNames[1]);
                return;
            }

            int bossElement = -1;
            var dtNpc = GameEntry.DataTable.GetDataTable<DRNpc>();
            List<int> elementIds = new List<int>();
            var instanceRows = instanceNpcs.GetAllDataRows();
            for (int i = 0; i < instanceRows.Length; i++)
            {
                DRNpc drNpc = dtNpc.GetDataRow(instanceRows[i].NpcId);
                if (drNpc == null)
                {
                    Log.Warning("Can not find DRNpc '{0}'.", instanceNpcs[i].NpcId.ToString());
                    return;
                }
                if (drNpc.Category == NpcCategory.Boss)
                {
                    bossElement = drNpc.ElementId;
                    continue;
                }
                if (!elementIds.Contains(drNpc.ElementId) && drNpc.ElementId != 0)
                {
                    elementIds.Add(drNpc.ElementId);
                }
            }

            if (bossElement == -1 || bossElement == 0)
            {
                //m_ElementBoss.SetActive(false);
            }
            else
            {
                //m_ElementBoss.SetActive(true);
                m_ElementBossView.InitElementView(bossElement);
            }

            for (int i = 0; i < m_ElementMonsterViews.Length; i++)
            {
                if (i < elementIds.Count)
                {
                    m_ElementMonsterViews[i].gameObject.SetActive(true);
                    m_ElementMonsterViews[i].InitElementView(elementIds[i]);
                }
                else
                {
                    m_ElementMonsterViews[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnPlayerDataChanged(object sender, GameEventArgs e)
        {
            SetCleanOutData();
        }

        private void OnCleanOutInstance(object sender, GameEventArgs e)
        {
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.NewLevel = GameEntry.Data.Player.Level;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.NewExp = GameEntry.Data.Player.Exp;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.NewCoin = GameEntry.Data.Player.Coin;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.PortraitId = GameEntry.Data.Player.PortraitType;

            var lobbyHeroes = GetHeroTeamData();
            for (int i = 0; i < GameEntry.Data.CleanOuts.HeroResultData.Heroes.Count; ++i)
            {
                var hero = GameEntry.Data.CleanOuts.HeroResultData.Heroes[i];

                hero.NewLevel = lobbyHeroes[i].Level;
                hero.NewExp = lobbyHeroes[i].Exp;
            }
            SetCleanOutData();
            GameEntry.UI.OpenUIForm(UIFormId.CleanOutResultForm);
        }

        private void CleanOut(int times)
        {
            if (m_InstanceId == 0)
            {
                return;
            }

            if (!GameEntry.Data.InstanceProgresses.HasKey(m_InstanceId))
            {
                return;
            }

            int usePower = 6; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.EnergyPerInstance, 6);
            if (!UIUtility.CheckEnergy(times * usePower))
            {
                return;
            }

            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.Name = GameEntry.Data.Player.Name;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.OldLevel = GameEntry.Data.Player.Level;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.OldExp = GameEntry.Data.Player.Exp;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.OldCoin = GameEntry.Data.Player.Coin;
            GameEntry.Data.CleanOuts.HeroResultData.ItsPlayer.PortraitId = GameEntry.Data.Player.PortraitType;

            var lobbyHeroes = GetHeroTeamData();
            GameEntry.Data.CleanOuts.HeroResultData.Heroes.Clear();
            for (int i = 0; i < lobbyHeroes.Count; ++i)
            {
                var lobbyHero = lobbyHeroes[i];
                var resultHero = new InstanceResultData.Hero();
                resultHero.Name = lobbyHero.Name;
                resultHero.PortraitSpriteName = lobbyHero.PortraitSpriteName;
                resultHero.Profession = lobbyHero.Profession;
                resultHero.ElementId = lobbyHero.ElementId;
                resultHero.OldLevel = lobbyHero.Level;
                resultHero.OldExp = lobbyHero.Exp;
                GameEntry.Data.CleanOuts.HeroResultData.Heroes.Add(resultHero);
            }
            GameEntry.Data.CleanOuts.Count = times;

            CLCleanOutInstance msg = new CLCleanOutInstance();
            msg.InstanceType = m_InstanceId;
            //msg.Count = times;
            GameEntry.Network.Send(msg);
        }

        private List<LobbyHeroData> GetHeroTeamData()
        {
            var ret = new List<LobbyHeroData>();
            var heroTypeIds = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType;

            for (int i = 0; i < heroTypeIds.Count; ++i)
            {
                if (heroTypeIds[i] == 0)
                {
                    break;
                }

                LobbyHeroData lobbyHero = null;

                for (int j = 0; j < GameEntry.Data.LobbyHeros.Data.Count; ++j)
                {
                    var tempHero = GameEntry.Data.LobbyHeros.Data[j];
                    if (tempHero.Type == heroTypeIds[i])
                    {
                        lobbyHero = tempHero;
                        break;
                    }
                }

                if (lobbyHero == null)
                {
                    break;
                }

                ret.Add(lobbyHero);
            }

            return ret;
        }
    }
}

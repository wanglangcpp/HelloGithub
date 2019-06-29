using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm : NGUIForm
    {
#pragma warning disable 0414

        [SerializeField]
        private int m_InstanceId = -1;

#pragma warning restore 0414

        [SerializeField]
        private UIScrollView m_PortraitScrollView = null;

        [SerializeField]
        private UITable m_PortraitListTable = null;

        [SerializeField]
        private GameObject m_PortraitTemplateNormal = null;

        [SerializeField]
        private GameObject m_PortraitTemplateChessBattle = null;

        [SerializeField]
        private GameObject m_OfflineLineObj = null;

        private GameObject PortraitTemplate
        {
            get
            {
                return m_Strategy.PortraitTemplate;
            }
        }

        [SerializeField]
        private HeroDisplay[] m_HeroDisplays = null;

        [SerializeField]
        private Camera m_SecondaryCamera = null;

        [SerializeField]
        private int m_MaxHeroCountPerTeam = 3;

        private List<HeroPortraitForSelection> m_HeroProtraits = new List<HeroPortraitForSelection>();
        private HeroTeamDisplayScenario m_Scenario;
        private StrategyBase m_Strategy = null;
        private InstanceLogicType m_InstanceType = InstanceLogicType.SinglePlayer;
        private HeroTeamDisplayData m_UserData = null;
        private List<int> m_OldHeroTeam = null;
        private int m_OldMight = 0;
        /// <summary>
        /// 改变阵容是否可以点击（限制同时点击多个英雄）
        /// </summary>
        public static bool IsCanClickChangeHeroTeamBtn = false;
        /// <summary>
        /// 是否可以打开英雄展示界面（限制同时点击多个英雄）
        /// </summary>
        private bool IsCanClickOpenHeroInfoForeBtn = false;

        private int CurrentSelectedCount
        {
            get
            {
                int ret = 0;
                for (int i = 0; i < m_HeroDisplays.Length; ++i)
                {
                    if (CharacterIsAvailable(i))
                    {
                        ++ret;
                    }
                }

                return ret;
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
            m_UserData = userData as HeroTeamDisplayData;
            if (m_UserData == null)
            {
                Log.Error("userData is invalid.");
                return;
            }
            InitData();
            RefreshData();
            GameEntry.Impact.IncreaseHidingNameBoardCount();
        }

        protected override void OnResume()
        {
            base.OnResume();
            RefreshHeroData();
            m_SecondaryCamera.enabled = true;
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(EventId.HeroTeamDataChanged, OnHeroTeamDataChanged);
            UpdateHeroDisplays();
            IsCanClickChangeHeroTeamBtn = true;
            IsCanClickOpenHeroInfoForeBtn = true;
        }

        protected override void OnPause()
        {
            if (GameEntry.IsAvailable)
            {
                base.OnPause();
            }

            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(EventId.HeroTeamDataChanged, OnHeroTeamDataChanged);
            m_SecondaryCamera.enabled = false;
            base.OnPause();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
            ClearFakeCharacters();
            ClearHeroPortraits();
            DeinitStrategy();

            GameEntry.Impact.DecreaseHidingNameBoardCount();
            m_OldMight = 0;
            m_OldHeroTeam = null;
            base.OnClose(userData);
        }

        // Called by NGUI via reflection
        public void OnClickCharacterButton(int key)
        {
            int index = key;

            if (!CharacterIsAvailable(index) || !m_Strategy.CanChangeHeroTeam)
            {
                return;
            }
            m_OldMight = GameEntry.Data.Player.TeamMight;
            m_OldHeroTeam = GetSelectedHeroIds();
            RemoveFakeCharacter(index);
            UpdateHeroDisplays();
            List<int> newHeroTeam = GetSelectedHeroIds();

            if (newHeroTeam.Count > 0)
            {
                m_Strategy.RequestChangeHeroTeam(newHeroTeam, m_Strategy.HeroTeamInfoType.Value);
            }
        }

        public void OnSelectHeroPortrait(HeroPortraitForSelection originalItem)
        {
            if (!IsCanClickChangeHeroTeamBtn)
            {
                return;
            }
            if (!m_Strategy.CanChangeHeroTeam)
            {
                return;
            }
            m_OldMight = GameEntry.Data.Player.TeamMight;
            m_OldHeroTeam = GetSelectedHeroIds();
            var item = originalItem;
            if (item == null)
            {
                return;
            }

            if (item.IndexInTeam >= 0)
            {
                OnClickCharacterButton(item.IndexInTeam);
                return;
            }

            int index = -1;
            for (int i = 0; i < m_MaxHeroCountPerTeam; ++i)
            {
                if (!CharacterIsAvailable(i))
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_HERO_TEAM_FULL"));
                return;
            }

            int characterId;
            if (!TryGetCharacterId(item.HeroId, out characterId))
            {
                Log.Warning("Character not found for hero '{0}'", item.HeroId);
            }

            SetPortraitDisplayIndexInTeam(item.HeroId, index);
            ShowFakeCharacter(characterId, item.HeroId, index);
        }

        private void OpenHeroInfoForm(int selectedHeroTypeId)
        {
            m_Strategy.OpenHeroInfoForm(selectedHeroTypeId);
        }

        private void InitData()
        {
            InitScenario();
            InitStrategy();
            m_OldMight = GameEntry.Data.Player.TeamMight;
        }

        private void InitStrategy()
        {
            m_Strategy = CreateStrategy(m_Scenario);
            m_Strategy.Init(this);
        }

        private void DeinitStrategy()
        {
            m_Strategy.Shutdown();
            m_Strategy = null;
        }

        private void InitScenario()
        {
            m_Scenario = m_UserData.Scenario;
            m_InstanceType = m_UserData.InstanceLogicType;
        }

        private void SetInstanceId()
        {
            m_InstanceId = m_UserData.InstanceId;
        }

        private bool TryGetCharacterId(int heroId, out int characterId)
        {
            characterId = 0;

            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero heroDataRow = heroDataTable.GetDataRow(heroId);
            if (heroDataRow == null)
            {
                return false;
            }

            characterId = heroDataRow.CharacterId;
            return true;
        }

        private bool CharacterIsAvailable(int index)
        {
            var character = m_HeroDisplays[index].Character;
            if (character == null)
            {
                return false;
            }

            return character.IsAvailable;
        }

        private void RefreshHeroData()
        {
            for (int i = 0; i < m_HeroProtraits.Count; i++)
            {
                m_HeroProtraits[i].gameObject.SetActive(false);
            }
            StartCoroutine(RefreshDataCo(false));
        }

        private void RefreshData()
        {
            StartCoroutine(RefreshDataCo());
        }

        private HeroPortraitForSelection GetHeroScript(int heroId)
        {
            for (int i = 0; i < m_HeroProtraits.Count; i++)
            {
                if (m_HeroProtraits[i].HeroId == heroId)
                {
                    m_HeroProtraits[i].gameObject.SetActive(true);
                    return m_HeroProtraits[i];
                }
            }
            return null;
        }

        public int ArenaHeroComparer(LobbyHeroData a, LobbyHeroData b)
        {
            int aIndex = -1;
            int bIndex = -1;

            var heroTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena);
            for (int i = 0; i < heroTeam.HeroType.Count; i++)
            {
                if (heroTeam.HeroType[i] == a.Type)
                {
                    aIndex = i;
                }

                if (heroTeam.HeroType[i] == b.Type)
                {
                    bIndex = i;
                }
            }

            if (aIndex >= 0 && bIndex < 0)
            {
                return -1;
            }

            if (bIndex >= 0 && aIndex < 0)
            {
                return 1;
            }

            if (aIndex < 0 && bIndex < 0)
            {
                if (b.StarLevel != a.StarLevel)
                {
                    return b.StarLevel.CompareTo(a.StarLevel);
                }

                if (b.Quality != a.Quality)
                {
                    return b.Quality.CompareTo(a.Quality);
                }

                if (b.Level != a.Level)
                {
                    return b.Level.CompareTo(a.Level);
                }

                if (b.Might != a.Might)
                {
                    return b.Might.CompareTo(a.Might);
                }
                return a.Type.CompareTo(b.Type);
            }

            return aIndex.CompareTo(bIndex);
        }

        private IEnumerator RefreshDataCo(bool isInit = true)
        {
            GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);
            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            var iconDataTable = GameEntry.DataTable.GetDataTable<DRIcon>();

            var selectedHeroes = m_Strategy.HeroTeam;
            var heroesToSelect = m_Strategy.SortedLobbyHeroes;

            var herosToSelectList = heroesToSelect as List<LobbyHeroData>;
            if (m_Strategy.HeroTeamInfoType == HeroTeamType.Arena)
            {
                herosToSelectList.Sort(ArenaHeroComparer);
            }
            else
            {
                herosToSelectList.Sort(Comparer.HeroComparer);
            }
            int i = 0;
            for (; i < herosToSelectList.Count; ++i)
            {
                var heroId = herosToSelectList[i].Key;

                DRHero heroDataRow = heroDataTable.GetDataRow(heroId);
                if (heroDataRow == null)
                {
                    continue;
                }
                var iconId = heroDataRow.IconId;
                DRIcon iconDataRow = iconDataTable.GetDataRow(iconId);
                if (iconDataRow == null)
                {
                    continue;
                }
                var script = GetHeroScript(heroId);
                if (script == null)
                {
                    var go = NGUITools.AddChild(m_PortraitListTable.gameObject, PortraitTemplate);
                    script = go.GetComponent<HeroPortraitForSelection>();
                    int panelDepth = gameObject.GetComponentInChildren<UIPanel>().depth;
                    var panels = go.GetComponentsInChildren<UIPanel>();
                    for (int j = 0; j < panels.Length; j++)
                    {
                        if (panels[j].depth < panelDepth)
                        {
                            panels[j].depth += panelDepth;
                        }
                    }
                    m_HeroProtraits.Add(script);
                }

                m_Strategy.SetPortraitForSelectionData(selectedHeroes, heroId, script);

                if (selectedHeroes.Contains(heroId) && isInit)
                {
                    ShowFakeCharacter(heroDataRow.CharacterId, heroId, script.IndexInTeam);
                }
                script.name = "HeroItem" + i.ToString("D2");
            }

            m_OfflineLineObj.name = "HeroItem" + i.ToString("D2");
            i++;
            bool hasOfflineLineObj = false;
            var heroAllRows = heroDataTable.GetAllDataRows();
            for (int j = 0; j < heroAllRows.Length; j++)
            {
                if (GameEntry.Data.LobbyHeros.GetData(heroAllRows[j].Id) != null)
                {
                    continue;
                }
                var script = GetHeroScript(heroAllRows[j].Id);
                if (script == null)
                {
                    var go = NGUITools.AddChild(m_PortraitListTable.gameObject, PortraitTemplate);
                    script = go.GetComponent<HeroPortraitForSelection>();
                    int panelDepth = gameObject.GetComponentInChildren<UIPanel>().depth;
                    var panels = go.GetComponentsInChildren<UIPanel>();
                    for (int m = 0; m < panels.Length; m++)
                    {
                        if (panels[m].depth < panelDepth)
                        {
                            panels[m].depth += panelDepth;
                        }
                    }
                }
                m_Strategy.SetPortraitForSelectionData(null, heroAllRows[j].Id, script, false);
                script.name = "HeroItem" + i.ToString("D2");
                m_HeroProtraits.Add(script);
                hasOfflineLineObj = true;
                i++;
            }
            m_OfflineLineObj.SetActive(hasOfflineLineObj);
            m_PortraitListTable.Reposition();
            yield return null;
            m_PortraitScrollView.ResetPosition();
            GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
        }

        private void SetPortraitDisplayIndexInTeam(int heroId, int index)
        {
            for (int i = 0; i < m_HeroProtraits.Count; ++i)
            {
                if (m_HeroProtraits[i].HeroId == heroId)
                {
                    m_HeroProtraits[i].IndexInTeam = index;
                    break;
                }
            }
        }

        private void ResetPortraitIndexInTeam(int heroId)
        {
            SetPortraitDisplayIndexInTeam(heroId, -1);
        }

        private void ClearHeroPortraits()
        {
            for (int i = 0; i < m_HeroProtraits.Count; ++i)
            {
                Destroy(m_HeroProtraits[i].gameObject);
            }

            m_HeroProtraits.Clear();
        }

        private void UpdateHeroDisplays()
        {
            for (int i = 0; i < m_HeroDisplays.Length; ++i)
            {
                var heroDisplay = m_HeroDisplays[i];

                if (!CharacterIsAvailable(i))
                {
                    heroDisplay.BottomPanel.SetActive(false);
                    heroDisplay.Name.text = string.Empty;
                    heroDisplay.Might.text = string.Empty;
                    continue;
                }

                int currentHeroId = heroDisplay.Character.Data.HeroId;
                int index = -1;
                var heroesData = m_Strategy.SortedLobbyHeroes;

                for (int j = 0; j < heroesData.Count; ++j)
                {
                    if (heroesData[j].Key == currentHeroId)
                    {
                        index = j;
                    }
                }

                var heroData = heroesData[index];
                heroDisplay.BottomPanel.SetActive(true);
                heroDisplay.Name.text = heroData.Name;
                heroDisplay.Might.text = GameEntry.Localization.GetString("UI_TEXT_HERO_GS_TEXT", heroData.Might);
                heroDisplay.Element.spriteName = UIUtility.GetElementSpriteName(heroData.ElementId);
            }

            m_Strategy.OnHeroDisplaysUpdated();
        }

        private List<int> GetSelectedHeroIds()
        {
            var ret = new List<int>();
            for (int i = 0; i < m_HeroDisplays.Length; ++i)
            {
                if (CharacterIsAvailable(i))
                {
                    ret.Add(m_HeroDisplays[i].Character.Data.HeroId);
                }
            }

            return ret;
        }

        private void ShowMightChangeForm(int oldMight, int newMight)
        {
            IsCanClickChangeHeroTeamBtn = true;
            Log.Debug("--------------------------Hreo Team Changed Success......{0}", IsCanClickChangeHeroTeamBtn);
            if (newMight <= oldMight)
            {
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.MightChangeForm, new MightChangeDisplayData
            {
                MightCurrentValue = newMight,
                MightChangeValue = newMight - oldMight,
                IsHeroMightChange = false,
            });
        }

        public void OnClickElementBtn()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ElementsRestrictionForm);
        }

        public void OnClickHeroInfoButton(int index)
        {
            if (IsCanClickOpenHeroInfoForeBtn)
                IsCanClickOpenHeroInfoForeBtn = false;
            else
                return;
            int heroType = GameEntry.Data.HeroTeams.GetData((int)m_Strategy.HeroTeamInfoType).HeroType[index];
            List<BaseLobbyHeroData> baseHeroData = new List<BaseLobbyHeroData>();
            var lobbyHeroes = GameEntry.Data.LobbyHeros.Data;
            int heroIndex = -1;
            lobbyHeroes.Sort(Comparer.CompareHeroes);
            for (int i = 0; i < lobbyHeroes.Count; i++)
            {
                baseHeroData.Add(lobbyHeroes[i]);
                if (lobbyHeroes[i].Type == heroType)
                {
                    heroIndex = i;
                }
            }

            if (index < 0)
            {
                Log.Error("Oops, hero type ID '{0}' cannot be found.", heroType.ToString());
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.HeroInfoForm_Possessed, new HeroInfoDisplayData
            {
                Scenario = HeroInfoScenario.Mine,
                IndexInAllHeroes = heroIndex,
                AllHeroes = baseHeroData,
            });
        }
    }
}

using GameFramework;
using GameFramework.DataTable;
using GameFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class UIUtility
    {
        /// <summary>
        /// 计算两次经验差值
        /// </summary>
        /// <param name="previousLevel">上次数据的等级</param>
        /// <param name="previousExp">上次数据的经验值</param>
        /// <param name="currentLevel">当前的等级</param>
        /// <param name="currentExp">当前的经验值</param>
        /// <returns>玩家经验的差值</returns>
        public static int CalculateDeltaExp(int previousLevel, int previousExp, int currentLevel, int currentExp)
        {
            if (previousLevel > currentLevel)
                return 0;

            if (previousLevel == currentLevel)
                return currentExp - previousExp;

            var playerConfig = GameEntry.DataTable.GetDataTable<DRPlayer>();

            int previousTotalExp = previousExp;
            int currentTotalExp = currentExp;
            for (int level = 1; level < playerConfig.Count; ++level)
            {
                var item = playerConfig.GetDataRow(level);

                if (level < previousLevel)
                    previousTotalExp += item.LevelUpExp;

                if (level < currentLevel)
                    currentTotalExp += item.LevelUpExp;
                else
                    break;
            }

            return currentTotalExp - previousTotalExp;
        }

        /// <summary>
        /// 计算两次经验差值
        /// 一般服务器下发的新数据都是PBPlayerInfo，
        /// 本地存储的老数据都是PlayerData
        /// </summary>
        /// <param name="currentPlayerData">最新的玩家数据</param>
        /// <param name="previousPlayerData">之前的玩家数据</param>
        /// <returns>玩家经验的差值</returns>
        public static int CalculateDeltaExp(PBPlayerInfo currentPlayerData, PlayerData previousPlayerData)
        {
            return CalculateDeltaExp(previousPlayerData.Level, previousPlayerData.Exp, currentPlayerData.Level, currentPlayerData.Exp);
        }

        public static IUpdatableUIFragment GetUpdatableUIFragment(GameObject go)
        {
            var mbs = go.GetComponents<MonoBehaviour>();

            for (int i = 0; i < mbs.Length; ++i)
            {
                var ret = mbs[i] as IUpdatableUIFragment;
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        public static bool WorldToUIPoint(Vector3 worldPoint, out Vector3 uiPoint)
        {
            if (!GameEntry.IsAvailable || GameEntry.Scene == null)
            {
                uiPoint = Vector3.zero;
                return false;
            }

            var currentUICamera = UICamera.currentCamera;
            if (currentUICamera == null)
            {
                uiPoint = Vector3.zero;
                return false;
            }

            if (GameEntry.Scene.MainCamera == null)
            {
                uiPoint = Vector3.zero;
                return false;
            }

            Vector3 screenPoint = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPoint);
            screenPoint.z = 0f;
            uiPoint = currentUICamera.ScreenToWorldPoint(screenPoint);
            return true;
        }

        /// <summary>
        /// 替换字典串。
        /// </summary>
        /// <param name="gameObject">根节点。</param>
        /// <param name="exemptedLabels">不受影响的文本框对象。</param>
        public static void ReplaceDictionaryTextForLabels(GameObject gameObject, params UILabel[] exemptedLabels)
        {
            UILabel[] labels = gameObject.GetComponentsInChildren<UILabel>(true);
            for (int i = 0; i < labels.Length; i++)
            {
                UILabel label = labels[i];
                if (string.IsNullOrEmpty(label.text))
                {
                    continue;
                }

                bool shouldExempt = false;
                for (int j = 0; j < exemptedLabels.Length; j++)
                {
                    if (exemptedLabels[j] != null && exemptedLabels[j] == labels[i])
                    {
                        shouldExempt = true;
                    }
                }

                if (shouldExempt)
                {
                    continue;
                }

                label.text = GameEntry.Localization.GetString(label.text);
                label.text = GameEntry.StringReplacement.GetString(label.text);
            }
        }

        /// <summary>
        /// 设置星等。
        /// </summary>
        /// <param name="stars">待用精灵图列表。</param>
        /// <param name="starLevel">星等。</param>
        public static void SetStarLevel(UISprite[] stars, int starLevel)
        {
            int i = 0;
            for (/* Empty initializer */; i < starLevel && i < stars.Length; ++i)
            {
                stars[i].gameObject.SetActive(true);
            }

            for (/* Empty initializer */; i < stars.Length; ++i)
            {
                stars[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置品阶。
        /// </summary>
        /// <param name="qualityLevelIcons">待用精灵图列表。</param>
        /// <param name="qualityLevel">品阶。</param>
        public static void SetHeroQualityLevel(UISprite[] qualityLevelIcons, int qualityLevel, QualityType quality)
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRHeroQualityMaxLevel>();
            DRHeroQualityMaxLevel maxLevelRow = dataTable.GetDataRow((int)quality);
            if (maxLevelRow == null)
            {
                Log.Warning("Cannot find Quality max level '{0}'.", quality);
                return;
            }
            for (int i = 0; i < qualityLevelIcons.Length; i++)
            {
                qualityLevelIcons[i].gameObject.SetActive(i < maxLevelRow.MaxLevel);
                qualityLevelIcons[i].spriteName = Constant.Quality.HeroQualityLevelSpriteNames[(int)quality];
                if (i < qualityLevel)
                {
                    qualityLevelIcons[i].color = Color.white;
                }
                else
                {
                    qualityLevelIcons[i].color = Color.grey;
                }
            }
        }

        /// <summary>
        /// 根据克制类属性取得图标精灵图名称。
        /// </summary>
        /// <param name="element">克制类属性。</param>
        /// <returns>精灵图名称。</returns>
        public static string GetElementSpriteName(int element)
        {
            if (element < Constant.AttributeName.HeroElementSpriteNames.Length && element > 0)
            {
                return Constant.AttributeName.HeroElementSpriteNames[element];
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据英雄种类编号获取其克制类属性。
        /// </summary>
        /// <param name="heroTypeId">英雄种类编号。</param>
        /// <returns>英雄克制类属性。</returns>
        public static int GetHeroElement(int heroTypeId)
        {
            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero heroDataRow = heroDataTable.GetDataRow(heroTypeId);
            if (heroDataRow == null)
            {
                Log.Warning("Hero type '{0}' not found.", heroTypeId);
                return -1;
            }

            return heroDataRow.ElementId;
        }

        /// <summary>
        /// 根据英雄种类编号获取英雄头像的图标编号。
        /// </summary>
        /// <param name="heroTypeId">英雄种类编号。</param>
        /// <returns>图标编号。</returns>
        public static int GetHeroProtraitIconId(int heroTypeId)
        {
            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero heroDataRow = heroDataTable.GetDataRow(heroTypeId);
            if (heroDataRow == null)
            {
                Log.Warning("Hero type '{0}' not found.", heroTypeId);
                return -1;
            }

            return heroDataRow.IconId;
        }

        /// <summary>
        /// 根据英雄种类编号获取英雄大头像贴图编号。
        /// </summary>
        /// <param name="heroTypeId">英雄种类编号。</param>
        /// <returns>贴图编号。</returns>
        public static int GetHeroBigPortraitTextureId(int heroTypeId)
        {
            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero heroDataRow = heroDataTable.GetDataRow(heroTypeId);
            if (heroDataRow == null)
            {
                Log.Warning("Hero type '{0}' not found.", heroTypeId);
                return -1;
            }

            return heroDataRow.ProtraitTextureId;
        }

        /// <summary>
        /// 根据英雄种类编号取得英雄头像精灵图名称。
        /// </summary>
        /// <param name="heroTypeId">英雄种类编号。</param>
        /// <returns>精灵图名称。</returns>
        public static string GetHeroPortraitSpriteName(int heroTypeId)
        {
            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero heroDataRow = heroDataTable.GetDataRow(heroTypeId);
            if (heroDataRow == null)
            {
                Log.Warning("Hero type '{0}' not found.", heroTypeId);
                return string.Empty;
            }

            var iconDataTable = GameEntry.DataTable.GetDataTable<DRIcon>();
            DRIcon iconDataRow = iconDataTable.GetDataRow(heroDataRow.IconId);
            if (iconDataRow == null)
            {
                Log.Warning("Icon '{0}' not found.", heroDataRow.IconId);
                return string.Empty;
            }

            return iconDataRow.SpriteName;
        }

        /// <summary>
        /// 获取玩家头像编号。
        /// </summary>
        /// <returns>玩家头像编号。</returns>
        public static int GetPlayerPortraitIconId()
        {
            var playerPortraitId = GameEntry.Data.Player.PortraitType;
            return GetPlayerPortraitIconId(playerPortraitId);
        }

        /// <summary>
        /// 获取玩家头像编号。
        /// </summary>
        /// <param name="playerPortraitId">玩家头像编号。</param>
        /// <returns>玩家头像编号。</returns>
        public static int GetPlayerPortraitIconId(int playerPortraitId)
        {
            var dtPlayerPortrait = GameEntry.DataTable.GetDataTable<DRPlayerPortrait>();
            if (!dtPlayerPortrait.HasDataRow(playerPortraitId))
            {
                playerPortraitId = 1;
            }

            DRPlayerPortrait drPlayerPortrait = dtPlayerPortrait.GetDataRow(playerPortraitId);
            if (drPlayerPortrait == null)
            {
                Log.Warning("Player portrait '{0}' not found.", playerPortraitId);
                return -1;
            }

            int iconId = 0;
            if (drPlayerPortrait.HeroId <= 0)
            {
                iconId = drPlayerPortrait.IconId;
            }
            else
            {
                var drHero = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(drPlayerPortrait.HeroId);
                if (drHero != null)
                {
                    iconId = drHero.IconId;
                }
                else
                {
                    Log.Warning("drHero not found, PlayerPortraitId is '{0}',HeroId is '{1}' .", playerPortraitId, drPlayerPortrait.HeroId);
                    return -1;
                }
            }

            return iconId;
        }

        /// <summary>
        /// 根据职业编号获取职业名称。
        /// </summary>
        /// <param name="profession">职业编号。</param>
        /// <returns>职业名称的本地化文本。</returns>
        public static string GetProfessionName(int profession)
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRProfession>();

            DRProfession dataRow = dataTable.GetDataRow(profession);
            if (dataRow == null)
            {
                Log.Warning("Profession '{0}' not found.", profession);
                return string.Empty;
            }

            return GameEntry.Localization.GetString(dataRow.NameKey);
        }

        /// <summary>
        /// 获取所有可用的大厅英雄数据，包括玩家已有和没有的。
        /// </summary>
        /// <returns>大厅英雄数据列表。</returns>
        public static List<BaseLobbyHeroData> GetLobbyHeroesIncludingUnpossessed()
        {
            var possessedHeroes = new HashSet<int>();
            var lobbyHeroDatas = GameEntry.Data.LobbyHeros.Data;
            var allHeroes = new List<BaseLobbyHeroData>();

            for (int i = 0; i < lobbyHeroDatas.Count; ++i)
            {
                var heroData = lobbyHeroDatas[i];
                possessedHeroes.Add(heroData.Type);
                allHeroes.Add(heroData);
            }

            var dataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            var rows = dataTable.GetAllDataRows();

            for (int i = 0; i < rows.Length; ++i)
            {
                int heroTypeId = rows[i].Id;
                if (possessedHeroes.Contains(heroTypeId))
                {
                    continue;
                }

                allHeroes.Add(new UnpossessedLobbyHeroData(heroTypeId));
            }
            return allHeroes;
        }

        /// <summary>
        /// 获取所有未获得大厅英雄数据。
        /// </summary>
        /// <returns></returns>
        public static List<UnpossessedLobbyHeroData> GetLobbyHeroesUnpossessed()
        {
            var allHeroesUnpossessed = new List<UnpossessedLobbyHeroData>();
            var lobbyHeroDatas = GameEntry.Data.LobbyHeros.Data;
            var possessedHeroes = new HashSet<int>();

            for (int i = 0; i < lobbyHeroDatas.Count; ++i)
            {
                var heroData = lobbyHeroDatas[i];
                possessedHeroes.Add(heroData.Type);
            }

            var dataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            var rows = dataTable.GetAllDataRows();

            for (int i = 0; i < rows.Length; ++i)
            {
                int heroTypeId = rows[i].Id;
                if (possessedHeroes.Contains(heroTypeId))
                {
                    continue;
                }
                allHeroesUnpossessed.Add(new UnpossessedLobbyHeroData(heroTypeId));
            }
            return allHeroesUnpossessed;
        }

        /// <summary>
        /// 获取基于时间差的上次登录时间字符串。
        /// </summary>
        /// <param name="timeSpan">时间跨度。</param>
        /// <returns>字符串。</returns>
        public static string GetLastLoginTimeString(TimeSpan timeSpan)
        {
            return GameEntry.Localization.GetString("UI_TEXT_LASTTIMEONLINE", GetTimeSpanString(timeSpan));
        }

        /// <summary>
        /// 获取时间差。
        /// </summary>
        /// <param name="serverUtcTime">服务器时间。</param>
        /// <returns>时间差。</returns>
        public static TimeSpan GetTimeSpan(long serverUtcTime)
        {
            return (new DateTime(serverUtcTime, DateTimeKind.Utc)) - GameEntry.Time.LobbyServerUtcTime;
        }

        /// <summary>
        /// 将时间差解析成字符串。
        /// </summary>
        /// <param name="timeSpan">时间跨度。</param>
        /// <returns>字符串。</returns>
        public static string GetTimeSpanString(TimeSpan timeSpan)
        {
            var localizer = GameEntry.Localization;
            if (timeSpan < TimeSpan.Zero)
            {
                return localizer.GetString("UI_TEXT_MINUTES", "0");
            }

            if (timeSpan.Days > 0)
            {
                return localizer.GetString("UI_TEXT_DAYS", timeSpan.Days.ToString());
            }

            if (timeSpan.Hours > 0)
            {
                return localizer.GetString("UI_TEXT_HOURS", timeSpan.Hours.ToString());
            }

            return localizer.GetString("UI_TEXT_MINUTES", timeSpan.Minutes.ToString());
        }

        /// <summary>
        /// 将是检查解析成字符串。
        /// </summary>
        /// <param name="timeSpan">时间跨度。</param>
        /// <returns>时、分、秒格式字符串。</returns>
        public static string GetTimeSpanStringHms(TimeSpan timeSpan)
        {
            if (timeSpan < TimeSpan.Zero)
            {
                return GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE_SECOND", 0, 0, 0);
            }

            return GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE_SECOND", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        /// <summary>
        /// 将是检查解析成字符串。
        /// </summary>
        /// <param name="dateTime">时间。</param>
        /// <returns>年、月、日格式字符串。</returns>
        public static string GetTimeSpanStringYmd(DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(GameEntry.Localization.GetString("UI_TEXT_DATE_FORMAT"));
        }

        /// <summary>
        /// 获取图集和精灵图名称。
        /// </summary>
        /// <param name="iconId">图标编号。</param>
        /// <param name="atlasName">图集名称。</param>
        /// <param name="spriteName">精灵图名称。</param>
        /// <returns>是否获取成功。</returns>
        public static bool TryGetAtlasAndSpriteName(int iconId, out string atlasName, out string spriteName)
        {
            atlasName = spriteName = null;

            var dt = GameEntry.DataTable.GetDataTable<DRIcon>();
            DRIcon dr = dt.GetDataRow(iconId);
            if (dr == null)
            {
                return false;
            }

            atlasName = dr.AtlasName;
            spriteName = dr.SpriteName;
            return true;
        }

        /// <summary>
        /// 检查玩家能量是否足够，不足弹出提示框。
        /// </summary>
        /// <param name="neededEnergy">所需能量。</param>
        /// <returns>玩家能量是否足够使用。</returns>
        public static bool CheckEnergy(int neededEnergy)
        {
            return CheckCurrency(CurrencyType.Energy, neededEnergy);
        }

        /// <summary>
        /// 检查玩家某种代币是否足够，不足弹出提示框。
        /// </summary>
        /// <param name="currencyType">代币种类。</param>
        /// <param name="neededAmount">所需数量。</param>
        /// <returns>玩家代币是否足够使用。</returns>
        public static bool CheckCurrency(CurrencyType currencyType, int neededAmount)
        {
            bool ret = (neededAmount <= GameEntry.Data.Player.GetCurrencyAmount(currencyType));

            if (!ret)
            {
                ShowToast(GameEntry.Localization.GetString(GetNotEnoughCurrencyStringKey(currencyType)));
            }

            return ret;
        }

        /// <summary>
        /// 获取战魂的属性名称。
        /// </summary>
        /// <param name="attrId">属性编号。</param>
        /// <returns>属性名称。</returns>
        public static string GetSoulAttributeName(int attrId)
        {
            string key = GetSoulAttributeNameKey(attrId);
            if (string.IsNullOrEmpty(key))
            {
                return key;
            }

            return GameEntry.Localization.GetString(key);
        }

        /// <summary>
        /// 获取战魂的属性名称字典的键。
        /// </summary>
        /// <param name="attrId">属性编号。</param>
        /// <returns>字典的键。</returns>
        public static string GetSoulAttributeNameKey(int attrId)
        {
            if (Constant.AttributeName.AttributeNameDics.Length < attrId)
            {
                return string.Empty;
            }
            else
            {
                return Constant.AttributeName.AttributeNameDics[attrId];
            }
        }

        private static int m_ToggleGroupBaseValue = 0;
        private const int ToggleGroupBaseValueInterval = 100;

        /// <summary>
        /// 刷新指定节点下的所有 <see cref="UIToggle"/> 组。
        /// </summary>
        /// <param name="gameObject">指定节点。</param>
        /// <returns><see cref="UIToggle"/> 组。</returns>
        public static int RefreshToggleGroupsForChildren(GameObject gameObject)
        {
            var toggles = gameObject.GetComponentsInChildren<UIToggle>(true);

            for (int i = 0; i < toggles.Length; ++i)
            {
                RefreshToggleGroup(toggles[i], m_ToggleGroupBaseValue);
            }

            int ret = m_ToggleGroupBaseValue;
            m_ToggleGroupBaseValue += ToggleGroupBaseValueInterval;
            return ret;
        }

        /// <summary>
        /// 使得 <see cref="UIGrid"/> 能自适应屏幕宽度。
        /// </summary>
        /// <param name="gridParentWidth">父节点宽度。</param>
        /// <param name="targetGrid">目标 <see cref="UIGrid"/> 对象。</param>
        /// <param name="updateColumnCount">是否自动调整列数。</param>
        /// <returns>是否发生改动。</returns>
        public static bool GridAutoAdaptScreen(float gridParentWidth, UIGrid targetGrid, bool updateColumnCount)
        {
            if (targetGrid == null)
            {
                Log.Warning("'targetGrid' is invalid.");
                return false;
            }

            if (targetGrid.arrangement != UIGrid.Arrangement.Horizontal)
            {
                Log.Warning("'targetGrid' arrangement is not horizontal.");
                return false;
            }

            if (updateColumnCount)
            {
                int ColumnCount = Mathf.FloorToInt(gridParentWidth / targetGrid.cellWidth);

                if (targetGrid.maxPerLine == ColumnCount && Mathf.Abs(targetGrid.cellWidth - gridParentWidth / ColumnCount) < .5f)
                {
                    return false;
                }

                targetGrid.maxPerLine = ColumnCount;
                targetGrid.cellWidth = gridParentWidth / ColumnCount;
                RefreshGridPosition(targetGrid, gridParentWidth);
                return true;
            }
            else if (targetGrid.maxPerLine <= 0)
            {
                return false;
            }
            else
            {
                if (Mathf.Abs(targetGrid.cellWidth - gridParentWidth / targetGrid.maxPerLine) < .5f)
                {
                    return false;
                }

                targetGrid.cellWidth = gridParentWidth / targetGrid.maxPerLine;
                RefreshGridPosition(targetGrid, gridParentWidth);
                return true;
            }
        }

        /// <summary>
        /// 刷新 <see cref="UIToggle"/> 组。
        /// </summary>
        /// <param name="toggle">目标 <see cref="UIToggle"/> 对象。</param>
        /// <param name="baseValue">组的基础值。</param>
        public static void RefreshToggleGroup(UIToggle toggle, int baseValue)
        {
            if (toggle == null)
            {
                return;
            }

            if (baseValue < 0)
            {
                Log.Error("Base value should not be less than zero.");
                return;
            }

            if (toggle.group == 0)
            {
                return;
            }

            if (toggle.group > 0)
            {
                toggle.group += baseValue;
            }
            else
            {
                toggle.group -= baseValue;
            }
        }

        /// <summary>
        /// Get <see cref="NGUIForm"/> instance by its ID.
        /// </summary>
        /// <param name="uiFormId">The ID of the <see cref="NGUIForm"/>.</param>
        /// <returns>The <see cref="NGUIForm"/> instance.</returns>
        public static NGUIForm GetUIForm(UIFormId uiFormId)
        {
            IUIGroup uiGroup;
            return GetUIForm(uiFormId, out uiGroup);
        }

        /// <summary>
        /// Get <see cref="NGUIForm"/> instance by its ID.
        /// </summary>
        /// <param name="uiFormId">The ID of the <see cref="NGUIForm"/>.</param>
        /// <param name="uiGroup">The <see cref="IUIGroup"/> instance containing the found <see cref="NGUIForm"/>.</param>
        /// <returns>The <see cref="NGUIForm"/> instance.</returns>
        public static NGUIForm GetUIForm(UIFormId uiFormId, out IUIGroup uiGroup)
        {
            uiGroup = null;
            var dt = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm dr = dt.GetDataRow((int)uiFormId);
            if (dr == null)
            {
                Log.Warning("UIFormId '{0}' is invalid.", uiFormId);
                return null;
            }

            var myUIGroup = GameEntry.UI.GetUIGroup(dr.UIGroupName);
            var ui = myUIGroup.GetUIForm((int)uiFormId);
            if (ui != null)
            {
                uiGroup = myUIGroup;
                return (ui as UIForm).Logic as NGUIForm;
            }

            return null;
        }

        /// <summary>
        /// 显示仅有确定按钮的对话框。
        /// </summary>
        /// <param name="dialogMessage">消息文本。</param>
        public static void ShowOkayButtonDialog(string dialogMessage)
        {
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Mode = 1, Message = dialogMessage });
        }

        /// <summary>
        /// 显示有重启和取消按钮的对话框。
        /// </summary>
        /// <param name="dialogMessage">消息文本。</param>
        public static void ShowRestartAndCancelDialog(string dialogMessage)
        {
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = dialogMessage,
                ConfirmText = GameEntry.Localization.GetString("UI_TEXT_RESTART"),
                OnClickConfirm = OnRestartFromDialog,
                CancelText = GameEntry.Localization.GetString("UI_TEXT_DONOT_RESTART"),
            });
        }

        /// <summary>
        /// 显示飘窗。
        /// </summary>
        /// <param name="toastMessage">消息文本。</param>
        public static void ShowToast(string toastMessage)
        {
            GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = toastMessage });
        }

        /// <summary>
        /// 显示仅有重启按钮的对话框。
        /// </summary>
        /// <param name="dialogMessage"></param>
        public static void ShowRestartDialog(string dialogMessage)
        {
            DialogDisplayData dialogData = new DialogDisplayData
            {
                Message = dialogMessage,
                ConfirmText = GameEntry.Localization.GetString("UI_TEXT_RESTART"),
                OnClickConfirm = OnRestartFromDialog,
            };

            if (Debug.isDebugBuild)
            {
                dialogData.Mode = 2;
                dialogData.CancelText = GameEntry.Localization.GetString("UI_TEXT_DONOT_RESTART");
            }
            else
            {
                dialogData.Mode = 1;
            }

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, dialogData);
        }

        /// <summary>
        /// 获取战魂效果值的字符串。
        /// </summary>
        /// <param name="soul">战魂数据。</param>
        /// <returns>战魂效果值的字符串</returns>
        public static string GetSoulEffectValueText(SoulData soul)
        {
            string strValue = string.Empty;
            float value = soul.EffectValue;

            if (value < 2 && (value - (int)value) > 0)
            {
                strValue = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", value);
            }
            else
            {
                strValue = value.ToString();
            }

            return strValue;
        }

        /// <summary>
        /// 设置挑战次数。
        /// </summary>
        /// <param name="label">文本标签。</param>
        /// <param name="remainingPlayCount">可挑战次数。</param>
        /// <param name="freePlayCount">免费次数。</param>
        /// <param name="formatKey">格式化字典。</param>
        public static void SetPlayCountLabel(UILabel label, int remainingPlayCount, int freePlayCount, string formatKey)
        {
            string playCountStr = GameEntry.Localization.GetString("UI_TEXT_SLASH", Mathf.Max(0, remainingPlayCount), Mathf.Max(0, freePlayCount));
            label.text = GameEntry.Localization.GetString(formatKey, playCountStr);
        }

        /// <summary>
        /// 获取离线竞技挑战次数。
        /// </summary>
        /// <param name="remainingPlayCount">可挑战次数。</param>
        /// <param name="freePlayCount">免费次数。</param>
        public static void GetPlayCount_OfflineArena(out int remainingPlayCount, out int freePlayCount)
        {
            remainingPlayCount = 0;
            freePlayCount = 0;
            var vipConfig = GameEntry.DataTable.GetDataTable<DRVip>();
            var drVip = vipConfig.GetDataRow(GameEntry.Data.Player.VipLevel);
            if (drVip == null)
            {
                Log.Error("Error vip configuration, can not get data where id (VipLevel) is '{0}'.", GameEntry.Data.Player.VipLevel);
                return;
            }

            remainingPlayCount = drVip.FreeArenaCount + GameEntry.Data.OfflineArena.TodayBoughtCount - GameEntry.Data.OfflineArena.TodayChallengedCount;
            freePlayCount = drVip.FreeArenaCount;
        }

        /// <summary>
        /// 获取金币资源副本挑战次数。
        /// </summary>
        /// <param name="remainingPlayCount">可挑战次数。</param>
        /// <param name="freePlayCount">免费次数。</param>
        public static void GetPlayCount_InstanceForCoinResource(out int remainingPlayCount, out int freePlayCount)
        {
            remainingPlayCount = 0;
            freePlayCount = 0;
            var vipConfig = GameEntry.DataTable.GetDataTable<DRVip>();
            var drVip = vipConfig.GetDataRow(GameEntry.Data.Player.VipLevel);
            if (drVip == null)
            {
                Log.Error("Error vip configuration, can not get data where id (VipLevel) is '{0}'.", GameEntry.Data.Player.VipLevel);
                return;
            }

            freePlayCount = drVip.FreeCoinResourceInstanceCount;
            remainingPlayCount = drVip.FreeCoinResourceInstanceCount - GameEntry.Data.InstanceForCoinResourceData.PlayedCount;
        }

        /// <summary>
        /// 获取经验资源副本挑战次数。
        /// </summary>
        /// <param name="remainingPlayCount">可挑战次数。</param>
        /// <param name="freePlayCount">免费次数。</param>
        public static void GetPlayCount_InstanceForExpResource(out int remainingPlayCount, out int freePlayCount)
        {
            remainingPlayCount = 0;
            freePlayCount = 0;
            var vipConfig = GameEntry.DataTable.GetDataTable<DRVip>();
            var drVip = vipConfig.GetDataRow(GameEntry.Data.Player.VipLevel);
            if (drVip == null)
            {
                Log.Error("Error vip configuration, can not get data where id (VipLevel) is '{0}'.", GameEntry.Data.Player.VipLevel);
                return;
            }

            freePlayCount = drVip.FreeExpResourceInstanceCount;
            remainingPlayCount = drVip.FreeExpResourceInstanceCount - GameEntry.Data.InstanceForExpResourceData.PlayedCount;
        }

        /// <summary>
        /// 获取模拟乱斗副本挑战次数。
        /// </summary>
        /// <param name="remainingPlayCount">可挑战次数。</param>
        /// <param name="freePlayCount">免费次数。</param>
        public static void GetPlayCount_MimicMelee(out int remainingPlayCount, out int freePlayCount)
        {
            // TODO：从后端获取数据。
            remainingPlayCount = 5;
            freePlayCount = 5;
        }

        /// <summary>
        /// 获取Boss副本挑战次数
        /// </summary>
        /// <param name="remainingPlayCount">可挑战次数</param>
        /// <param name="freePlayCount">免费次数</param>
        public static void GetPlayCount_BossChallenge(out int remainingPlayCount, out int freePlayCount)
        {
            // TODO：从后端获取数据。
            remainingPlayCount = 3;
            freePlayCount = 3;
        }

        /// <summary>
        /// 获取风暴之塔挑战次数
        /// </summary>
        /// <param name="remainingPlayCount">可挑战次数</param>
        /// <param name="freePlayCount">免费次数</param>
        public static void GetPlayCount_StormTower(out int remainingPlayCount, out int freePlayCount)
        {
            // TODO：从后端获取数据。
            freePlayCount = 2;
            remainingPlayCount = freePlayCount - GameEntry.Data.StromTowerData.StromTowerInfo.ChallengeNum;
        }

        private static void RefreshGridPosition(UIGrid targetGrid, float parentWidth)
        {
            Transform gridTransform = targetGrid.transform;
            float totalWidth = targetGrid.cellWidth * targetGrid.maxPerLine;
            UIPanel parentPanel = null;
            UIWidget parentWidget = null;

            if (gridTransform.parent != null)
            {
                parentPanel = gridTransform.parent.GetComponent<UIPanel>();
                parentWidget = gridTransform.parent.GetComponent<UIWidget>();
            }

            float baseOffset = 0f;
            if (parentWidget != null)
            {
                baseOffset = (parentWidget.pivot == UIWidget.Pivot.TopLeft || parentWidget.pivot == UIWidget.Pivot.Left || parentWidget.pivot == UIWidget.Pivot.BottomLeft) ? totalWidth * .5f
                    : (parentWidget.pivot == UIWidget.Pivot.TopRight || parentWidget.pivot == UIWidget.Pivot.Right || parentWidget.pivot == UIWidget.Pivot.BottomRight) ? -totalWidth * .5f
                    : 0f;
            }
            else if (parentPanel != null && parentPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                baseOffset = parentPanel.finalClipRegion.x;
            }

            if (targetGrid.pivot == UIWidget.Pivot.TopLeft || targetGrid.pivot == UIWidget.Pivot.Left || targetGrid.pivot == UIWidget.Pivot.BottomLeft)
            {
                gridTransform.SetLocalPositionX(baseOffset + targetGrid.cellWidth * .5f - totalWidth * .5f);
            }
            else if (targetGrid.pivot == UIWidget.Pivot.TopRight || targetGrid.pivot == UIWidget.Pivot.Right || targetGrid.pivot == UIWidget.Pivot.BottomRight)
            {
                gridTransform.SetLocalPositionX(baseOffset + totalWidth * .5f - targetGrid.cellWidth * .5f);
            }
            else
            {
                gridTransform.SetLocalPositionX(baseOffset);
            }
        }

        private static string GetNotEnoughCurrencyStringKey(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Energy:
                    return "UI_TEXT_NOTICE_ENERGYNOTENOUGH";
                case CurrencyType.Money:
                    return "UI_TEXT_NOTICE_DIAMONDNOTENOUGH";
                case CurrencyType.Coin:
                    return "UI_TEXT_NOTICE_COINNOTENOUGH";
                case CurrencyType.SpiritToken:
                    return "UI_TEXT_NOTICE_SPIRITNOTENOUGH";
                case CurrencyType.ArenaToken:
                    return "UI_TEXT_ARENA_TOKEN_INSUFFICIENT";
                default:
                    Log.Warning("CurrencyType '{0}' is unsupported.", currencyType);
                    return string.Empty;
            }
        }

        private static void OnRestartFromDialog(object userData)
        {
            GameEntry.Restart();
        }

        public static string GetAttributeValueStr(AttributeType type, float value)
        {
            string str = string.Empty;
            switch (type)
            {
                case AttributeType.MaxHP:
                case AttributeType.PhysicalAttack:
                case AttributeType.MagicAttack:
                case AttributeType.PhysicalDefense:
                case AttributeType.MagicDefense:
                case AttributeType.AdditionalDamage:
                    {
                        str = value.ToString();
                        break;
                    }
                case AttributeType.Speed:
                    {
                        str = GameEntry.Localization.GetString("UI_TEXT_ATTRIBUTE_NUMBER_SECOND", value);
                        break;
                    }
                case AttributeType.CriticalHitProb:
                case AttributeType.CriticalHitRate:
                case AttributeType.PhysicalAtkHPAbsorbRate:
                case AttributeType.MagicAtkHPAbsorbRate:
                case AttributeType.PhysicalAtkReflectRate:
                case AttributeType.MagicAtkReflectRate:
                case AttributeType.OppPhysicalDfsReduceRate:
                case AttributeType.OppMagicDfsReduceRate:
                case AttributeType.AntiCriticalHitProb:
                case AttributeType.DamageReductionRate:
                case AttributeType.PhysicalAtkIncreaseRate:
                case AttributeType.MagicAtkIncreaseRate:
                case AttributeType.PhysicalDfsIncreaseRate:
                case AttributeType.MagicDfsIncreaseRate:
                case AttributeType.MaxHPIncreaseRate:
                case AttributeType.AngerIncreaseRate:
                    {
                        str = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", value);
                        break;
                    }
                case AttributeType.RecoverHP:
                    {
                        str = GameEntry.Localization.GetString("UI_TEXT_ATTRIBUTE_NUMBER_SECOND", value);
                        break;
                    }
                case AttributeType.ReducedSkillCoolDown:
                case AttributeType.ReducedHeroSwitchCoolDown:
                    {
                        str = GameEntry.Localization.GetString("UI_TEXT_ATTRIBUTE_SECOND", value);
                        break;
                    }
            }
            return str;
        }

        public static bool IsInUIViewPort(Vector3 worldPoint, Vector3 offset = default(Vector3))
        {
            if (!GameEntry.IsAvailable || GameEntry.Scene == null)
            {
                return false;
            }

            if (GameEntry.Scene.MainCamera == null)
            {
                return false;
            }

            Vector3 screenPoint = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPoint);
            if (screenPoint.x < Screen.width * (-1) || screenPoint.x > Screen.width)
                return false;

            if (screenPoint.y < Screen.height * (-1) || screenPoint.y > Screen.height)
                return false;
            return true;
        }

        /// <summary>
        /// 获取某种类型的礼包的配置信息
        /// </summary>
        /// <param name="giftType">礼包的类型</param>
        /// <returns></returns>
        public static List<DRGfitBag> GetGiftType(GiftType giftType)
        {
            IDataTable<DRGfitBag> giftRow = GameEntry.DataTable.GetDataTable<DRGfitBag>();

            var all = giftRow.GetAllDataRows();
            List<DRGfitBag> GetGiftType = new List<DRGfitBag>();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].Type == (int)giftType)
                {
                    GetGiftType.Add(all[i]);
                }
            }
            if (GetGiftType.Count == 0)
            {
                Log.Info("没有找到{0}类型的礼包", giftType);
                return null;
            }
            return GetGiftType;
        }
    }
}

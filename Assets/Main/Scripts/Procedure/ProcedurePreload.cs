using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Resource;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ProcedurePreload : ProcedureBase
    {
        private bool m_LoadedTimeLineFlag = false;

        private IDictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();
        private LoadAssetCallbacks m_LoadAssetCallbacks;

        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure);
            GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);

            m_LoadedTimeLineFlag = false;

            GameEntry.Event.Subscribe(EventId.LoadTimeLineGroupSuccess, OnLoadTimeLineGroupSuccess);
            GameEntry.Event.Subscribe(EventId.LoadTimeLineGroupFailure, OnLoadTimeLineGroupFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDictionarySuccess, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDictionaryFailure, OnLoadDictionaryFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
            GameEntry.Event.Subscribe(EventId.LoadLuaScriptSuccess, OnLoadLuaScriptSuccess);
            GameEntry.Event.Subscribe(EventId.LoadLuaScriptFailure, OnLoadLuaScriptFailure);

            PreloadResources();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.LoadTimeLineGroupSuccess, OnLoadTimeLineGroupSuccess);
                GameEntry.Event.Unsubscribe(EventId.LoadTimeLineGroupFailure, OnLoadTimeLineGroupFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDictionarySuccess, OnLoadDictionarySuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDictionaryFailure, OnLoadDictionaryFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
                GameEntry.Event.Unsubscribe(EventId.LoadLuaScriptSuccess, OnLoadLuaScriptSuccess);
                GameEntry.Event.Unsubscribe(EventId.LoadLuaScriptFailure, OnLoadLuaScriptFailure);

                GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
            }

            m_LoadAssetCallbacks = null;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_LoadedTimeLineFlag)
            {
                return;
            }

            if (!GameEntry.Impact.PreloadComplete || !GameEntry.Loading.PreloadComplete || !GameEntry.UIBackground.PreloadComplete || !GameEntry.ScreenTouchEffect.PreloadComplete)
            {
                return;
            }

            foreach (bool i in m_LoadedFlag.Values)
            {
                if (!i)
                {
                    return;
                }
            }

            //var shaderWarmUpStartTime = Time.realtimeSinceStartup;
            //Shader.WarmupAllShaders();
            //var shaderWarmUpEndTime = Time.realtimeSinceStartup;
            //Log.Info("Shader warming up completed. Time elapsed is {0} seconds.", (shaderWarmUpEndTime - shaderWarmUpStartTime).ToString());

            GameEntry.Lua.StartVM();
            ChangeProcedure(procedureOwner);
        }

        private void PreloadResources()
        {
            LoadTimeLineGroup("EntityTimeLine");

            LoadDictionary("Achievement");
            LoadDictionary("Activity");
            LoadDictionary("Announcement");
            LoadDictionary("Common");
            LoadDictionary("EnemySkill");
            LoadDictionary("Gear");
            LoadDictionary("Guide");
            LoadDictionary("HeroSkill");
            LoadDictionary("Item");
            LoadDictionary("Name");
            LoadDictionary("OperationActivity");
            LoadDictionary("PlotDialogue");
            LoadDictionary("Propaganda");
            LoadDictionary("RandomName");
            LoadDictionary("ServerError");
            LoadDictionary("SkillBadgeEffectDesc");
            LoadDictionary("Tips");

            LoadDataTable("Achievement");
            LoadDataTable("Activity");
            LoadDataTable("AirWall");
            LoadDataTable("AltSkill");
            LoadDataTable("Animation");
            LoadDataTable("AnimationCrossFade");
            LoadDataTable("Announcement");
            LoadDataTable("ArenaCost");
            LoadDataTable("ArenaReward");
            LoadDataTable("Buff");
            LoadDataTable("BuffCharacterEffect");
            LoadDataTable("BuffReplace");
            LoadDataTable("Building");
            LoadDataTable("BuildingAnimation");
            LoadDataTable("BuildingModel");
            LoadDataTable("Bullet");
            LoadDataTable("BulletRebounder");
            LoadDataTable("ChanceCost");
            LoadDataTable("ChanceRefresh");
            LoadDataTable("ChangeColor");
            LoadDataTable("Character");
            LoadDataTable("CharacterEffect");
            //LoadDataTable("Charge");
            LoadDataTable("DailyLogin");
            LoadDataTable("DailyQuest");
            LoadDataTable("DailyQuestActiveness");
            LoadDataTable("DemoInstance");
            LoadDataTable("DodgeSkill");
            LoadDataTable("DropProbability");
            LoadDataTable("Effect");
            LoadDataTable("Element");
            LoadDataTable("Epigraph");
            LoadDataTable("Exchange");
            LoadDataTable("Gear");
            LoadDataTable("GearLevelUp");
            LoadDataTable("GeneralItemWhereToGet");
            LoadDataTable("GenericSkillBadge");
            LoadDataTable("GuidePointSet");
            LoadDataTable("Hero");
            LoadDataTable("HeroBase");
            LoadDataTable("HeroQualityItem");
            LoadDataTable("HeroQualityLevel");
            LoadDataTable("HeroQualityMaxLevel");
            LoadDataTable("Icon");
            LoadDataTable("Impact");
            LoadDataTable("Instance");
            LoadDataTable("InstanceCosmosCrack");
            LoadDataTable("InstanceForResource");
            LoadDataTable("InstanceGroup");
            LoadDataTable("Item");
            LoadDataTable("LevelRange");
            LoadDataTable("LoadingBackground");
            LoadDataTable("LobbyNpc");
            LoadDataTable("Meridian");
            LoadDataTable("MimicMeleeBase");
            LoadDataTable("MimicMeleeInstance");
            LoadDataTable("Music");
            LoadDataTable("NearbyPlayerRandomPosition");
            LoadDataTable("NewGear");
            LoadDataTable("NewGearQualityLevel");
            LoadDataTable("NewGearQualityMaxLevel");
            LoadDataTable("NewGearStrengthenLevel");
            LoadDataTable("Npc");
            LoadDataTable("NpcBase");
            LoadDataTable("NpcInMimicMelee");
            LoadDataTable("OperationActivity");
            LoadDataTable("Player");
            LoadDataTable("PlayerPortrait");
            LoadDataTable("PlotDialogue");
            LoadDataTable("Portal");
            LoadDataTable("Profession");
            LoadDataTable("PvpaiInstance");
            LoadDataTable("PvpTitle");
            LoadDataTable("Scene");
            LoadDataTable("ShopRefresh");
            LoadDataTable("SinglePvpInstance");
            LoadDataTable("Skill");
            LoadDataTable("SkillBadgeSlotUnlockCondition");
            LoadDataTable("SkillChargeTime");
            LoadDataTable("SkillContinualTap");
            LoadDataTable("SkillGroup");
            LoadDataTable("SkillLevelUp");
            LoadDataTable("Soul");
            LoadDataTable("Sound");
            LoadDataTable("SpecificSkillBadge");
            LoadDataTable("Task");
            LoadDataTable("UIForm");
            LoadDataTable("UITexture");
            LoadDataTable("Vip");
            LoadDataTable("Weapon");
            LoadDataTable("WeaponAnimation");
            LoadDataTable("WeaponAnimationCrossFade");
            LoadDataTable("WeaponSuite");
            LoadDataTable("WelfareCenter");
            LoadDataTable("OnlineRewards");
            LoadDataTable("SevenDayLogin");
            LoadDataTable("WhereToGet");
            LoadDataTable("GfitBag");
            LoadDataTable("DisplayModel");
            LoadDataTable("PvpRank");
            LoadDataTable("OpenFunction");
            LoadDataTable("TaskTalk");
            LoadDataTable("GuideUI");
            LoadDataTable("InstanceForTower");
            LoadDataTable("InstanceGroupForBoss");

            LoadLuaScriptsToPreload();
            LoadShadersForWarmUp();

            GameEntry.Impact.Preload();
            GameEntry.Loading.Preload();
            GameEntry.UIBackground.Preload();
            GameEntry.ScreenTouchEffect.Preload();
            GameEntry.Tutorial.Preload();
        }

        private void LoadShadersForWarmUp()
        {
            var shaderPathsForWarmUp = GameEntry.ClientConfig.GetShaderPathsForWarmUp();
            for (int i = 0; i < shaderPathsForWarmUp.Count; i++)
            {
                var shaderPath = shaderPathsForWarmUp[i];
                m_LoadedFlag.Add(string.Format("Shader.{0}", shaderPath), false);
                GameEntry.Resource.LoadAsset(shaderPath, m_LoadAssetCallbacks);
            }
        }

        private void LoadLuaScriptsToPreload()
        {
            var scriptsToPreload = GameEntry.ClientConfig.GetLuaScriptsToPreload();
            for (int i = 0; i < scriptsToPreload.Count; i++)
            {
                LoadLuaScript(scriptsToPreload[i].Name, scriptsToPreload[i].Category);
            }
        }

        private void LoadLuaScript(string scriptPath, LuaScriptCategory category)
        {
            m_LoadedFlag.Add(string.Format("Lua.{0}", scriptPath), false);
            GameEntry.Lua.LoadScript(scriptPath, category);
        }

        private void LoadDataTable(string dataTableName)
        {
            m_LoadedFlag.Add(string.Format("DataTable.{0}", dataTableName), false);
            GameEntry.DataTable.LoadDataTable(dataTableName);
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs;
            m_LoadedFlag[string.Format("DataTable.{0}", ne.DataTableName)] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadDataTableFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadDataTableFailureEventArgs;
            OnError("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void LoadDictionary(string dictionaryName)
        {
            m_LoadedFlag.Add(string.Format("Dictionary.{0}", dictionaryName), false);
            GameEntry.Localization.LoadDictionary(dictionaryName);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadDictionarySuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadDictionarySuccessEventArgs;
            m_LoadedFlag[string.Format("Dictionary.{0}", ne.DictionaryName)] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadDictionaryFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadDictionaryFailureEventArgs;
            OnError("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryName, ne.DictionaryAssetName, ne.ErrorMessage);
        }

        private void LoadTimeLineGroup(string timeLineGroupName)
        {
            m_LoadedTimeLineFlag = false;
            GameEntry.TimeLine.LoadTimeLineGroup(timeLineGroupName);
        }

        private void OnLoadTimeLineGroupSuccess(object sender, GameEventArgs e)
        {
            LoadTimeLineGroupSuccessEventArgs ne = e as LoadTimeLineGroupSuccessEventArgs;
            m_LoadedTimeLineFlag = true;
            Log.Info("Load time line '{0}' OK.", ne.TimeLineName);
        }

        private void OnLoadTimeLineGroupFailure(object sender, GameEventArgs e)
        {
            LoadTimeLineGroupFailureEventArgs ne = e as LoadTimeLineGroupFailureEventArgs;
            OnError("Can not load time line '{0}' from '{1}' with error message '{2}'.", ne.TimeLineName, ne.TimeLineAssetName, ne.ErrorMessage);
        }

        private void OnLoadLuaScriptSuccess(object sender, GameEventArgs e)
        {
            var ne = e as LoadLuaScriptSuccessEventArgs;
            m_LoadedFlag[string.Format("Lua.{0}", ne.ScriptName)] = true;
            Log.Info("Load Lua script '{0}' okay.", ne.ScriptName);
        }

        private void OnLoadLuaScriptFailure(object sender, GameEventArgs e)
        {
            var ne = e as LoadLuaScriptFailureEventArgs;
            OnError("Failed to load lua script '{0}'.", ne.ScriptName);
        }

        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            string key = string.Format("Shader.{0}", assetName);
            if (!m_LoadedFlag.ContainsKey(key))
            {
                return;
            }

            m_LoadedFlag[key] = true;
            Log.Info("Load shader '{0}' OK.", assetName);
        }

        private void OnLoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            OnError("Failed to load asset '{0}'.", assetName);
        }


        private void ChangeProcedure(ProcedureOwner procedureOwner)
        {
            ChangeState(procedureOwner, GameEntry.OfflineMode.OfflineModeEnabled ? typeof(ProcedureLoadTexts) : typeof(ProcedureCheckServerList));
        }
    }
}

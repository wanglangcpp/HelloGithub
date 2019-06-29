using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private class Preparer : AbstractInstancePreparer
        {
            private BaseSinglePlayerInstanceLogic m_BaseSinglePlayerInstanceLogic = null;

            public BaseSinglePlayerInstanceLogic InstanceLogic
            {
                get
                {
                    if (m_BaseSinglePlayerInstanceLogic == null)
                    {
                        m_BaseSinglePlayerInstanceLogic = m_InstanceLogic as BaseSinglePlayerInstanceLogic;
                    }

                    return m_BaseSinglePlayerInstanceLogic;
                }
            }

            public override void Init(BaseInstanceLogic instanceLogic)
            {
                base.Init(instanceLogic);

                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
                GameEntry.Event.Subscribe(EventId.InstanceMePrepared, OnMePrepared);
            }

            public override void Shutdown(bool isExternalShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
                    GameEntry.Event.Unsubscribe(EventId.InstanceMePrepared, OnMePrepared);
                }

                base.Shutdown(isExternalShutdown);
            }

            public override void OnLoadBehaviorSuccess(LoadBehaviorSuccessEventArgs e)
            {
                LoadBehaviorSuccessEventArgs ne = e as LoadBehaviorSuccessEventArgs;

                Log.Info("Load behavior '{0}' OK.", ne.BehaviorName);
                if (!InstanceLogic.m_BehaviorsToLoad.Contains(ne.Behavior))
                {
                    return;
                }

                if (ne.Behavior == InstanceLogic.m_SceneBehavior)
                {
                    Log.Info("Load behavior '{0}' OK.", ne.BehaviorName);
                    InstanceLogic.AddBehaviorTree(InstanceLogic.m_SceneBehavior);
                    InstanceLogic.m_BehaviorsToLoad.Remove(InstanceLogic.m_SceneBehavior);
                }
                else if (ne.Behavior == InstanceLogic.m_MyPlayerBehavior)
                {
                    Log.Debug("Load behavior '{0}' OK.", ne.BehaviorName);
                    InstanceLogic.m_BehaviorsToLoad.Remove(InstanceLogic.m_MyPlayerBehavior);
                }

                if (InstanceLogic.m_BehaviorsToLoad.Count <= 0)
                {
                    InstanceLogic.PrepareAndShowMeHero();
                }
            }

            public override void OnLoadSceneSuccess(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e)
            {
                LoadInstanceNpcsDataTableIfNeeded();
            }

            public override void OnOpenUIFormSuccess(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e)
            {
                if (e.UIForm.Logic is BattleForm)
                {
                    GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());
                }
            }

            public override void StartInstance()
            {
                InstanceLogic.m_Target = new TargetManager();
                FireShouldGoToWaiting();
            }

            private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
            {
                UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs;
                Log.Info("Load data table '{0}' OK.", ne.DataTableName);

                if (ne.DataTableName == InstanceLogic.m_InstanceNpcsDTName)
                {
                    LoadInstanceBuildingsIfNeeded();
                }

                if (ne.DataTableName == InstanceLogic.m_InstanceBuildingsDTName)
                {
                    LoadSceneLogic();
                }
            }

            private void OnLoadDataTableFailure(object sender, GameEventArgs e)
            {
                UnityGameFramework.Runtime.LoadDataTableFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadDataTableFailureEventArgs;
                Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
            }

            private void LoadSceneLogic()
            {
                GameObject gameObject = new GameObject("Scene Logic");
                InstanceLogic.m_SceneBehavior = gameObject.AddComponent<BehaviorTree>();
                InstanceLogic.m_SceneBehavior.StartWhenEnabled = false;
                InstanceLogic.m_SceneBehavior.ExternalBehavior = null;
                GameEntry.Behavior.LoadBehavior(InstanceLogic.m_SceneBehavior, InstanceLogic.m_SceneBehaviorName);
                InstanceLogic.m_BehaviorsToLoad.Add(InstanceLogic.m_SceneBehavior);

                InstanceLogic.LoadMyPlayerAI(gameObject);
                InstanceLogic.m_BehaviorsToLoad.Add(InstanceLogic.m_MyPlayerBehavior);
            }

            private void OnMePrepared(object sender, GameEventArgs e)
            {
                InstanceLogic.InitPropagandaManager();
                GameEntry.Input.JoystickActivated = true;
                GameEntry.Input.SkillActivated = true;
                GameEntry.SceneLogic.OpenBattleForm();
            }

            private void LoadInstanceNpcsDataTableIfNeeded()
            {
                var dtName = InstanceLogic.m_InstanceNpcsDTName;
                string[] splitNames = dtName.Split('_');
                if (splitNames.Length != 2)
                {
                    Log.Warning("Instance NPCs data table is invalid.");
                    return;
                }

                string dtRealName = splitNames[1];
                if (GameEntry.DataTable.HasDataTable<DRInstanceNpcs>(dtRealName))
                {
                    LoadInstanceBuildingsIfNeeded();
                }
                else
                {
                    GameEntry.DataTable.LoadDataTable(dtName);
                }
            }

            private void LoadInstanceBuildingsIfNeeded()
            {
                string dtName = InstanceLogic.m_InstanceBuildingsDTName;

                if (string.IsNullOrEmpty(dtName)) // Don't need InstanceBuildings
                {
                    LoadSceneLogic();
                    return;
                }

                string[] splitNames = dtName.Split('_');
                if (splitNames.Length != 2)
                {
                    Log.Warning("Instance Buildings data table is invalid.");
                    return;
                }

                string dtRealName = splitNames[1];
                if (GameEntry.DataTable.HasDataTable<DRInstanceBuildings>(dtRealName))
                {
                    LoadSceneLogic();
                }
                else
                {
                    GameEntry.DataTable.LoadDataTable(dtName);
                }
            }
        }
    }
}

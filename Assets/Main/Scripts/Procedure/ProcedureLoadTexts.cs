using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureLoadTexts : ProcedureBase
    {
        private IDictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);

            m_LoadedFlag.Clear();

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDictionarySuccess, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDictionaryFailure, OnLoadDictionaryFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
            LoadTexts();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDictionarySuccess, OnLoadDictionarySuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDictionaryFailure, OnLoadDictionaryFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadDataTableFailure, OnLoadDataTableFailure);

                GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            foreach (bool i in m_LoadedFlag.Values)
            {
                if (!i)
                {
                    return;
                }
            }

            GameEntry.DataTable.InitElementMatrix();
            ChangeProcedure(procedureOwner);
        }

        private void LoadTexts()
        {

        }

        private void LoadDictionary(string dictionaryName)
        {
            m_LoadedFlag.Add(string.Format("Dictionary.{0}", dictionaryName), false);
            if (GameEntry.OfflineMode.OfflineModeEnabled || GameEntry.OfflineMode.ForceUseLocalTexts)
            {
                GameEntry.Localization.LoadDictionary(dictionaryName);
            }
            else
            {
                string path = Utility.Path.GetCombinePath(TempTextsRootPath, TextType.Dictionary.ToString(), GameEntry.Localization.Language.ToString(), string.Format("{0}.txt", dictionaryName));
                string content = File.ReadAllText(path);
                GameEntry.Localization.ParseDictionary(content);
                m_LoadedFlag[string.Format("Dictionary.{0}", dictionaryName)] = true;
            }
        }

        private void LoadDataTable(string dataTableName)
        {
            m_LoadedFlag.Add(string.Format("DataTable.{0}", dataTableName), false);
            if (GameEntry.OfflineMode.OfflineModeEnabled || GameEntry.OfflineMode.ForceUseLocalTexts)
            {
                GameEntry.DataTable.LoadDataTable(dataTableName);
            }
            else
            {
                string path = Utility.Path.GetCombinePath(TempTextsRootPath, TextType.DataTable.ToString(), string.Format("{0}.txt", dataTableName));
                string content = File.ReadAllText(path);
                Type dataTableType = Type.GetType(string.Format("Genesis.GameClient.DR{0}", dataTableName));
                var method = GameEntry.DataTable.GetType().GetMethod("ReflectionCreateDataTable", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(dataTableType);
                method.Invoke(GameEntry.DataTable, new object[] { null, content });
                m_LoadedFlag[string.Format("DataTable.{0}", dataTableName)] = true;
            }
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

        private void ChangeProcedure(ProcedureOwner procedureOwner)
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.OfflineMode.PrepareData();
                int sceneId = GameEntry.OfflineMode.OfflineSceneId;
                GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.NonInstance, sceneId, true)); // Dummy event for refresh scene logic.
                ProcedureChangeScene procedureChangeScene = procedureOwner.GetState<ProcedureChangeScene>();
                procedureChangeScene.ChangeScene(procedureOwner, InstanceLogicType.NonInstance, true);
            }
            else
            {
                bool needCreatePlayer = false;
                if (GameEntry.Data.HasTempData(Constant.TempData.NeedCreatePlayer))
                {
                    needCreatePlayer = GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer);
                }

                if (needCreatePlayer)
                {
                    //manager.ChangeProcedure("PreDemoInstance");
                    //ChangeState<ProcedureCreatePlayer>(procedureOwner);
                    ChangeState<ProcedurePreDemoInstance>(procedureOwner);
                }
                else
                {
                    ChangeState<ProcedureSignIn>(procedureOwner);
                }
            }
        }
    }
}

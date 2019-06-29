using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureUpdateTexts : ProcedureBase
    {
        private ProcedureConfig.ProcedureUpdateTextsConfig m_Config = null;

        //private IFsm<ProcedureUpdateTexts> m_Fsm = null;
        private ProcedureOwner m_ProcedureOwner = null;

        private IDictionary<string, UpdateTextMetaData> m_RemoteMetaDatas = new Dictionary<string, UpdateTextMetaData>();
        private IDictionary<string, UpdateTextMetaData> m_LocalMetaDatas = new Dictionary<string, UpdateTextMetaData>();

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        private static string LocalMetaDatasFilePath
        {
            get
            {
                return Utility.Path.GetCombinePath(TempTextsRootPath, "update_text_meta_datas");
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Config = GameEntry.ClientConfig.ProcedureConfig.UpdateTextsConfig;
            m_ProcedureOwner = procedureOwner;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ChangeProcedure(procedureOwner); // 短路更新字典和数据表的行为。

            //GameEntry.Data.Account.ServerData.CheckUrls();
            //m_LocalMetaDatas.Clear();
            //m_RemoteMetaDatas.Clear();
            //m_Fsm = GameEntry.Fsm.CreateFsm(this, new StateRequestMetaData(), new StateDownload(), new StateEnd());
            //m_Fsm.Start<StateRequestMetaData>();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            //if (m_Fsm != null)
            //{
            //    GameEntry.Fsm.DestroyFsm(m_Fsm);
            //    m_Fsm = null;
            //}

            //m_LocalMetaDatas.Clear();
            //m_RemoteMetaDatas.Clear();
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        private void ChangeProcedure(ProcedureOwner procedureOwner)
        {
            ChangeState<ProcedureLoadTexts>(procedureOwner);
        }

        private abstract class StateBase : FsmState<ProcedureUpdateTexts>
        {
            protected IFsm<ProcedureUpdateTexts> m_CachedFsm = null;

            protected override void OnInit(IFsm<ProcedureUpdateTexts> fsm)
            {
                base.OnInit(fsm);
                m_CachedFsm = fsm;
            }
        }

        private static bool IsInCurLang(UpdateTextMetaData metaData)
        {
            if (metaData.Type != TextType.Dictionary.ToString())
            {
                return false;
            }

            var splits = metaData.Name.Split('/');
            if (splits.Length < 2)
            {
                return false;
            }

            return splits[1] == GameEntry.Localization.Language.ToString();
        }

        private class StateRequestMetaData : StateBase
        {
            protected override void OnEnter(IFsm<ProcedureUpdateTexts> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnDownloadMetaDataResponse);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnDownloadMetaDataError);
                GameEntry.Waiting.StartWaiting(WaitingType.Default, string.Format("{0}.{1}", fsm.Owner.GetType().Name, "StateRequestMetaData"));
                //Log.Info("[ProcedureUpdateTexts.StateRequestMetaData OnEnter] {0}", GameEntry.Data.Account.ServerData.DownloadMetaDataUrl);
                //GameEntry.WebRequest.AddWebRequest(string.Format("{0}?serverid={1}", GameEntry.Data.Account.ServerData.DownloadMetaDataUrl, GameEntry.Data.Account.ServerData.Id.ToString()));
            }

            protected override void OnLeave(IFsm<ProcedureUpdateTexts> fsm, bool isShutdown)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnDownloadMetaDataResponse);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnDownloadMetaDataError);
                GameEntry.Waiting.StopWaiting(WaitingType.Default, string.Format("{0}.{1}", fsm.Owner.GetType().Name, "StateRequestMetaData"));
                base.OnLeave(fsm, isShutdown);
            }

            private void OnDownloadMetaDataResponse(object sender, GameEventArgs e)
            {
                WebRequestSuccessEventArgs ne = e as WebRequestSuccessEventArgs;
                PrepareRemoteMetaDatas(ne.GetWebResponseBytes());
                PrepareLocalMetaDatas();
                ChangeState<StateDownload>(m_CachedFsm);
            }

            private void OnDownloadMetaDataError(object sender, GameEventArgs e)
            {
                WebRequestFailureEventArgs ne = e as WebRequestFailureEventArgs;
                m_CachedFsm.Owner.OnError(ne.ErrorMessage);
            }

            private void PrepareLocalMetaDatas()
            {
                UpdateTextMetaDatas rawDatas;
                if (!File.Exists(LocalMetaDatasFilePath))
                {
                    rawDatas = new UpdateTextMetaDatas();
                }
                else
                {
                    var bytes = File.ReadAllBytes(LocalMetaDatasFilePath);
                    rawDatas = Utility.Json.ToObject<UpdateTextMetaDatas>(bytes);
                }

                for (int i = 0; i < rawDatas.Data.Count; ++i)
                {
                    m_CachedFsm.Owner.m_LocalMetaDatas.Add(rawDatas.Data[i].Name, rawDatas.Data[i]);
                }
            }

            private void PrepareRemoteMetaDatas(byte[] responseData)
            {
                UpdateTextMetaDatas rawDatas = Utility.Json.ToObject<UpdateTextMetaDatas>(responseData);

                for (int i = 0; i < rawDatas.Data.Count; ++i)
                {
                    m_CachedFsm.Owner.m_RemoteMetaDatas.Add(rawDatas.Data[i].Name, rawDatas.Data[i]);
                }
            }
        }

        private class StateDownload : StateBase
        {
            private HashSet<int> m_PendingDownloadTaskIds = new HashSet<int>();
            private List<DownloadData> m_ToDownload = new List<DownloadData>();
            private List<UpdateTextMetaData> m_DownloadedMetaDataList = new List<UpdateTextMetaData>();

            protected override void OnEnter(IFsm<ProcedureUpdateTexts> fsm)
            {
                Log.Info("[ProceduresUpdateText.StateDownload OnEnter]");
                m_PendingDownloadTaskIds.Clear();
                m_ToDownload.Clear();
                m_DownloadedMetaDataList.Clear();

                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.DownloadSuccess, OnDownloadSuccess);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.DownloadFailure, OnDownloadFailure);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.DownloadStart, OnDownloadStart);

                var localMetaDataDict = m_CachedFsm.Owner.m_LocalMetaDatas;
                var remoteMetaDataDict = m_CachedFsm.Owner.m_RemoteMetaDatas;
                var deleteList = new List<string>();

                foreach (var kv in localMetaDataDict)
                {
                    var localMetaData = kv.Value;
                    var remoteMetaData = CheckNeedDownload(remoteMetaDataDict, localMetaData, deleteList);

                    if (remoteMetaData != null)
                    {
                        m_ToDownload.Add(new DownloadData { MetaData = remoteMetaData });
                    }
                }

                CheckNeedDownloadNewFiles(localMetaDataDict, remoteMetaDataDict);
                DeleteFiles(localMetaDataDict, deleteList);
                StartDownloading();
            }

            protected override void OnUpdate(IFsm<ProcedureUpdateTexts> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (m_PendingDownloadTaskIds.Count <= 0)
                {
                    ChangeState<StateEnd>(m_CachedFsm);
                }
            }

            protected override void OnLeave(IFsm<ProcedureUpdateTexts> fsm, bool isShutdown)
            {
                UpdateLocalMetaFile();

                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.DownloadSuccess, OnDownloadSuccess);
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.DownloadFailure, OnDownloadFailure);
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.DownloadStart, OnDownloadStart);
                }

                m_PendingDownloadTaskIds.Clear();
                m_ToDownload.Clear();
                m_DownloadedMetaDataList.Clear();
                base.OnLeave(fsm, isShutdown);
            }

            private void UpdateLocalMetaFile()
            {
                var localMetaDataDict = m_CachedFsm.Owner.m_LocalMetaDatas;
                for (int i = 0; i < m_DownloadedMetaDataList.Count; ++i)
                {
                    localMetaDataDict[m_DownloadedMetaDataList[i].Name] = m_DownloadedMetaDataList[i];
                }

                var rawDatas = new UpdateTextMetaDatas();
                foreach (var kv in localMetaDataDict)
                {
                    rawDatas.Data.Add(kv.Value);
                }

                var bytes = Utility.Json.ToJsonData(rawDatas);
                File.WriteAllBytes(LocalMetaDatasFilePath, bytes);
            }

            private void OnDownloadStart(object sender, GameEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.DownloadStartEventArgs;
                Log.Info("[ProceduresUpdateText.StateDownload OnDownloadStart] from URL '{0}' to path '{1}'.", ne.DownloadUri, ne.DownloadPath);
            }

            private void OnDownloadFailure(object sender, GameEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.DownloadFailureEventArgs;
                var userData = ne.UserData as DownloadData;

                if (userData == null)
                {
                    return;
                }

                GameEntry.Waiting.StopWaiting(WaitingType.Default, string.Format("{0}.{1}", m_CachedFsm.Owner.GetType().Name, "StateDownload"));

                userData.RetryCount++;
                if (userData.RetryCount >= m_CachedFsm.Owner.m_Config.DownloadRetryCount)
                {
                    m_CachedFsm.Owner.OnError("[ProceduresUpdateText.StateDownload OnDownloadFailure] Download URL: '{0}', Error Message: '{1}'.", ne.DownloadUri, ne.ErrorMessage);
                }
                else
                {
                    StartDownloading(userData);
                }
            }

            private void OnDownloadSuccess(object sender, GameEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.DownloadSuccessEventArgs;
                var userData = ne.UserData as DownloadData;

                if (userData == null)
                {
                    return;
                }

                var metaData = userData.MetaData;
                byte[] bytes = File.ReadAllBytes(ne.DownloadPath);
                string realHash = Utility.Verifier.GetMD5(bytes);
                string expectedHash = metaData.Hash;

                m_PendingDownloadTaskIds.Remove(ne.SerialId);
                GameEntry.Waiting.StopWaiting(WaitingType.Default, string.Format("{0}.{1}", m_CachedFsm.Owner.GetType().Name, "StateDownload"));

                if (realHash != expectedHash)
                {
                    Log.Info("[ProcedureUpdateTexts.StateDownload OnDownloadSuccess] URL is '{2}'. Download path is '{3}'. Hash code is '{0:X}', but should be '{1:X}'.", realHash, expectedHash, ne.DownloadUri, ne.DownloadPath);

                    if (File.Exists(ne.DownloadPath))
                    {
                        File.Delete(ne.DownloadPath);
                        m_CachedFsm.Owner.m_LocalMetaDatas.Remove(metaData.Name);
                    }

                    userData.RetryCount++;
                    if (userData.RetryCount >= m_CachedFsm.Owner.m_Config.DownloadRetryCount)
                    {
                        m_CachedFsm.Owner.OnError("[ProcedureUpdateTexts.StateDownload OnDownloadSuccess] Download fails. URL is '{0}'.", ne.DownloadUri);
                    }
                    else
                    {
                        StartDownloading(userData);
                    }
                    return;
                }

                try
                {
                    bytes = Utility.Zip.Decompress(bytes);
                    File.WriteAllBytes(ne.DownloadPath, bytes);
                }
                catch (Exception exception)
                {
                    m_CachedFsm.Owner.OnError("Unable to descompresss and resave file '{0}' with error message '{1}'.", ne.DownloadPath, exception.Message);
                    return;
                }

                m_DownloadedMetaDataList.Add(metaData);
            }

            private void CheckNeedDownloadNewFiles(IDictionary<string, UpdateTextMetaData> localMetaDataDict, IDictionary<string, UpdateTextMetaData> remoteMetaDataDict)
            {
                foreach (var kv in remoteMetaDataDict)
                {
                    var remoteMetaData = kv.Value;

                    if (localMetaDataDict.ContainsKey(remoteMetaData.Name))
                    {
                        continue;
                    }

                    if (remoteMetaData.Type == TextType.Dictionary.ToString() && !IsInCurLang(remoteMetaData))
                    {
                        continue;
                    }

                    m_ToDownload.Add(new DownloadData { MetaData = remoteMetaData });
                }
            }

            private void DeleteFiles(IDictionary<string, UpdateTextMetaData> localMetaDataDict, List<string> deleteList)
            {
                for (int i = 0; i < deleteList.Count; ++i)
                {
                    string path = Utility.Path.GetCombinePath(TempTextsRootPath, deleteList[i]);

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    localMetaDataDict.Remove(deleteList[i]);
                }
            }

            private UpdateTextMetaData CheckNeedDownload(IDictionary<string, UpdateTextMetaData> remoteMetaDataDict, UpdateTextMetaData localMetaData, IList<string> deleteList)
            {
                UpdateTextMetaData remoteMetaData;
                if (!remoteMetaDataDict.TryGetValue(localMetaData.Name, out remoteMetaData))
                {
                    deleteList.Add(localMetaData.Name);
                    return null;
                }

                if (remoteMetaData.Hash == localMetaData.Hash)
                {
                    return null;
                }

                if (remoteMetaData.Type == TextType.Dictionary.ToString() && !IsInCurLang(remoteMetaData))
                {
                    return null;
                }

                return remoteMetaData;
            }

            private void StartDownloading()
            {
                for (int i = 0; i < m_ToDownload.Count; ++i)
                {
                    StartDownloading(m_ToDownload[i]);
                }
            }

            private void StartDownloading(DownloadData downloadData)
            {
                //var serverData = GameEntry.Data.Account.ServerData;
                var metaData = downloadData.MetaData;
                var downloadPath = Utility.Path.GetCombinePath(TempTextsRootPath, metaData.Name);

                string downloadUrl = string.Empty;
                //string fileName = Path.GetFileNameWithoutExtension(metaData.Name);
                if (metaData.Type == TextType.DataTable.ToString())
                {
                    //downloadUrl = string.Format("{0}?name={1}&serverid={2}", serverData.DataTableDownloadUrl, fileName, serverData.Id.ToString());
                }
                else if (metaData.Type == TextType.Dictionary.ToString())
                {
                    //downloadUrl = string.Format("{0}?name={1}&language={2}&serverid={3}", serverData.DictionaryDownloadUrl, fileName, GameEntry.Localization.Language.ToString(), serverData.Id.ToString());
                }
                else
                {
                    m_CachedFsm.Owner.OnError("Unknown text type '{0}'", downloadData);
                    return;
                }

                GameEntry.Waiting.StartWaiting(WaitingType.Default, string.Format("{0}.{1}", m_CachedFsm.Owner.GetType().Name, "StateDownload"));
                int taskId = GameEntry.Download.AddDownload(downloadPath, downloadUrl, downloadData);
                m_PendingDownloadTaskIds.Add(taskId);
            }
        }

        private class StateEnd : StateBase
        {
            protected override void OnUpdate(IFsm<ProcedureUpdateTexts> fsm, float elapseSeconds, float realElapseSeconds)
            {
                fsm.Owner.ChangeProcedure(fsm.Owner.m_ProcedureOwner);
            }
        }

        [Serializable]
        public class UpdateTextMetaDatas
        {
            public List<UpdateTextMetaData> Data = new List<UpdateTextMetaData>();
        }

        [Serializable]
        public class UpdateTextMetaData
        {
            public string Type;
            public string Name;
            public string Hash;
        }

        private class DownloadData
        {
            public UpdateTextMetaData MetaData;
            public int RetryCount;
        }
    }
}

using GameFramework;
using GameFramework.Event;
using GameFramework.Localization;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureUpdateResource : ProcedureBase
    {
        private ProcedureConfig.ProcedureUpdateResourceConfig m_Config = null;
        private bool m_UpdateAllComplete = false;
        private int m_UpdateCount = 0;
        private int m_UpdateTotalZipLength = 0;
        private int m_UpdateSuccessCount = 0;
        private IList<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>();
        private Downloading m_DownloadingPanel = null;

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

            m_Config = GameEntry.ClientConfig.ProcedureConfig.UpdateResourceConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_UpdateAllComplete = false;
            m_UpdateCount = 0;
            m_UpdateTotalZipLength = 0;
            m_UpdateSuccessCount = 0;
            m_UpdateLengthData.Clear();

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ResourceCheckComplete, OnResourceCheckComplete);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateStart, OnResourceUpdateStart);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateChanged, OnResourceUpdateChanged);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateSuccess, OnResourceUpdateSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateFailure, OnResourceUpdateFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateAllComplete, OnResourceUpdateAllComplete);

            GameEntry.Resource.SetCurrentVariant(GetCurrentVariant());
            GameEntry.Resource.CheckResources();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ResourceCheckComplete, OnResourceCheckComplete);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateStart, OnResourceUpdateStart);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateChanged, OnResourceUpdateChanged);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateSuccess, OnResourceUpdateSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateFailure, OnResourceUpdateFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ResourceUpdateAllComplete, OnResourceUpdateAllComplete);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_UpdateAllComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        private string GetCurrentVariant()
        {
            switch (GameEntry.Localization.Language)
            {
                case Language.ChineseSimplified:
                    return "zh-cn";
                case Language.ChineseTraditional:
                    return "zh-tw";
                case Language.English:
                    return "en-us";
                case Language.Unspecified:
                default:
                    return "zh-cn";
            }
        }

        private void RefreshProgress()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            int currentTotalUpdateLength = 0;
            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                currentTotalUpdateLength += m_UpdateLengthData[i].Length;
            }

            float progressTotal = (float)currentTotalUpdateLength / m_UpdateTotalZipLength;
            m_DownloadingPanel.ProgressText = GameEntry.Localization.GetString("UI_TEXT_UPDATE_RESOURCE", m_UpdateSuccessCount.ToString(), m_UpdateCount.ToString(), GetSizeString(currentTotalUpdateLength), GetSizeString(m_UpdateTotalZipLength), progressTotal, GetSizeString((int)GameEntry.Download.CurrentSpeed), GetTimeString(GameEntry.Download.CurrentSpeed > 0 ? (int)((m_UpdateTotalZipLength - currentTotalUpdateLength) / GameEntry.Download.CurrentSpeed) : -1));
            m_DownloadingPanel.Progress = progressTotal;
        }

        private string GetSizeString(int size)
        {
            if (size < 1024)
            {
                return string.Format("{0} Bytes", size.ToString());
            }

            if (size < 1024 * 1024)
            {
                return string.Format("{0} KB", (size / 1024f).ToString("F2"));
            }

            if (size < 1024 * 1024 * 1024)
            {
                return string.Format("{0} MB", (size / 1024f / 1024f).ToString("F2"));
            }

            return string.Format("{0} GB", (size / 1024f / 1024f / 1024f).ToString("F2"));
        }

        private string GetTimeString(int seconds)
        {
            if (seconds < 0)
            {
                return string.Empty;
            }

            if (seconds < 60)
            {
                return GameEntry.Localization.GetString("UI_TEXT_SECOND", seconds.ToString());
            }

            if (seconds < 60 * 60)
            {
                return GameEntry.Localization.GetString("UI_TEXT_MINUTE_SECOND", (seconds / 60).ToString(), (seconds % 60).ToString());
            }

            return GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE", (seconds / 60 / 60).ToString(), (seconds / 60 % 60).ToString());
        }

        private void StartUpdateResources(object userData)
        {
            m_DownloadingPanel = NGUITools.AddChild(m_Config.ParentNode, m_Config.DownloadingTemplate).GetComponent<Downloading>();
            m_DownloadingPanel.name = m_Config.NodeName;
            m_DownloadingPanel.ProgressText = string.Empty;
            m_DownloadingPanel.Progress = 0f;
            m_DownloadingPanel.FadeIn();

            GameEntry.Resource.UpdateResources();
            Log.Info("Start update resources...");
        }

        private void OnResourceCheckComplete(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ResourceCheckCompleteEventArgs ne = e as UnityGameFramework.Runtime.ResourceCheckCompleteEventArgs;
            Log.Info("Check resource OK, '{0}' resources need to update, zipped length is '{1}', unzipped length is '{2}'.", ne.UpdateCount.ToString(), ne.UpdateTotalZipLength.ToString(), ne.UpdateTotalLength.ToString());

            GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
            m_UpdateCount = ne.UpdateCount;
            m_UpdateTotalZipLength = ne.UpdateTotalZipLength;
            if (m_UpdateCount <= 0)
            {
                ProcessUpdateAllComplete();
                return;
            }

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_NETWORK_REACHABLE_VIA_CARRIER_DATA_NETWORK", GetSizeString(m_UpdateTotalZipLength)),
                    ConfirmText = GameEntry.Localization.GetString("UI_TEXT_CONTINUE_DOWNLOAD"),
                    CancelText = GameEntry.Localization.GetString("UI_TEXT_QUIT_GAME"),
                    OnClickConfirm = StartUpdateResources,
                    OnClickCancel = QuitGame,
                });
            }
            else
            {
                StartUpdateResources(null);
            }
        }

        private void OnResourceUpdateStart(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ResourceUpdateStartEventArgs ne = e as UnityGameFramework.Runtime.ResourceUpdateStartEventArgs;
            //Log.Info("Update resource '{0}' start.", ne.Name);

            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.Name)
                {
                    Log.Warning("Update resource '{0}' is invalid.", ne.Name);
                    m_UpdateLengthData[i].Length = 0;
                    return;
                }
            }

            m_UpdateLengthData.Add(new UpdateLengthData(ne.Name));
        }

        private void OnResourceUpdateChanged(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs ne = e as UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs;

            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.Name)
                {
                    m_UpdateLengthData[i].Length = ne.CurrentLength;
                    RefreshProgress();
                    return;
                }
            }

            Log.Warning("Update resource '{0}' is invalid.", ne.Name);
        }

        private void OnResourceUpdateSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs ne = e as UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs;
            //Log.Info("Update resource '{0}' success.", ne.Name);

            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.Name)
                {
                    m_UpdateLengthData[i].Length = ne.ZipLength;
                    m_UpdateSuccessCount++;
                    RefreshProgress();
                    return;
                }
            }

            Log.Warning("Update resource '{0}' is invalid.", ne.Name);
        }

        private void OnResourceUpdateFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs ne = e as UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs;
            if (ne.RetryCount >= ne.TotalRetryCount)
            {
                OnError("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
                return;
            }
            else
            {
                Log.Info("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
            }

            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.Name)
                {
                    m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
                    RefreshProgress();
                    return;
                }
            }

            Log.Warning("Update resource '{0}' is invalid.", ne.Name);
        }

        private void OnResourceUpdateAllComplete(object sender, GameEventArgs e)
        {
            Log.Info("All resources update complete.");
            ProcessUpdateAllComplete();
        }

        private void ProcessUpdateAllComplete()
        {
            GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);

            m_UpdateAllComplete = true;
            if (m_UpdateCount > 0)
            {
                m_DownloadingPanel.FadeOut();
            }
        }

        private class UpdateLengthData
        {
            private readonly string m_Name;

            public UpdateLengthData(string name)
            {
                m_Name = name;
            }

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public int Length
            {
                get;
                set;
            }
        }
    }
}

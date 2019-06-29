using GameFramework;
using GameFramework.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureLaunch : ProcedureBase
    {
        private ProcedureConfig.ProcedureLaunchConfig m_Config = null;

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

            m_Config = GameEntry.ClientConfig.ProcedureConfig.LaunchConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Base.GameVersion = "0.2.0";
            GameEntry.UIBackground.ShowBlank();
            RemoveShuttingDownCamera();
            InitBuildInfo();
            ForceSetLanguage();

            // TODO: 重构使得此调用留在 GameEntry 中。
            GameEntry.RegisterAdditionalDebuggers();
            InitLogWhiteList();
            InitQualitySettings();
            InitAudioSettings();
            LoadDefaultDictionary();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            ChangeState<ProcedureSplash>(procedureOwner);
        }

        private void InitBuildInfo()
        {
            if (m_Config.BuildInfoTextAsset == null)
            {
                Log.Info("BuildInfo can not be found.");
                return;
            }

            if (string.IsNullOrEmpty(m_Config.BuildInfoTextAsset.text))
            {
                Log.Info("BuildInfo is empty.");
                return;
            }

            BuildInfo buildInfo = Utility.Json.ToObject<BuildInfo>(m_Config.BuildInfoTextAsset.text);
            if (buildInfo == null)
            {
                Log.Warning("Parse BuildInfo failure.");
                return;
            }

            GameEntry.BuildInfo = buildInfo;
            GameEntry.Base.InternalApplicationVersion = buildInfo.InternalVersion;
        }

        private void InitLogWhiteList()
        {
            if (m_Config.LogWhiteListTextAsset == null)
            {
                Log.Info("LogWhiteList can not be found.");
                return;
            }

            var logWhiteList = new List<string>();
            string[] rowTexts = Utility.Text.SplitToLines(m_Config.LogWhiteListTextAsset.text);
            for (int i = 0; i < rowTexts.Length; i++)
            {
                if (string.IsNullOrEmpty(rowTexts[i]))
                {
                    continue;
                }

                logWhiteList.Add(rowTexts[i]);
            }

            GameEntry.SetLogWhiteList(logWhiteList.ToArray());
        }

        private void InitQualitySettings()
        {
            int defaultQuality = (int)GameEntry.ClientConfig.GetDefaultQualityLevel();
            int qualityLevel = GameEntry.Setting.GetInt(Constant.Setting.QualityLevel, defaultQuality);
            QualitySettings.SetQualityLevel(qualityLevel, true);
        }

        private void InitAudioSettings()
        {
            GameEntry.Sound.MuteMusic(GameEntry.Setting.GetBool(Constant.Setting.MusicIsMuted, false));
            GameEntry.Sound.SetMusicVolume(GameEntry.Setting.GetFloat(Constant.Setting.MusicVolume, 1f / 3f));
            GameEntry.Sound.MuteSound(GameEntry.Setting.GetBool(Constant.Setting.SoundIsMuted, false));
            GameEntry.Sound.SetSoundVolume(GameEntry.Setting.GetFloat(Constant.Setting.SoundVolume, 1f));
        }

        private void LoadDefaultDictionary()
        {
            if (m_Config.DefaultDictionaryTextAsset == null)
            {
                return;
            }

            string text = m_Config.DefaultDictionaryTextAsset.text;
            GameEntry.Localization.ParseDictionary(text);
        }

        private void RemoveShuttingDownCamera()
        {
            var go = GameObject.Find(Constant.ShuttingDownCameraName);
            if (go == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(go);
        }

        /// <summary>
        /// 强制设置语言为简体中文。
        /// </summary>
        private void ForceSetLanguage()
        {
            GameEntry.Localization.Language = Language.English;
            GameEntry.Setting.SetString(Constant.Setting.Language, GameEntry.Localization.Language.ToString());
            GameEntry.Setting.Save();
        }
    }
}

using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 主角防遮挡组件。
    /// </summary>
    public class AntiOcclusionComponent : MonoBehaviour
    {
        [SerializeField]
        private AntiOcclusionConfig m_Config = null;

        private Dictionary<string, AntiOcclusionConfig.Config> m_OriginalShaderNamesToConfigs = new Dictionary<string, AntiOcclusionConfig.Config>();

        public float BeginAlpha
        {
            get
            {
                return m_Config.BeginAlpha;
            }
        }

        public float TargetAlpha
        {
            get
            {
                return m_Config.TargetAlpha;
            }
        }

        public float TransitionDuration
        {
            get
            {
                return m_Config.TransitionDuration;
            }
        }

        public AntiOcclusionConfig.Config GetConfig(string key)
        {
            AntiOcclusionConfig.Config ret = null;
            m_OriginalShaderNamesToConfigs.TryGetValue(key, out ret);
            return ret;
        }

        public AntiOcclusionConfig.Config GetConfig(Shader originalShader)
        {
            if (originalShader == null)
            {
                return null;
            }

            return GetConfig(originalShader.name);
        }

        #region MonoBehaviour

        private void Awake()
        {
            ReadConfigs();
        }

        private void OnDestroy()
        {
            m_OriginalShaderNamesToConfigs.Clear();
            m_Config = null;
        }

        #endregion MonoBehaviour

        private void ReadConfigs()
        {
            if (m_Config == null)
            {
                Log.Error("Config is invalid.");
                return;
            }

            var configItems = m_Config.GetConfigs();
            if (configItems == null)
            {
                Log.Error("Config is invalid.");
                return;
            }

            for (int i = 0; i < configItems.Count; ++i)
            {
                var configItem = configItems[i];

                if (configItem == null || configItem.OriginalShader == null)
                {
                    Log.Error("Config item at index '{0}' is invalid.", i);
                    return;
                }

                if (m_OriginalShaderNamesToConfigs.ContainsKey(configItem.OriginalShader.name))
                {
                    Log.Warning("Ignoring duplicate original shader '{0}'.", configItem.OriginalShader.name);
                    continue;
                }

                m_OriginalShaderNamesToConfigs.Add(configItem.OriginalShader.name, configItem);
            }
        }
    }
}

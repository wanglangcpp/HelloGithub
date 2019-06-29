using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ClientConfigComponent : MonoBehaviour
    {
        [SerializeField]
        private ClientConfig m_GameConfig = null;

        [SerializeField]
        private ProcedureConfig m_ProcedureConfig = null;

        public ClientConfig.ColorConfig ClientColorConfig
        {
            get
            {
                return m_GameConfig.ClientColorConfig;
            }
        }

        public ProcedureConfig ProcedureConfig
        {
            get
            {
                return m_ProcedureConfig;
            }
        }

        public ClientConfig.BuildAccountConfig ClinetBuildAccountConfig
        {
            get
            {
                return m_GameConfig.ClinetBuildAccountConfig;
            }
        }

        public int GetMaxNearbyPlayerCount()
        {
#if UNITY_EDITOR
            return 20;
#else
            return m_GameConfig.GetMaxNearbyPlayerCount(SystemInfo.deviceModel);
#endif
        }

        public int GetMaxNearbyPlayerModelTypeCount()
        {
#if UNITY_EDITOR
            return 10;
#else
            return m_GameConfig.GetMaxNearbyPlayerModelTypeCount(SystemInfo.deviceModel);
#endif
        }

        public ClientConfig.QualityLevel GetDefaultQualityLevel()
        {
#if UNITY_EDITOR
            return ClientConfig.QualityLevel.High;
#else
            return m_GameConfig.GetDefaultQualityLevel(SystemInfo.deviceModel);
#endif
        }

        public IList<ClientConfig.StringReplacementLabelColorConfig> GetStringReplacementLabelColorConfigs()
        {
            return m_GameConfig.GetStringReplacementLabelColorConfigs();
        }

        public IList<ClientConfig.LuaScriptToPreload> GetLuaScriptsToPreload()
        {
            return m_GameConfig.GetLuaScriptsToPreload();
        }

        public Color GetGenericSkillBadgeSlotColor(int colorEnum)
        {
            return m_GameConfig.GetGenericSkillBadgeSlotColor(colorEnum);
        }

        public IList<string> GetShaderPathsForWarmUp()
        {
            return m_GameConfig.GetShaderPathsForWarmUp();
        }
    }
}

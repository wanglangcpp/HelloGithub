using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ClientConfig : ScriptableObject
    {
        #region Structure Definitions

        [Serializable]
        public enum QualityLevel
        {
            Low = 0,
            Middle,
            High,
        }

        [Serializable]
        public class DeviceModel
        {
            [SerializeField]
            private string m_Comment = null;

            [SerializeField]
            private string m_ModelName = null;

            [SerializeField]
            private QualityLevel m_QualityLevel = QualityLevel.Middle;

            [SerializeField]
            private int m_MaxNearbyPlayerCount = 20;

            [SerializeField]
            private int m_MaxNearbyPlayerModelTypeCount = 10;

            public string Comment
            {
                get
                {
                    return m_Comment;
                }
            }

            public string ModelName
            {
                get
                {
                    return m_ModelName;
                }
            }

            public QualityLevel QualityLevel
            {
                get
                {
                    return m_QualityLevel;
                }
            }

            public int MaxNearbyPlayerCount
            {
                get
                {
                    return m_MaxNearbyPlayerCount;
                }
            }

            public int MaxNearbyPlayerModelTypeCount
            {
                get
                {
                    return m_MaxNearbyPlayerModelTypeCount;
                }
            }
        }

        [Serializable]
        public class ColorConfig
        {
            [SerializeField]
            private Color m_PlayerNameColor = Color.black;

            [SerializeField]
            private Color m_ActivityColor = Color.blue;

            public Color PlayerNameColor
            {
                get
                {
                    return m_PlayerNameColor;
                }
            }

            public Color ActivityColor
            {
                get
                {
                    return m_ActivityColor;
                }
            }
        }

        [Serializable]
        public class StringReplacementLabelColorConfig
        {
            [SerializeField]
            private string m_Key = null;

            [SerializeField]
            private Color m_Color = Color.white;

            public string Key
            {
                get
                {
                    return m_Key;
                }
            }

            public Color Color
            {
                get
                {
                    return m_Color;
                }
            }
        }

        [Serializable]
        public class LuaScriptToPreload
        {
            [SerializeField]
            private string m_Name = null;

            [SerializeField]
            private LuaScriptCategory m_Category = LuaScriptCategory.Base;

            public string Name { get { return m_Name; } }

            public LuaScriptCategory Category { get { return m_Category; } }
        }

        [Serializable]
        public class BuildAccountConfig
        {
            [SerializeField]
            private int m_AddMoney = 0;

            [SerializeField]
            private int m_AddCoin = 0;

            [SerializeField]
            private int m_MyMaxLevel = 1;

            [SerializeField]
            private int m_MaxStar = 1;

            [SerializeField]
            private int m_MaxUseHeroLevel = 1;

            [SerializeField]
            private int[] m_AddHeroIDList = { };

            [SerializeField]
            private int[] m_AddItemIDList = { };

            public int AddMoney { get { return m_AddMoney; } }

            public int AddCoin { get { return m_AddCoin; } }

            public int MyMaxLevel { get { return m_MyMaxLevel; } }

            public int MaxStar { get { return m_MaxStar; } }

            public int MaxUseHeroLevel { get { return m_MaxUseHeroLevel; } }

            public int[] AddHeroIDList { get { return m_AddHeroIDList; } }

            public int[] AddItemIDList { get { return m_AddItemIDList; } }
        }

        #endregion Structure Definitions

        #region Member Fields
        [SerializeField]
        private BuildAccountConfig m_BuildAccount = null;

        [SerializeField]
        private List<DeviceModel> m_DeviceModels = null;

        [SerializeField]
        private ColorConfig m_ColorConfig = null;

        [SerializeField]
        private List<StringReplacementLabelColorConfig> m_StringReplacementLabelColorConfigs = null;

        [SerializeField]
        private LuaScriptToPreload[] m_LuaScriptsToPreload = null;

        [SerializeField]
        private Color[] m_GenericSkillBadgeSlotColors = null;

        [SerializeField]
        private string[] m_ShaderPathsForWarmUp = null;


        #endregion Member Fields

        #region Utility Functions

        public List<DeviceModel> DeviceModels
        {
            get
            {
                return m_DeviceModels;
            }
        }

        public ColorConfig ClientColorConfig
        {
            get
            {
                return m_ColorConfig;
            }
        }

        public BuildAccountConfig ClinetBuildAccountConfig
        {
            get
            {
                return m_BuildAccount;
            }
        }

        public IList<StringReplacementLabelColorConfig> GetStringReplacementLabelColorConfigs()
        {
            return m_StringReplacementLabelColorConfigs;
        }

        public QualityLevel GetDefaultQualityLevel(string ModelName)
        {
            if (null != m_DeviceModels)
            {
                for (int i = 0; i < m_DeviceModels.Count; i++)
                {
                    if (ModelName == m_DeviceModels[i].ModelName)
                    {
                        return m_DeviceModels[i].QualityLevel;
                    }
                }
            }

            return QualityLevel.Middle;
        }

        public int GetMaxNearbyPlayerCount(string ModelName)
        {
            if (null != m_DeviceModels)
            {
                for (int i = 0; i < m_DeviceModels.Count; i++)
                {
                    if (ModelName == m_DeviceModels[i].ModelName)
                    {
                        return m_DeviceModels[i].MaxNearbyPlayerCount;
                    }
                }
            }

            return 20;
        }

        public int GetMaxNearbyPlayerModelTypeCount(string ModelName)
        {
            if (null != m_DeviceModels)
            {
                for (int i = 0; i < m_DeviceModels.Count; i++)
                {
                    if (ModelName == m_DeviceModels[i].ModelName)
                    {
                        return m_DeviceModels[i].MaxNearbyPlayerModelTypeCount;
                    }
                }
            }

            return 10;
        }

        public IList<LuaScriptToPreload> GetLuaScriptsToPreload()
        {
            return m_LuaScriptsToPreload;
        }

        public Color GetGenericSkillBadgeSlotColor(int colorEnum)
        {
            return m_GenericSkillBadgeSlotColors[colorEnum];
        }

        public IList<string> GetShaderPathsForWarmUp()
        {
            return m_ShaderPathsForWarmUp;
        }

        #endregion Utility Functions
    }
}

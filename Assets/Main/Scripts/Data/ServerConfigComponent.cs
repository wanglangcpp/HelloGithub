using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ServerConfigComponent : MonoBehaviour
    {
        private readonly Dictionary<string, string> m_Configs = new Dictionary<string, string>();

        public void UpdateData(List<PBServerConfigInfo> configInfos)
        {
            m_Configs.Clear();
            for (int i = 0; i < configInfos.Count; i++)
            {
                m_Configs.Add(configInfos[i].Key, configInfos[i].Value);
            }
        }

        public string GetString(string name, string defaultVal = "")
        {
            string valStr = null;
            if (m_Configs.TryGetValue(name, out valStr))
            {
                return valStr;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                Log.Info("Can not find string config '{0}'.", name);
            }

            return defaultVal;
        }

        public bool GetBool(string name, bool defaultVal = false)
        {
            bool valBool = false;

            if (string.IsNullOrEmpty(GetString(name)))
            {
                return defaultVal;
            }

            if (bool.TryParse(GetString(name), out valBool))
            {
                return valBool;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                Log.Info("Can not find bool config '{0}'.", name);
            }

            return defaultVal;
        }

        public int GetInt(string name, int defaultVal = 0)
        {
            int valInt = 0;

            if (string.IsNullOrEmpty(GetString(name)))
            {
                return defaultVal;
            }

            if (int.TryParse(GetString(name), out valInt))
            {
                return valInt;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                Log.Info("Can not find int config '{0}'.", name);
            }

            return defaultVal;
        }

        public float GetFloat(string name, float defaultVal = 0f)
        {
            float valFloat = 0f;

            if (string.IsNullOrEmpty(GetString(name)))
            {
                return defaultVal;
            }

            if (float.TryParse(GetString(name), out valFloat))
            {
                return valFloat;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                Log.Info("Can not find float config '{0}'.", name);
            }

            return defaultVal;
        }
    }
}

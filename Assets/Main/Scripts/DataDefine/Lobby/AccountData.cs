using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class AccountData
    {
        [SerializeField]
        private ServerData m_ServerData = null;

        public bool IsAvailable
        {
            get
            {
                return (!string.IsNullOrEmpty(AccountName)) && m_ServerData != null;
            }
        }

        public string AccountName
        {
            get
            {
                return GameEntry.Setting.GetString(Constant.Setting.AccountName);
            }
            set
            {
                GameEntry.Setting.SetString(Constant.Setting.AccountName, value);
                GameEntry.Setting.Save();
            }
        }

        public string LoginKey
        {
            get
            {
                return GameEntry.Setting.GetString(Constant.Setting.LoginKey);
            }
            set
            {
                GameEntry.Setting.SetString(Constant.Setting.LoginKey, value);
                GameEntry.Setting.Save();
            }
        }

        public int LastServerId
        {
            get
            {
                return GameEntry.Setting.GetInt(Constant.Setting.LastServerId);
            }
            set
            {
                GameEntry.Setting.SetInt(Constant.Setting.LastServerId, value);
                GameEntry.Setting.Save();
            }
        }

        public ServerData ServerData
        {
            get
            {
                return m_ServerData;
            }
            set
            {
                m_ServerData = value;
            }
        }
    }
}

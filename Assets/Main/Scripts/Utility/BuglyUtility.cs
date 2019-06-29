using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 腾讯 Bugly 库的工具类。
    /// </summary>
    public static class BuglyUtility
    {
        private static string AccountName
        {
            get
            {
                if (!GameEntry.IsAvailable || !GameEntry.Data.Account.IsAvailable)
                {
                    return string.Empty;
                }

                return GameEntry.Data.Account.AccountName;
            }
        }

        private static int ServerId
        {
            get
            {
                if (!GameEntry.IsAvailable || !GameEntry.Data.Account.IsAvailable)
                {
                    return -1;
                }

                return GameEntry.Data.Account.ServerData.Id;
            }
        }

        public static int PlayerId
        {
            get
            {
                if (!GameEntry.IsAvailable)
                {
                    return -1;
                }

                return GameEntry.Data.Player.Id;
            }
        }

        public static string UserId
        {
            get
            {
                return string.Format("{0},{1},{2}", AccountName ?? string.Empty, ServerId.ToString(), PlayerId.ToString());
            }
        }

        public static Dictionary<string, string> PrepareExtraData()
        {
            var ret = new Dictionary<string, string>();

            if (SceneManager.GetActiveScene().buildIndex == 0 || !GameEntry.IsAvailable)
            {
                return ret;
            }

            ret.Add("Procedure", GameEntry.Procedure.CurrentProcedure.GetType().Name);

            var lobbyChannel = GameEntry.Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName);

            if (lobbyChannel != null && lobbyChannel.Connected)
            {
                ret.Add("Lobby", string.Format("Local:'{0}:{1}',Remote:'{2}:{3}'", lobbyChannel.LocalIPAddress.ToString(), lobbyChannel.LocalPort.ToString(), lobbyChannel.RemoteIPAddress.ToString(), lobbyChannel.RemotePort.ToString()));
            }

            var roomChannel = GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);

            if (roomChannel != null && roomChannel.Connected)
            {
                ret.Add("Room", string.Format("Local:'{0}:{1}',Remote:'{2}:{3}'", roomChannel.LocalIPAddress.ToString(), roomChannel.LocalPort.ToString(), roomChannel.RemoteIPAddress.ToString(), roomChannel.RemotePort.ToString()));
            }

            var uiGroups = GameEntry.UI.GetAllUIGroups();
            var uiInfoStr = new StringBuilder();

            for (int i = 0; i < uiGroups.Length; ++i)
            {
                if (uiGroups[i] == null)
                {
                    continue;
                }

                var uiForm = (uiGroups[i].CurrentUIForm as UIForm);
                if (uiForm == null)
                {
                    continue;
                }

                var nguiForm = uiForm.Logic as NGUIForm;

                if (i > 0)
                {
                    uiInfoStr.Append(',');
                }

                uiInfoStr.AppendFormat("'{0}':'{1}'", uiGroups[i].Name, nguiForm.Name);
            }

            if (uiInfoStr.Length > 0)
            {
                ret.Add("UI", uiInfoStr.ToString());
            }

            return ret;
        }
    }
}

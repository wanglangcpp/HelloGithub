using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class DataTablePusher
    {
        private const string ConfigFilePath = "Main/Editor/DataTablePusherConfig.xml";

        private enum ServerType
        {
            Lobby,
            Room,
        }

        private class Config
        {
            public IDictionary<string, Server> Servers = new Dictionary<string, Server>();
            public IDictionary<string, DataTable> DataTables = new Dictionary<string, DataTable>();
            public string NotifyUrl;

            public class Server
            {
                public string Name;
                public ServerType Type;
                public string Host;
                public int Port;
                public string User;
                public string Passwd;
                public string TargetPath;
            }

            public class DataTable
            {
                public string Path;
                public bool NeededOnLobbyServers;
                public bool NeededOnRoomServers;
            }
        }

        private Config m_Config;

        public DataTablePusher()
        {
            ReadConfig();
        }

        public void Run()
        {
            Debug.Log("[DataTablePusher Run] Running...");
            var assets = GetSelectedAssets();

            foreach (var kv in m_Config.Servers)
            {
                var serverConfig = kv.Value;
                List<string> assetLocalPathsToPush = CollectLocalPathsToPush(assets, serverConfig);
                PushToServer(assetLocalPathsToPush, serverConfig);
            }

            NotifyServersToReloadDataTables();
            Debug.Log("[DataTablePusher Run] Finished.");
        }

        private List<string> CollectLocalPathsToPush(IEnumerable<TextAsset> assets, Config.Server serverConfig)
        {
            var assetLocalPathsToPush = new List<string>();
            foreach (var asset in assets)
            {
                var basePath = GetBasePath(asset);
                var localPath = Utility.Path.GetCombinePath(Application.dataPath, basePath);
                var dtConfig = m_Config.DataTables[basePath];
                if (dtConfig.NeededOnLobbyServers && serverConfig.Type == ServerType.Lobby ||
                    dtConfig.NeededOnRoomServers && serverConfig.Type == ServerType.Room)
                {
                    assetLocalPathsToPush.Add(localPath);
                }
            }

            return assetLocalPathsToPush;
        }

        private void NotifyServersToReloadDataTables()
        {
            Debug.LogFormat("Notifying servers to reload data tables.");
            var webRequest = (HttpWebRequest)WebRequest.Create(m_Config.NotifyUrl);
            webRequest.Timeout = 120000; // milliseconds
            webRequest.Method = "GET";

            var response = webRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            int contentLength = (int)response.ContentLength;
            var responseBuffer = new byte[contentLength];
            responseStream.Read(responseBuffer, 0, contentLength);
            Debug.LogFormat("response = {0}.", System.Text.Encoding.UTF8.GetString(responseBuffer));
        }

        private void PushToServer(IList<string> localPaths, Config.Server serverConfig)
        {
            if (localPaths == null || localPaths.Count <= 0)
            {
                Debug.LogFormat("No data table to push to server '{0}'.", serverConfig.Name);
                return;
            }

            var sftp = new Sftp(serverConfig.Host, serverConfig.User, serverConfig.Passwd);

            try
            {
                sftp.Connect(serverConfig.Port);
                for (int i = 0; i < localPaths.Count; ++i)
                {
                    Debug.LogFormat("Putting '{0}' to remote path '{1}' to server '{2}'.", localPaths[i], serverConfig.TargetPath, serverConfig.Name);
                    var fileName = Path.GetFileName(localPaths[i]);
                    sftp.Put(localPaths[i], Path.Combine(serverConfig.TargetPath, fileName));
                }
            }
            catch (SftpException ex)
            {
                Debug.LogWarningFormat("{0}: {1}\n{2}", ex.id.ToString(), ex.message, ex.StackTrace);
            }
            finally
            {
                sftp.Close();
            }
        }

        private IEnumerable<TextAsset> GetSelectedAssets()
        {
            return Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets)
                .Where(o => m_Config.DataTables.ContainsKey(GetBasePath(o)))
                .ToList()
                .ConvertAll(o => o as TextAsset);
        }

        private static string GetBasePath(UnityEngine.Object asset)
        {
            return Regex.Replace(AssetDatabase.GetAssetPath(asset), @"^Assets/", string.Empty);
        }

        private void ReadConfig()
        {
            Debug.Log("[DataTablePusher ReadConfig] Start reading config.");
            var doc = new XmlDocument();
            doc.Load(Path.Combine(Application.dataPath, ConfigFilePath));

            var root = doc.SelectSingleNode("DataTablePusher") as XmlElement;
            var servers = root.SelectNodes("Servers/Server");
            var dataTables = root.SelectNodes("DataTables/DataTable");

            m_Config = new Config();

            foreach (XmlElement server in servers)
            {
                var key = server.GetAttribute("Name");
                m_Config.Servers.Add(key, new Config.Server
                {
                    Name = key,
                    Type = (ServerType)(System.Enum.Parse(typeof(ServerType), server.GetAttribute("Type"))),
                    Host = server.GetAttribute("Host"),
                    Port = int.Parse(server.GetAttribute("Port")),
                    User = server.GetAttribute("User"),
                    Passwd = server.GetAttribute("Passwd"),
                    TargetPath = server.GetAttribute("TargetPath"),
                });
            }

            foreach (XmlElement dt in dataTables)
            {
                var key = dt.GetAttribute("Path");
                bool neededOnLobby;
                bool neededOnRoom;
                m_Config.DataTables.Add(key, new Config.DataTable
                {
                    Path = key,
                    NeededOnLobbyServers = dt.HasAttribute("NeededOnLobbyServers") && bool.TryParse(dt.GetAttribute("NeededOnLobbyServers"), out neededOnLobby) ? neededOnLobby : false,
                    NeededOnRoomServers = dt.HasAttribute("NeededOnRoomServers") && bool.TryParse(dt.GetAttribute("NeededOnRoomServers"), out neededOnRoom) ? neededOnRoom : false,
                });
            }

            m_Config.NotifyUrl = (root.SelectSingleNode("NotifyServer") as XmlElement).GetAttribute("Url");
        }

        [MenuItem("Assets/Push Data Tables")]
        public static void Execute()
        {
            new DataTablePusher().Run();
        }
    }
}

using System.IO;
using UnityEditor;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// Windows 版本打包类。
    /// </summary>
    public static class BuildPlayerWindows
    {
        private const string TargetPath = "../GameClient_Windows";
        private const string ExeFileName = "genesis.exe";

        [MenuItem("Game Framework/Build Windows Player/Build Development Version", false, 1020)]
        public static void Run()
        {
            if (Directory.Exists(TargetPath))
            {
                Directory.Delete(TargetPath, true);
            }

            DirectoryInfo dir = new DirectoryInfo(TargetPath);
            var scenes = new string[] { "Assets/Launch.unity" };
            string errorMessage = BuildPipeline.BuildPlayer(scenes, Path.Combine(dir.FullName, ExeFileName), BuildTarget.StandaloneWindows, BuildOptions.Development | BuildOptions.ConnectWithProfiler);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return;
            }
        }
    }
}

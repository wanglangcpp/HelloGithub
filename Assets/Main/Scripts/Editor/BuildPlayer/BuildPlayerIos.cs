using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// iOS 版本打包类，目前同时用于自动打包脚本和菜单项。
    /// </summary>
    public static class BuildPlayerIos
    {
        private const string TargetPath = "../GameClient_Xcode";

        [MenuItem("Game Framework/Build iOS Player/Build Development Version", false, 1000)]
        public static void BuildDev()
        {
            BuildInternal(BuildOptions.Development);
        }

        [MenuItem("Game Framework/Build iOS Player/Build Release Version", false, 1001)]
        public static void BuildRelease()
        {
            BuildInternal(BuildOptions.None);
        }

        private static void BuildInternal(BuildOptions buildOptions)
        {
            if (Directory.Exists(TargetPath))
            {
                Directory.Delete(TargetPath, true);
            }

            DirectoryInfo dir = new DirectoryInfo(TargetPath);
            var scenes = new string[] { "Assets/Launch.unity" };
            string errorMessage = BuildPipeline.BuildPlayer(scenes, dir.FullName, BuildTarget.iOS, buildOptions);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return;
            }

            PostProcessXcodeProj();
        }

        private static void PostProcessXcodeProj()
        {
            // Make main.mm use CustomeAppController instead of the default UnityAppController as the AppDelegate
            return;
            var mainFilePath = Path.Combine(TargetPath, "Classes/main.mm");
            var lines = File.ReadAllLines(mainFilePath);
            var regexPattern = "\"UnityAppController\"";
            var replacement = "\"CustomAppController\"";

            for (int i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, regexPattern))
                {
                    lines[i] = Regex.Replace(line, regexPattern, replacement);
                }
            }

            File.WriteAllLines(mainFilePath, lines);
        }
    }
}

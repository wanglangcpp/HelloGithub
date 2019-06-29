using System.IO;
using UnityEditor;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// Android 版本打包类，目前同时用于自动打包脚本和菜单项。
    /// </summary>
    public static class BuildPlayerAndroid
    {
        private const string TargetPath = "../GameClient_Android";
        private const string ToCopyBaseDir = "Android/ToCopy";

        [MenuItem("Game Framework/Build Android Player/Build Development Version", false, 1010)]
        public static void BuildDev()
        {
            BuildInternal(BuildOptions.Development);
        }

        [MenuItem("Game Framework/Build Android Player/Build Development Version", false, 1011)]
        public static void BuildRelease()
        {
            BuildInternal(BuildOptions.None);
        }

        private static void BuildInternal(BuildOptions buildOption)
        {
            if (Directory.Exists(TargetPath))
            {
                Directory.Delete(TargetPath, true);
            }

            DirectoryInfo dir = new DirectoryInfo(TargetPath);
            var scenes = new string[] { "Assets/Launch.unity" };
            string errorMessage = BuildPipeline.BuildPlayer(scenes, dir.FullName, BuildTarget.Android, buildOption | BuildOptions.AcceptExternalModificationsToPlayer);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return;
            }

            PostProcessAndroidProj();
        }

        private static void ModifyAndroidManifest()
        {
            var amModifier = new AndroidManifestModifier(Utility.Path.GetCombinePath(TargetPath, PlayerSettings.productName, "AndroidManifest.xml"));
            amModifier.Load();
            amModifier.RemoveApplicationAttribute("android:debuggable");
            amModifier.RemoveApplicationAttribute("android:isGame");
            amModifier.RemoveApplicationAttribute("android:banner");
            amModifier.UpdateMainActivityAttribute("android:name", "UnityPlayerActivity");
            amModifier.AddXmlFragmentUnderApplication(
                "<activity android:name=\"cn.jpush.android.ui.PushActivity\" android:configChanges=\"orientation|keyboardHidden\" android:theme=\"@android:style/Theme.NoTitleBar\" android:exported =\"false\">" +
                "  <intent-filter>" +
                "    <action android:name=\"cn.jpush.android.ui.PushActivity\" />" +
                "    <category android:name=\"android.intent.category.DEFAULT\" />" +
                "    <category android:name=\"com.antinc.genesis\" />" +
                "  </intent-filter>" +
                "</activity>");
            amModifier.AddXmlFragmentUnderApplication("<service android:name=\"cn.jpush.android.service.DownloadService\" android:enabled=\"true\" android:exported=\"false\"></service>");
            amModifier.AddXmlFragmentUnderApplication(
                "<service android:name=\"cn.jpush.android.service.PushService\" android:enabled=\"true\" android:exported=\"false\">" +
                "  <intent-filter>" +
                "    <action android:name=\"cn.jpush.android.intent.REGISTER\" />" +
                "    <action android:name=\"cn.jpush.android.intent.REPORT\" />" +
                "    <action android:name=\"cn.jpush.android.intent.PushService\" />" +
                "    <action android:name=\"cn.jpush.android.intent.PUSH_TIME\" />" +
                "  </intent-filter>" +
                "</service>");
            amModifier.AddXmlFragmentUnderApplication(
                "<receiver android:name=\"cn.jpush.android.service.PushReceiver\" android:enabled=\"true\" android:exported=\"false\">" +
                "  <intent-filter android:priority=\"1000\">" +
                "    <action android:name=\"cn.jpush.android.intent.NOTIFICATION_RECEIVED_PROXY\" />" +
                "    <category android:name=\"com.antinc.genesis\" />" +
                "  </intent-filter>" +
                "  <intent-filter>" +
                "    <action android:name=\"android.intent.action.USER_PRESENT\" />" +
                "    <action android:name=\"android.net.conn.CONNECTIVITY_CHANGE\" />" +
                "  </intent-filter>" +
                "  <!--Optional-->" +
                "  <intent-filter>" +
                "    <action android:name=\"android.intent.action.PACKAGE_ADDED\" />" +
                "    <action android:name=\"android.intent.action.PACKAGE_REMOVED\" />" +
                "    <data android:scheme=\"package\"/>" +
                "  </intent-filter>" +
                "</receiver>");
            amModifier.AddXmlFragmentUnderApplication("<receiver android:name=\"cn.jpush.android.service.AlarmReceiver\" android:exported=\"false\"/>");
            amModifier.AddXmlFragmentUnderApplication("<meta-data android:name=\"JPUSH_CHANNEL\" android:value=\"developer-default\"/>");
            amModifier.AddXmlFragmentUnderApplication("<meta-data android:name=\"JPUSH_APPKEY\" android:value=\"10ff5a719faa7d3c70ec9d34\"/>");
            amModifier.AddXmlFragmentUnderRoot("<permission android:name=\"com.antinc.genesis.permission.JPUSH_MESSAGE\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"com.antinc.genesis.permission.JPUSH_MESSAGE\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.RECEIVE_USER_PRESENT\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.WAKE_LOCK\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.WRITE_EXTERNAL_STORAGE\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.READ_EXTERNAL_STORAGE\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.WRITE_SETTINGS\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.VIBRATE\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.MOUNT_UNMOUNT_FILESYSTEMS\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.ACCESS_WIFI_STATE\" />");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.SYSTEM_ALERT_WINDOW\"/>");
            amModifier.AddXmlFragmentUnderRoot("<uses-permission android:name=\"android.permission.READ_LOGS\"/>");
            amModifier.SetSdkVersion(minVersion: 14, targetVersion: 23);
            amModifier.Save();
        }

        private static void PostProcessAndroidProj()
        {
            ModifyAndroidManifest();
            CopyFiles();
        }

        private static void CopyFiles()
        {
            DirectoryCopy(ToCopyBaseDir, Utility.Path.GetCombinePath(TargetPath, PlayerSettings.productName));
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Ref: https://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // Copy subdirectories and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }
    }
}

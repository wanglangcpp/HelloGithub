using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public static class FilesChecker
    {
        private static string[] CheckPath = new string[] { Path.Combine(Application.dataPath, "Main"), Path.Combine(Application.dataPath, "NGUI") };
        private static HashSet<string> IgnorePath = new HashSet<string>() { Path.Combine(Application.dataPath, "Main/Native") , Path.Combine(Application.dataPath, "Main/Publisher") };
        private static HashSet<string> IgnoreFileName = new HashSet<string>() { "readme.txt", "lightingdata.asset" , "navmesh.asset" , "lightmapsnapshot.asset" };

        [MenuItem("Game Framework/Check Files According Rules/Check All", priority = 3010)]
        public static void CheckFiles()
        {
            Dictionary<string, List<string>> fileChecked = new Dictionary<string, List<string>>();
            Dictionary<string, string> fileCacheMap = new Dictionary<string, string>();
            List<string> errorMetaFile = new List<string>();

            foreach (var path in CheckPath)
            {
                var files = Directory.GetFiles(path, @"*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    var filePath = Path.GetDirectoryName(file);

                    if (IgnorePath.Contains(filePath))
                        continue;

                    var fileName = Path.GetFileNameWithoutExtension(file);

                    if (fileName.Equals(string.Empty))
                        continue;

                    fileName = Path.GetFileName(file);

                    if (IgnoreFileName.Contains(fileName.ToLower()))
                        continue;

                    if (fileName.Equals(string.Empty))
                        continue;

                    var extention = Path.GetExtension(file);
                    if (extention.Equals(".meta"))
                    {
                        var contents = File.ReadAllText(file);
                        if (contents.Contains("<<<<") || contents.Contains(">>>>"))
                            errorMetaFile.Add(file);
                    }
                    else
                    {
                        if (fileCacheMap.ContainsKey(fileName.ToLower()))
                        {
                            if (fileChecked.ContainsKey(fileName.ToLower()))
                            {
                                fileChecked[fileName.ToLower()].Add(file);
                            }
                            else
                            {
                                fileChecked.Add(fileName.ToLower(), new List<string>());
                                fileChecked[fileName.ToLower()].Add(fileCacheMap[fileName.ToLower()]);
                                fileChecked[fileName.ToLower()].Add(file);
                            }
                        }
                        else
                        {
                            fileCacheMap[fileName.ToLower()] = file;
                        }

                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("".PadLeft(120, '*'));
            sb.AppendLine("Checked path:");
            sb.AppendLine();
            foreach (var p in CheckPath)
                sb.AppendLine(p);

            sb.AppendLine("".PadLeft(120, '*'));
            sb.AppendLine("Ignored Path:");
            sb.AppendLine();
            foreach (var ip in IgnorePath)
                sb.AppendLine(ip);

            sb.AppendLine("".PadLeft(120, '*'));
            sb.AppendLine("Ignored files:");
            sb.AppendLine();
            foreach (var ignorefile in IgnoreFileName)
                sb.AppendLine(ignorefile);

            sb.AppendLine("".PadLeft(120, '*'));
            sb.AppendLine("Duplicate file names...");
            sb.AppendLine();
            foreach (var item in fileChecked)
            {
                sb.AppendLine("".PadLeft(120, '='));
                item.Value.ForEach(
                    (s) =>
                    {
                        sb.AppendLine(s);
                    }
                    );
            }

            sb.AppendLine();
            sb.AppendLine("".PadLeft(120, '*'));

            sb.AppendLine("Error meta file...");
            sb.AppendLine();
            errorMetaFile.ForEach(
                (s) =>
                {
                    sb.AppendLine(s);
                }
                );

            var destinationFilePath = Path.Combine(Application.temporaryCachePath, "FileCheckResult.txt");
            File.WriteAllText(destinationFilePath, sb.ToString().Replace(@"\", "/"));
            System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);

            Debug.Log("<color=green>Destination file is here: " + destinationFilePath + "</color>");
        }

    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public static class SimpleAssetNameChecker
    {
        private static readonly string[] ExtWhiteList = new string[]
        {
            ".cs",
            ".dll",
            ".xml",
            ".shader",
            ".exr",
        };

        [MenuItem("Assets/Check Asset Names")]
        public static void CheckAssetNames()
        {
            var objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets).ToList();

            var namesToNotice = new List<string>();

            int count = 0;
            foreach (var o in objects)
            {
                CheckAssetName(o, namesToNotice);
                ++count;
            }

            var logSb = new StringBuilder();
            logSb.AppendFormat("{0} assets have been checked. {1} names should be noticed.\n\n", count, namesToNotice.Count);

            namesToNotice.Sort();
            foreach (var path in namesToNotice)
            {
                logSb.AppendLine(path);
            }

            if (namesToNotice.Count <= 0)
            {
                Debug.Log(logSb.ToString());
            }
            else
            {
                Debug.LogWarning(logSb.ToString());
            }
        }

        private static void CheckAssetName(Object o, List<string> namesToNotice)
        {
            var path = AssetDatabase.GetAssetPath(o);
            var fileNameWithNoExt = Path.GetFileNameWithoutExtension(path);
            var extName = Path.GetExtension(path);

            if (ExtWhiteList.Contains(extName.ToLower()))
            {
                return;
            }

            if (extName.ToLower() == ".fbx")
            {
                fileNameWithNoExt = fileNameWithNoExt.Replace("@", "");
            }

            if (Regex.IsMatch(fileNameWithNoExt, @"^[A-Za-z\d]+$") || Regex.IsMatch(fileNameWithNoExt, @"^[a-z\d_]+$"))
            {
                return;
            }

            namesToNotice.Add(path);
        }
    }
}

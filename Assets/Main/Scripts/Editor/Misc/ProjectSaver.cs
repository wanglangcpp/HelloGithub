//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// 存储整个工程相关的实用函数。
    /// </summary>
    public static class ProjectSaver
    {
        private static HashSet<string> s_AssetPathsToReimport = new HashSet<string>();

        /// <summary>
        /// 存储可序列化的资源。
        /// </summary>
        /// <remarks>等同于执行 Unity 菜单 File/Save Project。</remarks>
        [MenuItem("Game Framework/Save Assets &s", false, 0)]
        public static void SaveAssets()
        {
            EditorApplication.SaveAssets();
            ReimportAssets();
            Debug.Log("You have saved the serializable assets in the project.");
        }

        /// <summary>
        /// 增加需要强制重新导入的资源路径。
        /// </summary>
        /// <param name="assetPath">资源路径。</param>
        public static void AddAssetPathToReimport(string assetPath)
        {
            s_AssetPathsToReimport.Add(assetPath);
        }

        private static void ReimportAssets()
        {
            foreach (var assetPath in s_AssetPathsToReimport)
            {
                Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                if (asset == null)
                {
                    continue;
                }

                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }

            s_AssetPathsToReimport.Clear();
        }
    }
}

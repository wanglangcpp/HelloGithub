//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// <see cref="UnityEngine.ScriptableObject" /> 创建器相关的实用函数。
    /// </summary>
    public static class ScriptableObjectCreator
    {
        /// <summary>
        /// 创建 <see cref="UnityEngine.ScriptableObject" /> 的 Asset。
        /// </summary>
        /// <typeparam name="T">ScriptableObject 的具体类型。</typeparam>
        public static void CreateAsset<T>() where T : ScriptableObject
        {
            CreateAsset(typeof(T));
        }

        /// <summary>
        /// 创建 <see cref="UnityEngine.ScriptableObject" /> 的 Asset。
        /// </summary>
        /// <param name="type">ScriptableObject 的具体类型。</param>
        public static void CreateAsset(Type type)
        {
            CreateAsset(type, "New " + type.Name);
        }

        /// <summary>
        /// 创建 <see cref="UnityEngine.ScriptableObject" /> 的 Asset。
        /// </summary>
        /// <typeparam name="T">ScriptableObject 的具体类型。</typeparam>
        /// <param name="name">要创建的名称。</param>
        public static void CreateAsset<T>(string name) where T : ScriptableObject
        {
            CreateAsset(typeof(T), name);
        }

        /// <summary>
        /// 创建 <see cref="UnityEngine.ScriptableObject" /> 的 Asset。
        /// </summary>
        /// <param name="type">ScriptableObject 的具体类型。</param>
        /// <param name="name">要创建的名称。</param>
        public static void CreateAsset(Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            if (asset == null)
            {
                throw new GameFrameworkException("Create scriptable object instance failure.");
            }

            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(assetPath))
            {
                assetPath = "Assets";
            }
            else if (!string.IsNullOrEmpty(Path.GetExtension(assetPath)))
            {
                assetPath = assetPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), string.Empty);
            }

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", assetPath, name));
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}

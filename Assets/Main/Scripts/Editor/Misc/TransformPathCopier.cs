//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// Transform 路径相关的实用函数。
    /// </summary>
    public static class TransformPathCopier
    {
        /// <summary>
        /// 复制 Transform 路径的实用函数。
        /// </summary>
        [MenuItem("Game Framework/Copy Transform Path %&C", false, 1)]
        public static void CopyTransformPath()
        {
            IList<Transform> selectedTransforms = Selection.GetFiltered(typeof(Transform), SelectionMode.TopLevel).ToList().ConvertAll(x => x as Transform);
            if (selectedTransforms.Count != 1)
            {
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();

            bool last = true;
            for (Transform transform = selectedTransforms[0]; transform != null; transform = transform.parent)
            {
                if (last)
                {
                    stringBuilder.Append(transform.name);
                    last = false;
                    continue;
                }

                stringBuilder.Insert(0, '/');
                stringBuilder.Insert(0, transform.name);
            }

            string text = stringBuilder.ToString();
            EditorGUIUtility.systemCopyBuffer = text;
            Debug.Log(string.Format("Copy '{0}' to clipboard.", text));
        }

        /// <summary>
        /// 检查是否可复制 Transform 路径。
        /// </summary>
        /// <returns>是否可复制 Transform 路径。</returns>
        [MenuItem("Game Framework/Copy Transform Path %&C", true, 1)]
        private static bool Validate()
        {
            IList<Transform> selectedTransforms = Selection.GetFiltered(typeof(Transform), SelectionMode.TopLevel).ToList().ConvertAll(x => x as Transform);
            return selectedTransforms.Count == 1;
        }
    }
}

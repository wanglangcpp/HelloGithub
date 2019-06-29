//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// 统计 Game Object 数量相关的实用函数。
    /// </summary>
    public static class GameObjectCounter
    {
        /// <summary>
        /// 统计 Game Object 数量。
        /// </summary>
        [MenuItem("Game Framework/Count Game Objects %&G", false, 2)]
        public static void CountGameObjects()
        {
            Debug.Log(string.Format("There are {0} active game objects in the current scene.", Object.FindObjectsOfType<Transform>().Length));
        }
    }
}

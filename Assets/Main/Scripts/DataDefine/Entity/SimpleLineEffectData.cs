using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 简单线形特效数据。
    /// </summary>
    [Serializable]
    public class SimpleLineEffectData : EffectData
    {
        [SerializeField]
        private int m_EndingEntityId = 0;

        [SerializeField]
        private string m_EndingTransformPath = string.Empty;

        /// <summary>
        /// 连线终点实体编号。
        /// </summary>
        public int EndingEntityId
        {
            get { return m_EndingEntityId; }
            set { m_EndingEntityId = value; }
        }

        /// <summary>
        /// 连线终点 <see cref="UnityEngine.Transform"/> 路径。
        /// </summary>
        public string EndingTransformPath
        {
            get { return m_EndingTransformPath; }
            set { m_EndingTransformPath = value; }
        }

        /// <summary>
        /// 构造器。
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="targetParentPath">继承自父类，会用作连线的起点。</param>
        /// <param name="resourceName"></param>
        /// <param name="ownerId"></param>
        public SimpleLineEffectData(int entityId, string targetParentPath, string resourceName, int ownerId)
            : base(entityId, targetParentPath, resourceName, ownerId)
        {

        }
    }
}

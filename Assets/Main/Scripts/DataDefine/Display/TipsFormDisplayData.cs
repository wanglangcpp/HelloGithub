using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="TipsForm"/> 显示数据。
    /// </summary>
    public class TipsFormDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 物品编号。
        /// </summary>
        public int GeneralItemId = 0;

        /// <summary>
        /// 参考点。
        /// </summary>
        public Transform RefTransform = null;
    }
}

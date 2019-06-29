using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="GeneralItemInfoForm"/> 显示数据。
    /// </summary>
    public class GeneralItemInfoDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 物品道具。
        /// </summary>
        public int TypeId;

        /// <summary>
        /// 物品数量。
        /// </summary>
        public int Qty;

        /// <summary>
        /// 等级。
        /// </summary>
        public int Level;

        /// <summary>
        /// 星级。
        /// </summary>
        public int StarLevel;

        /// <summary>
        /// 是否可放入。
        /// </summary>
        /// <remark>仅用于英雄升品道具。</remark>
        public bool CanInlay;

        /// <summary>
        /// 放入操作的回调。
        /// </summary>
        public GameFrameworkAction OnInlay;
    }
}

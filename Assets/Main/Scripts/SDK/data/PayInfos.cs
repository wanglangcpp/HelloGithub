using System;

namespace Genesis.GameClient
{
    [Serializable]
    /// <summary>
    /// 所有参数尽量填写,以后随不同SDK所需参数而进行扩展
    /// </summary>
    public class PayInfos
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID">商品ID</param>
        /// <param name="productName">商品名</param>
        /// <param name="productDesc">商品描述</param>
        /// <param name="price">商品价格（元）</param>
        public PayInfos(string pItemID, string pProductName, string pProductDesc, int pPrice)
        {
            itemId = pItemID;
            productName = pProductName;
            productDesc = pProductDesc;
            price = pPrice;
        }
        /// <summary>
        /// 商品名
        /// </summary>
        public string productName = string.Empty;
        /// <summary>
        /// 商品描述 可与商品名一致
        /// </summary>
        public string productDesc = string.Empty;
        /// <summary>
        /// 商品数量 默认为0 
        /// </summary>
        public int amount = 1;
        /// <summary>
        /// 价格（元）
        /// </summary>
        public int price = 0;
        /// <summary>
        /// 服务器ID
        /// </summary>
        public string gameServerId = string.Empty;
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string gameServerName = string.Empty;
        /// <summary>
        /// 唯一的账户ID
        /// </summary>
        public string roleId = string.Empty;
        /// <summary>
        /// 唯一的账户名称
        /// </summary>
        public string roleName = string.Empty;

        public string userId = string.Empty;

        public string itemId = string.Empty;

        public string orderId = string.Empty;

        public string vipLv = string.Empty;

        public string playerLv = string.Empty;

        public string party = "None";

        public string balance = string.Empty;

    }
}

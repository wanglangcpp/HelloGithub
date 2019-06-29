using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class RecordMgr
    {
        ///// <summary>
        ///// 服务器id
        ///// </summary>
        //public int ServerId = 0;
        ///// <summary>
        ///// 服务器名字
        ///// </summary>
        //public string ServeName = string.Empty;
        ///// <summary>
        ///// 玩家id
        ///// </summary>
        //public int PlayerId = 0;
        ///// <summary>
        ///// 显示用id
        ///// </summary>
        //public int DisplayId = 0;
        ///// <summary>
        ///// 玩家名字
        ///// </summary>
        //public string PlayerName = string.Empty;
        ///// <summary>
        ///// 等级
        ///// </summary>
        //public int Level = 0;
        ///// <summary>
        ///// vip等级
        ///// </summary>
        //public int VipLevel = 0;
        ///// <summary>
        ///// 货币id（钻石的itemid）
        ///// </summary>
        //public int DiamondId = 6;
        ///// <summary>
        ///// 货币名字
        ///// </summary>
        //public string DiamondName = "diamond";
        ///// <summary>
        ///// 货币数量
        ///// </summary>
        //public int DiamondCount = 0;
        ///// <summary>
        ///// 战力
        ///// </summary>
        //public int Might = 0;

        /// <summary>
        /// 服务器信息
        /// </summary>
        public ServerData serverData;
        /// <summary>
        /// 玩家信息
        /// </summary>
        public PlayerData playerData;
        /// <summary>
        /// 好友列表
        /// </summary>
        public List<PlayerData> Friends = new List<PlayerData>();
    }
}


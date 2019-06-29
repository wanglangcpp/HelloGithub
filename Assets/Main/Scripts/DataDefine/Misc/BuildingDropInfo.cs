using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    /// <summary>
    /// 建筑物掉落信息。
    /// </summary>
    public class BuildingDropInfo
    {
        public BuildingDropType Type { get; set; }
        public int Weight { get; set; }
        public int[] Params { get; set; }
    }
}

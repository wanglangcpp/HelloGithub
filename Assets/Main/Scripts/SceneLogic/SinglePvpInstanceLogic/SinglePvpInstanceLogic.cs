using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 一对一在线副本逻辑。
    /// </summary>
    [Serializable]
    public partial class SinglePvpInstanceLogic : BasePvpInstanceLogic
    {
        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.SinglePvp;
            }
        }
    }
}

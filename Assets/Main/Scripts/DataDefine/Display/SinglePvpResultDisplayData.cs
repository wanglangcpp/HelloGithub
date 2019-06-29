using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="SinglePvpResultDisplayData"/> 的显示数据。
    /// </summary>
    public class SinglePvpResultDisplayData : UIFormBaseUserData
    {
        public InstanceResultType ResultType { get; set; }

        public int WinnerId { get; set; }

        public string WinnerName { get; set; }

        public string WinnerServerName { set; get; }

        public int Reason { get; set; }
        private List<PBPVPSettlement> settlements = new List<PBPVPSettlement>();
        public List<PBPVPSettlement> Settlements { get { return settlements; } }
    }
}

using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class PlayerInfoDisplayData : UIFormBaseUserData
    {
        public int PlayerId { get; set; }
        public List<int> HeroTypes { get; set; }
    }
}

using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="InventoryForm"/> 显示数据
    /// </summary>
    public class InventoryDisplayData : UIFormBaseUserData
    {
        public InventoryForm.InventoryType InventoryType { set; get; }

        public int ComposeQuality { set; get; }

        public int GearType { set; get; }

        public int HeroType { set; get; }

        public int EpigraphIndex { set; get; }

        public int ChangeGearPosition { set; get; }

        public int ChangeGearId { set; get; }

        public bool NeedToggle { set; get; }

        public List<GearData> MultiSelectedGears { set; get; }
    }
}

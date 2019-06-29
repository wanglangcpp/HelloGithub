using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient {
    /// <summary>
    /// <see cref="NoviceGuideDialog"/> 显示数据。
    /// </summary>
    public class NoviceGuideDialogData : UIFormBaseUserData
    {
        public List<DRGuideUI> oneGroup;
        public int lastGuideId;
        public NGUIForm mForm;
    }
}

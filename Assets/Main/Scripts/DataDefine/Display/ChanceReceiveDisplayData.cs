using GameFramework;

namespace Genesis.GameClient
{
    public class ChanceReceiveDisplayData : UIFormBaseUserData
    {
        public ReceivedGeneralItemsViewData ReceiveGoodsData { get; set; }
        public GameFrameworkAction<object> OnComplete { get; set; }
    }
}
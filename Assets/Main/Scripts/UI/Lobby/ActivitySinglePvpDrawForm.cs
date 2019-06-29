using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivitySinglePvpDrawForm : NGUIForm
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnInitData();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        private void OnInitData()
        {

        }

        public void OnClickConfirmButton()
        {
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.SinglePvpShouldPlayRankScoreAnims, true);
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
            GameEntry.SceneLogic.GoBackToLobby(false);
            //GameEntry.Network.DestroyNetworkChannel(Constant.Network.RoomNetworkChannelName);
            //GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName).Close();
            //GameEntry.Data.Room.Id = -1;
            //GameEntry.Data.Room.HasReconnected = false;
        }
    }
}

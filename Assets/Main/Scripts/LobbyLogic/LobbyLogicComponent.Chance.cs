namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void RequestChanceInfo(ChanceType chanceType)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLRefreshChance request = new CLRefreshChance();
                request.ChanceType = (int)chanceType;
                GameEntry.Network.Send(request);
            }
        }

        public void OpenChance(ChanceType chanceType, int openIndex)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLOpenChance request = new CLOpenChance();
                request.ChanceType = (int)chanceType;
                request.OpenDummyIndex = openIndex;
                GameEntry.Network.Send(request);
            }
            else
            {
                LCOpenChance mock = new LCOpenChance();
                mock.ChanceType = (int)chanceType;
                mock.OpenedRealIndex = openIndex;
                ItemData itemData = GameEntry.Data.Chances.GetChanceData(chanceType).GoodsForView.Data[openIndex];
                mock.CompoundItemInfo = new PBCompoundItemInfo();
                mock.CompoundItemInfo.ItemInfo.Type = itemData.Type;

                GameEntry.Data.Chances.UpdateData(mock);
                GameEntry.Event.Fire(this, new ChanceDataChangedEventArgs(mock.ChanceType));
            }
        }

        public void OpenAllChances(ChanceType chanceType)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLOpenAllChances request = new CLOpenAllChances();
                request.ChanceType = (int)chanceType;
                GameEntry.Network.Send(request);
            }
        }

        public void RefreshChanceInfo(ChanceType chanceType)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLRefreshChance request = new CLRefreshChance();
                request.ChanceType = (int)chanceType;
                GameEntry.Network.Send(request);
            }
            else
            {
                LCRefreshChance mock = new LCRefreshChance();
                mock.ChanceInfo.ChanceType = (int)chanceType;

                //mock.ChanceInfo.ChanceItems.Add(new PBChanceItem { Type = 202303, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 203502, Count = 2 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 209401, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 209410, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 113001, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 114016, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 171002, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 172004, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 181101, Count = 1 });
                //mock.GoodInfo.Add(new PBItemInfo { Type = 181106, Count = 1 });

                //GameEntry.Data.Chances.UpdateData(mock);
                //GameEntry.Event.Fire(this, new ChanceDataChangedEventArgs(mock.ChanceType));
            }
        }
    }
}

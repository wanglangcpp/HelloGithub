using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// PVP竞技玩家数据集合类。
    /// </summary>
    [Serializable]
    public class PvpArenaPlayersData : GenericData<PvpArenaPlayerAndTeamData, PBArenaPlayerAndTeamInfo>
    {


        public void UpdateData(PBArenaPlayerAndTeamInfo data)
        {
            throw new NotImplementedException();
        }
    }
}

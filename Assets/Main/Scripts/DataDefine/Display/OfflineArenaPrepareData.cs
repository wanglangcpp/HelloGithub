namespace Genesis.GameClient
{
    /// <summary>
    /// 用于传递给 <see cref="Genesis.GameClient.PvpOfflineArenaForm"/>
    /// </summary>
    public class OfflineArenaPrepareData : UIFormBaseUserData
    {
        /// <summary>
        /// 获取或设置是否为复仇战。
        /// </summary>
        public bool IsRevenge { get; set; }
    }
}

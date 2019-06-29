using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="PlotDialogueForm"/> 显示数据。
    /// </summary>
    public class PlotDialogDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 在 <see cref="PlotDialogueForm"/> 打开时调用的回调。
        /// </summary>
        public GameFrameworkAction PlotReturn;

        /// <summary>
        /// 剧情对话编号。
        /// </summary>
        public int PlotDialogId;
    }
}

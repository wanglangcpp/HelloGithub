using GameFramework;

namespace Genesis.GameClient
{
    public class DialogueFormDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 对话编号。
        /// </summary>
        public int DialogId;

        /// <summary>
        /// 任务编号
        /// </summary>
        public DRTask Task;

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object userData;
    }

}

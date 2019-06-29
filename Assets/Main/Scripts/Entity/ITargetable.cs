namespace Genesis.GameClient
{
    /// <summary>
    /// 可被选为目标的实体接口。
    /// </summary>
    public interface ITargetable : ICampable
    {
        /// <summary>
        /// 是否可用。
        /// </summary>
        bool IsAvailable
        {
            get;
        }

        /// <summary>
        /// 是否死亡。
        /// </summary>
        bool IsDead
        {
            get;
        }

        /// <summary>
        /// 是否假死。
        /// </summary>
        bool IsFakeDead
        {
            get;
        }

        /// <summary>
        /// 是否正在入场。
        /// </summary>
        bool IsEntering
        {
            get;
        }
    }
}

namespace Genesis.GameClient
{
    /// <summary>
    /// Reason for winning an instance.
    /// </summary>
    public enum InstanceSuccessReason
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The opponent gives up.
        /// </summary>
        AbandonedByOpponent,

        /// <summary>
        /// Time out.
        /// </summary>
        TimeOut,

        /// <summary>
        /// The user beats the opponent.
        /// </summary>
        HasBeatenOpponent,

        /// <summary>
        /// Claimed by the AI.
        /// </summary>
        ClaimedByAI,

        /// <summary>
        /// Has been beaten by the opponent.
        /// </summary>
        HasBeenBeaten, // 有时候团灭也胜利，真奇怪。
    }
}

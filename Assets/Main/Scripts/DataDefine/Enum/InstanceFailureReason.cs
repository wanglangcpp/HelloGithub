namespace Genesis.GameClient
{
    /// <summary>
    /// Reasons for losing an instance.
    /// </summary>
    public enum InstanceFailureReason
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The user gives up.
        /// </summary>
        AbandonedByUser,

        /// <summary>
        /// Time out.
        /// </summary>
        TimeOut,

        /// <summary>
        /// Beaten by opponent.
        /// </summary>
        HasBeenBeaten,

        /// <summary>
        /// Claimed by the AI.
        /// </summary>
        ClaimedByAI,
        /// <summary>
        ///  Npc who escorted is dead.
        /// </summary>
        Escort,
    }
}

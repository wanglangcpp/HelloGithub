namespace Genesis.GameClient
{
    /// <summary>
    /// 用别名播放实体动画。暂时只支持 <see cref="Building"/> 实体。
    /// </summary>
    public class EntityAliasedAnimationTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityAliasedAnimationTimeLineActionData m_Data;

        public EntityAliasedAnimationTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityAliasedAnimationTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            timeLineInstance.Owner.PlayAliasedAnimation(m_Data.AnimClipAlias);
        }
    }
}

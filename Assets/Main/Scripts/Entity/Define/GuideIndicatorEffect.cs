namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家英雄向导标志实体。
    /// </summary>
    public class GuideIndicatorEffect : Effect
    {
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            PerformLookAt();
        }

        private void LateUpdate()
        {
            PerformLookAt();
        }

        private void PerformLookAt()
        {
            if (GameEntry.SceneLogic.IsInstance)
            {
                CachedTransform.LookAt2D(GameEntry.SceneLogic.BaseInstanceLogic.GuideIndicatorTarget);
            }
        }
    }
}

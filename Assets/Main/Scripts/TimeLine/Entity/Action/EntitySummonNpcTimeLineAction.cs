using GameFramework;

namespace Genesis.GameClient
{
    public class EntitySummonNpcTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntitySummonNpcTimeLineActionData m_Data;

        public EntitySummonNpcTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntitySummonNpcTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (m_Data.NpcIndices == null)
            {
                Log.Warning("You haven't indicate any NPC to summon.");
                return;
            }

            if (timeLineInstance.Owner == null)
            {
                Log.Warning("Time line owner is invalid.");
                return;
            }

            for (int i = 0; i < m_Data.NpcIndices.Length; ++i)
            {
                TrySummonNpc(timeLineInstance, i);
            }
        }

        private void TrySummonNpc(ITimeLineInstance<Entity> timeLineInstance, int index)
        {
            for (int i = 0; i < m_Data.RetryCount; ++i)
            {
                if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.SummonNpc(m_Data.NpcIndices[index], timeLineInstance.Owner, m_Data.TransformType, m_Data.RandomRadius, m_Data.AttackOwnerTarget, m_Data.DieWithOwner))
                {
                    //Log.Info("[EntitySummonNpcTimeLineAction TrySummonNpc] NPC index: {0}, retry index: {1}.", m_Data.NpcIndices[index].ToString(), i.ToString());
                    break;
                }
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {

        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {

        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }
    }
}

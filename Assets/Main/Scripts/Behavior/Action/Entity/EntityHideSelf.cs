using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class EntityHideSelf : Action
    {
        private Entity m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
        }

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the Entity component!");
                return TaskStatus.Failure;
            }

            GameEntry.Entity.HideEntity(m_Self.Entity);
            return TaskStatus.Success;
        }
    }
}

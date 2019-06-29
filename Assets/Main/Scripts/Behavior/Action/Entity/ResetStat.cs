using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class ResetStat : Action
    {
        [SerializeField]
        private string m_CustomKey = null;

        [SerializeField]
        private bool m_AffectOwner = false;

        private Entity m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the Entity component!");
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.Stat.ResetStat(m_Self.Id, m_CustomKey, m_AffectOwner && m_Self.HasOwner ? m_Self.Owner.Id : 0);
            return TaskStatus.Success;
        }
    }
}

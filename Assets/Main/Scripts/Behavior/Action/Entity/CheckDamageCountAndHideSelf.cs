using BehaviorDesigner.Runtime.Tasks;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckDamageCountAndHideSelf : Action
    {
        [SerializeField]
        private string m_CustomKey = null;

        [SerializeField]
        private int m_DamageCount = 0;

        [SerializeField]
        private OrderRelationType m_OrderRelation = OrderRelationType.EqualTo;

        private Entity m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
            GameEntry.Event.Subscribe(EventId.StatDamageRecorded, OnStatDamageRecorded);
        }

        public override void OnEnd()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.StatDamageRecorded, OnStatDamageRecorded);
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

        private void OnStatDamageRecorded(object sender, GameEventArgs e)
        {
            var ne = e as StatDamageRecordedEventArgs;
            if (m_Self == null || m_Self.Entity == null || !m_Self.IsAvailable)
            {
                return;
            }

            if (ne.EntityId != m_Self.Entity.Id)
            {
                return;
            }

            int currentVal = GameEntry.SceneLogic.BaseInstanceLogic.Stat.GetDamageCount(m_Self.Id, m_CustomKey);
            if (OrderRelationUtility.AreSatisfying(currentVal, m_DamageCount, m_OrderRelation))
            {
                GameEntry.Entity.HideEntity(m_Self);
            }
        }
    }
}

using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityShowEffectToTargetTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityShowEffectToTargetTimeLineActionData m_Data;

        private int m_EffectEntityId = 0;
        private Effect m_EffectEntity = null;
        private Vector2 m_StartingPosition = Vector2.zero;
        private Vector2 m_TargetPosition = Vector2.zero;

        public EntityShowEffectToTargetTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityShowEffectToTargetTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);

            var targetHaver = timeLineInstance.Owner as ICanHaveTarget;
            if (targetHaver == null || targetHaver as Entity == null)
            {
                Log.Error("You cannot use this action on an entity that cannot have a target.");
                return;
            }

            var ownerTransform = (targetHaver as Entity).CachedTransform;
            m_StartingPosition = ownerTransform.localPosition.ToVector2();
            if (targetHaver.HasTarget)
            {
                m_TargetPosition = (targetHaver.Target as Entity).CachedTransform.localPosition.ToVector2();
            }
            else
            {
                m_TargetPosition = m_StartingPosition;
            }

            m_EffectEntityId = GameEntry.Entity.GetSerialId();
            GameEntry.Entity.ShowEffect(new EffectData(m_EffectEntityId, m_Data.ResourceName, ownerTransform.localPosition, ownerTransform.localRotation.y));
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            if (m_EffectEntity == null)
            {
                return;
            }

            var currentTime = timeLineInstance.CurrentTime - StartTime;
            var currentPosition = m_StartingPosition + currentTime / (Duration <= 0f ? 1f : Duration) * (m_TargetPosition - m_StartingPosition);

            m_EffectEntity.CachedTransform.localPosition = currentPosition.ToVector3(m_EffectEntity.CachedTransform.localPosition.y);
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            m_EffectEntity = null;
            CheckAutoStop(timeLineInstance);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            m_EffectEntity = null;
            CheckAutoStop(timeLineInstance);
        }

        private void CheckAutoStop(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (!m_Data.AutoStop)
            {
                return;
            }

            if (GameEntry.Entity.HasEntity(m_EffectEntityId) || GameEntry.Entity.IsLoadingEntity(m_EffectEntityId))
            {
                GameEntry.Entity.HideEntity(m_EffectEntityId);
            }
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            if (ne.Entity.Id != m_EffectEntityId)
            {
                return;
            }

            m_EffectEntity = ne.Entity.Logic as Effect;
            if (m_EffectEntity == null)
            {
                Log.Error("The entity '{0}' is not an Effect.", m_EffectEntityId.ToString());
            }
        }
    }
}

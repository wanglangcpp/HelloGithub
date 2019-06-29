using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class ShowEffect : Action
    {
        private enum Mode
        {
            Default,
            OwnedBySelf,
        }

        [SerializeField]
        private Mode m_Mode = Mode.Default;

        [SerializeField]
        private string m_EffectResourceName = null;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies if Mode is OwnedBySelf.")]
        [SerializeField]
        private bool m_CanAttachToDeadOwner = false;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies if Mode is OwnedBySelf.")]
        [SerializeField]
        private string m_TargetParentPath = null;

        private Entity m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();

            int effectEntityId = GameEntry.Entity.GetSerialId();
            switch (m_Mode)
            {
                case Mode.OwnedBySelf:
                    var effectData = new EffectData(effectEntityId, m_TargetParentPath, m_EffectResourceName, m_Self.Id);
                    effectData.CanAttachToDeadOwner = m_CanAttachToDeadOwner;
                    GameEntry.Entity.ShowEffect(effectData);
                    break;
                case Mode.Default:
                default:
                    GameEntry.Entity.ShowEffect(new EffectData(effectEntityId, m_EffectResourceName, m_Self.CachedTransform.position, m_Self.CachedTransform.rotation.eulerAngles.y));
                    break;
            }
        }
    }
}

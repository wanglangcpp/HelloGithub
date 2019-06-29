using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class DestroyUIEffects : Action
    {
        [SerializeField]
        private SharedString m_EffectKeyFormat = string.Empty;

        [SerializeField]
        private SharedInt m_BegIndex = 0;

        [SerializeField]
        private SharedInt m_EndIndex = 0;

        private IUpdatableUIFragment m_Owner = null;

        private UIEffectsController EffectsController
        {
            get { return m_Owner.EffectsController; }
        }

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                Log.Warning("Onwer is invalid.");
                return TaskStatus.Failure;
            }

            for (int i = m_BegIndex.Value; i < m_EndIndex.Value; i++)
            {
                var effectKey = string.Format(m_EffectKeyFormat.Value, i);
                EffectsController.DestroyEffect(effectKey);
            }

            return TaskStatus.Success;
        }
    }
}

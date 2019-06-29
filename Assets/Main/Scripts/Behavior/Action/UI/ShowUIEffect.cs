using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class ShowUIEffect : Action
    {
        [SerializeField]
        private SharedString m_EffectKeyFormat = string.Empty;

        [SerializeField]
        private SharedInt[] m_EffectKeyParams = null;

        private IUpdatableUIFragment m_Owner = null;

        private UIEffectsController EffectsController
        {
            get
            {
                return m_Owner.EffectsController;
            }
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

            var formatParams = new object[m_EffectKeyParams.Length];

            for (int i = 0; i < m_EffectKeyParams.Length; ++i)
            {
                formatParams[i] = m_EffectKeyParams[i];
            }

            int effectId = EffectsController.ShowEffect(m_EffectKeyParams.Length <= 0 ? m_EffectKeyFormat.Value : string.Format(m_EffectKeyFormat.Value, m_EffectKeyParams));
            return effectId == 0 ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}

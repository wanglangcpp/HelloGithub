using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowNpcRelativeToNpc : Action
    {
        [SerializeField]
        private int m_NpcIndex = 0;

        [SerializeField]
        private int m_TargetNpcIndex = 0;

        [SerializeField]
        private float m_DeltaX = 0;

        [SerializeField]
        private float m_DeltaY = 0;

        [SerializeField]
        private bool m_Random = false;

        [SerializeField]
        private float m_RandomRadius = 1.0f;

        [SerializeField]
        private float m_MinRandomRadius = 1.0f;

        [SerializeField]
        private int m_RetryCount = 20;

        [SerializeField]
        private float m_OffsetDegree = 180;

        private const float HalfRoundAngle = 180;
        private NpcCharacter m_Target = null;

        public override void OnStart()
        {
            IsFinished = false;
            m_Target = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetNpcFromIndex(m_TargetNpcIndex);
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsNpcForbidden(m_NpcIndex))
            {
                return TaskStatus.Failure;
            }

            if (m_Target == null || m_Target.IsDead)
            {
                return TaskStatus.Failure;
            }
            var path = new NavMeshPath();

            for (int i = 0; i < m_RetryCount; ++i)
            {
                var vec = GeneratePosition(i);
                var npcPosition = AIUtility.SamplePosition(vec);
                bool gotPath = NavMesh.CalculatePath(m_Target.CachedTransform.localPosition, npcPosition, NavMesh.AllAreas, path);
                if (gotPath && path.status == NavMeshPathStatus.PathComplete)
                {
                    if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ShowNpcRelativeToNpc(m_NpcIndex, vec))
                    {
                        IsFinished = true;
                        break;
                    }
                }
            }
            return IsFinished ? TaskStatus.Success : TaskStatus.Failure;
        }

        private Vector3 GeneratePosition(int times)
        {
            Vector2 vec = Vector2.zero;
            Vector2 offset = new Vector2(m_DeltaX, m_DeltaY);
            if (m_Target == null)
            {
                return vec;
            }
            vec = m_Target.CachedTransform.localPosition.ToVector2();
            if (m_Random || times > 0)
            {                
                float angle = Random.Range(-m_OffsetDegree, m_OffsetDegree);
                float radius = Random.Range(m_MinRandomRadius, m_RandomRadius);
                vec = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
            }
            vec += offset;
            return vec;
        }

        private bool IsFinished
        {
            get;
            set;
        }
    }
}

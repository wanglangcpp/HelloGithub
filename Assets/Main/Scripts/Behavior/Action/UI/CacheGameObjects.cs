using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class CacheGameObjects : Action
    {
        private enum Mode
        {
            Default,
            Format
        }

        [SerializeField]
        private Mode m_Mode = Mode.Default;

        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies for Default mode.")]
        [SerializeField]
        private string[] m_TransformPaths = null;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies for Format mode.")]
        [SerializeField]
        private SharedString m_TransformPathFormat = null;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies for Format mode.")]
        [SerializeField]
        private SharedInt m_BegIndex = 0;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies for Format mode.")]
        [SerializeField]
        private SharedInt m_EndIndex = 0;

        private IUpdatableUIFragment m_Owner = null;
        private GameFrameworkFunc<IList<string>> m_GetTransformPathsDelegate = null;

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);
            switch (m_Mode)
            {
                case Mode.Format:
                    m_GetTransformPathsDelegate = GetTransformPaths_Format;
                    break;
                default:
                    m_GetTransformPathsDelegate = GetTransformPaths_Default;
                    break;
            }
        }

        public override TaskStatus OnUpdate()
        {
            m_GOList.Value.Clear();
            var transformPaths = m_GetTransformPathsDelegate();

            for (int i = 0; i < transformPaths.Count; ++i)
            {
                var go = m_Owner.GetGameObject(transformPaths[i]);
                if (go == null)
                {
                    return TaskStatus.Failure;
                }

                m_GOList.Value.Add(go);
            }

            return TaskStatus.Success;
        }

        private IList<string> GetTransformPaths_Default()
        {
            List<string> transformPaths = new List<string>();
            for (int i = 0; i < m_TransformPaths.Length; ++i)
            {
                transformPaths.Add(m_TransformPaths[i]);
            }

            return transformPaths;
        }

        private IList<string> GetTransformPaths_Format()
        {
            List<string> transformPaths = new List<string>();
            for (int i = m_BegIndex.Value; i < m_EndIndex.Value; ++i)
            {
                transformPaths.Add(string.Format(m_TransformPathFormat.Value, i));
            }

            return transformPaths;
        }
    }
}

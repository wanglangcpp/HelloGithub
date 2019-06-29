using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 简单连线特效控制器。
    /// </summary>
    public class SimpleLineEffectController : MonoBehaviour
    {
        private LineRenderer m_CachedLineRenderer = null;

        private Effect m_EffectScript = null;
        private EffectData m_EffectData = null;

        public enum StartingDirection
        {
            Normal,
            Reverse,
        }

        [SerializeField]
        private StartingDirection m_StartingDirection = StartingDirection.Normal;

        [SerializeField]
        private float m_StartingDuration = 0f;

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedLineRenderer = GetComponentsInChildren<LineRenderer>(true)[0];
        }

        private void OnEnable()
        {
            StartCoroutine(ShowCo());
        }

        private void OnDisable()
        {
            m_EffectScript = null;
            m_EffectData = null;
            ResetLineRenderer();
        }

        #endregion MonoBehaviour

        private IEnumerator ShowCo()
        {
            ResetLineRenderer();
            yield return StartCoroutine(FetchEffectScript());
            yield return StartCoroutine(FetchEffectData());

            var simpleLineEffectData = m_EffectData as SimpleLineEffectData;
            if (simpleLineEffectData == null)
            {
                yield break;
            }

            var endingTransform = FetchEndingTransform(simpleLineEffectData);
            m_CachedLineRenderer.enabled = true;
            yield return StartCoroutine(ShowStartingTransition(simpleLineEffectData, endingTransform));
            yield return StartCoroutine(ShowNormal(endingTransform));
        }

        private IEnumerator FetchEffectScript()
        {
            while (true)
            {
                m_EffectScript = GetComponent<Effect>();
                if (m_EffectScript != null)
                {
                    break;
                }

                yield return null;
            }
        }

        private IEnumerator FetchEffectData()
        {
            while (true)
            {
                m_EffectData = m_EffectScript.Data;
                if (m_EffectData != null)
                {
                    break;
                }

                yield return null;
            }
        }

        private IEnumerator ShowStartingTransition(SimpleLineEffectData simpleLineEffectData, Transform endingTransform)
        {
            var time = 0f;
            while (time < m_StartingDuration)
            {
                if (!endingTransform || !endingTransform.gameObject.activeInHierarchy)
                {
                    yield break;
                }

                var startingPoint = Vector3.zero;
                var endingPoint = m_EffectScript.CachedTransform.InverseTransformPoint(endingTransform.TransformPoint(Vector3.zero));

                switch (m_StartingDirection)
                {
                    case StartingDirection.Normal:
                        endingPoint = time / m_StartingDuration * endingPoint;
                        break;
                    case StartingDirection.Reverse:
                        startingPoint = (1 - time / m_StartingDuration) * endingPoint;
                        break;
                    default:
                        break;
                }

                m_CachedLineRenderer.SetPosition(0, startingPoint);
                m_CachedLineRenderer.SetPosition(1, endingPoint);
                time += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ShowNormal(Transform endingTransform)
        {
            while (true)
            {
                if (!endingTransform || !endingTransform.gameObject.activeInHierarchy)
                {
                    yield break;
                }

                var startingPoint = Vector3.zero;
                var endingPoint = m_EffectScript.CachedTransform.InverseTransformPoint(endingTransform.TransformPoint(Vector3.zero));
                m_CachedLineRenderer.SetPosition(0, startingPoint);
                m_CachedLineRenderer.SetPosition(1, endingPoint);
                yield return null;
            }
        }

        private void ResetLineRenderer()
        {
            m_CachedLineRenderer.SetVertexCount(2);
            m_CachedLineRenderer.SetPosition(0, Vector3.zero);
            m_CachedLineRenderer.SetPosition(1, Vector3.zero);
            m_CachedLineRenderer.enabled = false;
        }

        private Transform FetchEndingTransform(SimpleLineEffectData simpleLineEffectData)
        {
            if (!GameEntry.IsAvailable)
            {
                return null;
            }

            var entity = GameEntry.Entity.GetGameEntity(simpleLineEffectData.EndingEntityId);
            if (entity == null)
            {
                return null;
            }

            return entity.CachedTransform.Find(simpleLineEffectData.EndingTransformPath);
        }
    }
}

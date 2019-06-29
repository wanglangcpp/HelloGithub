#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class BaseEntityPlanter : MonoBehaviour
    {
        [SerializeField]
        protected Color m_SphereColor = Color.yellow;

        [SerializeField]
        protected Color m_ArrowColor = Color.red;

        [SerializeField]
        protected float m_ArrowSize = 1f;

        [SerializeField]
        protected Color m_HandleTextColor = Color.green;

        [SerializeField]
        protected int m_TextGizmoSize = 24;

        [SerializeField]
        protected float m_GizmoRadius = 0.5f;

        [SerializeField, Tooltip("Comments/Remarks written by game designers")]
        protected string m_GDRemark = string.Empty;

        [SerializeField, Tooltip("When the entity is killed by the player, shall we count it for, say, an instance request.")]
        protected bool m_CountForPlayerKill = true;

        [SerializeField, HideInInspector]
        protected NavMeshAgent m_CachedNavMeshAgent = null;

        public Transform CachedTransform { get; protected set; }
        protected static readonly GUIStyle TextGizmoStyle = GUIStyle.none;

        public abstract int Index { get; }

        protected abstract int TypeId { get; }

        protected abstract string NameFormat { get; }
    }

    public abstract class AbstractEntityPlanter<TDR> : BaseEntityPlanter where TDR : DRInstanceEntities, new()
    {
        protected abstract TDR RefDataRow { get; }

        public virtual void Init(TDR refDataRow)
        {
            //Debug.LogFormat("[{0} Init]", GetType().Name);

            CachedTransform.rotation = Quaternion.Euler(0f, refDataRow.Rotation, 0f);
            SamplePosition(new Vector2(refDataRow.PositionX, refDataRow.PositionY));

            m_GDRemark = refDataRow.GDRemark;
            m_CountForPlayerKill = refDataRow.CountForPlayerKill;
            m_CachedNavMeshAgent.enabled = true;
        }

        public TDR SaveDataRow()
        {
            var ret = new TDR();
            ret.ParseDataRow(string.Join("\t", new string[]
            {
                string.Empty,
                Index.ToString(),
                m_GDRemark,
                TypeId.ToString(),
                CachedTransform.position.x.ToString(),
                CachedTransform.position.z.ToString(),
                CachedTransform.rotation.eulerAngles.y.ToString(),
                m_CountForPlayerKill.ToString(),
            }));

            Init(ret);
            return ret;
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedNavMeshAgent = GetComponent<NavMeshAgent>();
            CachedTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                gameObject.SetActive(false);
            }

            name = string.Format(NameFormat, Index);

            SamplePosition(CachedTransform.position.ToVector2());
        }

        #endregion MonoBehaviour

        private void SamplePosition(Vector2 pos2D)
        {
            CachedTransform.position = AIUtility.SamplePosition(pos2D);
        }

        private void OnDrawGizmos()
        {
            var cachedActiveTextColor = TextGizmoStyle.active.textColor;
            var cachedNormalTextColor = TextGizmoStyle.normal.textColor;
            var cachedFontSize = TextGizmoStyle.fontSize;

            TextGizmoStyle.active.textColor = TextGizmoStyle.normal.textColor = m_HandleTextColor;
            TextGizmoStyle.fontSize = m_TextGizmoSize;

            Color cachedColor = Gizmos.color;
            Gizmos.color = new Color(m_SphereColor.r, m_SphereColor.g, m_SphereColor.b, 0.3f);
            Gizmos.DrawSphere(CachedTransform.position, m_GizmoRadius);
            Gizmos.color = m_SphereColor;
            Gizmos.DrawWireSphere(CachedTransform.position, m_GizmoRadius);
            Gizmos.color = cachedColor;

            cachedColor = Handles.color;
            Handles.color = m_ArrowColor;
            Handles.ArrowCap(int.MinValue, CachedTransform.position, CachedTransform.rotation, m_ArrowSize);
            Handles.Label(CachedTransform.position + CachedTransform.up * m_GizmoRadius, Index.ToString(), TextGizmoStyle);
            Handles.color = cachedColor;

            TextGizmoStyle.active.textColor = cachedActiveTextColor;
            TextGizmoStyle.normal.textColor = cachedNormalTextColor;
            TextGizmoStyle.fontSize = cachedFontSize;
        }
    }
}

#endif

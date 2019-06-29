#if UNITY_EDITOR

using UnityEngine;

namespace Genesis.GameClient
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcPlanter : AbstractEntityPlanter<DRInstanceNpcs>
    {
        [SerializeField, Tooltip("The NPC ID (index) in the INSTANCE NPC data table.")]
        private int m_NpcIndex = 0;

        [SerializeField, Tooltip("The NPC ID in the NPC data table")]
        private int m_NpcId = 0;

        [SerializeField, HideInInspector]
        private DRInstanceNpcs m_RefDataRow = null;

        public override int Index
        {
            get
            {
                return m_NpcIndex;
            }
        }

        protected override DRInstanceNpcs RefDataRow
        {
            get
            {
                return m_RefDataRow;
            }
        }

        protected override int TypeId
        {
            get
            {
                return m_NpcId;
            }
        }

        protected override string NameFormat
        {
            get
            {
                return "NPC {0}";
            }
        }

        public override void Init(DRInstanceNpcs refDataRow)
        {
            m_RefDataRow = refDataRow;
            m_NpcIndex = refDataRow.Id;
            m_NpcId = refDataRow.EntityTypeId;

            base.Init(refDataRow);
        }
    }
}

#endif

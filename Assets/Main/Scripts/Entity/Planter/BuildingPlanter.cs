#if UNITY_EDITOR

using UnityEngine;

namespace Genesis.GameClient
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(NavMeshAgent))]
    public class BuildingPlanter : AbstractEntityPlanter<DRInstanceBuildings>
    {
        [SerializeField, Tooltip("The Building ID (index) in the INSTANCE BUILDINGS data table.")]
        private int m_Index = 0;

        [SerializeField, Tooltip("The Building ID in the BUILDING data table.")]
        private int m_TypeId = 0;

        [SerializeField, HideInInspector]
        private DRInstanceBuildings m_RefDataRow = null;

        public override int Index
        {
            get
            {
                return m_Index;
            }
        }

        protected override DRInstanceBuildings RefDataRow
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
                return m_TypeId;
            }
        }

        protected override string NameFormat
        {
            get
            {
                return "Building {0}";
            }
        }

        public override void Init(DRInstanceBuildings refDataRow)
        {
            m_RefDataRow = refDataRow;
            m_Index = refDataRow.Id;
            m_TypeId = refDataRow.EntityTypeId;

            base.Init(refDataRow);
        }
    }
}

#endif

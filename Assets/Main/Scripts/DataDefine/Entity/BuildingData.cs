using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class BuildingData : TargetableObjectData
    {
        public new Building Entity
        {
            get
            {
                return base.Entity as Building;
            }
        }

        [SerializeField]
        private string m_ResourceName;

        public string ResourceName
        {
            get
            {
                return m_ResourceName;
            }
            set
            {
                m_ResourceName = value;
            }
        }

        [SerializeField]
        private int m_BuildingIndex;

        public int BuildingIndex
        {
            get
            {
                return m_BuildingIndex;
            }
            set
            {
                m_BuildingIndex = value;
            }
        }

        [SerializeField]
        private int m_BuildingTypeId;

        public int BuildingTypeId
        {
            get
            {
                return m_BuildingTypeId;
            }
            set
            {
                m_BuildingTypeId = value;
            }
        }

        [SerializeField]
        private int m_BuildingModelId;

        public int BuildingModelId
        {
            get
            {
                return m_BuildingModelId;
            }
            set
            {
                m_BuildingModelId = value;
            }
        }


        [SerializeField]
        private bool m_CountForPlayerKill;

        public bool CountForPlayerKill
        {
            get
            {
                return m_CountForPlayerKill;
            }
            set
            {
                m_CountForPlayerKill = value;
            }
        }

        /// <summary>
        /// 用于伤害计算的状态。
        /// </summary>
        public override StateForImpactCalc StateForImpactCalc
        {
            get
            {
                return StateForImpactCalc.Normal;
            }
        }

        [SerializeField]
        private NpcCategory m_Category;

        public NpcCategory Category
        {
            get
            {
                return m_Category;
            }
            set
            {
                m_Category = value;
            }
        }

        [SerializeField]
        protected int m_ChestId = -1;

        public int ChestId
        {
            get
            {
                return m_ChestId;
            }
            set
            {
                m_ChestId = value;
            }
        }

        [SerializeField]
        private bool m_CanBeSelectedAsTargetByAI;

        public bool CanBeSelectedAsTargetByAI
        {
            get
            {
                return m_CanBeSelectedAsTargetByAI;
            }

            set
            {
                m_CanBeSelectedAsTargetByAI = value;
            }
        }

        public BuildingData(int entityId)
            : base(entityId)
        {

        }
    }
}

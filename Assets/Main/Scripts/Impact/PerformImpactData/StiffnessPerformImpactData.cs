using UnityEngine;

namespace Genesis.GameClient
{
    public class StiffnessPerformImpactData : BasePerformImpactData
    {
        public class StiffnessGround
        {
            public StiffnessGround(DRImpact dataRow)
            {
                int index = 0;
                StiffnessTime = dataRow.ImpactParams[index++];
                RepulseStartTime = dataRow.ImpactParams[index++];
                RepulseDistance = dataRow.ImpactParams[index++];
                RepulseTime = dataRow.ImpactParams[index++];
                RepulseType = Mathf.RoundToInt(dataRow.ImpactParams[index++]);
                RepulseAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
            }

            public float StiffnessTime { get; private set; }
            public float RepulseStartTime { get; private set; }
            public float RepulseDistance { get; private set; }
            public float RepulseTime { get; private set; }
            public int RepulseType { get; private set; }
            public ImpactAnimationType RepulseAnimation { get; private set; }
        }

        public class StiffnessAir
        {
            public StiffnessAir(DRImpact dataRow)
            {
                int index = 6;
                StiffnessTime = dataRow.ImpactParams[index++];
                RepulseStartTime = dataRow.ImpactParams[index++];
                RepulseDistance = dataRow.ImpactParams[index++];
                RepulseTime = dataRow.ImpactParams[index++];
                RepulseType = Mathf.RoundToInt(dataRow.ImpactParams[index++]);
                RepulseAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
                FallingFloatingTime = dataRow.ImpactParams[index++];
                FallingFloatingDistance = dataRow.ImpactParams[index++];
                DownTime = dataRow.ImpactParams[index++];
                FallingAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
            }

            public float StiffnessTime { get; private set; }
            public float RepulseStartTime { get; private set; }
            public float RepulseDistance { get; private set; }
            public float RepulseTime { get; private set; }
            public int RepulseType { get; private set; }
            public ImpactAnimationType RepulseAnimation { get; private set; }
            public float FallingFloatingTime { get; private set; }
            public float FallingFloatingDistance { get; private set; }
            public float DownTime { get; private set; }
            public ImpactAnimationType FallingAnimation { get; private set; }
        }

        public StiffnessGround Ground { get; private set; }
        public StiffnessAir Air { get; private set; }

        public override void SetDataRow(DRImpact dataRow)
        {
            base.SetDataRow(dataRow);
            Ground = new StiffnessGround(dataRow);
            Air = new StiffnessAir(dataRow);
        }
    }
}

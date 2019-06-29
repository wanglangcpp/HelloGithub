using UnityEngine;

namespace Genesis.GameClient
{
    public class HardHitPerformImpactData : BasePerformImpactData
    {
        public class HardHitGround
        {
            public HardHitGround(DRImpact dataRow)
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

        public class HardHitAir
        {
            public HardHitAir(DRImpact dataRow)
            {
                int index = 6;
                FallingFloatingTime = dataRow.ImpactParams[index++];
                FallingFloatingDistance = dataRow.ImpactParams[index++];
                DownTime = dataRow.ImpactParams[index++];
                FallingAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
            }

            public float FallingFloatingTime { get; private set; }
            public float FallingFloatingDistance { get; private set; }
            public float DownTime { get; private set; }
            public ImpactAnimationType FallingAnimation { get; private set; }
        }

        public HardHitGround Ground { get; private set; }
        public HardHitAir Air { get; private set; }

        public override void SetDataRow(DRImpact dataRow)
        {
            base.SetDataRow(dataRow);
            Ground = new HardHitGround(dataRow);
            Air = new HardHitAir(dataRow);
        }
    }
}

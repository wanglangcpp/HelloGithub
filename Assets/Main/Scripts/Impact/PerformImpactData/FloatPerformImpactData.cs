using UnityEngine;

namespace Genesis.GameClient
{
    public class FloatPerformImpactData : BasePerformImpactData
    {
        public class FloatGround
        {
            public FloatGround(DRImpact dataRow)
            {
                int index = 0;
                RaiseFloatingDistance = dataRow.ImpactParams[index++];
                RaiseFloatingTime = dataRow.ImpactParams[index++];
                FallingFloatingDistance = dataRow.ImpactParams[index++];
                FallingFloatingTime = dataRow.ImpactParams[index++];
                DownTime = dataRow.ImpactParams[index++];
                RepulseStartTime = dataRow.ImpactParams[index++];
                RepulseType = Mathf.RoundToInt(dataRow.ImpactParams[index++]);
                RaiseAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
                FallingAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
            }

            public float RaiseFloatingDistance { get; private set; }
            public float RaiseFloatingTime { get; private set; }
            public float FallingFloatingDistance { get; private set; }
            public float FallingFloatingTime { get; private set; }
            public float DownTime { get; private set; }
            public float RepulseStartTime { get; private set; }
            public int RepulseType { get; private set; }
            public ImpactAnimationType RaiseAnimation { get; private set; }
            public ImpactAnimationType FallingAnimation { get; private set; }
        }

        public class FloatAir
        {
            public FloatAir(DRImpact dataRow)
            {
                int index = 9;
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

        public FloatGround Ground { get; private set; }
        public FloatAir Air { get; private set; }

        public override void SetDataRow(DRImpact dataRow)
        {
            base.SetDataRow(dataRow);
            Ground = new FloatGround(dataRow);
            Air = new FloatAir(dataRow);
        }
    }
}

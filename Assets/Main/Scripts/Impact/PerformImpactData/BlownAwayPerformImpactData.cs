using UnityEngine;

namespace Genesis.GameClient
{
    public class BlownAwayPerformImpactData : BasePerformImpactData
    {
        public class BlownAwayGround
        {
            public BlownAwayGround(DRImpact dataRow)
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

            public float RaiseFloatingTime { get; private set; }
            public float RaiseFloatingDistance { get; private set; }
            public float FallingFloatingTime { get; private set; }
            public float FallingFloatingDistance { get; private set; }
            public float DownTime { get; private set; }
            public float RepulseStartTime { get; private set; }
            public int RepulseType { get; private set; }
            public ImpactAnimationType RaiseAnimation { get; private set; }
            public ImpactAnimationType FallingAnimation { get; private set; }
        }

        public class BlownAwayAir
        {
            public BlownAwayAir(DRImpact dataRow)
            {
                int index = 9;
                RepulseType = Mathf.RoundToInt(dataRow.ImpactParams[6]);
                FallingFloatingTime = dataRow.ImpactParams[index++];
                FallingFloatingDistance = dataRow.ImpactParams[index++];
                DownTime = dataRow.ImpactParams[index++];
                FallingAnimation = (ImpactAnimationType)(Mathf.RoundToInt(dataRow.ImpactParams[index++]));
            }

            public int RepulseType { get; private set; }
            public float FallingFloatingTime { get; private set; }
            public float FallingFloatingDistance { get; private set; }
            public float DownTime { get; private set; }
            public ImpactAnimationType FallingAnimation { get; private set; }
        }

        public BlownAwayGround Ground { get; private set; }
        public BlownAwayAir Air { get; private set; }

        public override void SetDataRow(DRImpact dataRow)
        {
            base.SetDataRow(dataRow);
            Ground = new BlownAwayGround(dataRow);
            Air = new BlownAwayAir(dataRow);
        }
    }
}

using UnityEngine;

namespace Genesis.GameClient
{
    public class ApplyBlownAwayImpactData : BaseApplyImpactData
    {
        public CharacterMotionStateCategory StateCategory { get; set; }

        public ImpactAnimationType RaiseAnimationType { get; set; }

        public ImpactAnimationType FallingAnimationType { get; set; }

        public Vector3 FloatVelocity { get; set; }

        public Vector3 FloatFallingVelocity { get; set; }

        public float FloatTime { get; set; }

        public float FloatFallingTime { get; set; }

        public float DownTime { get; set; }

        public float RepulseStartTime { get; set; }

        public Vector3 RepulseVelocity { get; set; }

        public float RepulseTime { get; set; }

        public Vector3 RepulseDirection { get; set; }

        public void Fill(PBBlownAwayImpact pb)
        {
            StateCategory = (CharacterMotionStateCategory)pb.StateCategory;
            RaiseAnimationType = (ImpactAnimationType)pb.RaiseAnimationType;
            FallingAnimationType = (ImpactAnimationType)pb.FallingAnimationType;
            FloatVelocity = pb.FloatVelocity;
            FloatFallingVelocity = pb.FloatFallingVelocity;
            FloatTime = pb.FloatTime;
            FloatFallingTime = pb.FloatFallingTime;
            DownTime = pb.DownTime;
            RepulseStartTime = pb.RepulseStartTime;
            RepulseVelocity = pb.RepulseVelocity;
            RepulseTime = pb.RepulseTime;
            RepulseDirection = pb.RepulseDirection;
        }
    }
}

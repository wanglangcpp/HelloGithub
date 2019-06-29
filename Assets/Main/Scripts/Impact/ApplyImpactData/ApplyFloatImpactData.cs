using UnityEngine;

namespace Genesis.GameClient
{
    public class ApplyFloatImpactData : BaseApplyImpactData
    {
        public CharacterMotionStateCategory StateCategory { get; set; }

        public ImpactAnimationType RaiseAnimationType { get; set; }

        public ImpactAnimationType FallingAnimationType { get; set; }

        public Vector3 FloatVelocity { get; set; }

        public Vector3 FloatFallingVelocity { get; set; }

        public float FloatingTime { get; set; }

        public float DownTime { get; set; }

        public float RepulseStartTime { get; set; }

        public Vector3 RepulseVelocity { get; set; }

        public float StiffTime { get; set; }

        public Vector3 RepulseDirection { get; set; }

        public float FloatFallingTime { get; set; }

        public float RepulseTime { get; set; }

        public void Fill(PBFloatImpact pb)
        {
            StateCategory = (CharacterMotionStateCategory)pb.StateCategory;
            RaiseAnimationType = (ImpactAnimationType)pb.RaiseAnimationType;
            FallingAnimationType = (ImpactAnimationType)pb.FallingAnimationType;
            FloatVelocity = pb.FloatVelocity;
            FloatFallingVelocity = pb.FloatFallingVelocity;
            FloatingTime = pb.FloatingTime;
            DownTime = pb.DownTime;
            RepulseStartTime = pb.RepulseStartTime;
            RepulseVelocity = pb.RepulseVelocity;
            RepulseDirection = pb.RepulseDirection;
            StiffTime = pb.StiffTime;
            FloatFallingVelocity = pb.FloatFallingVelocity;
            RepulseTime = pb.RepulseTime;
        }
    }
}

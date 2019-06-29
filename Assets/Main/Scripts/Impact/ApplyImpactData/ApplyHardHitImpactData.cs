using UnityEngine;

namespace Genesis.GameClient
{
    public class ApplyHardHitImpactData : BaseApplyImpactData
    {
        public ImpactAnimationType ImpactAnimationType { get; set; }

        public ImpactAnimationType FallingAnimationType { get; set; }

        public Vector3 FloatFallingVelocity { get; set; }

        public float DownTime { get; set; }

        public float RepulseStartTime { get; set; }

        public Vector3 RepulseVelocity { get; set; }

        public float StiffTime { get; set; }

        public Vector3 RepulseDirection { get; set; }

        public float FloatFallingTime { get; set; }

        public float RepulseTime { get; set; }

        public CharacterMotionStateCategory StateCategory { get; set; }

        public void Fill(PBHardHitImpact pb)
        {
            ImpactAnimationType = (ImpactAnimationType)pb.ImpactAnimationType;
            FallingAnimationType = (ImpactAnimationType)pb.FallingAnimationType;
            FloatFallingVelocity = pb.FloatFallingVelocity;
            DownTime = pb.DownTime;
            RepulseStartTime = pb.RepulseStartTime;
            RepulseVelocity = pb.RepulseVelocity;
            StiffTime = pb.StiffTime;
            RepulseDirection = pb.RepulseDirection;
            FloatFallingTime = pb.FloatFallingTime;
            RepulseTime = pb.RepulseTime;
            StateCategory = (CharacterMotionStateCategory)pb.StateCategory;
        }
    }
}

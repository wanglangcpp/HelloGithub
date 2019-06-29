using UnityEngine;

namespace Genesis.GameClient
{
    public class ApplyStiffnessImpactData : BaseApplyImpactData
    {
        public CharacterMotionStateCategory StateCatetory { get; set; }

        public Vector3 RepulseDirection { get; set; }

        public Vector3 FloatFallingVelocity { get; set; }

        public float FloatFallingTime { get; set; }

        public float DownTime { get; set; }

        public float StiffTime { get; set; }

        public float RepulseStartTime { get; set; }

        public Vector3 RepulseVelocity { get; set; }

        public float RepulseTime { get; set; }

        public ImpactAnimationType ImpactAnimationType { get; set; }

        public ImpactAnimationType FallingAnimationType { get; set; }

        public void Fill(PBStiffnessImpact pb)
        {
            StateCatetory = (CharacterMotionStateCategory)pb.StateCategory;
            RepulseDirection = pb.RepulseDirection;
            FloatFallingVelocity = pb.FloatFallingSpeed;
            FloatFallingTime = pb.FloatFallingTime;
            DownTime = pb.DownTime;
            StiffTime = pb.StiffnessTime;
            RepulseStartTime = pb.RepulseStartTime;
            RepulseVelocity = pb.RepulseSpeed;
            RepulseTime = pb.RepulseTime;
            ImpactAnimationType = (ImpactAnimationType)pb.ImpactAnimationType;
            FallingAnimationType = (ImpactAnimationType)pb.FallingAnimationType;
        }
    }
}

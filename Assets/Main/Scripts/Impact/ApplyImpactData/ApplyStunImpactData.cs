namespace Genesis.GameClient
{
    public class ApplyStunImpactData : BaseApplyImpactData
    {
        public float StunTime { get; set; }

        public void Fill(PBStunImpact pb)
        {
            StunTime = pb.StunTime;
        }
    }
}

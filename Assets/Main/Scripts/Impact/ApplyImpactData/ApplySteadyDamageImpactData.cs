namespace Genesis.GameClient
{
    public class ApplySteadyDamageImpactData : BaseApplyImpactData
    {
        public float DamageSteady
        {
            get;
            set;
        }

        public void Fill(PBSteadyDamageImpact pb)
        {
            DamageSteady = pb.DamageSteady;
        }
    }
}

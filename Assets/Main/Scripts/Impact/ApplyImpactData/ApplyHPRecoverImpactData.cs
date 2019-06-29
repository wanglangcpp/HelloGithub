namespace Genesis.GameClient
{
    public class ApplyHPRecoverImpactData : BaseApplyImpactData
    {
        public int RecoverHP
        {
            get;
            set;
        }

        public void Fill(PBHPRecoverImpact pb)
        {
            RecoverHP = pb.RecoverHP;
        }
    }
}

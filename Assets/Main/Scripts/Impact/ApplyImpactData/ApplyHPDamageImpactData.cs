namespace Genesis.GameClient
{
    public class ApplyHPDamageImpactData : BaseApplyImpactData
    {
        public int DamageHP
        {
            get;
            set;
        }

        public int RecoverHP
        {
            get;
            set;
        }

        public int SkillRecoverHP
        {
            get;
            set;
        }

        public int CounterHP
        {
            get;
            set;
        }

        public bool IsCritical
        {
            get;
            set;
        }

        public void Fill(PBHPDamageImpact pb)
        {
            DamageHP = pb.DamageHP;
            RecoverHP = pb.RecoverHP;
            SkillRecoverHP = pb.SkillRecoverHP;
            CounterHP = pb.CounterHP;
            IsCritical = pb.IsCritical;
        }
    }
}

namespace Genesis.GameClient
{
    public class ApplyFreezeImpactData : BaseApplyImpactData
    {
        public CharacterMotionStateCategory StateCategory { get; set; }

        public float FreezeTime { get; set; }

        public float DownTime { get; set; }

        public void Fill(PBFreezeImpact pb)
        {
            StateCategory = (CharacterMotionStateCategory)pb.StateCategory;
            FreezeTime = pb.FreezeTime;
            DownTime = pb.DownTime;
        }
    }
}

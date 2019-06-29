namespace Genesis.GameClient
{
    public class SkillRushingParams
    {
        public float SpeedFactor { get; private set; }
        public bool AcceptDirInput { get; private set; }
        public float AngularSpeed { get; private set; }
        public bool ForbidRotate { get; private set; }
        public bool MoveOnlyOnDirInput { get; private set; }

        public static implicit operator SkillRushingParams(EntityRushTimeLineActionData timeLineActionData)
        {
            return new SkillRushingParams
            {
                SpeedFactor = timeLineActionData.SpeedFactor,
                AcceptDirInput = timeLineActionData.AcceptDirInput,
                AngularSpeed = timeLineActionData.AngularSpeed,
                ForbidRotate = timeLineActionData.ForbidRotate,
                MoveOnlyOnDirInput = timeLineActionData.MoveOnlyOnDirInput,
            };
        }
    }
}

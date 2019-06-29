using GameFramework;

namespace Genesis.GameClient
{
    public class EntityShowOrHideWeaponTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityShowOrHideWeaponTimeLineActionData m_Data;

        public EntityShowOrHideWeaponTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityShowOrHideWeaponTimeLineActionData;
        }

        public int[] ShowWeaponIndices
        {
            get
            {
                return m_Data.ShowWeaponIndices;
            }
        }

        public int[] HideWeaponIndices
        {
            get
            {
                return m_Data.HideWeaponIndices;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            HeroCharacter heroCharacter = timeLineInstance.Owner as HeroCharacter;

            if (heroCharacter == null)
            {
                Log.Error("Time line instance owner should be HeroCharacter but is a '{0}'.", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            for (int i = 0; i < HideWeaponIndices.Length; i++)
            {
                if (!heroCharacter.HideWeapon(HideWeaponIndices[i]))
                {
                    Log.Warning("Can not find weapon index '{0}' from hero '{1}'.", HideWeaponIndices[i].ToString(), heroCharacter.Data.HeroId.ToString());
                }
            }

            for (int i = 0; i < ShowWeaponIndices.Length; i++)
            {
                if (!heroCharacter.ShowWeapon(ShowWeaponIndices[i]))
                {
                    Log.Warning("Can not find weapon index '{0}' from hero '{1}'.", ShowWeaponIndices[i].ToString(), heroCharacter.Data.HeroId.ToString());
                }
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            Reset(timeLineInstance);
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {

        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            Reset(timeLineInstance);
        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        private void Reset(ITimeLineInstance<Entity> timeLineInstance)
        {
            HeroCharacter heroCharacter = timeLineInstance.Owner as HeroCharacter;

            if (heroCharacter == null)
            {
                Log.Error("Time line instance owner should be HeroCharacter but is a '{0}'.", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            for (int i = 0; i < ShowWeaponIndices.Length; i++)
            {
                heroCharacter.HideWeapon(ShowWeaponIndices[i]);
            }

            for (int i = 0; i < HideWeaponIndices.Length; i++)
            {
                heroCharacter.ShowWeapon(HideWeaponIndices[i]);
            }
        }
    }
}

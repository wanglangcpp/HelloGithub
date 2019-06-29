using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityWeaponAnimationTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityWeaponAnimationTimeLineActionData m_Data;

        public EntityWeaponAnimationTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityWeaponAnimationTimeLineActionData;
        }

        public int WeaponIndex
        {
            get
            {
                return m_Data.WeaponIndex;
            }
        }

        public string AnimationName
        {
            get
            {
                return m_Data.AnimationName;
            }
        }

        public float? Speed
        {
            get
            {
                return m_Data.Speed;
            }
        }

        public float? Time
        {
            get
            {
                return m_Data.Time;
            }
        }

        public float FadeLength
        {
            get
            {
                return m_Data.FadeLength == null ? 0f : Mathf.Max(0f, m_Data.FadeLength.Value);
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

            Weapon weapon = heroCharacter.GetWeapon(WeaponIndex);
            if (weapon == null)
            {
                Log.Warning("Can not find weapon index '{0}' from hero '{1}'.", WeaponIndex.ToString(), heroCharacter.Data.HeroId.ToString());
                return;
            }

            IDataTable<DRWeaponAnimation> dtWeaponAnimation = GameEntry.DataTable.GetDataTable<DRWeaponAnimation>();
            DRWeaponAnimation dataRow = dtWeaponAnimation.GetDataRow(weapon.Data.WeaponId);
            if (dataRow == null)
            {
                Log.Warning("Can not load animation from data table for weapon '{0}'.", weapon.Data.WeaponId.ToString());
                return;
            }

            string animationName = dataRow.GetAnimationName(AnimationName);
            if (string.IsNullOrEmpty(animationName))
            {
                Log.Warning("Can find animation alias '{0}' for weapon '{1}'.", AnimationName, weapon.Data.WeaponId.ToString());
                return;
            }

            AnimationState animationState = weapon.CachedAnimation[animationName];
            if (animationState == null)
            {
                Log.Warning("Can not find animation '{0}' for weapon '{1}'.", animationName, weapon.Data.WeaponId.ToString());
                return;
            }

            if (Speed.HasValue)
            {
                animationState.speed = Speed.Value;
            }

            if (Time.HasValue)
            {
                animationState.time = Time.Value;
            }

            if (FadeLength > 0f)
            {
                weapon.CachedAnimation.CrossFade(animationName, FadeLength);
            }
            else
            {
                weapon.CachedAnimation.Play(animationName);
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {

        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {

        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }
    }
}

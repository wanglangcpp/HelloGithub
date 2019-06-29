using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityAnimationTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityAnimationTimeLineActionData m_Data;

        public EntityAnimationTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityAnimationTimeLineActionData;
        }

        public string AnimationName
        {
            get
            {
                return m_Data.AnimationName;
            }
        }

        public AnimationBlendMode? BlendMode
        {
            get
            {
                return null;
            }
        }

        public int? Layer
        {
            get
            {
                return null;
            }
        }

        public float? NormalizedSpeed
        {
            get
            {
                return null;
            }
        }

        public float? NormalizedTime
        {
            get
            {
                return null;
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

        public float? Weight
        {
            get
            {
                return null;
            }
        }

        public WrapMode? WrapMode
        {
            get
            {
                return null;
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
            var animationInfo = GetAnimationInfo(timeLineInstance);
            if (animationInfo == null)
            {
                Log.Warning("Animation info is invalid.");
                return;
            }

            var animationState = animationInfo.State;
            if (animationState == null)
            {
                Log.Warning("Animation state is invalid.");
                return;
            }

            var animationName = animationInfo.Name;

            if (BlendMode.HasValue)
            {
                animationState.blendMode = BlendMode.Value;
            }

            if (Layer.HasValue)
            {
                animationState.layer = Layer.Value;
            }

            if (NormalizedSpeed.HasValue)
            {
                animationState.normalizedSpeed = NormalizedSpeed.Value;
            }

            if (NormalizedTime.HasValue)
            {
                animationState.normalizedTime = NormalizedTime.Value;
            }

            if (Speed.HasValue)
            {
                animationState.speed = Speed.Value;
            }

            if (Time.HasValue)
            {
                animationState.time = Time.Value;
            }

            if (Weight.HasValue)
            {
                animationState.weight = Weight.Value;
            }

            if (WrapMode.HasValue)
            {
                animationState.wrapMode = WrapMode.Value;
            }

            if (FadeLength > 0f)
            {
                timeLineInstance.Owner.CachedAnimation.CrossFade(animationName, FadeLength);
            }
            else
            {
                timeLineInstance.Owner.CachedAnimation.Play(animationName);
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

        private AnimationInfo GetAnimationInfo(ITimeLineInstance<Entity> timeLineInstance)
        {
            var owner = timeLineInstance.Owner;
            var character = owner as Character;
            if (character != null)
            {
                return GetAnimationInfoForCharacter(character);
            }

            Log.Warning("Owner should be a character, but its in fact a '{0}'.", owner == null ? "<null>" : owner.GetType().Name);
            return null;
        }

        private AnimationInfo GetAnimationInfoForCharacter(Character character)
        {
            var dtAnimation = GameEntry.DataTable.GetDataTable<DRAnimation>();
            DRAnimation dataRow = dtAnimation.GetDataRow(character.Data.CharacterId);
            if (dataRow == null)
            {
                Log.Warning("Can not load animation from data table for character '{0}'.", character.Data.CharacterId.ToString());
                return null;
            }

            string animationName = dataRow.GetAnimationName(AnimationName);
            if (string.IsNullOrEmpty(animationName))
            {
                Log.Warning("Can not find animation alias '{0}' for character '{1}'.", AnimationName, character.Data.CharacterId.ToString());
                return null;
            }

            return new AnimationInfo { Name = animationName, State = character.CachedAnimation[animationName] };
        }

        private class AnimationInfo
        {
            public string Name;
            public AnimationState State;
        }
    }
}

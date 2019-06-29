using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体时间轴行为基类。
    /// </summary>
    public abstract class EntityAbstractTimeLineAction : TimeLineAction<Entity>
    {
        public EntityAbstractTimeLineAction(TimeLineActionData data)
            : base(data)
        {

        }

        protected void FastForwardSelfAndCommonCD(ITimeLineInstance<Entity> timeLineInstance)
        {
            var amount = FastForwardSelf(timeLineInstance);

            if (amount <= 0f)
            {
                return;
            }

            var userDataDict = timeLineInstance.UserData as Dictionary<string, object>;
            userDataDict[Constant.TimeLineFastForwardTillKey] = timeLineInstance.CurrentTime;

            var heroCharacter = timeLineInstance.Owner as HeroCharacter;
            if (heroCharacter == null)
            {
                return;
            }

            heroCharacter.FastForwardCommonCoolDown(amount);

            MeHeroCharacter meHeroCharacter = timeLineInstance.Owner as MeHeroCharacter;
            if (GameEntry.Data.Room.InRoom && meHeroCharacter != null)
            {
                GameEntry.RoomLogic.FastForwardMySkill(meHeroCharacter, timeLineInstance.Id, timeLineInstance.CurrentTime);
            }
        }

        protected static bool TryGetUserData<T>(ITimeLineInstance<Entity> timeLineInstance, string key, out T val)
        {
            return timeLineInstance.TryGetUserData(key, out val);
        }
        protected static SkillBadgesData GetSkillBadges(ITimeLineInstance<Entity> timeLineInstance, int skillIndex)
        {
            if (skillIndex < 0)
            {
                return null;
            }

            var hero = timeLineInstance.Owner as HeroCharacter;
            if (hero != null)
            {
                return hero.Data.SkillsBadges[skillIndex];
            }

            var bullet = timeLineInstance.Owner as Bullet;
            if (bullet != null)
            {
                var heroData = AIUtility.GetFinalOwnerData(bullet.Data) as HeroData;
                if (heroData != null)
                {
                    hero = heroData.Entity as HeroCharacter;
                }
            }

            if (hero != null)
            {
                return hero.Data.SkillsBadges[skillIndex];
            }

            return null;
        }

        protected static int GetSkillLevel(ITimeLineInstance<Entity> timeLineInstance)
        {
            int skillLevel;
            if (!timeLineInstance.TryGetUserData(Constant.TimeLineSkillLevelKey, out skillLevel) && !timeLineInstance.TryGetUserData(Constant.TimeLineOwnerSkillLevelKey, out skillLevel))
            {
                skillLevel = 1;
            }

            return Mathf.Max(1, skillLevel);
        }
        protected static int GetSkillIndex(ITimeLineInstance<Entity> timeLineInstance)
        {
            int skillIndex;
            if (!timeLineInstance.TryGetUserData(Constant.TimeLineSkillIndexKey, out skillIndex) && !timeLineInstance.TryGetUserData(Constant.TimeLineOwnerSkillIndexKey, out skillIndex))
            {
                skillIndex = -1;
            }

            return skillIndex;
        }
    }
}

using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Check whether my (the current player's) AI is allowed to switch hero.")]
    public class MeCanSwitchHero : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            if (instanceLogic.IsSwitchingHero)
            {
                return TaskStatus.Failure;
            }

            if (instanceLogic.IsDuringCommonCoolDown)
            {
                return TaskStatus.Failure;
            }

            if (!AIUtility.AnyBgHeroIsAliveAndNotCoolingDown(instanceLogic.MyHeroesData))
            {
                return TaskStatus.Failure;
            }

            var currentHeroData = instanceLogic.MyHeroesData.CurrentHeroData;

            var entity = GameEntry.Entity.GetEntity(currentHeroData.Id);
            if (entity == null)
            {
                return TaskStatus.Failure;
            }

            var heroCharacter = entity.Logic as HeroCharacter;

            if (!heroCharacter.CanSwitchHero)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
    }
}

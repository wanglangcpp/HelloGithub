using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Check whether the opponent AI is allowed to switch hero.")]
    public class OppCanSwitchHero : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (!(GameEntry.SceneLogic.BaseInstanceLogic is BasePvpaiInstanceLogic))
            {
                Log.Warning("Not in an player v.s. player AI instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BasePvpaiInstanceLogic;
            if (instanceLogic.IsOppSwitchingHero)
            {
                return TaskStatus.Failure;
            }

            if (instanceLogic.IsDuringOppCommonCoolDown)
            {
                return TaskStatus.Failure;
            }

            if (!AIUtility.AnyBgHeroIsAliveAndNotCoolingDown(instanceLogic.OppHeroesData))
            {
                return TaskStatus.Failure;
            }

            var currentHeroData = instanceLogic.OppHeroesData.CurrentHeroData;

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

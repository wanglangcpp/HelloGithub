using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class OppNeedsAutoSwitchHero : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (!(GameEntry.SceneLogic.BaseInstanceLogic is BasePvpaiInstanceLogic))
            {
                Log.Warning("Not in a player v.s. player AI instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BasePvpaiInstanceLogic;
            if (instanceLogic.OppIsSwitchingHero)
            {
                return TaskStatus.Failure;
            }

            var oppHeroesData = instanceLogic.OppHeroesData;

            if (!oppHeroesData.AnyIsAlive)
            {
                return TaskStatus.Failure;
            }

            var currentHeroData = oppHeroesData.CurrentHeroData;

            var entity = GameEntry.Entity.GetEntity(currentHeroData.Id);
            if (entity == null)
            {
                return TaskStatus.Failure;
            }

            var heroCharacter = entity.Logic as HeroCharacter;
            if (heroCharacter == null || !heroCharacter.DeadKeepTimeIsReached)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
    }
}

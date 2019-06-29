using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class OwnedByMyHero : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            var myHero = Owner.GetComponent<MeHeroCharacter>();
            return myHero == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}

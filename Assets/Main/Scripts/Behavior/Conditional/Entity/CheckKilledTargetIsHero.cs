using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    public class CheckKilledTargetIsHero : Conditional
    {
        private TargetableObject m_Self = null;
        private ICanHaveTarget m_CanHaveTargetSelf = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
            m_CanHaveTargetSelf = m_Self as ICanHaveTarget;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_CanHaveTargetSelf == null)
            {
                GameFramework.Log.Warning("Self is not valid.");
                return TaskStatus.Failure;
            }

            var targetableObj = m_CanHaveTargetSelf.Target as TargetableObject;
            if (!m_CanHaveTargetSelf.HasTarget || targetableObj == null)
            {
                GameFramework.Log.Warning("Target is invalid.");
            }

            if (targetableObj.IsDead)
            {
                var meHeroes = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.GetHeroes();
                for (int i = 0; i < meHeroes.Length; i++)
                {
                    if (targetableObj.Id == meHeroes[i].Id)
                    {
                        return TaskStatus.Success;
                    }
                }
            }

            return TaskStatus.Failure;
        }
    }
}

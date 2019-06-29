using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 对手英雄角色实体。目前仅用于 <see cref="Genesis.GameClient.PlayerVSPlayerAIInstanceLogic"/> 类型的副本，且始终由 AI 控制。
    /// </summary>
    public class OppHeroCharacter : HeroCharacter
    {
        public override bool IsDuringCommonCoolDown
        {
            get
            {
                if (!IsInPlayerVSPlayerAIInstance)
                {
                    return false;
                }

                return GameEntry.SceneLogic.BasePvpaiInstanceLogic.IsDuringOppCommonCoolDown;
            }
        }

        public override void StartCommonCoolDown(float coolDownTime)
        {
            if (!IsInPlayerVSPlayerAIInstance)
            {
                return;
            }

            GameEntry.SceneLogic.BasePvpaiInstanceLogic.StartOppCommonCoolDown(coolDownTime);
        }

        public override void StopCommonCoolDown()
        {
            if (!IsInPlayerVSPlayerAIInstance)
            {
                return;
            }

            GameEntry.SceneLogic.BasePvpaiInstanceLogic.StopOppCommonCoolDown();
        }

        public override void FastForwardCommonCoolDown(float amount)
        {
            if (!IsInPlayerVSPlayerAIInstance)
            {
                return;
            }

            GameEntry.SceneLogic.BasePvpaiInstanceLogic.FastForwardOppCommonCoolDown(amount);
        }

        protected override bool ShouldLoadAI
        {
            get
            {
                return (GameEntry.SceneLogic.BaseInstanceLogic is BasePvpaiInstanceLogic);
            }
        }

        protected override void OnLoadBehaviorSuccess(object sender, GameEventArgs e)
        {
            LoadBehaviorSuccessEventArgs ne = e as LoadBehaviorSuccessEventArgs;
            if (ne.Behavior != Behavior)
            {
                return;
            }

            if (GameEntry.IsAvailable && !GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                Behavior.EnableBehavior();
            }

            Log.Debug("Load behavior '{0}' OK.", ne.BehaviorName);
        }

        private bool IsInPlayerVSPlayerAIInstance
        {
            get
            {
                return !(GameEntry.SceneLogic.BaseInstanceLogic is BasePvpaiInstanceLogic);
            }
        }
    }
}

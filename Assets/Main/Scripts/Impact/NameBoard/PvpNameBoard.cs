using UnityEngine;

namespace Genesis.GameClient
{
    public class PvpNameBoard : BaseNameBoard
    {
        private UISprite m_SteadyProgress = null;

        private UISprite m_RocoverBar = null;

        private UILabel m_OppLevel = null;

        protected override TargetType NameBoardType
        {
            get
            {
                return TargetType.Pvp;
            }
        }

        public override void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            base.RefreshNameBoard(entity, mode);

            if (m_NameBoard != null)
            {
                SetMeleeName(GameEntry.Data.PvpArenaOpponent.Player.Name);
                m_SteadyProgress = m_NameBoard.PvpSteadyBar;
                m_RocoverBar = m_NameBoard.RecoverBar;
                m_OppLevel = m_NameBoard.MeleeLevel;
                StartTime = Time.time;
                SetSteady();
            }
        }

        private void SetSteady()
        {
            var target = Owner as HeroCharacter;
            m_SteadyProgress.fillAmount = m_RocoverBar.fillAmount = target.Data.Steady.SteadyRatio;
            m_SteadyProgress.gameObject.SetActive(target.Data.Steady.IsSteadying);
            m_RocoverBar.gameObject.SetActive(!target.Data.Steady.IsSteadying);
            m_OppLevel.text = target.Data.MeleeLevel.ToString();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            SetSteady();
        }
    }
}

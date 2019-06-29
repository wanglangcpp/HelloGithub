using UnityEngine;

namespace Genesis.GameClient
{
    public class PvpSelfNameBoard : BaseNameBoard
    {
        private UISprite m_SteadyProgress = null;

        private UISprite m_RocoverBar = null;

        private UILabel m_MeleeLevel = null;

        protected override TargetType NameBoardType
        {
            get
            {
                return TargetType.PvpSelf;
            }
        }

        public override void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            base.RefreshNameBoard(entity, mode);

            if (m_NameBoard != null)
            {
                SetMyMeleeName(GameEntry.Data.Player.Name);
                m_SteadyProgress = m_NameBoard.PvpSelfSteadyBar;
                m_RocoverBar = m_NameBoard.PvpSelfRecoverBar;
                m_MeleeLevel = m_NameBoard.MyMeleeLevel;
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
            m_MeleeLevel.text = target.Data.MeleeLevel.ToString();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            SetSteady();
        }

    }
}

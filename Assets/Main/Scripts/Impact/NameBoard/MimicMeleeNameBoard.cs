using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class MimicMeleeNameBoard : BaseNameBoard
    {
        private UISprite m_SteadyProgress = null;

        private UISprite m_RocoverBar = null;

        private UILabel m_MeleeLevel = null;

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
                TargetableObject targetableObject = entity as TargetableObject;
                m_NameBoard.NameLabel.gameObject.SetActive(Character.Data.ShowName);
                m_NameBoard.SetNameColor(m_NameBoard.OtherNameColor);
                m_NameBoard.NameLabel.gameObject.SetActive(false);
                m_NameBoard.MeleeNameLabel.gameObject.SetActive(true);
                SetMeleeName(Character.Data.Name);
                if (targetableObject.Camp == CampType.Player)
                {
                    SetHPBarColor(m_HPColorNames["Green"]);
                }
                else
                {
                    SetHPBarColor(m_HPColorNames["Red"]);
                }
                m_SteadyProgress = m_NameBoard.PvpSteadyBar;
                m_RocoverBar = m_NameBoard.RecoverBar;
                m_MeleeLevel = m_NameBoard.MeleeLevel;
                StartTime = Time.time;
                SetSteady();
            }
        }

        private void SetSteady()
        {
            var target = Owner as NpcCharacter;
            m_MeleeLevel.text = target.Data.MeleeLevel.ToString();
            m_SteadyProgress.fillAmount = m_RocoverBar.fillAmount = target.Data.Steady.SteadyRatio;
            m_SteadyProgress.gameObject.SetActive(target.Data.Steady.IsSteadying);
            m_RocoverBar.gameObject.SetActive(!target.Data.Steady.IsSteadying);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            SetSteady();
        }
    }
}
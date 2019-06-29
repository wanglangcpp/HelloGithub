using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureSplash : ProcedureBase
    {
        private ProcedureConfig.ProcedureSplashConfig m_Config = null;
        private GameObject m_Splash = null;
        private Animation m_SplashAnimation = null;

        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Config = GameEntry.ClientConfig.ProcedureConfig.SplashConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ChangeState<ProcedureSelectPublisher>(procedureOwner);

            //m_Splash = NGUITools.AddChild(m_Config.ParentNode, m_Config.SplashTemplate);
            //m_Splash.name = m_Config.NodeName;
            //m_Splash.SetActive(true);
            //m_SplashAnimation = m_Splash.GetComponentInChildren<Animation>();
            //m_SplashAnimation.Rewind();
            //m_SplashAnimation.Play();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            m_Config.SplashTemplate = null;

            if (m_Splash != null)
            {
                GameObject.Destroy(m_Splash);
                m_Splash = null;

                GameEntry.Resource.ForceUnloadUnusedAssets(true);
            }

            if (GameEntry.IsAvailable && GameEntry.UIBackground != null)
            {
                GameEntry.UIBackground.HideBlank();
            }

            GameEntry.Resource.ForceUnloadUnusedAssets(false);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_SplashAnimation.isPlaying)
            {
                return;
            }

            ChangeState<ProcedureSelectPublisher>(procedureOwner);
        }
    }
}

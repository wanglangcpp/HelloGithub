using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Portal : ShedObject
    {
        [SerializeField]
        protected PortalData m_PortalData = null;

        private float m_TotalRatio = 0f;

        public new PortalData Data
        {
            get
            {
                return m_PortalData;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            ScenePortalTrigger trigger = GetComponent<ScenePortalTrigger>();
            trigger.ScenePortalTriggerEnter += OnScenePortalTriggerEnter;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PortalData = userData as PortalData;
            if (m_PortalData == null)
            {
                Log.Error("Portal data is invalid.");
                return;
            }

            if (m_PortalData.GuidePointGroupToActivateOnShow.HasValue)
            {
                GameEntry.SceneLogic.BaseInstanceLogic.GuidePointSet.ActivateGroup(m_PortalData.GuidePointGroupToActivateOnShow.Value);
            }

            GameEntry.Event.Subscribe(EventId.TriggerPortal, OnTriggerPortal);
            GameEntry.Event.Subscribe(EventId.PortagingEffect, ShowPortageEffect);
            CachedTransform.localScale = new Vector3(m_PortalData.Radius, 1f, m_PortalData.Radius);

            PortalParam[] portalParams = Data.PortalParams;
            for (int i = 0; i < portalParams.Length; i++)
            {
                m_TotalRatio += portalParams[i].Ratio;
            }
        }

        protected override void OnHide(object userData)
        {
            base.OnHide(userData);

            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(EventId.TriggerPortal, OnTriggerPortal);
            GameEntry.Event.Unsubscribe(EventId.PortagingEffect, ShowPortageEffect);
        }

        private void OnScenePortalTriggerEnter(Collider portalCollider, Collider targetCollider)
        {
            if (m_TotalRatio <= 0f)
            {
                return;
            }

            MeHeroCharacter meHeroCharacter = targetCollider.GetComponent<MeHeroCharacter>();
            if (meHeroCharacter == null)
            {
                return;
            }

            float ratio = Random.Range(0f, m_TotalRatio);
            PortalParam[] portalParams = Data.PortalParams;
            for (int i = 0; i < portalParams.Length; i++)
            {
                if (ratio <= portalParams[i].Ratio)
                {
                    GameEntry.Event.Fire(this, new TriggerPortalEventArgs(meHeroCharacter, Data.PortalId, portalParams[i].PortalId));
                    break;
                }

                ratio -= portalParams[i].Ratio;
            }
        }

        private void OnTriggerPortal(object sender, GameEventArgs e)
        {
            TriggerPortalEventArgs ne = e as TriggerPortalEventArgs;


            if (ne.FromPortalId == Data.PortalId)
            {
                OnTriggerFromProtal(ne);
            }
            else if (ne.ToPortalId == Data.PortalId)
            {
                OnTriggerToPortal(ne);
            }
        }

        private void OnTriggerToPortal(TriggerPortalEventArgs ne)
        {
            //Log.Info("Exiting portal '{0}'.", Data.PortalId.ToString());

            if (Data.GuidePointGroupToActivateOnExit.HasValue)
            {
                GameEntry.SceneLogic.BaseInstanceLogic.GuidePointSet.ActivateGroup(Data.GuidePointGroupToActivateOnExit.Value);
            }
        }

        private void OnTriggerFromProtal(TriggerPortalEventArgs ne)
        {
            IDataTable<DRPortal> dtPortal = GameEntry.DataTable.GetDataTable<DRPortal>();
            //Log.Info("Entering portal '{0}'.", Data.PortalId.ToString());

            var drPortal = dtPortal.GetDataRow(ne.ToPortalId);
            if (dtPortal == null)
            {
                Log.Warning("Can not find portal '{0}'.", Data.PortalId.ToString());
            }
            else
            {
                ne.Target.NavAgent.enabled = false;
                GameEntry.SceneLogic.BaseInstanceLogic.CameraController.StartPortage(ne.FromPortalId, ne.ToPortalId);
                ne.Target.CachedTransform.localPosition = AIUtility.SamplePosition(new Vector2(drPortal.PositionX, drPortal.PositionY));
                ne.Target.NavAgent.enabled = true;
            }
        }

        private void ShowPortageEffect(object sender, GameEventArgs e)
        {
            var effectData = e as PortagingEffectEventArgs;

            if (effectData == null)
                return;

            var dtPortal = GameEntry.DataTable.GetDataTable<DRPortal>();
            DRPortal drPortal = dtPortal.GetDataRow(effectData.PortalId);

            if (drPortal == null)
                return;

            string effectName = effectData.IsPrepareToPortage ? drPortal.BeforePortageEffectNameOnTrigger : drPortal.AfterPortageEffectNameOnTrigger;

            if (!string.IsNullOrEmpty(effectName))
            {
                var showData = new EffectData(GameEntry.Entity.GetSerialId(),
                    effectName,
                    effectData.EffectPosition,
                    effectData.EffectRotation);
                GameEntry.Entity.ShowEffect(showData);
            }
        }
    }
}

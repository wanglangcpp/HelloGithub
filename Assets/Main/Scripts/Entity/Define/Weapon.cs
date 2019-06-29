using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 武器。
    /// </summary>
    public class Weapon : Entity
    {
        [SerializeField]
        private WeaponData m_WeaponData = null;

        [SerializeField]
        private Transform m_OriginalTransform = null;

#pragma warning disable 0414

        [SerializeField]
        private Transform m_ParentTransform = null;

#pragma warning restore 0414

        private DRWeaponAnimation m_AnimationDataRow = null;

        private DRWeaponAnimationCrossFade m_AnimationCrossFadeDataRow = null;

        public new WeaponData Data
        {
            get
            {
                return m_WeaponData;
            }
        }

        public DRWeaponAnimation AnimationDataRow
        {
            get
            {
                return m_AnimationDataRow;
            }
        }

        public DRWeaponAnimationCrossFade AnimationCrossFadeDataRow
        {
            get
            {
                return m_AnimationCrossFadeDataRow;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            if (!HasOwner)
            {
                GameEntry.Entity.HideEntity(Entity);
                return;
            }

            if (GameEntry.SceneLogic.InstanceLogicType != InstanceLogicType.NonInstance && Owner is HeroCharacter)
            {
                GameEntry.Entity.AddLock(this);
            }

            var dt = GameEntry.DataTable.GetDataTable<DRWeaponAnimation>();
            m_AnimationDataRow = dt.GetDataRow(Data.WeaponId);
            if (m_AnimationDataRow == null)
            {
                Log.Warning("Weapon animation '{0}' not found.", Data.WeaponId.ToString());
                return;
            }

            IDataTable<DRWeaponAnimationCrossFade> dtAnimationCrossFade = GameEntry.DataTable.GetDataTable<DRWeaponAnimationCrossFade>();
            m_AnimationCrossFadeDataRow = dtAnimationCrossFade.GetDataRow(Data.WeaponId);
            if (m_AnimationCrossFadeDataRow == null)
            {
                Log.Warning("Weapon AnimationCrossFade '{0}' not found.", Data.WeaponId.ToString());
                return;
            }

            // Set Layer
            //CachedTransform.SetLayerRecursively(Data.ShowType == WeaponData.WeaponShowType.ForShow ? Constant.Layer.UIModelLayerId : Constant.Layer.DefaultLayerId);
            CachedTransform.SetLayerRecursively(Owner.Entity.gameObject.layer);
            m_OriginalTransform = CachedTransform.parent;
            GameEntry.Entity.AttachEntity(Entity, Owner.Entity, Data.AttachPointPath);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(object userData)
        {
            base.OnHide(userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            m_ParentTransform = parentTransform;
            ResetTransform();
            AttachToColorChanger();
            gameObject.SetActive(Data.VisibleByDefault);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            DetachFromColorChanger();
            m_ParentTransform = null;
            CachedTransform.parent = m_OriginalTransform;
            ResetTransform();
            base.OnDetachFrom(parentEntity, userData);
        }

        private void ResetTransform()
        {
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }

        private void AttachToColorChanger()
        {
            ColorChanger colorChanger = GetComponentInParent<ColorChanger>();
            if (colorChanger != null)
            {
                Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < renderers.Length; i++)
                {
                    colorChanger.AddRenderer(renderers[i]);
                }
            }
        }

        private void DetachFromColorChanger()
        {
            if (this.gameObject==null || transform.parent==null)
            {
                return;
            }
            ColorChanger colorChanger = GetComponentInParent<ColorChanger>();
            if (colorChanger != null)
            {
                Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < renderers.Length; i++)
                {
                    colorChanger.RemoveRenderer(renderers[i]);
                }
            }
        }
    }
}

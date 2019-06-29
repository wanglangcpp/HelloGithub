using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public abstract class Entity : EntityLogic
    {
        [SerializeField]
        private EntityData m_EntityData = null;

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        public EntityData Data
        {
            get
            {
                return m_EntityData;
            }
        }

        public Animation CachedAnimation
        {

            get;
            private set;
        }

        public EntityMotion Motion
        {
            get;
            protected set;
        }

        public Entity Owner
        {
            get;
            private set;
        }

        public bool HasOwner
        {
            get
            {
                return Owner != null && Owner.IsAvailable;
            }
        }

        public virtual void UpdateTransform(PBTransformInfo pb)
        {
            CachedTransform.localPosition = new Vector3(pb.PositionX, 0f, pb.PositionY);
            CachedTransform.eulerAngles = new Vector3(0f, pb.Rotation, 0f);
        }

        public virtual void PlayAliasedAnimation(string animClipAlias)
        {
            throw new System.NotSupportedException();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CachedAnimation = GetComponent<Animation>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EntityData = userData as EntityData;
            if (m_EntityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }

            if (m_EntityData.HasOwner)
            {
                UnityGameFramework.Runtime.Entity entity = GameEntry.Entity.GetEntity(m_EntityData.OwnerId);
                if (entity == null)
                {
                    //            Log.Warning("Owner is an invalid entity.");
                    return;
                }

                Owner = entity.Logic as Entity;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(object userData)
        {
            Owner = null;
            base.OnHide(userData);
        }
    }
}

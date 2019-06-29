using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class ShedObject : Entity
    {
        [SerializeField]
        protected ShedObjectData m_ShedObjectData = null;

        public new ShedObjectData Data
        {
            get
            {
                return m_ShedObjectData;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ShedObjectData = userData as ShedObjectData;
            if (m_ShedObjectData == null)
            {
                Log.Error("Shed object data is invalid.");
                return;
            }

            CachedTransform.localPosition = AIUtility.SamplePosition(m_ShedObjectData.Position, true);
            CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_ShedObjectData.Rotation, 0f));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}

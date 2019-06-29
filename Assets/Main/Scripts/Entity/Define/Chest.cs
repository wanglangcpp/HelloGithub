using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Chest : ShedObject
    {
        private const string OnShowAnimClipName = "basic_idle";

        private const float MoveMinDistance = 2f;
        private const float MoveMaxDistance = 3f;
        private const float MoveEndTime = 1f;
        private const float SinkStartTime = 3f;
        private const float SinkSpeed = -1f;
        private const float KeepTime = 6f;

        [SerializeField]
        protected ChestData m_ChestData = null;

        [SerializeField]
        private float m_KeepTime = 0f;

        [SerializeField]
        private Vector3 m_MoveSpeed = Vector3.zero;

        public new ChestData Data
        {
            get
            {
                return m_ChestData;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ChestData = userData as ChestData;
            if (m_ChestData == null)
            {
                Log.Error("Chest data is invalid.");
                return;
            }

            m_KeepTime = 0f;
            float radian = Random.Range(0f, 2f * Mathf.PI);
            m_MoveSpeed = new Vector3(Mathf.Cos(radian), 0f, Mathf.Sin(radian)).normalized * (MoveMinDistance + (MoveMaxDistance - MoveMinDistance) * Random.value) / MoveEndTime;

            CachedTransform.localPosition = AIUtility.SamplePosition(m_ChestData.Position);
            CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_ChestData.Rotation, 0f));

            CachedAnimation.Play(OnShowAnimClipName);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_KeepTime += elapseSeconds;

            if (m_KeepTime >= KeepTime)
            {
                GameEntry.Entity.HideEntity(Entity);
            }
            else if (m_KeepTime >= SinkStartTime)
            {
                CachedTransform.AddLocalPositionY(elapseSeconds * SinkSpeed);
            }
            else if (m_KeepTime < MoveEndTime)
            {
                CachedTransform.localPosition = CachedTransform.localPosition + m_MoveSpeed * elapseSeconds;
            }
        }
    }
}

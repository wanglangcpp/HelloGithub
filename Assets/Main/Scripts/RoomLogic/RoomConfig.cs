using UnityEngine;

namespace Genesis.GameClient
{
    public class RoomConfig : ScriptableObject
    {
        [SerializeField]
        private float m_PositionSyncThDist = 1f;

        public float PositionSyncThDist
        {
            get
            {
                return Mathf.Max(0f, m_PositionSyncThDist);
            }
        }

        [SerializeField]
        private float m_PositionGradualSyncMaxSpeed = 2f;

        public float PositionGradualSyncMaxSpeed
        {
            get
            {
                return Mathf.Max(0f, m_PositionGradualSyncMaxSpeed);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class InstanceRegionData
    {
        [SerializeField]
        private int m_Id;

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        [SerializeField]
        private Vector2 m_Center;

        public Vector2 Center
        {
            get
            {
                return m_Center;
            }
        }

        [SerializeField]
        private int[] m_BuffIdsToAddOnEnter = new int[0];

        public IList<int> GetBuffIdsToAddOnEnter()
        {
            return m_BuffIdsToAddOnEnter;
        }

        [SerializeField]
        private CampType[] m_CampsForRegionBuffs = new CampType[0];

        public IList<CampType> GetCampsForRegionBuffs()
        {
            return m_CampsForRegionBuffs;
        }

        public abstract bool Contains(Vector2 point);

        public void UpdateBuffIdsToAddOnEnter(IList<int> buffIds)
        {
            if (buffIds == null)
            {
                m_BuffIdsToAddOnEnter = new int[0];
            }

            m_BuffIdsToAddOnEnter = new List<int>(buffIds).ToArray();
        }

        public void UpdateCampsForRegionBuffs(IList<CampType> camps)
        {
            if (camps == null)
            {
                m_CampsForRegionBuffs = new CampType[0];
            }

            m_CampsForRegionBuffs = new List<CampType>(camps).ToArray();
        }

        public InstanceRegionData(int id, Vector2 center)
        {
            m_Id = id;
            m_Center = center;
        }
    }

    [Serializable]
    public class InstanceRegionData_Circle : InstanceRegionData
    {
        [SerializeField]
        private float m_Radius;

        public float Radius
        {
            get
            {
                return m_Radius;
            }
        }

        public override bool Contains(Vector2 point)
        {
            return Vector2.SqrMagnitude(point - Center) <= m_Radius * m_Radius;
        }

        public InstanceRegionData_Circle(int id, Vector2 center, float radius)
            : base(id, center)
        {
            m_Radius = Mathf.Max(radius, 0f);
        }
    }

    [Serializable]
    public class InstanceRegionData_Rect : InstanceRegionData
    {
        [SerializeField]
        private float m_Rotation;

        public float Rotation
        {
            get
            {
                return m_Rotation;
            }
        }

        [SerializeField]
        private float m_Width;

        public float Width
        {
            get
            {
                return m_Width;
            }
        }

        [SerializeField]
        private float m_Height;

        public float Height
        {
            get
            {
                return m_Height;
            }
        }

        private Vector2 m_RectForward;
        private Vector2 m_RectRight;

        public override bool Contains(Vector2 point)
        {
            var myDisplacement = point - Center;
            return (Mathf.Abs(Vector2.Dot(myDisplacement, m_RectForward)) < m_Height / 2f) && (Mathf.Abs(Vector2.Dot(myDisplacement, m_RectRight)) < m_Width / 2f);
        }

        public InstanceRegionData_Rect(int id, Vector2 center, float rotation, float width, float height)
            : base(id, center)
        {
            m_Rotation = rotation;
            m_Width = width;
            m_Height = height;

            var rotInRadians = Mathf.Deg2Rad * m_Rotation;
            var cos = Mathf.Cos(rotInRadians);
            var sin = Mathf.Sin(rotInRadians);

            m_RectRight = new Vector2(cos, -sin);
            m_RectForward = new Vector2(sin, cos);
        }
    }
}

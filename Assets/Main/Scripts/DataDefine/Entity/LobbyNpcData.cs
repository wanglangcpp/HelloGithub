using GameFramework;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class LobbyNpcData : EntityData
    {
        public LobbyNpcData(int entityId, int id)
            : base(entityId)
        {

            IDataTable<DRLobbyNpc> dtAnimation = GameEntry.DataTable.GetDataTable<DRLobbyNpc>();
            var row = dtAnimation.GetDataRow(id);
            if (row == null)
            {
                Log.Warning("LobbyNpc '{0}' not found.", id.ToString());
                return;
            }

            m_LobbyNpcId = id;
            m_Name = row.Name;
            Scale = row.Scale;
            m_ColliderRadius = row.ColliderRadius;
            m_ColliderHeight = row.ColliderHeight;
            m_ColliderCenterX = row.ColliderCenterX;
            m_ColliderCenterY = row.ColliderCenterY;
            m_ColliderCenterZ = row.ColliderCenterZ;
            Rotation = row.Rotation;
            m_StandAnimationName = row.Stand;
            m_IdleAnimationName = row.Idle;
            Position = new Vector2(row.PositionX, row.PositionY);
            m_OpenUIId = row.FormId;
            m_LobbyNpcButtonIcon = row.IconId;
        }

        [SerializeField]
        private int m_LobbyNpcId;

        [SerializeField]
        private string m_Name;

        [SerializeField]
        private float m_ColliderRadius;

        [SerializeField]
        private float m_ColliderHeight;

        [SerializeField]
        private float m_ColliderCenterX;

        [SerializeField]
        private float m_ColliderCenterY;

        [SerializeField]
        private float m_ColliderCenterZ;

        [SerializeField]
        private string m_StandAnimationName;

        [SerializeField]
        private string m_IdleAnimationName;

        [SerializeField]
        private int m_OpenUIId;

        [SerializeField]
        private int m_LobbyNpcButtonIcon;

        public int LobbyNpcId { get { return m_LobbyNpcId; } }
        public float ColliderCenterX { get { return m_ColliderCenterX; } }
        public float ColliderCenterY { get { return m_ColliderCenterY; } }
        public float ColliderCenterZ { get { return m_ColliderCenterZ; } }
        public float ColliderRadius { get { return m_ColliderRadius; } }
        public float ColliderHeight { get { return m_ColliderHeight; } }
        public string StandAnimationName { get { return m_StandAnimationName; } }
        public string IdleAnimationName { get { return m_IdleAnimationName; } }
        public string Name { get { return m_Name; } }
        public int OpenUIId { get { return m_OpenUIId; } }
        public int LobbyNpcButtonIcon { get { return m_LobbyNpcButtonIcon; } }
    }
}

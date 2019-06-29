using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class FakeCharacterData : EntityData
    {
        [SerializeField]
        private int m_CharacterId;

        [SerializeField]
        private int m_HeroId;

        [SerializeField]
        private Transform m_ParentTransform;

        [SerializeField]
        private ActionOnShow m_ActionOnShow;

        public new FakeCharacter Entity
        {
            get
            {
                return base.Entity as FakeCharacter;
            }
        }

        public int CharacterId
        {
            get
            {
                return m_CharacterId;
            }
        }

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
        }

        public int WeaponSuiteId
        {
            get
            {
                var dt = GameEntry.DataTable.GetDataTable<DRHero>();
                DRHero dr = dt.GetDataRow(m_HeroId);
                if (dr == null)
                {
                    return 0;
                }

                return dr.DefaultWeaponSuiteId;
            }
        }

        public Transform ParentTransform
        {
            get
            {
                return m_ParentTransform;
            }
        }


        public ActionOnShow ItsActionOnShow
        {
            get
            {
                return m_ActionOnShow;
            }
        }

        public FakeCharacterData(int entityId, int characterId, int heroId, Transform parentTransform, ActionOnShow actionOnShow)
            : base(entityId)
        {
            m_CharacterId = characterId;
            m_ParentTransform = parentTransform;
            m_HeroId = heroId;
            m_ActionOnShow = actionOnShow;
        }

        public enum ActionOnShow
        {
            None,
            Debut,
            DebutForReceive,
        }
    }
}

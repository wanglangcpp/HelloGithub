using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class WeaponData : EntityData
    {
        [SerializeField]
        private int m_WeaponId;

        [SerializeField]
        private int m_WeaponSuiteId;

        [SerializeField]
        private int m_WeaponIndexInSuite;

        [SerializeField]
        private string m_AttachPointPath;

        [SerializeField]
        private bool m_VisibleByDefault;

        [SerializeField]
        private WeaponShowType m_ShowType;

        public WeaponData(int entityId, int weaponId, int weaponSuiteId, int weaponIndexInSuite, string attachPointPath, bool visibleByDefault, WeaponShowType showType, int ownerId)
            : base(entityId)
        {
            m_WeaponId = weaponId;
            m_WeaponSuiteId = weaponSuiteId;
            m_WeaponIndexInSuite = weaponIndexInSuite;
            m_AttachPointPath = attachPointPath;
            m_VisibleByDefault = visibleByDefault;
            m_ShowType = showType;
            OwnerId = ownerId;
        }

        public new Weapon Entity
        {
            get
            {
                return base.Entity as Weapon;
            }
        }

        /// <summary>
        /// 模型编号（对应 Weapon 表编号）。
        /// </summary>
        public int WeaponId
        {
            get
            {
                return m_WeaponId;
            }
        }

        /// <summary>
        /// 武器套装编号。
        /// </summary>
        public int WeaponSuiteId
        {
            get
            {
                return m_WeaponSuiteId;
            }
        }

        /// <summary>
        /// 武器在套装中的位置。
        /// </summary>
        public int WeaponIndexInSuite
        {
            get
            {
                return m_WeaponIndexInSuite;
            }
        }

        /// <summary>
        /// 武器挂接位置。
        /// </summary>
        public string AttachPointPath
        {
            get
            {
                return m_AttachPointPath;
            }
        }

        /// <summary>
        /// 武器默认是否显示。
        /// </summary>
        public bool VisibleByDefault
        {
            get
            {
                return m_VisibleByDefault;
            }
        }

        /// <summary>
        /// 是否用于展示界面。
        /// </summary>
        public WeaponShowType ShowType
        {
            get
            {
                return m_ShowType;
            }
        }

        public enum WeaponShowType
        {
            Normal,
            Lobby,
            ForShow,
        }
    }
}

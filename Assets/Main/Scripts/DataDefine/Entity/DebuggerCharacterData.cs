using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class DebuggerCharacterData : CharacterData
    {
        [SerializeField]
        private string m_ResourceName;

        public string ResourceName
        {
            get
            {
                return m_ResourceName;
            }
            set
            {
                m_ResourceName = value;
            }
        }

        public DebuggerCharacterData(int entityId)
            : base(entityId)
        {

        }

        public new DebuggerCharacter Entity
        {
            get
            {
                return base.Entity as DebuggerCharacter;
            }
        }
    }
}

using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        [Serializable]
        private class HeroDisplay
        {
            [SerializeField]
            private Transform m_Platform = null;

            public Transform Platform
            {
                get
                {
                    return m_Platform;
                }
            }

            [SerializeField]
            private FakeCharacter m_Character = null;

            public FakeCharacter Character
            {
                get
                {
                    return m_Character;
                }
                set
                {
                    m_Character = value;
                }
            }

            [SerializeField]
            private UILabel m_Name = null;

            public UILabel Name
            {
                get
                {
                    return m_Name;
                }
            }

            [SerializeField]
            private UILabel m_Might = null;

            public UILabel Might
            {
                get
                {
                    return m_Might;
                }
            }
            
            [SerializeField]
            private UISprite m_Element = null;

            public UISprite Element
            {
                get
                {
                    return m_Element;
                }
            }            
        
            [SerializeField]
            private GameObject m_BottomPanel = null;

            public GameObject BottomPanel
            {
                get
                {
                    return m_BottomPanel;
                }
            }
        }
    }
}

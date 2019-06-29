using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CleanOutHeroStateChange
    {
        [Serializable]
        private class HeroItem
        {
            [SerializeField]
            public GameObject m_Root = null;

            [SerializeField]
            public UISprite m_Icon = null;

            [SerializeField]
            public UILabel m_Name = null;

            [SerializeField]
            public GameObject m_LevelUp = null;

            [SerializeField]
            public Animation m_LevelUpAnimation = null;

            [SerializeField]
            public UILevelUpProgressController m_LevelUpController = null;
        }
    }
}

using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        [Serializable]
        private class Player
        {
            
        }

        [Serializable]
        private class Hero
        {
            public Transform Root = null;
            public UISprite Portrait = null;
            public UILabel ExpPlus = null;
            public UILevelUpProgressController LevelUpProgress = null;
            public UISprite LevelUpMark = null;
            public UISprite Profession = null;

            private Animation m_LevelUpMarkAnimation = null;

            public Animation LevelUpMarkAnimation
            {
                get
                {
                    if (m_LevelUpMarkAnimation == null)
                    {
                        m_LevelUpMarkAnimation = LevelUpMark.GetComponent<Animation>();
                    }

                    return m_LevelUpMarkAnimation;
                }
            }
        }
    }
}

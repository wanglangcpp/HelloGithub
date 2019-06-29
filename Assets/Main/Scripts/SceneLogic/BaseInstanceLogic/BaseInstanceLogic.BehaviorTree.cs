using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseInstanceLogic
    {
        protected const string MyPlayerBehaviorName = "Scenes/MyPlayer";

        protected BehaviorTree m_MyPlayerBehavior = null;

        [SerializeField]
        private List<BehaviorTree> m_SceneBehaviors = new List<BehaviorTree>();

        protected void AddBehaviorTree(BehaviorTree bt)
        {
            m_SceneBehaviors.Add(bt);
        }

        private void ClearBehaviorTrees()
        {
            DisableBehaviorTrees();
            UnloadBehaviorTrees();
            m_SceneBehaviors.Clear();
        }

        private void UnloadBehaviorTrees()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            for (int i = 0; i < m_SceneBehaviors.Count; i++)
            {
                var behavior = m_SceneBehaviors[i];
                if (behavior == null || behavior.ExternalBehavior == null)
                {
                    continue;
                }

                GameEntry.Behavior.UnloadBehavior(behavior.ExternalBehavior);
                behavior.ExternalBehavior = null;
            }
        }

        protected void DisableBehaviorTrees()
        {
            for (int i = 0; i < m_SceneBehaviors.Count; ++i)
            {
                if (m_SceneBehaviors[i] != null)
                {
                    m_SceneBehaviors[i].DisableBehavior();
                }
            }
        }

        private void EnableBahaviorTrees()
        {
            if (!GameEntry.IsAvailable || GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                return;
            }

            for (int i = 0; i < m_SceneBehaviors.Count; ++i)
            {
                m_SceneBehaviors[i].EnableBehavior();
            }
        }

        protected void EnableMyPlayerAI()
        {
            if (!GameEntry.IsAvailable || GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                return;
            }

            m_MyPlayerBehavior.EnableBehavior();
        }

        protected void DisableMyPlayerAI()
        {
            m_MyPlayerBehavior.DisableBehavior();
        }

        protected void UnloadMyPlayerAI()
        {
            if (!GameEntry.IsAvailable || m_MyPlayerBehavior.ExternalBehavior == null)
            {
                return;
            }

            GameEntry.Behavior.UnloadBehavior(m_MyPlayerBehavior.ExternalBehavior);
            m_MyPlayerBehavior.ExternalBehavior = null;
        }

        protected void EnableMyHeroAI()
        {
            if (m_MeHeroCharacter == null)
            {
                return;
            }

            if (!GameEntry.IsAvailable || GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                return;
            }

            m_MeHeroCharacter.EnableAutoFight();
        }

        protected void DisableMyHeroAI()
        {
            if (m_MeHeroCharacter == null)
            {
                return;
            }

            m_MeHeroCharacter.DisableAutoFight();
        }
    }
}

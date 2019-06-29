using BehaviorDesigner.Runtime;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BasePvpaiInstanceLogic
    {
        private class Preparer : AbstractInstancePreparer
        {
            private List<BehaviorTree> m_BehaviorsToLoad = new List<BehaviorTree>();

            public override void Init(BaseInstanceLogic instanceLogic)
            {
                base.Init(instanceLogic);
                GameEntry.Event.Subscribe(EventId.InstanceMePrepared, OnMePrepared);
                m_BehaviorsToLoad.Clear();
            }

            public override void Shutdown(bool isExternalShutdown)
            {
                m_BehaviorsToLoad.Clear();

                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.InstanceMePrepared, OnMePrepared);
                }

                base.Shutdown(isExternalShutdown);
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(elapseSeconds, realElapseSeconds);
            }

            public override void OnLoadBehaviorSuccess(LoadBehaviorSuccessEventArgs e)
            {
                if (!m_BehaviorsToLoad.Contains(e.Behavior))
                {
                    return;
                }

                //Log.Debug("Load behavior '{0}' OK.", e.BehaviorName);
                var instanceLogic = (m_InstanceLogic as BasePvpaiInstanceLogic);
                if (e.Behavior != instanceLogic.m_MyPlayerBehavior)
                {
                    instanceLogic.AddBehaviorTree(e.Behavior);
                }
                m_BehaviorsToLoad.Remove(e.Behavior);

                if (m_BehaviorsToLoad.Count <= 0)
                {
                    m_InstanceLogic.PrepareAndShowMeHero();
                }
            }

            public override void OnLoadSceneSuccess(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e)
            {
                StartLoadingBehavior();
            }

            public override void OnOpenUIFormSuccess(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e)
            {
                GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());
            }

            public override void StartInstance()
            {
                FireShouldGoToWaiting();
            }

            private void OnMePrepared(object sender, GameEventArgs e)
            {
                GameEntry.Input.JoystickActivated = true;
                GameEntry.Input.SkillActivated = true;
                GameEntry.SceneLogic.OpenBattleForm();
            }

            private void StartLoadingBehavior()
            {
                var instanceLogic = m_InstanceLogic as BasePvpaiInstanceLogic;
                var behaviorNames = instanceLogic.m_InstanceDataRow.AIBehaviors;
                GameObject gameObject = new GameObject("Scene Logic");

                for (int i = 0; i < behaviorNames.Length; ++i)
                {
                    var behaviorName = behaviorNames[i];

                    var script = gameObject.AddComponent<BehaviorTree>();
                    script.StartWhenEnabled = false;
                    script.ExternalBehavior = null;
                    m_BehaviorsToLoad.Add(script);
                    GameEntry.Behavior.LoadBehavior(script, behaviorName);
                }

                instanceLogic.LoadMyPlayerAI(gameObject);
                m_BehaviorsToLoad.Add(instanceLogic.m_MyPlayerBehavior);
            }
        }
    }
}

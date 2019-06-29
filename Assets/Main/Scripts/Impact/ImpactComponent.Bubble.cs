using GameFramework;
using GameFramework.Event;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent : MonoBehaviour
    {
        [Serializable]
        private class Bubbles
        {
            [SerializeField]
            private Transform m_BubbleInstanceRoot = null;

            [SerializeField]
            private string m_TemplateName = null;

            [SerializeField]
            private int m_InstancePoolCapacity = 16;

            private Bubble m_Template = null;
            private IObjectPool<BubbleObject> m_BubbleObjects;
            private IList<BaseBubble> m_ActiveBubbles = new List<BaseBubble>();

            public bool PreloadComplete
            {
                get
                {
                    return m_Template != null;
                }
            }

            public void Init()
            {
                GameEntry.Event.Subscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);

                if (m_BubbleInstanceRoot == null)
                {
                    Log.Error("You must set Bubble instance root first.");
                    return;
                }

                m_BubbleObjects = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<BubbleObject>("Bubble", m_InstancePoolCapacity);
            }

            public void Shutdown()
            {
                Clear();

                if (!GameEntry.IsAvailable)
                {
                    return;
                }

                GameEntry.Event.Unsubscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
            }

            public void Preload()
            {
                PreloadUtility.LoadPreloadResource(m_TemplateName);
            }

            public void Update()
            {
                float time = Time.time;
                for (int i = m_ActiveBubbles.Count - 1; i >= 0; i--)
                {
                    BaseBubble bubble = m_ActiveBubbles[i];
                    bubble.OnUpdate();
                    if (bubble.Owner == null || !bubble.Owner.IsAvailable)
                    {
                        DestroyBubble(i);
                        continue;
                    }

                    if (!bubble.IsAvailable)
                    {
                        continue;
                    }

                    float currentTime = time - bubble.StartTime;
                    if (currentTime >= bubble.KeepTime)
                    {
                        DestroyBubble(i);
                    }
                }
            }

            public void Clear()
            {
                while (m_ActiveBubbles.Count > 0)
                {
                    DestroyBubble(0);
                }
            }

            public void DestroyBubble(int index)
            {
                m_ActiveBubbles[index].DestroyBubble(m_BubbleObjects);
                m_ActiveBubbles.RemoveAt(index);
            }

            public void DestroyBubble(BaseBubble bubble)
            {
                int index = m_ActiveBubbles.IndexOf(bubble);
                if (index >= 0)
                {
                    DestroyBubble(index);
                }
            }

            private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
            {
                LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
                if (ne.Name != m_TemplateName)
                {
                    return;
                }

                m_Template = (ne.Resource as GameObject).GetComponent<Bubble>();
            }

            public BaseBubble CreateBubble(TargetableObject targetableObject, string content, float startTime, float keepTime)
            {
                if (targetableObject == null)
                {
                    Log.Warning("Targetable object is invalid.");
                    return null;
                }

                BaseBubble bubble = GetBubble(targetableObject);
                if (bubble == null)
                {
                    bubble = CreateBubble(CreatBubbleObject(), targetableObject);
                }
                bubble.RefreshBubble(targetableObject, content, startTime, keepTime);
                return bubble;
            }

            public BaseBubble GetBubble(TargetableObject targetableObject)
            {
                for (int i = 0; i < m_ActiveBubbles.Count; ++i)
                {
                    if (m_ActiveBubbles[i].Owner == targetableObject)
                    {
                        return m_ActiveBubbles[i];
                    }
                }
                return null;
            }

            private BaseBubble CreateBubble(Bubble bubble, TargetableObject targetableObject)
            {
                if (bubble == null)
                {
                    Log.Warning("CreateBubble bubble object is invalid.");
                    return null;
                }

                BaseBubble baseBubble = null;
                var target = targetableObject as NpcCharacter;
                if (target != null)
                {
                    baseBubble = new NpcBubble();
                }

                baseBubble.Init(bubble);
                m_ActiveBubbles.Add(baseBubble);
                return baseBubble;
            }

            private Bubble CreatBubbleObject()
            {
                Bubble bubble = null;
                BubbleObject bubbleObject = m_BubbleObjects.Spawn();
                if (bubbleObject != null)
                {
                    bubble = bubbleObject.Target as Bubble;
                }
                else
                {
                    bubble = Instantiate(m_Template);
                    Transform transform = bubble.GetComponent<Transform>();
                    transform.SetParent(m_BubbleInstanceRoot);
                    transform.localScale = Vector3.one;
                    m_BubbleObjects.Register(new BubbleObject(bubble), true);
                }
                bubble.gameObject.SetActive(false);
                return bubble;
            }
        }
    }
}

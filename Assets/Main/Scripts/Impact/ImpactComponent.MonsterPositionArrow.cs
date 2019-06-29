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
        private class MonsterPositionArrows
        {
            [SerializeField]
            private Transform m_ArrowPromptInstanceRoot = null;

            [SerializeField]
            private string m_TemplateName = null;

            [SerializeField]
            private int m_InstancePoolCapacity = 10;

            private MonsterPositionArrow m_Template = null;
            private IObjectPool<MonsterPositionArrowObject> m_ArrowPromptObjects;
            private IList<BaseMonsterPositionArrow> m_ActiveArrowPrompts = new List<BaseMonsterPositionArrow>();
            private const int MaxActiveArrowCount = 10;

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

                if (m_ArrowPromptInstanceRoot == null)
                {
                    Log.Error("You must set ArrowPrompt instance root first.");
                    return;
                }

                m_ArrowPromptObjects = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<MonsterPositionArrowObject>("ArrowPrompt", m_InstancePoolCapacity);
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
                for (int i = m_ActiveArrowPrompts.Count - 1; i >= 0; i--)
                {
                    var arrowPrompt = m_ActiveArrowPrompts[i];
                    arrowPrompt.OnUpdate();

                    if (arrowPrompt.Owner == null || arrowPrompt.Owner.IsDead || !arrowPrompt.Owner.IsAvailable)
                    {
                        DestroyArrowPrompt(i);
                        continue;
                    }
                    Vector3 viewVec = Camera.main.WorldToViewportPoint(arrowPrompt.Owner.transform.position);
                    if (viewVec.x < 1 && viewVec.x > 0 && viewVec.y < 1 && viewVec.y > 0)
                    {
                        DestroyArrowPrompt(i);
                        continue;
                    }
                }
            }

            public void Clear()
            {
                while (m_ActiveArrowPrompts.Count > 0)
                {
                    DestroyArrowPrompt(0);
                }
            }

            public void DestroyArrowPrompt(int index)
            {
                m_ActiveArrowPrompts[index].DestroyArrowPrompt(m_ArrowPromptObjects);
                m_ActiveArrowPrompts.RemoveAt(index);
            }

            public void DestroyAllArrowPrompt()
            {
                for (int i = 0; i < m_ActiveArrowPrompts.Count; i++)
                {
                    m_ActiveArrowPrompts[i].DestroyArrowPrompt(m_ArrowPromptObjects);
                }
                m_ActiveArrowPrompts.Clear();
            }

            private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
            {
                LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
                if (ne.Name != m_TemplateName)
                {
                    return;
                }

                m_Template = (ne.Resource as GameObject).GetComponent<MonsterPositionArrow>();
            }

            public BaseMonsterPositionArrow CreateArrowPrompt(TargetableObject targetableObject)
            {
                if (m_ActiveArrowPrompts.Count >= MaxActiveArrowCount || GetArrowPrompt(targetableObject) != null)
                {
                    return null;
                }

                BaseMonsterPositionArrow arrowPrompt = null;
                var arrowPromptObject = CreatBubbleObject(targetableObject.transform.position);
                if (arrowPromptObject == null)
                {
                    Log.Warning("CreateArrowPrompt object is invalid.");
                    return null;
                }

                var target = targetableObject as Character;

                if (target == null)
                {
                    return null;
                }
                arrowPrompt = new EnemyMonsterPositionArrow();
                arrowPrompt.Init(arrowPromptObject);
                arrowPrompt.RefreshArrowPrompt(targetableObject);

                m_ActiveArrowPrompts.Add(arrowPrompt);

                return arrowPrompt;
            }

            public BaseMonsterPositionArrow GetArrowPrompt(TargetableObject targetableObject)
            {
                for (int i = 0; i < m_ActiveArrowPrompts.Count; ++i)
                {
                    if (m_ActiveArrowPrompts[i].Owner == targetableObject)
                    {
                        return m_ActiveArrowPrompts[i];
                    }
                }
                return null;
            }

            private MonsterPositionArrow CreatBubbleObject(Vector3 targetPosition)
            {
                MonsterPositionArrow arrowPrompt = null;
                MonsterPositionArrowObject arrowPromptObject = m_ArrowPromptObjects.Spawn();
                if (arrowPromptObject != null)
                {
                    arrowPrompt = arrowPromptObject.Target as MonsterPositionArrow;
                }
                else
                {
                    arrowPrompt = Instantiate(m_Template);
                    m_ArrowPromptObjects.Register(new MonsterPositionArrowObject(arrowPrompt), true);
                }
                Transform transform = arrowPrompt.GetComponent<Transform>();
                transform.localScale = Vector3.one;
                transform.SetParent(m_ArrowPromptInstanceRoot);
                arrowPrompt.gameObject.SetActive(true);
                return arrowPrompt;
            }
        }
    }
}

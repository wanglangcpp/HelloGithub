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
        private class NameBoards
        {
            [SerializeField]
            private Transform m_NameBoardInstanceRoot = null;

            [SerializeField]
            private string m_TemplateName = null;

            [SerializeField]
            private int m_InstancePoolCapacity = 16;

            private NameBoard m_Template = null;
            private IObjectPool<NameBoardObject> m_NameBoardObjects;
            private IList<BaseNameBoard> m_ActiveNameBoards = new List<BaseNameBoard>();

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

                if (m_NameBoardInstanceRoot == null)
                {
                    Log.Error("You must set HP bar instance root first.");
                    return;
                }

                m_NameBoardObjects = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<NameBoardObject>("NameBoard", m_InstancePoolCapacity);
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
                for (int i = m_ActiveNameBoards.Count - 1; i >= 0; i--)
                {
                    BaseNameBoard nameBoard = m_ActiveNameBoards[i];
                    nameBoard.OnUpdate();
                    if (nameBoard.Owner == null || !nameBoard.Owner.IsAvailable)
                    {
                        DestroyNameBoard(i);
                        continue;
                    }

                    var target = nameBoard.Owner as TargetableObject;
                    if (target != null && target.IsDead)
                    {
                        DestroyNameBoard(i);
                        continue;
                    }

                    if (nameBoard.Owner is LobbyNpc)
                    {
                        continue;
                    }

                    Building ownerBuilding = nameBoard.Owner as Building;
                    if (nameBoard.Mode == NameBoardMode.NameOnly || (nameBoard is PvpNameBoard) || (nameBoard is PvpSelfNameBoard) ||
                        ((ownerBuilding != null) && (ownerBuilding.HpBarRule == HPBarDisplayRule.AlwaysDisplay)))
                    {
                        continue;
                    }

                    NpcCharacter npc = nameBoard.Owner as NpcCharacter;
                    if (npc != null && npc.AlwaysShowHPBar)
                    {
                        continue;
                    }

                    float currentTime = time - nameBoard.StartTime;
                    nameBoard.SetAlpha(Mathf.Clamp01((Constant.HPBarKeepTime - currentTime) / Constant.HPBarAlphaTime));
                    if (currentTime >= Constant.HPBarKeepTime)
                    {
                        DestroyNameBoard(i);
                    }
                }
            }

            public void Clear()
            {
                while (m_ActiveNameBoards.Count > 0)
                {
                    DestroyNameBoard(0);
                }
            }

            public void SetNameBoardVisible(bool visible)
            {
                m_NameBoardInstanceRoot.gameObject.SetActive(visible);
            }

            public BaseNameBoard CreateNameBoard(Entity entity, NameBoardMode mode)
            {
                if (entity == null)
                {
                    Log.Warning("Targetable object is invalid.");
                    return null;
                }

                BaseNameBoard nameBoard = GetNameBoard(entity);
                if (nameBoard == null)
                {
                    nameBoard = CreateNameBoard(CreatNameboard(), entity);
                }
                nameBoard.RefreshNameBoard(entity, mode);
                return nameBoard;
            }

            public BaseNameBoard CreateNameBoard(Entity entity, NameBoardMode mode, float hpRatio, float animSeconds)
            {
                if (entity == null)
                {
                    Log.Warning("entity object is invalid.");
                    return null;
                }

                BaseNameBoard nameBoard = GetNameBoard(entity);
                if (nameBoard == null)
                {
                    nameBoard = CreateNameBoard(CreatNameboard(), entity);
                }
                nameBoard.RefreshNameBoard(entity, mode, hpRatio, animSeconds);
                return nameBoard;
            }

            private BaseNameBoard CreateNameBoard(NameBoard nameBoard, Entity entity)
            {
                if (nameBoard == null)
                {
                    Log.Warning("CreateNameBoard NameBoard object is invalid.");
                    return null;
                }

                BaseNameBoard baseNameBoard = null;
                var target = entity as NpcCharacter;
                var targetBuilding = entity as Building;
                if (targetBuilding != null)
                {
                    baseNameBoard = new BuildingNameBoard();
                }
                else if (target != null && target.Data.Category == NpcCategory.Elite)
                {
                    baseNameBoard = new EliteNameBoard();
                }
                else if (GameEntry.SceneLogic.BaseInstanceLogic.Type == InstanceLogicType.MimicMelee)
                {
                    if (entity is MeHeroCharacter)
                    {
                        baseNameBoard = new PvpSelfNameBoard();
                    }
                    else
                    {
                        if (target.Data.Category == NpcCategory.MimicMelee)
                        {
                            baseNameBoard = new MimicMeleeNameBoard { Character = target };
                        }
                        else
                        {
                            baseNameBoard = new NormalNameBoard { Character = target };
                        }
                    }
                }
                else if (entity is Character && GameEntry.SceneLogic.BaseInstanceLogic is BasePvpInstanceLogic)
                {
                    if (entity is MeHeroCharacter)
                    {
                        baseNameBoard = new PvpSelfNameBoard();
                    }
                    else
                    {
                        baseNameBoard = new PvpNameBoard();
                    }
                }
                else if (entity is LobbyNpc)
                {
                    baseNameBoard = new LobbyNpcNameBoard();
                }
                else
                {
                    baseNameBoard = new NormalNameBoard();
                }
                baseNameBoard.Init(nameBoard);
                m_ActiveNameBoards.Add(baseNameBoard);
                return baseNameBoard;
            }

            private NameBoard CreatNameboard()
            {
                NameBoard hpBar = null;
                NameBoardObject hpBarObject = m_NameBoardObjects.Spawn();
                if (hpBarObject != null)
                {
                    hpBar = hpBarObject.Target as NameBoard;
                    hpBar.gameObject.SetActive(true);
                }
                else
                {
                    hpBar = Instantiate(m_Template);
                    Transform transform = hpBar.GetComponent<Transform>();
                    transform.SetParent(m_NameBoardInstanceRoot);
                    transform.localScale = Vector3.one;
                    m_NameBoardObjects.Register(new NameBoardObject(hpBar), true);
                }
                return hpBar;
            }

            public BaseNameBoard GetNameBoard(Entity entity)
            {
                for (int i = 0; i < m_ActiveNameBoards.Count; ++i)
                {
                    if (m_ActiveNameBoards[i].Owner == entity)
                    {
                        return m_ActiveNameBoards[i];
                    }
                }

                return null;
            }

            public void DestroyNameBoard(int index)
            {
                m_ActiveNameBoards[index].DestroyNameBoard(m_NameBoardObjects);
                m_ActiveNameBoards.RemoveAt(index);
            }

            public void DestroyNameBoard(BaseNameBoard hpBar)
            {
                int index = m_ActiveNameBoards.IndexOf(hpBar);
                if (index >= 0)
                {
                    DestroyNameBoard(index);
                }
            }

            private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
            {
                LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
                if (ne.Name != m_TemplateName)
                {
                    return;
                }

                m_Template = (ne.Resource as GameObject).GetComponent<NameBoard>();
            }
        }
    }
}

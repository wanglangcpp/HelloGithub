#if UNITY_EDITOR

using GameFramework.DataTable;
using GameFramework.Debugger;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient
{
    public class TimeLineDebuggerWindow : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;

        private DebuggerCharacter m_DebuggerCharacter = null;
        private ITimeLineInstance<Entity> m_TimeLineInstance = null;
        private bool m_Loop = false;
        private int m_TimeLineId = 0;
        private string m_TimeLineIdString = string.Empty;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
        }

        public void OnLeave()
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            StopTimeLine();
            RemoveDebuggerCharacter();
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_TimeLineInstance != null && !m_TimeLineInstance.IsActive)
            {
                if (m_Loop)
                {
                    m_TimeLineInstance = GameEntry.TimeLine.Entity.CreateTimeLineInstance(m_DebuggerCharacter, m_TimeLineId, new Dictionary<string, object>());
                }
                else
                {
                    m_TimeLineInstance = null;
                }
            }
        }

        public void OnDraw()
        {
            if (!(GameEntry.Base.EditorResourceMode && GameEntry.OfflineMode.OfflineModeEnabled && GameEntry.Procedure.CurrentProcedure is ProcedureMain && GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance))
            {
                DrawTips();
                return;
            }

            if (m_DebuggerCharacter == null)
            {
                DrawAddCharacterMenu();
            }
            else
            {
                DrawPlayTimeLineMenu();
            }
        }

        private void DrawTips()
        {
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("<b><color=yellow>You cannot use this page currently.</color></b>");
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawAddCharacterMenu()
        {
            IDataTable<DRCharacter> dt = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter[] drs = dt.GetAllDataRows();
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                GUILayout.Label("<b>Select Character</b>");
                GUILayout.BeginVertical("box");
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (i % 4 == 0)
                        {
                            GUILayout.BeginHorizontal();
                        }

                        if (GUILayout.Button(drs[i].ResourceName, GUILayout.Height(30)))
                        {
                            GameEntry.Entity.ShowDebuggerCharacter(GetDebuggerCharacterData(drs[i]));
                        }

                        if (i % 4 == 3)
                        {
                            GUILayout.EndHorizontal();
                        }
                    }

                    if (drs.Length % 4 != 0)
                    {
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }

        private void DrawPlayTimeLineMenu()
        {
            GUILayout.Label("<b>Play Time Line</b>");
            GUILayout.BeginVertical("box");
            {
                int timeLineId = 0;
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Please Input Time Line Id:", GUILayout.Width(180f));
                    m_TimeLineIdString = GUILayout.TextField(m_TimeLineIdString);
                }
                GUILayout.EndHorizontal();

                EditorGUI.BeginDisabledGroup(!int.TryParse(m_TimeLineIdString, out timeLineId));
                {
                    if (GUILayout.Button("Play Once", GUILayout.Height(30f)))
                    {
                        PlayTimeLine(timeLineId, false);
                    }

                    if (GUILayout.Button("Play Repeated", GUILayout.Height(30f)))
                    {
                        PlayTimeLine(timeLineId, true);
                    }

                    if (GUILayout.Button("Stop Play", GUILayout.Height(30f)))
                    {
                        StopTimeLine();
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Back", GUILayout.Height(30f)))
                {
                    RemoveDebuggerCharacter();
                }
            }
            GUILayout.EndVertical();
        }

        private void PlayTimeLine(int timeLineId, bool loop)
        {
            StopTimeLine();
            m_Loop = loop;
            m_TimeLineId = timeLineId;
            m_TimeLineInstance = GameEntry.TimeLine.Entity.CreateTimeLineInstance(m_DebuggerCharacter, m_TimeLineId, new Dictionary<string, object>());
        }

        private void StopTimeLine()
        {
            m_Loop = false;
            m_TimeLineId = 0;
            if (m_TimeLineInstance != null && m_TimeLineInstance.IsActive && !m_TimeLineInstance.IsBroken)
            {
                m_TimeLineInstance.Break();
            }
        }

        private void RemoveDebuggerCharacter()
        {
            if (m_DebuggerCharacter == null)
            {
                return;
            }

            GameEntry.Entity.HideEntity(m_DebuggerCharacter.Id);
            m_DebuggerCharacter = null;
        }

        private DebuggerCharacterData GetDebuggerCharacterData(DRCharacter dr)
        {
            DebuggerCharacterData data = new DebuggerCharacterData(GameEntry.Entity.GetSerialId());
            data.CharacterId = dr.Id;
            data.ResourceName = dr.ResourceName;
            data.Position = GameEntry.SceneLogic.MeHeroCharacter.CachedTransform.localPosition.ToVector2();
            data.Rotation = GameEntry.SceneLogic.MeHeroCharacter.CachedTransform.localRotation.eulerAngles.y;
            data.Scale = 1f;
            return data;
        }

        private void OnShowEntitySuccess(object o, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            DebuggerCharacter debuggerCharacter = ne.Entity.Logic as DebuggerCharacter;
            if (debuggerCharacter == null)
            {
                return;
            }

            if (m_DebuggerCharacter != null)
            {
                GameEntry.Entity.HideEntity(m_DebuggerCharacter.Id);
            }

            m_DebuggerCharacter = debuggerCharacter;
        }
    }
}

#endif

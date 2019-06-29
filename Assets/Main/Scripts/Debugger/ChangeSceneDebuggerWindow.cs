using GameFramework.DataTable;
using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChangeSceneDebuggerWindow : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void OnDraw()
        {
            if (!(GameEntry.Procedure.CurrentProcedure is ProcedureMain))
            {
                DrawTips();
                return;
            }

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionChangeScene();
            }
            GUILayout.EndScrollView();
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

        private void DrawSectionChangeScene()
        {
            GUILayout.Label("<b>Change Scene</b>");
            GUILayout.BeginVertical("box");
            {
                IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
                DRScene[] scenes = dtScene.GetAllDataRows();
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (i % 3 == 0)
                    {
                        GUILayout.BeginHorizontal();
                    }

                    if (GUILayout.Button(string.Format("{0}. {1}", scenes[i].Id.ToString(), GameEntry.Localization.GetString(scenes[i].Name)), GUILayout.Height(30)))
                    {
                        if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
                        {
                            GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.NonInstance, scenes[i].Id, true));
                        }
                    }

                    if (i % 3 == 2)
                    {
                        GUILayout.EndHorizontal();
                    }
                }

                if (scenes.Length % 3 != 0)
                {
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

    }
}

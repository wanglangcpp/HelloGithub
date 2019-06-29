using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GMCommandDebuggerWindow_Hero : IDebuggerWindow
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
            if (!(GameEntry.Procedure.CurrentProcedure is ProcedureMain && !GameEntry.OfflineMode.OfflineModeEnabled))
            {
                DrawTips();
                return;
            }

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionAddHeroCommand();
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

        private void DrawSectionAddHeroCommand()
        {
            GUILayout.Label("<b>Add Hero</b>");
            GUILayout.BeginVertical("box");
            {
                DRHero[] dtHeros = GameEntry.DataTable.GetDataTable<DRHero>().GetAllDataRows();
                const int columnCount = 7;
                int rowCount = (dtHeros.Length + columnCount - 1) / columnCount;
                int index = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            if (index >= dtHeros.Length)
                            {
                                break;
                            }

                            if (GUILayout.Button(string.Format("{0}. {1}", dtHeros[index].Id.ToString(), GameEntry.Localization.GetString(dtHeros[index].Name)), GUILayout.Height(30)))
                            {
                                CLGMCommand request = new CLGMCommand();
                                request.Type = (int)GMCommandType.AddHero;
                                request.Params.Add(dtHeros[index].Id.ToString());
                                GameEntry.Network.Send(request);
                            }

                            index++;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
    }
}

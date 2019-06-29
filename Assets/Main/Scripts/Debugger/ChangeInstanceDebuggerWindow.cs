using GameFramework.DataTable;
using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChangeInstanceDebuggerWindow : IDebuggerWindow
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
                DrawSectionChangeInstance();
                DrawSectionBuff();
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

        private void DrawSectionChangeInstance()
        {
            GUILayout.Label("<b>Change Instance</b>");
            GUILayout.BeginVertical("box");
            {
                IDataTable<DRInstance> dtInstance = GameEntry.DataTable.GetDataTable<DRInstance>();
                DRInstance[] instances = dtInstance.GetAllDataRows();
                for (int i = 0; i < instances.Length; i++)
                {
                    if (i % 3 == 0)
                    {
                        GUILayout.BeginHorizontal();
                    }

                    if (GUILayout.Button(string.Format("{0}. {1}", instances[i].Id.ToString(), GameEntry.Localization.GetString(instances[i].Name)), GUILayout.Height(30)))
                    {
                        GameEntry.LobbyLogic.EnterInstance(instances[i].Id);
                    }

                    if (i % 3 == 2)
                    {
                        GUILayout.EndHorizontal();
                    }
                }

                if (instances.Length % 3 != 0)
                {
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionBuff()
        {
            GUILayout.Label("<b>Buff</b>");
            GUILayout.BeginVertical("box");
            {
                IDataTable<DRBuff> dtBuff = GameEntry.DataTable.GetDataTable<DRBuff>();
                DRBuff[] buffs = dtBuff.GetAllDataRows();
                for (int i = 0; i < buffs.Length; ++i)
                {
                    if (i % 5 == 0)
                    {
                        GUILayout.BeginHorizontal();
                    }

                    var buff = buffs[i];
                    if (GUILayout.Button(buff.Id.ToString(), GUILayout.Height(30)))
                    {
                        var meHeroCharacter = GameEntry.SceneLogic.MeHeroCharacter;
                        if (meHeroCharacter != null)
                        {
                            meHeroCharacter.AddBuff(buff.Id, null, OfflineBuffPool.GetNextSerialId(), null);
                        }
                    }

                    if (i % 5 == 4)
                    {
                        GUILayout.EndHorizontal();
                    }
                }

                if (buffs.Length % 5 != 0)
                {
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
    }
}

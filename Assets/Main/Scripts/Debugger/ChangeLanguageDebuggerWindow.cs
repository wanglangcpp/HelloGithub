using GameFramework.Debugger;
using GameFramework.Localization;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChangeLanguageDebuggerWindow : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;
        private bool m_NeedRestart = false;

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
            if (m_NeedRestart)
            {
                m_NeedRestart = false;
                GameEntry.Restart();
            }
        }

        public void OnDraw()
        {
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionChangeLanguage();
            }
            GUILayout.EndScrollView();
        }

        private void DrawSectionChangeLanguage()
        {
            GUILayout.Label("<b>Change Language</b>");
            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Chinese Simplified", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.ChineseSimplified;
                    GameEntry.Setting.SetString(Constant.Setting.Language, GameEntry.Localization.Language.ToString());
                    GameEntry.Setting.Save();
                    m_NeedRestart = true;
                }
                if (GUILayout.Button("Chinese Traditional", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.ChineseTraditional;
                    GameEntry.Setting.SetString(Constant.Setting.Language, GameEntry.Localization.Language.ToString());
                    GameEntry.Setting.Save();
                    m_NeedRestart = true;
                }
                if (GUILayout.Button("English", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.English;
                    GameEntry.Setting.SetString(Constant.Setting.Language, GameEntry.Localization.Language.ToString());
                    GameEntry.Setting.Save();
                    m_NeedRestart = true;
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}

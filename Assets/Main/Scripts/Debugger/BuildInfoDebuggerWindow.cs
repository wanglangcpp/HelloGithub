using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class BuildInfoDebuggerWindow : IDebuggerWindow
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
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                GUILayout.Label("<b>Build Info</b>");
                GUILayout.BeginVertical("box");
                {
                    BuildInfo buildInfo = GameEntry.BuildInfo;

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Git Hash:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.GitHash ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Git Branch:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.GitBranch ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Publisher:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.Publisher);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Check Version Uri:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.CheckVersionUri ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Update Resource Uri:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.UpdateResourceUri ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Check Server List Uri:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.CheckServerListUri ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Login Uri:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.LoginUri ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("iOS Application Url:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.IosAppUrl ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Android Application Url:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.AndroidAppUrl ?? string.Empty);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Inner Publisher Enabled:", GUILayout.Width(200));
                        GUILayout.Label(buildInfo.InnerPublisherEnabled.ToString());
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }
    }
}

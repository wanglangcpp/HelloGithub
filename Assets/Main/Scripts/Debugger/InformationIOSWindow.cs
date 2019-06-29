#if UNITY_IOS

using GameFramework.Debugger;
using UnityEngine;
using UnityEngine.iOS;

namespace Genesis.GameClient
{
    public class InformationIOSWindow : IDebuggerWindow
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
                GUILayout.Label("<b>iOS Information</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Generation:", GUILayout.Width(200));
                        GUILayout.Label(Device.generation.ToString());
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("System Version:", GUILayout.Width(200));
                        GUILayout.Label(Device.systemVersion);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Vendor Identifier:", GUILayout.Width(200));
                        GUILayout.Label(Device.vendorIdentifier);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Advertising Identifier:", GUILayout.Width(200));
                        GUILayout.Label(Device.advertisingIdentifier);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Advertising Tracking:", GUILayout.Width(200));
                        GUILayout.Label(Device.advertisingTrackingEnabled.ToString());
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }
    }
}

#endif

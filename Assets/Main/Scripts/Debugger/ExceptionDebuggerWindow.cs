using GameFramework;
using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ExceptionDebuggerWindow : IDebuggerWindow
    {
        private const float DefaultButtonHeight = 30f;

        private bool m_IsLoggingErrorContinuously = false;
        private Vector2 m_ScrollPosition;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {
            m_IsLoggingErrorContinuously = false;
        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_IsLoggingErrorContinuously)
            {
                Log.Error("You triggered a lot of error logs.");
            }
        }

        public void OnDraw()
        {
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                if (GUILayout.Button("Trigger Unity Error Log", GUILayout.Height(DefaultButtonHeight)))
                {
                    Log.Error("You triggered an error log.");
                }

                if (GUILayout.Button("Trigger Unity Fatal Log", GUILayout.Height(DefaultButtonHeight)))
                {
                    Log.Fatal("You triggered an error log.");
                }

                if (GUILayout.Button("Trigger C# Exception", GUILayout.Height(DefaultButtonHeight)))
                {
                    throw new System.Exception("This is a C# exception you triggered yourself.");
                }

                if (GUILayout.Button("Trigger Native Exception", GUILayout.Height(DefaultButtonHeight)))
                {
                    TriggerNativeException();
                }
            }
            GUILayout.EndScrollView();
        }

        private void TriggerNativeException()
        {
            new ExceptionNativeCaller().Trigger();
        }
    }
}

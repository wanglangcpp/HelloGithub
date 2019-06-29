using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class MemoryDebuggerWindow : IDebuggerWindow
    {
        private const float DefaultButtonHeight = 30f;

        private MemoryNativeCaller m_NativeCaller = new MemoryNativeCaller();
        private Vector2 m_ScrollPosition;

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
            GUILayout.Label(string.Format("Current memory: {0:F2} MB.", m_NativeCaller.UsedMemory / 1024f / 1024f));
            GUILayout.Label(string.Format("Free memory: {0:F2} MB.", m_NativeCaller.FreeMemory / 1024f / 1024f));

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {

                if (GUILayout.Button("Object Pool Release", GUILayout.Height(DefaultButtonHeight)))
                {
                    GameEntry.ObjectPool.Release();
                }

                if (GUILayout.Button("Object Pool Release All Unused", GUILayout.Height(DefaultButtonHeight)))
                {
                    GameEntry.ObjectPool.ReleaseAllUnused();
                }

                if (GUILayout.Button("Unload Unused Assets", GUILayout.Height(30f)))
                {
                    GameEntry.Resource.ForceUnloadUnusedAssets(false);
                }

                if (GUILayout.Button("Unload Unused Assets and Garbage Collect", GUILayout.Height(30f)))
                {
                    GameEntry.Resource.ForceUnloadUnusedAssets(true);
                }

                if (GUILayout.Button("Restart Game", GUILayout.Height(DefaultButtonHeight)))
                {
                    GameEntry.Restart();
                }

                if (GUILayout.Button("Shutdown Game", GUILayout.Height(DefaultButtonHeight)))
                {
                    GameEntry.Shutdown();
                }
            }
            GUILayout.EndScrollView();
        }
    }
}

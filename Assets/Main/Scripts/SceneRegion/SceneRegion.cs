using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public sealed class SceneRegion : MonoBehaviour
    {
        private static HashSet<string> s_LoadingSceneRegions = new HashSet<string>();

        [SerializeField]
        private string m_SceneRegionName = null;

        [SerializeField]
        private Collider[] m_SceneRegionTriggers = null;

        private SceneRegionState m_SceneRegionState = SceneRegionState.Hiding;
        private HashSet<Collider> m_OccupiedTriggers = new HashSet<Collider>();
        private bool m_Loading = false;

        public static int LoadingSceneRegionCount
        {
            get
            {
                return s_LoadingSceneRegions.Count;
            }
        }

        public static void Reset()
        {
            s_LoadingSceneRegions.Clear();
        }

        private void Start()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneFailure, OnLoadSceneFailure);

            if (string.IsNullOrEmpty(m_SceneRegionName))
            {
                Log.Warning("Scene region name is invalid.");
                return;
            }

            if (m_SceneRegionTriggers.Length <= 0)
            {
                Log.Warning("No region trigger exist in scene region.");
                return;
            }

            for (int i = 0; i < m_SceneRegionTriggers.Length; i++)
            {
                if (m_SceneRegionTriggers[i] == null)
                {
                    continue;
                }

                SceneRegionTrigger regionTrigger = m_SceneRegionTriggers[i].gameObject.AddComponent<SceneRegionTrigger>();
                regionTrigger.SceneRegionTriggerEnter += OnSceneRegionTriggerEnter;
                regionTrigger.SceneRegionTriggerExit += OnSceneRegionTriggerExit;
            }
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneFailure, OnLoadSceneFailure);
            }
        }

        private void Update()
        {
            if (m_SceneRegionState == SceneRegionState.ToHide && !m_Loading)
            {
                m_SceneRegionState = SceneRegionState.Hiding;
                GameEntry.Scene.UnloadScene(m_SceneRegionName);
            }
        }

        private void OnSceneRegionTriggerEnter(Collider trigger, Collider target)
        {
            if (target.GetComponent<MeHeroCharacter>() == null)
            {
                return;
            }

            m_OccupiedTriggers.Add(trigger);
            if (m_SceneRegionState != SceneRegionState.Showing)
            {
                m_SceneRegionState = SceneRegionState.Showing;
                if (!m_Loading)
                {
                    m_Loading = true;
                    s_LoadingSceneRegions.Add(m_SceneRegionName);
                    GameEntry.Scene.LoadScene(m_SceneRegionName, AssetUtility.GetSceneAsset(m_SceneRegionName), true);
                }
            }
        }

        private void OnSceneRegionTriggerExit(Collider trigger, Collider target)
        {
            if (target.GetComponent<MeHeroCharacter>() == null)
            {
                return;
            }

            m_OccupiedTriggers.Remove(trigger);
            if (m_SceneRegionState == SceneRegionState.Showing && m_OccupiedTriggers.Count <= 0)
            {
                m_SceneRegionState = SceneRegionState.ToHide;
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneSuccessEventArgs;
            if (ne.SceneName != m_SceneRegionName)
            {
                return;
            }

            m_Loading = false;
            s_LoadingSceneRegions.Remove(ne.SceneName);
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneFailureEventArgs;
            if (ne.SceneName != m_SceneRegionName)
            {
                return;
            }

            m_Loading = false;
            s_LoadingSceneRegions.Remove(ne.SceneName);
        }

        private enum SceneRegionState
        {
            Hiding,
            Showing,
            ToHide
        }
    }
}

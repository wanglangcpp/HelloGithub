using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 高亮投影器组件。用于控制主人公及其周围的高亮效果。
    /// </summary>
    public class HighlightProjectorComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ProjectorTemplate = null;

        [SerializeField]
        private GameObject m_ParentNode = null;

        [SerializeField]
        private string m_NodeName = "Highlight Projector";

        [SerializeField]
        private float m_OffsetYFromHero = 5f;

        private Projector m_CachedProjector = null;
        private Transform m_CachedTransform = null;

        [Serializable]
        private class ProjectorParam
        {
            internal float OffsetX;
            internal float OffsetZ;
            internal float Size;
            internal float HighlightFactor;
            internal Color HighlightColor;
        }

        private ProjectorParam m_ProjectorParam = null;

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedProjector = Instantiate(m_ProjectorTemplate).GetComponent<Projector>();
            m_CachedProjector.name = m_NodeName;
            m_CachedProjector.transform.parent = m_ParentNode.transform;
            m_CachedTransform = m_CachedProjector.transform;
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.ChangeSceneStart, OnChangeSceneStart);
            GameEntry.Event.Subscribe(EventId.ChangeSceneComplete, OnChangeSceneComplete);
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.ChangeSceneStart, OnChangeSceneStart);
                GameEntry.Event.Unsubscribe(EventId.ChangeSceneComplete, OnChangeSceneComplete);
            }
        }

        private void Update()
        {
            UpdateProjector();
        }

        private void OnEnable()
        {
            if (m_CachedTransform != null)
            {
                m_CachedTransform.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (m_CachedTransform != null)
            {
                m_CachedTransform.gameObject.SetActive(false);
            }
        }

        #endregion MonoBehaviour

        private void UpdateProjector()
        {
            var myHeroCharacter = GameEntry.SceneLogic.MeHeroCharacter;
            if (!myHeroCharacter || !myHeroCharacter.IsAvailable)
            {
                return;
            }

            m_CachedTransform.position = new Vector3(myHeroCharacter.CachedTransform.position.x + m_ProjectorParam.OffsetX,
                myHeroCharacter.CachedTransform.position.y + m_OffsetYFromHero,
                myHeroCharacter.CachedTransform.position.z + m_ProjectorParam.OffsetZ);
        }

        private void OnChangeSceneComplete(object sender, GameEventArgs e)
        {
            enabled = true;
            RefreshParams();
        }

        private void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            ClearParams();
            enabled = false;
        }

        private void ClearParams()
        {
            m_ProjectorParam = null;
        }

        private void RefreshParams()
        {
            string sceneName = null;
            string[] loadedSceneNames = GameEntry.Scene.GetLoadedSceneNames();
            for (int i = 0; i < loadedSceneNames.Length; i++)
            {
                if (loadedSceneNames[i].Contains("_"))
                {
                    continue;
                }

                sceneName = loadedSceneNames[i];
                break;
            }

            var sceneDRs = GameEntry.DataTable.GetDataTable<DRScene>().GetAllDataRows();

            DRScene dr = null;
            for (int i = 0; i < sceneDRs.Length; ++i)
            {
                if (sceneDRs[i].ResourceName == sceneName)
                {
                    dr = sceneDRs[i];
                    break;
                }
            }

            if (dr == null || dr.HighlightFactor <= 0f)
            {
                enabled = false;
            }
            else
            {
                m_ProjectorParam = new ProjectorParam
                {
                    OffsetX = dr.ProjectorOffsetX,
                    OffsetZ = dr.ProjectorOffsetY,
                    Size = dr.ProjectorSize,
                    HighlightFactor = dr.HighlightFactor,
                    HighlightColor = dr.HighlightColor
                };

                m_CachedProjector.orthographicSize = m_ProjectorParam.Size;
                var mat = new Material(m_CachedProjector.material);
                mat.SetColor("_MainColor", m_ProjectorParam.HighlightColor);
                mat.SetFloat("_HighlightFactor", m_ProjectorParam.HighlightFactor);
                m_CachedProjector.material = mat;

                UpdateProjector();
            }
        }
    }
}

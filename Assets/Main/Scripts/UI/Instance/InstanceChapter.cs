using UnityEngine;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本章节
    /// </summary>
    [Serializable]
    public class InstanceChapter : MonoBehaviour
    {
        [SerializeField]
        private UITexture m_BackgroundTexture = null;

        [SerializeField]
        private List<InstanceLevel> m_Levels = null;

        public GameObject MaskBackground = null;

        [SerializeField]
        private TweenAlpha m_MaskAlphaTween = null;

        [SerializeField]
        private TweenPosition m_PositionTween = null;

        [SerializeField]
        private Animation m_ChapterInAnimation = null;

        private const string LeftAnimationName = "ChapterSwitchingLeft";
        private const string RightAnimationName = "ChapterSwitchingRight";
        /// <summary>
        /// 缓存的章节中的关卡数据，Key是Prefab上绑定的UIIntKey，Value是Prefab上的Object
        /// </summary>
        private Dictionary<int, InstanceLevel> m_LevelMap;

        private InstanceGroupData m_ChapterData;
        public InstanceGroupData ChapterData
        {
            get { return m_ChapterData; }
        }

        public float BackgroundWidth
        {
            get { return m_BackgroundTexture.localSize.x; }
        }

        /// <summary>
        /// 绑定在Prefab上的，对应上章节的Index（与策划确定好的，ID就是顺序）
        /// </summary>
        public UIIntKey ChapterIndex
        {
            get
            {
                return GetComponent<UIIntKey>();
            }
        }

        private void InitLevelFlags()
        {
            if (m_LevelMap == null)
            {
                m_LevelMap = new Dictionary<int, InstanceLevel>();

                for (int i = 0; i < m_Levels.Count; i++)
                {
                    m_Levels[i].InitializeLevel();
                    var keyFlag = m_Levels[i].GetComponent<UIIntKey>();
                    // 这里要是报Key重复错，说明Prefab上绑的IntKey是有重复的，是不合法的
                    m_LevelMap.Add(keyFlag.Key, m_Levels[i]);
                }
            }
        }

        /// <summary>
        /// 数据填充规则：
        ///     1、找到第一个关卡开始的ID。方法是这个章节组里的前置关卡ID没有本关卡的ID，那么本关卡就是第一个关卡。
        ///     2、然后通过第一个关卡里配置的InstanceKeyNumber，找到对应的（UIIntKey）Object。
        ///     3、再然后通过当前的ID找到前置关卡ID是本关卡ID的所有关，然后重复步骤2
        ///     4、以数据为准，数据配的条目多少显示多少。
        /// </summary>
        /// <param name="chapterData"></param>
        public void SetChapterData(InstanceGroupData chapterData)
        {
            InitLevelFlags();

            m_ChapterData = chapterData;
            var firstLevelData = chapterData.FirstLevelData;

            int index = firstLevelData.LevelConfig.InstanceKeyNumber;
            InstanceLevel levelObject = null;

            m_LevelMap.TryGetValue(index, out levelObject);
            SetLevel(levelObject, firstLevelData);
        }

        public void LoadBackgroundTexture()
        {
            m_BackgroundTexture.LoadAsync(m_ChapterData.ChapterConfig.ChapterIconName, null, null, null);
        }

        public void UnloadBackgroundTexture()
        {
            m_BackgroundTexture.mainTexture = null;
            NGUIExtensionMethods.ReleaseTextureIfNeeded(m_BackgroundTexture.GetHashCode());
            GameEntry.Resource.ForceUnloadUnusedAssets(true);
        }

        public void PlayAnimation(bool isFromLeftToRight, bool isIn)
        {
            m_PositionTween.ResetToBeginning();
            
            Vector3 from;
            Vector3 to;

            if(isIn)
            {
                if (isFromLeftToRight)
                    from = new Vector3(-BackgroundWidth, 0f, 0f);
                else
                    from = new Vector3(BackgroundWidth, 0f, 0f);

                to = Vector3.zero;

                m_PositionTween.onFinished.Clear();
                m_PositionTween.SetOnFinished(() =>
                {
                    m_ChapterInAnimation["InstanceItemAllIn"].time = 0;
                    m_ChapterInAnimation["InstanceItemAllIn"].speed = 1;
                    m_ChapterInAnimation.Play("InstanceItemAllIn");
                });
            }
            else
            {
                if (isFromLeftToRight)
                    to = new Vector3(BackgroundWidth, 0f, 0f);
                else
                    to = new Vector3(-BackgroundWidth, 0f, 0f);

                from = Vector3.zero;

                m_PositionTween.onFinished.Clear();
                m_PositionTween.SetOnFinished(()=>
                {
                    gameObject.SetActive(false);
                    UnloadBackgroundTexture();
                });
            }

            m_PositionTween.from = from;
            m_PositionTween.to = to;
            m_PositionTween.duration = 0.7f;
 
            m_PositionTween.PlayForward();
        }

        public void BackGroundFadeOut()
        {
            m_MaskAlphaTween.ResetToBeginning();
            m_MaskAlphaTween.duration = 1f;
            m_MaskAlphaTween.PlayForward();
        }

        /// <summary>
        /// 递归填充所有关卡（如果效率低的话，这里优化一下）
        /// </summary>
        /// <param name="chapterData">所属章节数据</param>
        /// <param name="levelObject">关卡Object</param>
        /// <param name="levelData">关卡数据</param>
        private void SetLevel(InstanceLevel levelObject, InstanceGroupData.InstanceData levelData)
        {
            if (levelObject == null || levelData == null)
                return;

            levelObject.SetLevelData(levelData);
//#if UNITY_EDITOR
            levelObject.name = string.Format("Chapter {0} Level {1} IntKey {2}", m_ChapterData.ChapterId, levelData.Id, levelData.LevelConfig.InstanceKeyNumber);
//#endif
            var nextLevels = m_ChapterData.GetNextLevelId(levelData.Id);
            for (int i = 0; i < nextLevels.Count; i++)
            {
                var data = m_ChapterData.GetLevelDataById(nextLevels[i]);
                InstanceLevel obj = null;
                m_LevelMap.TryGetValue(data.LevelConfig.InstanceKeyNumber, out obj);
                SetLevel(obj, data);
            }
        }
    }
}

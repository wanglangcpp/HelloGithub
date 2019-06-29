using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    public class InstanceSelectForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_ChapterNameLabel = null;

        [SerializeField]
        private UIProgressBar m_CurrentChapterProgress = null;

        [SerializeField]
        private UILabel m_CurrentStarCountLabel = null;

        [SerializeField]
        private RewardChest m_FirstBox = null;

        [SerializeField]
        private RewardChest m_SecondBox = null;

        [SerializeField]
        private RewardChest m_ThirdBox = null;

        [SerializeField]
        private UIButton m_LeftPageButton = null;

        [SerializeField]
        private UIButton m_RightPageButton = null;

        [SerializeField]
        private List<InstanceChapter> m_Chapters = null;

        [SerializeField]
        private Transform m_LeftMaskTransform = null;

        [SerializeField]
        private Transform m_RightMaskTransform = null;

        [SerializeField]
        private Animation m_TitleAnimation = null;

        [SerializeField]
        private Animation m_ChestAnimation = null;

        private Dictionary<int, InstanceChapter> m_ChaptersMap = null;
        private InstanceChapter m_CurrentShowingChapter = null;

        private Vector3 m_MaskPosition = Vector3.zero;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(EventId.OpenInstanceGroupChest, OnOpenChest);

            if (m_ChaptersMap == null)
            {
                m_ChaptersMap = new Dictionary<int, InstanceChapter>();
                for (int i = 0; i < m_Chapters.Count; i++)
                {
                    m_Chapters[i].gameObject.SetActive(false);
                    var intKey = m_Chapters[i].ChapterIndex;
                    m_ChaptersMap.Add(intKey.Key, m_Chapters[i]);
                }
            }

            SetChapterAndShow(GameEntry.Data.InstanceGroups.GetCurrentChapterData(), false);
        }

        private void OnOpenChest(object sender, GameEventArgs e)
        {
            var eventData = e as OpenInstanceGroupChestEventArgs;

            if (eventData == null)
                return;

            var chapterData = m_CurrentShowingChapter.ChapterData;
            if (eventData.ChestIndex == 0)
            {
                m_FirstBox.SetBoxData(chapterData.ChapterConfig, eventData.ChestIndex, chapterData.CurrentStarCount);
            }
            else if (eventData.ChestIndex == 1)
            {
                m_SecondBox.SetBoxData(chapterData.ChapterConfig, eventData.ChestIndex, chapterData.CurrentStarCount);
            }
            else if (eventData.ChestIndex == 2)
            {
                m_ThirdBox.SetBoxData(chapterData.ChapterConfig, eventData.ChestIndex, chapterData.CurrentStarCount);
            }
            else
                return;

            GameEntry.RewardViewer.RequestShowRewards(eventData.RewardShowHelper.ReceiveGoodsData, true);
        }

        private void SetChapterAndShow(InstanceGroupData chapterData, bool showAnimation = true, bool isFromLeftToRight = true)
        {
            InstanceChapter selectedChapter = null;

            if (!m_ChaptersMap.TryGetValue(chapterData.ChapterId, out selectedChapter))
            {
                Log.Error("Error occured in prefab InstanceSelectForm, can not find IntKey is '{0}' in prefab");
                return;
            }

            selectedChapter.SetChapterData(chapterData);
            selectedChapter.LoadBackgroundTexture();
            selectedChapter.gameObject.SetActive(true);

            SetArrowState(selectedChapter.ChapterIndex.Key, GameEntry.Data.InstanceGroups.GetCurrentChapterData().ChapterId > chapterData.ChapterId);
            SetMaskParent(selectedChapter.transform);

            selectedChapter.MaskBackground.SetActive(true);
            int willShowIndex = m_Chapters.IndexOf(selectedChapter);
            if (showAnimation)
            {
                PlayAnimation(chapterData, willShowIndex, isFromLeftToRight);
            }
            else
            {
                m_Chapters[willShowIndex].transform.localPosition = Vector3.zero;
                m_Chapters[willShowIndex].MaskBackground.SetActive(false);

                m_ChapterNameLabel.text = GameEntry.Localization.GetString(chapterData.ChapterConfig.Name);
                m_CurrentStarCountLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", chapterData.CurrentStarCount, chapterData.TotalStarCount);
                SetBoxStatus(chapterData);
            }
            m_CurrentShowingChapter = selectedChapter;
        }

        private void SetArrowState(int currentIndex, bool showRightArrow)
        {
            if (currentIndex == m_Chapters[0].ChapterIndex.Key)
            {
                m_LeftPageButton.gameObject.SetActive(false);
                m_LeftPageButton.isEnabled = false;
                m_RightPageButton.isEnabled = true;
                m_RightPageButton.gameObject.SetActive(showRightArrow);
            }
            else if (currentIndex == m_Chapters[m_Chapters.Count - 1].ChapterIndex.Key)
            {
                m_LeftPageButton.isEnabled = true;
                m_RightPageButton.isEnabled = false;
                m_RightPageButton.gameObject.SetActive(false);
            }
            else
            {
                m_LeftPageButton.isEnabled = true;
                m_RightPageButton.isEnabled = true;
                m_LeftPageButton.gameObject.SetActive(true);
                m_RightPageButton.gameObject.SetActive(showRightArrow);
            }
        }

        private void SetBoxStatus(InstanceGroupData chapterData)
        {
            m_CurrentChapterProgress.value = (float)chapterData.CurrentStarCount / chapterData.TotalStarCount;

            m_FirstBox.SetBoxData(chapterData.ChapterConfig, 0, chapterData.CurrentStarCount);
            m_SecondBox.SetBoxData(chapterData.ChapterConfig, 1, chapterData.CurrentStarCount);
            m_ThirdBox.SetBoxData(chapterData.ChapterConfig, 2, chapterData.CurrentStarCount);
        }

        private void SetMaskParent(Transform parentTransform)
        {
            m_LeftMaskTransform.SetParent(parentTransform);
            m_LeftMaskTransform.localPosition = CalculateMaskPosition(true);
            m_RightMaskTransform.SetParent(parentTransform);
            m_RightMaskTransform.localPosition = CalculateMaskPosition(false);
        }

        private void PlayAnimation(InstanceGroupData chapterData, int willShowIndex, bool isFromLeftToRight)
        {
            if (isFromLeftToRight)
            {
                m_Chapters[willShowIndex + 1].PlayAnimation(isFromLeftToRight, false);
                m_Chapters[willShowIndex].PlayAnimation(isFromLeftToRight, true);
                m_Chapters[willShowIndex].BackGroundFadeOut();
            }
            else
            {
                m_Chapters[willShowIndex - 1].PlayAnimation(isFromLeftToRight, false);
                m_Chapters[willShowIndex].PlayAnimation(isFromLeftToRight, true);
                m_Chapters[willShowIndex].BackGroundFadeOut();
            }

            m_TitleAnimation.Stop();
            m_ChestAnimation.Stop();
            m_TitleAnimation.Play();
            m_ChestAnimation.Play();

            // 这里为什么这么写：副本章节的宝箱跟标题在切换时需要播放动画，动画是Alpha 1-0-1的过程。
            // 策划想要在Alpha为0的时候再改变标题的内容，因此实际上是用Tween做了一个延时的作用。
            // 0.25f是测出来的时间，开始用Animation.clip.length / 2来计算，后来发现这个动画的时间并不是前后对称的。
            var tween = gameObject.GetComponent<TweenPosition>();
            if (tween == null)
                tween = gameObject.AddComponent<TweenPosition>();

            tween.ResetToBeginning();

            tween.from = Vector3.one;
            tween.to = Vector3.one;
            tween.duration = 0.25f;

            tween.onFinished.Clear();
            tween.SetOnFinished(() =>
            {
                m_ChapterNameLabel.text = GameEntry.Localization.GetString(chapterData.ChapterConfig.Name);
                m_CurrentStarCountLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", chapterData.CurrentStarCount, chapterData.TotalStarCount);
                SetBoxStatus(chapterData);
            });
            tween.PlayForward();
        }

        private Vector3 CalculateMaskPosition(bool isLeft)
        {
            if (m_MaskPosition == Vector3.zero)
            {
                var bgSize = m_Chapters[0].BackgroundWidth;
                var maskSize = NGUIMath.CalculateRelativeWidgetBounds(m_LeftMaskTransform).size;
                m_MaskPosition = new Vector3(bgSize / 2 + maskSize.x / 2, m_LeftMaskTransform.localPosition.y, m_LeftMaskTransform.localPosition.z);
            }

            if (isLeft)
                return -m_MaskPosition;
            else
                return m_MaskPosition;
        }

        public void OnLeftPageButtonClicked()
        {
            SetChapterAndShow(GameEntry.Data.InstanceGroups.GetChapterDataById(m_CurrentShowingChapter.ChapterData.ChapterId - 1), isFromLeftToRight: true);
        }

        public void OnRightPageButtonClicked()
        {
            SetChapterAndShow(GameEntry.Data.InstanceGroups.GetChapterDataById(m_CurrentShowingChapter.ChapterData.ChapterId + 1), isFromLeftToRight: false);
        }

        protected override void OnClose(object userData)
        {            
            GameEntry.Event.Unsubscribe(EventId.OpenInstanceGroupChest, OnOpenChest);

            for (int i = 0; i < m_Chapters.Count; i++)
            {
                m_Chapters[i].gameObject.SetActive(false);
            }

            m_FirstBox.PauseEffect();
            m_SecondBox.PauseEffect();
            m_ThirdBox.PauseEffect();

            base.OnClose(userData);
        }
    }
}

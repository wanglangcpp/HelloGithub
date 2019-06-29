using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 旧内容淡出状态。
        /// </summary>
        private class StateOldContentsFadingOut : StateBase
        {
            private const string PreviewItemFadeOutAnimClipName = "ChancePreviewItemOut";
            private const string CardItemFadeOutAnimClipName = "ChanceItemFadeOut";

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);

                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    PreviewItems[i].CachedAnimation.Play(PreviewItemFadeOutAnimClipName);
                }

                for (int i = 0; i < CardItems.Count; ++i)
                {
                    CardItems[i].CachedAnimation.Play(CardItemFadeOutAnimClipName);
                }
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                for (int i = 0; i < CardItems.Count; ++i)
                {
                    CardItems[i].OuterWidget.alpha = 1f;
                    CardItems[i].Back.SetActive(true);
                    CardItems[i].Front.SetActive(false);
                }

                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    if (PreviewItems[i].CachedAnimation.IsPlaying(PreviewItemFadeOutAnimClipName))
                    {
                        return;
                    }
                }

                for (int i = 0; i < CardItems.Count; ++i)
                {
                    if (CardItems[i].CachedAnimation.IsPlaying(CardItemFadeOutAnimClipName))
                    {
                        return;
                    }
                }

                m_Form.m_CardScrollViewCache.SetActive(false);
                ChangeState<StateNewContentsFadingIn>(fsm);
            }
        }
    }
}

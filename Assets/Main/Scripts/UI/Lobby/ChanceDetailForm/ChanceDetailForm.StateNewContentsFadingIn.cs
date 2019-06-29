using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 新内容淡入状态。
        /// </summary>
        private class StateNewContentsFadingIn : StateBase
        {
            private const string PreviewItemFadeInAnimClipName = "ChancePreviewItemIn";

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                StartLoadingPreviewItems();
                m_UpdateSubState = UpdateLoadingPreviewItems;
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                m_UpdateSubState = null;
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                m_UpdateSubState(fsm, elapseSeconds, realElapseSeconds);
            }

            private void StartLoadingPreviewItems()
            {
                m_Form.RefreshPreviewItems();
            }

            private void UpdateLoadingPreviewItems(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (m_Form.IsLoadingPreviewItems)
                {
                    return;
                }

                StartPreviewItemsFadingIn();
                m_UpdateSubState = UpdatePreviewItemsFadingIn;
            }

            private void StartPreviewItemsFadingIn()
            {
                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    PreviewItems[i].CachedAnimation.Play(PreviewItemFadeInAnimClipName);
                }
            }

            private void UpdatePreviewItemsFadingIn(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    if (PreviewItems[i].CachedAnimation.IsPlaying(PreviewItemFadeInAnimClipName))
                    {
                        return;
                    }
                }

                ChangeState<StateNewContentsSpreading>(fsm);
            }
        }
    }
}

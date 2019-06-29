using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroInfoBaseForm
    {
        private void OnShowEntitySuccess(object o, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            if (ne == null)
            {
                return;
            }

            if (!(ne.Entity.Logic is FakeCharacter))
            {
                return;
            }

            var newFakeCharacter = ne.Entity.Logic as FakeCharacter;

            if (newFakeCharacter.CachedTransform.parent != m_PlatformRoot)
            {
                return;
            }

            m_Character = newFakeCharacter;
        }

        private void OnSwipeEnd(object o, GameEventArgs e)
        {
            var ne = e as SwipeEndEventArgs;
            if (ne == null /*|| m_SwitchingHero*/ || !m_IsRotatingHero)
            {
                return;
            }

            m_IsRotatingHero = false;
        }

        private void OnSwipeStart(object o, GameEventArgs e)
        {
            var ne = e as SwipeStartEventArgs;
            if (ne == null/* || m_SwitchingHero*/)
            {
                return;
            }

            var uiRoot = GetComponentInParent<UIRoot>();
            float uiHeight = uiRoot.manualHeight;
            float uiWidth = uiHeight / Screen.height * Screen.width;

            float left = m_HeroRotationRegion.cachedTransform.localPosition.x - m_HeroRotationRegion.width / 2f;
            float bottom = m_HeroRotationRegion.cachedTransform.localPosition.y - m_HeroRotationRegion.height / 2f;
            float width = m_HeroRotationRegion.width;
            float height = m_HeroRotationRegion.height;

            float leftOnScreen = left / uiWidth + .5f;
            float bottomOnScreen = bottom / uiHeight + .5f;
            float widthOnScreen = width / uiWidth;
            float heightOnScreen = height / uiHeight;

            var startPositionOnScreen = new Vector2(ne.StartPosition.x / Screen.width, ne.StartPosition.y / Screen.height);

            if (!new Rect(leftOnScreen, bottomOnScreen, widthOnScreen, heightOnScreen).Contains(startPositionOnScreen))
            {
                return;
            }

            m_IsRotatingHero = true;
        }

        private void OnSwipe(object o, GameEventArgs e)
        {
            var ne = e as SwipeEventArgs;
            if (ne == null /*|| m_SwitchingHero*/ || !m_IsRotatingHero)
            {
                return;
            }

            var deltaPosition = ne.DeltaPosition;
            var angleToRotate = -deltaPosition.x * m_HeroRotationSpeedFactor;
            GameEntry.DisplayModel.SetModelRotate(angleToRotate);
            //m_PlatformModel.Rotate(0f, angleToRotate, 0f, Space.Self);
            //m_Character.CachedTransform.Rotate(0f, angleToRotate, 0f, Space.Self);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BattleForm
    {
        [SerializeField]
        private MovieMask m_MovieMask = null;

        private bool m_MovieMaskGoingIn = false;
        private bool m_MovieMaskGoingOut = false;
        private bool m_MovieMaskPlaying;

        [Serializable]
        private class MovieMask
        {
            public GameObject Root = null;
            public Animation AnimIn = null;
        }

        private IEnumerator PlayMovieMaskInAnimAndWait()
        {
            PrepareForMovieMaskInAnim();

            m_MovieMaskGoingIn = true;

            m_MovieMask.AnimIn.gameObject.SetActive(true);
            m_MovieMask.AnimIn[InstanceMovieInAnimName].speed = 1f;
            m_MovieMask.AnimIn[InstanceMovieInAnimName].normalizedTime = 0f;
            m_MovieMask.AnimIn.Play(InstanceMovieInAnimName);

            while (m_MovieMask.AnimIn.IsPlaying(InstanceMovieInAnimName) && m_MovieMask.AnimIn[InstanceMovieInAnimName].speed == 1f)
            {
                yield return null;
            }

            m_MovieMaskGoingIn = false;
            m_MovieMask.AnimIn.gameObject.SetActive(false);
        }

        private void PrepareForMovieMaskInAnim()
        {
            m_NormalRoot.SetActive(false);
            m_MovieMask.Root.SetActive(true);
            GameEntry.Input.RefreshJoystick();
            if (GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled)
            {
                m_CachedAutoFightIsEnabled = true;
                GameEntry.SceneLogic.BaseInstanceLogic.DisableAutoFight();
            }

            var timer = GameEntry.SceneLogic.BaseInstanceLogic.Timer;
            if (timer != null)
            {
                timer.IsPaused = true;
            }
        }

        private IEnumerator PlayMovieMaskOutAnimAndWait()
        {
            m_MovieMaskGoingOut = true;

            m_MovieMask.AnimIn.gameObject.SetActive(true);
            m_MovieMask.AnimIn[InstanceMovieInAnimName].speed = -1f;
            m_MovieMask.AnimIn[InstanceMovieInAnimName].normalizedTime = 1f;
            m_MovieMask.AnimIn.Play(InstanceMovieInAnimName);

            while (m_MovieMask.AnimIn.IsPlaying(InstanceMovieInAnimName) && m_MovieMask.AnimIn[InstanceMovieInAnimName].speed == -1f)
            {
                yield return null;
            }

            CleanUpMovieMaskOutAnim();
        }

        private IEnumerator PlayMovieMaskInAndOut()
        {
            PrepareForMovieMaskInAnim();
            m_MovieMask.AnimIn.gameObject.SetActive(true);
            m_MovieMask.AnimIn[InstanceMovieInAnimName].speed = 1f;
            m_MovieMask.AnimIn[InstanceMovieInAnimName].normalizedTime = 0f;
            m_MovieMask.AnimIn.Play(InstanceMovieInAnimName);

            m_MovieMaskPlaying = true;
            while (m_MovieMask.AnimIn.IsPlaying(InstanceMovieInAnimName))
                yield return null;

            m_MovieMaskPlaying = false;

            CleanUpMovieMaskOutAnim();
        }

        private void CleanUpMovieMaskOutAnim()
        {
            m_MovieMaskGoingOut = false;
            m_NormalRoot.SetActive(true);
            m_MovieMask.Root.SetActive(false);
            GameEntry.Input.RefreshJoystick();

            if (m_CachedAutoFightIsEnabled)
            {
                m_CachedAutoFightIsEnabled = false;
                if (GameEntry.SceneLogic.BaseInstanceLogic.CanEnableAutoFight)
                {
                    GameEntry.SceneLogic.BaseInstanceLogic.EnableAutoFight();
                }
            }

            var timer = GameEntry.SceneLogic.BaseInstanceLogic.Timer;
            if (timer != null)
            {
                timer.IsPaused = false;
            }
        }
    }
}

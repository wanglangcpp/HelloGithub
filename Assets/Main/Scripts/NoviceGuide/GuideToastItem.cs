using UnityEngine;
using System.Collections;
namespace Genesis.GameClient
{
    public class GuideToastItem : MonoBehaviour
    {
        [SerializeField]
        private Animation m_Animation = null;

        [SerializeField]
        private UILabel m_ToastText = null;

        private string m_ToastStr = string.Empty;
        private float m_AnimationTime = 0.0f;
        private float m_CurPlayTime = 0.0f;
        private bool m_ToastAnimPlaying = false;

        public delegate void Voidfunction();

        public Voidfunction ToastPlayFinished = null;

        private void Start()
        {
            m_AnimationTime = m_Animation.clip.length;
        }

        public void InitToast(string str)
        {
            m_ToastText.text = str;
            m_Animation.clip.wrapMode = WrapMode.Default;
            m_Animation.wrapMode = WrapMode.Default;
        }

        public void PlayAnimation(Voidfunction func) {
            if (m_ToastAnimPlaying) {
                return;
            }
            ToastPlayFinished = func;
            gameObject.SetActive(true);
            m_CurPlayTime = 0.0f;
           
            m_Animation.Play();
        }

        private void Update()
        {
            m_CurPlayTime += Time.deltaTime;
            if (m_CurPlayTime > m_AnimationTime)
            {
                m_ToastAnimPlaying = false;
                gameObject.SetActive(false);
                if (ToastPlayFinished != null)
                {
                    ToastPlayFinished();
                }
            }
        }
    }
}

using UnityEngine;

namespace Genesis.GameClient
{
    public class ToastItem : MonoBehaviour
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

        public void InitToast(string str, Voidfunction func)
        {
            m_ToastAnimPlaying = true;
            m_ToastStr = str;
            ToastPlayFinished = func;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            m_Animation.enabled = false;
        }

        private void OnEnable()
        {
            if (!m_ToastAnimPlaying)
            {
                return;
            }

            m_CurPlayTime = 0.0f;
            m_ToastText.text = m_ToastStr;
            m_Animation.enabled = true;
            m_Animation.clip.wrapMode = WrapMode.Clamp;
            m_Animation.wrapMode = WrapMode.Clamp;
            m_Animation.Play();
        }

        public bool GetAnimPlaying()
        {
            return m_ToastAnimPlaying;
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

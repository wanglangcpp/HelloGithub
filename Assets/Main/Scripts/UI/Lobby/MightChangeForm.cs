using UnityEngine;

namespace Genesis.GameClient
{
    public class MightChangeForm : NGUIForm
    {
        [SerializeField]
        private Animation m_MightAnimation = null;

        [SerializeField]
        private Animation m_NumberUpAnimation = null;

        [SerializeField]
        private UILabel m_MightCurNumber = null;

        [SerializeField]
        private UILabel m_MightChangeNumber = null;

        [SerializeField]
        private float m_ChangeDuration = 2f;

        [SerializeField]
        private GameObject[] m_TeamBg = null;

        [SerializeField]
        private GameObject[] m_HeroBg = null;

        [SerializeField]
        private UISprite[] m_HeroIcon = null;

        [SerializeField]
        private UILabel m_HeroNameLabel = null;

        [SerializeField]
        private UILabel m_TeamNameLabel = null;

        private const string MightInAnimationClipName = "GsUp";

        private const string MightOutAnimationClipName = "GsUpOut";

        private int m_MightNumber = 100;

        private int m_MightChangeNum = 10;

        private bool m_IsPlus = true;

        private float m_CurDuration = 0;

        private bool m_IsPlayOutAnim = false;

        private bool m_IsHeroChangeMight = true;

        private int m_HeroType = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitData(userData);
            RefreshData();
            PlayMightAnim();
        }

        private void RefreshData()
        {
            for (int i = 0; i < m_TeamBg.Length; i++)
            {
                m_TeamBg[i].SetActive(!m_IsHeroChangeMight);
                m_HeroBg[i].SetActive(m_IsHeroChangeMight);
            }
            m_HeroNameLabel.gameObject.SetActive(m_IsHeroChangeMight);
            m_TeamNameLabel.gameObject.SetActive(!m_IsHeroChangeMight);
            if (m_HeroType != 0)
            {
                var heroInfo = GameEntry.Data.LobbyHeros.GetData(m_HeroType);
                for (int i = 0; i < m_TeamBg.Length; i++)
                {
                    m_HeroIcon[i].LoadAsync(heroInfo.IconId);
                }
            }
        }

        private void PlayMightAnim()
        {
            m_IsPlayOutAnim = false;
            m_CurDuration = 0;
            m_MightAnimation.GetComponent<UISprite>().alpha = 1;
            m_NumberUpAnimation.GetComponent<UILabel>().alpha = 1;
            PlayAnim(m_MightAnimation, MightInAnimationClipName);
            m_MightCurNumber.text = (m_MightNumber - m_MightChangeNum).ToString();
            m_IsPlus = m_MightChangeNum > 0;
            if (m_IsPlus)
            {
                m_MightChangeNumber.gameObject.SetActive(true);
                m_MightChangeNumber.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", m_MightChangeNum.ToString());
                PlayAnim(m_NumberUpAnimation, m_NumberUpAnimation.clip.name);
            }
            else
            {
                m_MightChangeNumber.gameObject.SetActive(false);
            }
            TweenNumber.Begin(m_MightCurNumber.gameObject, m_ChangeDuration, m_MightNumber);
        }

        private void InitData(object userData)
        {
            MightChangeDisplayData mightValue = (MightChangeDisplayData)userData;
            if (mightValue == null)
            {
                return;
            }
            m_MightNumber = mightValue.MightCurrentValue;
            m_MightChangeNum = mightValue.MightChangeValue;
            m_IsHeroChangeMight = mightValue.IsHeroMightChange;
            m_HeroType = mightValue.HeroType;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_CurDuration += elapseSeconds;
            if (m_CurDuration >= m_ChangeDuration)
            {
                if (!m_IsPlayOutAnim)
                {
                    m_IsPlayOutAnim = true;
                    PlayAnim(m_MightAnimation, MightOutAnimationClipName);
                    m_CurDuration -= m_MightAnimation[MightOutAnimationClipName].length;
                }
                else
                {
                    CloseSelf();
                }
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        private void PlayAnim(Animation anim, string name)
        {
            anim[name].time = 0;
            anim.Sample();
            anim.Play(name);
        }
    }
}

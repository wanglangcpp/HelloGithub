using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class InstanceGroupSelectionController : MonoBehaviour
    {
        [SerializeField]
        private Animation m_Animation = null;

        [SerializeField]
        private Animation m_MaskAnimation = null;

        [SerializeField]
        private Animation m_MapAnimation = null;

        [SerializeField]
        private GameObject m_SelectMap = null;

        [SerializeField]
        private float m_MapPosMagnification = 2.63f;

        [SerializeField]
        private Transform m_ParentTran = null;

        [SerializeField]
        private GameObject m_ChapterList = null;

        private bool m_IsEnter = false;

        private const string m_AnimName = "InstanceChapterIn";

        private const string m_MaskAnimName = "InstanceChapterMaskIn";

        private const string m_MapAnimName = "InstanceWorldMaprIn";

        private Vector3 m_InitialPosition = Vector3.zero;

        private Vector3 m_ScreenCenterPosition = Vector3.zero;

        private Vector3 m_SelectMapInitialPosition = Vector3.zero;

        private GameFrameworkAction<int> InstanceEnterReturn = null;

        private GameFrameworkAction<float> InstanceLeaveReturn = null;

        private int m_InstanceGroupId = 0;

        private UITexture m_CachedUITexture = null;

        public bool IsPlayingAnim
        {
            get;
            set;
        }

        private void Awake()
        {
            m_CachedUITexture = m_SelectMap.GetComponent<UITexture>();
            if (m_MapPosMagnification <= 0f)
            {
                m_MapPosMagnification = 1f;
            }
        }

        private void OnDestroy()
        {
            m_CachedUITexture = null;
        }

        private void Start()
        {
            m_SelectMapInitialPosition = new Vector3(m_SelectMap.transform.localPosition.x, m_SelectMap.transform.localPosition.y, m_SelectMap.transform.localPosition.z);
        }

        private void Update()
        {
            if (IsPlayingAnim)
            {
                if (!(m_Animation.isPlaying || m_MaskAnimation.isPlaying || m_MapAnimation.isPlaying))
                {
                    IsPlayingAnim = false;
                }
            }
        }

        public void Recover()
        {
            if (!m_IsEnter)
            {
                return;
            }
            m_IsEnter = false;

            m_MaskAnimation.GetComponent<UISprite>().alpha = 0.01f;
            m_SelectMap.transform.localScale = new Vector3(1.0f / m_MapPosMagnification, 1.0f / m_MapPosMagnification, 1);
            m_SelectMap.transform.localPosition = new Vector3(m_SelectMapInitialPosition.x, m_SelectMapInitialPosition.y, m_SelectMapInitialPosition.z);
            m_MapAnimation.transform.localScale = new Vector3(1, 1, 1);
            m_MapAnimation.transform.localPosition = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
        }

        public void InitInstanceGroup(int instanceGroup, GameFrameworkAction<int> EnterReturn, GameFrameworkAction<float> LeaveReturn)
        {
            m_IsEnter = false;
            m_MaskAnimation.GetComponent<UISprite>().alpha = 0.01f;
            m_SelectMap.transform.localScale = new Vector3(1.0f / m_MapPosMagnification, 1.0f / m_MapPosMagnification, 1);
            m_SelectMap.transform.localPosition = new Vector3(m_SelectMapInitialPosition.x, m_SelectMapInitialPosition.y, m_SelectMapInitialPosition.z);
            m_MapAnimation.transform.localScale = new Vector3(1, 1, 1);
            m_MapAnimation.transform.localPosition = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
            InstanceEnterReturn = EnterReturn;
            InstanceLeaveReturn = LeaveReturn;
            m_InstanceGroupId = instanceGroup;
        }

        private void StartMove(GameObject obj, float duration, Vector3 position)
        {
            TweenPosition tweenPos = UITweener.Begin<TweenPosition>(obj, duration);

            tweenPos.from = tweenPos.value;
            tweenPos.to = position;
            tweenPos.PlayForward();
        }

        private void InstanceEnter()
        {
            gameObject.SetActive(true);
            m_IsEnter = true;
            m_ScreenCenterPosition = new Vector3(-m_ParentTran.localPosition.x, -m_ParentTran.localPosition.y, -m_ParentTran.localPosition.z);
            m_InitialPosition = new Vector3(m_SelectMap.transform.localPosition.x, m_SelectMap.transform.localPosition.y, m_SelectMap.transform.localPosition.z);
            PlayAnim(m_Animation, m_AnimName, m_SelectMap, m_Animation[m_AnimName].length, m_ScreenCenterPosition);
            PlayAnim(m_MaskAnimation, m_MaskAnimName, m_MaskAnimation.gameObject, m_MaskAnimation[m_MaskAnimName].length, Vector3.zero, true);
            var MapPos = new Vector3(((m_ScreenCenterPosition.x / m_MapPosMagnification) - m_InitialPosition.x) * m_MapPosMagnification, ((m_ScreenCenterPosition.y / m_MapPosMagnification) - m_InitialPosition.y) * m_MapPosMagnification, 0);
            PlayAnim(m_MapAnimation, m_MapAnimName, m_MapAnimation.transform.parent.gameObject, m_MapAnimation[m_MapAnimName].length, MapPos);
            m_ChapterList.transform.localPosition = new Vector3(-m_ParentTran.localPosition.x, -m_ParentTran.localPosition.y, -m_ParentTran.localPosition.z);
        }

        private void InstanceLeave()
        {
            m_IsEnter = false;
            PlayAnim(m_Animation, m_AnimName, m_SelectMap, m_Animation[m_AnimName].length, m_InitialPosition);
            PlayAnim(m_MaskAnimation, m_MaskAnimName, m_MaskAnimation.gameObject, m_MaskAnimation[m_MaskAnimName].length, Vector3.zero, true);
            PlayAnim(m_MapAnimation, m_MapAnimName, m_MapAnimation.transform.parent.gameObject, m_MapAnimation[m_MapAnimName].length, Vector3.zero);
        }

        private void PlayAnim(Animation animation, string name, GameObject obj, float Length, Vector3 Pos, bool isMask = false)
        {
            IsPlayingAnim = true;
            if (m_IsEnter)
            {
                animation[name].speed = 1f;
                animation[name].time = 0;
            }
            else
            {
                animation[name].speed = -1f;
                animation[name].time = Length;
            }
            animation.Play();
            if (!isMask)
            {
                StartMove(obj, Length, Pos);
            }
        }

        public void OnClickMap()
        {
            if (!m_IsEnter)
            {
                InstanceEnter();
                if (InstanceEnterReturn != null)
                {
                    InstanceEnterReturn(m_InstanceGroupId);
                }
            }
        }

        private void OnOpenChapterImmediately()
        {
            if (m_IsEnter)
            {
                return;
            }
            gameObject.SetActive(true);
            m_IsEnter = true;
            m_ScreenCenterPosition = new Vector3(-m_ParentTran.localPosition.x, -m_ParentTran.localPosition.y, -m_ParentTran.localPosition.z);
            m_InitialPosition = new Vector3(m_SelectMap.transform.localPosition.x, m_SelectMap.transform.localPosition.y, m_SelectMap.transform.localPosition.z);
            PlayAnimImmediately(m_Animation, m_AnimName, m_SelectMap, m_Animation[m_AnimName].length, m_ScreenCenterPosition);
            PlayAnimImmediately(m_MaskAnimation, m_MaskAnimName, m_MaskAnimation.gameObject, m_MaskAnimation[m_MaskAnimName].length, Vector3.zero, true);
            var MapPos = new Vector3(((m_ScreenCenterPosition.x / m_MapPosMagnification) - m_InitialPosition.x) * m_MapPosMagnification, ((m_ScreenCenterPosition.y / m_MapPosMagnification) - m_InitialPosition.y) * m_MapPosMagnification, 0);
            PlayAnimImmediately(m_MapAnimation, m_MapAnimName, m_MapAnimation.transform.parent.gameObject, m_MapAnimation[m_MapAnimName].length, MapPos);
            m_ChapterList.transform.localPosition = new Vector3(-m_ParentTran.localPosition.x, -m_ParentTran.localPosition.y, -m_ParentTran.localPosition.z);

            if (InstanceEnterReturn != null)
            {
                InstanceEnterReturn(m_InstanceGroupId);
            }
        }

        private void PlayAnimImmediately(Animation animation, string name, GameObject obj, float Length, Vector3 Pos, bool isMask = false)
        {
            IsPlayingAnim = true;
            if (m_IsEnter)
            {
                // animation[name].time = Length;
                animation.clip.SampleAnimation(animation.gameObject, Length);
            }
            else
            {
                /* animation[name].time = 0;*/
                animation.clip.SampleAnimation(animation.gameObject, 0);
            }
            /* animation.Sample();*/
            if (!isMask)
            {
                obj.transform.localPosition = Pos;
            }
        }

        public void OnClickLeave()
        {
            if (m_IsEnter)
            {
                InstanceLeave();
                if (InstanceLeaveReturn != null)
                {
                    InstanceLeaveReturn(m_Animation[m_AnimName].length);
                }
            }
        }

        public bool HasEnterInstance()
        {
            return m_IsEnter;
        }

        public void LoadChapterTexture(string resPath, int instanceGroupId, GameFrameworkAction<int> onEnterInstance, GameFrameworkAction<float> onLeaveInstance, bool isImmediately)
        {
            m_CachedUITexture.LoadAsync(resPath, OnLoadChapterTextureSuccess, null, null, new LoadChapterTextureData
            {
                Controller = this,
                InstanceGroupId = instanceGroupId,
                OnEnterInstance = onEnterInstance,
                OnLeaveInstance = onLeaveInstance,
                IsImmediately = isImmediately,
            });
        }

        public void ResetChapterTexture()
        {
            m_CachedUITexture.mainTexture = null;
        }

        private void OnLoadChapterTextureSuccess(UITexture uiTexture, object userData)
        {
            var data = userData as LoadChapterTextureData;
            if (data == null)
            {
                Log.Warning("User data is invalid.");
                return;
            }

            var controller = data.Controller;
            if (this == null || controller != this)
            {
                return;
            }

            InitInstanceGroup(data.InstanceGroupId, data.OnEnterInstance, data.OnLeaveInstance);

            if (data.IsImmediately)
            {
                OnOpenChapterImmediately();
            }
            else
            {
                OnClickMap();
            }
        }

        private class LoadChapterTextureData
        {
            public InstanceGroupSelectionController Controller;
            public int InstanceGroupId;
            public GameFrameworkAction<int> OnEnterInstance;
            public GameFrameworkAction<float> OnLeaveInstance;
            public bool IsImmediately;
        }
    }
}

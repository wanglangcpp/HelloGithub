using UnityEngine;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Resource;
using GameFramework.Event;
using UnityEngine.SceneManagement;

namespace Genesis.GameClient
{

    /// <summary>
    /// 展示英雄的模块,使用CameraViewportController
    /// </summary>
    public class DisplayModelComponent : MonoBehaviour
    {
        [SerializeField]
        private AssetBundle bundle;
#if UNITY_EDITOR
        [SerializeField]
        private UnityEditor.SceneAsset sceneAsset;
#endif
        [SerializeField]
        protected FakeCharacter m_Character = null;
        [SerializeField]
        private Transform m_PlatformRoot = null;
        [SerializeField]
        private DisplayModelCamera modelCamera = null;
        [SerializeField]
        private RenderTexture renderTexture = null;
        [SerializeField]
        private CameraViewportController viewportController;
        [SerializeField]
        private UITexture curTexture;


        DRDisplayModel drDisplay;
        DRHero drHero = null;
        DRCharacter drCharacter;
        bool isShowModel = false;
        bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
        }
        private void Start()
        {
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            //GameEntry.Event.Subscribe(EventId.SwipeStart, OnSwipeStart);
            //GameEntry.Event.Subscribe(EventId.SwipeEnd, OnSwipeEnd);
            //GameEntry.Event.Subscribe(EventId.Swipe, OnSwipe);
            GameEntry.Event.Subscribe(EventId.ChangeSceneStart, OnChangeSceneStart);
        }
        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            //GameEntry.Event.Unsubscribe(EventId.SwipeStart, OnSwipeStart);
            //GameEntry.Event.Unsubscribe(EventId.SwipeEnd, OnSwipeEnd);
            //GameEntry.Event.Unsubscribe(EventId.Swipe, OnSwipe);
            GameEntry.Event.Unsubscribe(EventId.ChangeSceneStart, OnChangeSceneStart);
        }
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    DisplayModel(2, 4, testTexture);
            //}

        }
        public void DisplayModel(int displayModelId, int heroId, UITexture uiTexture)
        {
            isShowModel = true;
            isLoading = true;
            if (curTexture != uiTexture)
            {
                //if (renderTexture != null && curTexture == null)
                //{
                //    //renderTexture.Release();
                //    Destroy(renderTexture);
                //}
                if (renderTexture == null)
                {
                    renderTexture = new RenderTexture(uiTexture.width, uiTexture.height, 24);
                }
            }
            curTexture = uiTexture;
            uiTexture.mainTexture = renderTexture;
            if (this.drDisplay != null && this.drDisplay.Id == displayModelId && drHero != null && drHero.Id == heroId && m_Character != null)
            {
                if (modelCamera.modelCamera != null)
                {
                    modelCamera.modelCamera.targetTexture = renderTexture;
                    m_PlatformRoot.eulerAngles = new Vector3(0f, 180f, 0f);//Vector3.zero;
                }
                m_Character.Debut();
                isLoading = false;
                return;
            }
            DRDisplayModel newDrDisplay = GameEntry.DataTable.GetDataTable<DRDisplayModel>().GetDataRow(displayModelId);
            drHero = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(heroId);
            drCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>().GetDataRow(drHero.CharacterId);
            if (this.modelCamera && drDisplay != null && drDisplay.SceneName == newDrDisplay.SceneName)
            {
                if (drDisplay.Id != newDrDisplay.Id)
                {
                    //共用一个展示场景，但是摄像机参数不同
                    modelCamera.transform.position = new Vector3(newDrDisplay.CameraX, newDrDisplay.CameraY, newDrDisplay.CameraZ);
                    modelCamera.transform.eulerAngles = new Vector3(newDrDisplay.RotationX, newDrDisplay.RotationY, newDrDisplay.RotationZ);
                    viewportController.UpdateCameraViewport(new Vector2(newDrDisplay.ViewOffsetX, newDrDisplay.ViewOffsetY));
                }
                drDisplay = newDrDisplay;
                LoadDisplayModel();
            }
            else
            {
                drDisplay = newDrDisplay;
                OnChangeSceneStart(null, null);
                LoadDisplayScene();
            }

        }
        public void HideDisplayModel()
        {
            isShowModel = false;
            if (modelCamera != null && modelCamera.enabled && renderTexture == null)
            {
                modelCamera.modelCamera.enabled = false;
            }
        }
        /// <summary>
        /// 预加载展示场景，暂时留着不优化
        /// </summary>
        /// <param name="displayModelId"></param>
        public void PreLoadDisplayScene(int displayModelId)
        {

        }
        private void LoadDisplayScene()
        {
            //LoadAssetCallbacks loadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFail);
            //string  AssetUtility.GetSceneAsset(drDisplay.SceneName)
            //GameEntry.Resource.LoadAsset(AssetUtility.GetSceneAsset(drDisplay.SceneName), loadAssetCallbacks);
            GameEntry.Scene.LoadScene(drDisplay.SceneName, AssetUtility.GetSceneAsset(drDisplay.SceneName));
        }

        private void LoadDisplayModel()
        {
            if (m_Character)
            {
                GameEntry.Entity.HideEntity(m_Character);
            }
            m_PlatformRoot.eulerAngles = new Vector3(0f, 180f, 0f);//Vector3.zero;
            FakeCharacter.Show(drHero.CharacterId, drHero.Id, m_PlatformRoot, null, 0f, FakeCharacterData.ActionOnShow.Debut);
        }

        private void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
//#if UNITY_EDITOR
//            sceneAsset = asset as UnityEditor.SceneAsset;

//#else
//            bundle = asset as AssetBundle;
//#endif
            //Application.LoadLevelAdditive(drDisplay.SceneName);
            SceneManager.LoadScene(drDisplay.SceneName, LoadSceneMode.Additive);
        }
        private void LoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            //Debug.LogError("加载失败" + status + "    " + errorMessage);
        }

        /// <summary>
        /// 新场景加载完成后设置模型摄像机引用和模型挂载点
        /// </summary>
        /// <param name="modelCamera"></param>
        public void SetModelCamera(DisplayModelCamera modelCamera)
        {
            this.modelCamera = modelCamera;
            m_PlatformRoot = modelCamera.platformRoot;
            viewportController = modelCamera.viewportController;
            modelCamera.modelCamera.targetTexture = renderTexture;
            modelCamera.transform.position = new Vector3(drDisplay.CameraX, drDisplay.CameraY, drDisplay.CameraZ);
            modelCamera.transform.eulerAngles = new Vector3(drDisplay.RotationX, drDisplay.RotationY, drDisplay.RotationZ);
            viewportController.UpdateCameraViewport(new Vector2(drDisplay.ViewOffsetX, drDisplay.ViewOffsetY));
            LoadDisplayModel();
        }
        protected virtual void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            if (modelCamera == null)
            {
                return;
            }
            SceneManager.UnloadScene(drDisplay.SceneName);
            if (modelCamera.modelCamera)
            {
                modelCamera.modelCamera.targetTexture = null;
            }
            modelCamera = null;
            m_Character = null;
            m_PlatformRoot = null;
            renderTexture.Release();
            Destroy(renderTexture);
            renderTexture = null;
            drDisplay = null;
            drHero = null;
            drCharacter = null;
            viewportController = null;
        }

        public void SetModelRotate(float rotateAngle)
        {
            if (isShowModel == false)
            {
                return;
            }
            m_PlatformRoot.Rotate(0f, rotateAngle, 0f, Space.Self);
        }
        #region Event
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
            isLoading = false;
            m_Character = newFakeCharacter;
        }
        public void OnHeroInteractor()
        {
            if (isShowModel == false)
            {
                return;
            }
            if (m_Character)
            {
                m_Character.Interact();
            }
        }
        //bool m_SwitchingHero = false;
        //bool m_IsRotatingHero = false;
        //private void OnSwipeEnd(object o, GameEventArgs e)
        //{
        //    var ne = e as SwipeEndEventArgs;
        //    if (ne == null || m_SwitchingHero || !m_IsRotatingHero)
        //    {
        //        return;
        //    }

        //    m_IsRotatingHero = false;
        //}

        //private void OnSwipeStart(object o, GameEventArgs e)
        //{
        //    var ne = e as SwipeStartEventArgs;
        //    if (ne == null || m_SwitchingHero)
        //    {
        //        return;
        //    }

        //    var uiRoot = GetComponentInParent<UIRoot>();
        //    float uiHeight = uiRoot.manualHeight;
        //    float uiWidth = uiHeight / Screen.height * Screen.width;

        //    float left = m_HeroRotationRegion.cachedTransform.localPosition.x - m_HeroRotationRegion.width / 2f;
        //    float bottom = m_HeroRotationRegion.cachedTransform.localPosition.y - m_HeroRotationRegion.height / 2f;
        //    float width = m_HeroRotationRegion.width;
        //    float height = m_HeroRotationRegion.height;

        //    float leftOnScreen = left / uiWidth + .5f;
        //    float bottomOnScreen = bottom / uiHeight + .5f;
        //    float widthOnScreen = width / uiWidth;
        //    float heightOnScreen = height / uiHeight;

        //    var startPositionOnScreen = new Vector2(ne.StartPosition.x / Screen.width, ne.StartPosition.y / Screen.height);

        //    if (!new Rect(leftOnScreen, bottomOnScreen, widthOnScreen, heightOnScreen).Contains(startPositionOnScreen))
        //    {
        //        return;
        //    }

        //    m_IsRotatingHero = true;
        //}

        //private void OnSwipe(object o, GameEventArgs e)
        //{
        //    Debug.LogError("OnSwipe");
        //    //var ne = e as SwipeEventArgs;
        //    //if (ne == null || m_SwitchingHero || !m_IsRotatingHero)
        //    //{
        //    //    return;
        //    //}

        //    //var deltaPosition = ne.DeltaPosition;
        //    //var angleToRotate = -deltaPosition.x * m_HeroRotationSpeedFactor;
        //    //m_PlatformModel.Rotate(0f, angleToRotate, 0f, Space.Self);
        //    //m_Character.CachedTransform.Rotate(0f, angleToRotate, 0f, Space.Self);
        //}

        #endregion
    }
}
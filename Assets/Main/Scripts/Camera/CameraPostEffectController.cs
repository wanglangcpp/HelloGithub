using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机后处理控制器。
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public class CameraPostEffectController : MonoBehaviour
    {
        public MotionBlur MotionBlur { get; private set; }
        public RadialBlur RadialBlur { get; private set; }
        public SceneDarkening SceneDarkening { get; private set; }

        private void Awake()
        {
            MotionBlur = gameObject.AddComponent<MotionBlur>();
            RadialBlur = gameObject.AddComponent<RadialBlur>();
            SceneDarkening = gameObject.AddComponent<SceneDarkening>();
        }
    }
}

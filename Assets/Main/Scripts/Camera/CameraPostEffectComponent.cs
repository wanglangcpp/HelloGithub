using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机后处理组件。
    /// </summary>
    [DisallowMultipleComponent]
    public class CameraPostEffectComponent : MonoBehaviour
    {
        private CameraPostEffectController Controller
        {
            get
            {
                if (GameEntry.Scene.MainCamera == null)
                {
                    return null;
                }

                return GameEntry.Scene.MainCamera.GetComponent<CameraPostEffectController>();
            }
        }

        public void StartSceneColorChange(Color targetColor, float attack, float sustain, float release)
        {
            if (Controller == null)
            {
                return;
            }

            Controller.SceneDarkening.StartColorChange(targetColor, attack, sustain, release);
        }

        public void ResetSceneColorChange()
        {
            if (Controller == null)
            {
                return;
            }

            Controller.SceneDarkening.ResetColorChange();
        }

        public void EnableMotionBlur()
        {
            if (Controller == null)
            {
                return;
            }

            Controller.MotionBlur.EnableMotionBlur();
        }

        public void DisableMotionBlur()
        {
            if (Controller == null)
            {
                return;
            }

            Controller.MotionBlur.DisableMotionBlur();
        }

        public void EnableRadialBlur()
        {
            if (Controller == null)
            {
                return;
            }

            Controller.RadialBlur.FadeInRadialBlur();
        }

        public void DisableRadialBlur()
        {
            if (Controller == null)
            {
                return;
            }

            Controller.RadialBlur.FadeOutRadialBlur();
        }
    }
}

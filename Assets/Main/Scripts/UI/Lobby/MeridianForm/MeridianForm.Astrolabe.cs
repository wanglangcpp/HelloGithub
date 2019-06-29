using GameFramework;
using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class MeridianForm
    {
        [SerializeField]
        private Transform m_AstrolabeTransform = null;

        [SerializeField]
        private Color[] m_MeriColors = null;

        [SerializeField]
        private UISprite[] m_Decals = null;

        [SerializeField]
        private AstrolabeItem[] m_AstrolabeItems = null;

        [SerializeField]
        private UIButton m_RewardBtn = null;

        [SerializeField]
        private UIButton m_ActivationBtn = null;

        [SerializeField]
        private GameObject m_MeridianInsufficientSubForm = null;

        private int m_MiddleIndex = 0;

        private float m_LastSumAngle = 0;

        private float m_SumAngle = 0;

        private const float RoundAngle = 360;

        private int m_EffectId = -1;

        private int m_AstrolabeItemKey = 0;

        private bool m_IsHaveMeridian = false;

        private void RefreshAstrolabe(bool isInit)
        {
            for (int i = 0; i < m_Decals.Length; i++)
            {
                m_Decals[i].color = m_MeriColors[m_CurrentMeridianIndex];
            }
            m_LastSumAngle = m_SumAngle;
            m_SumAngle = 0;
            if (m_CurrentMeridianIndex < GameEntry.Data.Meridian.MeridianProgress / Constant.MaxMeridianAstrolabe + 1)
            {
                m_IsHaveMeridian = GameEntry.Data.Player.MeridianEnergy > 0;
                m_RewardBtn.gameObject.SetActive(!m_IsHaveMeridian);
                m_ActivationBtn.gameObject.SetActive(m_IsHaveMeridian);
                int activateStar = GameEntry.Data.Meridian.MeridianProgress;
                if (activateStar == 360)
                {
                    return;
                }
                m_AstrolabeItemKey = activateStar % Constant.MaxMeridianAstrolabe;
                for (int i = 0; i < Constant.MaxMeridianAstrolabe; i++)
                {
                    m_AstrolabeItems[i].InitItem(i < m_AstrolabeItemKey, isInit && i == m_AstrolabeItemKey, m_CurrentMeridianIndex, m_MeriColors[m_CurrentMeridianIndex]);
                }

                m_MiddleIndex = m_AstrolabeItemKey;
                m_SumAngle = RoundAngle / Constant.MaxMeridianAstrolabe * m_AstrolabeItemKey;

                if (m_SumAngle >= RoundAngle)
                {
                    m_SumAngle -= RoundAngle;
                }
            }

            UpgradeAstrolabe(isInit);
        }

        private void UpgradeAstrolabe(bool isInit)
        {
            if (!isInit)
            {
                TweenRotation tweenRo = m_AstrolabeTransform.GetComponent<TweenRotation>();
                if (tweenRo == null)
                {
                    tweenRo = m_AstrolabeTransform.gameObject.AddComponent<TweenRotation>();
                }
                var lastAngle = (CaculAngle(m_LastSumAngle) == (-RoundAngle / 2)) ? (RoundAngle / 2) : (CaculAngle(m_LastSumAngle));
                tweenRo.from = new Vector3(0, 0, lastAngle);
                tweenRo.to = new Vector3(0, 0, CaculAngle(m_SumAngle));
                tweenRo.duration = 1.0f;
                tweenRo.ResetToBeginning();
                tweenRo.AddOnFinished(AstrolabeTweenFinished);
                tweenRo.PlayForward();
                m_EffectsController.DestroyEffect(m_EffectId);
                m_EffectId = -1;
            }
            else
            {
                m_AstrolabeTransform.localEulerAngles = new Vector3(0, 0, CaculAngle(m_SumAngle));
            }
        }

        private void ShowAstrolabeEffect()
        {
            if (m_EffectId == -1)
            {
                m_EffectId = m_EffectsController.ShowEffect("EffectMeridianHov");
            }
        }

        private void AstrolabeTweenFinished()
        {
            ShowAstrolabeEffect();
            m_ActivationBtn.isEnabled = true;
            TweenRotation tweenRo = m_AstrolabeTransform.GetComponent<TweenRotation>();
            for (int i = 0; i < tweenRo.onFinished.Count;)
            {
                tweenRo.RemoveOnFinished(tweenRo.onFinished[i]);
            }
            if (m_MiddleIndex == Constant.MaxMeridianAstrolabe)
            {
                m_AstrolabeItems[0].InitItem(true, false, m_CurrentMeridianIndex, m_MeriColors[m_CurrentMeridianIndex]);
            }
            else
            {
                m_AstrolabeItems[m_MiddleIndex].InitItem(false, true, m_CurrentMeridianIndex, m_MeriColors[m_CurrentMeridianIndex]);
            }
        }

        private float CaculAngle(float angle)
        {
            if (angle <= RoundAngle / 2)
            {
                angle = -angle;
            }
            else
            {
                angle = RoundAngle - angle;
            }
            return angle;
        }

        public void OnClickActivationBtn()
        {
            OpenMeridianStar(m_AstrolabeItemKey);
        }

        public void OnClickRewardBtn()
        {
            m_MeridianInsufficientSubForm.SetActive(true);
        }

        public void ClickToCloseSubForm()
        {
            m_MeridianInsufficientSubForm.SetActive(false);
            ShowAstrolabeEffect();
        }

        public void OnClickOpenInstance()
        {
            GameEntry.UI.OpenUIForm(UIFormId.InstanceSelectForm);
            ClickToCloseSubForm();
        }

        public void OnClickAstrolabeItem(int index)
        {
            int meridianId = GameEntry.Data.Meridian.MeridianProgress;
            DRMeridian drMeridian = GameEntry.Data.Meridian.MeridianRow[meridianId];
            int meridianEnergy = GameEntry.Data.Player.MeridianEnergy;
            if (meridianEnergy >= drMeridian.CostMeridianEnergy)
            {
                OpenMeridianStar(meridianId);
            }
            else
            {
                m_MeridianInsufficientSubForm.SetActive(true);
                m_EffectsController.DestroyEffect(m_EffectId);
                m_EffectId = -1;
            }
        }

        private void OpenMeridianStar(int index)
        {
            int meridianId = GameEntry.Data.Meridian.MeridianProgress;
            DRMeridian drMeridian = GameEntry.Data.Meridian.MeridianRow[meridianId];
            if (drMeridian == null)
            {
                Log.Error("MeridianForm OpenMeridianStar'DRMeridian is null!!!");
                return;
            }
            int playerCoin = GameEntry.Data.Player.Coin;
            if (playerCoin >= drMeridian.CostCoin)
            {
                m_AstrolabeItems[meridianId % Constant.MaxMeridianAstrolabe].MiddleCollider = false;
                m_EffectsController.ShowEffect("EffectMeridianBoom");
                m_ActivationBtn.isEnabled = false;
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    CLOpenMeridianStar request = new CLOpenMeridianStar();
                    request.StarId = drMeridian.Id;
                    GameEntry.Network.Send(request);
                }
            }
            else
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_MERIDIAN_INSUFFICIENT_GOLD"));
            }
        }
    }
}

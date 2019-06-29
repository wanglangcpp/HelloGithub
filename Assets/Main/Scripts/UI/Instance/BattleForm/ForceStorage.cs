using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

namespace Genesis.GameClient
{
    public class ForceStorage : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_DivideTemplete = null;

        [SerializeField]
        private UIProgressBar m_ProgressBar = null;

        [SerializeField]
        private Transform m_DivideLineParent = null;

        [SerializeField]
        private Transform m_DivideArea = null;

        [SerializeField]
        private UIEffectsController m_Effect = null;

        private float m_ElapsedTime = 0f;
        private bool m_Enable = false;
        private Queue<GameObject> m_DivideInstanceCache = new Queue<GameObject>();
        private float m_MaxShowTime = float.MaxValue;

        private List<float> m_DivideValues = new List<float>();
        private int m_CurrentShowingEffectIndex = -1;
        private string[] m_EffectNames = {"effect_ui_forcestorage_03", "effect_ui_forcestorage_04", "effect_ui_forcestorage_02" };
        //private string m_ProgressThumbEffect = "effect_ui_forcestorage_01";
        private int m_ShowingEffectId = int.MinValue;

        public void Show(DRSkillChargeTime divideData)
        {
            gameObject.SetActive(true);
            m_DivideTemplete.SetActive(false);

            m_MaxShowTime = divideData.TotalTime;
            var bounds = NGUIMath.CalculateRelativeWidgetBounds(m_DivideArea.transform);

            float minPosition = bounds.min.x;
            float maxPosition = bounds.max.x;

            m_DivideValues.Clear();
            for (int i = 0; i < divideData.ValidChargeEndTime.Count; i++)
            {
                float timePersent = divideData.ValidChargeEndTime[i] / m_MaxShowTime;
                if (timePersent > 1)
                {
                    Log.Error("Error data for charge skill data. call the designer for this function.");
                    continue;
                }
                m_DivideValues.Add(timePersent);
                var ins = InstaniateDivideLine();
                float xPosition = (maxPosition - minPosition) * timePersent + minPosition;

                ins.transform.SetParent(m_DivideLineParent);
                ins.transform.localPosition = new Vector3(xPosition, m_ProgressBar.transform.localPosition.y, m_ProgressBar.transform.localPosition.z);
                ins.transform.localScale = Vector3.one;
                ins.SetActive(true);
            }
            m_DivideValues.Add(1.1f);   // 故意让最后多出来一个、方便逻辑用

            m_Effect.Resume();
            m_Enable = true;
        }

        private GameObject InstaniateDivideLine()
        {
            if (m_DivideInstanceCache.Count > 0)
                return m_DivideInstanceCache.Dequeue();

            return Instantiate(m_DivideTemplete);
        }

        public void Hide()
        {
            m_Enable = false;
            gameObject.SetActive(false);

            while (m_DivideLineParent.childCount > 0)
            {
                var item = m_DivideLineParent.GetChild(0);
                item.SetParent(transform);
                item.gameObject.SetActive(false);
                m_DivideInstanceCache.Enqueue(item.gameObject);
            }

            m_ElapsedTime = 0f;
            m_Effect.Pause();
        }

        protected void Update()
        {
            if (m_Enable == false)
                return;

            m_ElapsedTime += Time.deltaTime;

            m_ProgressBar.value = m_ElapsedTime / m_MaxShowTime;

            int effectIndex = GetEffectIndex(m_ProgressBar.value);
             
            if (effectIndex >= 0 && effectIndex != m_CurrentShowingEffectIndex)
            {
                if (m_Effect.HasEffect(m_EffectNames[effectIndex]))
                {
                    if (m_ShowingEffectId != int.MinValue)
                        m_Effect.DestroyEffect(m_ShowingEffectId);

                    m_ShowingEffectId = m_Effect.ShowEffect(m_EffectNames[effectIndex]);
                }
                m_CurrentShowingEffectIndex = effectIndex;
            }

            if (m_ProgressBar.value >= 1)
            {
                Hide();
            }
        }

        private int GetEffectIndex(float currentValue)
        {
            for (int i = 0; i < m_DivideValues.Count - 1; i++)
                if (m_DivideValues[i] <= currentValue && m_DivideValues[i + 1] > currentValue)
                    return i;

            return -1;
        }
    }
}
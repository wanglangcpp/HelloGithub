using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体变色组件。
    /// </summary>
    public class ColorChanger : MonoBehaviour
    {
        private enum ColorChangeStatus
        {
            Starting,
            Normal,
            Stopping,
            Stopped,
        }

        private static readonly Color DefaultRimColor = new Color(0f, 0f, 0f, 0f);
        private static readonly Color DefaultInnerColor = new Color(0f, 0f, 0f, 0f);
        private const float DefaultInnerColorPower = 0f;
        private const float DefaultRimPower = 0f;
        private const float DefaultAlphaPower = 0f;
        private const float DefaultAllPower = 0f;
        private const float DefaultWeight = 0f;

        private int m_RimColorPropertyId = -1;
        private int m_InnerColorPropertyId = -1;
        private int m_InnerColorPowerPropertyId = -1;
        private int m_RimPowerPropertyId = -1;
        private int m_AlphaPowerPropertyId = -1;
        private int m_AllPowerPropertyId = -1;

        private const string ShaderName = "Game/Character";

        [Serializable]
        private class ColorChangeData
        {
            internal static int CurrentSerialId = 0;

            internal ColorChangeStatus State { get; private set; }

            internal DRChangeColor DataRow { get; private set; }
            internal int SerialId { get; private set; }
            internal float Duration { get; private set; }

            internal Color RimColor
            {
                get
                {
                    return Weight * DataRow.RimColor;
                }
            }

            internal Color InnerColor
            {
                get
                {
                    return Weight * DataRow.InnerColor;
                }
            }

            internal float InnerColorPower
            {
                get
                {
                    return Weight * DataRow.InnerColorPower;
                }
            }

            internal float RimPower
            {
                get
                {
                    return Weight * DataRow.RimPower;
                }
            }

            internal float AlphaPower
            {
                get
                {
                    return Weight * DataRow.AlphaPower;
                }
            }

            internal float AllPower
            {
                get
                {
                    return Weight * DataRow.AllPower;
                }
            }

            internal float CurrentTime { get; private set; }
            internal float StoppingTime { get; private set; }
            internal float Weight { get; private set; }

            internal ColorChangeData(int changeColorId, float duration)
            {
                SerialId = CurrentSerialId++;
                Duration = duration;
                Weight = 0f;
                State = ColorChangeStatus.Starting;

                var dt = GameEntry.DataTable.GetDataTable<DRChangeColor>();
                DRChangeColor dr = dt.GetDataRow(changeColorId);
                if (dr == null)
                {
                    Log.Error("Change color id '{0}' is not exist.", changeColorId.ToString());
                    return;
                }

                DataRow = dr;
            }

            internal void OnUpdate()
            {
                CurrentTime += Time.deltaTime;
                switch (State)
                {
                    case ColorChangeStatus.Starting:
                        UpdateStarting();
                        break;
                    case ColorChangeStatus.Normal:
                        UpdateNormal();
                        break;
                    case ColorChangeStatus.Stopping:
                        UpdateStopping();
                        break;
                    default:
                        Weight = 0f;
                        break;
                }
            }

            internal void Stop()
            {
                if (State != ColorChangeStatus.Normal && State != ColorChangeStatus.Starting)
                {
                    return;
                }

                if (DataRow.EndDuration <= 0f)
                {
                    State = ColorChangeStatus.Stopped;
                }
                else
                {
                    State = ColorChangeStatus.Stopping;
                }
            }

            private void UpdateStarting()
            {
                float portion;
                if (DataRow.BeginDuration <= 0.001f)
                {
                    portion = 1f;
                }
                else
                {
                    portion = NumericalCalcUtility.CalcProportion(0f, DataRow.BeginDuration, CurrentTime);
                }

                if (portion >= 1f)
                {
                    portion = 1f;
                    State = ColorChangeStatus.Normal;
                }

                Weight = portion;
            }

            private void UpdateNormal()
            {
                Weight = 1f;

                if (CurrentTime - DataRow.BeginDuration > Duration)
                {
                    Stop();
                }
            }

            private void UpdateStopping()
            {
                StoppingTime += Time.deltaTime;

                float portion;
                if (DataRow.EndDuration < 0.001f)
                {
                    portion = 1f;
                }
                else
                {
                    portion = NumericalCalcUtility.CalcProportion(0f, DataRow.EndDuration, StoppingTime);
                }

                if (portion >= 1f)
                {
                    portion = 1f;
                    State = ColorChangeStatus.Stopped;
                }

                Weight = 1f - portion;
            }
        }

        private const int PoolCapacity = 5;

        [SerializeField]
        private List<Renderer> m_Renderers = new List<Renderer>();

        private List<ColorChangeData> m_Pool = new List<ColorChangeData>();

        private bool m_IsReset = true;

        public IList<Renderer> GetRenderers()
        {
            return m_Renderers;
        }

        /// <summary>
        /// 增加需要变色的渲染器。
        /// </summary>
        /// <param name="mr">渲染器。</param>
        public void AddRenderer(Renderer mr)
        {
            if (mr == null)
            {
                Log.Warning("You're trying to add a null.");
                return;
            }

            m_Renderers.Add(mr);
        }

        /// <summary>
        /// 移除需要变色的渲染器。
        /// </summary>
        /// <param name="mr">渲染器。</param>
        /// <returns>删除是否成功。</returns>
        public bool RemoveRenderer(Renderer mr)
        {
            if (mr == null)
            {
                return false;
            }

            var ret = m_Renderers.Remove(mr);
            if (ret)
            {
                UpdateMaterialsPerRenderer(DefaultRimColor, DefaultInnerColor, DefaultInnerColorPower, DefaultRimPower, DefaultAlphaPower, DefaultAllPower, mr);
            }

            return ret;
        }

        /// <summary>
        /// 开始变色。
        /// </summary>
        /// <param name="id">变色编号。</param>
        /// <param name="duration">变色持续时长。</param>
        /// <returns>变色序列号。</returns>
        public int StartColorChange(int id, float duration)
        {
            if (m_Pool.Count >= PoolCapacity)
            {
                return -1;
            }

            var toAdd = new ColorChangeData(id, duration);
            m_Pool.Add(toAdd);
            return toAdd.SerialId;
        }

        /// <summary>
        /// 停止变色。
        /// </summary>
        /// <param name="serialId">变色序列号。</param>
        public void StopColorChange(int serialId)
        {
            for (int i = 0; i < m_Pool.Count; ++i)
            {
                if (m_Pool[i].SerialId == serialId)
                {
                    m_Pool[i].Stop();
                    return;
                }
            }
        }

        /// <summary>
        /// 复位，即立即停止所有变色并回复默认颜色。
        /// </summary>
        public void Reset()
        {
            m_Pool.Clear();
            ResetMaterials();
        }

        /// <summary>
        /// 显示所有渲染器。
        /// </summary>

        public void ShowAllRenderers()
        {
            for (int i = 0; i < m_Renderers.Count; ++i)
            {
                if (m_Renderers[i] == null)
                {
                    continue;
                }

                m_Renderers[i].enabled = true;
            }
        }

        /// <summary>
        /// 隐藏所有渲染器。
        /// </summary>
        public void HideAllRenderers()
        {
            for (int i = 0; i < m_Renderers.Count; ++i)
            {
                if (m_Renderers[i] == null)
                {
                    continue;
                }

                m_Renderers[i].enabled = false;
            }
        }

        private void RemoveStoppedDatas()
        {
            var toRemove = new List<ColorChangeData>();

            for (int i = 0; i < m_Pool.Count; ++i)
            {
                var data = m_Pool[i];
                if (data.State == ColorChangeStatus.Stopped)
                {
                    toRemove.Add(data);
                }
            }

            for (int i = 0; i < toRemove.Count; ++i)
            {
                m_Pool.Remove(toRemove[i]);
            }
        }

        private void ResetMaterials()
        {
            UpdateMaterials(DefaultRimColor, DefaultInnerColor, DefaultInnerColorPower, DefaultRimPower, DefaultAlphaPower, DefaultAllPower);
            m_IsReset = true;
        }

        private void UpdateDatas()
        {
            for (int i = 0; i < m_Pool.Count; ++i)
            {
                m_Pool[i].OnUpdate();
            }
        }

        private void UpdateMaterials(Color rimColor, Color innerColor, float innerColorPower, float rimPower, float alphaPower, float allPower)
        {
            //Debug.LogWarningFormat(string.Format("[Time: {6}] rimColor={0}, innerColor={1}, innerColorPower={2}, rimPower={3}, alphaPower={4}, allPower={5}, count={7}",
            //    rimColor, innerColor, innerColorPower, rimPower, alphaPower, allPower, Time.time, m_Pool.Count));

            for (int i = 0; i < m_Renderers.Count; ++i)
            {
                var mr = m_Renderers[i];
                UpdateMaterialsPerRenderer(rimColor, innerColor, innerColorPower, rimPower, alphaPower, allPower, mr);
            }
        }

        private void UpdateMaterialsPerRenderer(Color rimColor, Color innerColor, float innerColorPower, float rimPower, float alphaPower, float allPower, Renderer mr)
        {
            var mat = mr.material;

            // TODO: 为啥直接比 Shader 在使用 AssetBundle 的时候不好用？？
            if (mat.shader.name != ShaderName)
            {
                return;
            }

            mat.SetColor(m_RimColorPropertyId, rimColor);
            mat.SetColor(m_InnerColorPropertyId, innerColor);
            mat.SetFloat(m_InnerColorPowerPropertyId, innerColorPower);
            mat.SetFloat(m_RimPowerPropertyId, rimPower);
            mat.SetFloat(m_AlphaPowerPropertyId, alphaPower);
            mat.SetFloat(m_AllPowerPropertyId, allPower);
        }

        private void CalcValuesAndUpdateMaterials()
        {
            m_IsReset = false;

            float totalWeight = DefaultWeight;

            for (int i = 0; i < m_Pool.Count; ++i)
            {
                totalWeight += m_Pool[i].Weight;
            }

            if (totalWeight < 0.001f)
            {
                ResetMaterials();
                return;
            }

            Color rimColor = DefaultRimColor;
            Color innerColor = DefaultInnerColor;
            float innerColorPower = DefaultInnerColorPower;
            float rimPower = DefaultRimPower;
            float alphaPower = DefaultAlphaPower;
            float allPower = DefaultAllPower;

            for (int i = 0; i < m_Pool.Count; ++i)
            {
                var data = m_Pool[i];
                rimColor += data.Weight * data.RimColor / totalWeight;
                innerColor += data.Weight * data.InnerColor / totalWeight;
                innerColorPower = Mathf.Max(innerColorPower, data.InnerColorPower);
                rimPower = Mathf.Max(rimPower, data.RimPower);
                alphaPower = Mathf.Max(alphaPower, data.AlphaPower);
                allPower = Mathf.Max(allPower, data.AllPower);
            }

            UpdateMaterials(rimColor, innerColor, innerColorPower, rimPower, alphaPower, allPower);
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_RimColorPropertyId = Shader.PropertyToID("_RimColor");
            m_InnerColorPropertyId = Shader.PropertyToID("_InnerColor");
            m_InnerColorPowerPropertyId = Shader.PropertyToID("_InnerColorPower");
            m_RimPowerPropertyId = Shader.PropertyToID("_RimPower");
            m_AlphaPowerPropertyId = Shader.PropertyToID("_AlphaPower");
            m_AllPowerPropertyId = Shader.PropertyToID("_AllPower");
        }

        private void Update()
        {
            RemoveStoppedDatas();

            if (m_Pool.Count <= 0)
            {
                if (!m_IsReset)
                {
                    ResetMaterials();
                }
                return;
            }

            UpdateDatas();
            CalcValuesAndUpdateMaterials();
        }

        #endregion MonoBehaviour
    }
}

using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 升级进度条控制器。
    /// </summary>
    public class UILevelUpProgressController : MonoBehaviour
    {
        /// <summary>
        /// 动画效果完成的回调。
        /// </summary>
        public event GameFrameworkAction OnComplete;

        /// <summary>
        /// 升级时的回调。
        /// </summary>
        public event GameFrameworkAction<int> OnLevelUp;

        [SerializeField]
        private float m_Speed = 1f;

        [SerializeField]
        private UIProgressBar m_ProgressBar = null;

        [SerializeField]
        private UILabel m_Level = null;

        [SerializeField]
        private UILabel m_Exp = null;

        private int m_BegLevel;
        private int m_BegExp;
        private int m_EndLevel;
        private int m_EndExp;
        private IList<KeyValuePair<int, int>> m_ExpList;
        private int m_SerialId = 0;
        private string m_CurLevelDictionary = string.Empty;

        /// <summary>
        /// 是否正在播放动画。
        /// </summary>
        public bool IsPlaying { get; private set; }

        public bool ProgressBarIsFull { get { return m_ProgressBar.value >= 1; } }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="begLevel">起始等级。</param>
        /// <param name="begExp">起始经验。</param>
        /// <param name="endLevel">终止等级。</param>
        /// <param name="endExp">终止经验。</param>
        /// <param name="expList">经验列表。</param>
        public void Init(int begLevel, int begExp, int endLevel, int endExp, IList<KeyValuePair<int, int>> expList, string dictionaryStr = "UI_TEXT_ITEMNUMBER_WITHOUTPLUS")
        {
            if (endLevel < begLevel || expList.Count != endLevel - begLevel + 1)
            {
                Log.Warning("Invalid data");
                return;
            }

            IsPlaying = false;

            m_BegLevel = begLevel;
            m_BegExp = begExp;
            m_EndLevel = endLevel;
            m_EndExp = endExp;
            m_ExpList = expList;
            m_CurLevelDictionary = dictionaryStr;
            if (m_Level != null)
            {
                m_Level.text = GameEntry.Localization.GetString(m_CurLevelDictionary, m_BegLevel);
            }

            if (m_Exp != null)
            {
                m_Exp.text = GetExpText(m_BegExp, m_ExpList[0].Value);
            }

            if (m_ProgressBar != null)
            {
                m_ProgressBar.value = NumericalCalcUtility.CalcProportion(0, m_ExpList[0].Value, m_BegExp);
            }
        }

        /// <summary>
        /// 刷新进度条。
        /// </summary>
        /// <param name="endLevel">终止等级。</param>
        /// <param name="endExp">终止经验。</param>
        /// <param name="addExpList">添加的经验列表。</param>
        public void RefreshProgress(int endLevel, int endExp, IList<KeyValuePair<int, int>> addExpList)
        {
            if (!IsPlaying)
            {
                Log.Warning("Cannot refresh progress when not playing progress animation.");
                return;
            }

            if (endLevel < m_EndLevel || (endLevel == m_EndLevel && endExp < m_EndExp))
            {
                Log.Warning("Invalid data");
                return;
            }

            if (m_ExpList.Count + addExpList.Count != endLevel - m_BegLevel + 1)
            {
                Log.Warning("Invalid data");
                return;
            }

            m_EndLevel = endLevel;
            m_EndExp = endExp;

            for (int i = 0; addExpList != null && i < addExpList.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < m_ExpList.Count; ++j)
                {
                    if (m_ExpList[j].Key == addExpList[i].Key)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    continue;
                }

                m_ExpList.Add(addExpList[i]);
            }

        }

        /// <summary>
        /// 播放动画。
        /// </summary>
        public void Play()
        {
            StartCoroutine(PlayCo(++m_SerialId));
        }

        /// <summary>
        /// 停止动画。
        /// </summary>
        public void Stop()
        {
            if (!IsPlaying)
            {
                return;
            }

            IsPlaying = false;
        }

        private void SkipToEnd()
        {
            RefreshExpLabel(m_EndExp, m_ExpList[m_ExpList.Count - 1].Value);
            RefreshProgressBar(m_EndExp, m_ExpList[m_ExpList.Count - 1].Value);
        }

        private IEnumerator PlayCo(int serialId)
        {
            IsPlaying = true;
            int currentLevel = m_BegLevel;
            float currentExp = (float)m_BegExp;
            int currentMaxExp = m_ExpList[0].Value;
            //Log.Info("[UILevelUpProgressController PlayCo] start, serialId={0}.", serialId.ToString());

            // 循环条件：
            // a) 未达到目标等级；或
            // b) 达到了目标等级但经验值还没有加满/加超。
            while (currentLevel < m_EndLevel || (currentExp < m_EndExp && currentExp < m_ExpList[m_ExpList.Count - 1].Value))
            {
                if (!IsPlaying)
                {
                    SkipToEnd();
                    yield break;
                }
                
                yield return null;

                if (serialId != m_SerialId)
                {
                    yield break;
                }

                float deltaExp = currentMaxExp * m_Speed * Time.unscaledDeltaTime;


                if (currentLevel == m_EndLevel && currentExp + deltaExp > currentMaxExp) // 已达目标等级并且已经加超经验。
                {
                    //Log.Info("[UILevelUpProgressController PlayCo] End level, exceeding max exp.");
                    currentExp = currentMaxExp;
                }
                else if (currentLevel == m_EndLevel && currentExp + deltaExp >= m_EndExp) // 已达目标等级并且已经加足经验。
                {
                    //Log.Info("[UILevelUpProgressController PlayCo] End level, reach end exp.");
                    currentExp = m_EndExp;
                }
                else if (currentLevel != m_EndLevel && currentExp + deltaExp > currentMaxExp) // 未达目标等级并且已经加超经验，级别应提高一级。
                {
                    //Log.Info("[UILevelUpProgressController PlayCo] Should level up.");
                    currentExp = 0;
                    currentLevel++;
                    CallOnLevelUp(currentLevel);
                    currentMaxExp = m_ExpList[currentLevel - m_BegLevel].Value;
                    RefreshLevelLabel(currentLevel);
                }
                else // 不论是否已达到目标等级，经验不会加超，所以直接增加经验值。
                {
                    currentExp += deltaExp;
                    //Log.Info("[UILevelUpProgressController PlayCo] Add exp {0}.", deltaExp.ToString());
                }

                //Log.Info("[UILevelUpProgressController PlayCo] Refresh exp and progress bar: currentExp={0}, currentMaxExp={1}.", currentExp.ToString(), currentMaxExp.ToString());
                RefreshExpLabel(Mathf.RoundToInt(currentExp), currentMaxExp);
                RefreshProgressBar(currentExp, currentMaxExp);
            }

            SkipToEnd();
            IsPlaying = false;
            CallOnComplete();
        }

        private void RefreshProgressBar(float currentExp, int currentMaxExp)
        {
            if (m_ProgressBar != null)
            {
                m_ProgressBar.value = NumericalCalcUtility.CalcProportion(0, currentMaxExp, (float)currentExp);
            }
        }

        private void RefreshExpLabel(int currentExp, int currentMaxExp)
        {
            if (m_Exp != null)
            {
                m_Exp.text = GetExpText(currentExp, currentMaxExp);
            }
        }

        private void RefreshLevelLabel(int currentLevel)
        {
            if (m_Level != null)
            {
                m_Level.text = GameEntry.Localization.GetString(m_CurLevelDictionary, currentLevel);
            }
        }

        private void CallOnComplete()
        {
            if (OnComplete != null)
            {
                OnComplete();
            }
        }

        private void CallOnLevelUp(int newLevel)
        {
            if (OnLevelUp != null)
            {
                OnLevelUp(newLevel);
            }
        }

        private string GetExpText(int curExp, int maxExp)
        {
            return GameEntry.Localization.GetString("UI_TEXT_SLASH", curExp.ToString(), maxExp.ToString());
        }
    }
}

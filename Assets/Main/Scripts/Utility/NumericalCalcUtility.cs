using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 数值计算工具类。
    /// </summary>
    public static class NumericalCalcUtility
    {
        /// <summary>
        /// 对一组浮点数按制定策略求值。
        /// </summary>
        /// <param name="originalData">输入数据</param>
        /// <param name="strategy">计算策略</param>
        /// <returns>计算结果</returns>
        public static float CalcFloat(float[] originalData, NumericalCalcStrategy strategy)
        {
            switch (strategy)
            {
                case NumericalCalcStrategy.Sum:
                    return CalcFloatSum(originalData);
                case NumericalCalcStrategy.Min:
                    return Mathf.Min(originalData);
                case NumericalCalcStrategy.Max:
                    return Mathf.Max(originalData);
                case NumericalCalcStrategy.ArithmeticAverage:
                    return CalcFloatArithmeticAverage(originalData);
                default:
                    throw new Exception(string.Format("Numerical calculation strategy '{0}' doesn't exist.", strategy.ToString()));
            }
        }

        public static float CalcFloatSum(float[] originalData)
        {
            var sum = 0f;
            for (int i = 0; i < originalData.Length; ++i)
            {
                sum += originalData[i];
            }
            return sum;
        }

        public static float CalcFloatArithmeticAverage(float[] originalData)
        {
            var ave = 0f;
            for (int i = 0; i < originalData.Length; ++i)
            {
                ave += originalData[i] / originalData.Length;
            }
            return ave;
        }

        /// <summary>
        /// 根据基础值，先加上增量值，然后再乘以 (1 + 增加率)。用于整型数值。
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="increseAmount">增量值</param>
        /// <param name="increaseRate">增加率</param>
        /// <returns>计算结果</returns>
        public static int CalcIntProperty(int baseVal, int increseAmount, float increaseRate)
        {
            return Mathf.RoundToInt((baseVal + increseAmount) * (1f + increaseRate));
        }

        /// <summary>
        /// 根据基础值，先加上增量值，然后再乘以 (1 + 增加率)。用于浮点型数值。
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="increseAmount">增量值</param>
        /// <param name="increaseRate">增加率</param>
        /// <returns>计算结果</returns>
        public static float CalcFloatProperty(float baseVal, float increaseAmount, float increaseRate)
        {
            return (baseVal + increaseAmount) * (1f + increaseRate);
        }

        /// <summary>
        /// 根据基础值，先乘以 (1 + 增加率)，再加上增量值。用于整型数值。
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="increseAmount">增量值</param>
        /// <param name="increaseRate">增加率</param>
        /// <returns>计算结果</returns>
        public static int CalcIntProperty2(int baseVal, int increseAmount, float increaseRate)
        {
            return Mathf.RoundToInt(baseVal * (1f + increaseRate)) + increseAmount;
        }

        /// <summary>
        /// 根据基础值，先乘以 (1 + 增加率)，再加上增量值。用于浮点型数值。
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="increseAmount">增量值</param>
        /// <param name="increaseRate">增加率</param>
        /// <returns>计算结果</returns>
        public static float CalcFloatProperty2(float baseVal, float increaseAmount, float increaseRate)
        {
            return baseVal * (1f + increaseRate) + increaseAmount;
        }

        /// <summary>
        /// 计算定比分点的比值
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">终点</param>
        /// <param name="point">当前点</param>
        /// <returns>比值</returns>
        public static float CalcProportion(int from, int to, int point)
        {
            if (from == to)
            {
                throw new DivideByZeroException();
            }

            return ((float)(point - from)) / (to - from);
        }

        /// <summary>
        /// 计算定比分点的比值
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">终点</param>
        /// <param name="point">当前点</param>
        /// <returns>比值</returns>
        public static float CalcProportion(float from, float to, float point)
        {
            if (from == to)
            {
                throw new DivideByZeroException();
            }

            return (point - from) / (to - from);
        }
    }
}

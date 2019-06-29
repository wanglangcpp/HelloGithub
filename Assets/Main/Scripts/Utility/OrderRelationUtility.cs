using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 序关系工具类。
    /// </summary>
    public static class OrderRelationUtility
    {
        /// <summary>
        /// 判定两个输入数据是否满足特定序关系。
        /// </summary>
        /// <typeparam name="T">输入数据类型</typeparam>
        /// <param name="a">数据一</param>
        /// <param name="b">数据二</param>
        /// <param name="relation">序关系</param>
        /// <returns>是否满足</returns>
        public static bool AreSatisfying<T>(T a, T b, OrderRelationType relation)
            where T : IComparable<T>
        {
            switch (relation)
            {
                case OrderRelationType.EqualTo:
                    return a.CompareTo(b) == 0;
                case OrderRelationType.NotEqualTo:
                    return a.CompareTo(b) != 0;
                case OrderRelationType.GreaterThan:
                    return a.CompareTo(b) > 0;
                case OrderRelationType.GreaterThanOrEqualTo:
                    return a.CompareTo(b) >= 0;
                case OrderRelationType.LessThan:
                    return a.CompareTo(b) < 0;
                case OrderRelationType.LessThanOrEqualTo:
                    return a.CompareTo(b) <= 0;
                default:
                    throw new Exception(string.Format("Order relation '{0}' doesn't exist.", relation));
            }
        }
    }
}

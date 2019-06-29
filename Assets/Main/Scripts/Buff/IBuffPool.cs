using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// Buff 池。
    /// </summary>
    public interface IBuffPool
    {
        /// <summary>
        /// 获取 Buff 数据。
        /// </summary>
        BuffData[] Buffs { get; }

        /// <summary>
        /// 获取 Buff 作用对象。
        /// </summary>
        IBuffTargetData TargetData { get; }

        /// <summary>
        /// 更新 Buff 池。
        /// </summary>
        /// <param name="currentTime"></param>
        void Update(float currentTime);

        /// <summary>
        /// 添加 Buff。
        /// </summary>
        /// <param name="buffId">Buff 编号。</param>
        /// <param name="ownerData">拥有者数据。</param>
        /// <param name="buffSerialId">序列号。</param>
        /// <param name="startTime">起始时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Add(int buffId, EntityData ownerData, long buffSerialId, float startTime, object userData = null);

        /// <summary>
        /// 添加传递来的 Buff。
        /// </summary>
        /// <param name="buffData">原 Buff 数据。</param>
        void AddTransferred(BuffData buffData);

        /// <summary>
        /// 根据类型移除 Buff。
        /// </summary>
        /// <param name="buffType">Buff 类型。</param>
        void RemoveByType(BuffType buffType);

        /// <summary>
        /// 根据编号移除 Buff。
        /// </summary>
        /// <param name="ids">待移除的 Buff 编号。</param>
        void RemoveByIds(IList<int> ids);

        /// <summary>
        /// 根据类型获取 Buff 数据。
        /// </summary>
        /// <param name="buffType">Buff 类型。</param>
        /// <returns>获取到的 Buff 数据。</returns>
        BuffData GetByType(BuffType buffType);

        /// <summary>
        /// 根据编号获取 Buff 数据。
        /// </summary>
        /// <param name="id">Buff 编号。</param>
        /// <returns>获取到的 Buff 数据。</returns>
        BuffData GetById(int id);

        /// <summary>
        /// 清除 Buff。
        /// </summary>
        void Clear();
    }
}

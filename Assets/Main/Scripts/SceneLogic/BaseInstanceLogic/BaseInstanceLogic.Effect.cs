using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        private IDictionary<int, int> m_ManagedEffectKeysToEntityIds = new Dictionary<int, int>();

        /// <summary>
        /// 显示托管的特效。
        /// </summary>
        /// <param name="key">在副本中的关键字。</param>
        /// <param name="effectId">在 Effect 表中的编号。</param>
        /// <param name="position2D">平面位置。</param>
        /// <param name="rotation">旋转角度。</param>
        /// <returns>特效实体编号。</returns>
        public int ShowManagedEffect(int key, int effectId, Vector2 position2D, float rotation)
        {
            if (m_ManagedEffectKeysToEntityIds.ContainsKey(key))
            {
                Log.Warning("Effect of the same key '{0}' is already displayed or being displayed.", key.ToString());
                return 0;
            }

            var dt = GameEntry.DataTable.GetDataTable<DREffect>();
            DREffect dr = dt.GetDataRow(effectId);
            if (dr == null)
            {
                Log.Warning("Effect '{0}' not found.", effectId.ToString());
                return 0;
            }

            int entityId = GameEntry.Entity.GetSerialId();

            var effectData = new EffectData(entityId, dr.ResourceName, position2D.ToVector3(), rotation);
            effectData.NeedSamplePosition = true;
            effectData.UserData = new ManagedEffectUserData { Key = key };

            m_ManagedEffectKeysToEntityIds.Add(key, entityId);
            GameEntry.Entity.ShowEffect(effectData);
            return entityId;
        }

        /// <summary>
        /// 隐藏托管的特效。
        /// </summary>
        /// <param name="key">在副本中的关键字。</param>
        /// <returns>是否成功隐藏。</returns>
        public bool HideManagedEffect(int key)
        {
            int entityId;
            if (!m_ManagedEffectKeysToEntityIds.TryGetValue(key, out entityId))
            {
                return false;
            }

            m_ManagedEffectKeysToEntityIds.Remove(key);
            if (!GameEntry.Entity.HasEntity(entityId) && !GameEntry.Entity.IsLoadingEntity(entityId))
            {
                return false;
            }

            GameEntry.Entity.HideEntity(entityId);
            return true;
        }

        private void OnShowEffectFailure(EffectData effectData)
        {
            var managedEffectUserData = effectData.UserData as ManagedEffectUserData;
            if (effectData.UserData == null)
            {
                return;
            }

            m_ManagedEffectKeysToEntityIds.Remove(managedEffectUserData.Key);
        }

        private void OnHideManagedEffectComplete(ManagedEffectUserData managedEffectUserData)
        {
            if (managedEffectUserData == null)
            {
                return;
            }

            m_ManagedEffectKeysToEntityIds.Remove(managedEffectUserData.Key);
        }

        [Serializable]
        private class ManagedEffectUserData
        {
            public int Key;
        }
    }
}

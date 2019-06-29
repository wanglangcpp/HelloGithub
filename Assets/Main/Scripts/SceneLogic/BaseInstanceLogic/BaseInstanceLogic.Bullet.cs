using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        private Dictionary<int, Bullet> m_Bullets = new Dictionary<int, Bullet>();

        protected IList<Bullet> GetBullets()
        {
            List<Bullet> bullets = new List<Bullet>();
            foreach (var kv in m_Bullets)
            {
                bullets.Add(kv.Value);
            }

            return bullets;
        }

        /// <summary>
        /// 显示子弹。
        /// </summary>
        /// <param name="bulletId">子弹编号。</param>
        /// <param name="owner">所有者。</param>
        /// <param name="ownerSkillIndex">所有者技能索引。</param>
        /// <param name="ownerSkillLevel">所有者技能等级。</param>
        /// <returns>是否成功。</returns>
        public bool ShowBullet(int bulletId, Entity owner, int ownerSkillIndex, int ownerSkillLevel)
        {
            IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet>();
            DRBullet dataRow = dtBullet.GetDataRow(bulletId);
            if (dataRow == null)
            {
                Log.Warning("Can not find bullet '{0}'.", bulletId.ToString());
                return false;
            }

            BulletData bulletData = new BulletData(GameEntry.Entity.GetSerialId());
            bulletData.BulletId = bulletId;

            bulletData.OwnerId = owner != null ? owner.Id : 0;
            SetBulletTransformData(owner, dataRow, bulletData);

            bulletData.Height = dataRow.Height;
            bulletData.Speed = dataRow.Speed;
            bulletData.TransformParentType = dataRow.TransformParentType;
            bulletData.IsReboundable = dataRow.IsReboundable;
            bulletData.OwnerSkillIndex = ownerSkillIndex;
            bulletData.OwnerSkillLevel = ownerSkillLevel;

            ICampable campedOwner = owner as ICampable;
            bulletData.Camp = campedOwner != null ? campedOwner.Camp : CampType.Neutral;
            SetBulletAtkDfsData(owner, bulletData);
            GameEntry.Entity.ShowBullet(bulletData);

            return true;
        }

        /// <summary>
        /// 替换子弹。
        /// </summary>
        /// <param name="bulletId">要替换的子弹编号。</param>
        /// <param name="originalBullet">源子弹实体。</param>
        /// <returns>是否成功。</returns>
        public bool ReplaceBullet(int bulletId, Bullet originalBullet)
        {
            if (originalBullet == null || !originalBullet.IsAvailable)
            {
                Log.Warning("Original bullet is invalid.");
                return false;
            }

            IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet>();
            DRBullet dataRow = dtBullet.GetDataRow(bulletId);
            if (dataRow == null)
            {
                Log.Warning("Can not find bullet '{0}'.", bulletId.ToString());
                return false;
            }

            BulletData bulletData = new BulletData(GameEntry.Entity.GetSerialId());
            bulletData.BulletId = bulletId;

            BulletData originalBulletData = originalBullet.Data;
            var ownerEntity = GameEntry.Entity.GetEntity(originalBulletData.OwnerId);
            Entity owner = ownerEntity == null ? null : (ownerEntity.Logic as Entity);

            bulletData.OwnerId = owner != null ? owner.Id : 0;

            // 位置和旋转均由源子弹决定，忽略 dataRow 中的 TransformType。
            bulletData.Position = originalBulletData.Position;
            bulletData.Rotation = originalBulletData.Rotation;
            bulletData.TransformParentType = dataRow.TransformParentType;

            bulletData.OwnerSkillIndex = originalBulletData.OwnerSkillIndex;
            bulletData.OwnerSkillLevel = originalBulletData.OwnerSkillLevel;

            bulletData.Height = dataRow.Height;
            bulletData.Speed = dataRow.Speed;

            CopyBulletAtkDfsData(originalBulletData, bulletData);

            ICampable campedOwner = owner as ICampable;
            bulletData.Camp = campedOwner != null ? campedOwner.Camp : CampType.Neutral;

            GameEntry.Entity.ShowBullet(bulletData);
            GameEntry.Entity.HideEntity(originalBullet.Entity);

            return true;
        }

        /// <summary>
        /// 注册子弹实体。
        /// </summary>
        /// <param name="bullet">子弹实体。</param>
        public void RegisterBullet(Bullet bullet)
        {
            m_Bullets.Add(bullet.Id, bullet);
        }

        /// <summary>
        /// 注销子弹实体。
        /// </summary>
        /// <param name="bulletEntityId">子弹实体编号。</param>
        /// <returns></returns>
        public bool UnregisterBullet(int bulletEntityId)
        {
            return m_Bullets.Remove(bulletEntityId);
        }

        private static void SetBulletTransformData(Entity owner, DRBullet dataRow, BulletData bulletData)
        {
            Vector2 offset = new Vector2(dataRow.OffsetX, dataRow.OffsetY);

            Building buildingOwner = owner as Building;
            ICanHaveTarget canHaveTargetOwner = owner as ICanHaveTarget;

            Transform transformToRef = null;

            switch (dataRow.TransformType)
            {
                case TransformType.RelativeToOwner:
                    if (buildingOwner != null && buildingOwner.ShooterTransform != null)
                    {
                        transformToRef = buildingOwner.ShooterTransform;
                    }
                    else if (owner != null)
                    {
                        transformToRef = owner.CachedTransform;
                    }
                    break;
                case TransformType.RelativeToTarget:
                    if (canHaveTargetOwner == null || !canHaveTargetOwner.HasTarget)
                    {
                        transformToRef = null;
                    }
                    else
                    {
                        transformToRef = (canHaveTargetOwner.Target as Entity).CachedTransform;
                    }
                    break;
                case TransformType.Default:
                    transformToRef = null;
                    break;
            }

            if (transformToRef == null)
            {
                bulletData.Position = offset;
                bulletData.Rotation = dataRow.Angle;
            }
            else
            {
                bulletData.Position = transformToRef.TransformPoint(offset.ToVector3()).ToVector2();
                bulletData.Rotation = transformToRef.eulerAngles.y + dataRow.Angle;
            }
        }

        private static void SetBulletAtkDfsData(Entity owner, BulletData bulletData)
        {
            if (owner == null)
            {
                return;
            }

            var ownerData = owner.Data as IImpactDataProvider;
            CopyBulletAtkDfsData(ownerData, bulletData);
        }

        private static void CopyBulletAtkDfsData(IImpactDataProvider src, BulletData dst)
        {
            dst.PhysicalAttack = src.PhysicalAttack;
            dst.PhysicalDefense = src.PhysicalDefense;
            dst.MagicAttack = src.MagicAttack;
            dst.MagicDefense = src.MagicDefense;
            dst.OppPhysicalDfsReduceRate = src.OppPhysicalDfsReduceRate;
            dst.OppMagicDfsReduceRate = src.OppMagicDfsReduceRate;
            dst.PhysicalAtkHPAbsorbRate = src.PhysicalAtkHPAbsorbRate;
            dst.MagicAtkHPAbsorbRate = src.MagicAtkHPAbsorbRate;
            dst.PhysicalAtkReflectRate = src.PhysicalAtkReflectRate;
            dst.MagicAtkReflectRate = src.MagicAtkReflectRate;
            dst.DamageReductionRate = src.DamageReductionRate;
            dst.CriticalHitProb = src.CriticalHitProb;
            dst.CriticalHitRate = src.CriticalHitRate;
            dst.AntiCriticalHitProb = src.AntiCriticalHitProb;
            dst.DamageRandomRate = src.DamageRandomRate;
            dst.AdditionalDamage = src.AdditionalDamage;
        }
    }
}

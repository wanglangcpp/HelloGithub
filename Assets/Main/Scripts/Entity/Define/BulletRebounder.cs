using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 子弹反弹器。
    /// </summary>
    public class BulletRebounder : ShedObject
    {
        [SerializeField]
        protected BulletRebounderData m_BulletRebounderData = null;

        public new BulletRebounderData Data
        {
            get
            {
                return m_BulletRebounderData;
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_BulletRebounderData = userData as BulletRebounderData;
            if (m_BulletRebounderData == null)
            {
                Log.Error("Bullet rebounder data is invalid.");
                return;
            }
        }

        private void ReboundBulletIfNeeded(Bullet bullet)
        {
            if (!bullet.IsReboundable)
            {
                return;
            }

            // 此处仅将 forward 作为法向量，将入射子弹的运动方向，依照反射定律改变，精度不高。
            // 可能的改进方案：限制子弹和反弹器的碰撞器形状（圆和方），然后估算碰撞点的位置，它离哪个面近就按哪个面的法线来反射。
            var normal = CachedTransform.forward.ToVector2().normalized;
            var bulletForward = bullet.CachedTransform.forward.ToVector2().normalized;
            var innerProduct = Vector2.Dot(normal, bulletForward);
            if (Vector2.Dot(normal, bulletForward) >= 0f)
            {
                return;
            }

            var bulletNewForward = bulletForward - 2 * innerProduct * normal;
            bullet.CachedTransform.LookAt2D(bullet.CachedTransform.position.ToVector2() + bulletNewForward);
        }

        #region MonoBehaviour

        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                ReboundBulletIfNeeded(bullet);
            }
        }

        #endregion MonoBehaviour
    }
}

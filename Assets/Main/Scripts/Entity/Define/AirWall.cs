using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    [RequireComponent(typeof(NavMeshObstacle))]
    public class AirWall : ShedObject
    {
        [SerializeField]
        protected AirWallData m_AirWallData = null;

        private NavMeshObstacle m_CachedObstacle = null;

        public new AirWallData Data
        {
            get
            {
                return m_AirWallData;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CachedObstacle = GetComponent<NavMeshObstacle>();
            if (m_CachedObstacle == null)
            {
                Log.Warning("Can not find NavMeshObstacle in '{0}'.", Name);
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_AirWallData = userData as AirWallData;
            if (m_AirWallData == null)
            {
                Log.Error("Air wall data is invalid.");
                return;
            }

            EnableObstacle(true);
        }

        public bool EnableObstacle(bool enabled)
        {
            if (m_CachedObstacle.enabled == enabled)
            {
                return false;
            }

            m_CachedObstacle.enabled = enabled;
            return true;
        }

        public void PlayAnimation(string animationName)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                Log.Warning("Air wall animation name is invalid.");
                return;
            }

            AnimationState animationState = CachedAnimation[animationName];
            if (animationState == null)
            {
                Log.Warning("Can not find animation '{0}' for air wall '{1}'.", animationName, Data.AirWallId.ToString());
                return;
            }

            CachedAnimation.Play(animationName);
        }
    }
}

using UnityEngine;

namespace Genesis.GameClient
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);

            public const string TargetableObjectLayerName = "Targetable Object";
            public static readonly int TargetableObjectLayerId = LayerMask.NameToLayer(TargetableObjectLayerName);

            public const string StaticColliderLayerName = "Static Collider";
            public static readonly int StaticColliderLayerId = LayerMask.NameToLayer(StaticColliderLayerName);

            public const string ColliderTriggerLayerName = "Collider Trigger";
            public static readonly int ColliderTriggerLayerId = LayerMask.NameToLayer(ColliderTriggerLayerName);

            public const string RegionTriggerLayerName = "Region Trigger";
            public static readonly int RegionTriggerLayerId = LayerMask.NameToLayer(RegionTriggerLayerName);

            public const string UIModelLayerName = "UI Model";
            public static readonly int UIModelLayerId = LayerMask.NameToLayer(UIModelLayerName);

            public const string AffectedByProjectorLayerName = "Affected By Projector";
            public static readonly int AffectedByProjectorLayerId = LayerMask.NameToLayer(AffectedByProjectorLayerName);

            /// <summary>
            /// 无条件忽略光照。场景实时光源应避免对该层物体起作用。
            /// </summary>
            public const string IgnoreLightLayerName = "Ignore Light";
            public static readonly int IgnoreLightLayerId = LayerMask.NameToLayer(IgnoreLightLayerName);

            public const string OcclusionColliderLayerName = "Occlusion Collider";
            public static readonly int OcclusionColliderLayerId = LayerMask.NameToLayer(OcclusionColliderLayerName);

            public const string BulletRebounderLayerName = "Bullet Rebounder";
            public static readonly int BulletRebounderLayerId = LayerMask.NameToLayer(BulletRebounderLayerName);

            public const string PortalTriggerLayerName = "Portal Trigger";
            public static readonly int PortalTriggerLayerId = LayerMask.NameToLayer(PortalTriggerLayerName);

            /// <summary>
            /// 不参与烘焙的场景物体。场景实时光源应对该层物体其作用。
            /// </summary>
            public const string UnbakedSceneObjectLayerName = "Unbaked Scene Object";
            public static readonly int UnbakedSceneObjectLayerId = LayerMask.NameToLayer(UnbakedSceneObjectLayerName);
        }
    }
}

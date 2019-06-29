using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 占位控件。用于将特效等的材质插入 NGUI 渲染过程中，会牺牲一些 Draw Call。
    /// </summary>
    public class UIFakeWidget : UIWidget
    {
        private Renderer m_CachedRenderer = null;
        private Material m_CachedMaterial = null;
        private int m_MaterialIndex = -1;

        public override Material material
        {
            get
            {
                return RealMaterial;
            }
        }

        private Material RealMaterial
        {
            get
            {
                return m_CachedRenderer.materials[m_MaterialIndex];
            }

            set
            {
                var materials = m_CachedRenderer.materials;
                materials[m_MaterialIndex] = value;
                m_CachedRenderer.materials = materials;
            }
        }

        public void Init(Renderer renderer, int materialIndex, int depth, int width, int height)
        {
            m_CachedRenderer = renderer;
            m_CachedRenderer.sortingOrder = 0;
            m_MaterialIndex = materialIndex;
            m_CachedMaterial = RealMaterial;
            this.depth = depth;
            this.width = Mathf.Max(width, minWidth);
            this.height = Mathf.Max(height, minHeight);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnStart()
        {
            base.OnStart();
            UpdateMaterial();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            UpdateMaterial();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            if (m_CachedRenderer != null)
            {
                RealMaterial = m_CachedMaterial;
            }

            base.OnDisable();
        }

        private void UpdateMaterial()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif

            if (drawCall == null || drawCall.dynamicMaterial == null || RealMaterial.renderQueue == drawCall.dynamicMaterial.renderQueue)
            {
                return;
            }

            var lastOffset = RealMaterial.mainTextureOffset;
            var lastScale = RealMaterial.mainTextureScale;
            RealMaterial = drawCall.dynamicMaterial;
            RealMaterial.mainTextureOffset = lastOffset;
            RealMaterial.mainTextureScale = lastScale;
            RealMaterial.name = string.Format("[NGUI] {0}", m_CachedMaterial.name);
        }

        public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
        {
            var transparent = new Color(1, 1, 1, 0f);
            geometry.verts.Add(Vector3.zero - new Vector3(-width / 2, height / 2, 0f));
            geometry.uvs.Add(Vector2.zero);
            geometry.cols.Add(transparent);

            geometry.verts.Add(Vector3.zero - new Vector3(-width / 2, -height / 2, 0f));
            geometry.uvs.Add(new Vector2(0, 1));
            geometry.cols.Add(transparent);

            geometry.verts.Add(Vector3.zero - new Vector3(width / 2, -height / 2, 0f));
            geometry.uvs.Add(new Vector2(1, 1));
            geometry.cols.Add(transparent);

            geometry.verts.Add(Vector3.zero - new Vector3(width / 2, height / 2, 0f));
            geometry.uvs.Add(new Vector2(1, 0));
            geometry.cols.Add(transparent);
        }
    }
}

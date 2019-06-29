using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Genesis.GameClient
{
    public class SceneLightmapData : MonoBehaviour
    {
        public List<LightmapParam> data = new List<LightmapParam>();
        private void Awake()
        {
            Renderer[] renders = transform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < data.Count; i++)
            {
                renders[i].lightmapIndex = data[i].lightmapIndex;
                renders[i].lightmapScaleOffset = data[i].lightmapScaleOffset;
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        [ContextMenu("Store Lightmap Data")]
        void StoreLightmapData()
        {
            Renderer[] renders = null;
            renders = transform.GetComponentsInChildren<Renderer>();
            data = new List<LightmapParam>(renders.Length);
            for (int i = 0; i < renders.Length; i++)
            {
                LightmapParam lightmapParam = renders[i].GetComponent<LightmapParam>();
                if (lightmapParam == null)
                    lightmapParam = renders[i].gameObject.AddComponent<LightmapParam>();
                lightmapParam.lightmapIndex = renders[i].lightmapIndex;
                lightmapParam.lightmapScaleOffset = renders[i].lightmapScaleOffset;
                data.Add(lightmapParam);
            }
        }
        [ContextMenu("Clear Lightmap Data")]
        void ClearLightmapData()
        {
            Renderer[] renders = null;
            renders = transform.GetComponentsInChildren<Renderer>();
            data = new List<LightmapParam>(renders.Length);
            for (int i = 0; i < renders.Length; i++)
            {
                LightmapParam lightmapParam = renders[i].GetComponent<LightmapParam>();
                if (lightmapParam == null)
                    continue;
                DestroyImmediate(lightmapParam);
            }
            data.Clear();
        }
    }
}

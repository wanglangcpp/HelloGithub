using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace Genesis.GameClient.Editor
{
    public class BuildSceneAssetBundle : MonoBehaviour
    {

        private void BuildAssetBundle(string assetName)
        {
            //获取所有Gameobject重新指定父节点
            GameObject assetParent = new GameObject();
            assetParent.name = assetName;
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (assetParent != obj)
                {
                    obj.transform.SetParent(assetParent.transform, true);
                    Renderer r = obj.GetComponent<Renderer>();
                    if (r != null)
                    {
                        LightmapParam lmp = obj.GetComponent<LightmapParam>();
                        if (lmp == null)
                            lmp = obj.AddComponent<LightmapParam>();
                        lmp.lightmapIndex = r.lightmapIndex;
                        lmp.lightmapScaleOffset = r.lightmapScaleOffset;
                    }
                }
            }
            //然后创建prefab保存每个物体的lightmap数据
            string path = "Assets/Main/Scenes/" + assetName + "/" + assetName + ".prefab";
            PrefabUtility.CreatePrefab(path, assetParent, ReplacePrefabOptions.ReplaceNameBased);
            Debug.Log("prefab " + path);



            //AssetBundleBuild[] abb = new AssetBundleBuild[1];
            //abb[0].assetBundleName = assetName;
            //abb[0].assetNames = new string[]{ path};
            //BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", abb);



            string curScene = EditorApplication.currentScene;
            string[] parts = curScene.Split('/', '\\');
            string sceneName = parts[parts.Length - 1].Split('.')[0];
            string lightmapPath = Path.GetDirectoryName(curScene) + "/" + sceneName + "/";
            List<string> assetList = new List<string>();
            for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
            {
                LightmapData lmd = LightmapSettings.lightmaps[i];
                string lmp = lightmapPath + "Lightmap-" + i + "_comp_light.exr";
                assetList.Add(lmp);
                Debug.Log(lmp + " " + File.Exists(lmp));
            }
            Debug.Log(assetList[0] + " " + assetList.Count);



            //string path = "Assets/AssetPrefabs/" + assetName + ".prefab";

            assetList.Add(path);

            AssetBundleBuild[] abb = new AssetBundleBuild[1]; // *************************************** lightmaps 2
            abb[0].assetNames = assetList.ToArray();// new string[]{ path}; // create asset bundle from this prefab
                                                    // build mac
            Debug.Log("make osx bundle");
            abb[0].assetBundleName = assetName + "_osx.unity3d";


            BuildPipeline.BuildAssetBundles("Assets/AssetBundles", abb, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
        }
    }
}

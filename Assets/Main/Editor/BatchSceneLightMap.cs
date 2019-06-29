using UnityEngine;
using System.Collections;
using UnityEditor;

public class BatchSceneLightMap : Editor
{

    [MenuItem("Tools/处理场景LightMap问题")]
    static void CheckSceneLightMap()
    {
        GameObject root = GameObject.Find("Scene");
        CheckGameObjectLightMap(root);

        EditorUtility.DisplayDialog("处理完毕", "", "ok");
    }

    private static void CheckGameObjectLightMap(GameObject o)
    {
        if (o == null)
        {
            return;
        }

        PrefabType prefabType = PrefabUtility.GetPrefabType(o);
        if (prefabType == PrefabType.PrefabInstance)
        {
            Renderer r = o.GetComponent<Renderer>();
            Object findPrefab = PrefabUtility.GetPrefabParent(o);
            string path = AssetDatabase.GetAssetPath(findPrefab);
            r = (Renderer)AssetDatabase.LoadAssetAtPath(path, typeof(Renderer));
            if (r != null && r.lightmapIndex == -1)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(findPrefab);
                r = go.GetComponent<Renderer>();
                r.lightmapIndex = 0;
                r.lightmapScaleOffset = Vector4.zero;
                PrefabUtility.ReplacePrefab(go, findPrefab);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                GameObject.DestroyImmediate(go);
            }
        }
        else
        {
            for (int i = 0; i < o.transform.childCount; i++)
            {
                CheckGameObjectLightMap(o.transform.GetChild(i).gameObject);
            }
        }
    }
}

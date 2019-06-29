using UnityEngine;
using System.Collections;
using UnityEditor;

public class SceneCombineTool
{
    /*
     * SceneRoot
     * ----Lights
     * ----RunCollider
     * ----ObstacleBox
     * ----Static
     * --------Block0
     * --------Block1
     * --------...
     * ----Dynamic
     * --------Objects
     * --------Effects
     * --------Sounds
     * 
     */
    [MenuItem("Tools/Scene Mesh Combine", false, 9999)]
    static public void MenuCombine()
    {
        GameObject sceneRoot = GameObject.Find("SceneRoot");
        if (sceneRoot==null)
        {
            EditorUtility.DisplayDialog("Error!", "请确保存在SceneRoot节点!", "确定");
            return;
        }
        Transform staticRoot = sceneRoot.transform.Find("Static");
        if (staticRoot == null)
        {
            EditorUtility.DisplayDialog("Error!", "请确保存在SceneRoot/Static节点!", "确定");
            return;
        }
        if (staticRoot.gameObject.isStatic==false)
        {
            EditorUtility.DisplayDialog("Error!", "请确保SceneRoot/Static节点被勾选static!", "确定");
            return;
        }
        for (int i = 0; i < staticRoot.childCount; i++)
        {
            Transform block = staticRoot.Find("Block" + i);
            if (block==null)
            {
                Debug.LogError("Block" + i + "不存在!");
            }
            if (block.gameObject.isStatic==false)
            {
                Debug.LogError("Block" + i + "没有设置为静态!");
            }
            SimpleMeshCombine meshCombine = block.GetComponent<SimpleMeshCombine>();
            if (meshCombine==null)
            {
                meshCombine = block.gameObject.AddComponent<SimpleMeshCombine>();
            }
            meshCombine.generateLightmapUV = true;
            if(meshCombine.combined==null)
            {
                meshCombine.CombineMeshes();
            }
            
        }

    }
    [MenuItem("Tools/Scene Mesh Release", false, 10000)]
    static public void MenuRelease()
    {
        GameObject sceneRoot = GameObject.Find("SceneRoot");
        if (sceneRoot == null)
        {
            EditorUtility.DisplayDialog("Error!", "请确保存在SceneRoot节点!", "确定");
            return;
        }
        Transform staticRoot = sceneRoot.transform.Find("Static");
        if (staticRoot == null)
        {
            EditorUtility.DisplayDialog("Error!", "请确保存在SceneRoot/Static节点!", "确定");
            return;
        }
        if (staticRoot.gameObject.isStatic == false)
        {
            EditorUtility.DisplayDialog("Error!", "请确保SceneRoot/Static节点被勾选static!", "确定");
            return;
        }
        for (int i = 0; i < staticRoot.childCount; i++)
        {
            Transform block = staticRoot.Find("Block" + i);
            if (block == null)
            {
                Debug.LogError("Block" + i + "不存在!");
            }
            if (block.gameObject.isStatic == false)
            {
                Debug.LogError("Block" + i + "没有设置为静态!");
            }
            SimpleMeshCombine meshCombine = block.GetComponent<SimpleMeshCombine>();
            if (meshCombine == null)
            {
                meshCombine = block.gameObject.AddComponent<SimpleMeshCombine>();
            }
            meshCombine.generateLightmapUV = true;
            if (meshCombine.combined != null)
            {
                meshCombine.RealseCombine();
            }

        }

    }

}

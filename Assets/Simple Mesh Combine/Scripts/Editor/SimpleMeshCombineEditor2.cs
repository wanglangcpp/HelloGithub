using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SimpleMeshCombine))]
public class SimpleMeshCombineEditor2 : Editor
{
    void ExportMesh(MeshFilter meshFilter, string folder, string filename)
    {
        string path = SaveFile(folder, filename, "obj");
        if (path != null)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(MeshToString(meshFilter));
            sw.Flush();
            sw.Close();
            AssetDatabase.Refresh();
            Debug.Log("Exported OBJ file to folder: " + path);
        }
    }

    string MeshToString(MeshFilter meshFilter)
    {
        Mesh sMesh = meshFilter.sharedMesh;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("g ").Append(meshFilter.name).Append("\n");
        foreach (Vector3 vert in sMesh.vertices)
        {
            Vector3 tPoint = meshFilter.transform.TransformPoint(vert);
            stringBuilder.Append(string.Format("v {0} {1} {2}\n", -tPoint.x, tPoint.y, tPoint.z));
        }
        stringBuilder.Append("\n");
        foreach (Vector3 norm in sMesh.normals)
        {
            Vector3 tDir = meshFilter.transform.TransformDirection(norm);
            stringBuilder.Append(string.Format("vn {0} {1} {2}\n", -tDir.x, tDir.y, tDir.z));
        }
        stringBuilder.Append("\n");
        foreach (Vector3 uv in sMesh.uv)
        {
            stringBuilder.Append(string.Format("vt {0} {1}\n", uv.x, uv.y));
        }
        for (int material = 0; material < sMesh.subMeshCount; material++)
        {
            stringBuilder.Append("\n");
            int[] tris = sMesh.GetTriangles(material);
            for (int i = 0; i < tris.Length; i += 3)
            {
                stringBuilder.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n", tris[i] + 1, tris[i + 1] + 1, tris[i + 2] + 1));
            }
        }
        return stringBuilder.ToString();
    }

    string SaveFile(string folder, string name, string type)
    {
        var path = EditorUtility.SaveFilePanel("Select Folder ", folder, name, type);
        if (path.Length > 0)
        {
            if (path.Contains("" + Application.dataPath))
            {
                string s = "" + path + "";
                string d = "" + Application.dataPath + "/";
                string p = "Assets/" + s.Remove(0, d.Length);
                bool cancel;
                return p;
            }
            else
            {
                Debug.LogError("Prefab Save Failed: Can't save outside project: " + path);
            }
        }
        return "";
    }

    Texture tex = (Texture)Resources.Load("SMC_Title");
    public override void OnInspectorGUI()
    {
        //
        //	STYLE AND COLOR
        //
        Color color2 = Color.yellow;
        Color color1 = Color.cyan;
        var buttonStyle = new GUIStyle(GUI.skin.button);
        var buttonStyle2 = new GUIStyle(GUI.skin.button);
        var titleStyle = new GUIStyle(GUI.skin.label);
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fixedWidth = 150;
        buttonStyle.fixedHeight = 35;
        buttonStyle.fontSize = 15;
        buttonStyle2.fixedWidth = 200;
        buttonStyle2.fixedHeight = 25;
        buttonStyle2.margin = new RectOffset((int)((Screen.width - 200) * .5), (int)((Screen.width - 200) * .5), 0, 0);
        buttonStyle.margin = new RectOffset((int)((Screen.width - 150) * .5), (int)((Screen.width - 150) * .5), 0, 0);
        titleStyle.fixedWidth = 256;
        titleStyle.fixedHeight = 64;
        titleStyle.margin = new RectOffset((int)((Screen.width - 256) * .5), (int)((Screen.width - 256) * .5), 0, 0);
        var infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 10;
        infoStyle.margin.top = 0;
        infoStyle.margin.bottom = 0;
        GUILayout.Label(tex, titleStyle);

        if (!Application.isPlaying)
        {
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Label("Editor can't combine in play-mode", infoStyle);
            GUILayout.Label("Use SimpleMeshCombine.CombineMeshes();", infoStyle);
            GUI.enabled = false;
        }
        GUILayout.Space(15);
        //
        //	COMBINE MESH AREA
        //
        GUI.color = color1;
        SimpleMeshCombine simpleTarget = target as SimpleMeshCombine;
        if ((simpleTarget.combinedGameOjects == null || simpleTarget.combinedGameOjects.Length == 0))
        {
            if (GUILayout.Button("Combine", buttonStyle))
            {
                if (simpleTarget.transform.childCount > 1) simpleTarget.CombineMeshes();
                simpleTarget.combined.isStatic = true;
            }
        }
        else
        {
            if (GUILayout.Button("Release", buttonStyle))
            {
                simpleTarget.EnableRenderers(true);
                if (simpleTarget.combined) DestroyImmediate(simpleTarget.combined);
                simpleTarget.combinedGameOjects = null;
            }
        }
        GUILayout.Space(5);
        //
        //	SAVE MESH AREA
        //
        if (simpleTarget.combined)
        {
            if (!simpleTarget._canGenerateLightmapUV)
            {
                GUILayout.Label("Warning: Mesh has too high vertex count", EditorStyles.boldLabel);
                GUI.enabled = false;
            }

            if (simpleTarget.combined.GetComponent<MeshFilter>().sharedMesh.name != "")
            {
                GUI.enabled = false;
            }
            else if (!Application.isPlaying)
            {
                GUI.enabled = true;
            }

            if (GUILayout.Button("Save Mesh", buttonStyle2))
            {
                string path = SaveFile("Assets/", simpleTarget.transform.name + " [SMC Asset]", "asset");
                if (path != null)
                {
                    Mesh asset = AssetDatabase.LoadAssetAtPath<Mesh>(path);
                    if (asset == null)
                    {
                        AssetDatabase.CreateAsset(simpleTarget.combined.GetComponent<MeshFilter>().sharedMesh, path);
                    }
                    else
                    {
                        asset.Clear();
                        EditorUtility.CopySerialized(simpleTarget.combined.GetComponent<MeshFilter>().sharedMesh, asset);
                        AssetDatabase.SaveAssets();
                    }
                    simpleTarget.combined.GetComponent<MeshFilter>().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
                    Debug.Log("Saved mesh asset: " + path);
                }
            }
            GUILayout.Space(5);
        }

        if (!Application.isPlaying)
        {
            GUI.enabled = true;
        }

        if (simpleTarget.combined)
        {
            if (GUILayout.Button("Export OBJ", buttonStyle2))
            {
                if (simpleTarget.combined)
                {
                    ExportMesh(simpleTarget.combined.GetComponent<MeshFilter>(), "Assets/", simpleTarget.transform.name + " [SMC Mesh]" + ".obj");
                }
            }
            GUILayout.Space(15);
            //
            // COPY
            //
            GUI.color = color2;
            string bText = "Create Copy";
            if (simpleTarget.combined.GetComponent<MeshFilter>().sharedMesh.name == "")
            {
                bText = bText + " (Saved mesh)";
                GUI.enabled = false;
            }
            else if (!Application.isPlaying)
            {
                GUI.enabled = true;
            }

            if (GUILayout.Button(bText, buttonStyle2))
            {
                var newCopy = new GameObject();
                var newCopy2 = new GameObject();
                newCopy2.transform.parent = newCopy.transform;
                newCopy2.transform.localPosition = simpleTarget.combined.transform.localPosition;
                newCopy2.transform.localRotation = simpleTarget.combined.transform.localRotation;
                newCopy.name = simpleTarget.name + " [SMC Copy]";
                newCopy2.name = "Mesh [SMC]";
                newCopy.transform.position = simpleTarget.transform.position;
                newCopy.transform.rotation = simpleTarget.transform.rotation;


                var mf = newCopy2.AddComponent<MeshFilter>();
                newCopy2.AddComponent<MeshRenderer>();
                mf.sharedMesh = simpleTarget.combined.GetComponent<MeshFilter>().sharedMesh;

                simpleTarget.copyTarget = newCopy;
                CopyMaterials(newCopy2.transform);
                CopyColliders();
                Selection.activeTransform = newCopy.transform;

            }


            GUILayout.Space(5);
            if (!simpleTarget.copyTarget)
            {
                GUI.enabled = false;
            }
            else if (!Application.isPlaying)
            {
                GUI.enabled = true;
            }
            if (GUILayout.Button("Copy Colliders", buttonStyle2))
            {
                CopyColliders();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Copy Materials", buttonStyle2))
            {
                CopyMaterials(simpleTarget.copyTarget.transform.FindChild("Mesh [SMC]"));
            }

            if (!Application.isPlaying)
            {
                GUI.enabled = true;
            }
            simpleTarget.destroyOldColliders = EditorGUILayout.Toggle("Destroy old colliders", simpleTarget.destroyOldColliders);
            simpleTarget.keepStructure = EditorGUILayout.Toggle("Keep collider structure", simpleTarget.keepStructure);
            simpleTarget.copyTarget = (GameObject)EditorGUILayout.ObjectField("Copy to: ", simpleTarget.copyTarget, typeof(GameObject), true);
        }

        if (!simpleTarget.combined)
        {
            simpleTarget.generateLightmapUV = EditorGUILayout.Toggle("Create Lightmap UV", simpleTarget.generateLightmapUV);
        }

        GUILayout.Space(5);
        GUI.color = color1;
        EditorGUILayout.BeginVertical("Box");
        if (simpleTarget.combined)
        {
            GUILayout.Label("Vertex count: " + simpleTarget.vCount + " / 65536", infoStyle);
            GUILayout.Label("Material count: " + simpleTarget.combined.GetComponent<Renderer>().sharedMaterials.Length, infoStyle);
        }
        else
        {
            GUILayout.Label("Vertex count: - / 65536", infoStyle);
            GUILayout.Label("Material count: -", infoStyle);

        }
        EditorGUILayout.EndVertical();



        if (GUI.changed)
        {
            EditorUtility.SetDirty(simpleTarget);
        }
    }

    void DestroyComponentsExeptColliders(Transform t)
    {
        var transforms = t.GetComponentsInChildren(typeof(Transform));
        foreach (Transform trans in transforms)
        {
            if (!((SimpleMeshCombine)target).keepStructure && trans.transform.parent != t && trans.transform != t && trans.GetComponent<Collider>())
            {
                trans.transform.name = "" + GetParentStructure(t, trans.transform);
                trans.transform.parent = t;
            }
        }
        var components = t.GetComponentsInChildren(typeof(Component));
        foreach (Component comp in components)
        {
            if (!(comp is Collider) && !(comp is GameObject) && !(comp is Transform))
            {
                DestroyImmediate(comp);
            }
        }
    }

    string GetParentStructure(Transform root, Transform t)
    {
        Transform ct = t;
        string s = "";
        while (ct != root)
        {
            s = s.Insert(0, ct.name + " - ");
            ct = ct.parent;

        }
        s = s.Remove(s.Length - 3, 3);
        return s;
    }

    void DestroyEmptyGameObjects(Transform t)
    {
        var components = t.GetComponentsInChildren<Transform>();
        foreach (Transform comp in components)
        {
            if (comp && (comp.childCount == 0 || !CheckChildrenForColliders(comp)))
            {
                Collider col = comp.GetComponent<Collider>();
                if (!col)
                {
                    DestroyImmediate(comp.gameObject);
                }
            }
        }
    }
    bool CheckChildrenForColliders(Transform t)
    {
        var components = t.GetComponentsInChildren(typeof(Collider));
        if (components.Length > 0)
        {
            return true;
        }
        return false;
    }

    void CopyMaterials(Transform t)
    {
        var r = t.GetComponent<Renderer>();
        r.sharedMaterials = ((SimpleMeshCombine)target).combined.transform.GetComponent<Renderer>().sharedMaterials;
    }

    void CopyColliders()
    {
        SimpleMeshCombine simpleTarget = (SimpleMeshCombine)target;
        GameObject clone = (GameObject)Instantiate(simpleTarget.gameObject, simpleTarget.copyTarget.transform.position, simpleTarget.copyTarget.transform.rotation);
        if (simpleTarget.destroyOldColliders)
        {
            var o = simpleTarget.copyTarget.transform.FindChild("Colliders [SMC]");
            if (o)
            {
                DestroyImmediate(o.gameObject);
            }
        }
        clone.transform.name = "Colliders [SMC]";
        clone.transform.parent = simpleTarget.copyTarget.transform;
        DestroyComponentsExeptColliders(clone.transform);
        DestroyEmptyGameObjects(clone.transform);
    }

}

using UnityEngine;
using System.Collections;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections.Generic;

[InitializeOnLoad]
[CustomEditor(typeof(UIParticle))]
public class UIParticleEditor : UIWidgetInspector
{
    //static UIParticleEditor()
    //{
    //    EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    //}

    private static void HierarchyItemCB(int instanceid, Rect selectionrect)
    {
        return;
        var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
        if (obj != null)
        {
            if (obj.GetComponent<UIParticle>())
            {
                UIParticle particle = obj.GetComponent<UIParticle>();
                Rect r = new Rect(selectionrect);
                r.x = r.width - 50;
                r.width = 50;
                var style = new GUIStyle();
                if (particle.Particles == null || particle.Particles.Count == 0)
                {
                    style.normal.textColor = Color.red;
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, string.Format("'{0}'",
                        "No Particle!"),
                        style);
                }
                else
                {
                    style.normal.textColor = Color.green;
                    style.hover.textColor = Color.cyan;
                    GUI.Label(r, string.Format("'{0}'",
                        particle.renderQueue.ToString()),
                        style);
                }
            }
#if SHOW_HIDDEN_OBJECTS && UNITY_EDITOR
            else if (obj.GetComponent<UIDrawCall>())
            {
                Rect r = new Rect(selectionrect);
                r.x = r.width - 50;
                r.width = 50;
                var style = new GUIStyle();
                style.normal.textColor = Color.green;
                style.hover.textColor = Color.cyan;
                MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
                GUI.Label(r, string.Format("=>'{0}'",
                    mesh.enabled.ToString()),
                    style);
            }
#endif
        }
    }
    int lastRQ = 0;
    protected override void DrawCustomProperties()
    {
        UIParticle particle = target as UIParticle;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Particles"))
        {
            if (EditorUtility.DisplayDialogComplex("重置控件", "重置控件会导致特效顺序重置!", "YES", "NO", "Cancel") == 0)
            {
                particle.RefreshParticles();
                serializedObject.ApplyModifiedProperties();
            }
        }
        if (GUILayout.Button("Play"))
        {
            if (Application.isPlaying)
                particle.Play(1f);
        }
        if (GUILayout.Button("Loop"))
        {
            if (Application.isPlaying)
                particle.Play(-1f);
        }
        GUILayout.EndHorizontal();

        if (particle.Particles == null || particle.Particles.Count == 0)
        {
            var style = new GUIStyle();
            style.normal.textColor = Color.red;
            style.hover.textColor = Color.yellow;
            GUILayout.Label("No Particle exits in children!", style);
        }
        else
        {
            serializedObject.Update();
            ReorderableListGUI.Title("Particles");
            ReorderableListGUI.ListField(_particlesProperty, ReorderableListFlags.HideRemoveButtons | ReorderableListFlags.HideAddButton | ReorderableListFlags.ShowIndices);
            serializedObject.FindProperty("extraRQ").intValue = _particlesProperty.arraySize > 0 ? _particlesProperty.arraySize - 1 : 0;
            serializedObject.ApplyModifiedProperties();
            //GUILayout.BeginHorizontal();
            NGUIEditorTools.DrawProperty("animator", serializedObject, "animator", GUILayout.MinWidth(20f));
            if(particle.animator!=null)
                NGUIEditorTools.DrawProperty("↘defaultClip", serializedObject, "defaultClip", GUILayout.MinWidth(20f));
            //GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            NGUIEditorTools.DrawProperty("playOnAwake", serializedObject, "playOnAwake", GUILayout.MinWidth(20f));
            if(particle.playOnAwake)
                NGUIEditorTools.DrawProperty("duration", serializedObject, "duration", GUILayout.MinWidth(20f));
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginHorizontal();
            NGUIEditorTools.DrawProperty("extraRQ", serializedObject, "extraRQ", GUILayout.MinWidth(20f));
            NGUIEditorTools.DrawProperty("RQ", serializedObject, "renderQueue", GUILayout.MinWidth(20f));
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
#if UNITY_EDITOR
            //if (lastRQ != particle.renderQueue || particle.Particles != particles)
            //{
            //    lastRQ = particle.renderQueue;
            //    particle.ResetRQ();
            //    EditorApplication.RepaintHierarchyWindow();
            //}
#endif
            base.DrawCustomProperties();
        }
        particles.Clear();
        particles.AddRange(particle.Particles);
    }
    private List<Renderer> particles = new List<Renderer>();
    private SerializedProperty _particlesProperty;

    protected override void OnEnable()
    {
        _particlesProperty = serializedObject.FindProperty("Particles");
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

}

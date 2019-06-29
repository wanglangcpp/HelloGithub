using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(ShowAtlasAndDepth))]
public class ShowAtlasAndDepthEditor : Editor
{
    static ShowAtlasAndDepthEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }

    private static void HierarchyItemCB(int instanceid, Rect selectionrect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
        if (obj != null)
        {
            if (obj.GetComponent<UIWidget>())
            {
                UIWidget widget = obj.GetComponent<UIWidget>();
                Rect r = new Rect(selectionrect);
                r.x = 0; //r.width - 70;
                //r.width = 140;
                var style = new GUIStyle();
                if (widget.material == null)
                {
                    style.normal.textColor = Color.red;
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, string.Format("{0}:{1}",
                        "Widget", widget.depth),
                        style);
                }
                else
                {
                    style.normal.textColor = Color.yellow;
                    style.hover.textColor = Color.cyan;
                    GUI.Label(r, string.Format("{0}:{1}",
                         widget.depth.ToString().PadRight(4), widget.material.name)
                        , style);
                }
            }
            if (obj.GetComponent<UIPanel>())
            {
                UIPanel[] panels = obj.transform.GetComponentsInChildren<UIPanel>(true);
                int dcAll = 0;
                foreach (var p in panels)
                {
                    dcAll += p.drawCalls.Count;
                }
                UIPanel panel = obj.GetComponent<UIPanel>();
                Rect r = new Rect(selectionrect);
                r.x = 0;
                var style = new GUIStyle();
                if (panel.renderQueue == UIPanel.RenderQueue.Automatic)
                {
                    style.normal.textColor = Color.green;
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, string.Format("{0}->{1}[{2}]",
                        panel.depth, panel.drawCalls.Count, dcAll),
                        style);
                }
                else if (panel.renderQueue == UIPanel.RenderQueue.StartAt)
                {
                    style.normal.textColor = new Color32(255, 0, 128, 255);
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, string.Format("{0}->{1}[{2}]",
                        panel.depth, panel.drawCalls.Count, dcAll),
                        style);
                }

            }
            if (obj.GetComponent<ParticleSystem>())
            {
                ParticleSystem widget = obj.GetComponent<ParticleSystem>();
                Rect r = new Rect(selectionrect);
                r.x = 0; //r.width - 70;
                //r.width = 140;
                var style = new GUIStyle();
                if (widget.GetComponent<Renderer>() != null && widget.GetComponent<Renderer>().sharedMaterial != null)
                {
                    style.normal.textColor = Color.yellow;
                    style.hover.textColor = Color.cyan;
                    GUI.Label(r, string.Format("{0}",
                         widget.GetComponent<Renderer>().sharedMaterial.renderQueue),
                        style);
                }
                else
                {
                    style.normal.textColor = Color.red;
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, "Error Particle"
                        , style);
                }
            }
            if (obj.GetComponent<MeshRenderer>())
            {
                MeshRenderer widget = obj.GetComponent<MeshRenderer>();
                Rect r = new Rect(selectionrect);
                r.x = 0; //r.width - 70;
                //r.width = 140;
                var style = new GUIStyle();
                if (widget != null && widget.sharedMaterial != null)
                {
                    style.normal.textColor = Color.yellow;
                    style.hover.textColor = Color.cyan;
                    GUI.Label(r, string.Format("{0}",
                         widget.sharedMaterial.renderQueue),
                        style);
                }
                else
                {
                    style.normal.textColor = Color.red;
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, "Error MeshRenderer"
                        , style);
                }
            }
            if (obj.GetComponent<TrailRenderer>())
            {
                TrailRenderer widget = obj.GetComponent<TrailRenderer>();
                Rect r = new Rect(selectionrect);
                r.x = 0; //r.width - 70;
                //r.width = 140;
                var style = new GUIStyle();
                if (widget.GetComponent<Renderer>() != null && widget.GetComponent<Renderer>().sharedMaterial != null)
                {
                    style.normal.textColor = Color.yellow;
                    style.hover.textColor = Color.cyan;
                    GUI.Label(r, string.Format("{0}",
                         widget.GetComponent<Renderer>().sharedMaterial.renderQueue),
                        style);
                }
                else
                {
                    style.normal.textColor = Color.red;
                    style.hover.textColor = Color.yellow;
                    GUI.Label(r, "Error Particle"
                        , style);
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Refresh"))
        {
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}

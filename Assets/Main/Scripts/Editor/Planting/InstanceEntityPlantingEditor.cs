using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Genesis.GameClient.Editor
{
    public class InstanceEntityPlantingEditor : EditorWindow
    {
        [SerializeField]
        private bool m_ContentEnabled = true;

        [SerializeField]
        private InstanceEntityPlantingEditorController m_Controller = new InstanceEntityPlantingEditorController();

        [SerializeField]
        private bool m_SelectedPlantersAreShown = false;

        [SerializeField]
        private Vector2 m_ScrollPosition;

        [SerializeField]
        private const float DefaultSpace = 18f;

        private void Init()
        {
            autoRepaintOnSceneChange = true;
        }

        private void ResetSelf()
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "You'll lose the planters' information in the current scene.", "OK", "Cancel"))
            {
                m_Controller.Reset();
            }
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Choose an instance NPCs/Buildings data table", InstanceEntityPlantingEditorController.DataTableAssetPath, "txt");
            if (!string.IsNullOrEmpty(filePath))
            {
                m_Controller.Load(filePath);
            }
        }

        private void Save()
        {
            foreach (var kv in m_Controller.GetPlanterInfos())
            {
                var planterInfo = kv.Value;
                Save(planterInfo);
            }
        }

        private void Save(InstanceEntityPlantingEditorController.PlanterInfo planterInfo)
        {
            string assetDir, fileName;
            var currentDTAssetPath = planterInfo.CurrentDTAssetPath;
            if (string.IsNullOrEmpty(currentDTAssetPath))
            {
                assetDir = InstanceEntityPlantingEditorController.DataTableAssetPath;
                fileName = planterInfo.DefaultNewTableName;
            }
            else
            {
                assetDir = Path.GetDirectoryName(currentDTAssetPath);
                fileName = Path.GetFileNameWithoutExtension(currentDTAssetPath);
            }

            string filePath = EditorUtility.SaveFilePanel(string.Format("Save Instance {0}s Data Table", planterInfo.Key), assetDir, fileName, "txt");

            if (!string.IsNullOrEmpty(filePath))
            {
                m_Controller.GetType().GetMethod("Save", BindingFlags.Instance | BindingFlags.Public)
                    .MakeGenericMethod(planterInfo.DRType, planterInfo.PlanterType)
                    .Invoke(m_Controller, new object[] { filePath, planterInfo.Key });
            }
        }

        #region EditorWindow

        private void OnFocus()
        {

        }

        private void OnLostFocus()
        {

        }

        private void OnProjectChange()
        {

        }

        private void OnSelectionChange()
        {
            Object[] filtered = Selection.GetFiltered(typeof(BaseEntityPlanter), SelectionMode.Deep);
            IList<BaseEntityPlanter> planters = filtered.ToList().ConvertAll(o => o as BaseEntityPlanter);
            m_Controller.SelectedPlanters.Clear();
            m_Controller.SelectedPlanters.AddRange(planters);
            m_Controller.SelectedPlanters.Sort((a, b) => (a.Index.CompareTo(b.Index)));
            Repaint();
        }

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;

            m_ContentEnabled = !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling;

            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            {
                GUILayout.BeginVertical();
                {
                    DrawTips();
                    EditorGUI.BeginDisabledGroup(!m_ContentEnabled);
                    {
                        DrawMainPart();
                        DrawLookAtPart();
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        #endregion EditorWindow

        #region Drawing

        private void DrawTips()
        {
            if (m_ContentEnabled)
            {
                return;
            }

            EditorGUILayout.HelpBox("You can only use this window when the application is NOT playing.", MessageType.Info);
        }

        private void DrawMainPart()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label("<b>Main Section</b>");
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("Current asset path:");

                foreach (var kv in m_Controller.GetPlanterInfos())
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Save", GUILayout.Width(80f)))
                        {
                            Save(kv.Value);
                        }

                        GUILayout.Space(10f);
                        GUILayout.Label(string.Format("Instance {0}s: {1}", kv.Value.Key, string.IsNullOrEmpty(kv.Value.CurrentDTAssetPath) ? "<None>" : kv.Value.CurrentDTAssetPath));
                    }
                    GUILayout.EndHorizontal();
                }

                var totalWidth = position.width;
                var buttonWidth = (totalWidth - 2 * DefaultSpace) / 3f;
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Reset", GUILayout.Width(buttonWidth)))
                    {
                        ResetSelf();
                    }

                    if (GUILayout.Button("Load", GUILayout.Width(buttonWidth)))
                    {
                        Load();
                    }

                    if (GUILayout.Button("Save All", GUILayout.Width(buttonWidth)))
                    {
                        Save();
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawLookAtPart()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label("<b>Look-At Section</b>");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Look-at point", GUILayout.MaxWidth(120f));
                    m_Controller.LookAtPoint = EditorGUILayout.Vector2Field(string.Empty, m_Controller.LookAtPoint);
                }
                GUILayout.EndHorizontal();

                var totalWidth = position.width;
                var buttonWidth = (totalWidth - 2 * DefaultSpace) / 3f;
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Perform Look-At", GUILayout.Width(buttonWidth)))
                    {
                        m_Controller.PerformLookAt();
                    }

                    if (GUILayout.Button("Clear Selected Planters", GUILayout.Width(buttonWidth)))
                    {
                        m_Controller.SelectedPlanters.Clear();
                    }

                    if (GUILayout.Button(m_SelectedPlantersAreShown ? "Hide Selected Planters" : "Show Selected Planters", GUILayout.Width(buttonWidth)))
                    {
                        m_SelectedPlantersAreShown = !m_SelectedPlantersAreShown;
                    }
                    GUILayout.FlexibleSpace();

                }
                GUILayout.EndHorizontal();

                if (m_SelectedPlantersAreShown)
                {
                    DrawScrollView();
                }
            }
            GUILayout.EndVertical();
        }

        private void DrawScrollView()
        {
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            {
                if (m_Controller.SelectedPlanters.Count <= 0)
                {
                    EditorGUILayout.HelpBox("No planter is selected.", MessageType.Info);
                }

                var toRemove = new List<BaseEntityPlanter>();
                foreach (var planter in m_Controller.SelectedPlanters)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.ObjectField(planter, typeof(BaseEntityPlanter), true);
                        if (GUILayout.Button("Remove", GUILayout.MaxWidth(120f)))
                        {
                            toRemove.Add(planter);
                        }

                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (toRemove.Count > 0)
                {
                    m_Controller.SelectedPlanters.RemoveAll(p => toRemove.Contains(p));
                    Repaint();
                }
            }
            EditorGUILayout.EndScrollView();
        }

        #endregion Drawing

        #region Menu Items

        [MenuItem("Game Framework/Plant Entities", false, 2000)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<InstanceEntityPlantingEditor>("Plant Entities", true).Init();
        }

        [MenuItem("Game Framework/Plant Entities", true)]
        public static bool CanShowWindow()
        {
            return !Application.isPlaying;
        }

        #endregion Menu Items
    }
}

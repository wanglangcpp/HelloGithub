using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

namespace Genesis.GameClient.Editor
{
    public class ScriptableObjectAssetCreatorEditorWindow : EditorWindow
    {
        [MenuItem("Game Framework/Create Scriptable Object Asset", false, 4002)]
        public static void Run()
        {
            var window = GetWindow<ScriptableObjectAssetCreatorEditorWindow>(true, "Create Scriptable Object Asset");
            window.minSize = new Vector2(200f, 150f);
            window.Reset();
        }

        private string m_ClassFullName = string.Empty;

        private bool m_ClassFullNameIsValid = false;
        private string m_InvalidClassFullNameErrorMessage = string.Empty;
        private Type m_Type = null;

        private bool CanGenerate
        {
            get
            {
                return m_ClassFullNameIsValid;
            }
        }

        private void Reset()
        {
            m_ClassFullName = string.Empty;
            m_ClassFullNameIsValid = false;
            m_InvalidClassFullNameErrorMessage = string.Empty;
            m_Type = null;
            Check();
        }

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;

            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginVertical("box");
                    {
                        DrawMainPart();
                    }
                    GUILayout.EndVertical();

                    EditorGUI.BeginDisabledGroup(!CanGenerate);
                    {
                        if (DrawButtonPart())
                        {
                            return;
                        }
                    }
                    EditorGUI.EndDisabledGroup();

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private void DrawMainPart()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Class full name:", GUILayout.Width(100f));
                var oldValue = m_ClassFullName;
                m_ClassFullName = GUILayout.TextField(m_ClassFullName);
                if (oldValue != m_ClassFullName)
                {
                    Check();
                }
            }
            GUILayout.EndHorizontal();

            if (!m_ClassFullNameIsValid)
            {
                EditorGUILayout.HelpBox(m_InvalidClassFullNameErrorMessage, MessageType.Warning);
            }
        }

        private bool DrawButtonPart()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Create", GUILayout.MinWidth(150f)))
                {
                    Generate();
                    Repaint();
                    return true;
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            return false;
        }

        private void Generate()
        {
            Type scriptableObjectCreator = typeof(ScriptableObjectCreator);
            var mi = scriptableObjectCreator.GetMethod("CreateAsset", BindingFlags.Public | BindingFlags.Static);
            var generic = mi.MakeGenericMethod(m_Type);
            generic.Invoke(null, null);
        }

        private void Check()
        {
            m_ClassFullNameIsValid = false;
            m_Type = null;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Type type = null;
            if (string.IsNullOrEmpty(m_ClassFullName))
            {
                m_InvalidClassFullNameErrorMessage = "Class full name is empty.";
                return;
            }

            foreach (var assembly in assemblies)
            {
                type = assembly.GetType(m_ClassFullName);
                if (type != null)
                {
                    break;
                }
            }

            if (type == null || !type.IsClass || type.IsAbstract)
            {
                m_InvalidClassFullNameErrorMessage = "Cannot find the class.";
                return;
            }

            if (!type.IsSubclassOf(typeof(ScriptableObject)))
            {
                m_InvalidClassFullNameErrorMessage = "The class is not a subclass of ScriptableObject.";
                return;
            }

            m_ClassFullNameIsValid = true;
            m_InvalidClassFullNameErrorMessage = string.Empty;
            m_Type = type;
        }
    }
}

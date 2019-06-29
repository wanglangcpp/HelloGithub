using GameFramework.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class EventGeneratorEditorWindow : EditorWindow
    {
        private const string EventRootPath = "Main/Scripts/Event";
        private const string DefaultNameSpace = "Genesis.GameClient";
        private const string DefaultSubFolderName = "Misc";
        private const string EventIdFileName = "EventId.cs";
        private const string DefaultBaseClassName = "GameEventArgs";
        private const float TagWidth = 200f;

        private string m_NameSpace = DefaultNameSpace;
        private string m_SubFolderName = DefaultSubFolderName;
        private string m_EventName = string.Empty;
        private int m_BaseClassIndex = 0;
        private HashSet<string> m_ExistingEventNames = null;
        private string[] m_CandidateBaseClassNames = null;

        private string EventClassName
        {
            get
            {
                return m_EventName + "EventArgs";
            }
        }

        private string m_InvalidEventNameMessage = string.Empty;
        private bool m_EventNameIsValid = true;
        private bool m_ContentEnabled = false;
        private bool m_LastIsCompiling = false;

        private string m_EnumComment = string.Empty;
        private string m_ClassComment = string.Empty;

        private bool CanGenerate
        {
            get
            {
                return m_EventNameIsValid;
            }
        }

        public void Reset()
        {
            titleContent = new GUIContent("Generate New Event Type");
            minSize = new Vector2(480f, 320f);
            m_NameSpace = DefaultNameSpace;
            m_SubFolderName = DefaultSubFolderName;
            m_EventName = string.Empty;
            m_ExistingEventNames = null;
            m_BaseClassIndex = 0;
            m_CandidateBaseClassNames = null;
            CheckEventNameValidation();
            m_EnumComment = string.Empty;
            m_ClassComment = string.Empty;
        }

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;

            m_ContentEnabled = !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling;

            if (m_LastIsCompiling && !EditorApplication.isCompiling)
            {
                Reset();
            }

            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            {
                GUILayout.BeginVertical();
                {
                    if (!m_ContentEnabled)
                    {
                        DrawTips();
                    }

                    EditorGUI.BeginDisabledGroup(!m_ContentEnabled);
                    {
                        GUILayout.BeginVertical("box");
                        {
                            DrawMainPart();
                        }
                        GUILayout.EndVertical();

                        EditorGUI.BeginDisabledGroup(!CanGenerate);
                        {
                            DrawButtonPart();
                        }
                        EditorGUI.EndDisabledGroup();

                        GUILayout.FlexibleSpace();
                    }
                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();

            m_LastIsCompiling = EditorApplication.isCompiling;
        }

        private void DrawTips()
        {
            EditorGUILayout.HelpBox("You can only use this window when the application is NOT playing or compiling.", MessageType.Info);
        }

        private void DrawMainPart()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Namespace:", GUILayout.Width(TagWidth));

                EditorGUI.BeginDisabledGroup(true);
                {
                    m_NameSpace = EditorGUILayout.TextField(m_NameSpace);
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Event Name:", GUILayout.Width(TagWidth));
                var oldEventName = m_EventName;
                m_EventName = EditorGUILayout.TextField(oldEventName);

                if (m_EventName != oldEventName)
                {
                    CheckEventNameValidation();
                }
            }
            GUILayout.EndHorizontal();

            if (!m_EventNameIsValid)
            {
                EditorGUILayout.HelpBox(m_InvalidEventNameMessage, MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox(string.Format("Event Class Name: {0}", EventClassName), MessageType.Info);
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Base Class:", GUILayout.Width(TagWidth));
                EnsureBaseClassNames();
                m_BaseClassIndex = EditorGUILayout.Popup(m_BaseClassIndex, m_CandidateBaseClassNames);

            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Sub-Folder Name:", GUILayout.Width(TagWidth));
                m_SubFolderName = EditorGUILayout.TextField(m_SubFolderName);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Enum Comment:", GUILayout.Width(TagWidth));
                m_EnumComment = EditorGUILayout.TextField(m_EnumComment);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Class Comment:", GUILayout.Width(TagWidth));
                m_ClassComment = EditorGUILayout.TextField(m_ClassComment);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawButtonPart()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Generate", GUILayout.MinWidth(150f)))
                {
                    Generate();
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        private void CheckEventNameValidation()
        {
            m_EventNameIsValid = false;

            if (string.IsNullOrEmpty(m_EventName))
            {
                m_InvalidEventNameMessage = "Event name is empty.";
                return;
            }

            if (!Regex.IsMatch(m_EventName, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$"))
            {
                m_InvalidEventNameMessage = "Event name is illegal.";
                return;
            }

            if (m_ExistingEventNames == null)
            {
                m_ExistingEventNames = new HashSet<string>(Enum.GetNames(typeof(EventId)));
            }

            if (m_ExistingEventNames.Contains(m_EventName))
            {
                m_InvalidEventNameMessage = "Event name has already been used.";
                return;
            }

            m_EventNameIsValid = true;
            m_InvalidEventNameMessage = string.Empty;
        }

        private void EnsureBaseClassNames()
        {
            if (m_CandidateBaseClassNames != null)
            {
                return;
            }

            var list = new List<string>();
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsSealed && type.Namespace == DefaultNameSpace && type.IsSubclassOf(typeof(GameEventArgs)))
                {
                    list.Add(type.Name);
                }
            }

            list.Sort();

            list.Insert(0, DefaultBaseClassName);
            m_CandidateBaseClassNames = list.ToArray();
        }

        private void Generate()
        {
            var eventIdAssetPath = Utility.Path.GetCombinePath("Assets", EventRootPath, EventIdFileName);
            var eventIdFilePath = Utility.Path.GetCombinePath(Application.dataPath, EventRootPath, EventIdFileName);
            var lines = File.ReadAllLines(eventIdFilePath, System.Text.Encoding.UTF8);
            int rightBraceCount = 0;
            int lineNumToInsert = -1;
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (line.Trim() == "}")
                {
                    rightBraceCount++;
                }

                if (rightBraceCount == 2)
                {
                    lineNumToInsert = lines.Length - rightBraceCount;
                    break;
                }
            }

            if (lineNumToInsert < 0)
            {
                EditorUtility.DisplayDialog(string.Empty, "Cannot find where to insert the new Event ID (name).", "OKAY");
                return;
            }

            var newLines = new List<string>(lines);
            newLines.Insert(lineNumToInsert++, string.Empty);
            newLines.Insert(lineNumToInsert++, "        /// <summary>");
            newLines.Insert(lineNumToInsert++, string.Format("        ///{0}", string.IsNullOrEmpty(m_EnumComment) ? string.Empty : (" " + m_EnumComment)));
            newLines.Insert(lineNumToInsert++, "        /// </summary>");
            newLines.Insert(lineNumToInsert++, string.Format("        {0},", m_EventName));
            File.WriteAllLines(eventIdFilePath, newLines.ToArray(), System.Text.Encoding.UTF8);

            string classBaseDir = EventRootPath;
            if (!string.IsNullOrEmpty(m_SubFolderName))
            {
                classBaseDir = Utility.Path.GetCombinePath(classBaseDir, m_SubFolderName);
            }

            var classBaseDirFullPath = Utility.Path.GetCombinePath(Application.dataPath, classBaseDir);
            var classBaseDirAssetPath = Utility.Path.GetCombinePath("Assets", classBaseDir);

            if (!Directory.Exists(classBaseDirFullPath))
            {
                Directory.CreateDirectory(classBaseDirFullPath);
                AssetDatabase.ImportAsset(classBaseDirAssetPath);
            }

            var eventClassFileName = EventClassName + ".cs";
            var classFilePath = Utility.Path.GetCombinePath(classBaseDirFullPath, eventClassFileName);
            var classAssetPath = Utility.Path.GetCombinePath(classBaseDirAssetPath, eventClassFileName);

            File.WriteAllText(classFilePath, string.Format(
@"using GameFramework.Event;

namespace Genesis.GameClient
{{
    /// <summary>
    ///{0}
    /// </summary>
    public class {1} : {3}
    {{
        public {1}()
        {{

        }}

        public override int Id
        {{
            get
            {{
                return (int)EventId.{2};
            }}
        }}
    }}
}}
",
                string.IsNullOrEmpty(m_ClassComment) ? string.Empty : (" " + m_ClassComment), EventClassName, m_EventName, m_CandidateBaseClassNames[m_BaseClassIndex]), System.Text.Encoding.UTF8);

            AssetDatabase.ImportAsset(eventIdAssetPath);
            AssetDatabase.ImportAsset(classAssetPath);
        }
    }
}

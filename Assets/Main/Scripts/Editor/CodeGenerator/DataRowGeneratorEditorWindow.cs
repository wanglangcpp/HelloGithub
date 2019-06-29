using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class DataRowGeneratorEditorWindow : EditorWindow
    {
        private const string DataRowRootPath = "Main/Scripts/DataTable";
        private const string DefaultNameSpace = "Genesis.GameClient";
        private const float TagWidth = 200f;
        private const string ClassNamePrefix = "DR";
        private const int RequestedLineCount = 4;
        private const char ChineseFullStop = '\u3002';
        private const string ClassTemplateFilePath = "Main/Editor/DataRowClassFileTemplate.txt";
        private const string ProcedureClassPath = "Main/Scripts/Procedure/ProcedurePreload.cs";

        private TextAsset m_CachedTextAsset;
        private bool m_ContentEnabled = false;
        private bool m_LastIsCompiling = false;
        private bool m_TextAssetIsValid = false;
        private string m_InvalidTextAssetMessage = string.Empty;

        private bool CanGenerate
        {
            get
            {
                return m_TextAssetIsValid;
            }
        }

        private string ClassName
        {
            get
            {
                if (m_CachedTextAsset == null)
                {
                    return null;
                }

                return ClassNamePrefix + m_CachedTextAsset.name;
            }
        }

        public void Reset()
        {
            titleContent = new GUIContent("Generate New Data Row Type");
            minSize = new Vector2(480f, 320f);
            m_CachedTextAsset = null;
            m_TextAssetIsValid = false;
            m_InvalidTextAssetMessage = string.Empty;
            CheckTextAssetValidation();
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
                GUILayout.Label("Data table: ", GUILayout.Width(TagWidth));
                string oldAssetPath = m_CachedTextAsset == null ? string.Empty : AssetDatabase.GetAssetPath(m_CachedTextAsset);
                m_CachedTextAsset = EditorGUILayout.ObjectField(m_CachedTextAsset, typeof(TextAsset), false) as TextAsset;
                string newAssetPath = m_CachedTextAsset == null ? string.Empty : AssetDatabase.GetAssetPath(m_CachedTextAsset);
                if (oldAssetPath != newAssetPath)
                {
                    CheckTextAssetValidation();
                }
            }
            GUILayout.EndHorizontal();

            if (!m_TextAssetIsValid)
            {
                EditorGUILayout.HelpBox(m_InvalidTextAssetMessage, MessageType.Warning);
            }
        }

        private void CheckTextAssetValidation()
        {
            m_TextAssetIsValid = false;

            if (m_CachedTextAsset == null)
            {
                m_InvalidTextAssetMessage = "No data table is selected.";
                return;
            }

            if (!Regex.IsMatch(m_CachedTextAsset.name, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$"))
            {
                m_InvalidTextAssetMessage = "Data table name is not valid.";
                return;
            }

            if (Assembly.Load("Assembly-CSharp").GetType(string.Format("{0}.{1}", DefaultNameSpace, ClassName)) != null)
            {
                m_InvalidTextAssetMessage = string.Format("Class '{0}' already exists in 'Assembly-CSharp.dll'.", ClassName);
                return;
            }

            m_TextAssetIsValid = true;
            m_InvalidTextAssetMessage = string.Empty;
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

        private void Generate()
        {
            var dataTableAssetPath = AssetDatabase.GetAssetPath(m_CachedTextAsset);
            var dataTableFilePath = Utility.Path.GetCombinePath(Application.dataPath, Regex.Replace(dataTableAssetPath, @"^Assets/", string.Empty));
            var dataTableLines = File.ReadAllLines(dataTableFilePath, Encoding.Default);

            if (dataTableLines.Length < RequestedLineCount)
            {
                EditorUtility.DisplayDialog(string.Empty, "Data table has less than 3 lines and so cannot be parsed.", "OKAY");
                return;
            }

            for (int i = 0; i < RequestedLineCount; ++i)
            {
                var line = dataTableLines[i];
                if (!line.StartsWith("#"))
                {
                    EditorUtility.DisplayDialog(string.Empty, string.Format("Line {0} of data table should start with a '#'.", i + 1), "OKAY");
                    return;
                }
            }

            string[] segments;
            segments = dataTableLines[0].Split('\t');

            string classComment = string.Empty;
            if (segments.Length >= 2)
            {
                classComment = " " + segments[1] + ChineseFullStop;
            }

            var nameLineSegments = dataTableLines[1].Split('\t');
            var typeLineSegments = dataTableLines[2].Split('\t');
            var commentLineSegments = dataTableLines[3].Split('\t');

            if (nameLineSegments.Length != typeLineSegments.Length)
            {
                EditorUtility.DisplayDialog(string.Empty, "The line of property names and types have different lengths.", "OKAY");
                return;
            }

            GenerateDataRowClass(classComment, nameLineSegments, typeLineSegments, commentLineSegments);
            ModifyProcedurePreload();
        }

        private void GenerateDataRowClass(string classComment, string[] nameLineSegments, string[] typeLineSegments, string[] commentLineSegments)
        {
            var propertiesSb = new StringBuilder();
            var parseDataRowMethodBodySb = new StringBuilder();

            for (int i = 0; i < nameLineSegments.Length; ++i)
            {
                if (i == 0 || i == 2)
                {
                    parseDataRowMethodBodySb.AppendLine("            index++;");
                    continue;
                }

                bool isLastLine = (i == nameLineSegments.Length - 1);

                var pi = new PropertyInfo
                {
                    Name = nameLineSegments[i],
                    Type = typeLineSegments[i],
                    Comment = commentLineSegments.Length > i ? string.Format(" {0}{1}", commentLineSegments[i], ChineseFullStop) : string.Empty,
                };

                propertiesSb.AppendLine("        /// <summary>");
                propertiesSb.AppendLine(string.Format("        ///{0}", pi.Comment));
                propertiesSb.AppendLine("        /// </summary>");
                propertiesSb.AppendLine(string.Format("        public {0} {1} {{ get; private set; }}", pi.Type, pi.Name));

                var lineContent = "            " + GetParserLine(pi.Type, pi.Name);
                if (!isLastLine)
                {
                    parseDataRowMethodBodySb.AppendLine(lineContent);
                    propertiesSb.AppendLine();
                }
                else
                {
                    parseDataRowMethodBodySb.Append(lineContent);
                }
            }

            var templateAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(Utility.Path.GetCombinePath("Assets", ClassTemplateFilePath));
            var codeText = templateAsset.text;
            codeText = Regex.Replace(codeText, @"\$Namespace", DefaultNameSpace);
            codeText = Regex.Replace(codeText, @"\$ClassComment", classComment);
            codeText = Regex.Replace(codeText, @"\$ClassName", ClassName);
            codeText = Regex.Replace(codeText, @"\$ParseDataRowMethodBody", parseDataRowMethodBodySb.ToString());
            codeText = Regex.Replace(codeText, @"\$Properties", propertiesSb.ToString());
            codeText = Regex.Replace(codeText, @"(?<!\r)\n", "\r\n");

            var classFileName = ClassName + ".cs";
            var classAssetPath = Utility.Path.GetCombinePath("Assets/", DataRowRootPath, classFileName);
            File.WriteAllText(Utility.Path.GetCombinePath(Application.dataPath, DataRowRootPath, classFileName), codeText, Encoding.UTF8);
            AssetDatabase.ImportAsset(classAssetPath);
        }

        private string GetParserLine(string type, string name)
        {
            switch (type)
            {
                case "bool":
                    return string.Format("{0} = bool.Parse(text[index++]);", name);
                case "int":
                    return string.Format("{0} = int.Parse(text[index++]);", name);
                case "float":
                    return string.Format("{0} = float.Parse(text[index++]);", name);
                case "DateTime":
                    return string.Format("{0} = DateTime.Parse(text[index++]);", name);
                case "Vector2":
                    return string.Format("{0} = ConverterEx.ParseVector2(text[index++]).Value;", name);
                case "Vector3":
                    return string.Format("{0} = ConverterEx.ParseVector3(text[index++]).Value;", name);
                case "string":
                default:
                    return string.Format("{0} = text[index++];", name);
            }
        }

        private void ModifyProcedurePreload()
        {
            var filePath = Utility.Path.GetCombinePath(Application.dataPath, ProcedureClassPath);
            var assetPath = Utility.Path.GetCombinePath("Assets/", ProcedureClassPath);

            var lines = new List<string>(File.ReadAllLines(filePath, Encoding.UTF8));

            int begLineNum = -1;
            int endLineNum = -1;
            for (int i = 0; i < lines.Count; ++i)
            {
                if (begLineNum < 0 && Regex.IsMatch(lines[i], @"\s*LoadDataTable\(.*\);"))
                {
                    begLineNum = i;
                    continue;
                }

                if (begLineNum >= 0 && endLineNum < 0 && !Regex.IsMatch(lines[i], @"\s*LoadDataTable\(.*\)"))
                {
                    endLineNum = i;
                    break;
                }
            }

            if (endLineNum < 0)
            {
                endLineNum = lines.Count;
            }

            lines.Insert(endLineNum++, string.Format("            LoadDataTable(\"{0}\");", m_CachedTextAsset.name));
            lines.Sort(begLineNum, endLineNum - begLineNum, Comparer.Instance);
            File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
            AssetDatabase.ImportAsset(assetPath);
        }

        private class PropertyInfo
        {
            public string Name = "";
            public string Type = "";
            public string Comment = "";
        }

        private class Comparer : IComparer<string>
        {
            private static Comparer s_Instance;

            public static Comparer Instance
            {
                get
                {
                    if (s_Instance == null)
                    {
                        s_Instance = new Comparer();
                    }

                    return s_Instance;
                }
            }

            public int Compare(string x, string y)
            {
                return x.CompareTo(y);
            }
        }
    }
}

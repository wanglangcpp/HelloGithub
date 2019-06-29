using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// 时间轴群组编辑器
    /// </summary>
    public class TimeLineGroupEditor : EditorWindow
    {
        private const string AssetExtension = "txt";
        private static readonly Vector2 MinSize = new Vector2(800f, 480f);
        private const float DefaultHorizontalIndent = 16f;
        private int FindId;
        private string FindValue;
        private string LocalizationPath;
        private int SelectLocalizationVesion =0;
        /// <summary>
        /// 菜单项入口
        /// </summary>
        [MenuItem("Assets/Edit Time Lines")]
        public static void Run()
        {
            var selectedObjs = Selection.objects;

            if (selectedObjs.Length == 1 && selectedObjs[0] is TextAsset && Path.GetExtension(AssetDatabase.GetAssetPath(selectedObjs[0])) == "." + AssetExtension)
            {
                ShowWindow(selectedObjs[0] as TextAsset);
                return;
            }

            var filePath = EditorUtility.OpenFilePanel("Choose the time line file", string.Empty, AssetExtension);
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            filePath = filePath.Replace(Application.dataPath, "");
            filePath = "Assets" + filePath;
            ShowWindow(AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset)) as TextAsset);
        }

        private static void ShowWindow(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                Debug.LogWarning("Oops, the text asset doesn't exist!");
                return;
            }

            var window = EditorWindow.GetWindow<TimeLineGroupEditor>(true, "Time Line Group Editor");
            window.Init(textAsset);
        }

        private TimeLineGroupEditorController m_Controller = null;

        private string[] m_ActionTypeNamesToSelect;
        private Dictionary<string, int> m_IntFieldCacheValues = new Dictionary<string, int>();
        private GUIStyle m_DuplicateTimeLineIdWarningGuiStyle = new GUIStyle();

        [SerializeField]
        private TextAsset m_TextAsset = null;

        private void Init(TextAsset textAsset)
        {
            m_TextAsset = textAsset;
            Reinit();
            minSize = MinSize;

        }

        private void Reinit()
        {
            m_Controller = new TimeLineGroupEditorController();
            string errorMsg = m_Controller.Init(m_TextAsset);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Close();
                Debug.LogError("[TimeLineGroupEditor Init] Parsing data error: " + errorMsg);
                return;
            }

            //加载本地化
            LoadTimeLineLocalization();

            LoadActionTypes();
            m_IntFieldCacheValues.Clear();
            m_DuplicateTimeLineIdWarningGuiStyle.normal.textColor = Color.yellow;
        }

        private void LoadActionTypes()
        {
            var tmp = new List<string>();
            //tmp.Add("Select...");
            //tmp.AddRange(m_Controller.ActionDataTypes.ToList().ConvertAll(t => Utility.Text.FieldNameForDisplay(Regex.Replace(t.Name, @"Data$", string.Empty))));
            tmp.Add(GetLocalizationString("TIMELINE_19"));
            tmp.AddRange(m_Controller.ActionDataTypes.ToList().ConvertAll(t => GetLocalizationString(t.Name)));
            m_ActionTypeNamesToSelect = tmp.ToArray();
        }

        private void LoadTimeLineLocalization()
        {
            LocalizationPath = m_Controller.AssetPath.Replace(m_TextAsset.name, "LocalizationTimeLine");

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(LocalizationPath, typeof(TextAsset)) as TextAsset;
            ParseData(textAsset.text);
        }

        Dictionary<string, string> m_TimeLineChineseTxt = new Dictionary<string, string>();
        Dictionary<string, string> m_TimeLineEnglishTxt = new Dictionary<string, string>();
        public string ParseData(string rawText)
        {
            int timeLineId = 0;
            int timeLineActionCount = 0;

            string[] rowTexts = Utility.Text.SplitToLines(rawText);
  
            try
            {
                //每一行数据
                for (int row = 0; row < rowTexts.Length; row++)
                {
                    string rowText = rowTexts[row];
                    if (rowText.Length <= 0 || rowText[0] == '#')
                    {
                        continue;
                    }
                    //被制表符分开的字段
                    string[] splitLine = rowText.Split('\t');
                    if (splitLine.Length < 2 && splitLine.Length < 4)
                        continue;
                    if (splitLine[1] == null || splitLine[1] == "")
                        continue;
                    if (splitLine[3] == null || splitLine[3] == "")
                    {
                        splitLine[3] = "unknow";
                    }
                    m_TimeLineChineseTxt.Add(splitLine[1], splitLine[3]);

                    if (splitLine.Length < 6)
                        continue;
                    if (splitLine[5] == null || splitLine[5] == "")
                    {
                        splitLine[5] = "unknow";
                    }
                    m_TimeLineEnglishTxt.Add(splitLine[1], splitLine[5]);
                }

                return string.Empty;
            }
            catch (Exception exception)
            {
                return string.Format("Can not load time line localization at time line id '{0}', action count '{1}' with exception '{2}'.",  timeLineId.ToString(), timeLineActionCount.ToString(), exception.Message);
            }
        }

        public string GetLocalizationString(string key)
        {
            if (SelectLocalizationVesion == 0)
            {
                if (m_TimeLineChineseTxt == null)
                    return "";
                if (m_TimeLineChineseTxt.ContainsKey(key))
                {
                    return m_TimeLineChineseTxt[key];
                }
            }
            else if(SelectLocalizationVesion == 1)
            {
                if (m_TimeLineEnglishTxt == null)
                    return "";
                if (m_TimeLineEnglishTxt.ContainsKey(key))
                {
                    return m_TimeLineEnglishTxt[key];
                }
            }
      
            return "";
        }

        private void OnGUI()
        {
            if (m_Controller == null)
            {
                Reinit();
            }

            GUI.enabled = !EditorApplication.isCompiling && !EditorApplication.isPlayingOrWillChangePlaymode;

            GUILayout.BeginArea(new Rect(0f, 0f, position.width, position.height));
            {
                GUILayout.BeginVertical();
                {
                    DrawHeaderView();
                    DrawMainView();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private void DrawHeaderView()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(GetLocalizationString("TIMELINE_16")))
                {
                    m_Controller.SaveData();
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_17")))
                {
                    m_Controller.SortTimeLinesById();
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_18")))
                {
                    m_Controller.AddTimeLine();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {                
                GUILayout.Label(GetLocalizationString("TIMELINE_1"));                
                FindId = EditorGUILayout.IntField(FindId, GUILayout.Width(100f));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(GetLocalizationString("TIMELINE_2")))
                {
                    //开始查找
                    int index = m_Controller.FindTimeLineById(FindId);
                    if (index != -1)
                    {
                        FindValue = GetLocalizationString("TIMELINE_3");
                        m_Controller.SetAllUnfolded(false);
                        m_ScrollPos = new Vector2(0, index*20);
                    }
                    else
                    {
                        FindValue = GetLocalizationString("TIMELINE_4");
                        
                    }
                   
                }
                GUILayout.Label(FindValue);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            {
                GUILayout.Label(string.Format(GetLocalizationString("TIMELINE_8"), m_Controller.AssetPath));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            {
                GUILayout.Label(string.Format(GetLocalizationString("TIMELINE_9"), LocalizationPath));
                string[] vesion = { "中文", "English" };
                var curSelectLocalizationVesion = EditorGUILayout.Popup(SelectLocalizationVesion, vesion);
                if (curSelectLocalizationVesion != SelectLocalizationVesion)
                {
                    SelectLocalizationVesion = curSelectLocalizationVesion;
                    LoadActionTypes();                  
                }
            }
            GUILayout.EndHorizontal();
        }

        private Vector2 m_ScrollPos = Vector2.zero;

        private void DrawMainView()
        {
            bool guiEnabled = GUI.enabled;

            GUI.enabled = true;
            m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
            {
                GUI.enabled = guiEnabled;

                if (m_Controller != null && m_Controller.GroupData != null && m_Controller.GroupData.TimeLines != null)
                {
                    for (int i = 0; i < m_Controller.GroupData.TimeLines.Count; ++i)
                    {
                        DrawTimeLineData(i);
                    }
                }

                GUI.enabled = true;
            }
            GUILayout.EndScrollView();
            GUI.enabled = guiEnabled;
        }

        private void DrawTimeLineData(int index)
        {
            var timeLineData = m_Controller.GroupData.TimeLines[index];

            bool unfolded = false;

            GUILayout.BeginHorizontal();
            {
                var oldUnfolded = m_Controller.IsUnfolded(timeLineData);
                unfolded = EditorGUILayout.Foldout(oldUnfolded, "");
                if (unfolded != oldUnfolded)
                {
                    m_Controller.SetUnfolded(timeLineData, unfolded);
                }

                GUILayout.Space(-40f);
                GUILayout.Label("Time Line " + index);
                GUILayout.FlexibleSpace();
                GUILayout.Label("ID: ");
                int newId = EditorGUILayout.IntField(timeLineData.Id, GUILayout.Width(100f));

                if (newId < 0)
                {
                    Debug.LogWarning("You have input an invalid time line ID.");
                    newId = timeLineData.Id;
                }

                if (newId != timeLineData.Id)
                {
                    m_Controller.ReduceTimeLineIdUsedCount(timeLineData.Id);
                    timeLineData.GetType().GetField("m_Id", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(timeLineData, newId);
                    m_Controller.AddTimeLineIdUsedCount(timeLineData.Id);
                }

                if (m_Controller.GetTimeLineIdUsedCount(timeLineData.Id) > 1)
                {
                    GUILayout.Label("Duplicate Id!", m_DuplicateTimeLineIdWarningGuiStyle);
                }

                GUILayout.Space(30f);
                GUILayout.Label(GetLocalizationString("TIMELINE_10"));
                int actionTypeIndex = EditorGUILayout.Popup(0, m_ActionTypeNamesToSelect, GUILayout.Width(110f));

                if (actionTypeIndex > 0)
                {
                    Type type = m_Controller.ActionDataTypes[actionTypeIndex - 1];
                    m_Controller.AddActionData(timeLineData, type);
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(GetLocalizationString("TIMELINE_11")))
                {
                    m_Controller.SortTimeLineActionsByName(timeLineData);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_12"), GUILayout.Width(45f)))
                {
                    m_Controller.MoveUpTimeLine(index);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_13"), GUILayout.Width(45f)))
                {
                    m_Controller.MoveDownTimeLine(index);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_14"), GUILayout.Width(80f)))
                {
                    m_Controller.DuplicateTimeLine(timeLineData);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_15"), GUILayout.Width(70f)))
                {
                    if (EditorUtility.DisplayDialog("Caution", "Are you sure you want to remove this time line?", "Ok", "Cancel"))
                    {
                        m_Controller.RemoveTimeLine(timeLineData);
                        Repaint();
                    }
                }
            }
            GUILayout.EndHorizontal();

            if (!unfolded)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(DefaultHorizontalIndent);

                if (timeLineData.Actions.Count <= 0)
                {
                    EditorGUILayout.HelpBox("This time line contains no action.", MessageType.Error);
                }
                else
                {
                    GUILayout.BeginVertical();
                    {
                        for (int i = 0; i < timeLineData.Actions.Count; ++i)
                        {
                            DrawTimeLineActionData(timeLineData, i);
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawTimeLineActionData(TimeLineData timeLineData, int index)
        {
            var timeLineActionData = timeLineData.Actions[index];
            var type = timeLineActionData.GetType();

            bool unfolded = false;
            GUILayout.BeginHorizontal();
            {
                var oldUnfolded = m_Controller.IsUnfolded(timeLineActionData);
                unfolded = EditorGUILayout.Foldout(oldUnfolded, "");
                if (unfolded != oldUnfolded)
                {
                    m_Controller.SetUnfolded(timeLineActionData, unfolded);
                }

                GUILayout.Space(-40f);

                //var actionTypeName = Regex.Replace(type.Name, @"Data$", string.Empty);
                //GUILayout.Label(Utility.Text.FieldNameForDisplay(actionTypeName));
                GUILayout.Label(GetLocalizationString(type.Name));
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(GetLocalizationString("TIMELINE_12"), GUILayout.Width(45f)))
                {
                    m_Controller.MoveUpTimeLineAction(timeLineData, index);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_13"), GUILayout.Width(45f)))
                {
                    m_Controller.MoveDownTimeLineAction(timeLineData, index);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_14"), GUILayout.Width(80f)))
                {
                    m_Controller.DuplicateTimeLineAction(timeLineData, timeLineActionData);
                }

                if (GUILayout.Button(GetLocalizationString("TIMELINE_15"), GUILayout.Width(100f)))
                {
                    if (EditorUtility.DisplayDialog("Caution", "Are you sure you want to remove this action?", "Ok", "Cancel"))
                    {
                        m_Controller.RemoveActionData(timeLineData, timeLineActionData);
                        Repaint();
                    }
                }
            }
            GUILayout.EndHorizontal();

            if (!unfolded)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(DefaultHorizontalIndent);
                var fields = timeLineActionData.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                GUILayout.BeginVertical("box");
                {
                    foreach (var field in fields)
                    {
                        DrawField(field, timeLineActionData);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawField(FieldInfo field, TimeLineActionData timeLineActionData)
        {
            bool needDrawArray = false;
            bool needDrawHelpBox = false;
            GUILayout.BeginHorizontal();
            {
                object originalValue = field.GetValue(timeLineActionData);
                if (field.FieldType.IsValueType)
                {
                    DrawFieldName(field);
                    object val = null;
                    bool enableFieldModify = true;
                    Type underlyingType = Nullable.GetUnderlyingType(field.FieldType);
                    if (underlyingType != null)
                    {
                        bool isNull = (field.GetValue(timeLineActionData) == null);
                        //GUILayout.Label("Is Null: ", GUILayout.Width(50f));
                        GUILayout.Label(GetLocalizationString("TIMELINE_7"), GUILayout.Width(50f));
                        bool newIsNull = EditorGUILayout.Toggle(isNull, GUILayout.Width(30f));
                        enableFieldModify = !newIsNull;
                        if (newIsNull != isNull)
                        {
                            if (newIsNull)
                            {
                                field.SetValue(timeLineActionData, null);
                            }
                            else
                            {
                                field.SetValue(timeLineActionData, Activator.CreateInstance(underlyingType));
                            }
                        }

                        if (!newIsNull)
                        {
                            val = Convert.ChangeType(field.GetValue(timeLineActionData), underlyingType);
                        }
                        else
                        {
                            val = Activator.CreateInstance(underlyingType);
                        }
                    }
                    else
                    {
                        val = Convert.ChangeType(originalValue, field.FieldType);
                    }

                    GUILayout.Space(DefaultHorizontalIndent);

                    DrawSimpleField(enableFieldModify, field, val, val.GetType(), timeLineActionData);
                }
                else if (field.FieldType == typeof(string))
                {
                    DrawFieldName(field);
                    DrawSimpleField(true, field, field.GetValue(timeLineActionData) ?? "", typeof(string), timeLineActionData);
                }
                else if (field.FieldType.IsArray)
                {
                    object array = field.GetValue(timeLineActionData);
                    var oldUnfolded = m_Controller.IsUnfolded(array);
                    var newUnfolded = EditorGUILayout.Foldout(oldUnfolded, "");
                    GUILayout.Space(-40f);
                    DrawFieldName(field);
                    if (newUnfolded != oldUnfolded)
                    {
                        m_Controller.SetUnfolded(array, newUnfolded);
                    }

                    if (newUnfolded)
                    {
                        needDrawArray = true;
                    }
                }
                else
                {
                    needDrawHelpBox = true;
                }

                GUILayout.FlexibleSpace();

            }
            GUILayout.EndHorizontal();

            if (needDrawHelpBox)
            {
                EditorGUILayout.HelpBox(string.Format("Cannot draw field {0} of type {1} for {2}", field.Name, field.FieldType, timeLineActionData.GetType()), MessageType.Warning);
            }

            if (needDrawArray)
            {
                DrawArrayField(field, timeLineActionData);
            }
        }

        private void DrawFieldName(FieldInfo field)
        {
            //GUILayout.Label(Utility.Text.FieldNameForDisplay(field.Name), GUILayout.Width(180f));
            GUILayout.Label(GetLocalizationString(field.Name), GUILayout.Width(180f));
        }

        private void DrawArrayField(FieldInfo field, TimeLineActionData timeLineActionData)
        {
            Type elementType = field.FieldType.GetElementType();
            Array array = field.GetValue(timeLineActionData) as Array;

            if (array == null)
            {
                array = Array.CreateInstance(elementType, 0);
                field.SetValue(timeLineActionData, array);
            }

            bool dontDrawArrayElements = false;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(DefaultHorizontalIndent);

                EditorGUILayout.BeginVertical();
                {
                    dontDrawArrayElements = DrawArrayLength(ref array, timeLineActionData, field, elementType, dontDrawArrayElements);

                    if (!dontDrawArrayElements)
                    {
                        for (int i = 0; i < array.Length; ++i)
                        {
                            DrawArrayElement(array, i, elementType);
                        }
                    }
                    else
                    {
                        Repaint();
                    }

                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private bool DrawArrayLength(ref Array array, TimeLineActionData timeLineActionData, FieldInfo field, Type elementType, bool dontDrawArrayElements)
        {
            GUILayout.BeginHorizontal();
            {
                var length = array.Length;
                var lengthInputControlName = timeLineActionData.GetHashCode() + field.Name;
                //GUILayout.Label("Size: ", GUILayout.Width(150f));TIMELINE_5
                GUILayout.Label(GetLocalizationString("TIMELINE_5"), GUILayout.Width(150f));
                GUI.SetNextControlName(lengthInputControlName);
                var newLength = EditorGUILayout.IntField(m_IntFieldCacheValues.ContainsKey(lengthInputControlName) ? m_IntFieldCacheValues[lengthInputControlName] : length, GUILayout.Width(150f));
                if (newLength < 0)
                {
                    newLength = 0;
                }

                if (GUI.GetNameOfFocusedControl() == lengthInputControlName)
                {
                    if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
                    {
                        newLength = m_IntFieldCacheValues.ContainsKey(lengthInputControlName) ? m_IntFieldCacheValues[lengthInputControlName] : length;
                        m_IntFieldCacheValues.Remove(lengthInputControlName);
                        if (newLength != length)
                        {
                            array = m_Controller.ChangeArrayLength(array, timeLineActionData, field, elementType, length, newLength);
                            dontDrawArrayElements = true;
                        }
                    }
                    else
                    {
                        m_IntFieldCacheValues[lengthInputControlName] = newLength;
                    }
                }
                else
                {
                    m_IntFieldCacheValues.Remove(lengthInputControlName);
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            return dontDrawArrayElements;
        }

        private void DrawArrayElement(Array array, int index, Type elementType)
        {
            GUILayout.BeginHorizontal();
            {
                //GUILayout.Label(string.Format("Element at {0}: ", index), GUILayout.Width(150f));
                GUILayout.Label(string.Format(GetLocalizationString("TIMELINE_6"), index), GUILayout.Width(150f));
                object oldVal = array.GetValue(index);
                object newVal = DrawRawDataType(oldVal, elementType);
                if (newVal != null)
                {
                    array.SetValue(newVal, index);
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawSimpleField(bool enableFieldModify, FieldInfo field, object val, Type underlyingType, TimeLineActionData timeLineActionData)
        {
            EditorGUI.BeginDisabledGroup(!enableFieldModify);
            {
                object newVal = DrawRawDataType(val, underlyingType);
                if (newVal != null)
                {
                    field.SetValue(timeLineActionData, newVal);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private object DrawRawDataType(object val, Type type)
        {
            if (type == typeof(bool))
            {
                bool oldVal = (bool)val;
                bool newVal = EditorGUILayout.Toggle(oldVal, GUILayout.Width(30f));
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(int))
            {
                int oldVal = (int)val;
                int newVal = EditorGUILayout.IntField(oldVal, GUILayout.Width(150f));
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(float))
            {
                float oldVal = (float)val;
                float newVal = EditorGUILayout.FloatField(oldVal, GUILayout.Width(150f));
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(Vector2))
            {
                Vector2 oldVal = (Vector2)val;
                Vector2 newVal = EditorGUILayout.Vector2Field("", oldVal);
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(Vector3))
            {
                Vector3 oldVal = (Vector3)val;
                Vector3 newVal = EditorGUILayout.Vector3Field("", oldVal);
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(string))
            {
                string oldVal = (string)val;
                string newVal = EditorGUILayout.TextField(oldVal, GUILayout.Width(150f));
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type.IsEnum)
            {
                Enum oldVal = (Enum)val;
                Enum newVal = EditorGUILayout.EnumPopup(oldVal, GUILayout.Width(150f));
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(Color))
            {
                Color oldVal = (Color)val;
                Color newVal = EditorGUILayout.ColorField(oldVal);
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            if (type == typeof(Rect))
            {
                Rect oldVal = (Rect)val;
                Rect newVal = EditorGUILayout.RectField(oldVal);
                if (newVal != oldVal)
                {
                    return newVal;
                }
                return null;
            }

            EditorGUILayout.HelpBox("Unrecognized type " + type, MessageType.Warning);
            return null;
        }
    }
}

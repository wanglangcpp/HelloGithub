using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class ClientConfigEditorWindow : EditorWindow
    {
        public static void OpenWindow(ClientConfig config)
        {
            if (null == config)
                return;

            var window = GetWindow<ClientConfigEditorWindow>(true, "Client Config");
            window.m_Config = config;
        }

        private ClientConfig m_Config;

        private void OnGUI()
        {
            if (null == m_Config)
                return;

            OnDeviceModelGUI();
        }

        #region Device Model UI

        private static bool s_DeviceModelFoldOut = true;

        private Vector2 m_DeviceModelTablePosition = Vector2.zero;

        private void OnDeviceModelGUI()
        {
            if (!(s_DeviceModelFoldOut = EditorGUILayout.Foldout(s_DeviceModelFoldOut, "Device Model Table")))
                return;

            var list = m_Config.DeviceModels;

            m_DeviceModelTablePosition = EditorGUILayout.BeginScrollView(m_DeviceModelTablePosition, GUILayout.Width(this.position.width));
            DeviceModelTableHeader();

            int deleteIndex = -1;

            for (int i = 0; i < list.Count; i++)
            {
                if (DeviceModelTableRow(list[i]))
                {
                    deleteIndex = i;
                }
            }

            if (deleteIndex >= 0)
            {
                list.RemoveAt(deleteIndex);
            }

            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                list.Add(new ClientConfig.DeviceModel());
            }

            EditorGUILayout.EndScrollView();
        }

        private const float CommentCellWidth = 300;
        private const float ModelNameCellWidth = 300;
        private const float QualityLevelCellWidth = 300;
        private const float MaxNearbyPlayerCountWidth = 200;
        private const float MaxNearbyPlayerModelTypeCountWidth = 250;

        private void DeviceModelTableHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(30));
            EditorGUILayout.LabelField("Comment", GUILayout.Width(CommentCellWidth));
            EditorGUILayout.LabelField("Model Name", GUILayout.Width(ModelNameCellWidth));
            EditorGUILayout.LabelField("Quality Level", GUILayout.Width(QualityLevelCellWidth));
            EditorGUILayout.LabelField("Max Nearby Player Count", GUILayout.Width(MaxNearbyPlayerCountWidth));
            EditorGUILayout.LabelField("Max Nearby Player Model Type Count", GUILayout.Width(MaxNearbyPlayerModelTypeCountWidth));
            EditorGUILayout.EndHorizontal();
        }

        private bool DeviceModelTableRow(ClientConfig.DeviceModel row)
        {
            EditorGUILayout.BeginHorizontal();

            bool deleteMe = GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight));

            TextCell(row, m_CommentCellField, CommentCellWidth);
            TextCell(row, m_ModelNameCellField, ModelNameCellWidth);
            EnumCell(row, m_QualityLevelCellField, QualityLevelCellWidth);
            IntCell(row, m_MaxNearbyPlayerCountField, MaxNearbyPlayerCountWidth);
            IntCell(row, m_MaxNearbyPlayerModelTypeCountField, MaxNearbyPlayerModelTypeCountWidth);

            EditorGUILayout.EndHorizontal();

            return deleteMe;
        }

        #endregion Device Model UI

        #region Utilities

        private void IntCell(object obj, FieldInfo field, float width = 300)
        {
            int oldVal = (int)field.GetValue(obj);
            string text = EditorGUILayout.TextField(oldVal.ToString(), GUILayout.Width(width));
            int newVal = 0;
            if (int.TryParse(text, out newVal) && newVal != oldVal)
            {
                EditorUtility.SetDirty(m_Config);
                field.SetValue(obj, newVal);
            }
        }

        private void TextCell(object obj, FieldInfo field, float width = 300)
        {
            string oldText = field.GetValue(obj) as string;
            string text = oldText;

            text = EditorGUILayout.TextField(text, GUILayout.Width(width));

            if (text != oldText)
            {
                EditorUtility.SetDirty(m_Config);
            }

            field.SetValue(obj, text);
        }

        private void EnumCell(object obj, FieldInfo field, float width = 300)
        {
            Enum oldValue = field.GetValue(obj) as Enum;
            Enum value = oldValue;

            value = EditorGUILayout.EnumPopup(value, GUILayout.Width(width));

            if (value != oldValue)
            {
                EditorUtility.SetDirty(m_Config);
            }

            field.SetValue(obj, value);
        }

        private void ColorField(string name, object obj, FieldInfo field, float width = 300)
        {
            Color oldColor = (Color)field.GetValue(obj);
            Color color = oldColor;

            color = EditorGUILayout.ColorField(name, color, GUILayout.MaxWidth(width));

            if (oldColor != color)
            {
                EditorUtility.SetDirty(m_Config);
            }

            field.SetValue(obj, color);
        }

        #endregion Utilities

        #region Reflection

        private FieldInfo m_CommentCellField = typeof(ClientConfig.DeviceModel).GetField("m_Comment", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo m_ModelNameCellField = typeof(ClientConfig.DeviceModel).GetField("m_ModelName", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo m_QualityLevelCellField = typeof(ClientConfig.DeviceModel).GetField("m_QualityLevel", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo m_MaxNearbyPlayerCountField = typeof(ClientConfig.DeviceModel).GetField("m_MaxNearbyPlayerCount", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo m_MaxNearbyPlayerModelTypeCountField = typeof(ClientConfig.DeviceModel).GetField("m_MaxNearbyPlayerModelTypeCount", BindingFlags.NonPublic | BindingFlags.Instance);

        #endregion Reflection
    }
}

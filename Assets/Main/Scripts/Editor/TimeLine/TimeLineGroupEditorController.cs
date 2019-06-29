using GameFramework;
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
    internal class TimeLineGroupEditorController
    {
        private TextAsset m_TextAsset;
        private TimeLineGroupData m_GroupData;

        public TimeLineGroupData GroupData
        {
            get
            {
                return m_GroupData;
            }
        }

        private Dictionary<int, int> m_TimeLineIdUsedCount = new Dictionary<int, int>();

        public string AssetPath
        {
            get
            {
                return AssetDatabase.GetAssetPath(m_TextAsset);
            }
        }

        private HashSet<object> m_UnfoldedItems = new HashSet<object>();

        private List<Type> m_ActionDataTypes = new List<Type>();

        public IList<Type> ActionDataTypes
        {
            get
            {
                return m_ActionDataTypes;
            }
        }

        public TimeLineGroupEditorController()
        {

        }

        public string Init(TextAsset textAsset)
        {
            m_TextAsset = textAsset;

            string errorMessage = InitTimeLineGroupData();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }

            LoadActionTypes();
            InitTimeLineIdUsedCounts();
            return string.Empty;
        }
        
        private string InitTimeLineGroupData()
        {
            string timeLineGroupName = Path.GetFileNameWithoutExtension(AssetPath);
            string factoryTypeName = timeLineGroupName + "ActionFactory";
            Type factoryType = Assembly.Load("Assembly-CSharp").GetTypes().Where(t => t.Name == factoryTypeName).ToList()[0];
            MethodInfo factoryMethodInfo = factoryType.GetMethod("CreateData", BindingFlags.Static | BindingFlags.Public);
            GameFrameworkFunc<string, TimeLineActionData> actionDataCreator = Delegate.CreateDelegate(
                typeof(GameFrameworkFunc<string, TimeLineActionData>), factoryMethodInfo, true)
                as GameFrameworkFunc<string, TimeLineActionData>;
            m_GroupData = new TimeLineGroupData(timeLineGroupName, actionDataCreator);
            string errorMsg = m_GroupData.ParseData(m_TextAsset.text);
            return errorMsg;
        }

        public void AddTimeLineIdUsedCount(int id)
        {
            if (!m_TimeLineIdUsedCount.ContainsKey(id))
            {
                m_TimeLineIdUsedCount.Add(id, 1);
                return;
            }

            m_TimeLineIdUsedCount[id]++;
        }

        public void ReduceTimeLineIdUsedCount(int id)
        {
            if (!m_TimeLineIdUsedCount.ContainsKey(id))
            {
                return;
            }

            if (m_TimeLineIdUsedCount[id] <= 1)
            {
                m_TimeLineIdUsedCount.Remove(id);
                return;
            }

            m_TimeLineIdUsedCount[id]--;
        }

        public int GetTimeLineIdUsedCount(int id)
        {
            if (!m_TimeLineIdUsedCount.ContainsKey(id))
            {
                return 0;
            }

            return m_TimeLineIdUsedCount[id];
        }

        private void InitTimeLineIdUsedCounts()
        {
            foreach (var timeLineData in m_GroupData.TimeLines)
            {
                AddTimeLineIdUsedCount(timeLineData.Id);
            }
        }

        public void SetUnfolded(object o, bool unfolded)
        {
            if (unfolded)
            {
                m_UnfoldedItems.Add(o);
            }
            else
            {
                m_UnfoldedItems.Remove(o);
            }
        }

        public void SetAllUnfolded(bool unfolded)
        {
            if (!unfolded)
            {
                m_UnfoldedItems.Clear();
            }
        }

        public bool IsUnfolded(object o)
        {
            return m_UnfoldedItems.Contains(o);
        }

        public TimeLineData AddTimeLine()
        {
            int newId = 1;
            if (m_GroupData.TimeLines.Count > 0)
            {
                newId = Mathf.Max(0, m_GroupData.TimeLines.Max(tl => tl.Id)) + 1;
            }
            var newTimeLineData = new TimeLineData(newId);
            m_GroupData.TimeLines.Add(newTimeLineData);
            SetUnfolded(newTimeLineData, true);
            AddTimeLineIdUsedCount(newTimeLineData.Id);
            return newTimeLineData;
        }

        public void SaveData()
        {
            var filePath = Regex.Replace(AssetPath, @"^Assets", Application.dataPath);
            File.WriteAllLines(filePath, m_GroupData.SerializeData());
            AssetDatabase.ImportAsset(AssetPath);
        }

        public void RemoveTimeLine(TimeLineData timeLineData)
        {
            m_GroupData.TimeLines.Remove(timeLineData);
            SetUnfolded(timeLineData, false);
            foreach (var action in timeLineData.Actions)
            {
                SetUnfolded(action, false);
            }
            ReduceTimeLineIdUsedCount(timeLineData.Id);
        }

        private void LoadActionTypes()
        {
            m_ActionDataTypes.Clear();
            Assembly.Load("Assembly-CSharp").GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(TimeLineActionData))).ToList()
                .ForEach(t => m_ActionDataTypes.Add(t));
        }

        public void DuplicateTimeLine(TimeLineData timeLineData)
        {
            var newTimeLineData = AddTimeLine();
            foreach (var actionData in timeLineData.Actions)
            {
                newTimeLineData.Actions.Add(CopyTimeLineAction(actionData));
            }
        }

        public void DuplicateTimeLineAction(TimeLineData timeLineData, TimeLineActionData actionData)
        {
            int index = timeLineData.Actions.IndexOf(actionData);
            if (index < 0)
            {
                Log.Warning("Oops, action not found in time line.");
                return;
            }

            var newActionData = CopyTimeLineAction(actionData);
            timeLineData.Actions.Insert(index + 1, newActionData);
        }

        private TimeLineActionData CopyTimeLineAction(TimeLineActionData actionData)
        {
            Type type = actionData.GetType();
            var ret = Activator.CreateInstance(type) as TimeLineActionData;
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (field.FieldType.IsArray)
                {
                    Array srcArray = field.GetValue(actionData) as Array;
                    Array dstArray = Array.CreateInstance(field.FieldType.GetElementType(), srcArray.Length);
                    Array.Copy(srcArray, dstArray, srcArray.Length);
                    field.SetValue(ret, dstArray);
                    continue;
                }

                field.SetValue(ret, field.GetValue(actionData));
            }

            return ret;
        }

        public void AddActionData(TimeLineData timeLineData, Type type)
        {
            var newActionData = Activator.CreateInstance(type, true) as TimeLineActionData;
            SetUnfolded(newActionData, true);
            timeLineData.Actions.Add(newActionData);
            SetUnfolded(timeLineData, true);
        }

        public void RemoveActionData(TimeLineData timeLineData, TimeLineActionData timeLineActionData)
        {
            timeLineData.Actions.Remove(timeLineActionData);
            SetUnfolded(timeLineActionData, false);
        }

        public Array ChangeArrayLength(Array array, TimeLineActionData timeLineActionData, FieldInfo field, Type elementType, int length, int newLength)
        {
            Array newArray = Activator.CreateInstance(field.FieldType, new object[] { newLength }) as Array;

            for (int i = 0; i < newLength; ++i)
            {
                if (i < length)
                {
                    newArray.SetValue(array.GetValue(i), i);
                }
                else
                {
                    if (elementType == typeof(string))
                    {
                        newArray.SetValue(string.Empty, i);
                    }
                    else
                    {
                        newArray.SetValue(Activator.CreateInstance(elementType), i);
                    }
                }
            }

            SetUnfolded(array, false);
            array = newArray;
            field.SetValue(timeLineActionData, array);
            SetUnfolded(array, true);
            return array;
        }

        public void MoveUpTimeLine(int index)
        {
            if (index <= 0)
            {
                return;
            }

            var tmp = m_GroupData.TimeLines[index];
            m_GroupData.TimeLines[index] = m_GroupData.TimeLines[index - 1];
            m_GroupData.TimeLines[index - 1] = tmp;
        }

        public void MoveDownTimeLine(int index)
        {
            if (index >= m_GroupData.TimeLines.Count)
            {
                return;
            }

            var tmp = m_GroupData.TimeLines[index];
            m_GroupData.TimeLines[index] = m_GroupData.TimeLines[index + 1];
            m_GroupData.TimeLines[index + 1] = tmp;
        }

        public void SortTimeLinesById()
        {
            (m_GroupData.TimeLines as List<TimeLineData>).Sort((a, b) => a.Id.CompareTo(b.Id));
        }

        public int FindTimeLineById(int id)
        {
            //int index = -1;
            var index = (m_GroupData.TimeLines as List<TimeLineData>).FindIndex((p) => p.Id == id );
            return index;
        }

        public void MoveUpTimeLineAction(TimeLineData timeLineData, int index)
        {
            if (index <= 0)
            {
                return;
            }

            var tmp = timeLineData.Actions[index];
            timeLineData.Actions[index] = timeLineData.Actions[index - 1];
            timeLineData.Actions[index - 1] = tmp;
        }

        public void MoveDownTimeLineAction(TimeLineData timeLineData, int index)
        {
            if (index >= timeLineData.Actions.Count)
            {
                return;
            }

            var tmp = timeLineData.Actions[index];
            timeLineData.Actions[index] = timeLineData.Actions[index + 1];
            timeLineData.Actions[index + 1] = tmp;
        }

        public void SortTimeLineActionsByName(TimeLineData timeLineData)
        {
            (timeLineData.Actions as List<TimeLineActionData>).Sort((a, b) => a.ActionType.CompareTo(b.ActionType));
        }
    }
}

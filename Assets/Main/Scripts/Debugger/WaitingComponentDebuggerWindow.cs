using GameFramework.Debugger;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    public class WaitingComponentDebuggerWindow : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;
        private IDictionary<WaitingType, IDictionary<string, int>> m_SampledData = new Dictionary<WaitingType, IDictionary<string, int>>();
        private string m_SerializedSampledData = string.Empty;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void OnDraw()
        {
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Sample", GUILayout.Height(30f)))
                {
                    CopySampledData(GameEntry.Waiting.GetDetailedWaitingCounts());
                    GetSerializedSampledData();
                }

                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {
                    GUILayout.Label("<b>Sampled Data</b>");
                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.Label(m_SerializedSampledData);
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void GetSerializedSampledData()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("{");

            foreach (var kv in m_SampledData)
            {
                sb.AppendFormat("  \"{0}\": {{\n", kv.Key);

                var subDict = m_SampledData[kv.Key];
                foreach (var kv2 in subDict)
                {
                    if (kv2.Value != 0)
                    {
                        sb.AppendFormat("    {{ \"{0}\": {1} }},\n", kv2.Key, kv2.Value.ToString());
                    }
                }

                sb.Append("  },\n");
            }

            sb.AppendLine("}\n");
            m_SerializedSampledData = sb.ToString();
        }

        private void CopySampledData(IDictionary<WaitingType, IDictionary<string, int>> sampledData)
        {
            if (sampledData == null)
            {
                return;
            }

            foreach (var kv in sampledData)
            {
                m_SampledData[kv.Key] = new Dictionary<string, int>();
                var subDict = m_SampledData[kv.Key];
                foreach (var kv2 in sampledData[kv.Key])
                {
                    subDict[kv2.Key] = sampledData[kv.Key][kv2.Key];
                }
            }
        }
    }
}

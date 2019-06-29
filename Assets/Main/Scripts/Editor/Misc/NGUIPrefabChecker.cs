using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// NGUI Prefab 检查器。
    /// </summary>
    public static class NGUIPrefabChecker
    {
        private const int SpacesPerIndent = 4;

        [MenuItem("Assets/Check NGUI Prefab", validate = true)]
        public static bool Validate()
        {
            var prefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
            return prefabs.Length > 0;
        }

        [MenuItem("Assets/Check NGUI Prefab")]
        public static void Run()
        {
            var prefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets).ToList().ConvertAll(o => o as GameObject);

            StringBuilder sb = new StringBuilder();
            foreach (var prefab in prefabs)
            {
                int indent = 0;

                sb.Append(' ', indent * SpacesPerIndent);
                sb.AppendFormat("Prefab - {0}: {{\n", prefab.name);
                //var buttons = prefab.GetComponentsInChildren<UIButton>(true);

                //foreach (UIButton button in buttons)
                //{
                //    ++indent;
                //    sb.Append(' ', indent * SpacesPerIndent);
                //    sb.AppendFormat("Button - {0}: [\n", button.name);

                //    foreach (var del in button.onClick)
                //    {
                //        ++indent;
                //        sb.Append(' ', indent * SpacesPerIndent);
                //        sb.AppendFormat("{0}.{1},\n", del.target.GetType().Name, del.methodName);
                //        --indent;
                //    }

                //    sb.Append(' ', indent * SpacesPerIndent);
                //    sb.Append("],\n");
                //    --indent;
                //}

                var dynamicAnchorsCtls = prefab.GetComponentsInChildren<UIDynamicAnchors>(true);

                foreach (var anchorsCtl in dynamicAnchorsCtls)
                {
                    ++indent;
                    sb.Append(' ', indent * SpacesPerIndent);
                    sb.AppendFormat("DynamicAnchors - {0}: [\n", anchorsCtl.name);

                    ++indent;
                    sb.Append(' ', indent * SpacesPerIndent);
                    sb.AppendFormat("Target path: {0}\n", anchorsCtl.GetType().GetField("m_TargetPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(anchorsCtl) as string);
                    --indent;

                    sb.Append(' ', indent * SpacesPerIndent);
                    sb.Append("],\n");
                    --indent;
                }

                sb.Append(' ', indent * SpacesPerIndent);
                sb.Append("}\n");
                --indent;
            }

            Debug.Log(sb.ToString());
        }
    }
}

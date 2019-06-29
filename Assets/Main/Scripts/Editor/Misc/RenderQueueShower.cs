using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public static class RenderQueueShower
    {
        private static int s_Indent = 0;

        private static string IndentStr
        {
            get
            {
                return new string(' ', s_Indent * 4);
            }
        }

        [MenuItem("Game Framework/Show Render Queue", validate = false, priority = 4001)]
        public static void Run()
        {
            var gos = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel).Where(o => o is GameObject).Cast<GameObject>().ToList();
            if (gos.Count <= 0)
            {
                return;
            }

            var go = gos[0];
            var renderers = go.GetComponentsInChildren<Renderer>(true);

            StringBuilder logSb = new StringBuilder();
            logSb.AppendLine(string.Format("{0}GameObject: {1}", IndentStr, go.name));
            s_Indent = 0;

            ++s_Indent;
            for (int i = 0; i < renderers.Length; ++i)
            {

                var renderer = renderers[i];
                logSb.AppendLine(string.Format("{0}Renderer: {1} ({2})", IndentStr, renderer.name, renderer.GetType().Name));

                var mats = renderer.materials;
                ++s_Indent;
                for (int j = 0; j < mats.Length; ++j)
                {

                    var mat = mats[j];
                    logSb.AppendLine(string.Format("{0}Material: {1} (Shader: {2}, Renderer Queue: {3})", IndentStr, mat.name, mat.shader.name, mat.renderQueue));
                }
                --s_Indent;
            }
            --s_Indent;

            Debug.Log(logSb.ToString());
        }

        [MenuItem("Game Framework/Show Render Queue", validate = true, priority = 12000)]
        public static bool Validate()
        {
            var gos = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel).Where(o => o is GameObject).Cast<GameObject>().ToList();
            return gos.Count > 0;
        }
    }
}

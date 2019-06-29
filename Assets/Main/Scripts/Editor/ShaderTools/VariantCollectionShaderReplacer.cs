using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class VariantCollectionShaderReplacer
    {
        [MenuItem("Game Framework/Shader Tools/Replace Shaders in VariantCollection", priority = 5002)]
        private static void Run()
        {
            var shaderList = ShaderGUIDInfo.ShaderGUIDs;
            if (null == shaderList || shaderList.Length <= 0)
                return;

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!path.EndsWith(".shadervariants"))
                return;

            string content = File.ReadAllText(path);

            foreach (var info in shaderList)
            {
                Shader shader = Shader.Find(info.Name);
                if (null == shader)
                    continue;

                string shaderPath = AssetDatabase.GetAssetPath(shader);
                if (string.IsNullOrEmpty(shaderPath))
                    continue;

                string newGUID = AssetDatabase.AssetPathToGUID(shaderPath);
                if (string.IsNullOrEmpty(newGUID) || newGUID == info.GUID)
                    continue;

                Regex regex = new Regex(@"\{\s*fileID\:\s*" + info.FileID + @"\s*,\s*guid\:\s*" + info.GUID + @"\s*,\s*type\:\s*" + info.Type + @"\s*\}");
                string newString = "{fileID: 4800000, guid: " + newGUID + ", type: 3}";
                content = regex.Replace(content, newString);

                //var matches = regex.Matches(content);
                //foreach (Match match in matches)
                //{
                //    Debug.Log(match.Groups[0].Value + "\n" + newString);
                //}
            }

            File.WriteAllText(path, content);
            Debug.Log("Done.");
        }
    }
}

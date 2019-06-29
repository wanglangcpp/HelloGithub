using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

public class DefaultDictionary2txt
{
    [MenuItem("Tools/DoDefaultDictionary2xml")]
    static void DoDefaultDictionary2xml()
    {
        
        Dictionary<string, string> dic = new Dictionary<string, string>();
        using (FileStream fs = File.Open(@"E:\gameclient\Assets\Main\Publisher\DefaultDictionary.txt", FileMode.Open))
        {
            StreamReader sr = new StreamReader(fs, Encoding.Unicode);
            string lineText = null;
            while (!string.IsNullOrEmpty(lineText = sr.ReadLine()))
            {
                if (lineText.StartsWith("#"))
                    continue;
                string[] texts = lineText.Split('\t');
                if(texts.Length == 4)
                {
                    dic.Add(texts[1], texts[3]);
                }
                else
                {
                    Debug.LogError("Error: " + lineText);
                }
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"E:\gameclient\Assets\Main\Publisher\DefaultDictionary.xml");
            XmlNode xmlRoot = xmlDocument.SelectSingleNode("Dictionaries");
            XmlNodeList xmlNodeDictionaryList = xmlRoot.ChildNodes;
            EditorUtility.DisplayProgressBar("转表", "已完成0%", 0);
            for (int i = 0,j = xmlNodeDictionaryList.Count; i < j; i++)
            {
                EditorUtility.DisplayProgressBar("转表", "已完成0%", (float)i/j);
                if (xmlNodeDictionaryList[i].Attributes.GetNamedItem("Language").Value != "English")
                    continue;
                XmlNodeList keyValues = xmlNodeDictionaryList[i].ChildNodes;
                string key = null;
               for(int a = 0,b = keyValues.Count;a < b;a++)
                {
                    key = keyValues[a].Attributes.GetNamedItem("Key").Value;
                    if(!dic.ContainsKey(key))
                    {
                        Debug.LogError("Error: dic have no key " + key);
                        continue;
                    }
                    keyValues[a].Attributes.GetNamedItem("Value").Value = dic[key];
                }
            }
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("转表","转表完成","好的~退下");
            xmlDocument.Save(@"E:\gameclient\Assets\Main\Publisher\DefaultDictionary.xml");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Tools/DefaultDictionary2txt")]
    static void DoDefaultDictionary2txt()
    {
        Object obj = Selection.activeObject;
        string objPath = null;
        if(null != obj)
        {
            objPath = AssetDatabase.GetAssetPath(obj);
        }
        if(null != objPath && Path.GetExtension(objPath) == ".xml")
        {
            TextAsset ta = AssetDatabase.LoadAssetAtPath(objPath,typeof(TextAsset)) as TextAsset;
            if(null == ta)
            {
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(ta.text);
            XmlNode xmlRoot = xmlDocument.SelectSingleNode("Dictionaries");
            XmlNodeList xmlNodeDictionaryList = xmlRoot.ChildNodes;
            using (FileStream fs = File.Create(Application.dataPath + objPath.Replace("Assets", "").Replace("xml", "txt")))
            {
                using (System.IO.StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("#\t字典配置表");
                    sw.WriteLine("#\tKey\t\tValue");
                    sw.WriteLine("#\tstring\t\tstring");
                    sw.WriteLine("#\t字典主键\t策划备注\t字典内容，可使用转义符");
                    for (int i = 0; i < xmlNodeDictionaryList.Count; i++)
                    {
                        XmlNode xmlNode = xmlNodeDictionaryList.Item(i);
                        if (xmlNode.Name != "Dictionary")
                        {
                            continue;
                        }
                        XmlNodeList xmlNodeStringList = xmlNode.ChildNodes;
                        for (int j = 0; j < xmlNodeStringList.Count; j++)
                        {
                            XmlNode xmlNodeString = xmlNodeStringList.Item(j);
                            if (xmlNodeString.Name != "String")
                            {
                                continue;
                            }
                            string key = xmlNodeString.Attributes.GetNamedItem("Key").Value;
                            string value = xmlNodeString.Attributes.GetNamedItem("Value").Value;
                            //sw.WriteLine("	" + key + "		" + value);
                            sw.WriteLine("\t" + key + "\t\t" + value);
                        }
                    }
                }
            }

        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class AndroidManifestModifier
    {
        private XmlDocument m_Doc;
        private string m_Path;
        private XmlElement m_RootNode;
        private XmlElement m_ApplicationNode;
        private XmlElement m_MainActivityNode;

        public AndroidManifestModifier(string path)
        {
            m_Path = path;
        }

        public void Load()
        {
            m_Doc = new XmlDocument();
            m_Doc.Load(m_Path);

            m_RootNode = m_Doc.SelectSingleNode("manifest") as XmlElement;
            m_ApplicationNode = m_RootNode.SelectSingleNode("application") as XmlElement;
            foreach (XmlNode rawActivityNode in m_ApplicationNode.SelectNodes("activity"))
            {
                var activityNode = rawActivityNode as XmlElement;
                if (activityNode == null)
                {
                    continue;
                }

                var intentFilterNode = activityNode.SelectSingleNode("intent-filter");
                if (intentFilterNode == null)
                {
                    continue;
                }

                var actionNode = intentFilterNode.SelectSingleNode("action");
                var categoryNode = intentFilterNode.SelectSingleNode("category");
                if (actionNode == null || categoryNode == null)
                {
                    continue;
                }

                var actionNameAttr = actionNode.Attributes["android:name"];
                var categoryNameAttr = categoryNode.Attributes["android:name"];
                if (actionNameAttr != null && actionNameAttr.Value == "android.intent.action.MAIN" && categoryNameAttr != null && categoryNameAttr.Value == "android.intent.category.LAUNCHER")
                {
                    m_MainActivityNode = activityNode;
                    break;
                }
            }

            if (m_MainActivityNode == null)
            {
                throw new Exception("Main activity not found.");
            }
        }

        public void Save()
        {
            m_Doc.Save(m_Path);
        }

        public void RemoveApplicationAttribute(string name)
        {
            m_ApplicationNode.RemoveAttribute(name);
        }

        public void UpdateMainActivityAttribute(string name, string value)
        {
            if (m_MainActivityNode.Attributes[name] == null)
            {
                m_MainActivityNode.Attributes.Append(m_Doc.CreateAttribute(name));
            }

            m_MainActivityNode.Attributes[name].Value = value;
        }

        public void AddXmlFragmentUnderApplication(string xmlContent)
        {
            AddXmlFragmentUnderNode(xmlContent, m_ApplicationNode);
        }

        public void AddXmlFragmentUnderRoot(string xmlContent)
        {
            AddXmlFragmentUnderNode(xmlContent, m_RootNode);
        }

        public void Print()
        {
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, IndentChars = " ", Encoding = Encoding.UTF8 }))
                {
                    m_Doc.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                    Debug.Log(stringWriter.GetStringBuilder().ToString());
                }
            }
        }

        private void AddXmlFragmentUnderNode(string xmlContent, XmlElement node)
        {
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("android", "http://schemas.android.com/apk/res/android");
            var setting = new XmlReaderSettings();
            setting.NameTable = nt;
            setting.ConformanceLevel = ConformanceLevel.Fragment;
            var context = new XmlParserContext(nt, nsmgr, string.Empty, XmlSpace.None);
            using (var stringReader = new StringReader(xmlContent))
            {
                using (var xmlReader = XmlReader.Create(stringReader, setting, context))
                {
                    var doc = new XmlDocument();
                    doc.Load(xmlReader);
                    var frag = doc.DocumentElement;
                    var importedFrag = m_Doc.ImportNode(frag, true);
                    node.AppendChild(importedFrag);
                }
            }
        }

        public void SetSdkVersion(int minVersion, int targetVersion)
        {
            var usesSdkNode = m_RootNode.SelectSingleNode("uses-sdk");
            usesSdkNode.Attributes["android:minSdkVersion"].Value = minVersion.ToString();
            usesSdkNode.Attributes["android:targetSdkVersion"].Value = targetVersion.ToString();
        }
    }
}

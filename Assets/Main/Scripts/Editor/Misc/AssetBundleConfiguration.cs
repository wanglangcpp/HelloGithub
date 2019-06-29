using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class AssetBundleConfiguration
    {
        public class AssetBundle
        {
            public string Name;
            public string Variant;
            public bool Packed;
            public int LoadType;

            public AssetBundle(string assetBundleName, string assetBundleVariant, int assetBundleLoadType, bool assetBundlePacked)
            {
                Name = assetBundleName;
                Variant = assetBundleVariant;
                LoadType = assetBundleLoadType;
                Packed = assetBundlePacked;
            }
        }

        public class Asset
        {
            public string Guid;
            public string AssetBundleName;
            public string AssetBundleVariant;

            public Asset(string assetGuid, string assetBundleName, string assetBundleVariant)
            {
                Guid = assetGuid;
                AssetBundleName = assetBundleName;
                AssetBundleVariant = assetBundleVariant;
            }
        }

        public string AssetSorter;
        public string SourceAssetExceptLabelFilter;
        public string SourceAssetExceptTypeFilter;
        public string SourceAssetRootPath;
        public string SourceAssetUnionLabelFilter;
        public string SourceAssetUnionTypeFilter;

        private List<AssetBundle> m_AssetBundles;
        private List<Asset> m_Assets;

        public List<AssetBundle> AssetBundles
        {
            get
            {
                return m_AssetBundles;
            }
        }

        public List<Asset> Assets
        {
            get
            {
                return m_Assets;
            }
        }

        public bool Load(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("Can not found asset bundle editor configuration.");
                return false;
            }

            m_AssetBundles = new List<AssetBundle>();
            m_Assets = new List<Asset>();

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("UnityGameFramework");
                XmlNode xmlEditor = xmlRoot.SelectSingleNode("AssetBundleEditor");
                XmlNode xmlSetting = xmlEditor.SelectSingleNode("Setting");
                XmlNode xmlAssetBundles = xmlEditor.SelectSingleNode("AssetBundles");
                XmlNode xmlAssets = xmlEditor.SelectSingleNode("Assets");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlSetting.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    switch (xmlNode.Name)
                    {
                        case "SourceAssetRootPath":
                            SourceAssetRootPath = xmlNode.InnerText;
                            break;
                        case "SourceAssetUnionTypeFilter":
                            SourceAssetUnionTypeFilter = xmlNode.InnerText;
                            break;
                        case "SourceAssetUnionLabelFilter":
                            SourceAssetUnionLabelFilter = xmlNode.InnerText;
                            break;
                        case "SourceAssetExceptTypeFilter":
                            SourceAssetExceptTypeFilter = xmlNode.InnerText;
                            break;
                        case "SourceAssetExceptLabelFilter":
                            SourceAssetExceptLabelFilter = xmlNode.InnerText;
                            break;
                        case "AssetSorter":
                            AssetSorter = xmlNode.InnerText;
                            break;
                    }
                }

                xmlNodeList = xmlAssetBundles.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "AssetBundle")
                    {
                        continue;
                    }

                    string assetBundleName = xmlNode.Attributes.GetNamedItem("Name").Value;
                    string assetBundleVariant = xmlNode.Attributes.GetNamedItem("Variant") != null ? xmlNode.Attributes.GetNamedItem("Variant").Value : null;
                    int assetBundleLoadType = 0;
                    if (xmlNode.Attributes.GetNamedItem("LoadType") != null)
                    {
                        int.TryParse(xmlNode.Attributes.GetNamedItem("LoadType").Value, out assetBundleLoadType);
                    }
                    bool assetBundlePacked = false;
                    if (xmlNode.Attributes.GetNamedItem("Packed") != null)
                    {
                        bool.TryParse(xmlNode.Attributes.GetNamedItem("Packed").Value, out assetBundlePacked);
                    }

                    m_AssetBundles.Add(new AssetBundle(assetBundleName, assetBundleVariant, assetBundleLoadType, assetBundlePacked));

                }

                m_AssetBundles.Sort(AssetBundleComparer);

                xmlNodeList = xmlAssets.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "Asset")
                    {
                        continue;
                    }

                    string assetGuid = xmlNode.Attributes.GetNamedItem("Guid").Value;
                    string assetBundleName = xmlNode.Attributes.GetNamedItem("AssetBundleName").Value;
                    string assetBundleVariant = xmlNode.Attributes.GetNamedItem("AssetBundleVariant") != null ? xmlNode.Attributes.GetNamedItem("AssetBundleVariant").Value : null;

                    m_Assets.Add(new Asset(assetGuid, assetBundleName, assetBundleVariant));
                }

                return true;
            }
            catch
            {
                m_AssetBundles = null;
                m_Assets = null;

                return false;
            }
        }

        public bool Save(string path)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement xmlRoot = xmlDocument.CreateElement("UnityGameFramework");
                xmlDocument.AppendChild(xmlRoot);

                XmlElement xmlEditor = xmlDocument.CreateElement("AssetBundleEditor");
                xmlRoot.AppendChild(xmlEditor);

                XmlElement xmlSetting = xmlDocument.CreateElement("Setting");
                xmlEditor.AppendChild(xmlSetting);

                XmlElement xmlAssetBundles = xmlDocument.CreateElement("AssetBundles");
                xmlEditor.AppendChild(xmlAssetBundles);

                XmlElement xmlAssets = xmlDocument.CreateElement("Assets");
                xmlEditor.AppendChild(xmlAssets);

                XmlElement xmlElement = null;
                XmlAttribute xmlAttribute = null;

                xmlElement = xmlDocument.CreateElement("SourceAssetRootPath");
                xmlElement.InnerText = SourceAssetRootPath.ToString();
                xmlSetting.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetUnionTypeFilter");
                xmlElement.InnerText = SourceAssetUnionTypeFilter ?? string.Empty;
                xmlSetting.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetUnionLabelFilter");
                xmlElement.InnerText = SourceAssetUnionLabelFilter ?? string.Empty;
                xmlSetting.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetExceptTypeFilter");
                xmlElement.InnerText = SourceAssetExceptTypeFilter ?? string.Empty;
                xmlSetting.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetExceptLabelFilter");
                xmlElement.InnerText = SourceAssetExceptLabelFilter ?? string.Empty;
                xmlSetting.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AssetSorter");
                xmlElement.InnerText = AssetSorter ?? string.Empty;
                xmlSetting.AppendChild(xmlElement);

                foreach (AssetBundle assetBundle in m_AssetBundles)
                {
                    xmlElement = xmlDocument.CreateElement("AssetBundle");
                    xmlAttribute = xmlDocument.CreateAttribute("Name");
                    xmlAttribute.Value = assetBundle.Name;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    if (assetBundle.Variant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("Variant");
                        xmlAttribute.Value = assetBundle.Variant;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    xmlAttribute = xmlDocument.CreateAttribute("LoadType");
                    xmlAttribute.Value = assetBundle.LoadType.ToString();
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    xmlAttribute = xmlDocument.CreateAttribute("Packed");
                    xmlAttribute.Value = assetBundle.Packed.ToString();
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    xmlAssetBundles.AppendChild(xmlElement);
                }

                m_Assets.Sort(AssetComparer);

                foreach (Asset asset in m_Assets)
                {
                    xmlElement = xmlDocument.CreateElement("Asset");
                    xmlAttribute = xmlDocument.CreateAttribute("Guid");
                    xmlAttribute.Value = asset.Guid;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    xmlAttribute = xmlDocument.CreateAttribute("AssetBundleName");
                    xmlAttribute.Value = asset.AssetBundleName;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    if (asset.AssetBundleVariant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("AssetBundleVariant");
                        xmlAttribute.Value = asset.AssetBundleVariant;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    xmlAssets.AppendChild(xmlElement);
                }

                xmlDocument.Save(path);
                return true;
            }
            catch
            {
                File.Delete(path);
                return false;
            }
        }

        public bool ReassignAssetBundle(string assetGuid, string assetBundleName, string assetBundleVariant, bool clean = true)
        {
            Asset asset = GetAsset(assetGuid);

            if (null == asset)
                return false;

            AssetBundle oldBundle = GetAssetBundle(asset.AssetBundleName, asset.AssetBundleVariant);
            AssetBundle newBundle = GetAssetBundle(assetBundleName, assetBundleVariant);

            if (null == oldBundle)
                return false;

            if (null == newBundle)
            {
                newBundle = new AssetBundle(assetBundleName, assetBundleVariant, oldBundle.LoadType, oldBundle.Packed);
                m_AssetBundles.Add(newBundle);
            }

            asset.AssetBundleName = newBundle.Name;
            asset.AssetBundleVariant = newBundle.Variant;

            if (clean && IsBundleEmpty(oldBundle))
            {
                m_AssetBundles.Remove(oldBundle);
            }

            return true;
        }

        public void Clean()
        {
            for (int i = m_AssetBundles.Count - 1; i >= 0; i--)
            {
                if (IsBundleEmpty(m_AssetBundles[i]))
                {
                    m_AssetBundles.RemoveAt(i);
                }
            }

            m_AssetBundles.Sort(AssetBundleComparer);
            m_Assets.Sort(AssetComparer);
        }

        private bool IsBundleEmpty(AssetBundle bundle)
        {
            foreach (var asset in m_Assets)
            {
                if (asset.AssetBundleName.ToLower() == bundle.Name.ToLower() && asset.AssetBundleVariant == bundle.Variant)
                {
                    return false;
                }
            }

            return true;
        }

        private Asset GetAsset(string assetGuid)
        {
            foreach (var asset in m_Assets)
            {
                if (asset.Guid == assetGuid)
                {
                    return asset;
                }
            }
            return null;
        }

        private AssetBundle GetAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            foreach (var bundle in m_AssetBundles)
            {
                if (bundle.Name.ToLower() == assetBundleName.ToLower() && bundle.Variant == assetBundleVariant)
                {
                    return bundle;
                }
            }
            return null;
        }

        private int AssetBundleComparer(AssetBundle a, AssetBundle b)
        {
            int compareValue = a.Name.CompareTo(b.Name);
            if (compareValue != 0)
            {
                return compareValue;
            }

            string aVariant = a.Variant ?? string.Empty;
            string bVariant = b.Variant ?? string.Empty;

            return aVariant.CompareTo(bVariant);
        }

        private int AssetComparer(Asset a, Asset b)
        {
            return a.Guid.CompareTo(b.Guid);
        }
    }
}

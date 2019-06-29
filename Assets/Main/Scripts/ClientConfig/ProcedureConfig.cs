using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ProcedureConfig
    {
        [SerializeField]
        private ProcedureLaunchConfig m_ProcedureLaunchConfig = null;

        [SerializeField]
        private ProcedureSplashConfig m_ProcedureSplashConfig = null;

        [SerializeField]
        private ProcedureCheckVersionConfig m_ProcedureCheckVersionConfig = null;

        [SerializeField]
        private ProcedureUpdateResourceConfig m_ProcedureUpdateResourceConfig = null;

        [SerializeField]
        private ProcedureCheckServerListConfig m_ProcedureCheckServerListConfig = null;

        [SerializeField]
        private ProcedureLoginConfig m_ProcedureLoginConfig = null;

        [SerializeField]
        private ProcedureUpdateTextsConfig m_ProcedureUpdateTextsConfig = null;

        [SerializeField]
        private ProcedureChargeTableConfig m_ProcedureChargeTableConfig = null;

        public ProcedureLaunchConfig LaunchConfig
        {
            get { return m_ProcedureLaunchConfig; }
        }

        public ProcedureSplashConfig SplashConfig
        {
            get { return m_ProcedureSplashConfig; }
        }

        public ProcedureCheckVersionConfig CheckVersionConfig
        {
            get { return m_ProcedureCheckVersionConfig; }
        }

        public ProcedureUpdateResourceConfig UpdateResourceConfig
        {
            get { return m_ProcedureUpdateResourceConfig; }
        }

        public ProcedureCheckServerListConfig CheckServerListConfig
        {
            get { return m_ProcedureCheckServerListConfig; }
        }

        public ProcedureLoginConfig LoginConfig
        {
            get { return m_ProcedureLoginConfig; }
        }

        public ProcedureUpdateTextsConfig UpdateTextsConfig
        {
            get { return m_ProcedureUpdateTextsConfig; }
        }

        public ProcedureChargeTableConfig ChargeTableConfig
        {
            get { return m_ProcedureChargeTableConfig; }
        }

        [Serializable]
        public class ProcedureLaunchConfig
        {
            [SerializeField]
            private TextAsset m_DefaultDictionaryTextAsset = null;

            [SerializeField]
            private TextAsset m_BuildInfoTextAsset = null;

            [SerializeField]
            private TextAsset m_LogWhiteListTextAsset = null;

            public TextAsset DefaultDictionaryTextAsset
            {
                get { return m_DefaultDictionaryTextAsset; }
            }

            public TextAsset BuildInfoTextAsset
            {
                get { return m_BuildInfoTextAsset; }
            }

            public TextAsset LogWhiteListTextAsset
            {
                get { return m_LogWhiteListTextAsset; }
            }
        }

        [Serializable]
        public class ProcedureSplashConfig
        {
            [SerializeField]
            private GameObject m_ParentNode = null;

            [SerializeField]
            private string m_NodeName = "Splash";

            [SerializeField]
            private GameObject m_SplashTemplate = null;

            public GameObject ParentNode
            {
                get { return m_ParentNode; }
            }

            public string NodeName
            {
                get { return m_NodeName; }
            }

            public GameObject SplashTemplate
            {
                get { return m_SplashTemplate; }
                set { m_SplashTemplate = value; }
            }
        }

        [Serializable]
        public class ProcedureCheckVersionConfig
        {
            [SerializeField]
            private string m_CheckVersionUri = null;

            [SerializeField]
            private string m_UpdateResourceUri = null;

            public string CheckVersionUri
            {
                get { return m_CheckVersionUri; }
                set { m_CheckVersionUri = value; }
            }

            public string UpdateResourceUri
            {
                get { return m_UpdateResourceUri; }
                set { m_UpdateResourceUri = value; }
            }
        }

        [Serializable]
        public class ProcedureUpdateResourceConfig
        {
            [SerializeField]
            private GameObject m_DownloadingTemplate = null;

            [SerializeField]
            private GameObject m_ParentNode = null;

            [SerializeField]
            private string m_NodeName = "Downloading";

            public UnityEngine.GameObject DownloadingTemplate
            {
                get { return m_DownloadingTemplate; }
                set { m_DownloadingTemplate = value; }
            }

            public UnityEngine.GameObject ParentNode
            {
                get { return m_ParentNode; }
                set { m_ParentNode = value; }
            }

            public string NodeName
            {
                get { return m_NodeName; }
                set { m_NodeName = value; }
            }
        }

        [Serializable]
        public class ProcedureCheckServerListConfig
        {
            [SerializeField]
            private string m_CheckServerListUri = null;

            public string CheckServerListUri
            {
                get { return m_CheckServerListUri; }
                set { m_CheckServerListUri = value; }
            }
        }

        [Serializable]
        public class ProcedureLoginConfig
        {
            [SerializeField]
            private string m_LoginUri = null;

            public string LoginUri
            {
                get { return m_LoginUri; }
                set { m_LoginUri = value; }
            }
        }

        [Serializable]
        public class ProcedureUpdateTextsConfig
        {
            [SerializeField]
            private int m_DownloadRetryCount = 3;

            public int DownloadRetryCount
            {
                get { return m_DownloadRetryCount; }
                set { m_DownloadRetryCount = value; }
            }
        }

        [Serializable]
        public class ProcedureChargeTableConfig
        {
            [SerializeField]
            private string m_ChargeTableUri = null;

            public string ChargeTableUri
            {
                get { return m_ChargeTableUri; }
                set { m_ChargeTableUri = value; }
            }
        }
    }
}

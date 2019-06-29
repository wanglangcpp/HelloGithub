using System.Collections.Generic;
using System.Text;

namespace Genesis.GameClient
{
    public class StringReplacementRule_LabelColor : IStringReplacementRule
    {
        private Dictionary<string, string> m_ColorNames = new Dictionary<string, string>();

        private bool m_Inited = false;

        private StringBuilder m_SharedStringBuilder = new StringBuilder();

        public string Key
        {
            get
            {
                return "LC";
            }
        }

        public int MinArgCount
        {
            get
            {
                return 3;
            }
        }

        public string DoAction(string[] args)
        {
            EnsureColorNames();

            m_SharedStringBuilder.Length = 0;
            for (int i = 2; i < args.Length; ++i)
            {
                m_SharedStringBuilder.Append(args[i]);

                if (i < args.Length - 1)
                {
                    m_SharedStringBuilder.Append(',');
                }
            }

            return ColorUtility.AddStringColorToString(m_ColorNames[args[1]], m_SharedStringBuilder.ToString());
        }

        private void EnsureColorNames()
        {
            if (m_Inited)
            {
                return;
            }

            m_Inited = true;
            var configs = GameEntry.ClientConfig.GetStringReplacementLabelColorConfigs();
            for (int i = 0; i < configs.Count; ++i)
            {
                m_ColorNames.Add(configs[i].Key, ColorUtility.ColorToHexColor(configs[i].Color));
            }
        }
    }
}

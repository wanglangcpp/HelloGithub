using System.Collections.Generic;
using System.Text;

namespace Genesis.GameClient
{
    public partial class ProcedureCreatePlayer : ProcedureBase
    {
        private class RandomName
        {
            private const int NameNum = 3;

            private string[] m_RandomNameDic;
            private Dictionary<string, int> m_RandomNameCounts;

            private readonly static string[] KeyInfixes = new string[]
            {
                "_FIRST_NAME_",
                "_SECOND_NAME_",
                "_THIRD_NAME_",
            };

            public RandomName()
            {
                m_RandomNameDic = new string[NameNum];
                m_RandomNameCounts = new Dictionary<string, int>();
            }

            private int GetRandomNameCount(string namePrefix)
            {
                if (m_RandomNameCounts.ContainsKey(namePrefix))
                {
                    return m_RandomNameCounts[namePrefix];
                }

                int count = 0;
                for (int i = 1; /* Empty terminating condition */; i++)
                {
                    if (!GameEntry.Localization.HasRawString(namePrefix + i.ToString("D3")))
                    {
                        count = i - 1;
                        break;
                    }
                }
                m_RandomNameCounts.Add(namePrefix, count);

                return count;
            }

            private void ProduceName()
            {
                string str = string.Empty;
                if (GameEntry.Data.Player.IsFemale)
                {
                    str = "RANDOM_FEMALE";
                }
                else
                {
                    str = "RANDOM_MALE";
                }

                for (int i = 0; i < NameNum; i++)
                {
                    int randomNum = 0;
                    m_RandomNameDic[i] = str + KeyInfixes[i];
                    randomNum = UnityEngine.Random.Range(1, GetRandomNameCount(m_RandomNameDic[i]) + 1);
                    m_RandomNameDic[i] += randomNum.ToString("D3");
                }
            }

            public string GetRandomName()
            {
                ProduceName();
                var sb = new StringBuilder();
                for (int i = 0; i < NameNum; i++)
                {
                    sb.Append(GameEntry.Localization.GetString(m_RandomNameDic[i]));
                }

                return sb.ToString();
            }
        }
    }
}

using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Genesis.GameClient
{
    public class StringReplacementComponent : MonoBehaviour
    {
        private readonly IDictionary<string, IStringReplacementRule> m_StringReplacementRules = new Dictionary<string, IStringReplacementRule>();
        private readonly Regex m_FormatString = new Regex(@"`(.+?)`");
        private const char MatchStart = '`';
        private const char MatchEnd = '`';
        public const char MatchSeparator = ',';

        public string GetString(string stringForReplacement)
        {
            if (string.IsNullOrEmpty(stringForReplacement))
            {
                return string.Empty;
            }
            return m_FormatString.Replace(stringForReplacement, MatchEvaluator);
        }

        public string Pack(string key, params string[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MatchStart);
            sb.Append(key);
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == null || args[i].Contains(MatchStart.ToString()) || args[i].Contains(MatchEnd.ToString()))
                {
                    Log.Error("StringReplacementComponent's PackParam's args is null.");
                }
                sb.Append(MatchSeparator);
                sb.Append(args[i]);
            }
            sb.Append(MatchEnd);
            return sb.ToString();
        }

        private void Awake()
        {
            Type stringReplacementRuleType = typeof(IStringReplacementRule);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (stringReplacementRuleType.IsAssignableFrom(types[i]) && stringReplacementRuleType != types[i])
                {
                    IStringReplacementRule stringReplacementRule = (IStringReplacementRule)Activator.CreateInstance(types[i]);
                    m_StringReplacementRules.Add(stringReplacementRule.Key, stringReplacementRule);
                }
            }
        }

        private string MatchEvaluator(Match match)
        {
            string[] args = match.Groups[1].Value.Split(MatchSeparator);
            for(int i = 0,j = args.Length;i<j;i++)//我艹，有些替换字符会多出空格，不想改表只能这里手动去掉了
            {
                args[i] = args[i].Trim();
            }
            IStringReplacementRule stringReplacementRule = null;
            if (!m_StringReplacementRules.TryGetValue(args[0], out stringReplacementRule))
            {
                Log.Warning("Can not find string replacement rule '{0}' for string '{1}'.", args[0], match.Groups[1].Value);
                return match.Groups[0].Value;
            }

            if (stringReplacementRule.MinArgCount > args.Length)
            {
                Log.Warning("Argument count of string replacement rule '{0}' is wrong for string '{1}', need at least '{2}', input '{3}'.", args[0], match.Groups[1].Value, stringReplacementRule.MinArgCount.ToString(), args.Length.ToString());
                return match.Groups[0].Value;
            }

            return stringReplacementRule.DoAction(args);
        }
    }
}

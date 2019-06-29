using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色特效表。
    /// </summary>
    public class DRCharacterEffect : IDataRow
    {
        private const int EffectMaxCountPerScenario = 3;

        /// <summary>
        /// 模型编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 角色特效数据。
        /// </summary>
        public CharacterEffectData CharacterEffectData { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;

            CharacterEffectData = new CharacterEffectData();
            index = ParseOneScenario(CharacterEffectData.ModelShowDebut, text, index);

            for (int i = 0; i < Constant.CharacterForShowInteractionCount; ++i)
            {
                index = ParseOneScenario(CharacterEffectData.ModelShowInteractions[i], text, index);
            }

            index = ParseOneScenario(CharacterEffectData.ReceiveModelShowDebut, text, index);
        }

        private int ParseOneScenario(IList<KeyValuePair<string, string>> scenario, string[] text, int index)
        {
            for (int i = 0; i < EffectMaxCountPerScenario; ++i)
            {
                string effectResource = text[index++];
                string effectTransformPath = text[index++];
                if (string.IsNullOrEmpty(effectResource))
                {
                    continue;
                }

                scenario.Add(new KeyValuePair<string, string>(effectResource, effectTransformPath));
            }

            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRCharacterEffect>();
        }
    }
}

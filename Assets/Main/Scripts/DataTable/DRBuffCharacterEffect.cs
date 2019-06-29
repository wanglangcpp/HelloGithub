using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    internal class DRBuffCharacterEffect : IDataRow
    {

        public int Id
        {
            get;
            private set;
        }

        public string[] EffectPath
        {
            get;
            private set;
        }

        public string[] AttachPointPath
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            EffectPath = new string[Constant.MaxCharacterBuffEffectCount];
            AttachPointPath = new string[Constant.MaxCharacterBuffEffectCount];
            for (int i = 0; i < Constant.MaxCharacterBuffEffectCount; i++)
            {
                EffectPath[i] = text[index++];
                AttachPointPath[i] = text[index++];
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBuffCharacterEffect>();
        }
    }
}

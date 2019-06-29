using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 武器模型表。
    /// </summary>
    public class DRWeaponSuite : IDataRow
    {
        /// <summary>
        /// 模型编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 武器编号。
        /// </summary>
        public int[] WeaponId
        {
            get;
            private set;
        }

        public int GetWeaponId(int index)
        {
            return WeaponId[index];
        }

        /// <summary>
        /// 武器挂接点。
        /// </summary>
        public string[] AttachPointPath
        {
            get;
            private set;
        }

        public string GetAttachPointPath(int index)
        {
            return AttachPointPath[index];
        }

        /// <summary>
        /// 默认是否显示。
        /// </summary>
        public bool[] VisibleByDefault
        {
            get;
            private set;
        }

        public bool GetVisibleByDefault(int index)
        {
            return VisibleByDefault[index];
        }

        public bool IsWeaponAvailable(int index)
        {
            if (index < 0 || index >= Constant.MaxWeaponCountInSuite)
            {
                return false;
            }

            return WeaponId[index] > 0;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            WeaponId = new int[Constant.MaxWeaponCountInSuite];
            AttachPointPath = new string[Constant.MaxWeaponCountInSuite];
            VisibleByDefault = new bool[Constant.MaxWeaponCountInSuite];
            for (int i = 0; i < Constant.MaxWeaponCountInSuite; i++)
            {
                WeaponId[i] = int.Parse(text[index++]);
                AttachPointPath[i] = text[index++];
                VisibleByDefault[i] = bool.Parse(text[index++]);
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRWeaponSuite>();
        }
    }
}

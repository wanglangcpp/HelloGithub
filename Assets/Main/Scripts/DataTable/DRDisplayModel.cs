using System.Collections.Generic;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    /// <summary>
    /// 用来展示角色模型的场景及相关配置
    /// </summary>
    public class DRDisplayModel : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }


        /// <summary>
        /// 资源名称。
        /// </summary>
        public string SceneName
        {
            get;
            private set;
        }
        public float CameraX
        {
            get;
            private set;
        }
        public float CameraY
        {
            get;
            private set;
        }
        public float CameraZ
        {
            get;
            private set;
        }
        public float RotationX
        {
            get;
            private set;
        }
        public float RotationY
        {
            get;
            private set;
        }
        public float RotationZ
        {
            get;
            private set;
        }
        public float ViewOffsetX
        {
            get;
            private set;
        }
        public float ViewOffsetY
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
            SceneName = text[index++];
            CameraX = float.Parse(text[index++]);
            CameraY = float.Parse(text[index++]);
            CameraZ = float.Parse(text[index++]);
            RotationX = float.Parse(text[index++]);
            RotationY = float.Parse(text[index++]);
            RotationZ = float.Parse(text[index++]);
            ViewOffsetX = float.Parse(text[index++]);
            ViewOffsetY = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDisplayModel>();
        }

    }
}

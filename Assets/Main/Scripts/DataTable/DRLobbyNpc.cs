using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 主城Npc配置表。
    /// </summary>
    public class DRLobbyNpc : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// LobbyNpc名字。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// LobbyNpc对应名字版Button的IconId。
        /// </summary>
        public int IconId { get; private set; }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// 位置X。
        /// </summary>
        public float PositionX { get; private set; }

        /// <summary>
        /// 位置Y。
        /// </summary>
        public float PositionY { get; private set; }

        /// <summary>
        /// 朝向。
        /// </summary>
        public float Rotation { get; private set; }

        /// <summary>
        /// 碰撞半径（拾取范围）。
        /// </summary>
        public float ColliderRadius { get; private set; }

        /// <summary>
        /// 碰撞高度（名字板高度）。
        /// </summary>
        public float ColliderHeight { get; private set; }

        /// <summary>
        /// 触发中心点X坐标。
        /// </summary>
        public float ColliderCenterX { get; private set; }

        /// <summary>
        /// 触发中心点Y坐标。
        /// </summary>
        public float ColliderCenterY { get; private set; }

        /// <summary>
        /// 触发中心点Z坐标。
        /// </summary>
        public float ColliderCenterZ { get; private set; }

        /// <summary>
        /// 模型缩放比例。
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        /// 站立动作名称。
        /// </summary>
        public string Stand { get; private set; }

        /// <summary>
        /// 休闲动作名称。
        /// </summary>
        public string Idle { get; private set; }

        /// <summary>
        /// UIForm.txt对应Id。
        /// </summary>
        public int FormId { get; private set; }

        /// <summary>
        /// 随机喊话Id。
        /// </summary>
        public int PropagandaId { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            IconId = int.Parse(text[index++]);
            ResourceName = text[index++];
            PositionX = float.Parse(text[index++]);
            PositionY = float.Parse(text[index++]);
            Rotation = float.Parse(text[index++]);
            ColliderRadius = float.Parse(text[index++]);
            ColliderHeight = float.Parse(text[index++]);
            ColliderCenterX = float.Parse(text[index++]);
            ColliderCenterY = float.Parse(text[index++]);
            ColliderCenterZ = float.Parse(text[index++]);
            Scale = float.Parse(text[index++]);
            Stand = text[index++];
            Idle = text[index++];
            FormId = int.Parse(text[index++]);
            PropagandaId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRLobbyNpc>();
        }
    }
}

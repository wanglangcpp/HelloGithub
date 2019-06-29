using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色模型表。
    /// </summary>
    public class DRCharacter : IDataRow
    {
        /// <summary>
        /// 模型编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string ResourceName
        {
            get;
            private set;
        }

        /// <summary>
        /// 大厅用资源名称。
        /// </summary>
        public string ResourceNameForLobby
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示用资源名称。
        /// </summary>
        public string ResourceNameForShow
        {
            get;
            private set;
        }

        /// <summary>
        /// 寻路/碰撞半径。
        /// </summary>
        public float ColliderRadius
        {
            get;
            private set;
        }

        /// <summary>
        /// 寻路/碰撞高度。
        /// </summary>
        public float ColliderHeight
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害触发中心点X。
        /// </summary>
        public float ImpactCenterX
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害触发中心点Y。
        /// </summary>
        public float ImpactCenterY
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害触发中心点Z。
        /// </summary>
        public float ImpactCenterZ
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害触发半径。
        /// </summary>
        public float ImpactRadius
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害触发高度。
        /// </summary>
        public float ImpactHeight
        {
            get;
            private set;
        }

        /// <summary>
        /// 材质类型。
        /// </summary>
        public int MaterialType
        {
            get;
            private set;
        }

        /// <summary>
        /// 受击点。
        /// </summary>
        public string[] HitPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 眩晕特效点。
        /// </summary>
        public string StunEffectPoint
        {
            get;
            private set;
        }

        /// <summary>
        /// 冰冻特效点。
        /// </summary>
        public string FreezeEffectPoint
        {
            get;
            private set;
        }

        /// <summary>
        /// 冰冻结束特效点。
        /// </summary>
        public string FreezeBrokenEffectPoint
        {
            get;
            private set;
        }

        /// <summary>
        /// 受击点。
        /// </summary>
        /// <param name="index">受击点索引。</param>
        /// <returns></returns>
        public string GetHitPoint(int index)
        {
            return HitPoints[index];
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ResourceName = text[index++];
            ResourceNameForLobby = text[index++];
            ResourceNameForShow = text[index++];
            ColliderRadius = float.Parse(text[index++]);
            ColliderHeight = float.Parse(text[index++]);
            ImpactCenterX = float.Parse(text[index++]);
            ImpactCenterY = float.Parse(text[index++]);
            ImpactCenterZ = float.Parse(text[index++]);
            ImpactRadius = float.Parse(text[index++]);
            ImpactHeight = float.Parse(text[index++]);
            MaterialType = int.Parse(text[index++]);
            HitPoints = new string[Constant.HitPointCount];
            for (int i = 0; i < Constant.HitPointCount; i++)
            {
                HitPoints[i] = text[index++];
            }
            StunEffectPoint = text[index++];
            FreezeEffectPoint = text[index++];
            FreezeBrokenEffectPoint = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRCharacter>();
        }
    }
}

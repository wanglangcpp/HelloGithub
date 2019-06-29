using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获取途径及跳转组件。
    /// </summary>
    public class WhereToGetComponent : MonoBehaviour
    {
        private IDataTable<DRWhereToGet> m_CachedDataTable;

        private IDataTable<DRWhereToGet> CachedDataTable
        {
            get
            {
                if (m_CachedDataTable == null)
                {
                    m_CachedDataTable = GameEntry.DataTable.GetDataTable<DRWhereToGet>();
                }

                return m_CachedDataTable;
            }
        }
            
        private IDictionary<int, WhereToGetLogic_Base> m_Logics = new Dictionary<int, WhereToGetLogic_Base>();

        public WhereToGetLogic_Base GetLogic(int whereToGetId)
        {
            WhereToGetLogic_Base ret;
            if (m_Logics.TryGetValue(whereToGetId, out ret))
            {
                return ret;
            }

            DRWhereToGet dr = CachedDataTable.GetDataRow(whereToGetId);
            if (dr == null)
            {
                Log.Error("Where-to-get '{0}' not found.", whereToGetId);
                return null;
            }

            var logic = CreateLogic(dr);
            m_Logics.Add(whereToGetId, logic);
            return logic;
        }

        private static WhereToGetLogic_Base CreateLogic(DRWhereToGet dr)
        {
            switch (dr.Type)
            {
                case WhereToGetType.Text:
                    return new WhereToGetLogic_Text(dr.Params[0], dr.IconId);
                case WhereToGetType.UI:
                    return new WhereToGetLogic_UI(dr.Params[0], dr.IconId, (UIFormId)int.Parse(dr.Params[1]), dr.Params);
                case WhereToGetType.SinglePlayerInstance:
                    return new WhereToGetLogic_SinglePlayerInstance(dr);
                default:
                    throw new System.ArgumentOutOfRangeException("Where-to-get type '{0}' not found.", dr.Type.ToString());
            }
        }
    }
}

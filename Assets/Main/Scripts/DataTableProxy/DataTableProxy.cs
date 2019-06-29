using GameFramework.DataTable;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 数据表代理基类。
    /// </summary>
    public abstract class DataTableProxy<T> : IDataTable<T> where T : IDataRow
    {
        private IDataTable<T> m_RealDataTable = null;

        protected IDataTable<T> RealDataTable
        {
            get
            {
                if (m_RealDataTable == null)
                {
                    m_RealDataTable = GameEntry.DataTable.GetDataTable<T>();
                }

                if (m_RealDataTable != null)
                {
                    return m_RealDataTable;
                }

                throw new InvalidOperationException(string.Format("Cannot get data table with type '{0}'.", typeof(T).FullName));
            }
        }

        public T this[int id]
        {
            get
            {
                return RealDataTable[id];
            }
        }

        public int Count
        {
            get
            {
                return RealDataTable.Count;
            }
        }

        public string Name
        {
            get
            {
                return RealDataTable.Name;
            }
        }

        public Type Type
        {
            get
            {
                return RealDataTable.Type;
            }
        }

        public T MinIdDataRow
        {
            get
            {
                return RealDataTable.MinIdDataRow;
            }
        }

        public T MaxIdDataRow
        {
            get
            {
                return RealDataTable.MaxIdDataRow;
            }
        }

        public T[] GetAllDataRows()
        {
            return RealDataTable.GetAllDataRows();
        }

        public T[] GetAllDataRows(Predicate<T> condition)
        {
            return RealDataTable.GetAllDataRows(condition);
        }

        public T[] GetAllDataRows(Comparison<T> comparison)
        {
            return RealDataTable.GetAllDataRows(comparison);
        }

        public T[] GetAllDataRows(Predicate<T> condition, Comparison<T> comparison)
        {
            return RealDataTable.GetAllDataRows(condition, comparison);
        }

        public T GetDataRow(int id)
        {
            return RealDataTable.GetDataRow(id);
        }

        public T GetDataRow(Predicate<T> condition)
        {
            return RealDataTable.GetDataRow(condition);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return RealDataTable.GetEnumerator();
        }

        public bool HasDataRow(int id)
        {
            return RealDataTable.HasDataRow(id);
        }

        public bool HasDataRow(Predicate<T> condition)
        {
            return RealDataTable.HasDataRow(condition);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }
    }
}

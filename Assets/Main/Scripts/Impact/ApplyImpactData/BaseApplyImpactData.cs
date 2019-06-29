using GameFramework;
using GameFramework.DataTable;
using System.Reflection;
using System.Text;

namespace Genesis.GameClient
{
    public abstract class BaseApplyImpactData
    {
        public EntityData OriginData
        {
            get;
            private set;
        }

        public EntityData TargetData
        {
            get;
            private set;
        }

        public ImpactSourceType SourceType
        {
            get;
            private set;
        }

        public DRImpact DataRow
        {
            get;
            private set;
        }

        public int? SkillId
        {
            get;
            set;
        }

        public int? AIId
        {
            get;
            set;
        }

        public float? CurrentTime
        {
            get;
            set;
        }

        public static T Create<T>(EntityData originData, EntityData targetData, ImpactSourceType sourceType, int impactId) where T : BaseApplyImpactData, new()
        {
            T applyImpactData = new T();
            applyImpactData.Init(originData, targetData, sourceType, impactId);
            return applyImpactData;
        }

        private void Init(EntityData originData, EntityData targetData, ImpactSourceType sourceType, int impactId)
        {
            OriginData = originData;
            TargetData = targetData;
            SourceType = sourceType;

            IDataTable<DRImpact> dtImpact = GameEntry.DataTable.GetDataTable<DRImpact>();
            DRImpact dataRow = dtImpact.GetDataRow(impactId);
            if (dataRow == null)
            {
                Log.Warning("Can not load impact '{0}' from data table.", impactId.ToString());
                return;
            }

            DataRow = dataRow;

            SkillId = null;
            AIId = null;
            CurrentTime = null;
        }

        public void FillBaseInfo(RCPushEntityImpact rc)
        {
            if (rc.HasSkillId)
            {
                SkillId = rc.SkillId;
            }

            if (rc.HasAIId)
            {
                AIId = rc.AIId;
            }

            if (rc.HasCurrentTime)
            {
                CurrentTime = rc.CurrentTime;
            }
        }

        public override string ToString()
        {
            var pis = GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            for (int i = 0; i < pis.Length; ++i)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }

                var value = pis[i].GetValue(this, null);
                sb.AppendFormat("{0}: {1}", pis[i].Name, value == null ? "null" : value.ToString());
            }

            return sb.ToString();
        }
    }
}

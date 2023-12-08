using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Dev
{
    public class EntityDev : DBEntity
    {
#if !NET461
        public EntityDev(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityDev(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class DevPhasePara : DBCulture
        {
            public DevPhasePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class DevPhase : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<DevPhase> SelectDevPhaseList(DevPhasePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0012' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DevPhasePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(DevPhasePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<DevPhase> devPhaseList = new List<DevPhase>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DevPhase devPhase = new DevPhase()
                    {
                        CodeID = new DBVarChar(dataRow[DevPhase.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[DevPhase.DataField.CODE_NM.ToString()])
                    };
                    devPhaseList.Add(devPhase);
                }
                return devPhaseList;
            }
            return null;
        }
    }
}
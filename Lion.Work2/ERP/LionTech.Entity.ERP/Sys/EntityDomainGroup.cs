using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityDomainGroup : EntitySys
    {
        public EntityDomainGroup(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class DomainGroupPara : DBCulture
        {
            public DomainGroupPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                DOMAIN_NAME,
                DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM,
                PROJECT_NM
            }

            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
        }

        public class DomainGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                DOMAIN_NAME,
                DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM,
                PROJECT_NM,
                PROJECT_PARENT,
                IS_SVN,
                IS_WRITE,
                UPD_USER_NM,
                UPD_DT
            }

            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
            public DBNVarChar DomainGroupNM;
            public DBNVarChar ProjectNM;
            public DBVarChar ProjectParent;
            public DBChar IsSVN;
            public DBChar IsWrite;
            public DBVarChar UpdUserNM;
            public DBDateTime UpdDT;

            public string ItemText()
            {
                return this.DomainGroupNM.StringValue();
            }

            public string ItemValue()
            {
                return this.DomainGroupID.StringValue();
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

        public List<DomainGroup> SelectDomainGroupList(DomainGroupPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.DomainGroupID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere, "AND G.DOMAIN_GROUP_ID ={DOMAIN_GROUP_ID}"
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT G.DOMAIN_NAME ", Environment.NewLine,
                "	   , G.DOMAIN_GROUP_ID ", Environment.NewLine,
                "	   , dbo.FN_GET_NMID(G.DOMAIN_GROUP_ID, G.{DOMAIN_GROUP_NM}) AS DOMAIN_GROUP_NM ", Environment.NewLine,
                "	   , (CASE WHEN P.PROJECT_ID IS NOT NULL THEN dbo.FN_GET_NMID(P.PROJECT_ID, P.PROJECT_NM_ZH_TW) ELSE NULL END) AS PROJECT_NM ", Environment.NewLine,
                "	   , P.PROJECT_PARENT ", Environment.NewLine,
                "	   , P.IS_SVN ", Environment.NewLine,
                "	   , S.IS_WRITE ", Environment.NewLine,
                "	   , dbo.FN_GET_USER_NM(G.UPD_USER_ID) AS UPD_USER_NM ", Environment.NewLine,
                "	   , G.UPD_DT ", Environment.NewLine,
                "FROM SYS_DOMAIN_GROUP G ", Environment.NewLine,
                "LEFT JOIN SYS_SRC_DOMAIN_GROUP S ON S.DOMAIN_GROUP_ID = G.DOMAIN_GROUP_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SRC_PROJECT P ON P.PROJECT_ID = S.PROJECT_ID ", Environment.NewLine,
                "WHERE  G.DOMAIN_NAME ={DOMAIN_NAME} ", commandWhere, Environment.NewLine,
                "ORDER BY DOMAIN_GROUP_ID, IS_SVN DESC, PROJECT_PARENT, P.PROJECT_ID", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DomainGroupPara.ParaField.DOMAIN_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(DomainGroupPara.ParaField.DOMAIN_GROUP_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = DomainGroupPara.ParaField.PROJECT_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(DomainGroupPara.ParaField.PROJECT_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = DomainGroupPara.ParaField.DOMAIN_NAME.ToString(), Value = para.DomainName });
            dbParameters.Add(new DBParameter { Name = DomainGroupPara.ParaField.DOMAIN_GROUP_ID.ToString(), Value = para.DomainGroupID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<DomainGroup> domainGroupList = new List<DomainGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DomainGroup domainGroup = new DomainGroup()
                    {
                        DomainName = new DBVarChar(dataRow[DomainGroup.DataField.DOMAIN_NAME.ToString()]),
                        DomainGroupID = new DBVarChar(dataRow[DomainGroup.DataField.DOMAIN_GROUP_ID.ToString()]),
                        DomainGroupNM = new DBNVarChar(dataRow[DomainGroup.DataField.DOMAIN_GROUP_NM.ToString()]),
                        ProjectNM = new DBNVarChar(dataRow[DomainGroup.DataField.PROJECT_NM.ToString()]),
                        ProjectParent = new DBVarChar(dataRow[DomainGroup.DataField.PROJECT_PARENT.ToString()]),
                        IsSVN = new DBChar(dataRow[DomainGroup.DataField.IS_SVN.ToString()]),
                        IsWrite = new DBChar(dataRow[DomainGroup.DataField.IS_WRITE.ToString()]),
                        UpdUserNM = new DBVarChar(dataRow[DomainGroup.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[DomainGroup.DataField.UPD_DT.ToString()]),
                    };
                    domainGroupList.Add(domainGroup);
                }
                return domainGroupList;
            }
            return null;
        }
    }
}
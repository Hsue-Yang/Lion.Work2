using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySRCProject : EntitySys
    {
        public EntitySRCProject(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SRCProjectPara : DBCulture
        {
            public SRCProjectPara(string cultureID)
                : base(cultureID)
            {

            }
            public enum ParaField
            {
                PROJECT_ID, DOMAIN_NAME, DOMAIN_GROUP_ID,
                IS_WRITE,
                PROJECT_NM,
                PROJECT_NM_ZH_TW,
                PROJECT_NM_ZH_CN,
                PROJECT_NM_EN_US,
                PROJECT_PARENT,
                DOMAIN_GROUP_NM,
                UPD_USER_ID
            }

            public DBVarChar ProjectID;
            public DBVarChar ProjectNM;
            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
            public DBChar IsWrite;
            public DBChar IsSVN;
            public DBNVarChar ProjectNMZHTW;
            public DBNVarChar ProjectNMZHCN;
            public DBNVarChar ProjectNMENUS;
            public DBVarChar ProjectPraent;
            public DBVarChar UpdUserID;
        }

        public class SRCProject : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                PROJECT_ID, DOMAIN_NAME, DOMAIN_GROUP_ID,
                IS_WRITE,
                IS_SVN,
                PROJECT_NM,
                PROJECT_NM_ZH_TW,
                PROJECT_NM_ZH_CN,
                PROJECT_NM_EN_US,
                PROJECT_PARENT,
                REMARK,
                UPD_USER_NM,
                DOMAIN_GROUP_NM,
                UPD_DT
            }

            public DBVarChar ProjectID;
            public DBVarChar ProjectNM;
            public DBNVarChar Remark;
            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
            public DBVarChar DomainGroupNM;
            public DBChar IsWrite;
            public DBChar IsSVN;
            public DBNVarChar ProjectNMZHTW;
            public DBNVarChar ProjectNMZHCN;
            public DBNVarChar ProjectNMENUS;
            public DBVarChar ProjectPraent;
            public DBVarChar UpdUserNM;
            public DBDateTime UpdDT;

            public string ItemText()
            {
                return this.ProjectNM.StringValue();
            }

            public string ItemValue()
            {
                return this.ProjectID.StringValue();
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


        public List<SRCProject> SelectProjectMenuList(SRCProjectPara para)
        {
            string commandWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(para.DomainGroupID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, "AND D.DOMAIN_GROUP_ID ={DOMAIN_GROUP_ID} ", Environment.NewLine
                    });
            }
            string commandText = string.Concat(new object[]
                {
                    " SELECT DISTINCT(UPPER(P.PROJECT_ID)) ", Environment.NewLine,
                    "	   , P.PROJECT_ID ", Environment.NewLine,
                    "	   , dbo.FN_GET_NMID(P.PROJECT_ID, P.{PROJECT_NM}) AS PROJECT_NM ", Environment.NewLine,
                    "	   , IS_SVN ", Environment.NewLine,
                    " FROM SYS_SRC_PROJECT P ", Environment.NewLine,
                    "JOIN SYS_SRC_DOMAIN_GROUP D ON D.PROJECT_ID = P.PROJECT_ID ", Environment.NewLine,
                    "WHERE D.DOMAIN_NAME ={DOMAIN_NAME} ",commandWhere, Environment.NewLine,
                    "ORDER BY IS_SVN DESC, UPPER(P.PROJECT_ID) ", Environment.NewLine,
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.PROJECT_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SRCProjectPara.ParaField.PROJECT_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.DOMAIN_GROUP_ID, Value = para.DomainGroupID });
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.DOMAIN_NAME, Value = para.DomainName });
            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SRCProject> srcProjectList = new List<SRCProject>();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SRCProject srcProject = new SRCProject()
                    {
                        ProjectNM = new DBVarChar(dataRow[SRCProject.DataField.PROJECT_NM.ToString()]),
                        ProjectID = new DBVarChar(dataRow[SRCProject.DataField.PROJECT_ID.ToString()]),
                        IsSVN = new DBChar(dataRow[SRCProject.DataField.IS_SVN.ToString()]),
                    };

                    srcProjectList.Add(srcProject);
                }
                return srcProjectList;
            }
            return null;
        }

        public List<SRCProject> SelectProjectList(SRCProjectPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.DomainGroupID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, "AND S.DOMAIN_GROUP_ID ={DOMAIN_GROUP_ID} "
                    });
            }

            if (!string.IsNullOrWhiteSpace(para.ProjectID.GetValue()))
            {
                if (para.ProjectID.GetValue() == "isSVN")
                {
                    commandWhere = string.Concat(new object[]
                    {
                        commandWhere,"AND P.IS_SVN ='Y' "
                    });
                }
                else if (para.ProjectID.GetValue() == "notSVN")
                {
                    commandWhere = string.Concat(new object[]
                        {
                            commandWhere,"AND P.IS_SVN ='N' "
                        });
                }
                else
                {
                    commandWhere = string.Concat(new object[]
                    {
                        commandWhere,"AND S.PROJECT_ID ={PROJECT_ID} "
                    });
                }
                
            }

            string commandText = string.Concat(new object[]
                {
                    " SELECT dbo.FN_GET_NMID(S.PROJECT_ID, P.{PROJECT_NM}) AS PROJECT_NM", Environment.NewLine,
                    "      , S.DOMAIN_NAME", Environment.NewLine,
                    "      , dbo.FN_GET_NMID(S.DOMAIN_GROUP_ID, G.{DOMAIN_GROUP_NM}) AS DOMAIN_GROUP_NM", Environment.NewLine,
                    "      , S.IS_WRITE", Environment.NewLine,
                    "      , P.PROJECT_PARENT", Environment.NewLine,
                    "      , P.IS_SVN", Environment.NewLine,
                    "      , dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UPD_USER_NM", Environment.NewLine,
                    "      , S.PROJECT_ID", Environment.NewLine,
                    "      , P.REMARK", Environment.NewLine,
                    "      , S.DOMAIN_GROUP_ID", Environment.NewLine,
                    "      , S.UPD_DT", Environment.NewLine,
                    " FROM SYS_SRC_DOMAIN_GROUP S", Environment.NewLine,
                    "      JOIN SYS_SRC_PROJECT P ON S.PROJECT_ID = P.PROJECT_ID", Environment.NewLine,
                    "      JOIN SYS_DOMAIN_GROUP G ON S.DOMAIN_GROUP_ID = G.DOMAIN_GROUP_ID", Environment.NewLine,    
                    "WHERE S.DOMAIN_NAME ={DOMAIN_NAME} ",commandWhere, Environment.NewLine,    
                    "ORDER BY IS_SVN DESC, PROJECT_PARENT, UPPER(P.PROJECT_ID), S.DOMAIN_NAME", Environment.NewLine,
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.PROJECT_ID, Value = para.ProjectID });
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.DOMAIN_NAME, Value = para.DomainName });
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.DOMAIN_GROUP_ID, Value = para.DomainGroupID });
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.DOMAIN_GROUP_NM, Value = para.GetCultureFieldNM(new DBObject(SRCProjectPara.ParaField.DOMAIN_GROUP_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SRCProjectPara.ParaField.PROJECT_NM, Value = para.GetCultureFieldNM(new DBObject(SRCProjectPara.ParaField.PROJECT_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SRCProject> srcProjectList = new List<SRCProject>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SRCProject srcProject = new SRCProject()
                    {
                        ProjectID = new DBVarChar(dataRow[SRCProject.DataField.PROJECT_ID.ToString()]),
                        ProjectNM = new DBVarChar(dataRow[SRCProject.DataField.PROJECT_NM.ToString()]),
                        Remark = new DBNVarChar(dataRow[SRCProject.DataField.REMARK.ToString()]),
                        DomainName = new DBVarChar(dataRow[SRCProject.DataField.DOMAIN_NAME.ToString()]),
                        DomainGroupID = new DBVarChar(dataRow[SRCProject.DataField.DOMAIN_GROUP_ID.ToString()]),
                        DomainGroupNM = new DBVarChar(dataRow[SRCProject.DataField.DOMAIN_GROUP_NM.ToString()]),
                        IsWrite = new DBChar(dataRow[SRCProject.DataField.IS_WRITE.ToString()]),
                        ProjectPraent = new DBVarChar(dataRow[SRCProject.DataField.PROJECT_PARENT.ToString()]),
                        IsSVN = new DBChar(dataRow[SRCProject.DataField.IS_SVN.ToString()]),
                        UpdUserNM = new DBVarChar(dataRow[SRCProject.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SRCProject.DataField.UPD_DT.ToString()]),
                    };
                    srcProjectList.Add(srcProject);
                }
                return srcProjectList;
            }
            return null;
        }
    }
}
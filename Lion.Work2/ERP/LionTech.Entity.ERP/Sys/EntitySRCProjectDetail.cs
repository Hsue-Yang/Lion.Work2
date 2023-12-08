using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySRCProjectDetail : EntitySys
    {
        public EntitySRCProjectDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SRCProjectDetailPara : DBCulture
        {
            public SRCProjectDetailPara(string cultureID)
                : base(cultureID)
            {

            }
            public enum ParaField
            {
                PROJECT_ID, DOMAIN_NAME, DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM,
                IS_WRITE,
                IS_SVN,
                PROJECT_NM,
                PROJECT_NM_ZH_TW,
                PROJECT_NM_ZH_CN,
                PROJECT_NM_EN_US,
                PROJECT_NM_TH_TH,
                PROJECT_NM_JA_JP,
                PROJECT_NM_KO_KR,
                PROJECT_PARENT,
                REMARK,
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
            public DBNVarChar ProjectNMTHTH;
            public DBNVarChar ProjectNMJAJP;
            public DBNVarChar ProjectNMKOKR;
            public DBVarChar ProjectPraent;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class SRCProjectDetail : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                PROJECT_ID, DOMAIN_NAME, DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM,
                HAS_DOMAIN_GROUP_ID,
                IS_WRITE,
                IS_SVN,
                PROJECT_NM,
                PROJECT_NM_ZH_TW,
                PROJECT_NM_ZH_CN,
                PROJECT_NM_EN_US,
                PROJECT_NM_TH_TH,
                PROJECT_NM_JA_JP,
                PROJECT_NM_KO_KR,
                PROJECT_PARENT,
                PROJECT_PARENT_ID,
                REMARK,
                UPD_USER_ID
            }

            public DBVarChar ProjectID;
            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
            public DBNVarChar DomainGroupNM;
            public DBChar HasDomainGroupID;
            public DBChar IsWrite;
            public DBChar IsSVN;
            public DBNVarChar ProjectNMZHTW;
            public DBNVarChar ProjectNMZHCN;
            public DBNVarChar ProjectNMENUS;
            public DBNVarChar ProjectNMTHTH;
            public DBNVarChar ProjectNMJAJP;
            public DBNVarChar ProjectNMKOKR;
            public DBVarChar ProjectParent;
            public DBVarChar ProjectParentID;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;

            public string ItemText()
            {
                return this.ProjectParent.StringValue();
            }

            public string ItemValue()
            {
                return this.ProjectParentID.StringValue();
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

        public List<SRCProjectDetail> SelectDomainGroupList(SRCProjectDetailPara para)
        {
            string commandText = string.Concat(new object[]{
                "SELECT G.DOMAIN_NAME ", Environment.NewLine,
                "       ,G.DOMAIN_GROUP_ID ", Environment.NewLine,
                "       ,P.PROJECT_ID ", Environment.NewLine,
                "       ,(CASE WHEN P.IS_WRITE <> 'N' THEN 'Y' ELSE 'N' END) AS IS_WRITE ", Environment.NewLine,
                "       ,dbo.FN_GET_NMID(G.DOMAIN_GROUP_ID, G.{DOMAIN_GROUP_NM}) AS DOMAIN_GROUP_NM ", Environment.NewLine,
                "       ,(CASE WHEN P.PROJECT_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HAS_DOMAIN_GROUP_ID ", Environment.NewLine,
                "FROM SYS_DOMAIN_GROUP G ", Environment.NewLine,
                "LEFT OUTER JOIN( ", Environment.NewLine,
                "       SELECT PROJECT_ID, DOMAIN_GROUP_ID, DOMAIN_NAME, IS_WRITE ", Environment.NewLine,
                "       FROM SYS_SRC_DOMAIN_GROUP ", Environment.NewLine,
                "       WHERE PROJECT_ID = {PROJECT_ID} ", Environment.NewLine,
                "       ) P ON G.DOMAIN_GROUP_ID = P.DOMAIN_GROUP_ID ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.DOMAIN_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SRCProjectDetailPara.ParaField.DOMAIN_GROUP_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_ID.ToString(), Value = para.ProjectID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SRCProjectDetail> domainNameList = new List<SRCProjectDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SRCProjectDetail domainName = new SRCProjectDetail()
                    {
                        DomainName = new DBVarChar(dataRow[SRCProjectDetail.DataField.DOMAIN_NAME.ToString()]),
                        DomainGroupID = new DBVarChar(dataRow[SRCProjectDetail.DataField.DOMAIN_GROUP_ID.ToString()]),
                        ProjectID = new DBVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_ID.ToString()]),
                        IsWrite = new DBChar(dataRow[SRCProjectDetail.DataField.IS_WRITE.ToString()]),
                        DomainGroupNM = new DBNVarChar(dataRow[SRCProjectDetail.DataField.DOMAIN_GROUP_NM.ToString()]),
                        HasDomainGroupID = new DBChar(dataRow[SRCProjectDetail.DataField.HAS_DOMAIN_GROUP_ID.ToString()])
                    };
                    domainNameList.Add(domainName);
                }
                return domainNameList;
            }
            return null;
        }

        public List<SRCProjectDetail> SelectProjectParentList(SRCProjectDetailPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "SELECT DISTINCT(PROJECT_PARENT) ", Environment.NewLine,
                    "  , PROJECT_PARENT AS PROJECT_PARENT_ID ", Environment.NewLine,
                    "FROM SYS_SRC_PROJECT ", Environment.NewLine,
                    "WHERE PROJECT_PARENT <> 'NULL' ", Environment.NewLine,
                    "ORDER BY PROJECT_PARENT ", Environment.NewLine,
                });
            List<DBParameter> dbParameters = new List<DBParameter>();
            

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SRCProjectDetail> srcProjectDetailList = new List<SRCProjectDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SRCProjectDetail srcProjectDetail = new SRCProjectDetail()
                        {
                            ProjectParent = new DBVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_PARENT.ToString()]),
                            ProjectParentID = new DBVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_PARENT_ID.ToString()]),
                        };
                    srcProjectDetailList.Add(srcProjectDetail);
                }
                return srcProjectDetailList;
            }
            return null;
        }

        public SRCProjectDetail SelectSRCProjectDetail(SRCProjectDetailPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "SELECT DISTINCT(P.PROJECT_NM_ZH_TW) " , Environment.NewLine,
	                "   , P.PROJECT_NM_ZH_CN " , Environment.NewLine,
	                "   , P.PROJECT_NM_EN_US " , Environment.NewLine,
	                "   , P.PROJECT_NM_TH_TH " , Environment.NewLine,
	                "   , P.PROJECT_NM_JA_JP " , Environment.NewLine,
	                "   , P.PROJECT_NM_KO_KR " , Environment.NewLine,
	                "   , P.PROJECT_ID " , Environment.NewLine,
	                "   , P.PROJECT_PARENT " , Environment.NewLine,
                    "   , P.IS_SVN " , Environment.NewLine,
                    "   , P.REMARK " , Environment.NewLine,
                    "FROM SYS_SRC_PROJECT P " , Environment.NewLine,
                    "JOIN SYS_SRC_DOMAIN_GROUP D ON P.PROJECT_ID = D.PROJECT_ID " , Environment.NewLine,
                    "WHERE P.PROJECT_ID = {PROJECT_ID} ", Environment.NewLine,
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_ID.ToString(), Value = para.ProjectID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SRCProjectDetail srcProjectDetail = new SRCProjectDetail()
                {
                    ProjectNMZHTW = new DBNVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_NM_ZH_TW.ToString()]),
                    ProjectNMZHCN = new DBNVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_NM_ZH_CN.ToString()]),
                    ProjectNMENUS = new DBNVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_NM_EN_US.ToString()]),
                    ProjectNMTHTH = new DBNVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_NM_TH_TH.ToString()]),
                    ProjectNMJAJP = new DBNVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_NM_JA_JP.ToString()]),
                    ProjectNMKOKR = new DBNVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_NM_KO_KR.ToString()]),
                    ProjectID = new DBVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_ID.ToString()]),
                    ProjectParent = new DBVarChar(dataRow[SRCProjectDetail.DataField.PROJECT_PARENT.ToString()]),
                    IsSVN = new DBChar(dataRow[SRCProjectDetail.DataField.IS_SVN.ToString()]),
                    Remark = new DBNVarChar(dataRow[SRCProjectDetail.DataField.REMARK.ToString()]),
                };
                return srcProjectDetail;
            }
            return null;
        }

        public enum EnumEditSRCProjectDetailListResult
        {
            Success, Failure
        }

        public EnumEditSRCProjectDetailListResult EditSRCProjectDetailList(SRCProjectDetailPara para, List<SRCProjectDetailPara>domainGroupIDParaList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            string deleteCommand;
            deleteCommand = string.Concat(new object[]
                {
                    "       DELETE FROM SYS_SRC_PROJECT ", Environment.NewLine,
                    "       WHERE PROJECT_ID={PROJECT_ID} ", Environment.NewLine,

                    "       DELETE FROM SYS_SRC_DOMAIN_GROUP ", Environment.NewLine,
                    "       WHERE PROJECT_ID={PROJECT_ID}; ", Environment.NewLine,
                });

            dbParameters.Add(new DBParameter{Name = SRCProjectDetailPara.ParaField.PROJECT_ID.ToString(),Value = para.ProjectID});

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
            dbParameters.Clear();

            string insertSRCProjectCommand;
            insertSRCProjectCommand = string.Concat(new object[]
                {
                    "       INSERT INTO SYS_SRC_PROJECT(PROJECT_ID, PROJECT_NM_ZH_TW, PROJECT_NM_ZH_CN, PROJECT_NM_EN_US, PROJECT_NM_TH_TH, PROJECT_NM_JA_JP, PROJECT_NM_KO_KR, PROJECT_PARENT, IS_SVN, REMARK, UPD_USER_ID, UPD_DT) ", Environment.NewLine,
                    "       VALUES ({PROJECT_ID}, {PROJECT_NM_ZH_TW}, {PROJECT_NM_ZH_CN}, {PROJECT_NM_EN_US}, {PROJECT_NM_TH_TH}, {PROJECT_NM_JA_JP}, {PROJECT_NM_KO_KR}, {PROJECT_PARENT}, {IS_SVN}, {REMARK}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,
                });

            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_ID.ToString(), Value = para.ProjectID });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_NM_ZH_TW.ToString(), Value = para.ProjectNMZHTW });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_NM_ZH_CN.ToString(), Value = para.ProjectNMZHCN });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_NM_EN_US.ToString(), Value = para.ProjectNMENUS });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_NM_TH_TH.ToString(), Value = para.ProjectNMTHTH });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_NM_JA_JP.ToString(), Value = para.ProjectNMJAJP });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_NM_KO_KR.ToString(), Value = para.ProjectNMKOKR });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_PARENT.ToString(), Value = para.ProjectPraent });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.IS_SVN.ToString(), Value = para.IsSVN });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.REMARK.ToString(), Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertSRCProjectCommand,dbParameters));
            dbParameters.Clear();

            if (domainGroupIDParaList != null && domainGroupIDParaList.Count > 0)
            {
                foreach (SRCProjectDetailPara domainGroupIDPara in domainGroupIDParaList)
                {
                    string insertDomainGroupCommand;
                    insertDomainGroupCommand = string.Concat(new object[]
                        {
                            "       SELECT @DOMAIN_NAME = DOMAIN_NAME ", Environment.NewLine,
                            "       FROM dbo.SYS_DOMAIN_GROUP ", Environment.NewLine,
                            "       WHERE DOMAIN_GROUP_ID = {DOMAIN_GROUP_ID} ", Environment.NewLine,

                            "       INSERT INTO SYS_SRC_DOMAIN_GROUP(PROJECT_ID, DOMAIN_NAME, DOMAIN_GROUP_ID, IS_WRITE, UPD_USER_ID, UPD_DT)", Environment.NewLine,
                            "       VALUES ({PROJECT_ID}, @DOMAIN_NAME, {DOMAIN_GROUP_ID}, {IS_WRITE}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,
                        });

                    dbParameters.Add(new DBParameter{Name = SRCProjectDetailPara.ParaField.PROJECT_ID.ToString(),Value = domainGroupIDPara.ProjectID});
                    dbParameters.Add(new DBParameter{Name = SRCProjectDetailPara.ParaField.DOMAIN_GROUP_ID.ToString(),Value = domainGroupIDPara.DomainGroupID});
                    dbParameters.Add(new DBParameter{Name = SRCProjectDetailPara.ParaField.IS_WRITE.ToString(),Value = domainGroupIDPara.IsWrite});
                    dbParameters.Add(new DBParameter{Name = SRCProjectDetailPara.ParaField.UPD_USER_ID.ToString(),Value = domainGroupIDPara.UpdUserID});

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertDomainGroupCommand,dbParameters));
                    dbParameters.Clear();
                }
            }

            string commandText = string.Concat(new object[]
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "DECLARE @DOMAIN_NAME VARCHAR(20)= ''; ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,
                    commandTextStringBuilder.ToString(), Environment.NewLine,
                    "       SET @RESULT = 'Y'; ", Environment.NewLine,
                    "       COMMIT; ", Environment.NewLine,
                    "    END TRY ", Environment.NewLine,
                    "    BEGIN CATCH ", Environment.NewLine,
                    "       SET @RESULT = 'N'; ", Environment.NewLine,
                    "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "    END CATCH ", Environment.NewLine,
                    "; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString())? EnumEditSRCProjectDetailListResult.Success: EnumEditSRCProjectDetailListResult.Failure;
        }

        public enum EnumDeleteSRCProjectDetailListResult
        {
            Success, Failure
        }
        public EnumDeleteSRCProjectDetailListResult DeleteSRCProjectDetailList(SRCProjectDetailPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,

                    "       DELETE FROM SYS_SRC_DOMAIN_GROUP ", Environment.NewLine,
                    "       WHERE PROJECT_ID={PROJECT_ID} ; ", Environment.NewLine,

                    "       DELETE FROM SYS_SRC_PROJECT ", Environment.NewLine,
                    "       WHERE PROJECT_ID={PROJECT_ID}; ", Environment.NewLine,
                    
                    "       SET @RESULT = 'Y'; ", Environment.NewLine,
                    "       COMMIT; ", Environment.NewLine,
                    "    END TRY ", Environment.NewLine,
                    "    BEGIN CATCH ", Environment.NewLine,
                    "       SET @RESULT = 'N'; ", Environment.NewLine,
                    "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "    END CATCH ", Environment.NewLine,
                    "; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SRCProjectDetailPara.ParaField.PROJECT_ID.ToString(), Value = para.ProjectID });


            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteSRCProjectDetailListResult.Success : EnumDeleteSRCProjectDetailListResult.Failure;
        }
    }
}
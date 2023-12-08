using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityDomainGroupDetail : EntitySys
    {
        public EntityDomainGroupDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class DomainGroupDetailPara : DBCulture
        {
            public DomainGroupDetailPara(string cultureID)
                : base(cultureID)
            {
            
            }
            public enum ParaField
            {
                DOMAIN_NAME, DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM_ZH_TW,
                DOMAIN_GROUP_NM_ZH_CN,
                DOMAIN_GROUP_NM_EN_US,
                DOMAIN_GROUP_NM_TH_TH,
                DOMAIN_GROUP_NM_JA_JP,
                DOMAIN_GROUP_NM,
                UPD_USER_ID
            }

            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
            public DBNVarChar DomainGroupNMZHTW;
            public DBNVarChar DomainGroupNMZHCN;
            public DBNVarChar DomainGroupNMENUS;
            public DBNVarChar DomainGroupNMTHTH;
            public DBNVarChar DomainGroupNMJAJP;
            public DBVarChar UpdUserID;
        }

        public class DomainGroupDetail :DBTableRow
        {
            public enum DataField
            {
                DOMAIN_NAME, DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM_ZH_TW,
                DOMAIN_GROUP_NM_ZH_CN,
                DOMAIN_GROUP_NM_EN_US,
                DOMAIN_GROUP_NM_TH_TH,
                DOMAIN_GROUP_NM_JA_JP,
                UPD_USER_ID
            }

            public DBVarChar DomainName;
            public DBVarChar DomainGroupID;
            public DBNVarChar DomainGroupNMZHTW;
            public DBNVarChar DomainGroupNMZHCN;
            public DBNVarChar DomainGroupNMENUS;
            public DBNVarChar DomainGroupNMTHTH;
            public DBNVarChar DomainGroupNMJAJP;
            public DBVarChar UpdUserID;
            }

        public DomainGroupDetail SelectDomainGroupDetail(DomainGroupDetailPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "SELECT DOMAIN_GROUP_NM_ZH_TW", Environment.NewLine,
                    "	   , DOMAIN_GROUP_NM_ZH_CN", Environment.NewLine,
                    "	   , DOMAIN_GROUP_NM_EN_US", Environment.NewLine,
                    "	   , DOMAIN_GROUP_NM_TH_TH", Environment.NewLine,
                    "	   , DOMAIN_GROUP_NM_JA_JP", Environment.NewLine,
                    "	   , DOMAIN_GROUP_ID ", Environment.NewLine,
                    "FROM SYS_DOMAIN_GROUP " , Environment.NewLine,
                    "WHERE DOMAIN_NAME = {DOMAIN_NAME} AND DOMAIN_GROUP_ID = {DOMAIN_GROUP_ID} ", Environment.NewLine,
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_ID.ToString(), Value = para.DomainGroupID});
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_NAME.ToString(), Value = para.DomainName });
            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                DomainGroupDetail domainGroupDetail = new DomainGroupDetail()
                    {
                        DomainGroupNMZHTW = new DBNVarChar(dataRow[DomainGroupDetail.DataField.DOMAIN_GROUP_NM_ZH_TW.ToString()]),
                        DomainGroupNMZHCN = new DBNVarChar(dataRow[DomainGroupDetail.DataField.DOMAIN_GROUP_NM_ZH_CN.ToString()]),
                        DomainGroupNMENUS = new DBNVarChar(dataRow[DomainGroupDetail.DataField.DOMAIN_GROUP_NM_EN_US.ToString()]),
                        DomainGroupNMTHTH = new DBNVarChar(dataRow[DomainGroupDetail.DataField.DOMAIN_GROUP_NM_TH_TH.ToString()]),
                        DomainGroupNMJAJP = new DBNVarChar(dataRow[DomainGroupDetail.DataField.DOMAIN_GROUP_NM_JA_JP.ToString()]),                       
                        DomainGroupID = new DBVarChar(dataRow[DomainGroupDetail.DataField.DOMAIN_GROUP_ID.ToString()]),
                    };
                return domainGroupDetail;
            }
            return null;
        }

        public enum EnumEditDomainGroupDetailListResult
        {
            Success, Failure
        }
        public EnumEditDomainGroupDetailListResult EditDomainGroupDetailList(DomainGroupDetailPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,

                    "       DELETE FROM SYS_DOMAIN_GROUP WHERE DOMAIN_NAME={DOMAIN_NAME} AND DOMAIN_GROUP_ID={DOMAIN_GROUP_ID}; ", Environment.NewLine,

                    "       INSERT INTO SYS_DOMAIN_GROUP(DOMAIN_NAME, DOMAIN_GROUP_ID, DOMAIN_GROUP_NM_ZH_TW, DOMAIN_GROUP_NM_ZH_CN, DOMAIN_GROUP_NM_EN_US, DOMAIN_GROUP_NM_TH_TH, DOMAIN_GROUP_NM_JA_JP, UPD_USER_ID, UPD_DT)", Environment.NewLine,
                    "       VALUES ({DOMAIN_NAME}, {DOMAIN_GROUP_ID}, {DOMAIN_GROUP_NM_ZH_TW}, {DOMAIN_GROUP_NM_ZH_CN}, {DOMAIN_GROUP_NM_EN_US}, {DOMAIN_GROUP_NM_TH_TH}, {DOMAIN_GROUP_NM_JA_JP}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_NAME.ToString(), Value = para.DomainName });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_ID.ToString(), Value = para.DomainGroupID });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_NM_ZH_TW.ToString(), Value = para.DomainGroupNMZHTW });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_NM_ZH_CN.ToString(), Value = para.DomainGroupNMZHCN });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_NM_EN_US.ToString(), Value = para.DomainGroupNMENUS });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_NM_TH_TH.ToString(), Value = para.DomainGroupNMTHTH });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_NM_JA_JP.ToString(), Value = para.DomainGroupNMJAJP });
            dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });


            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditDomainGroupDetailListResult.Success : EnumEditDomainGroupDetailListResult.Failure;
        }

        public enum EnumDeleteDomainGroupDetailListResult
        {
            Success, Failure, DataExist
        }

        public EnumDeleteDomainGroupDetailListResult DeleteDomainGroupDetailList(DomainGroupDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[]
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,
                    "       IF     NOT EXISTS (SELECT * FROM SYS_DOMAIN_GROUP_ACCOUNT WHERE DOMAIN_NAME={DOMAIN_NAME} AND DOMAIN_GROUP_ID={DOMAIN_GROUP_ID}) ", Environment.NewLine,
                    "          AND NOT EXISTS (SELECT * FROM SYS_SRC_DOMAIN_GROUP WHERE DOMAIN_NAME={DOMAIN_NAME} AND DOMAIN_GROUP_ID={DOMAIN_GROUP_ID}) ", Environment.NewLine,
                    "       BEGIN ", Environment.NewLine,
                    "           DELETE FROM SYS_DOMAIN_GROUP WHERE DOMAIN_NAME={DOMAIN_NAME} AND DOMAIN_GROUP_ID={DOMAIN_GROUP_ID}; ", Environment.NewLine,

                    "           SET @RESULT = 'Y'; ", Environment.NewLine,
                    "           COMMIT; ", Environment.NewLine,
                    "       END ", Environment.NewLine,
                    "    END TRY ", Environment.NewLine,
                    "    BEGIN CATCH ", Environment.NewLine,
                    "       SET @RESULT = 'N'; ", Environment.NewLine,
                    "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "    END CATCH ", Environment.NewLine,
                    "; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_NAME.ToString(), Value = para.DomainName });
                dbParameters.Add(new DBParameter { Name = DomainGroupDetailPara.ParaField.DOMAIN_GROUP_ID.ToString(), Value = para.DomainGroupID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));

                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteDomainGroupDetailListResult.Success;
                }
                else 
                {
                    return EnumDeleteDomainGroupDetailListResult.DataExist;
                }
            }
            catch 
            {
                return EnumDeleteDomainGroupDetailListResult.Failure;
            }
        }
    }
}
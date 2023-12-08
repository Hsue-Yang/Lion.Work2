using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserDomainDetail : EntitySys
    {
        public EntityUserDomainDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserDomainDetailPara : DBCulture
        {
            public UserDomainDetailPara(string cultureID) : base(cultureID)
            {
                
            }

            public enum ParaField
            {
                CODE_NM, CODE_KIND,

                USER_ID,
                DOMAIN_GROUP_NM,
                DOMAIN_NAME, DOMAIN_ACCOUNT,
                DOMAIN_GROUP_ID,
                UPD_USER_ID, UPD_DT
            }

            public DBVarChar CodeKind;
            public DBVarChar UserID;
            public DBVarChar DomainName;
            public DBVarChar DomainAccount;
            public DBVarChar DomainGroupID;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
        }

        public class UserDomainDetail : DBTableRow
        {
            public enum DataField
            {
                USER_ID,
                DOMAIN_GROUP_NM,
                DOMAIN_NAME, DOMAIN_ACCOUNT,
                DOMAIN_GROUP_ID,
                CODE_ID, CODE_NM,
                HAS_DOMAIN_NAME,
                HAS_DOMAIN_GROUP_ID,
            }

            public DBVarChar UserID;
            public DBVarChar DomainName;
            public DBVarChar DomainAccount;
            public DBVarChar DomainGroupID;
            public DBNVarChar DomainGroupNM;
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;
            public DBChar HasDomainName;
            public DBChar HasDomainGroupID;
        }

        public List<UserDomainDetail> SelectDomainGroupList(UserDomainDetailPara para)
        {
            string commandText = string.Concat(new object[]{
                "SELECT G.DOMAIN_NAME ", Environment.NewLine,
                "       ,G.DOMAIN_GROUP_ID ", Environment.NewLine,
                "       ,A.DOMAIN_ACCOUNT ", Environment.NewLine,
                "       ,dbo.FN_GET_NMID(G.DOMAIN_GROUP_ID, G.{DOMAIN_GROUP_NM}) AS DOMAIN_GROUP_NM ", Environment.NewLine,
                "       ,(CASE WHEN A.DOMAIN_ACCOUNT IS NOT NULL THEN 'Y' ELSE 'N' END) AS HAS_DOMAIN_GROUP_ID ", Environment.NewLine,
                "FROM SYS_DOMAIN_GROUP G ", Environment.NewLine,
                "LEFT OUTER JOIN( ", Environment.NewLine,
                "       SELECT DOMAIN_ACCOUNT,DOMAIN_GROUP_ID,DOMAIN_NAME ", Environment.NewLine,
                "       FROM SYS_DOMAIN_GROUP_ACCOUNT ", Environment.NewLine,
                "       WHERE DOMAIN_ACCOUNT = {DOMAIN_ACCOUNT} ", Environment.NewLine,
                "       ) A ON G.DOMAIN_GROUP_ID = A.DOMAIN_GROUP_ID ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.DOMAIN_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserDomainDetailPara.ParaField.DOMAIN_GROUP_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.DOMAIN_ACCOUNT.ToString(), Value = para.DomainAccount });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserDomainDetail> domainNameList = new List<UserDomainDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserDomainDetail domainName = new UserDomainDetail()
                    {
                        DomainName = new DBVarChar(dataRow[UserDomainDetail.DataField.DOMAIN_NAME.ToString()]),
                        DomainGroupID = new DBVarChar(dataRow[UserDomainDetail.DataField.DOMAIN_GROUP_ID.ToString()]),
                        DomainAccount = new DBVarChar(dataRow[UserDomainDetail.DataField.DOMAIN_ACCOUNT.ToString()]),
                        DomainGroupNM = new DBNVarChar(dataRow[UserDomainDetail.DataField.DOMAIN_GROUP_NM.ToString()]),
                        HasDomainGroupID = new DBChar(dataRow[UserDomainDetail.DataField.HAS_DOMAIN_GROUP_ID.ToString()])
                    };
                    domainNameList.Add(domainName);
                }
                return domainNameList;
            }
            return null;
        }

        public enum EnumEditUserDomainDetailListResult
        {
            Success, Failure
        }

        public EnumEditUserDomainDetailListResult EditUserDomainDetailList(UserDomainDetailPara para, List<UserDomainDetailPara>domainNameParaList,List<UserDomainDetailPara>domainGroupIDParaList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            string beginIFCommand = string.Concat(new object[]
                {
                    "IF EXISTS (SELECT USER_ID FROM RAW_CM_USER WHERE USER_ID={USER_ID}) ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                });
            dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, beginIFCommand, dbParameters));
            dbParameters.Clear();

            string deleteCommand;
            deleteCommand = string.Concat(new object[]
                {
                    "       DELETE FROM SYS_USER_DOMAIN ", Environment.NewLine,
                    "       WHERE USER_ID={USER_ID} AND DOMAIN_ACCOUNT={DOMAIN_ACCOUNT}; ", Environment.NewLine,

                    "       DELETE FROM SYS_DOMAIN_GROUP_ACCOUNT ", Environment.NewLine,
                    "       WHERE DOMAIN_ACCOUNT={DOMAIN_ACCOUNT}; ", Environment.NewLine,
                });

            dbParameters.Add(new DBParameter{Name = UserDomainDetailPara.ParaField.USER_ID.ToString(),Value = para.UserID});
            dbParameters.Add(new DBParameter{Name = UserDomainDetailPara.ParaField.DOMAIN_ACCOUNT.ToString(),Value = para.DomainAccount});

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
            dbParameters.Clear();

            if (domainGroupIDParaList != null && domainGroupIDParaList.Count > 0)
            {
                foreach (UserDomainDetailPara domainGroupIDPara in domainGroupIDParaList)
                {
                    string insertDomainGroupCommand;
                    insertDomainGroupCommand = string.Concat(new object[]
                        {
                            "       SELECT @DOMAIN_NAME = DOMAIN_NAME ", Environment.NewLine,
                            "       FROM dbo.SYS_DOMAIN_GROUP ", Environment.NewLine,
                            "       WHERE DOMAIN_GROUP_ID = {DOMAIN_GROUP_ID} ", Environment.NewLine,

                            "       INSERT INTO SYS_DOMAIN_GROUP_ACCOUNT(DOMAIN_NAME, DOMAIN_ACCOUNT, DOMAIN_GROUP_ID, UPD_USER_ID, UPD_DT)", Environment.NewLine,
                            "       VALUES (@DOMAIN_NAME, {DOMAIN_ACCOUNT}, {DOMAIN_GROUP_ID}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,
                        });

                    dbParameters.Add(new DBParameter{Name = UserDomainDetailPara.ParaField.USER_ID.ToString(),Value = domainGroupIDPara.UserID});
                    dbParameters.Add(new DBParameter{Name = UserDomainDetailPara.ParaField.DOMAIN_GROUP_ID.ToString(),Value = domainGroupIDPara.DomainGroupID});
                    dbParameters.Add(new DBParameter{Name = UserDomainDetailPara.ParaField.DOMAIN_ACCOUNT.ToString(),Value = domainGroupIDPara.DomainAccount});
                    dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.UPD_USER_ID.ToString(), Value = domainGroupIDPara.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertDomainGroupCommand, dbParameters));
                    dbParameters.Clear();
                }
            }

            if (domainNameParaList != null && domainNameParaList.Count > 0)
            {
                foreach (UserDomainDetailPara domainNamePara in domainNameParaList)
                {
                    string insertDomainNameCommand;
                    insertDomainNameCommand = string.Concat(new object[]
                        {
                            "IF EXISTS (SELECT DOMAIN_NAME ", Environment.NewLine,
                            "                       FROM SYS_DOMAIN_GROUP_ACCOUNT ", Environment.NewLine,
                            "                       WHERE DOMAIN_ACCOUNT = {DOMAIN_ACCOUNT} AND DOMAIN_NAME = {DOMAIN_NAME}) ", Environment.NewLine,
                            "BEGIN ", Environment.NewLine,
                            "       INSERT INTO SYS_USER_DOMAIN(USER_ID, DOMAIN_NAME, DOMAIN_ACCOUNT, UPD_USER_ID, UPD_DT) ", Environment.NewLine,
                            "       VALUES ({USER_ID}, {DOMAIN_NAME}, {DOMAIN_ACCOUNT}, {UPD_USER_ID}, GETDATE()); ",Environment.NewLine,
                            "END; ",Environment.NewLine,

                        });

                    dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.USER_ID.ToString(), Value = domainNamePara.UserID });
                    dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.DOMAIN_NAME.ToString(), Value = domainNamePara.DomainName });
                    dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.DOMAIN_ACCOUNT.ToString(), Value = domainNamePara.DomainAccount });
                    dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.UPD_USER_ID.ToString(), Value = domainNamePara.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertDomainNameCommand, dbParameters));
                    dbParameters.Clear();
                }
            }


            string endIFCommand = string.Concat(new object[]
                {
                    "       SET @RESULT = 'Y'; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "ELSE ", Environment.NewLine,
                        "BEGIN ", Environment.NewLine,
                        "       SET @RESULT='N' ", Environment.NewLine,
                        "END; ", Environment.NewLine,
                });
            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, endIFCommand, dbParameters));
            dbParameters.Clear();

            string commandText = string.Concat(new object[]
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "DECLARE @DOMAIN_NAME VARCHAR(20)= ''; ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,
                    commandTextStringBuilder.ToString(), Environment.NewLine,
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
            return (result.GetValue() == EnumYN.Y.ToString())? EnumEditUserDomainDetailListResult.Success: EnumEditUserDomainDetailListResult.Failure;
        }

        public enum EnumDeleteUserDomainDetailListResult
        {
            Success, Failure
        }

        public EnumDeleteUserDomainDetailListResult DeleteUserDomainDetailList(UserDomainDetailPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,
                    
                    "       DELETE FROM SYS_USER_DOMAIN ", Environment.NewLine,
                    "       WHERE USER_ID={USER_ID} AND DOMAIN_ACCOUNT={DOMAIN_ACCOUNT}; ", Environment.NewLine,

                    "       DELETE FROM SYS_DOMAIN_GROUP_ACCOUNT ", Environment.NewLine,
                    "       WHERE DOMAIN_ACCOUNT={DOMAIN_ACCOUNT}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserDomainDetailPara.ParaField.DOMAIN_ACCOUNT.ToString(), Value = para.DomainAccount });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteUserDomainDetailListResult.Success : EnumDeleteUserDomainDetailListResult.Failure;
        }
    }
}
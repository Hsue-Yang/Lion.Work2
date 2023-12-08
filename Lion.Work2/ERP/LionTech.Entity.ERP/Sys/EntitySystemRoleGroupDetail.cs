using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRoleGroupDetail : EntitySys
    {
        public EntitySystemRoleGroupDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleGroupDetailPara
        {
            public enum Field
            {
                ROLE_GROUP_ID,

                ROLE_GROUP_NM_ZH_TW,
                ROLE_GROUP_NM_ZH_CN,
                ROLE_GROUP_NM_EN_US,
                ROLE_GROUP_NM_TH_TH,
                ROLE_GROUP_NM_JA_JP,
                ROLE_GROUP_NM_KO_KR,

                SORT_ORDER, REMARK,

                UPD_USER_ID, UPD_DT
            }

            public DBVarChar RoleGroupID;

            public DBNVarChar RoleGroupNMZhTW;
            public DBNVarChar RoleGroupNMZhCN;
            public DBNVarChar RoleGroupNMEnUS;
            public DBNVarChar RoleGroupNMThTH;
            public DBNVarChar RoleGroupNMJaJP;
            public DBNVarChar RoleGroupNMKoKR;

            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public DBVarChar UpdUserID;
        }

        public class SystemRoleGroupDetail : DBTableRow
        {
            public enum DataField
            {
                ROLE_GROUP_ID,

                ROLE_GROUP_NM_ZH_TW,
                ROLE_GROUP_NM_ZH_CN,
                ROLE_GROUP_NM_EN_US,
                ROLE_GROUP_NM_TH_TH,
                ROLE_GROUP_NM_JA_JP,
                ROLE_GROUP_NM_KO_KR,

                SORT_ORDER, REMARK,

                UPD_USER_ID, UPD_DT
            }

            public DBVarChar RoleGroupID;

            public DBNVarChar RoleGroupNMZhTW;
            public DBNVarChar RoleGroupNMZhCN;
            public DBNVarChar RoleGroupNMEnUS;
            public DBNVarChar RoleGroupNMThTH;
            public DBNVarChar RoleGroupNMJaJP;
            public DBNVarChar RoleGroupNMKoKR;

            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public DBVarChar UpdUserID;
        }

        public SystemRoleGroupDetail SelectSystemRoleGroupDetail(SystemRoleGroupDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT G.ROLE_GROUP_ID ", Environment.NewLine,
                "     , G.ROLE_GROUP_NM_ZH_TW ", Environment.NewLine,
                "     , G.ROLE_GROUP_NM_ZH_CN ", Environment.NewLine,
                "     , G.ROLE_GROUP_NM_EN_US ", Environment.NewLine,
                "     , G.ROLE_GROUP_NM_TH_TH ", Environment.NewLine,
                "     , G.ROLE_GROUP_NM_JA_JP ", Environment.NewLine,
                "     , G.ROLE_GROUP_NM_KO_KR ", Environment.NewLine,
                "     , G.SORT_ORDER, G.REMARK ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(G.UPD_USER_ID) AS UPD_USER_NM, G.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_GROUP G ", Environment.NewLine,
                "WHERE ROLE_GROUP_ID={ROLE_GROUP_ID} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_ID, Value = para.RoleGroupID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemRoleGroupDetail systemRoleGroup = new SystemRoleGroupDetail()
                {
                    RoleGroupID = new DBVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_ID.ToString()]),
                    RoleGroupNMZhTW = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_NM_ZH_TW.ToString()]),
                    RoleGroupNMZhCN = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_NM_ZH_CN.ToString()]),
                    RoleGroupNMEnUS = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_NM_EN_US.ToString()]),
                    RoleGroupNMThTH = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_NM_TH_TH.ToString()]),
                    RoleGroupNMJaJP = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_NM_JA_JP.ToString()]),
                    RoleGroupNMKoKR = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.ROLE_GROUP_NM_KO_KR.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemRoleGroupDetail.DataField.SORT_ORDER.ToString()]),
                    Remark = new DBNVarChar(dataRow[SystemRoleGroupDetail.DataField.REMARK.ToString()]),
                };
                return systemRoleGroup;
            }
            return null;
        }

        public enum EnumEditSystemRoleGroupDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemRoleGroupDetailResult EditSystemRoleGroupDetail(SystemRoleGroupDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_ROLE_GROUP ", Environment.NewLine,
                "        WHERE ROLE_GROUP_ID={ROLE_GROUP_ID} ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_ROLE_GROUP VALUES ( ", Environment.NewLine,
                "            {ROLE_GROUP_ID} ", Environment.NewLine,
                "          , {ROLE_GROUP_NM_ZH_TW} ", Environment.NewLine,
                "          , {ROLE_GROUP_NM_ZH_CN} ", Environment.NewLine,
                "          , {ROLE_GROUP_NM_EN_US} ", Environment.NewLine,
                "          , {ROLE_GROUP_NM_TH_TH} ", Environment.NewLine,
                "          , {ROLE_GROUP_NM_JA_JP} ", Environment.NewLine,
                "          , {ROLE_GROUP_NM_KO_KR} ", Environment.NewLine,
                "          , {SORT_ORDER}, {REMARK} ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_ID, Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_NM_ZH_TW, Value = para.RoleGroupNMZhTW });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_NM_ZH_CN, Value = para.RoleGroupNMZhCN });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_NM_EN_US, Value = para.RoleGroupNMEnUS });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_NM_TH_TH, Value = para.RoleGroupNMThTH });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_NM_JA_JP, Value = para.RoleGroupNMJaJP });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_NM_KO_KR, Value = para.RoleGroupNMKoKR });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.REMARK, Value = para.Remark });
            
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemRoleGroupDetailResult.Success : EnumEditSystemRoleGroupDetailResult.Failure;
        }

        public enum EnumDeleteSystemRoleGroupDetailResult //刪除
        {
            Success, Failure, DataExist
        }

        public EnumDeleteSystemRoleGroupDetailResult DeleteSystemRoleGroupDetail(SystemRoleGroupDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,
                    "        IF NOT EXISTS (SELECT USER_ID FROM SYS_USER_MAIN WHERE ROLE_GROUP_ID={ROLE_GROUP_ID}) ", Environment.NewLine,
                    "        BEGIN ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_ROLE_GROUP WHERE ROLE_GROUP_ID={ROLE_GROUP_ID}; ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_ROLE_GROUP_COLLECT WHERE ROLE_GROUP_ID={ROLE_GROUP_ID}; ", Environment.NewLine,
                    "            SET @RESULT = 'Y'; ", Environment.NewLine,
                    "        END; ", Environment.NewLine,
                    "        COMMIT; ", Environment.NewLine,
                    "    END TRY ", Environment.NewLine,
                    "    BEGIN CATCH ", Environment.NewLine,
                    "        SET @RESULT = 'N'; ", Environment.NewLine,
                    "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "    END CATCH ", Environment.NewLine,
                    "; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemRoleGroupDetailPara.Field.ROLE_GROUP_ID, Value = para.RoleGroupID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemRoleGroupDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemRoleGroupDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemRoleGroupDetailResult.Failure;
            }
        }
    }
}
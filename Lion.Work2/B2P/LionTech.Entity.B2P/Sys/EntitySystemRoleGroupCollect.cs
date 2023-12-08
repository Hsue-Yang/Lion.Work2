using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemRoleGroupCollect : EntitySys
    {
        public EntitySystemRoleGroupCollect(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SysSystemRoleGroupPara : DBCulture
        {
            public SysSystemRoleGroupPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                ROLE_GROUP_ID, ROLE_GROUP_NM
            }

            public DBVarChar RoleGroupID;
        }

        public class SysSystemRoleGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                ROLE_GROUP_ID, ROLE_GROUP_NM,
                SORT_ORDER, REMARK
            }

            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public string ItemText()
            {
                return this.RoleGroupNM.StringValue();
            }

            public string ItemValue()
            {
                return this.RoleGroupID.StringValue();
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

        public SysSystemRoleGroup SelectSysSystemRoleGroup(SysSystemRoleGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ROLE_GROUP_ID, dbo.FN_GET_NMID(ROLE_GROUP_ID, {ROLE_GROUP_NM}) AS ROLE_GROUP_NM ", Environment.NewLine,
                "     , SORT_ORDER, REMARK ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_GROUP ", Environment.NewLine,
                "WHERE ROLE_GROUP_ID={ROLE_GROUP_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SysSystemRoleGroup sysSystemRoleGroup = new SysSystemRoleGroup()
                {
                    RoleGroupID = new DBVarChar(dataTable.Rows[0][SysSystemRoleGroup.DataField.ROLE_GROUP_ID.ToString()]),
                    RoleGroupNM = new DBNVarChar(dataTable.Rows[0][SysSystemRoleGroup.DataField.ROLE_GROUP_NM.ToString()]),
                    SortOrder = new DBVarChar(dataTable.Rows[0][SysSystemRoleGroup.DataField.SORT_ORDER.ToString()]),
                    Remark = new DBNVarChar(dataTable.Rows[0][SysSystemRoleGroup.DataField.REMARK.ToString()])
                };
                return sysSystemRoleGroup;
            }
            return null;
        }

        public class SysSystemRoleGroupCollectPara : DBCulture
        {
            public SysSystemRoleGroupCollectPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                ROLE_GROUP_ID,
                SYS_ID, SYS_NM, ROLE_ID, ROLE_NM,
                UPD_USER_ID
            }

            public DBVarChar RoleGroupID;
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
        }

        public class SysSystemRoleGroupCollect : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, ROLE_ID, ROLE_NM, HAS_ROLE
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
        }

        public List<SysSystemRoleGroupCollect> SelectSysSystemRoleGroupCollectList(SysSystemRoleGroupCollectPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT R.SYS_ID, dbo.FN_GET_NMID(R.SYS_ID, S.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , R.ROLE_ID, dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS ROLE_NM ", Environment.NewLine,
                "     , (CASE WHEN C.SYS_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HAS_ROLE ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE R ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN S ON R.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "LEFT OUTER JOIN (SELECT SYS_ID, ROLE_ID ", Environment.NewLine,
                "                 FROM SYS_SYSTEM_ROLE_GROUP_COLLECT ", Environment.NewLine,
                "                 WHERE ROLE_GROUP_ID={ROLE_GROUP_ID}) C ", Environment.NewLine,
                "ON R.SYS_ID=C.SYS_ID AND R.ROLE_ID=C.ROLE_ID ", Environment.NewLine,
                "ORDER BY S.SORT_ORDER, R.ROLE_ID "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleGroupCollectPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleGroupCollectPara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleGroupCollect> sysSystemRoleGroupCollectList = new List<SysSystemRoleGroupCollect>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleGroupCollect sysSystemRoleGroupCollect = new SysSystemRoleGroupCollect()
                    {
                        SysID = new DBVarChar(dataRow[SysSystemRoleGroupCollect.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SysSystemRoleGroupCollect.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[SysSystemRoleGroupCollect.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[SysSystemRoleGroupCollect.DataField.ROLE_NM.ToString()]),
                        HasRole = new DBChar(dataRow[SysSystemRoleGroupCollect.DataField.HAS_ROLE.ToString()])
                    };
                    sysSystemRoleGroupCollectList.Add(sysSystemRoleGroupCollect);
                }
                return sysSystemRoleGroupCollectList;
            }
            return null;
        }

        public enum EnumEditUserSystemRoleResult
        {
            Success, Failure
        }

        public EnumEditUserSystemRoleResult EditSysSystemRoleGroupCollect(SysSystemRoleGroupCollectPara para, List<SysSystemRoleGroupCollectPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            if (paraList == null || paraList.Count == 0)
            {
                string deleteCommand = string.Concat(new object[]
                {
                    "        DELETE FROM SYS_SYSTEM_ROLE_GROUP_COLLECT WHERE ROLE_GROUP_ID={ROLE_GROUP_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();
            }
            else
            {
                string deleteCommand = string.Concat(new object[]
                {
                    "        DELETE FROM SYS_SYSTEM_ROLE_GROUP_COLLECT WHERE ROLE_GROUP_ID={ROLE_GROUP_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();

                foreach (SysSystemRoleGroupCollectPara sysSystemRoleGroupCollectPara in paraList)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        "        INSERT INTO SYS_SYSTEM_ROLE_GROUP_COLLECT VALUES ({ROLE_GROUP_ID}, {SYS_ID}, {ROLE_ID}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine
                    });

                    dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });
                    dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.SYS_ID, Value = sysSystemRoleGroupCollectPara.SysID });
                    dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_ID, Value = sysSystemRoleGroupCollectPara.RoleID });
                    dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                    dbParameters.Clear();
                }
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(), Environment.NewLine,
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

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserSystemRoleResult.Success : EnumEditUserSystemRoleResult.Failure;
        }
    }
}
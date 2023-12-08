using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemServiceList : EntitySys
    {
        public EntitySystemServiceList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {

        }

        public class SystemServiceListPara : DBCulture
        {
            public SystemServiceListPara(string cultureID)
                : base(cultureID)
            {

            }
            
            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                SERVICE_ID,
                REMARK, 
                UPD_USER_ID,
                CODE_NM
                
            }

            public DBVarChar SysID;
            public DBVarChar ServiceID;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class SystemServiceList : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                SERVICE_ID, SERVICE_NM, 
                REMARK,
                UPD_USER_ID, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar ServiceID;
            public DBNVarChar ServiceNM;
            public DBNVarChar Remark;
            public DBNVarChar UpdUesrNM;
            public DBDateTime UpdDT;
        }

        public List<SystemServiceList> SelectSystemServiceList(SystemServiceListPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT S.SYS_ID ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(S.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , S.SERVICE_ID ", Environment.NewLine,
                "     , (CASE WHEN S.SERVICE_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(S.SERVICE_ID, C.{CODE_NM}) END) AS SERVICE_NM ", Environment.NewLine,
                "     , S.REMARK ", Environment.NewLine,
                "     , S.UPD_USER_ID ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UPD_USER_NM ", Environment.NewLine,
                "     , S.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_SERVICE S", Environment.NewLine,
                "INNER JOIN SYS_SYSTEM_MAIN M ON S.SYS_ID = M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN CM_CODE C ON C.CODE_KIND='0001' AND S.SERVICE_ID=C.CODE_ID ", Environment.NewLine,
                "WHERE S.SYS_ID={SYS_ID} ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.SYS_ID.ToString(), Value = para.SysID});
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemServiceListPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemServiceListPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemServiceList> sysServiceList = new List<SystemServiceList>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemServiceList sysService = new SystemServiceList()
                    {
                        SysID = new DBVarChar(dataRow[SystemServiceList.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemServiceList.DataField.SYS_NM.ToString()]),
                        ServiceID = new DBVarChar(dataRow[SystemServiceList.DataField.SERVICE_ID.ToString()]),
                        ServiceNM = new DBNVarChar(dataRow[SystemServiceList.DataField.SERVICE_NM.ToString()]),
                        Remark = new DBNVarChar(dataRow[SystemServiceList.DataField.REMARK.ToString()]),
                        UpdUesrNM = new DBNVarChar(dataRow[SystemServiceList.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemServiceList.DataField.UPD_DT.ToString()])
                    };
                    sysServiceList.Add(sysService);
                }
                return sysServiceList;
            }
            return null;
        }

        public enum EnumEditSystemServiceResult
        {
            Success, Failure
        }

        public EnumEditSystemServiceResult EditSystemService(SystemServiceListPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "   BEGIN TRY ", Environment.NewLine,
                
                "        DELETE FROM SYS_SYSTEM_SERVICE WHERE SYS_ID={SYS_ID} AND SERVICE_ID ={SERVICE_ID}; ", Environment.NewLine,
                
                "        INSERT INTO SYS_SYSTEM_SERVICE (SYS_ID, SERVICE_ID, REMARK, UPD_USER_ID, UPD_DT) ", Environment.NewLine,
                "        VALUES ({SYS_ID}, {SERVICE_ID}, {REMARK}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,
                
                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "       SET @RESULT = 'N'; ", Environment.NewLine,
                "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT;", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.SERVICE_ID, Value = para.ServiceID });
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemServiceResult.Success : EnumEditSystemServiceResult.Failure;
        }

        public enum EnumDeleteSystemServiceResult
        {
            Success, Failure
        }

        public EnumDeleteSystemServiceResult DeleteSystemService(SystemServiceListPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "   BEGIN TRY ", Environment.NewLine,
                
                "        DELETE FROM SYS_SYSTEM_SERVICE WHERE SYS_ID={SYS_ID} AND SERVICE_ID ={SERVICE_ID}; ", Environment.NewLine,
                
                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "       SET @RESULT = 'N'; ", Environment.NewLine,
                "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT;", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemServiceListPara.ParaField.SERVICE_ID, Value = para.ServiceID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteSystemServiceResult.Success : EnumDeleteSystemServiceResult.Failure;
        }
    }
}

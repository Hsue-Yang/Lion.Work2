using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemIPList : EntitySys
    {
        public EntitySystemIPList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemIPListPara
        {
            public enum ParaField
            {
                SYS_ID, IP_ADDRESS,
                IS_AP_SERVER, IS_API_SERVER, IS_DB_SERVER, IS_FILE_SERVER,
                FOLDER_PATH, REMARK,
                UPD_USER_ID, UPD_DT
            }

            public DBVarChar SysID;
            public DBVarChar IPAddress;
            public DBChar IsAPServer;
            public DBChar IsAPIServer;
            public DBChar IsDBServer;
            public DBChar IsFileServer;
            public DBNVarChar FolderPath;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
        }

        public class SystemIPList : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, IP_ADDRESS,
                IS_AP_SERVER, IS_API_SERVER, IS_DB_SERVER, IS_FILE_SERVER, 
                FOLDER_PATH, REMARK,
                UPD_USER_ID, UPD_DT, UPD_USER_NM
            }

            public DBVarChar SysID;
            public DBVarChar IPAddress;
            public DBChar IsAPServer;
            public DBChar IsAPIServer;
            public DBChar IsDBServer;
            public DBChar IsFileServer;
            public DBNVarChar FolderPath;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemIPList> SelectSystemIPList(SystemIPListPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, IP_ADDRESS ", Environment.NewLine,
                "     , IS_AP_SERVER, IS_API_SERVER, IS_DB_SERVER, IS_FILE_SERVER, FOLDER_PATH, REMARK ", Environment.NewLine,
                "     , UPD_USER_ID, dbo.FN_GET_USER_NM(UPD_USER_ID) AS UPD_USER_NM, UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_IP WHERE SYS_ID={SYS_ID} "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemIPList> sysIPLists = new List<SystemIPList>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemIPList sysIPList = new SystemIPList()
                    {
                        SysID = new DBVarChar(dataRow[SystemIPList.DataField.SYS_ID.ToString()]),
                        IPAddress = new DBVarChar(dataRow[SystemIPList.DataField.IP_ADDRESS.ToString()]),
                        IsAPServer = new DBChar(dataRow[SystemIPList.DataField.IS_AP_SERVER.ToString()]),
                        IsAPIServer = new DBChar(dataRow[SystemIPList.DataField.IS_API_SERVER.ToString()]),
                        IsDBServer = new DBChar(dataRow[SystemIPList.DataField.IS_DB_SERVER.ToString()]),
                        IsFileServer = new DBChar(dataRow[SystemIPList.DataField.IS_FILE_SERVER.ToString()]),
                        FolderPath = new DBNVarChar(dataRow[SystemIPList.DataField.FOLDER_PATH.ToString()]),
                        Remark = new DBNVarChar(dataRow[SystemIPList.DataField.REMARK.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[SystemIPList.DataField.UPD_USER_ID.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[SystemIPList.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemIPList.DataField.UPD_DT.ToString()])
                    };
                    sysIPLists.Add(sysIPList);
                }
                return sysIPLists;
            }
            return null;
        }

        public enum EnumInsertSystemIPResult
        {
            Success, Failure
        }

        public EnumInsertSystemIPResult InsertSystemIP(SystemIPListPara para)
        {
            string commandText = string.Concat(new object[] 
            { 
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "       DELETE FROM SYS_SYSTEM_IP WHERE SYS_ID={SYS_ID} AND IP_ADDRESS={IP_ADDRESS}; ", Environment.NewLine,

                "       INSERT INTO SYS_SYSTEM_IP(SYS_ID, IP_ADDRESS, IS_AP_SERVER, IS_API_SERVER, IS_DB_SERVER, IS_FILE_SERVER, FOLDER_PATH, REMARK, UPD_USER_ID, UPD_DT)", Environment.NewLine,
                "       VALUES ({SYS_ID}, {IP_ADDRESS}, {IS_AP_SERVER}, {IS_API_SERVER}, {IS_DB_SERVER}, {IS_FILE_SERVER}, {FOLDER_PATH}, {REMARK}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IP_ADDRESS, Value = para.IPAddress });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_AP_SERVER, Value = para.IsAPServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_API_SERVER, Value = para.IsAPIServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_DB_SERVER, Value = para.IsDBServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_FILE_SERVER, Value = para.IsFileServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.FOLDER_PATH, Value = para.FolderPath });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumInsertSystemIPResult.Success : EnumInsertSystemIPResult.Failure;
        }

        public enum EnumUpdateSystemIPResult
        {
            Success, Failure
        }

        public EnumUpdateSystemIPResult UpdateSystemIP(SystemIPListPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_SYSTEM_IP SET ", Environment.NewLine,
                "               IS_AP_SERVER={IS_AP_SERVER} ", Environment.NewLine,
                "             , IS_API_SERVER={IS_API_SERVER} ", Environment.NewLine,
                "             , IS_DB_SERVER={IS_DB_SERVER} ", Environment.NewLine,
                "             , IS_FILE_SERVER={IS_FILE_SERVER} ", Environment.NewLine,
                "             , FOLDER_PATH={FOLDER_PATH} ", Environment.NewLine,
                "             , REMARK={REMARK} ", Environment.NewLine,
                "             , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "             , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND IP_ADDRESS={IP_ADDRESS}; ", Environment.NewLine,

                "       SET @RESULT = 'Y'; ", Environment.NewLine,
                "       COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "       SET @RESULT = 'N'; ", Environment.NewLine,
                "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IP_ADDRESS, Value = para.IPAddress });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_AP_SERVER, Value = para.IsAPServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_API_SERVER, Value = para.IsAPIServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_DB_SERVER, Value = para.IsDBServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IS_FILE_SERVER, Value = para.IsFileServer });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.FOLDER_PATH, Value = para.FolderPath });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumUpdateSystemIPResult.Success : EnumUpdateSystemIPResult.Failure;
        }

        public enum EnumDeleteSystemIPResult
        {
            Success, Failure
        }

        public EnumDeleteSystemIPResult DeleteSystemIP(SystemIPListPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE SYS_SYSTEM_IP WHERE SYS_ID={SYS_ID} AND IP_ADDRESS={IP_ADDRESS}; ", Environment.NewLine,

                "       SET @RESULT = 'Y'; ", Environment.NewLine,
                "       COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "       SET @RESULT = 'N'; ", Environment.NewLine,
                "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemIPListPara.ParaField.IP_ADDRESS, Value = para.IPAddress });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteSystemIPResult.Success : EnumDeleteSystemIPResult.Failure;
        }
    }
}
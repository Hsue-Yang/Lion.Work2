using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LionTech.Entity.EDI;

namespace LionTech.Entity.ERP.EDIService
{
    public class EntityERPExternal : EntityEDIService
    {
        public EntityERPExternal(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemEventTargetPara
        {
            public enum ParaField
            {
                INSERT_EDI_NO
            }

            public DBChar InsertEDINo;
        }

        public class SystemEventTarget : DBTableRow
        {
            public enum DataField
            {
                EDI_EVENT_NO, SYS_ID, EVENT_GROUP_ID, EVENT_ID, EXEC_EDI_EVENT_NO,
                TARGET_SYS_ID, TARGET_PATH
            }

            public DBChar EDIEventNo;
            public DBVarChar SysID;
            public DBVarChar EventGroupID;
            public DBVarChar EventID;
            public DBChar ExecEDIEventNo;
            public DBVarChar TargetSysID;
            public DBNVarChar TargetPath;

            public override string ToString()
            {
                return string.Concat(new object[]
                    {
                        "EDIEventNo: ", (EDIEventNo == null ? string.Empty : EDIEventNo.GetValue()),
                        " ; SysID: ", (SysID == null ? string.Empty : SysID.GetValue()),
                        " ; EventGroupID: ", (EventGroupID == null ? string.Empty : EventGroupID.GetValue()),
                        " ; EventID: ", (EventID == null ? string.Empty : EventID.GetValue()),
                        " ; ExecEDIEventNo: ", (ExecEDIEventNo == null ? string.Empty : ExecEDIEventNo.GetValue()),
                        " ; TargetSysID: ", (TargetSysID == null ? string.Empty : TargetSysID.GetValue()),
                        " ; TargetPath: ", (TargetPath == null ? string.Empty : TargetPath.GetValue())
                    });
            }
        }

        public List<SystemEventTarget> SelectSystemEventTargetList(SystemEventTargetPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT D.EDI_EVENT_NO, D.SYS_ID, D.EVENT_GROUP_ID, D.EVENT_ID, D.EXEC_EDI_EVENT_NO ", Environment.NewLine,
                "     , T.TARGET_SYS_ID, T.TARGET_PATH ", Environment.NewLine,
                "FROM EDI_EVENT D ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_EVENT E ON D.SYS_ID=E.SYS_ID AND D.EVENT_GROUP_ID=E.EVENT_GROUP_ID AND D.EVENT_ID=E.EVENT_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_EVENT_TARGET T ON E.SYS_ID=T.SYS_ID AND E.EVENT_GROUP_ID=T.EVENT_GROUP_ID AND E.EVENT_ID=T.EVENT_ID ", Environment.NewLine,
                "WHERE D.INSERT_EDI_NO={INSERT_EDI_NO} AND D.STATUS_ID='W' ", Environment.NewLine,
                "  AND E.IS_DISABLE='N' ", Environment.NewLine,
                "ORDER BY D.EDI_EVENT_NO, D.SYS_ID, D.EVENT_GROUP_ID, D.EVENT_ID, T.TARGET_SYS_ID; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEventTargetPara.ParaField.INSERT_EDI_NO.ToString(), Value = para.InsertEDINo });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEventTarget> systemEventTargetList = new List<SystemEventTarget>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEventTarget systemEventTarget = new SystemEventTarget()
                    {
                        EDIEventNo = new DBChar(dataRow[SystemEventTarget.DataField.EDI_EVENT_NO.ToString()]),
                        SysID = new DBVarChar(dataRow[SystemEventTarget.DataField.SYS_ID.ToString()]),
                        EventGroupID = new DBVarChar(dataRow[SystemEventTarget.DataField.EVENT_GROUP_ID.ToString()]),
                        EventID = new DBVarChar(dataRow[SystemEventTarget.DataField.EVENT_ID.ToString()]),
                        ExecEDIEventNo = new DBChar(dataRow[SystemEventTarget.DataField.EXEC_EDI_EVENT_NO.ToString()]),
                        TargetSysID = new DBVarChar(dataRow[SystemEventTarget.DataField.TARGET_SYS_ID.ToString()]),
                        TargetPath = new DBNVarChar(dataRow[SystemEventTarget.DataField.TARGET_PATH.ToString()])
                    };
                    systemEventTargetList.Add(systemEventTarget);
                }
                return systemEventTargetList;
            }
            return null;
        }

        public class EDIEventPara
        {
            public enum ParaField
            {
                EDI_EVENT_NO, RESULT_ID, UPD_USER_ID,
                STATUS_ID, DT_BEGIN, DT_END
            }

            public DBChar EDIEventNo;
            public bool Result;
            public DBVarChar UpdUserID;
        }

        public void UpdateEDIEventBeginData(EDIEventPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "UPDATE EDI_EVENT SET ", Environment.NewLine,
                "    STATUS_ID={STATUS_ID}, DT_BEGIN={DT_BEGIN} ", Environment.NewLine,
                "  , UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                "WHERE EDI_EVENT_NO={EDI_EVENT_NO}", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.STATUS_ID, Value = new DBVarChar(EnumStatusID.B) });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.DT_BEGIN, Value = new DBDateTime(DateTime.Now) });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.EDI_EVENT_NO, Value = para.EDIEventNo });

            base.ExecuteNonQuery(commandText, dbParameters);
        }

        public void UpdateEDIEventEndData(EDIEventPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "UPDATE EDI_EVENT SET ", Environment.NewLine,
                "    STATUS_ID={STATUS_ID}, RESULT_ID={RESULT_ID} ", Environment.NewLine,
                "  , RESULT_CODE=(CASE WHEN {RESULT_ID}='F' AND RESULT_CODE IS NULL THEN 'Z9999' ", Environment.NewLine,
                "                      WHEN {RESULT_ID}='C' AND RESULT_CODE IS NULL THEN 'C0000' ", Environment.NewLine,
                "                      ELSE RESULT_CODE ", Environment.NewLine,
                "                 END) ", Environment.NewLine,
                "  , DT_END={DT_END} ", Environment.NewLine,
                "  , UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                "WHERE EDI_EVENT_NO={EDI_EVENT_NO}", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.STATUS_ID, Value = new DBVarChar(EnumStatusID.F) });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.RESULT_ID, Value = new DBVarChar(para.Result ? EnumResultID.S : EnumResultID.F) });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.DT_END, Value = new DBDateTime(DateTime.Now) });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = EDIEventPara.ParaField.EDI_EVENT_NO, Value = para.EDIEventNo });

            base.ExecuteNonQuery(commandText, dbParameters);
        }

        public class EDIEventTargetPara
        {
            public enum ParaField
            {
                EDI_EVENT_NO, TARGET_SYS_ID, RESULT_ID, UPD_USER_ID,
                STATUS_ID, DT_BEGIN, DT_END, RETURN_API_NO
            }

            public DBChar EDIEventNo;
            public DBVarChar TargetSysID;
            public bool Result;
            public DBChar ReturnAPINo;
            public DBVarChar UpdUserID;
        }

        public void InsertNewEDIEventTarget(EDIEventTargetPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "INSERT INTO EDI_EVENT_TARGET VALUES ( ", Environment.NewLine,
                "    {EDI_EVENT_NO}, {TARGET_SYS_ID} ", Environment.NewLine,
                "  , {STATUS_ID}, NULL, NULL, {DT_BEGIN}, NULL ", Environment.NewLine,
                "  , NULL ", Environment.NewLine,
                "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "); ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.EDI_EVENT_NO, Value = para.EDIEventNo });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.TARGET_SYS_ID, Value = para.TargetSysID });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.STATUS_ID, Value = new DBVarChar(EnumStatusID.B) });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.DT_BEGIN, Value = new DBDateTime(DateTime.Now) });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            base.ExecuteNonQuery(commandText, dbParameters);
        }

        public void UpdateEDIEventTargetEndData(EDIEventTargetPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "UPDATE EDI_EVENT_TARGET SET ", Environment.NewLine,
                "    STATUS_ID={STATUS_ID}, RESULT_ID={RESULT_ID} ", Environment.NewLine,
                "  , RESULT_CODE=(CASE WHEN {RESULT_ID}='F' AND RESULT_CODE IS NULL THEN 'Z9999' ", Environment.NewLine,
                "                      WHEN {RESULT_ID}='C' AND RESULT_CODE IS NULL THEN 'C0000' ", Environment.NewLine,
                "                      ELSE RESULT_CODE ", Environment.NewLine,
                "                 END) ", Environment.NewLine,
                "  , DT_END={DT_END} ", Environment.NewLine,
                "  , RETURN_API_NO={RETURN_API_NO} ", Environment.NewLine,
                "  , UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                "WHERE EDI_EVENT_NO={EDI_EVENT_NO} AND TARGET_SYS_ID={TARGET_SYS_ID} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.STATUS_ID, Value = new DBVarChar(EnumStatusID.F) });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.RESULT_ID, Value = new DBVarChar(para.Result ? EnumResultID.S : EnumResultID.F) });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.DT_END, Value = new DBDateTime(DateTime.Now) });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.RETURN_API_NO, Value = para.ReturnAPINo });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.EDI_EVENT_NO, Value = para.EDIEventNo });
            dbParameters.Add(new DBParameter { Name = EDIEventTargetPara.ParaField.TARGET_SYS_ID, Value = para.TargetSysID });

            base.ExecuteNonQuery(commandText, dbParameters);
        }

        public class SysUserFunMenu : DBTableRow
        {
            public enum DataField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public List<SysUserFunMenu> SelectSysUserFunMenuList()
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT USER_ID FROM SYS_USER_FUN_MENU; ", Environment.NewLine
            });

            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<SysUserFunMenu> sysUserFunMenuList = new List<SysUserFunMenu>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysUserFunMenu sysUserFunMenu = new SysUserFunMenu()
                    {
                        UserID = new DBVarChar(dataRow[SysUserFunMenu.DataField.USER_ID.ToString()])
                    };
                    sysUserFunMenuList.Add(sysUserFunMenu);
                }
                return sysUserFunMenuList;
            }
            return null;
        }

        public class UserMainPara
        {
            public enum ParaField
            {
                USER_ID, USER_PWD,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar UserPWD;
            public DBNVarChar UpdUserID;
        }

        public class UserMain
        {
            public enum DataField
            {
                USER_ID, USER_PWD
            }

            public DBVarChar UserID;
            public DBVarChar UserPWD;
        }

        public List<UserMain> SelectUserMainList()
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @PWD_KEY VARCHAR(10); ", Environment.NewLine,

                "SET @PWD_KEY='p5PNGx@rnc'; ", Environment.NewLine,

                "SELECT U.USER_ID, CAST(DECRYPTBYPASSPHRASE(@PWD_KEY, Z.USER_PWD) AS VARCHAR(40)) AS USER_PWD ", Environment.NewLine,
                "FROM SYS_USER_MAIN U ", Environment.NewLine,
                "LEFT JOIN FERP_OPAGM Z ON U.USER_ID=Z.USER_ID; "
            });

            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<UserMain> userMainList = new List<UserMain>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserMain userMain = new UserMain()
                    {
                        UserID = new DBVarChar(dataRow[UserMain.DataField.USER_ID.ToString()]),
                        UserPWD = new DBVarChar(dataRow[UserMain.DataField.USER_PWD.ToString()])
                    };
                    userMainList.Add(userMain);
                }
                return userMainList;
            }
            return null;
        }

        public enum EnumUpdateUserMainResult
        {
            Success, Failure
        }

        public EnumUpdateUserMainResult UpdateUserMain(List<UserMainPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (UserMainPara para in paraList)
            {
                string updateCommand = string.Concat(new object[]
                {
                    "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                    "            USER_PWD={USER_PWD}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                    "        WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });
                dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommand, dbParameters));
                dbParameters.Clear();
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumUpdateUserMainResult.Success : EnumUpdateUserMainResult.Failure;
        }

        public class UserDetailPara
        {
            public enum ParaField
            {
                USER_ID, USER_IDNO, USER_BIRTHDAY,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar UserIDNo;
            public DBChar UserBirthday;
            public DBNVarChar UpdUserID;
        }

        public class UserDetail
        {
            public enum DataField
            {
                USER_ID, USER_IDNO, USER_BIRTHDAY
            }

            public DBVarChar UserID;
            public DBVarChar UserIDNo;
            public DBChar UserBirthday;
        }

        public List<UserDetail> SelectUserDetailList()
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @PWD_KEY VARCHAR(10); ", Environment.NewLine,

                "SET @PWD_KEY='p5PNGx@rnc'; ", Environment.NewLine,

                "SELECT U.USER_ID ", Environment.NewLine,
                "     , CAST(DECRYPTBYPASSPHRASE(@PWD_KEY, Z.USER_IDNO) AS VARCHAR(20)) AS USER_IDNO ", Environment.NewLine,
                "     , CAST(DECRYPTBYPASSPHRASE(@PWD_KEY, Z.USER_BIRTHDAY) AS CHAR(8)) AS USER_BIRTHDAY ", Environment.NewLine,
                "FROM SYS_USER_DETAIL U ", Environment.NewLine,
                "LEFT JOIN FERP_OPAGM Z ON U.USER_ID=Z.USER_ID; "
            });

            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<UserDetail> userDetailList = new List<UserDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserDetail userDetail = new UserDetail()
                    {
                        UserID = new DBVarChar(dataRow[UserDetail.DataField.USER_ID.ToString()]),
                        UserIDNo = new DBVarChar(dataRow[UserDetail.DataField.USER_IDNO.ToString()]),
                        UserBirthday = new DBChar(dataRow[UserDetail.DataField.USER_BIRTHDAY.ToString()])
                    };
                    userDetailList.Add(userDetail);
                }
                return userDetailList;
            }
            return null;
        }

        public enum EnumUpdateUserDetailResult
        {
            Success, Failure
        }

        public EnumUpdateUserDetailResult UpdateUserDetail(List<UserDetailPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (UserDetailPara para in paraList)
            {
                string updateCommand = string.Concat(new object[]
                {
                    "        UPDATE SYS_USER_DETAIL SET ", Environment.NewLine,
                    "            USER_IDNO={USER_IDNO}, USER_BIRTHDAY={USER_BIRTHDAY}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                    "        WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserDetailPara.ParaField.USER_IDNO.ToString(), Value = para.UserIDNo });
                dbParameters.Add(new DBParameter { Name = UserDetailPara.ParaField.USER_BIRTHDAY.ToString(), Value = para.UserBirthday });
                dbParameters.Add(new DBParameter { Name = UserDetailPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommand, dbParameters));
                dbParameters.Clear();
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumUpdateUserDetailResult.Success : EnumUpdateUserDetailResult.Failure;
        }

        public class SystemMain
        {
            public enum DataField
            {
                PARENT_SYS_ID, SYS_ID, IS_MASTER, HAS_API
            }

            public DBVarChar ParentSysID;
            public DBVarChar SysID;
            public DBChar IsMaster;
            public DBChar HasAPI;
        }

        public List<SystemMain> SelectSystemMainList()
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT Z.PARENT_SYS_ID, Z.SYS_ID ", Environment.NewLine,
                "     , (CASE WHEN Z.PARENT_SYS_ID=Z.SYS_ID THEN 'Y' ELSE 'N' END) AS IS_MASTER ", Environment.NewLine,
                "     , (CASE WHEN S.SERVICE_ID IS NOT NULL THEN 'Y' ", Environment.NewLine,
                "             WHEN Z.SYS_ID IN ('TKNAP', 'WFAP') THEN 'Y' ", Environment.NewLine,
                "             ELSE 'N' ", Environment.NewLine,
                "        END) AS HAS_API ", Environment.NewLine,
                "FROM ( ", Environment.NewLine,
                "    SELECT SYS_ID AS PARENT_SYS_ID, SYS_ID, NULL AS SORT_ORDER ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MAIN M ", Environment.NewLine,
                "    WHERE IS_OUTSOURCING='N' ", Environment.NewLine,
                "    UNION ", Environment.NewLine,
                "    SELECT PARENT_SYS_ID, SYS_ID, SORT_ORDER ", Environment.NewLine,
                "    FROM SYS_SYSTEM_SUB ", Environment.NewLine,
                ") Z ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON Z.PARENT_SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_SERVICE S ON Z.SYS_ID=S.SYS_ID AND S.SERVICE_ID='API' ", Environment.NewLine,
                "ORDER BY M.SORT_ORDER, Z.SORT_ORDER; ", Environment.NewLine
            });

            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemMain> systemMainList = new List<SystemMain>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemMain systemMain = new SystemMain()
                    {
                        ParentSysID = new DBVarChar(dataRow[SystemMain.DataField.PARENT_SYS_ID.ToString()]),
                        SysID = new DBVarChar(dataRow[SystemMain.DataField.SYS_ID.ToString()]),
                        IsMaster = new DBChar(dataRow[SystemMain.DataField.IS_MASTER.ToString()]),
                        HasAPI = new DBChar(dataRow[SystemMain.DataField.HAS_API.ToString()])
                    };
                    systemMainList.Add(systemMain);
                }
                return systemMainList;
            }
            return null;
        }

        public class SQLAgentStatus
        {
            public enum DataField
            {
                RUN_VALUE
            }

            public DBInt RunValue;
        }

        public bool SelectSQLAgentStatus()
        {
            string commandText = string.Concat(new object[]
            {
                "EXEC sp_configure 'Agent XPs'; "
            });

            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SQLAgentStatus status = new SQLAgentStatus()
                {
                    RunValue = new DBInt(dataRow[SQLAgentStatus.DataField.RUN_VALUE.ToString().ToLower()]),
                };

                if (!status.RunValue.IsNull() && status.RunValue.GetValue() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        #region - 查詢事件訂閱關於授權清單 -
        public class AuthEventTargetPara
        {
            public enum ParaField
            {
                SYS_ID, EVENT_GROUP_ID, EVENT_ID
            }

            public class Event
            {
                public DBVarChar SysID;
                public DBVarChar EventGroupID;
                public DBVarChar EventID;
            }

            public List<Event> EventList;
        }


        public class AuthEventTarget : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar EventGroupID;
            public DBVarChar EventID;
        }

        /// <summary>
        /// 查詢事件訂閱關於授權清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<AuthEventTarget> SelectAuthEventTargetList(AuthEventTargetPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandWhere = new List<string>();

            if (para.EventList != null &&
                para.EventList.Any())
            {
                commandWhere.AddRange
                    (
                        (from s in para.EventList
                         select GetCommandText
                             (
                                 ProviderName,
                                 "(SYS_ID = {SYS_ID} AND EVENT_GROUP_ID = {EVENT_GROUP_ID} AND EVENT_ID = {EVENT_ID})",
                                 new DBParameters
                                 {
                                     new DBParameter { Name = AuthEventTargetPara.ParaField.SYS_ID, Value = s.SysID },
                                     new DBParameter { Name = AuthEventTargetPara.ParaField.EVENT_GROUP_ID, Value = s.EventGroupID },
                                     new DBParameter { Name = AuthEventTargetPara.ParaField.EVENT_ID, Value = s.EventID }
                                 })
                            )
                    );
            }

            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT M.SYS_ID AS SysID ",
                    "     , EVENT_GROUP_ID AS EventGroupID ",
                    "     , EVENT_ID AS EventID ",
                    "  FROM SYS_SYSTEM_MAIN M  ",
                    "  JOIN ( ",
                    "        SELECT TARGET_SYS_ID",
                    "          , EVENT_GROUP_ID",
                    "          , EVENT_ID",
                    "       FROM SYS_SYSTEM_EVENT_TARGET  ",
                    string.Format("    WHERE {0}", string.Join(" OR ", commandWhere)),
                    "       ) T ON M.SYS_ID = T.TARGET_SYS_ID ",
                    " WHERE M.IS_DISABLE='N'",
                    " ORDER BY M.SORT_ORDER",
                }));
            return GetEntityList<AuthEventTarget>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢使用已離職清單 -
        public class UserLeft : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar IsLeft;
            public DBChar LeftDate;
        }

        /// <summary>
        /// 查詢使用已離職清單
        /// </summary>
        /// <returns></returns>
        public List<UserLeft> SelectUserLeftList()
        {
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @PREV_YMD CHAR(8) = dbo.FN_GET_SYSDATE(DATEADD(DAY, -2, GETDATE()));",
                    "SELECT M.USER_ID AS UserID",
                    "     , R.USER_NM AS UserNM ",
                    "     , M.IS_LEFT AS IsLeft ",
                    "     , M.LEFT_DATE AS LeftDate",
                    "  FROM SYS_USER_MAIN M",
                    "  JOIN RAW_CM_USER R",
                    "    ON M.USER_ID = R.USER_ID",
                    " WHERE M.LEFT_DATE IS NOT NULL",
                    "   AND M.LEFT_DATE < @PREV_YMD",
                    "   AND M.IS_LEFT = 'Y'",
                    "   AND (EXISTS(SELECT * FROM SYS_USER_SYSTEM_ROLE R WHERE R.USER_ID = M.USER_ID)",
                    "   OR   EXISTS(SELECT * FROM SYS_USER_FUN F WHERE F.USER_ID = M.USER_ID))",
                    " UNION",
                    "SELECT M.USER_ID AS UserID",
                    "     , R.USER_NM AS UserNM ",
                    "     , M.IS_LEFT AS IsLeft ",
                    "     , M.LEFT_DATE AS LeftDate",
                    "  FROM SYS_USER_MAIN M",
                    "  JOIN RAW_CM_USER R",
                    "    ON M.USER_ID = R.USER_ID",
                    " WHERE M.USER_ID LIKE 'T%'",
                    "   AND M.IS_LEFT = 'Y'",
                    "   AND M.LEFT_DATE IS NULL",
                    "   AND (EXISTS(SELECT * FROM SYS_USER_SYSTEM_ROLE R WHERE R.USER_ID = M.USER_ID)",
                    "   OR   EXISTS(SELECT * FROM SYS_USER_FUN F WHERE F.USER_ID = M.USER_ID))"
                }));
            return GetEntityList<UserLeft>(commandText.ToString(), new List<DBParameter>());
        }
        #endregion

        #region - 查詢功能指派使用者清單 -
        public class UserSystemFunAssignPara
        {
            public enum ParaField
            {
                USER_ID, SYS_ID
            }

            public DBVarChar UserID;
            public List<DBVarChar> SysIDList;
        }

        public class UserSystemFunAssign : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
        }

        /// <summary>
        /// 查詢功能指派使用者清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<UserSystemFunAssign> SelectUserSystemFunAssignList(UserSystemFunAssignPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT SUF.USER_ID AS UserID ",
                    "     , SUF.SYS_ID AS SysID ",
                    "     , SUF.FUN_CONTROLLER_ID AS FunControllerID ",
                    "     , SUF.FUN_ACTION_NAME AS FunActionName ",
                    "  FROM SYS_USER_FUN SUF",
                    "  JOIN (",
                    "        SELECT SYS_ID",
                    "             , FUN_CONTROLLER_ID",
                    "             , FUN_ACTION_NAME",
                    "          FROM SYS_USER_FUN",
                    "         WHERE IS_ASSIGN = 'Y'",
                    "           AND USER_ID = {USER_ID}",
                    "           AND SYS_ID IN ({SYS_ID})",
                    "       ) FUN",
                    "    ON SUF.SYS_ID = FUN.SYS_ID",
                    "   AND SUF.FUN_CONTROLLER_ID = FUN.FUN_CONTROLLER_ID",
                    "   AND SUF.FUN_ACTION_NAME = FUN.FUN_ACTION_NAME",
                    "   AND SUF.USER_ID <> {USER_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = UserSystemFunAssignPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemFunAssignPara.ParaField.SYS_ID, Value = para.SysIDList });

            return GetEntityList<UserSystemFunAssign>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 移除離職人員使用者權限 -
        public enum EnumDeleteLeftUserRoleResult
        {
            Success,
            Failure
        }

        public class DeleteLeftUserRolePara
        {
            public enum ParaField
            {
                USER_ID,
                IP_ADDRESS,
                UPD_USER_ID,
                EXEC_SYS_ID
            }

            public DBVarChar ExecSysID;
            public DBVarChar UpdUserID;
            public DBVarChar IPAddress;
            public List<DBVarChar> UserIDList;
        }

        /// <summary>
        /// 移除離職人員使用者權限
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumDeleteLeftUserRoleResult DeleteLeftUserRole(DeleteLeftUserRolePara para)
        {
            var dbParameters = new List<DBParameter>();
            var updateLeftUserComm = new List<string>();
            var commandText = new StringBuilder();

            foreach (var userID in para.UserIDList)
            {
                updateLeftUserComm.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine, new object[]
                {
                    "DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID = {USER_ID}; ",
                    "DELETE FROM SYS_USER_SYSTEM WHERE USER_ID = {USER_ID}; ",
                    "DELETE FROM SYS_USER_FUN WHERE USER_ID = {USER_ID}; ",
                    "DELETE FROM SYS_USER_FUN_MENU WHERE USER_ID = {USER_ID}; ",
                    "UPDATE SYS_USER_MAIN ",
                    "   SET ROLE_GROUP_ID = NULL ",
                    "     , IS_DISABLE = 'Y' ",
                    "     , UPD_USER_ID = {UPD_USER_ID} ",
                    "     , UPD_DT = GETDATE() ",
                    " WHERE USER_ID = {USER_ID}; ",
                    "EXECUTE dbo.SP_LOG_USER_SYSTEM_ROLE {USER_ID} ,NULL ,NULL ,NULL ,'" + Mongo_BaseAP.EnumModifyType.D + "', {EXEC_SYS_ID} ,{IP_ADDRESS} ,{UPD_USER_ID};"
                }), new List<DBParameter>
                {
                    new DBParameter { Name = DeleteLeftUserRolePara.ParaField.USER_ID, Value = userID },
                    new DBParameter { Name = DeleteLeftUserRolePara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID },
                    new DBParameter { Name = DeleteLeftUserRolePara.ParaField.IP_ADDRESS, Value = para.IPAddress },
                    new DBParameter { Name = DeleteLeftUserRolePara.ParaField.UPD_USER_ID, Value = UpdUserID }
                }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    #region - 移除離職人員使用者權限 -
                    string.Join(Environment.NewLine, updateLeftUserComm),
                    #endregion
                    "       SET @RESULT = 'Y';",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = 'N';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteLeftUserRoleResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 更新離職人員 -
        public enum EnumUpdateLeftUserResult
        {
            Success,
            Failure
        }

        /// <summary>
        /// 查詢離職人員erp Opagm20清單
        /// </summary>
        /// <returns></returns>
        public EnumUpdateLeftUserResult UpdateLeftUser()
        {
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        WITH main",
                    "             AS (SELECT F.USER_ID, ",
                    "                        F.IS_LEFT",
                    "                 FROM FERP_OPAGM F",
                    "                 WHERE LEFT(RIGHT(F.USER_ID, 4), 1) <> 'T'",
                    "                 EXCEPT",
                    "                 SELECT R.USER_ID, ",
                    "                        R.IS_LEFT",
                    "                 FROM RAW_CM_USER R",
                    "                 WHERE LEFT(RIGHT(R.USER_ID, 4), 1) <> 'T')",

                    "             UPDATE RAW_CM_USER",
                    "                SET IS_LEFT = M.IS_LEFT",
                    "               FROM RAW_CM_USER RCM",
                    "               JOIN main M",
                    "                 ON RCM.USER_ID = M.USER_ID;",

                    "        UPDATE SYS_USER_MAIN",
                    "           SET IS_LEFT = RCM.IS_LEFT",
                    "             , IS_DISABLE = RCM.IS_LEFT",
                    "             , UPD_USER_ID = {UPD_USER_ID} ",
                    "             , UPD_DT = GETDATE() ",
                    "          FROM SYS_USER_MAIN M",
                    "          JOIN RAW_CM_USER RCM",
                    "            ON RCM.USER_ID = M.USER_ID",
                    "           AND RCM.IS_LEFT <> M.IS_LEFT;",

                    "        SET @RESULT = 'Y';",
                    "        COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "        SET @RESULT = 'N';",
                    "        SET @ERROR_LINE = ERROR_LINE();",
                    "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "        ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = "UPD_USER_ID", Value = UpdUserID });
            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumUpdateLeftUserResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 發送留言 -
        public enum EnumInsertErpMessageResult
        {
            Success,
            Failure
        }

        public class InsertErpMessagePara
        {
            public enum ParaField
            {
                MSG_MSTFN,
                MSG_MESSAGE,
                MSG_STFN,
                MSG_URL
            }

            public DBVarChar MsgMstfn;
            public DBNVarChar MsgMessage;
            public DBVarChar MsgStfn;
            public DBVarChar MsgUrl;
        }

        public EnumInsertErpMessageResult InsertErpMessageResult(InsertErpMessagePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder();
            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        INSERT INTO message",
                    "             ( msg_stfn",
                    "             , msg_message",
                    "             , msg_url",
                    "             , msg_mstfn",
                    "             , msg_mdate",
                    "             )",
                    "        VALUES ({MSG_STFN}",
                    "             , {MSG_MESSAGE}",
                    "             , {MSG_URL}",
                    "             , {MSG_MSTFN}",
                    "             , GETDATE()",
                    "             )",
                    "       SET @RESULT = 'Y';",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = 'N';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = InsertErpMessagePara.ParaField.MSG_STFN, Value = para.MsgStfn });
            dbParameters.Add(new DBParameter { Name = InsertErpMessagePara.ParaField.MSG_MESSAGE, Value = para.MsgMessage });
            dbParameters.Add(new DBParameter { Name = InsertErpMessagePara.ParaField.MSG_URL, Value = para.MsgUrl });
            dbParameters.Add(new DBParameter { Name = InsertErpMessagePara.ParaField.MSG_MSTFN, Value = para.MsgMstfn });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumInsertErpMessageResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 查詢離職人員erp Opagm20清單 -
        public class UserErpInfoPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public List<DBNVarChar> UserIDList;
        }

        public class UserErpInfo : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserUnitID;
            public DBNVarChar UserUnitNM;
            public DBNVarChar UserJob1NM;
            public DBNVarChar UserJob2NM;
        }

        /// <summary>
        /// 查詢離職人員erp Opagm20清單
        /// </summary>
        /// <returns></returns>
        public List<UserErpInfo> SelectUserErpInfoList(UserErpInfoPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT op20.stfn_stfn AS UserID",
                    "    , op20.stfn_cname AS UserNM",
                    "	 , op20.stfn_prof AS UserUnitID",
                    "	 , prof.prof_cname AS UserUnitNM",
                    "	 , istb1.tabl_dname AS UserJob1NM",
                    "	 , istb2.tabl_dname AS UserJob2NM",
                    "FROM opagm20 op20",
                    "LEFT JOIN ispfm00 prof",
                    "ON op20.stfn_prof = prof.prof_prof",
                    "LEFT JOIN istbm00 istb1",
                    "ON op20.stfn_job1 = istb1.tabl_code",
                    "AND istb1.tabl_type = 'JOB1'",
                    "AND istb1.tabl_code > ''",
                    "LEFT JOIN istbm00 istb2",
                    "ON op20.stfn_job2 = istb2.tabl_code",
                    "AND istb2.tabl_type = 'JOB2'",
                    "AND istb2.tabl_code > ''",
                    "WHERE op20.stfn_stfn IN ({USER_ID})",
                }));
            dbParameters.Add(new DBParameter { Name = UserErpInfoPara.ParaField.USER_ID, Value = para.UserIDList });
            return GetEntityList<UserErpInfo>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢ERP所有在職員工ID -
        public class UserIDInfo : DBTableRow
        {
            public enum DataField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public List<UserIDInfo> SelectUserIDList()
        {
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT stfn_stfn AS UserID",
                "  FROM opagm20 ",
                "  JOIN psppm00",
                "    ON stfn_stfn = pp00_stfn",
                "   AND (pp00_ldate = '' OR pp00_ldate >= CONVERT(CHAR(8), GETDATE(), 112)) ",
                " WHERE stfn_sts = 0;"
            });

            return GetEntityList<UserIDInfo>(commandText, null);
        }
        #endregion

        #region - 查詢ERP員工選單資料 -

        public class ERPUserMenuPara
        {

            public enum ParaField
            {
                USER_ID,
                MENU_NO,
                MM00_ALL_USE
            }

            public DBVarChar UserID;
            public DBChar Menu_NO;
            public DBChar mm00_all_use;
        }

        public class ERPUserMenu : DBTableRow
        {
            public DBNVarChar tabl_cname;
            public DBNVarChar mm00_name;
            public DBVarChar mm00_sys1;
            public DBVarChar mm00_aspid;
        }

        public List<ERPUserMenu> SelectERPUserMenuList(ERPUserMenuPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "EXECUTE dbo.usp_UserMenu {USER_ID},{MENU_NO},{MM00_ALL_USE}",
            });

            dbParameters.Add(new DBParameter { Name = ERPUserMenuPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserMenuPara.ParaField.MENU_NO.ToString(), Value = para.Menu_NO });
            dbParameters.Add(new DBParameter { Name = ERPUserMenuPara.ParaField.MM00_ALL_USE.ToString(), Value = para.mm00_all_use });

            return GetEntityList<ERPUserMenu>(commandText, dbParameters);
        }
        #endregion

        #region  - 查詢系統角色ID清單 -
        public class SysUserSystemRolePara
        {
            public enum ParaField
            {
                SYS_ID, ROLE_ID
            }

            public DBChar SYS_ID;
            public DBChar ROLE_ID;
        }

        public class SystemRoleData : DBTableRow
        {
            public DBVarChar USER_ID;
        }

        /// <summary>
        /// 取得系統角色ID清單
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="roleID"></param>
        /// <param name="userCount">取得數量</param>
        /// <returns></returns>
        public List<SystemRoleData> SelectSystemRoleUserIdBySysId(string sysID, string roleID, int userCount)
        {
            string commandText = string.Concat(new object[]
            {
                $"SELECT TOP({userCount}) USER_ID ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "WHERE SYS_ID = {SYS_ID} ", Environment.NewLine,
                "AND ROLE_ID = {ROLE_ID}", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysUserSystemRolePara.ParaField.SYS_ID.ToString(), Value = new DBVarChar(sysID) });
            dbParameters.Add(new DBParameter { Name = SysUserSystemRolePara.ParaField.ROLE_ID.ToString(), Value = new DBVarChar(roleID) });
            return GetEntityList<SystemRoleData>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}
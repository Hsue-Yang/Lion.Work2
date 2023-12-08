using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserMain : EntitySys
    {
        public EntityUserMain(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserMainPara
        {
            public enum ParaField
            {
                USER_ID, USER_NM, USER_PWD, USER_RAW_PWD, PWD_VALID_DATE, IS_LEFT,
                USER_ENM, USER_IDNO, USER_BIRTHDAY, USER_TEL, USER_EXTENSION, USER_MOBILE,
                USER_EMAIL, USER_GOOGLE_ACCOUNT, IS_GACC_ENABLE,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;
            public DBVarChar UserRawPWD;
            public DBVarChar PWDValidDate;
            public DBChar IsLeft;

            public DBNVarChar UserENM;
            public DBVarChar UserIDNO;
            public DBChar UserBirthday;
            public DBVarChar UserTel;
            public DBVarChar UserExtension;
            public DBVarChar UserMoblie;
            public DBVarChar UserEMail;
            public DBVarChar UserGoogleAccount;
            public DBChar IsGAccEnable;

            public DBVarChar UpdUserID;
        }

        public class UserMain : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, USER_PWD, PWD_VALID_DATE,
                USER_ENM, USER_TEL, USER_EXTENSION, USER_MOBILE, 
                USER_EMAIL, USER_GOOGLE_ACCOUNT, IS_GACC_ENABLE,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;
            public DBDateTime PWDValidDate;

            public DBNVarChar UserENM;
            public DBVarChar UserTel;
            public DBVarChar UserExtension;
            public DBVarChar UserMobile;
            public DBVarChar UserEMail;
            public DBVarChar UserGoogleAccount;
            public DBChar IsGAccEnable;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT; 
        }

        public UserMain SelectUserMain(UserMainPara para)
        {
            string commandText = string.Concat(new object[]
            {   
                "SELECT M.USER_ID, M.USER_NM, M.USER_PWD, M.PWD_VALID_DATE ", Environment.NewLine,
                "     , D.USER_ENM, D.USER_TEL, D.USER_EXTENSION, D.USER_MOBILE, D.USER_EMAIL ", Environment.NewLine,
                "     , D.USER_GOOGLE_ACCOUNT, D.IS_GACC_ENABLE", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UPD_USER_NM, M.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_MAIN M ", Environment.NewLine,
                "JOIN SYS_USER_DETAIL D ON D.USER_ID=M.USER_ID ", Environment.NewLine,
                "WHERE M.USER_ID={USER_ID} AND M.IS_DISABLE='N' AND M.IS_LEFT='N'; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            
            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserMain userMain = new UserMain()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.USER_NM.ToString()]),
                    UserPWD = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_PWD.ToString()]),
                    PWDValidDate = new DBDateTime(dataTable.Rows[0][UserMain.DataField.PWD_VALID_DATE.ToString()]),

                    UserENM = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.USER_ENM.ToString()]),
                    UserTel = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_TEL.ToString()]),
                    UserExtension = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_EXTENSION.ToString()]),
                    UserMobile = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_MOBILE.ToString()]),
                    UserEMail = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_EMAIL.ToString()]),
                    UserGoogleAccount = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_GOOGLE_ACCOUNT.ToString()]),
                    IsGAccEnable = new DBChar(dataTable.Rows[0][UserMain.DataField.IS_GACC_ENABLE.ToString()]),

                    UpdUserNM = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.UPD_USER_NM.ToString()]),
                    UpdDT = new DBDateTime(dataTable.Rows[0][UserMain.DataField.UPD_DT.ToString()])
                };
                return userMain;
            }
            return null;
        }

        public enum EnumEditUserMainResult
        {
            Success, Failure
        }

        public EnumEditUserMainResult EditUserMain(UserMainPara para)
        {
            #region 判斷是否有修改密碼

            string commandUpdateText = string.Empty;

            if (!para.UserPWD.IsNull())
            {
                commandUpdateText = string.Concat(new object[]
                {
                    commandUpdateText,
                    "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                    "            USER_NM={USER_NM} ", Environment.NewLine,
                    "          , USER_PWD={USER_PWD} ", Environment.NewLine,
                    "          , PWD_VALID_DATE={PWD_VALID_DATE} ", Environment.NewLine,
                    "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                    "          , UPD_DT=GETDATE() ", Environment.NewLine,
                    "        FROM SYS_USER_MAIN ", Environment.NewLine,
                    "        WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });
            }
            else
            {
                commandUpdateText = string.Concat(new object[]
                {
                    commandUpdateText,
                    "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                    "            USER_NM={USER_NM} ", Environment.NewLine,
                    "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                    "          , UPD_DT=GETDATE() ", Environment.NewLine,
                    "        FROM SYS_USER_MAIN ", Environment.NewLine,
                    "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                });
            }

            #endregion

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                commandUpdateText,

                "        DELETE FROM SYS_USER_DETAIL WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        INSERT INTO SYS_USER_DETAIL VALUES ( ", Environment.NewLine,
                "            {USER_ID}, {USER_ENM} ", Environment.NewLine,
                "          , {USER_IDNO}, {USER_BIRTHDAY} ", Environment.NewLine,
                "          , {USER_TEL}, {USER_EXTENSION}, {USER_MOBILE}, {USER_EMAIL} ", Environment.NewLine,
                "          , {USER_GOOGLE_ACCOUNT}, {IS_GACC_ENABLE}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.PWD_VALID_DATE.ToString(), Value = para.PWDValidDate });

            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ENM.ToString(), Value = para.UserENM });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_IDNO.ToString(), Value = para.UserIDNO });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_BIRTHDAY.ToString(), Value = para.UserBirthday });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_TEL.ToString(), Value = para.UserTel });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_EXTENSION.ToString(), Value = para.UserExtension });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_MOBILE.ToString(), Value = para.UserMoblie });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_EMAIL.ToString(), Value = para.UserEMail });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_GOOGLE_ACCOUNT.ToString(), Value = para.UserGoogleAccount });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.IS_GACC_ENABLE.ToString(), Value = para.IsGAccEnable });
            
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserMainResult.Success : EnumEditUserMainResult.Failure;
        }


        public enum EnumSyncEditOpagmResult
        {
            Success, Failure
        }

        public EnumSyncEditOpagmResult SyncEditOpagm(UserMainPara para)
        {
            #region 判斷是否有修改密碼

            string commandUpdateText = string.Empty;

            if (!para.UserRawPWD.IsNull())
            {
                commandUpdateText = string.Concat(new object[]
                {
                    commandUpdateText,
                    "        UPDATE opagm20 SET ", Environment.NewLine,
                    "            stfn_pswd={USER_RAW_PWD} ", Environment.NewLine,
                    "          , stfn_pswd_date={PWD_VALID_DATE} ", Environment.NewLine,
                    "        FROM opagm20 ", Environment.NewLine,
                    "        WHERE stfn_stfn={USER_ID}; ", Environment.NewLine
                });
            }

            #endregion

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                commandUpdateText,

                "        UPDATE opagm24 SET ", Environment.NewLine,
                "            stf4_alias={USER_ENM} ", Environment.NewLine,
                "          , stf4_otel={USER_TEL} ", Environment.NewLine,
                "          , stf4_extel={USER_EXTENSION} ", Environment.NewLine,
                "          , stf4_mtel={USER_MOBILE} ", Environment.NewLine,
                "          , stf4_email2={USER_EMAIL} ", Environment.NewLine,
                "        FROM opagm24 ", Environment.NewLine,
                "        WHERE stf4_stfn={USER_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_RAW_PWD.ToString(), Value = para.UserRawPWD });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.PWD_VALID_DATE.ToString(), Value = para.PWDValidDate });

            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ENM.ToString(), Value = para.UserENM });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_TEL.ToString(), Value = para.UserTel });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_EXTENSION.ToString(), Value = para.UserExtension });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_MOBILE.ToString(), Value = para.UserMoblie });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_EMAIL.ToString(), Value = para.UserEMail });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumSyncEditOpagmResult.Success : EnumSyncEditOpagmResult.Failure;
        }
    }

    public class MongoUserMain : MongoSys
    {
        public MongoUserMain(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class LogUserPWDPara : MongoElement
        {
            public enum ParaField
            {
                USER_ID, MODIFY_DATE
            }

            public DBVarChar UserID;
            public DBChar ModifyDate;
        }

        public class LogUserPWD
        {
            public enum DataField
            {
                USER_ID, USER_PWD, MODIFY_DATE
            }

            public DBVarChar UserID;
            public DBVarChar UserPWD;
            public DBChar ModifyDate;
        }

        public List<LogUserPWD> SelectUserPastPWDList(LogUserPWDPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_USER_PWD");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPWD.DataField.USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPWD.DataField.USER_PWD.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPWD.DataField.MODIFY_DATE.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.In, LogUserPWD.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.GreaterThan, LogUserPWD.DataField.MODIFY_DATE.ToString(), para.ModifyDate);

            return base.Select<LogUserPWD>(command);
        }
    }
}
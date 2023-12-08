// 新增日期：2017-01-06
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityAppUserMobile : EntityPub
    {
        public EntityAppUserMobile(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得使用者行動裝置清單 -
        public class AppUserMobilePara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class AppUserMobile : DBTableRow
        {
            public DBVarChar AppUUID;
            public DBVarChar DeviceTokenID;
            public DBVarChar AppID;
            public DBVarChar MobileOS;
            public DBVarChar MobileType;
            public DBChar IsMaster;
            public DBDateTime UpdDT;
        }

        public List<AppUserMobile> SelectAppUserMobileList(AppUserMobilePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT M.APP_UUID AS AppUUID",
	                "     , T.DEVICE_TOKEN_ID AS DeviceTokenID",
                    "     , APP_ID AS AppID",
                    "     , MOBILE_OS AS MobileOS",
                    "	  , MOBILE_TYPE AS MobileType",
                    "	  , T.IS_MASTER AS IsMaster",
                    "     , T.UPD_DT AS UpdDT",
                    "  FROM APP_USER_MOBILE M",
                    "  JOIN APP_USER_DEVICE_TOKEN T",
                    "    ON M.USER_ID = T.USER_ID",
                    "   AND M.APP_UUID = T.APP_UUID",
                    " WHERE M.IS_DISABLE = '"+ EnumYN.N +"'",
                    "   AND M.USER_ID = {USER_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = AppUserMobilePara.ParaField.USER_ID, Value = para.UserID });

            return GetEntityList<AppUserMobile>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 更新使用者裝置 -
        public class AppUserDevicePara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID,
                DEVICE_TOKEN_ID
            }

            public DBVarChar UserID;
            public List<Device> AppUserDeviceList;
        }

        public class Device
        {
            public DBVarChar AppUUID;
            public DBVarChar DeviceTokenID;
        }

        public enum EnumUpdateAppUserDeviceResult
        {
            Success,
            Failure
        }

        public EnumUpdateAppUserDeviceResult UpdateAppUserDevice(AppUserDevicePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "DECLARE @USER_ID VARCHAR(20) = {USER_ID}",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        UPDATE APP_USER_DEVICE_TOKEN",
                    "           SET IS_MASTER = '" + EnumYN.N + "'",
                    "             , UPD_USER_ID = @USER_ID",
                    "             , UPD_DT = GETDATE()",
                    "         WHERE USER_ID = @USER_ID;"
                }));

            foreach (var device in para.AppUserDeviceList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    "        UPDATE APP_USER_DEVICE_TOKEN",
                    "           SET IS_MASTER = '" + EnumYN.Y + "'",
                    "             , UPD_USER_ID = @USER_ID",
                    "             , UPD_DT = GETDATE()",
                    "         WHERE USER_ID = @USER_ID",
                    "           AND APP_UUID = {APP_UUID}",
                    "           AND DEVICE_TOKEN_ID = {DEVICE_TOKEN_ID};"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = AppUserDevicePara.ParaField.APP_UUID, Value = device.AppUUID },
                        new DBParameter { Name = AppUserDevicePara.ParaField.DEVICE_TOKEN_ID, Value = device.DeviceTokenID }
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
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

            dbParameters.Add(new DBParameter { Name = AppUserDevicePara.ParaField.USER_ID, Value = para.UserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumUpdateAppUserDeviceResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}
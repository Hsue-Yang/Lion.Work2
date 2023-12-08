// 新增日期：2016-12-23
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityLionGroupAppService : DBEntity
    {
#if !NET461
        public EntityLionGroupAppService(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityLionGroupAppService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
        
        #region - 查詢App使用者行動裝置 -
        public class AppUserMobilePara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID
            }


            public DBVarChar UserID;
            public DBVarChar AppUUID;
        }

        public class AppUserMobile : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar AppUUID;
        }

        /// <summary>
        /// 查詢App使用者行動裝置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public AppUserMobile SelectAppUserMobile(AppUserMobilePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT USER_ID AS UserID",
                    "     , APP_UUID AS AppUUID",
                    "  FROM APP_USER_MOBILE",
                    " WHERE USER_ID = {USER_ID}",
                    "   AND APP_UUID = {APP_UUID};"
                }));

            dbParameters.Add(new DBParameter { Name = AppUserMobilePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = AppUserMobilePara.ParaField.APP_UUID, Value = para.AppUUID });

            return GetEntityList<AppUserMobile>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 取得推播訊息發送對象資訊 -
        public class PushMessageUserInfoPara
        {
            public enum ParaField
            {
                USER_ID,
                APP_FUN_ID
            }

            public DBVarChar FunID;
            public List<DBVarChar> UserIDs;
        }
        
        public class PushMessageUserInfo : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar Os;
            public DBVarChar AppID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
            public DBVarChar AppRoleID;
            public DBVarChar RegistrationID;
            public DBInt RemindMinute;
            public DBChar IsOpenPush;
            public DBChar HasRole;
        }

        public enum EnumDeviceTokenType
        {
            Firebase
        }

        public enum EnumAppServicePushStatus
        {
            [Description("Cancel")]
            C,
            [Description("NotSend")]
            N
        }

        public List<PushMessageUserInfo> SelectPushMessageUserInfoList(PushMessageUserInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT M.USER_ID AS UserID",
                "     , M.MOBILE_OS AS OS",
                "     , M.APP_ID AS AppID",
                "     , M.APP_UUID AS AppUUID",
                "     , F.APP_FUN_ID AS AppFunID",
                "     , FR.APP_ROLE_ID AS AppRoleID",
                "     , TOKEN.DEVICE_TOKEN_ID AS RegistrationID",
                "     , F.REMIND_MINUTE AS RemindMinute",
                "     , F.IS_OPEN_PUSH AS IsOpenPush",
                "     , CASE WHEN UFR.USER_ID IS NOT NULL THEN 'Y' ELSE 'N' END AS HasRole",
                "  FROM APP_USER_MOBILE M",
                "  JOIN APP_USER_FUN F",
                "    ON M.USER_ID = F.USER_ID",
                "   AND M.APP_UUID = F.APP_UUID",
                "  JOIN APP_FUN_ROLE FR",
                "    ON M.APP_ID = FR.APP_ID",
                "   AND F.APP_FUN_ID = FR.APP_FUN_ID",
                "   AND FR.APP_FUN_ID = {APP_FUN_ID}",
                "  JOIN APP_USER_DEVICE_TOKEN TOKEN",
                "    ON M.USER_ID = TOKEN.USER_ID",
                "   AND M.APP_UUID = TOKEN.APP_UUID",
                "   AND TOKEN.DEVICE_TOKEN_TYPE = '" + EnumDeviceTokenType.Firebase + "'",
                "   AND TOKEN.IS_MASTER = '" + EnumYN.Y + "'",
                "  LEFT JOIN APP_USER_FUN_ROLE UFR",
                "    ON UFR.USER_ID = M.USER_ID",
                "   AND UFR.APP_UUID = F.APP_UUID",
                "   AND UFR.APP_FUN_ID = FR.APP_FUN_ID",
                "   AND UFR.APP_ROLE_ID = FR.APP_ROLE_ID",
                " WHERE M.USER_ID IN ({USER_ID})"
            }));

            dbParameters.Add(new DBParameter { Name = PushMessageUserInfoPara.ParaField.APP_FUN_ID, Value = para.FunID });
            dbParameters.Add(new DBParameter { Name = PushMessageUserInfoPara.ParaField.USER_ID, Value = para.UserIDs });
            
            return GetEntityList<PushMessageUserInfo>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得使用者功能 -
        public class AppUserFunPara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID,
                APP_FUN_ID
            }

            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
        }

        public class AppUserFun : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
            public DBInt RemindMinute;
            public DBChar IsOpenPush;
        }

        public AppUserFun SelectAppUserFun(AppUserFunPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT USER_ID AS UserID",
                    "     , APP_UUID AS AppUUID",
                    "     , APP_FUN_ID AS AppFunID",
                    "     , REMIND_MINUTE AS RemindMinute",
                    "     , IS_OPEN_PUSH AS IsOpenPush",
                    "  FROM APP_USER_FUN",
                    " WHERE USER_ID = {USER_ID}",
                    "   AND APP_UUID = {APP_UUID}",
                    "   AND APP_FUN_ID = {APP_FUN_ID};"
                }));

            dbParameters.Add(new DBParameter { Name = AppUserFunPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = AppUserFunPara.ParaField.APP_UUID, Value = para.AppUUID });
            dbParameters.Add(new DBParameter { Name = AppUserFunPara.ParaField.APP_FUN_ID, Value = para.AppFunID });

            return GetEntityList<AppUserFun>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion
    }
}
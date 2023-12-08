// 新增日期：2016-06-20
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------
using System.Collections.Generic;
namespace LionTech.Entity.ERP.PubData
{
    public class MongoUserSystemNotifications : MongoPubData
    {
        public MongoUserSystemNotifications(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SysUserSystemNotifications : MongoDocument
        {
            public enum DataField
            {
                USER_ID, NOTICE_DT, NOTICE_CONTENT, NOTICE_URL, IS_READ
            }

            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("NOTICE_DT")]
            public DBDateTime NoticeDT;

            [DBTypeProperty("NOTICE_CONTENT")]
            public DBNVarChar NoticeContent;

            [DBTypeProperty("NOTICE_URL")]
            public DBVarChar NoticeURL;

            [DBTypeProperty("IS_READ")]
            public DBChar IsRead;

            [DBTypeProperty("API_NO")]
            public DBChar APINO;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;
        }

        public class SysUserSystemNotificationsPara
        {

            public enum ParaField
            {
                USER_ID, IS_READ
            }

            public DBVarChar UserID;
            public DBInt DataIndex;
        }

        public List<SysUserSystemNotifications> SelectSysUserSystemNotificationsList(SysUserSystemNotificationsPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.SYS_USER_SYSTEM_NOTIFICATIONS.ToString());

            command.SetRowSkip(para.DataIndex.GetValue());
            command.SetRowCount(10);

            command.AddFields(EnumSpecifiedFieldType.Select, SysUserSystemNotifications.DataField.NOTICE_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, SysUserSystemNotifications.DataField.NOTICE_CONTENT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, SysUserSystemNotifications.DataField.NOTICE_URL.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, SysUserSystemNotifications.DataField.IS_READ.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, SysUserSystemNotificationsPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddSortBy(EnumSortType.DESC, SysUserSystemNotifications.DataField.NOTICE_DT.ToString());

            List<SysUserSystemNotifications> result = Select<SysUserSystemNotifications>(command);

            command.Clear();

            command.AddUpdateSet(SysUserSystemNotificationsPara.ParaField.IS_READ.ToString(), new DBChar(EnumYN.Y));

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, SysUserSystemNotificationsPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysUserSystemNotificationsPara.ParaField.IS_READ.ToString(), new DBChar(EnumYN.N));

            Update(command);

            return result;
        }

        public long SelectSysUserSystemNotificationsUnReadCount(SysUserSystemNotificationsPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.SYS_USER_SYSTEM_NOTIFICATIONS.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, SysUserSystemNotificationsPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysUserSystemNotificationsPara.ParaField.IS_READ.ToString(), new DBChar(EnumYN.N));

            return Count(command);
        }
    }
}
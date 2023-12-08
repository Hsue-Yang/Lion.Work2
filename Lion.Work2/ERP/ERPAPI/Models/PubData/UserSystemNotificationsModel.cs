using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.PubData;

namespace ERPAPI.Models.PubData
{
    public class UserSystemNotificationsModel : PubDataModel
    {
        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        //[AllowHtml]
        public string APIPara { get; set; }
        #endregion

        public class APIParaData
        {
            public int? DataIndex { get; set; }
            public string UserID { get; set; }
            public string Content { get; set; }
            public string URL { get; set; }
        }
        
        public APIParaData APIData { get; set; }
        public long UnReadCount { get; set; }
        public List<MongoUserSystemNotifications.SysUserSystemNotifications> SysUserSystemNotificationsList { get; set; }

        public bool GetInsertUserSystemNotifications(string apiNo)
        {
            try
            {
                var mongo = new MongoUserSystemNotifications(ConnectionStringMSERP, ProviderNameMSERP);
                var para =
                    new MongoUserSystemNotifications.SysUserSystemNotifications
                    {
                        UserID = new DBVarChar(string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID),
                        NoticeDT = new DBDateTime(DateTime.Now),
                        NoticeContent = new DBNVarChar(string.IsNullOrWhiteSpace(APIData.Content) ? null : APIData.Content),
                        NoticeURL = new DBVarChar(string.IsNullOrWhiteSpace(APIData.URL) ? null : APIData.URL),
                        IsRead = new DBChar(EnumYN.N),
                        APINO = new DBChar(string.IsNullOrWhiteSpace(apiNo) ? null : apiNo),
                        UpdUserID = new DBVarChar("APIService.ERP.PubData"),
                        UpdDT = new DBDateTime(DateTime.Now)
                    };

                mongo.Insert(Mongo_BaseAP.EnumMongoDocName.SYS_USER_SYSTEM_NOTIFICATIONS, para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public bool GetUserSystemNotifications()
        {
            try
            {
                var mongo = new MongoUserSystemNotifications(ConnectionStringMSERP, ProviderNameMSERP);
                var para =
                    new MongoUserSystemNotifications.SysUserSystemNotificationsPara
                    {
                        UserID = new DBVarChar(APIData.UserID),
                        DataIndex = new DBInt(APIData.DataIndex)
                    };

                SysUserSystemNotificationsList = mongo.SelectSysUserSystemNotificationsList(para);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public bool GetUserSystemNotificationsUnReadCount()
        {
            try
            {
                var mongo = new MongoUserSystemNotifications(ConnectionStringMSERP, ProviderNameMSERP);
                var para =
                    new MongoUserSystemNotifications.SysUserSystemNotificationsPara
                    {
                        UserID = new DBVarChar(APIData.UserID)
                    };

                UnReadCount = mongo.SelectSysUserSystemNotificationsUnReadCount(para);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}
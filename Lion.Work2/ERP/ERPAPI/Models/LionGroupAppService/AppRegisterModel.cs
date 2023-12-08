// 新增日期：2017-01-03
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LionGroupAppService;
using LionTech.Utility;
using System.Configuration;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using FirebaseAdmin.Messaging;
using System.Linq;

namespace ERPAPI.Models.LionGroupAppService
{
    public class AppRegisterModel : LionGroupAppServiceModel
    {
        #region - Definitions -
        private enum EnumTopicID
        {
            [Description("All")]
            ALL
        }
        #endregion

        #region - Constructor -
        public AppRegisterModel()
        {
            _entity = new EntityAppRegister(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public LionGroupAppRegister LionGroupAppRegister;
        #endregion

        #region - Private -
        private readonly EntityAppRegister _entity;
        #endregion

        #region - Validation -
        public bool GetValidUserAccountResult()
        {
            try
            {
                string userPWD = Security.Decrypt(LionGroupAppRegister.UserPWD, Security.EnumEncodeType.UTF8);

                Entity_BaseAP.UserAccountPara para = new Entity_BaseAP.UserAccountPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(LionGroupAppRegister.UserID) ? null : LionGroupAppRegister.UserID)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(userPWD) ? null : userPWD))
                };

                return new Entity_BaseAP(ConnectionStringERP, ProviderNameERP).ValidUserAccount(para);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 編輯使用者註冊資料 -
        /// <summary>
        /// 編輯使用者註冊資料
        /// </summary>
        /// <param name="updUser"></param>
        /// <returns></returns>
        public bool EditAppRegister(string updUser)
        {
            try
            {
                EntityAppRegister.AppRegisterUserInfoPara para = new EntityAppRegister.AppRegisterUserInfoPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.UserID) ? null : LionGroupAppRegister.UserID),
                    AppUUID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.UUID) ? null : LionGroupAppRegister.UUID),
                    DeviceTokenType = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.DeviceTokenType) ? null : LionGroupAppRegister.DeviceTokenType),
                    DeviceTokenID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.DeviceToken) ? null : LionGroupAppRegister.DeviceToken),
                    AppID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.AppID) ? null : LionGroupAppRegister.AppID),
                    MobileOS = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.OS) ? null : LionGroupAppRegister.OS),
                    MobileType = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppRegister.MobileType) ? null : LionGroupAppRegister.MobileType),
                    UpdUserID = new DBVarChar(updUser)
                };

                return _entity.EditAppRegister(para) == EntityAppRegister.EnumEditAppRegisterResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 新增集團同仁TopicID -
        /// <summary>
        /// 新增集團同仁TopicID
        /// </summary>
        public void CreateInstanceIDTopics()
        {
            try
            {
                var response = FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new[] {LionGroupAppRegister.DeviceToken }, Common.GetEnumDesc(EnumTopicID.ALL)).Result;

                if (response.FailureCount != 0)
                {
                    OnException(new Exception(JsonConvert.SerializeObject(response)));
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion
    }
}
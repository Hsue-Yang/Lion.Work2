// 新增日期：2016-12-23
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LionGroupAppService;

namespace ERPAPI.Models.LionGroupAppService
{
    public class LionGroupAppServiceModel : _BaseAPModel
    {
        #region - Constructor -
        public LionGroupAppServiceModel()
        {
            _entity = new EntityLionGroupAppService(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Private -
        private readonly EntityLionGroupAppService _entity;
        #endregion

        #region - Validation -
        public bool GetValidAppUserMobileResult(string userID, string appUUID)
        {
            try
            {
                return
                    _entity.SelectAppUserMobile(new EntityLionGroupAppService.AppUserMobilePara
                    {
                        UserID = new DBVarChar(userID),
                        AppUUID = new DBVarChar(appUUID)
                    }) != null;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public bool ValidLionGroupAppFunType(string appfunID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appfunID) == false)
                {
                    return Enum.IsDefined(typeof(LionTech.APIService.LionGroupApp.EnumLionGroupFunType), appfunID);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 紀錄使用者功能 -
        /// <summary>
        /// 紀錄使用者功能
        /// </summary>
        /// <param name="updUser"></param>
        /// <param name="ipAddress"></param>
        /// <param name="userID"></param>
        /// <param name="appUUID"></param>
        /// <param name="appFunID"></param>
        /// <returns></returns>
        public bool GetERPRecordAppUserFunResult(string updUser, string ipAddress, string userID, string appUUID, string appFunID)
        {
            try
            {
                var appUserFunPara = _GetRecordLogAppUserFunPara(updUser, ipAddress, userID, appUUID, appFunID);

                if (appUserFunPara != null)
                {
                    new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP)
                        .Insert(Mongo_BaseAP.EnumLogDocName.LOG_APP_USER_FUN, appUserFunPara);
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取得紀錄使用者功能參數 -
        /// <summary>
        /// 取得紀錄使用者功能參數
        /// </summary>
        /// <param name="updUser"></param>
        /// <param name="ipAddress"></param>
        /// <param name="userID"></param>
        /// <param name="appUUID"></param>
        /// <param name="appFunID"></param>
        /// <returns></returns>
        private Mongo_BaseAP.RecordLogAppUserFunPara _GetRecordLogAppUserFunPara(string updUser, string ipAddress, string userID, string appUUID, string appFunID)
        {
            var result = new Mongo_BaseAP.RecordLogAppUserFunPara();

            EntityLionGroupAppService.AppUserFunPara para = new EntityLionGroupAppService.AppUserFunPara
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                AppUUID = new DBVarChar(string.IsNullOrWhiteSpace(appUUID) ? null : appUUID),
                AppFunID = new DBVarChar(string.IsNullOrWhiteSpace(appFunID) ? null : appFunID)
            };

            var appUserFun = _entity.SelectAppUserFun(para);

            if (appUserFun != null)
            {
                result = new Mongo_BaseAP.RecordLogAppUserFunPara
                {
                    UserID = appUserFun.UserID,
                    AppUUID = appUserFun.AppUUID,
                    AppFunID = appUserFun.AppFunID,
                    RemindMinute = appUserFun.RemindMinute,
                    IsOpenPush = appUserFun.IsOpenPush,
                    UpdUserID = new DBVarChar(updUser),
                    UpdUserNM = new DBNVarChar(updUser),
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecIPAddress = new DBVarChar(ipAddress)
                };
            }

            return result;
        }
        #endregion
    }
}
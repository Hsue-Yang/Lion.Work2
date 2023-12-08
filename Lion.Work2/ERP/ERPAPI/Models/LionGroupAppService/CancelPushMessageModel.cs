// 新增日期：2016-12-28
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LionGroupAppService;

namespace ERPAPI.Models.LionGroupAppService
{
    public class CancelPushMessageModel : LionGroupAppServiceModel
    {
        #region - Constructor -
        public CancelPushMessageModel()
        {
            _entity = new EntityCancelPushMessage(ConnectionStringSERP, ProviderNameSERP);
            _mongoEntity = new MongoCancelPushMessage(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string MessageID { get; set; }
        public List<string> UserIDList { get; set; }
        #endregion

        #region - Private -
        private readonly EntityCancelPushMessage _entity;
        private readonly MongoCancelPushMessage _mongoEntity;
        private List<MongoCancelPushMessage.AppUserPush> _appUserPushList;
        #endregion

        #region - 取得取消推播排程清單 -
        /// <summary>
        /// 取得取消推播排程清單
        /// </summary>
        /// <returns></returns>
        public bool GetAppUserPushList()
        {
            try
            {
                MongoCancelPushMessage.AppUserPushPara para = new MongoCancelPushMessage.AppUserPushPara
                {
                    MessageID = new DBVarChar(MessageID),
                    UserIDList = Utility.ToDBTypeList<DBVarChar>(UserIDList)
                };

                _appUserPushList = _mongoEntity.SelectAppUserPushList(para);

                return _appUserPushList.Any();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取消推播 -
        /// <summary>
        /// 取消推播
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="apiNo"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool CancelAppPushMessage(string updUserID, string execSysID, string apiNo, string ipAddress)
        {
            try
            {
                var cancelPushMessagePara = _GetAppUserPushParaList(updUserID, execSysID, apiNo, ipAddress);

                if (cancelPushMessagePara.Any())
                {
                    _mongoEntity.Insert(Mongo_BaseAP.EnumLogDocName.LOG_APP_USER_PUSH, cancelPushMessagePara);
                }

                _mongoEntity.DeleteAppUserPushList(_appUserPushList);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取得取消推播參數 -
        /// <summary>
        /// 取得取消推播參數
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="apiNo"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private List<Mongo_BaseAP.RecordLogAppUserPushPara> _GetAppUserPushParaList(string updUserID, string execSysID, string apiNo, string ipAddress)
        {
            EntityLionGroupAppService.PushMessageUserInfoPara para = new EntityLionGroupAppService.PushMessageUserInfoPara
            {
                FunID = _appUserPushList.Select(f => f.AppFunID).FirstOrDefault(),
                UserIDs = _appUserPushList.Select(u => u.UserID).ToList()
            };

            var execSysNM = _GetSysNM(execSysID);

            var pushMessageUserInfoList = _entity.SelectPushMessageUserInfoList(para);

            return (from userPush in _appUserPushList
                    join s in pushMessageUserInfoList
                        on new
                        {
                            UserID = userPush.UserID.GetValue(),
                            AppUUID = userPush.AppUUID.GetValue(),
                            AppFunID = userPush.AppFunID.GetValue(),
                            AppRoleID = userPush.AppRoleID.GetValue()
                        } equals new
                        {
                            UserID = s.UserID.GetValue(),
                            AppUUID = s.AppUUID.GetValue(),
                            AppFunID = s.AppFunID.GetValue(),
                            AppRoleID = s.AppRoleID.GetValue()
                        }
                    select new Mongo_BaseAP.RecordLogAppUserPushPara
                    {
                        MessageID = userPush.MessageID,
                        UserID = userPush.UserID,
                        AppID = userPush.AppID,
                        AppUUID = userPush.AppUUID,
                        AppFunID = userPush.AppFunID,
                        AppRoleID = userPush.AppRoleID,
                        DeviceTokenID = s.RegistrationID,
                        MobileOS = s.Os,
                        RemindMinute = s.RemindMinute,
                        IsOpenPush = s.IsOpenPush,
                        PushDT = userPush.PushDT,
                        PushSts = new DBChar(EntityLionGroupAppService.EnumAppServicePushStatus.C),
                        Title = userPush.Title,
                        Body = userPush.Body,
                        Data = new Mongo_BaseAP.PushMsgData
                        {
                            SourceID = userPush.Data.SourceID,
                            SourceType = userPush.Data.SourceType
                        },
                        APINo = new DBChar(apiNo),
                        UpdUserID = new DBVarChar(updUserID),
                        UpdUserNM = new DBNVarChar(updUserID),
                        UpdDT = new DBDateTime(DateTime.Now),
                        ExecSysID = new DBVarChar(execSysID),
                        ExecSysNM = execSysNM,
                        ExecIPAddress = new DBVarChar(ipAddress)
                    }).ToList();
        }
        #endregion

        #region - 取得系統名稱 -
        /// <summary>
        /// 取得系統名稱
        /// </summary>
        /// <param name="SysID"></param>
        /// <returns></returns>
        private DBNVarChar _GetSysNM(string SysID)
        {
            Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                new Entity_BaseAP.BasicInfoPara(EnumCultureID.zh_TW.ToString())
                {
                    ExecSysID = new DBVarChar(SysID)
                };
            Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                .SelectBasicInfo(entityBasicInfoPara);

            return entityBasicInfo.ExecSysNM;
        }
        #endregion
    }
}
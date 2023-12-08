// 新增日期：2017-01-04
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP.LionGroupAppService;
using LionTech.Utility;

namespace ERPAPI.Models.LionGroupAppService
{
    public class OpenPushModel : LionGroupAppServiceModel
    {
        #region - Constructor -
        public OpenPushModel()
        {
            _entity = new EntityOpenPush(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public LionGroupAppOpenPush LionGroupAppOpenPush;
        #endregion

        #region - Private -
        private readonly EntityOpenPush _entity;
        #endregion

        #region - 編輯是否開啟推播 -
        public bool EditOpenPush(string updUser)
        {
            try
            {
                EntityOpenPush.OpenPushPara para = new EntityOpenPush.OpenPushPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppOpenPush.UserID) ? null : LionGroupAppOpenPush.UserID),
                    AppUUID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppOpenPush.UUID) ? null : LionGroupAppOpenPush.UUID),
                    AppFunID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppOpenPush.FunID) ? null : LionGroupAppOpenPush.FunID),
                    RemindMinute = new DBInt(Common.GetEnumDesc(EntityOpenPush.EnumAppPushRemindMinute.Five)),
                    IsOpenPush = new DBChar((LionGroupAppOpenPush.IsOpen) ? EnumYN.Y : EnumYN.N),
                    UpdUserID = new DBVarChar(updUser)
                };
                
                return _entity.EditOpenPush(para) == EntityOpenPush.EnumEditOpenPushResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion
    }
}
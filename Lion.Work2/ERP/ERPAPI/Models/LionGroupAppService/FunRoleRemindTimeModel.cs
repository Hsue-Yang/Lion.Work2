// 新增日期：2017-01-05
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LionGroupAppService;

namespace ERPAPI.Models.LionGroupAppService
{
    public class FunRoleRemindTimeModel : LionGroupAppServiceModel
    {
        #region - Constructor -
        public FunRoleRemindTimeModel()
        {
            _entity = new EntityFunRoleRemindTime(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public LionGroupAppFunRoleRemindTime LionGroupAppFunRoleRemindTime;
        #endregion

        #region - Private -
        private readonly EntityFunRoleRemindTime _entity;
        #endregion

        #region - Validation -
        public bool GetAppFunRoleResult()
        {
            try
            {
                if (LionGroupAppFunRoleRemindTime.Roles == null ||
                    LionGroupAppFunRoleRemindTime.Roles.Any() == false)
                {
                    return true;
                }

                var appFunRoles =
                    _entity.SelectAppFunRoleList(new EntityFunRoleRemindTime.AppFunRolePara
                    {
                        AppFunID = new DBVarChar(LionGroupAppFunRoleRemindTime.FunID)
                    });

                return LionGroupAppFunRoleRemindTime.Roles.All(role => appFunRoles.Any(w => role == w.AppRoleID.GetValue()));
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 編輯使用者功能 -
        /// <summary>
        /// 編輯使用者功能
        /// </summary>
        /// <param name="updUser"></param>
        /// <returns></returns>
        public bool EditFunRoleRemindTime(string updUser)
        {
            try
            {
                EntityFunRoleRemindTime.FunRoleRemindTimePara para = new EntityFunRoleRemindTime.FunRoleRemindTimePara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppFunRoleRemindTime.UserID) ? null : LionGroupAppFunRoleRemindTime.UserID),
                    AppUUID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppFunRoleRemindTime.UUID) ? null : LionGroupAppFunRoleRemindTime.UUID),
                    AppFunID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppFunRoleRemindTime.FunID) ? null : LionGroupAppFunRoleRemindTime.FunID),
                    RemindMinute = new DBInt(LionGroupAppFunRoleRemindTime.RemindTime),
                    UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(updUser) ? null : updUser),
                    AppRoleIDList = Utility.ToDBTypeList<DBVarChar>(LionGroupAppFunRoleRemindTime.Roles)
                };
                
                return _entity.EditFunRoleRemindTime(para) == EntityFunRoleRemindTime.EnumEditFunRoleRemindTimeResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 紀錄使用者功能角色 -
        /// <summary>
        /// 紀錄使用者功能角色
        /// </summary>
        /// <param name="updUser"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool GetERPRecordAppUserFunRoleResult(string updUser, string ipAddress)
        {
            try
            {
                var appUserFunRolePara = _GetRecordLogAppUserFunRolePara(updUser, ipAddress);

                if (appUserFunRolePara != null &&
                    appUserFunRolePara.Any())
                {
                    new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP)
                        .Insert(Mongo_BaseAP.EnumLogDocName.LOG_APP_USER_FUN_ROLE, appUserFunRolePara);
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

        #region - 取得記錄使用者功能角色參數 -
        /// <summary>
        /// 取得記錄使用者功能角色參數
        /// </summary>
        /// <param name="updUser"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private List<Mongo_BaseAP.RecordLogAppUserFunRolePara> _GetRecordLogAppUserFunRolePara(string updUser, string ipAddress)
        {
            var result = new List<Mongo_BaseAP.RecordLogAppUserFunRolePara>();

            EntityFunRoleRemindTime.AppUserFunRolePara para = new EntityFunRoleRemindTime.AppUserFunRolePara
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppFunRoleRemindTime.UserID) ? null : LionGroupAppFunRoleRemindTime.UserID),
                AppFunID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppFunRoleRemindTime.FunID) ? null : LionGroupAppFunRoleRemindTime.FunID),
                AppUUID = new DBVarChar(string.IsNullOrWhiteSpace(LionGroupAppFunRoleRemindTime.UUID) ? null : LionGroupAppFunRoleRemindTime.UUID)
            };

            var appUserFunRoleList = _entity.SelectAppUserFunRoleList(para);

            if (appUserFunRoleList != null &&
                appUserFunRoleList.Any())
            {
                result.AddRange((from role in appUserFunRoleList
                                 select new Mongo_BaseAP.RecordLogAppUserFunRolePara
                                 {
                                     UserID = role.UserID,
                                     AppUUID = role.AppUUID,
                                     AppFunID = role.AppFunID,
                                     AppRoleID = role.AppRoleID,
                                     UpdUserID = new DBVarChar(updUser),
                                     UpdUserNM = new DBNVarChar(updUser),
                                     UpdDT = new DBDateTime(DateTime.Now),
                                     ExecIPAddress = new DBVarChar(ipAddress)
                                 }));
            }

            return result;
        }
        #endregion

    }
}
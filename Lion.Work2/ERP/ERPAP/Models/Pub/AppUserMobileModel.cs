// 新增日期：2017-01-06
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Pub;
using Resources;

namespace ERPAP.Models.Pub
{
    public class AppUserMobileModel : PubModel
    {
        #region - Definitions -
        public enum EnumAppID
        {
            LionGroupApp
        }

        public class AppUserDevice
        {
            public string IsMaster { get; set; }
            public string AppUUID { get; set; }
            public string DeviceTokenID { get; set; }
        }
        #endregion

        #region - Constructor -
        public AppUserMobileModel()
        {
            _entity = new EntityAppUserMobile(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public List<AppUserDevice> IsMasterCheckList { get; set; }

        public List<EntityAppUserMobile.AppUserMobile> AppUserMobileList { get; private set; }

        private Dictionary<string, string> _appIDDic;

        public Dictionary<string, string> AppIDDic
        {
            get
            {
                return _appIDDic ?? (_appIDDic = new Dictionary<string, string>
                {
                    { EnumAppID.LionGroupApp.ToString(), PubAppUserMobile.Label_LionGroupApp }
                });
            }
        }
        #endregion

        #region - Private -
        private readonly EntityAppUserMobile _entity;
        #endregion

        #region - 取得使用者行動裝置清單 -
        /// <summary>
        /// 取得使用者行動裝置清單
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public bool GetAppUserMobileList(string userID, EnumCultureID cultureId)
        {
            try
            {
                EntityAppUserMobile.AppUserMobilePara para = new EntityAppUserMobile.AppUserMobilePara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                };

                AppUserMobileList = _entity.SelectAppUserMobileList(para);

                GetCMCodeDictionary(cultureId, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.IOSType);

                var iosTypeDic = (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.IOSType].Cast<Entity_BaseAP.CMCode>()
                          select new
                          {
                              CodeID = s.CodeID.GetValue(),
                              CodeNM = s.CodeNM.GetValue()
                          }).ToDictionary(k => k.CodeID, v => v.CodeNM);

                AppUserMobileList = (from r in AppUserMobileList
                                     let mobileType = iosTypeDic.ContainsKey(r.MobileType.GetValue()) ? iosTypeDic[r.MobileType.GetValue()] : r.MobileType.GetValue()
                                     select new EntityAppUserMobile.AppUserMobile
                                     {
                                         AppUUID = r.AppUUID,
                                         DeviceTokenID = r.DeviceTokenID,
                                         AppID = r.AppID,
                                         MobileOS = r.MobileOS,
                                         MobileType = new DBVarChar(mobileType),
                                         IsMaster = r.IsMaster,
                                         UpdDT = r.UpdDT
                                     }).ToList();

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 更新使用者裝置-
        /// <summary>
        /// 更新使用者裝置
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UpdateAppUserDevice(string userID)
        {
            try
            {
                if (IsMasterCheckList != null &&
                    IsMasterCheckList.Any())
                {
                    var isMasterCheckList = IsMasterCheckList.Where(m => m.IsMaster == EnumYN.Y.ToString());

                    EntityAppUserMobile.AppUserDevicePara para = new EntityAppUserMobile.AppUserDevicePara
                    {
                        UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                        AppUserDeviceList = (from s in isMasterCheckList
                                             select new EntityAppUserMobile.Device
                                             {
                                                 AppUUID = new DBVarChar(s.AppUUID),
                                                 DeviceTokenID = new DBVarChar(s.DeviceTokenID)
                                             }).ToList()
                    };

                    return _entity.UpdateAppUserDevice(para) == EntityAppUserMobile.EnumUpdateAppUserDeviceResult.Success;
                }
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
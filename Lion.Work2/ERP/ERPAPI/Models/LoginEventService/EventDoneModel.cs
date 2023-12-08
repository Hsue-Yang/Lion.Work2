// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.Entity;
using LionTech.Entity.ERP.LoginEventService;
using LionTech.Utility;

namespace ERPAPI.Models.LoginEventService
{
    public class EventDoneModel : LoginEventServiceModel
    {
        #region - Constructor -
        public EventDoneModel()
        {
            _entity = new EntityEventDone(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string SysID { get; set; }
        public string EvnetID { get; set; }
        public string UserID { get; set; }
        public string DoneDateTime { get; set; }
        #endregion

        #region - Private -
        private readonly EntityEventDone _entity;
        #endregion

        #region - 編輯使用者登入事件 -
        public bool EditEventDone(string updUserID)
        {
            try
            {
                EntityEventDone.EventDonePara para = new EntityEventDone.EventDonePara
                {
                    UserID = new DBVarChar(UserID),
                    SysID = new DBVarChar(SysID),
                    LoginEventID = new DBVarChar(EvnetID),
                    DoneDT = new DBDateTime(Common.GetDateTime(Common.FormatDateTimeString(DoneDateTime))),
                    UpdUserID = new DBVarChar(updUserID)
                };

                return _entity.EditEventDone(para) == EntityEventDone.EnumEditEventDoneResult.Success;
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
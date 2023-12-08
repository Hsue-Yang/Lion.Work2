using System;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPOpagm20Model : SubscriberModel
    {
        #region - Event Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string EDIEventNo { get; set; }
        public string EventPara { get; set; }
        #endregion

        public class EventParaData
        {
            public string stfn_stfn { get; set; }
            public string stfn_cname { get; set; }
            public string stfn_prof { get; set; }
            public string stfn_sts { get; set; }
            public string stfn_comp { get; set; }
            public string stfn_team { get; set; }
            public string stfn_job1 { get; set; }
            public string stfn_job2 { get; set; }
            public string stfn_email { get; set; }
        }

        public EventParaData EventData { get; set; }

        public bool EditRawCMUser(string apiNo, string ipAddress, string execSysID)
        {
            try
            {
                EntityERPAPOpagm20.RawCMUserPara para = new EntityERPAPOpagm20.RawCMUserPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_stfn) ? null : EventData.stfn_stfn)),
                    UserNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.stfn_cname) ? null : EventData.stfn_cname)),
                    UserEMail = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_email) ? null : EventData.stfn_email)),
                    UserComID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_comp) ? null : EventData.stfn_comp)),
                    UserUnitID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_prof) ? null : EventData.stfn_prof)),
                    UserTeamID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_team) ? null : EventData.stfn_team)),
                    UserTitleID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_job2) ? null : EventData.stfn_job2)),
                    UserWorkID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_job1) ? null : EventData.stfn_job1)),
                    IsLeft = new DBChar(EventData.stfn_sts.Trim() == "1" ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    ApiNO = new DBChar(apiNo),
                    IPAddress = new DBVarChar(ipAddress),
                    ExecSysID = new DBVarChar(execSysID),
                    UpdEDIEventNo = new DBChar((string.IsNullOrWhiteSpace(EDIEventNo) ? null : EDIEventNo))
                };

                if (new EntityERPAPOpagm20(ConnectionStringSERP, ProviderNameSERP)
                    .EditRawCMUser(para) == EntityERPAPOpagm20.EnumEditRawCMUserResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public bool DeleteRawCMUser()
        {
            try
            {
                EntityERPAPOpagm20.RawCMUserPara para = new EntityERPAPOpagm20.RawCMUserPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.stfn_stfn) ? null : EventData.stfn_stfn))
                };

                if (new EntityERPAPOpagm20(ConnectionStringSERP, ProviderNameSERP)
                        .DeleteRawCMUser(para) == EntityERPAPOpagm20.EnumDeleteRawCMUserResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}
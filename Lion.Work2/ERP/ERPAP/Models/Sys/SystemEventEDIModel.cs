using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemEventEDIModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID, QueryTargetSysID, QueryEventGroupID, QueryEventID,
            DTBegin, DTEnd, IsOnlyFail
        }

        public enum EnumResultID
        {
            S, F
        }
        #endregion

        #region - Class -
        public class SystemEventEDI
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string EventGroupID { get; set; }
            public string EventGroupNM { get; set; }

            public string EventID { get; set; }
            public string EventNM { get; set; }

            public string EDIEventNo { get; set; }

            public string StatusID { get; set; }
            public string StatusNM { get; set; }
            public string ResultID { get; set; }
            public string ResultNM { get; set; }

            public string InsertEDINo { get; set; }
            public string InsertEDIDate { get; set; }
            public string InsertEDITime { get; set; }
            public string ExecEDIEventNo { get; set; }

            public string TargetSysID { get; set; }
            public string TargetSysNM { get; set; }

            public string TargetStatusID { get; set; }
            public string TargetStatusNM { get; set; }
            public string TargetResultID { get; set; }
            public string TargetResultNM { get; set; }

            public DateTime? TargetDTBegin { get; set; }
            public DateTime? TargetDTEnd { get; set; }
        }
        #endregion

        #region - Property -
        public string QuerySysID { get; set; }

        public string QueryTargetSysID { get; set; }

        public string QueryEventGroupID { get; set; }

        public string QueryEventID { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DTBegin { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DTEnd { get; set; }

        public string IsOnlyFail { get; set; }

        public List<SystemEventEDI> SystemEventEDIList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
            QueryTargetSysID = string.Empty;
            DTBegin = Common.GetDateString();
            DTEnd = Common.GetDateString();
        }
        #endregion

        public async Task<bool> GetSystemEventEDIList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEventEDIs(QuerySysID, userID, QueryTargetSysID, QueryEventGroupID, QueryEventID, DTBegin, DTEnd, IsOnlyFail, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemEventEDIs = (List<SystemEventEDI>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemEventEDIList = responseObj.SystemEventEDIs;

                    SetPageCount();
                }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
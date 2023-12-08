using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class UserConnectModel : SysModel
    {
        #region - Class -
        public class UserConnectAPI
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string LastConnectDT { get; set; }
            public string CustLogout { get; set; }
            public string IPAddress { get; set; }
        }
        #endregion
        public enum Field
        {
            DateBegin, DateEnd, TimeBegin, TimeEnd
        }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DateBegin { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DateEnd { get; set; }

        [Required]
        public string TimeBegin { get; set; }

        [Required]
        public string TimeEnd { get; set; }

        public Dictionary<string, string> BeginTimeList = new Dictionary<string, string>()
        {
            {"000000000", "00:00"},
            {"010000000", "01:00"},
            {"020000000", "02:00"},
            {"030000000", "03:00"},
            {"040000000", "04:00"},
            {"050000000", "05:00"},
            {"060000000", "06:00"},
            {"070000000", "07:00"},
            {"080000000", "08:00"},
            {"090000000", "09:00"},
            {"100000000", "10:00"},
            {"110000000", "11:00"},
            {"120000000", "12:00"},
            {"130000000", "13:00"},
            {"140000000", "14:00"},
            {"150000000", "15:00"},
            {"160000000", "16:00"},
            {"170000000", "17:00"},
            {"180000000", "18:00"},
            {"190000000", "19:00"},
            {"200000000", "20:00"},
            {"210000000", "21:00"},
            {"220000000", "22:00"},
            {"230000000", "23:00"}
        };

        public Dictionary<string, string> EndTimeList = new Dictionary<string, string>()
        {
            {"005959999", "00:59"},
            {"015959999", "01:59"},
            {"025959999", "02:59"},
            {"035959999", "03:59"},
            {"045959999", "04:59"},
            {"055959999", "05:59"},
            {"065959999", "06:59"},
            {"075959999", "07:59"},
            {"085959999", "08:59"},
            {"095959999", "09:59"},
            {"105959999", "10:59"},
            {"115959999", "11:59"},
            {"125959999", "12:59"},
            {"135959999", "13:59"},
            {"145959999", "14:59"},
            {"155959999", "15:59"},
            {"165959999", "16:59"},
            {"175959999", "17:59"},
            {"185959999", "18:59"},
            {"195959999", "19:59"},
            {"205959999", "20:59"},
            {"215959999", "21:59"},
            {"225959999", "22:59"},
            {"235959999", "23:59"}
        };

        public UserConnectModel()
        {
        }

        public void FormReset()
        {
            this.DateBegin = Common.GetDateString();
            this.TimeBegin = "000000000";
            this.DateEnd = Common.GetDateString();
            this.TimeEnd = "235959999";
        }

        public List<UserConnectAPI> UserConnectLists { get; private set; }

        public async Task<bool> GetUserConnectList(string userID, int pageSize)
        {
            try
            {
                var connectDTBegin = Convert.ToDateTime(Common.FormatDateTimeString(this.DateBegin + this.TimeBegin)).ToString("yyyy-MM-dd HH:mm:ss");
                var connectDTEnd = Convert.ToDateTime(Common.FormatDateTimeString(this.DateEnd + this.TimeEnd)).ToString("yyyy-MM-dd HH:mm:ss");

                string response = string.Empty;
                if (connectDTBegin != null & connectDTEnd != null)
                {
                    string apiUrl = API.UserConnect.QueryUserConnectList(userID, connectDTBegin, connectDTEnd, PageIndex, pageSize);
                    response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                }

                var responseObj = new
                {
                    RowCount = 0,
                    UserConnectLists = (List<UserConnectAPI>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    UserConnectLists = responseObj.UserConnectLists;
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
using LionTech.Utility;
using LionTech.Utility.TRAINING;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TRAININGAP.Models.Leave
{
    public class LeaveIndexModel : LeaveModel
    {
        public List<Leave> LeaveList { get; private set; }
        public class Leave
        {
            public int ppm96_id { get; set; }
            public string ppm96_stfn { get; set; }
            public DateTime ppm96_begin { get; set; }
            public DateTime ppm96_end { get; set; }
            public string ppm96_sign_string { get; set; }
            public string ppm96_beginFormatted { get; set; }
            public string ppm96_endFormatted { get; set; }
            public string ppm95_name { get; set; }
            public string ppd95_name { get; set; }
        }

        public async Task<bool> GetLeaveList(string userID)
        {
            try
            {
                string apiUrl = API.Leave.GetLeave(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                List<Leave> responseObj = Common.GetJsonDeserializeObject<List<Leave>>(response);

                if (responseObj != null)
                {
                    foreach (var leave in responseObj)
                    {
                        leave.ppm96_beginFormatted = leave.ppm96_begin.ToString("yyyy-MM-dd HH:mm");
                        leave.ppm96_endFormatted = leave.ppm96_end.ToString("yyyy-MM-dd HH:mm");
                    }
                    LeaveList = responseObj;
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
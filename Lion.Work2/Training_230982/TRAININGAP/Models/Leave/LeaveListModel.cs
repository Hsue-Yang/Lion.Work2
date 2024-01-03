using LionTech.Utility;
using LionTech.Utility.TRAINING;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRAININGAP.Models.Leave
{
    public class LeaveListModel : LeaveModel
    {
        #region - Class -
        public class LeaveList
        {
            public int ppm96_id { get; set; }
            public string ppm96_stfn { get; set; } //員編
            public string ppm96_begin { get; set; } //開始時間
            public string ppm96_end { get; set; } //結束時間
            public string ppm96_sign { get; set; } //送審主管
            public string ppm95_name { get; set; } //假別名稱
            public string ppd95_name { get; set; } //假別子項目名稱
            public List<int> ppm96_signList { get; set; }
        }
        #endregion

        //Property
        public List<LeaveList> LeaveModel { get; set; }

        public async Task<bool> GetLeaveList(string userID)
        {
            try
            {
                string apiUrl = API.PracticeLeave.GetLeaveList(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new List<LeaveList>();

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    LeaveDetailModel dm = new LeaveDetailModel();
                    foreach (var leave in responseObj)
                    {
                        leave.ppm96_signList = ConvertTo2Ary(leave.ppm96_sign);
                        var signNames = leave.ppm96_signList.Select(sign => dm.SignDic.ContainsKey(sign) ? dm.SignDic[sign] : "");
                        leave.ppm96_sign = string.Join(",", signNames);
                    }

                    LeaveModel = responseObj;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}
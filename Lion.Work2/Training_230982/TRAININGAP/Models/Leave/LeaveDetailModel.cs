using LionTech.Entity;
using LionTech.Utility;
using LionTech.Utility.TRAINING;
using LionTech.Web.ERPHelper;
using Microsoft.SqlServer.Server;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using static LionTech.Entity.TRAINING.Leave.EntityLeaveList;
using static TRAININGAP.Models.Leave.LeaveDetailModel;

namespace TRAININGAP.Models.Leave
{
    public class LeaveDetailModel : LeaveListModel
    {
        public LeaveDetailModel() { }

        #region -Property-
        public int Ppm96_id { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBox)]
        public string Ppm96_stfn { get; set; } //員編
        [Required]
        public string Ppm96_beginDate { get; set; } //開始日期
        [Required]
        public string Ppm96_beginTime { get; set; } //開始時間
        [Required]
        public string Ppm96_endDate { get; set; } //結束日期
        [Required]
        public string Ppm96_endTime { get; set; } //結束時間
        [Required]
        public string Ppm95_id { get; set; } //假別名稱
        public string Ppd95_id { get; set; } //假別子項目名稱
        [Required(ErrorMessage = "至少選擇一個送審主管")]
        public string ppm96_sign { get; set; } //送審主管
        #endregion

        public class LeaveDetails
        {
            public string Ppm96_stfn { get; set; } //員編

            public string Ppm96_begin { get; set; } //開始時間

            public string Ppm96_end { get; set; } //結束時間

            public string Ppm95_id { get; set; } //假別名稱
            public string Ppd95_id { get; set; } //假別子項目名稱
            public string? ppm96_sign { get; set; }
        }

        public class Ppm95_idList : DBEntity.ISelectItem
        {
            public string ppm95_id { get; set; }
            public string ppm95_name { get; set; }
            public string ItemText()
            {
                return $"{ppm95_name}";
            }

            public string ItemValue()
            {
                return ppm95_id;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }
        public class Ppd95_idList : DBEntity.ISelectItem
        {
            public string ppd95_id { get; set; }
            public string ppd95_name { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{ppd95_name}";
            }

            public string ItemValue()
            {
                return ppd95_id;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        private enum EnumSignDirector
        {
            Director = 1,
            Department = 2,
            Office = 4,
            Group = 8
        }
        private Dictionary<int, string> _signDic;
        public Dictionary<int, string> SignDic
        {
            get
            {
                return _signDic ?? (_signDic = new Dictionary<int, string>()
                {
                    { (int)EnumSignDirector.Director, PracticeLeave.sign_director },
                    { (int) EnumSignDirector.Department, PracticeLeave.sign_deparment },
                    { (int) EnumSignDirector.Office, PracticeLeave.sign_office },
                    {  (int) EnumSignDirector.Group, PracticeLeave.sign_group }
                });
            }
        }

        public List<Ppm95_idList> Ppm95_List { get; set; }
        public List<Ppd95_idList> Ppd95_List { get; set; }
        public List<int> Ppm96_signList { get; set; }


        public void FormReset()
        {
            Ppm96_stfn = string.Empty;
            Ppm96_beginDate = string.Empty;
            Ppm96_endDate = string.Empty;
            ppm96_sign = string.Empty; //要修改
        }

        public async Task<bool> GetPpm95List(string userID)
        {
            try
            {
                string apiUrl = API.PracticeLeave.GetPpm95List(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new List<Ppm95_idList>();

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    Ppm95_List = responseObj;
                }

                return true;
            }
            catch (Exception ex) { OnException(ex); }

            return false;
        }

        public async Task<bool> GetPpd95List(string userID, string ppm95_id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ppm95_id))
                {
                    Ppd95_List = new List<Ppd95_idList>();
                    return true;
                }
                string apiUrl = API.PracticeLeave.GetPpd95List(userID, ppm95_id);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new List<Ppd95_idList>();

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    Ppd95_List = responseObj;
                    return true;
                }

            }
            catch (Exception ex) { OnException(ex); }

            return false;
        }
        public async Task<bool> GetLeaveDetail(string userID)
        {
            try
            {
                string apiUrl = API.PracticeLeave.GetLeaveData(userID, Ppm96_id);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new LeaveDetails();

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    Ppm96_stfn = responseObj.Ppm96_stfn;
                    DateTime beginDateTime = DateTime.ParseExact(responseObj.Ppm96_begin, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                    Ppm96_beginDate = beginDateTime.ToString("yyyy/MM/dd");
                    Ppm96_beginTime = beginDateTime.ToString("HH:mm");
                    DateTime endDateTime = DateTime.ParseExact(responseObj.Ppm96_end, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                    Ppm96_endDate = endDateTime.ToString("yyyy/MM/dd");
                    Ppm96_endTime = endDateTime.ToString("HH:mm");
                    Ppm95_id = Ppm95_List.FirstOrDefault(item => item.ppm95_id == responseObj.Ppm95_id)?.ppm95_name;
                    Ppd95_id = Ppd95_List.FirstOrDefault(item => item.ppd95_id == responseObj.Ppm95_id)?.ppd95_name;
                    //Ppm96_signList = ConvertTo2Ary(Convert.ToInt32(responseObj.ppm96_sign)).ToList();
                    Ppm96_signList = ConvertTo2Ary(responseObj.ppm96_sign) ?? null;
                }

                return true;
            }
            catch (Exception ex) { OnException(ex); }

            return false;
        }
        public async Task<bool> SubmitFormData(string userID)
        {
            try
            {

                LeaveDetails para = new LeaveDetails();
                string[] formats = { "yyyy/MM/dd HH:mm", "yyyyMMdd HHmm", "yyyy/MM/dd HHmm", "yyyyMMdd HH:mm" };
                if (!string.IsNullOrWhiteSpace(Ppm96_beginDate) && !string.IsNullOrWhiteSpace(Ppm96_beginTime))
                {
                    string dateTimeStr = $"{Ppm96_beginDate} {Ppm96_beginTime}";
                    DateTime.TryParseExact(dateTimeStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime combinedDateTime);
                    para.Ppm96_begin = combinedDateTime.ToString("yyyy/MM/dd HH:mm");
                }

                if (!string.IsNullOrWhiteSpace(Ppm96_endDate) && !string.IsNullOrWhiteSpace(Ppm96_endTime))
                {
                    string dateTimeStr = $"{Ppm96_endDate} {Ppm96_endTime}";
                    DateTime.TryParseExact(dateTimeStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime combinedDateTime);
                    para.Ppm96_end = combinedDateTime.ToString("yyyy/MM/dd HH:mm");
                }

                para.Ppm96_stfn = string.IsNullOrWhiteSpace(Ppm96_stfn) ? null : Ppm96_stfn;
                para.Ppd95_id = string.IsNullOrWhiteSpace(Ppd95_id) ? null : Ppd95_id;
                para.Ppm95_id = string.IsNullOrWhiteSpace(Ppm95_id) ? null : Ppm95_id;

                if (Ppm96_signList != null)
                {

                    para.ppm96_sign = ConvertToInt(Ppm96_signList);
                }
                else
                {
                    para.ppm96_sign = null;
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.PracticeLeave.SubmitFormData(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex); return false;
            }
        }
    }
}
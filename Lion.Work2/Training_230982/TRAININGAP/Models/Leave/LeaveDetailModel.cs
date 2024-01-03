using LionTech.Entity;
using LionTech.Utility;
using LionTech.Utility.TRAINING;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TRAININGAP.Models.Leave
{
    public class LeaveDetailModel : LeaveListModel
    {
        public LeaveDetailModel() { }

        #region -Property-
        public int Ppm96_id { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string Ppm96_stfn { get; set; } //員編
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxInterval)]
        public string Ppm96_beginDate { get; set; } //開始日期
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Ppm96_beginTime { get; set; } //開始時間
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxInterval)]
        public string Ppm96_endDate { get; set; } //結束日期
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Ppm96_endTime { get; set; } //結束時間
        [Required]
        public string Ppm95_id { get; set; } //假別名稱
        public string Ppd95_id { get; set; } //假別子項目名稱
        public string ppm96_sign { get; set; } //送審主管
        #endregion

        public class LeaveDetails
        {
            public int Ppm96_id { get; set; }
            public string Ppm96_stfn { get; set; } //員編

            public string Ppm96_begin { get; set; } //開始時間

            public string Ppm96_end { get; set; } //結束時間

            public string Ppm95_id { get; set; } //假別名稱
            public string Ppd95_id { get; set; } //假別子項目名稱
            public string ppm96_sign { get; set; }
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
                    { (int)EnumSignDirector.Director, LeaveLeave.sign_director },
                    { (int) EnumSignDirector.Department, LeaveLeave.sign_deparment },
                    { (int) EnumSignDirector.Office, LeaveLeave.sign_office },
                    { (int) EnumSignDirector.Group, LeaveLeave.sign_group }
                });
            }
        }

        public List<Ppm95_idList> Ppm95_List { get; set; }
        public List<Ppd95_idList> Ppd95_List { get; set; }
        public List<int> Ppm96_signList { get; set; }

        public async Task<bool> GetPpm95List()
        {
            try
            {
                string apiUrl = API.PracticeLeave.GetPpm95List();
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

        public async Task<bool> GetPpd95List(string ppm95_id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ppm95_id))
                {
                    Ppd95_List = new List<Ppd95_idList>();
                    return true;
                }
                string apiUrl = API.PracticeLeave.GetPpd95List(ppm95_id);
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
        public async Task<bool> GetLeaveDetail()
        {
            try
            {
                string apiUrl = API.PracticeLeave.GetLeaveData(Ppm96_id);

                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new LeaveDetails();

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    Ppm96_id = responseObj.Ppm96_id;
                    Ppm96_stfn = responseObj.Ppm96_stfn;
                    DateTime beginDateTime = DateTime.ParseExact(responseObj.Ppm96_begin, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                    Ppm96_beginDate = beginDateTime.ToString("yyyyMMdd");
                    Ppm96_beginTime = beginDateTime.ToString("HHmm");
                    DateTime endDateTime = DateTime.ParseExact(responseObj.Ppm96_end, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                    Ppm96_endDate = endDateTime.ToString("yyyyMMdd");
                    Ppm96_endTime = endDateTime.ToString("HHmm");
                    Ppm95_id = Ppm95_List.FirstOrDefault(item => item.ppm95_id == responseObj.Ppm95_id)?.ppm95_id;
                    await GetPpd95List(responseObj.Ppm95_id);
                    Ppd95_id = responseObj.Ppd95_id != null ? Ppd95_List.FirstOrDefault(item => item.ppd95_id == responseObj.Ppd95_id)?.ppd95_id : null;
                    Ppm96_signList = ConvertTo2Ary(responseObj.ppm96_sign) ?? null;
                }

                return true;
            }
            catch (Exception ex) { OnException(ex); }

            return false;
        }
        public async Task<bool> EditLeaveData()
        {
            try
            {
                LeaveDetails para = new LeaveDetails();

                var beginDT = Convert.ToDateTime(Common.FormatLongDateTimeString(Ppm96_beginDate + Ppm96_beginTime)).ToString("yyyy-MM-dd HH:mm");
                var endDT = Convert.ToDateTime(Common.FormatLongDateTimeString(Ppm96_endDate + Ppm96_endTime)).ToString("yyyy-MM-dd HH:mm");

                para.Ppm96_begin = beginDT;
                para.Ppm96_end = endDT;
                para.Ppm96_id = Ppm96_id;
                para.Ppm96_stfn = Ppm96_stfn;
                para.Ppd95_id = string.IsNullOrWhiteSpace(Ppd95_id) ? null : Ppd95_id;
                para.Ppm95_id = Ppm95_id;
                para.ppm96_sign = ConvertToInt(Ppm96_signList);

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.PracticeLeave.EditLeaveData();
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex); return false;
            }
        }

        public async Task<bool> DeleteLeaveData()
        {
            try
            {
                LeaveDetails para = new LeaveDetails();
                para.Ppm96_id = Ppm96_id != 0 ? Ppm96_id : 0;

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.PracticeLeave.DeleteLeaveData(Ppm96_id);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);

                return true;
            }
            catch (Exception ex) { OnException(ex); }

            return false;
        }
    }
}
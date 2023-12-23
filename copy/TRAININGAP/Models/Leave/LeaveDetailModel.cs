using LionTech.Entity;
using LionTech.Utility;
using LionTech.Utility.TRAINING;
using LionTech.Web.ERPHelper;
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
    public class LeaveDetailModel : LeaveModel
    {
        public class Leave
        {
            public int ppm96_id { get; set; }
            public string ppm96_stfn { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
            public DateTime ppm96_begin { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
            public DateTime ppm96_end { get; set; }
            public string ppm95_id { get; set; }
            public string ppd95_id { get; set; }
            public string ppm96_sign_string { get; set; }
        }

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        [Required]
        public string ppm96_stfn { get; set; }

        [Required]
        public List<string> options { get; set; }
        public string ppd95_id { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxDatePicker)]
        public string ppm96_EndDate { get; set; }
        public int ppm96_id { get; set; }

        [Required]
        public string ppm96_EndTime { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxDatePicker)]
        public string ppm96_beginDate { get; set; }
        [Required]
        public string ppm96_beginTime { get; set; }

        public List<LeaveCategory> LeaveCategoryByIdList { get; private set; }
        [Required]
        public string ppm95_id { get; set; }
        public List<LeaveChildCategory> LeaveChildCategoryByIdList { get; private set; }

        public void FormReset()
        {
            options = new List<string>();
        }

        public async Task<bool> GetLeaveDetail(string userID)
        {
            try
            {
                string apiUrl = API.Leave.GetLeaveDetail(ppm96_id, userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                Leave leaveObj = Common.GetJsonDeserializeObject<Leave>(response);
                if (leaveObj != null)
                {
                    ppm96_id = leaveObj.ppm96_id;
                    ppm96_stfn = leaveObj.ppm96_stfn;
                    ppm95_id = leaveObj.ppm95_id;
                    ppd95_id = leaveObj.ppd95_id;
                    ppm96_beginDate = leaveObj.ppm96_begin.ToString("yyyyMMdd");
                    ppm96_beginTime = leaveObj.ppm96_begin.ToString("HH:mm");
                    ppm96_EndDate = leaveObj.ppm96_end.ToString("yyyyMMdd");
                    ppm96_EndTime = leaveObj.ppm96_end.ToString("HH:mm");
                    options = leaveObj.ppm96_sign_string.Split(',').Select(s => s.Trim()).ToList();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditLeaveDetail(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };
                List<int> decimalValues = new List<int>();
                foreach (string binaryString in options)
                {
                    int decimalValue = Convert.ToInt32(binaryString, 2);
                    decimalValues.Add(decimalValue);
                }
                int sum = decimalValues.Sum();
                var ppm96_end = DateTime.ParseExact(ppm96_EndDate + ppm96_EndTime, "yyyyMMddHH:mm", CultureInfo.InvariantCulture);
                var ppm96_begin = DateTime.ParseExact(ppm96_beginDate + ppm96_beginTime, "yyyyMMddHH:mm", CultureInfo.InvariantCulture);
                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    ppm96_id,
                    ppm96_stfn,
                    ppm96_begin,
                    ppm96_end,
                    ppm95_id,
                    ppd95_id,
                    ppm96_sign = sum,
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.Leave.EditLeave(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteLeaveDetail(string userID)
        {
            try
            {
                string apiUrl = API.Leave.DeleteLeave(ppm96_id, userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public class LeaveChildCategory : DBEntity.ISelectItem
        {
            public string ppd95_id { get; set; }
            public string ppd95_name { get; set; }
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
            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public class LeaveCategory : DBEntity.ISelectItem
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

        public async Task<bool> GetLeaveCategoryList(string userID)
        {
            try
            {
                string apiUrl = API.Leave.LeaveMenu(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                LeaveCategoryByIdList = Common.GetJsonDeserializeObject<List<LeaveCategory>>(response);
                if (LeaveCategoryByIdList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetLeaveCategoryChildList(string ppm95_id, string userID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ppm95_id))
                {
                    LeaveChildCategoryByIdList = new List<LeaveChildCategory>();
                    return true;
                }
                string apiUrl = API.Leave.LeaveMenuChild(ppm95_id, userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                LeaveChildCategoryByIdList = Common.GetJsonDeserializeObject<List<LeaveChildCategory>>(response);
                if (LeaveChildCategoryByIdList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
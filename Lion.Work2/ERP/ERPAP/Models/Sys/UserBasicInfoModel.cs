using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class UserBasicInfoModel : SysModel
    {
        public enum Field
        {
            QueryUserID, QueryUserNM,
            IsDisable, IsLeft,
            DateBegin, DateEnd, TimeBegin, TimeEnd
        }

        private string _QueryUserID;

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_QueryUserID))
                {
                    return _QueryUserID;
                }
                return _QueryUserID.ToUpper();
            }
            set
            {
                _QueryUserID = value;
            }
        }

        [InputType(EnumInputType.TextBox)]
        public string QueryUserNM { get; set; }

        public string IsDisable { get; set; }

        public string IsLeft { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DateBegin { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DateEnd { get; set; }

        public string TimeBegin { get; set; }

        public string TimeEnd { get; set; }

        #region Object Binding

        public Dictionary<string, string> BeginTimeList = new Dictionary<string, string>()
        {
            {Common.GetEnumDesc(EnumBeginTimeValue.AM00), Common.GetEnumDesc(EnumBeginTimeText.AM00)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM01), Common.GetEnumDesc(EnumBeginTimeText.AM01)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM02), Common.GetEnumDesc(EnumBeginTimeText.AM02)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM03), Common.GetEnumDesc(EnumBeginTimeText.AM03)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM04), Common.GetEnumDesc(EnumBeginTimeText.AM04)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM05), Common.GetEnumDesc(EnumBeginTimeText.AM05)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM06), Common.GetEnumDesc(EnumBeginTimeText.AM06)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM07), Common.GetEnumDesc(EnumBeginTimeText.AM07)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM08), Common.GetEnumDesc(EnumBeginTimeText.AM08)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM09), Common.GetEnumDesc(EnumBeginTimeText.AM09)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM10), Common.GetEnumDesc(EnumBeginTimeText.AM10)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM11), Common.GetEnumDesc(EnumBeginTimeText.AM11)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM12), Common.GetEnumDesc(EnumBeginTimeText.AM12)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM01), Common.GetEnumDesc(EnumBeginTimeText.PM01)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM02), Common.GetEnumDesc(EnumBeginTimeText.PM02)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM03), Common.GetEnumDesc(EnumBeginTimeText.PM03)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM04), Common.GetEnumDesc(EnumBeginTimeText.PM04)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM05), Common.GetEnumDesc(EnumBeginTimeText.PM05)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM06), Common.GetEnumDesc(EnumBeginTimeText.PM06)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM07), Common.GetEnumDesc(EnumBeginTimeText.PM07)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM08), Common.GetEnumDesc(EnumBeginTimeText.PM08)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM09), Common.GetEnumDesc(EnumBeginTimeText.PM09)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM10), Common.GetEnumDesc(EnumBeginTimeText.PM10)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM11), Common.GetEnumDesc(EnumBeginTimeText.PM11)}
        };

        public Dictionary<string, string> EndTimeList = new Dictionary<string, string>()
        {
            {Common.GetEnumDesc(EnumEndTimeValue.AM00), Common.GetEnumDesc(EnumEndTimeText.AM00)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM01), Common.GetEnumDesc(EnumEndTimeText.AM01)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM02), Common.GetEnumDesc(EnumEndTimeText.AM02)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM03), Common.GetEnumDesc(EnumEndTimeText.AM03)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM04), Common.GetEnumDesc(EnumEndTimeText.AM04)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM05), Common.GetEnumDesc(EnumEndTimeText.AM05)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM06), Common.GetEnumDesc(EnumEndTimeText.AM06)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM07), Common.GetEnumDesc(EnumEndTimeText.AM07)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM08), Common.GetEnumDesc(EnumEndTimeText.AM08)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM09), Common.GetEnumDesc(EnumEndTimeText.AM09)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM10), Common.GetEnumDesc(EnumEndTimeText.AM10)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM11), Common.GetEnumDesc(EnumEndTimeText.AM11)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM12), Common.GetEnumDesc(EnumEndTimeText.AM12)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM01), Common.GetEnumDesc(EnumEndTimeText.PM01)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM02), Common.GetEnumDesc(EnumEndTimeText.PM02)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM03), Common.GetEnumDesc(EnumEndTimeText.PM03)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM04), Common.GetEnumDesc(EnumEndTimeText.PM04)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM05), Common.GetEnumDesc(EnumEndTimeText.PM05)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM06), Common.GetEnumDesc(EnumEndTimeText.PM06)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM07), Common.GetEnumDesc(EnumEndTimeText.PM07)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM08), Common.GetEnumDesc(EnumEndTimeText.PM08)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM09), Common.GetEnumDesc(EnumEndTimeText.PM09)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM10), Common.GetEnumDesc(EnumEndTimeText.PM10)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM11), Common.GetEnumDesc(EnumEndTimeText.PM11)}
        };

        #endregion

        public UserBasicInfoModel()
        {
        }

        public void FormReset(string userID)
        {
            QueryUserID = userID;
            QueryUserNM = string.Empty;
            TimeEnd = Common.GetEnumDesc(EnumEndTimeValue.PM11);
        }

        public List<UserBasicInfo> UserBasicInfoList { get; private set; }
        public async Task<bool> GetUserBasicInfoList(string clientUserID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string connectDTBegin = string.Empty;
                string connectDTEnd = string.Empty;

                if (!string.IsNullOrWhiteSpace(DateBegin) && !string.IsNullOrWhiteSpace(TimeBegin))
                {
                    connectDTBegin = Common.FormatDateTimeString(string.Format("{0}{1}", DateBegin, TimeBegin));
                }

                if (!string.IsNullOrWhiteSpace(DateEnd) && !string.IsNullOrWhiteSpace(TimeEnd))
                {
                    connectDTEnd = Common.FormatDateTimeString(string.Format("{0}{1}", DateEnd, TimeEnd));
                }       

                string userID = QueryUserID ?? null;
                string userNM = QueryUserNM ?? string.Empty;
                string isDisable = string.IsNullOrEmpty(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString();
                string isLeft = string.IsNullOrEmpty(IsLeft) ? EnumYN.N.ToString() : EnumYN.Y.ToString();
                string DTBegin = connectDTBegin ?? null;
                string DTEnd = connectDTEnd ?? null;

                string apiUrl = API.UserBasicInfo.QueryUserBasicInfoList(clientUserID, userID, userNM, isDisable, isLeft, DTBegin, DTEnd, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    UserBasicInfoList = (List<UserBasicInfo>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    UserBasicInfoList = responseObj.UserBasicInfoList;

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
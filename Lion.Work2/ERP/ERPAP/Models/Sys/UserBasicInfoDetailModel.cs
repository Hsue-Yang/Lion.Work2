using System;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class UserBasicInfoDetailModel : SysModel
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string ComID { get; set; }
        public string ComNM { get; set; }
        public string UnitID { get; set; }
        public string UnitNM { get; set; }
        public string RestrictType { get; set; }
        public string RestrictTypeNM { get; set; }
        public int ErrorTimes { get; set; }
        public string IsLock { get; set; }
        public string IsDisable { get; set; }
        public string IsLeft { get; set; }
              

        public async Task<bool> GetUserBasicInfoDetail(EnumCultureID cultureID,string userID)
        {
            try
            {

                string apiUrl = API.UserBasicInfo.QueryUserBasicInfoDetail(userID, UserID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var userBasicInfoDetail = Common.GetJsonDeserializeObject<UserBasicInfo>(response);
                if (userBasicInfoDetail != null)
                {
                    UserID = userBasicInfoDetail.UserID;
                    UserNM = string.Format("{0} {1}", userBasicInfoDetail.UserID, userBasicInfoDetail.UserNM);
                    ComNM = userBasicInfoDetail.ComNM;
                    UnitNM = userBasicInfoDetail.UnitNM;
                    IsLeft = userBasicInfoDetail.IsLeft;
                    RestrictType = userBasicInfoDetail.RestrictType;
                    IsDisable = userBasicInfoDetail.IsDisable;
                    ErrorTimes = userBasicInfoDetail.ErrorTimes;
                    IsLock = userBasicInfoDetail.IsLock;
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
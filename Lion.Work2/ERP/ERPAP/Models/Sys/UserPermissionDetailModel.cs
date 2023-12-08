using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class UserPermissionDetailModel : SysModel
    {
        public enum EnumModifyResult
        {
            Success,
            Failure,
            SyncASPFailure
        }

        public class UserRawDataDetail
        {
            public string UserNM;
            public string RestrictType;
            public string IsLock;
        }

        public string UserID { get; set; }

        public string UserNM { get; set; }

        [Required]
        public string RestrictType { get; set; }

        public string IsLock { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName = string.Empty,
                ActionName = string.Empty,
                TabText = SysUserPermissionDetail.TabText_UserPermissionDetail,
                ImageURL = string.Empty
            }
        };

        public UserPermissionDetailModel()
        {

        }

        public void FormReset()
        {

        }

        public UserRawDataDetail UserRawData { get; private set; }

        public async Task<bool> GetUserRawData()
        {
            try
            {
                string userID = string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID;
                string apiUrl = API.UserPermission.QueryUserRawData(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    UserRawData = (UserRawDataDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    UserRawData = responseObj.UserRawData;
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public EnumModifyResult GetEditUserPermissionResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                EntityUserPermissionDetail.UserPermissionPara para = new EntityUserPermissionDetail.UserPermissionPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    RestrictType = new DBChar((string.IsNullOrWhiteSpace(this.RestrictType) ? null : this.RestrictType)),
                    IsLock = new DBChar((string.IsNullOrWhiteSpace(this.IsLock) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID))
                };

                if (new EntityUserPermissionDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditUserPermission(para) == EntityUserPermissionDetail.EnumEditUserPermissionResult.Success)
                {
                    base.GetRecordUserAccessResult(this.UserID, this.RestrictType,
                                                   (string.IsNullOrWhiteSpace(this.IsLock) ? EnumYN.N.ToString() : EnumYN.Y.ToString()), null,
                                                   updUserID, ipAddress, cultureID);

                    return EnumModifyResult.Success;

                    //if (new EntityUserPermissionDetail(this.ConnectionStringERP, this.ProviderNameERP)
                    //    .SyncEditOpagm20(para) == LionTech.Entity.ERP.Sys.EntityUserPermissionDetail.EnumSyncEditOpagm20Result.Success)
                    //{
                    //    return EnumModifyResult.Success;
                    //}
                    //else
                    //{
                    //    return EnumModifyResult.SyncASPFailure;
                    //}
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return EnumModifyResult.Failure;
        }
    }
}
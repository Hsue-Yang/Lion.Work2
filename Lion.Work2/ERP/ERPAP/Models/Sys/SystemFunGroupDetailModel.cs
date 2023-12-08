using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemFunGroupDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemFunGroupDetailResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunControllerID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupKOKR { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        #endregion
        
        #region - Reset -
        public void FormReset()
        {
            FunGroupZHTW = string.Empty;
            FunGroupZHCN = string.Empty;
            FunGroupENUS = string.Empty;
            FunGroupTHTH = string.Empty;
            FunGroupJAJP = string.Empty;
            FunGroupKOKR = string.Empty;
            SortOrder = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemFunGroupDetail(string userID)
        {
            try
            {
                string response = await GetSystemFunGroup(userID);

                var responseObj = new
                {
                    FunGroupZHTW = (string)null,
                    FunGroupZHCN = (string)null,
                    FunGroupENUS = (string)null,
                    FunGroupTHTH = (string)null,
                    FunGroupJAJP = (string)null,
                    FunGroupKOKR = (string)null,
                    SortOrder = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    FunGroupZHTW = responseObj.FunGroupZHTW;
                    FunGroupZHCN = responseObj.FunGroupZHCN;
                    FunGroupENUS = responseObj.FunGroupENUS;
                    FunGroupTHTH = responseObj.FunGroupTHTH;
                    FunGroupJAJP = responseObj.FunGroupJAJP;
                    FunGroupKOKR = responseObj.FunGroupKOKR;
                    SortOrder = responseObj.SortOrder;
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        
        public async Task<bool> EditSystemFunGroupDetail(EnumActionType actionType, string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID,
                    FunControllerID,
                    FunGroupZHTW,
                    FunGroupZHCN,
                    FunGroupENUS,
                    FunGroupTHTH,
                    FunGroupJAJP,
                    FunGroupKOKR,
                    SortOrder,
                    UpdUserID = userID,
                    ExecSysID = EnumSystemID.ERPAP.ToString(),
                    ExecIPAddress = ipAddress
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemFunGroup.EditSystemFunGroup(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                Mongo_BaseAP.EnumModifyType modifyType = Mongo_BaseAP.EnumModifyType.U;
                if (actionType == EnumActionType.Add)
                {
                    modifyType = Mongo_BaseAP.EnumModifyType.I;
                }

                var systemFunGroupJsonStr = await GetSystemFunGroup(userID);
                GetRecordSysSystemFunGroupResult(systemFunGroupJsonStr, modifyType, userID, ipAddress, cultureID);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemFunGroupDetailResult> DeleteSystemFunGroupDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            var result = EnumDeleteSystemFunGroupDetailResult.Failure;
            try
            {
                string systemFunGroupJsonStr = await GetSystemFunGroup(userID);

                string apiUrl = API.SystemFunGroup.DeleteSystemFunGroup(SysID, userID, FunControllerID, EnumSystemID.ERPAP.ToString(), ipAddress);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemFunGroupDetailResult.Success;
                GetRecordSysSystemFunGroupResult(systemFunGroupJsonStr, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID);

                return result;
            }
            catch (WebException webException)
                when (webException.Response is HttpWebResponse &&
                      ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.BadRequest)
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)webException.Response;

                if (string.IsNullOrWhiteSpace(httpWebResponse.StatusDescription) == false)
                {
                    string errorMsg = Common.GetStreamToString(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                    var msg = new { Message = (string)null };
                    msg = Common.GetJsonDeserializeAnonymousType(errorMsg, msg);

                    if (msg.Message == EnumDeleteSystemFunGroupDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemFunGroupDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        private async Task<string> GetSystemFunGroup(string userID)
        {
            string apiUrl = API.SystemFunGroup.QuerySystemFunGroup(SysID, userID, FunControllerID);
            string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

            return response;
        }

        private void GetRecordSysSystemFunGroupResult(string systemFunGroupJsonStr, Mongo_BaseAP.EnumModifyType modifyType, string uerID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                var systemFunGroupDetail = new
                {
                    FunGroupZHTW = (string)null,
                    FunGroupZHCN = (string)null,
                    FunGroupENUS = (string)null,
                    FunGroupTHTH = (string)null,
                    FunGroupJAJP = (string)null,
                    FunGroupKOKR = (string)null,
                    SortOrder = (string)null
                };

                systemFunGroupDetail = Common.GetJsonDeserializeAnonymousType(systemFunGroupJsonStr, systemFunGroupDetail);

                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(uerID) ? null : uerID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(basicInfoPara);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> modifyTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(codePara);              

                Mongo_BaseAP.RecordSysSystemFunGroupPara recordPara = new Mongo_BaseAP.RecordSysSystemFunGroupPara()
                {
                    ModifyType = new DBChar(modifyType.ToString()),
                    ModifyTypeNM = new DBNVarChar(null),
                    SysID = SysID,
                    SysNM = UserSystemByIdList.Find(x => x.SysID.Contains(SysID)).SysNM,
                    FunControllerID = FunControllerID,
                    FunGroupZHTW = systemFunGroupDetail.FunGroupZHTW,
                    FunGroupZHCN = systemFunGroupDetail.FunGroupZHCN,
                    FunGroupENUS = systemFunGroupDetail.FunGroupENUS,
                    FunGroupTHTH = systemFunGroupDetail.FunGroupTHTH,
                    FunGroupJAJP = systemFunGroupDetail.FunGroupJAJP,
                    FunGroupKOKR = systemFunGroupDetail.FunGroupKOKR,
                    SortOrder = systemFunGroupDetail.SortOrder,
                    APINo = new DBChar(null),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (modifyTypeList != null && modifyTypeList.Count > 0 && !string.IsNullOrWhiteSpace(modifyType.ToString()))
                {
                    recordPara.ModifyTypeNM = (modifyTypeList.Find(e => e.CodeID.GetValue() == modifyType.ToString())).CodeNM;
                }

                //new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP).RecordSysSystemFunGroup(recordPara);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }

        #region Event
        public string GetEventParaSysSystemFunGroupEdit()
        {
            var entityEventParaSystemFunGroupEdit = new 
            {
                TargetSysIDList = new List<string> {SysID},
                SysID,
                FunControllerID,
                FunGroupZHTW,
                FunGroupZHCN,
                FunGroupENUS,
                FunGroupTHTH,
                FunGroupJAJP,
                FunGroupKOKR,
                SortOrder
            };

            return  Common.GetJsonSerializeObject(entityEventParaSystemFunGroupEdit);
        }

        public string GetEventParaSysSystemFunGroupDelete()
        {
            
            var entityEventParaSystemFunGroupDelete = new 
            {
                TargetSysIDList = new List<string> {SysID},
                SysID,
                FunControllerID
            };

            return Common.GetJsonSerializeObject(entityEventParaSystemFunGroupDelete);
        }
        #endregion
    }
}
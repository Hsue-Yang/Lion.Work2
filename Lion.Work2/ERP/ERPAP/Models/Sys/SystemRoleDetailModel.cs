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
    public class SystemRoleDetailModel : SysModel
    {
        #region - Property - 

        [Required]
        public string SysID { get; set; }

        public string RoleCategoryID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string RoleID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMKOKR { get; set; }

        public string IsMaster { get; set; }

        #endregion

        #region - Reset -

        public void FormReset()
        {
            RoleNMZHTW = string.Empty;
            RoleNMZHCN = string.Empty;
            RoleNMENUS = string.Empty;
            RoleNMTHTH = string.Empty;
            RoleNMJAJP = string.Empty;
            RoleNMKOKR = string.Empty;
        }

        #endregion

        public async Task<bool> GetSystemRoleDetail(string userID)
        {
            try
            {
                string response = await GetSystemRole(userID);

                var responseObj = new
                {
                    SysID = (string) null,
                    RoleCategoryID = (string) null,
                    RoleID = (string) null,
                    RoleNMZHTW = (string) null,
                    RoleNMZHCN = (string) null,
                    RoleNMENUS = (string) null,
                    RoleNMTHTH = (string) null,
                    RoleNMJAJP = (string) null,
                    RoleNMKOKR = (string) null,
                    IsMaster = IsMaster ?? EnumYN.N.ToString()
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SysID = responseObj.SysID;
                    RoleCategoryID = responseObj.RoleCategoryID;
                    RoleID = responseObj.RoleID;
                    RoleNMZHTW = responseObj.RoleNMZHTW;
                    RoleNMZHCN = responseObj.RoleNMZHCN;
                    RoleNMENUS = responseObj.RoleNMENUS;
                    RoleNMTHTH = responseObj.RoleNMTHTH;
                    RoleNMJAJP = responseObj.RoleNMJAJP;
                    RoleNMKOKR = responseObj.RoleNMKOKR;
                    IsMaster = responseObj.IsMaster;
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public async Task<bool> EditSystemRole(EnumActionType actionType, string userID, string ipAddress, EnumCultureID cultureID)
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
                    RoleCategoryID,
                    RoleID,
                    RoleNMZHTW,
                    RoleNMZHCN,
                    RoleNMENUS,
                    RoleNMTHTH,
                    RoleNMJAJP,
                    RoleNMKOKR,
                    IsMaster = IsMaster ?? EnumYN.N.ToString(),
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemRole.EditSystemRole(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                Mongo_BaseAP.EnumModifyType modifyType = Mongo_BaseAP.EnumModifyType.U;
                if (actionType == EnumActionType.Add)
                {
                    modifyType = Mongo_BaseAP.EnumModifyType.I;
                }

                var jsonStr = await GetSystemRole(userID);
                GetRecordSysSystemRoleResult(jsonStr, modifyType, userID, ipAddress, cultureID);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteSystemRole(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                var jsonStr = await GetSystemRole(userID);

                string apiUrl = API.SystemRole.DeleteSystemRole(SysID, userID, RoleID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");
                
                GetRecordSysSystemRoleResult(jsonStr, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        private async Task<string> GetSystemRole(string userID)
        {
            string apiUrl = API.SystemRole.QuerySystemRole(SysID, userID, RoleID);
            var jsonStr = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

            return jsonStr;
        }

        private void GetRecordSysSystemRoleResult(string jsonStr, Mongo_BaseAP.EnumModifyType modifyType, string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                var systemRoleDetail = new
                {
                    SysID = (string)null,
                    RoleCategoryID = (string)null,
                    RoleID = (string)null,
                    RoleNMZHTW = (string)null,
                    RoleNMZHCN = (string)null,
                    RoleNMENUS = (string)null,
                    RoleNMTHTH = (string)null,
                    RoleNMJAJP = (string)null,
                    RoleNMKOKR = (string)null,
                    IsMaster = IsMaster ?? EnumYN.N.ToString()
                };

                systemRoleDetail = Common.GetJsonDeserializeAnonymousType(jsonStr, systemRoleDetail);

                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
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

                Mongo_BaseAP.RecordSysSystemRolePara recordPara = new Mongo_BaseAP.RecordSysSystemRolePara()
                {
                    ModifyType = new DBChar(modifyType.ToString()),
                    ModifyTypeNM = new DBNVarChar(null),
                    SysID = systemRoleDetail.SysID,
                    RoleCategoryID = systemRoleDetail.RoleCategoryID,
                    RoleID = systemRoleDetail.RoleID,
                    RoleNMZHTW = systemRoleDetail.RoleNMZHTW,
                    RoleNMZHCN = systemRoleDetail.RoleNMZHCN,
                    RoleNMENUS = systemRoleDetail.RoleNMENUS,
                    RoleNMTHTH = systemRoleDetail.RoleNMTHTH,
                    RoleNMJAJP = systemRoleDetail.RoleNMJAJP,
                    RoleNMKOKR = systemRoleDetail.RoleNMKOKR,
                    IsMaster = systemRoleDetail.IsMaster,
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

                //new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP).RecordSysSystemRole(recordPara);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }

        #region Event

        public string GetEventParaSysSystemRoleEdit()
        {
            try
            {
                var entityEventParaSystemRoleEdit = new
                {
                    TargetSysIDList = new List<string> {SysID},
                    SysID,
                    RoleCategoryID = string.IsNullOrWhiteSpace(RoleCategoryID) ? null : RoleCategoryID,
                    RoleID,
                    RoleNMzhTW = RoleNMZHTW,
                    RoleNMzhCN = RoleNMZHCN,
                    RoleNMenUS = RoleNMENUS,
                    RoleNMthTH = RoleNMTHTH,
                    RoleNMjaJP = RoleNMJAJP,
                    RoleNMkoKR = RoleNMKOKR,
                    IsMaster = IsMaster ?? EnumYN.N.ToString()
                };

                return Common.GetJsonSerializeObject(entityEventParaSystemRoleEdit);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return null;
        }

        public string GetEventParaSysSystemRoleDelete()
        {
            try
            {
                var entityEventParaSystemRoleDelete = new
                {
                    TargetSysIDList = new List<string> {SysID},
                    SysID,
                    RoleID
                };

                return Common.GetJsonSerializeObject(entityEventParaSystemRoleDelete);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return null;
        }

        #endregion
    }
}
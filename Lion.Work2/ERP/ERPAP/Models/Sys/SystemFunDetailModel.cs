using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemFunDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemFunDetailResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Class -
        public class SystemFunMain
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }

            public string PurviewID { get; set; }
            public string PurviewNM { get; set; }

            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }

            public string FunActionName { get; set; }
            public string FunNMZHTW { get; set; }
            public string FunNMZHCN { get; set; }
            public string FunNMENUS { get; set; }
            public string FunNMTHTH { get; set; }
            public string FunNMJAJP { get; set; }
            public string FunNMKOKR { get; set; }

            public string FunType { get; set; }
            public string FunTypeNM { get; set; }

            public string IsDisable { get; set; }
            public string IsOutside { get; set; }
            public string SortOrder { get; set; }
        }

        public class SystemMenuFun : DBEntity.ValueListRow
        {
            public string FunMenuSysID { get; set; }
            public string FunMenu { get; set; }

            public string FunMenuXAxis { get; set; }
            public string FunMenuYAxis { get; set; }

            public List<SysFunMenu> SystemFunMenuList { get; set; }

            public string GetAllFields()
            {
                return $"{FunMenuSysID}|{FunMenu}|{FunMenuXAxis}|{FunMenuYAxis}";
            }
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        public string SubSysID { get; set; }

        public string PurviewID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunControllerID { get; set; }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunActionName { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMKOKR { get; set; }

        [Required]
        public string FunType { get; set; }

        public string IsOutside { get; set; }

        public string IsDisable { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<string> HasRole { get; set; }
        
        public List<SystemMenuFun> SystemMenuFunList { get; private set; }

        public List<SystemMenuFun> SystemMenuFunValueList { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            SubSysID = string.Empty;
            PurviewID = string.Empty;
            FunNMZHTW = string.Empty;
            FunNMZHCN = string.Empty;
            FunNMENUS = string.Empty;
            FunNMTHTH = string.Empty;
            FunNMJAJP = string.Empty;
            FunNMKOKR = string.Empty;
            FunType = string.Empty;
            IsOutside = EnumYN.N.ToString();
            IsDisable = EnumYN.N.ToString();
            SortOrder = string.Empty;
            HasRole = new List<string>();
        }
        #endregion

        public async Task<bool> GetSystemFunDetail(string userID, EnumCultureID cultureID)
        {
            try
            {
                var systemFunDetail = await GetSystemFun(userID, cultureID.ToString().ToUpper());

                if (systemFunDetail != null)
                {
                    SubSysID = systemFunDetail.SubSysID;
                    PurviewID = systemFunDetail.PurviewID;
                    FunNMZHTW = systemFunDetail.FunNMZHTW;
                    FunNMZHCN = systemFunDetail.FunNMZHCN;
                    FunNMENUS = systemFunDetail.FunNMENUS;
                    FunNMTHTH = systemFunDetail.FunNMTHTH;
                    FunNMJAJP = systemFunDetail.FunNMJAJP;
                    FunNMKOKR = systemFunDetail.FunNMKOKR;
                    FunType = systemFunDetail.FunType;
                    IsOutside = systemFunDetail.IsOutside;
                    IsDisable = systemFunDetail.IsDisable;
                    SortOrder = systemFunDetail.SortOrder;

                    await GetSystemMenuFunDetail(userID, cultureID);

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemFunDetail(EnumActionType actionType, string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                SubSysID = string.IsNullOrWhiteSpace(SubSysID) ? SysID : SubSysID;

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID,
                    SubSysID,
                    PurviewID,
                    FunControllerID,
                    FunActionName,
                    FunNMZHTW,
                    FunNMZHCN,
                    FunNMENUS,
                    FunNMTHTH,
                    FunNMJAJP,
                    FunNMKOKR,
                    FunType,
                    IsOutside = IsOutside ?? EnumYN.N.ToString(),
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder,
                    UpdUserID = userID,
                    RoleIDs = HasRole?
                        .Select(r => string.IsNullOrWhiteSpace(r.Split('|')[1]) ? null : r.Split('|')[1]),
                    SystemMenuFuns = SystemMenuFunValueList?
                        .Where(menu => string.IsNullOrWhiteSpace(menu.FunMenuSysID) == false && string.IsNullOrWhiteSpace(menu.FunMenu) == false)
                        .Select(menu => new
                        {
                            menu.FunMenuSysID,
                            menu.FunMenu,
                            menu.FunMenuXAxis,
                            menu.FunMenuYAxis
                        })
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemFun.EditSystemFun(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                Mongo_BaseAP.EnumModifyType modifyType = Mongo_BaseAP.EnumModifyType.U;
                if (actionType == EnumActionType.Add)
                {
                    modifyType = Mongo_BaseAP.EnumModifyType.I;
                }

                var systemFunDetail = await GetSystemFun(userID, cultureID.ToString().ToUpper());

                GetRecordSysSystemFunResult(systemFunDetail, modifyType, userID, ipAddress, cultureID);
                GetRecordSysSystemRoleFunResult(systemFunDetail, userID, ipAddress, cultureID);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemFunDetailResult> DeleteSystemFunDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            var result = EnumDeleteSystemFunDetailResult.Failure;
            try
            {
                var systemFunDetail = await GetSystemFun(userID, cultureID.ToString().ToUpper());

                string apiUrl = API.SystemFun.DeleteSystemFun(SysID, userID, FunControllerID, FunActionName);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemFunDetailResult.Success;

                GetRecordSysSystemFunResult(systemFunDetail, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID);
                GetRecordSysSystemRoleFunResult(systemFunDetail, userID, ipAddress, cultureID);
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

                    if (msg.Message == EnumDeleteSystemFunDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemFunDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        #region - Private -
        private async Task<SystemFunMain> GetSystemFun(string userID, string cultureID)
        {
            string apiUrl = API.SystemFun.QuerySystemFun(SysID, userID, FunControllerID, FunActionName, cultureID);
            string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

            return Common.GetJsonDeserializeObject<SystemFunMain>(response);
        }

        private async Task GetSystemMenuFunDetail(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFun.QuerySystemMenuFuns(SysID, userID, FunControllerID, FunActionName);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemMenuFunList = Common.GetJsonDeserializeObject<List<SystemMenuFun>>(response);

                if (SystemMenuFunList != null && SystemMenuFunList.Any())
                {
                    foreach (var row in SystemMenuFunList)
                    {
                        await GetSystemFunMenuByIdList(row.FunMenuSysID, userID, cultureID);
                        row.SystemFunMenuList = SystemFunMenuByIdList;
                    }
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }

        private void GetRecordSysSystemFunResult(SystemFunMain SystemFunDetail, Mongo_BaseAP.EnumModifyType modifyType, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP).SelectBasicInfo(basicInfoPara);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> modifyTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP).SelectCMCodeList(codePara);

                Mongo_BaseAP.RecordSysSystemFunPara recordPara = new Mongo_BaseAP.RecordSysSystemFunPara
                {
                    ModifyType = new DBChar(modifyType.ToString()),
                    ModifyTypeNM = new DBNVarChar(null),
                    SysID = SystemFunDetail.SysID,
                    SysNM = SystemFunDetail.SysNM,
                    SubSysID = SystemFunDetail.SubSysID,
                    SubSysNM = SystemFunDetail.SysNM,
                    PurviewID = SystemFunDetail.PurviewID,
                    PurviewNM = SystemFunDetail.PurviewNM,
                    FunControllerID = SystemFunDetail.FunControllerID,
                    FunGroupNM = SystemFunDetail.FunGroupNM,
                    FunActionNM = SystemFunDetail.FunActionName,
                    FunNMZHTW = SystemFunDetail.FunNMZHTW,
                    FunNMZHCN = SystemFunDetail.FunNMZHCN,
                    FunNMENUS = SystemFunDetail.FunNMENUS,
                    FunNMTHTH = SystemFunDetail.FunNMTHTH,
                    FunNMJAJP = SystemFunDetail.FunNMJAJP,
                    FunNMKOKR = SystemFunDetail.FunNMKOKR,
                    FunType = SystemFunDetail.FunType,
                    FunTypeNM = SystemFunDetail.FunTypeNM,
                    IsOutside = SystemFunDetail.IsOutside,
                    IsDisable = SystemFunDetail.IsDisable,
                    SortOrder = SystemFunDetail.SortOrder,
                    APINo = new DBChar(null),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (modifyTypeList != null && modifyTypeList.Any() && !string.IsNullOrWhiteSpace(modifyType.ToString()))
                {
                    recordPara.ModifyTypeNM = (modifyTypeList.Find(e => e.CodeID.GetValue() == modifyType.ToString())).CodeNM;
                }

                //new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP).RecordSysSystemFun(recordPara);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }

        private void GetRecordSysSystemRoleFunResult(SystemFunMain systemFunDetail, string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP).SelectBasicInfo(basicInfoPara);

                Mongo_BaseAP.RecordSysSystemRoleFunPara mongoPara = new Mongo_BaseAP.RecordSysSystemRoleFunPara
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID)),
                    FunActionNM = new DBVarChar((string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName))
                };

                List<Mongo_BaseAP.RecordSysSystemRoleFunPara> mongoParaList = new List<Mongo_BaseAP.RecordSysSystemRoleFunPara>();

                if (HasRole != null && HasRole.Any())
                {
                    mongoParaList = HasRole.Select(r => new Mongo_BaseAP.RecordSysSystemRoleFunPara
                    {
                        SysID = string.IsNullOrWhiteSpace(r.Split('|')[0]) ? null : r.Split('|')[0],
                        SysNM = systemFunDetail.SysNM,
                        RoleID = string.IsNullOrWhiteSpace(r.Split('|')[1]) ? null : r.Split('|')[1],
                        RoleNM = string.IsNullOrWhiteSpace(r.Split('|')[2]) ? null : r.Split('|')[2],
                        FunControllerID = FunControllerID,
                        FunGroupNM = systemFunDetail.FunGroupNM,
                        FunActionNM = FunActionName,
                        FunNM = systemFunDetail.FunNMZHTW,
                        APINo = new DBChar(null),
                        UpdUserID = entityBasicInfo.UpdUserID,
                        UpdUserNM = entityBasicInfo.UpdUserNM,
                        UpdDT = new DBDateTime(DateTime.Now),
                        ExecSysID = entityBasicInfo.ExecSysID,
                        ExecSysNM = entityBasicInfo.ExecSysNM,
                        ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                    }).ToList();
                }

                //new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP).RecordSysSystemRoleFun(mongoPara, mongoParaList);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }
        #endregion

        #region - Event -
        public string GetEventParaSysSystemFunEdit()
        {
            var eventParaSystemFunEdit = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                SubSysID,
                PurviewID,
                FunControllerID,
                FunActionName,
                FunNMzhTW = FunNMZHTW,
                FunNMzhCN = FunNMZHCN,
                FunNMenUS = FunNMENUS,
                FunNMthTH = FunNMTHTH,
                FunNMjaJP = FunNMJAJP,
                FunNMkoKR = FunNMKOKR,
                IsOutside = string.IsNullOrWhiteSpace(IsOutside) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                SortOrder,
                RoleIDList = new List<string>()
            };

            if (HasRole != null && HasRole.Any())
            {
                foreach (string role in HasRole)
                {
                    eventParaSystemFunEdit.RoleIDList.Add(role.Split('|')[1]);
                }
            }
            return Common.GetJsonSerializeObject(eventParaSystemFunEdit);
        }

        public string GetEventParaSysSystemFunDelete()
        {
            var eventParaSystemFunDelete = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                FunControllerID,
                FunActionName
            };

            return Common.GetJsonSerializeObject(eventParaSystemFunDelete);
        }

        public string GetEventParaSysSystemFunMenuEdit()
        {
            var eventParaSystemFunMenuEdit = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                FunControllerID,
                FunActionName,
                FunMenuList = new List<string>()
            };

            if (SystemMenuFunList != null && SystemMenuFunList.Any())
            {
                foreach (var funMenu in SystemMenuFunList)
                {
                    eventParaSystemFunMenuEdit.FunMenuList.Add(funMenu.GetAllFields());
                }
            }

            return Common.GetJsonSerializeObject(eventParaSystemFunMenuEdit);
        }
        #endregion
    }
}
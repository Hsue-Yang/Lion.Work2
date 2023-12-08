using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models
{
    public class _BaseAPModel : _BaseModel
    {
        public enum EnumTabController
        {
            Pub, Sys, Dev, Help
        }

        public enum EnumTabAction
        {
            [Description("Signature")]
            PubSignature,
            [Description("Document")]
            PubDocument,
            [Description("Remark")]
            PubRemark,

            [Description("GoogleAccountSetting")]
            GoogleAccountSetting,

            [Description("SystemAPIGroup")]
            SysSystemAPIGroup,
            [Description("SystemAPI")]
            SysSystemAPI,
            [Description("SystemAPILog")]
            SysSystemAPILog,

            [Description("SystemEDIFlow")]
            SysSystemEDIFlow,
            [Description("SystemEDIJob")]
            SysSystemEDIJob,
            [Description("SystemEDICon")]
            SysSystemEDICon,
            [Description("SystemEDIPara")]
            SysSystemEDIPara,
            [Description("SystemEDIFlowLog")]
            SysSystemEDIFlowLog,
            [Description("SystemEDIJobLog")]
            SysSystemEDIJobLog,
            [Description("SystemEDIFlowLogSetting")]
            SysSystemEDIFlowLogSetting,

            [Description("SystemEventGroup")]
            SysSystemEventGroup,
            [Description("SystemEvent")]
            SysSystemEvent,
            [Description("SystemEventEDI")]
            SysSystemEventEDI,

            [Description("SystemSetting")]
            SysSystemSetting,
            [Description("SystemRole")]
            SysSystemRole,
            [Description("SystemRoleCondition")]
            SysSystemRoleCondition,
            [Description("SystemPurview")]
            SysSystemPurview,
            [Description("SystemFunMenu")]
            SysSystemFunMenu,
            [Description("SystemFunGroup")]
            SysSystemFunGroup,
            [Description("SystemFun")]
            SysSystemFun,
            [Description("SystemFunDetail")]
            SysSystemFunDetail,
            [Description("SystemFunAssign")]
            SysSystemFunAssign,
            [Description("SystemFunElm")]
            SysSystemFunElm,

            [Description("UserRoleFun")]
            SysUserRoleFun,
            [Description("UserRoleFunDetail")]
            SysUserRoleFunDeatil,
            [Description("UserFunction")]
            SysUserFunction,
            [Description("UserSystem")]
            SysUserSystem,
            [Description("UserPurview")]
            SysUserPurview,
            [Description("RoleUser")]
            SysRoleUser,

            [Description("SystemRecord")]
            SysSystemRecord,
            [Description("UserBasicInfo")]
            SysUserBasicInfo,
            [Description("UserBasicInfoDetail")]
            SysUserBasicInfoDetail,
            [Description("UserSystemRole")]
            SysUserSystemRole,
            [Description("ADSReport")]
            SysADSReport,

            [Description("UserDomain")]
            SysUserDomain,
            [Description("DomainGroup")]
            SysDomainGroup,
            [Description("SRCProject")]
            SysSRCProject,

            [Description("SystemWorkFlowGroup")]
            SystemWorkFlowGroup,
            [Description("SystemWorkFlowGroupDetail")]
            SystemWorkFlowGroupDetail,
            [Description("SystemWorkFlow")]
            SystemWorkFlow,
            [Description("SystemWorkFlowDetail")]
            SystemWorkFlowDetail,
            [Description("SystemWorkFlowChart")]
            SystemWorkFlowChart,
            [Description("SystemWorkFlowNode")]
            SystemWorkFlowNode,
            [Description("SystemWorkFlowNodeDetail")]
            SystemWorkFlowNodeDetail,
            [Description("SystemWorkFlowNext")]
            SystemWorkFlowNext,
            [Description("SystemWorkFlowNextDetail")]
            SystemWorkFlowNextDetail,
            [Description("SystemWorkFlowSignature")]
            SystemWorkFlowSignature,
            [Description("SystemWorkFlowSignatureDetail")]
            SystemWorkFlowSignatureDetail,
            [Description("SystemWorkFlowDocument")]
            SystemWorkFlowDocument,
            [Description("SystemWorkFlowDocumentDetail")]
            SystemWorkFlowDocumentDetail,

            [Description("UserConnect")]
            SysUserConnect,
            [Description("LatestUseIP")]
            SysLatestUseIP
        }

        public enum EnumCookieKey
        {

        }

        public enum EnumSysFunToolNo
        {
            [Description("000001")]
            DefaultNo,
            [Description("000002")]
            FirstNo
        }

        public enum EnumBeginTimeText
        {
            [Description("00:00")]
            AM00,
            [Description("01:00")]
            AM01,
            [Description("02:00")]
            AM02,
            [Description("03:00")]
            AM03,
            [Description("04:00")]
            AM04,
            [Description("05:00")]
            AM05,
            [Description("06:00")]
            AM06,
            [Description("07:00")]
            AM07,
            [Description("08:00")]
            AM08,
            [Description("09:00")]
            AM09,
            [Description("10:00")]
            AM10,
            [Description("11:00")]
            AM11,
            [Description("12:00")]
            AM12,

            [Description("13:00")]
            PM01,
            [Description("14:00")]
            PM02,
            [Description("15:00")]
            PM03,
            [Description("16:00")]
            PM04,
            [Description("17:00")]
            PM05,
            [Description("18:00")]
            PM06,
            [Description("19:00")]
            PM07,
            [Description("20:00")]
            PM08,
            [Description("21:00")]
            PM09,
            [Description("22:00")]
            PM10,
            [Description("23:00")]
            PM11
        }

        public enum EnumBeginTimeValue
        {
            [Description("000000000")]
            AM00,
            [Description("010000000")]
            AM01,
            [Description("020000000")]
            AM02,
            [Description("030000000")]
            AM03,
            [Description("040000000")]
            AM04,
            [Description("050000000")]
            AM05,
            [Description("060000000")]
            AM06,
            [Description("070000000")]
            AM07,
            [Description("080000000")]
            AM08,
            [Description("090000000")]
            AM09,
            [Description("100000000")]
            AM10,
            [Description("110000000")]
            AM11,
            [Description("120000000")]
            AM12,

            [Description("130000000")]
            PM01,
            [Description("140000000")]
            PM02,
            [Description("150000000")]
            PM03,
            [Description("160000000")]
            PM04,
            [Description("170000000")]
            PM05,
            [Description("180000000")]
            PM06,
            [Description("190000000")]
            PM07,
            [Description("200000000")]
            PM08,
            [Description("210000000")]
            PM09,
            [Description("220000000")]
            PM10,
            [Description("230000000")]
            PM11
        }

        public enum EnumEndTimeText
        {
            [Description("00:59")]
            AM00,
            [Description("01:59")]
            AM01,
            [Description("02:59")]
            AM02,
            [Description("03:59")]
            AM03,
            [Description("04:59")]
            AM04,
            [Description("05:59")]
            AM05,
            [Description("06:59")]
            AM06,
            [Description("07:59")]
            AM07,
            [Description("08:59")]
            AM08,
            [Description("09:59")]
            AM09,
            [Description("10:59")]
            AM10,
            [Description("11:59")]
            AM11,
            [Description("12:59")]
            AM12,

            [Description("13:59")]
            PM01,
            [Description("14:59")]
            PM02,
            [Description("15:59")]
            PM03,
            [Description("16:59")]
            PM04,
            [Description("17:59")]
            PM05,
            [Description("18:59")]
            PM06,
            [Description("19:59")]
            PM07,
            [Description("20:59")]
            PM08,
            [Description("21:59")]
            PM09,
            [Description("22:59")]
            PM10,
            [Description("23:59")]
            PM11
        }

        public enum EnumEndTimeValue
        {
            [Description("005959000")]
            AM00,
            [Description("015959000")]
            AM01,
            [Description("025959000")]
            AM02,
            [Description("035959000")]
            AM03,
            [Description("045959000")]
            AM04,
            [Description("055959000")]
            AM05,
            [Description("065959000")]
            AM06,
            [Description("075959000")]
            AM07,
            [Description("085959000")]
            AM08,
            [Description("095959000")]
            AM09,
            [Description("105959000")]
            AM10,
            [Description("115959000")]
            AM11,
            [Description("125959000")]
            AM12,

            [Description("135959000")]
            PM01,
            [Description("145959000")]
            PM02,
            [Description("155959000")]
            PM03,
            [Description("165959000")]
            PM04,
            [Description("175959000")]
            PM05,
            [Description("185959000")]
            PM06,
            [Description("195959000")]
            PM07,
            [Description("205959000")]
            PM08,
            [Description("215959000")]
            PM09,
            [Description("225959000")]
            PM10,
            [Description("235959000")]
            PM11
        }

        public enum EnumCMCodeKind
        {
            [Description("0000")]
            Culture,

            [Description("0001")]
            SystemService,

            [Description("0002")]
            EDIStatusCode,

            [Description("0003")]
            EDIResultStatusCode,

            [Description("0009")]
            DomainName,

            [Description("0011")]
            SystemFunType,

            [Description("0013")]
            APIReturnType,

            [Description("0015")]
            OrgArea,

            [Description("0016")]
            OrgGroup,

            [Description("0017")]
            OrgPlace,

            [Description("0018")]
            OrgDept,

            [Description("0019")]
            OrgTeam,

            [Description("0020")]
            OrgPTitle,

            [Description("0021")]
            OrgPTitle2,

            [Description("0022")]
            OrgLevel,

            [Description("0023")]
            OrgTitle,

            [Description("0024")]
            OrgProperty,

            [Description("0026")]
            WorkFlowType,

            [Description("0027")]
            WorkFlowNodeType,

            [Description("0028")]
            WorkFlowResultType,

            [Description("0029")]
            SignatureResultType,

            [Description("0030")]
            SignatureUserType,

            [Description("0031")]
            IPTrustType,

            [Description("0032")]
            IPSourceType,

            [Description("0033")]
            RestrictType,

            [Description("0034")]
            ModifyType,

            [Description("0035")]
            RecordType,

            [Description("0036")]
            UserLocation,

            [Description("0037")]
            SignatureID,

            [Description("0038")]
            Work,

            [Description("0039")]
            Title,

            [Description("0040")]
            LionCountryCode,

            [Description("0041")]
            PurviewType,

            [Description("0042")]
            PurviewOperationType,

            [Description("0043")]
            LineReceiverSourceType,

            [Description("0044")]
            IOSType,

            [Description("0045")]
            ElmDisplayType
        }

        public Dictionary<Entity_BaseAP.EnumCMCodeKind, List<DBEntity.IExtendedSelectItem>> CMCodeDictionary { protected set; get; }

        #region - API -
        public enum EnumContentType
        {
            [Description("image/bmp")]
            bmp,
            [Description("image/gif")]
            gif,
            [Description("image/jpeg")]
            jpeg,
            [Description("image/jpeg")]
            jpg,
            [Description("image/png")]
            png
        }

        #region - 取得自動完成使用者清單 -
        /// <summary>
        /// 取得自動完成使用者清單
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Entity_BaseAP.AutoUserInfo> GetAutoUserInfoList(string userID)
        {
            var result = new List<Entity_BaseAP.AutoUserInfo>();

            try
            {
                Entity_BaseAP.AutoUserInfoPara para = new Entity_BaseAP.AutoUserInfoPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                };

                result = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP).SelectAutoUserInfoList(para);

                return result;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return result;
        }
        #endregion

        public string GenerateUserMenuXML(string userId, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return null;
                }

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userId) ? null : userId))
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserMenuFunList(para);

                var userMenu = LionTech.Entity.ERP.Utility.GetUserMenu(userFunMenuList, userId);
                var xml = LionTech.Entity.ERP.Utility.StringToXmlDocument(userMenu.SerializeToXml());
                return xml.OuterXml;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }

        public string GetContentType(string filePath)
        {
            try
            {
                string fileType = Path.GetExtension(filePath);
                fileType = fileType.Substring(1, fileType.Length - 1);

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    if (Enum.IsDefined(typeof(EnumContentType), fileType))
                    {
                        return Common.GetEnumDesc((EnumContentType)Enum.Parse(typeof(EnumContentType), fileType));
                    }
                    else
                    {
                        return MediaTypeNames.Application.Octet;
                    }
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return MediaTypeNames.Application.Octet;
        }

        public string GetFileName(string filePath)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return Common.GetDateTimeString();
        }

        protected override void OnException(Exception exception)
        {
            base.OnException(exception);

            try
            {
                string userId = null;
                string userName = null;

                var httpSessionState = HttpContext.Current.Session;
                if (httpSessionState != null &&
                    httpSessionState.Count > 0)
                {
                    userId = (string)httpSessionState[EnumSessionKey.UserID.ToString()];
                    userName = (string)httpSessionState[EnumSessionKey.UserNM.ToString()];
                }

                var serpMailMessages =
                    new SERPMailMessages
                    {
                        MialAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SysSDMail.ToString()],
                        SmtpClientIPAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SmtpClientIPAddress.ToString()],
                        AppName = Common.GetEnumDesc(EnumSystemID.ERPAP),
                        UserID = userId,
                        UserName = userName,
                        Ex = exception
                    };

                PublicFun.SendErrorMailForSERP(serpMailMessages);

                string lineID = ConfigurationManager.AppSettings[EnumAppSettingKey.LineID.ToString()];
                if (string.IsNullOrWhiteSpace(lineID) == false)
                {
                    var to = ConfigurationManager.AppSettings[EnumAppSettingKey.LineTo.ToString()].Split(';');
                    PublicFun.SendErrorLineForSERP(Common.GetEnumDesc(EnumSystemID.ERPAP), lineID, to, exception);
                }

                string teamsID = ConfigurationManager.AppSettings[EnumAppSettingKey.TeamsTo.ToString()];
                if (string.IsNullOrWhiteSpace(teamsID) == false)
                {
                    PublicFun.SendErrorTeamsForSERP(teamsID, Common.GetEnumDesc(EnumSystemID.ERPAP), userId, userName, exception); ;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }

        public bool GetCMCodeDictionary(
            EnumCultureID cultureID,
            Entity_BaseAP.EnumCMCodeItemTextType enumCMCodeItemTextType,
            params Entity_BaseAP.EnumCMCodeKind[] enumCMCodeKinds)
        {
            try
            {
                var entity = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP);

                CMCodeDictionary =
                    (from s in enumCMCodeKinds
                     select new
                     {
                         key = s,
                         value = ((IEnumerable<DBEntity.IExtendedSelectItem>)entity.SelectCMCodeList(new Entity_BaseAP.CMCodePara
                         {
                             CodeKind = s,
                             ItemTextType = enumCMCodeItemTextType,
                             CultureID = new DBVarChar(cultureID)
                         })).ToList()
                     }).ToDictionary(key => key.key, value => value.value);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        #region - 取的選單預設單一文字 -
        /// <summary>
        /// 取的選單預設單一文字
        /// </summary>
        /// <param name="selectItems">Entity集合</param>
        /// <param name="value">預設值</param>
        /// <returns>預設文字</returns>
        public string GetSelectedText(IEnumerable<DBEntity.ISelectItem> selectItems, string value)
        {
            if (selectItems == null)
            {
                return null;
            }
            return selectItems.Where(a => a.ItemValue() == value).Select(s => s.ItemText()).SingleOrDefault();
        }

        /// <summary>
        /// 取的選單預設單一文字
        /// </summary>
        /// <param name="selectItems">集合</param>
        /// <param name="value">預設值</param>
        /// <returns>預設文字</returns>
        public string GetSelectedText(IEnumerable<SelectListItem> selectItems, string value)
        {
            if (selectItems == null)
            {
                return null;
            }
            return selectItems.Where(a => a.Value == value).Select(s => s.Text).SingleOrDefault();
        }

        /// <summary>
        /// 取的選單預設單一文字
        /// </summary>
        /// <param name="dictionary">Dictionary</param>
        /// <param name="value">預設值</param>
        /// <returns>預設文字</returns>
        public string GetSelectedText(Dictionary<string, string> dictionary, string value)
        {
            return string.IsNullOrWhiteSpace(value) == false && dictionary.ContainsKey(value)
                ? dictionary[value]
                : null;
        }
        #endregion
        #endregion

        #region - Log -

        public bool GetRecordUserFunctionResult(string userID, string erpWFNo, string memo, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.UserFunctionPara para = new Entity_BaseAP.UserFunctionPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                };

                List<Entity_BaseAP.UserFunction> userFunctionList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserFunctionList(para);

                List<Mongo_BaseAP.RecordUserFunctionPara> mongoParaList = new List<Mongo_BaseAP.RecordUserFunctionPara>();

                if (userFunctionList != null)
                {
                    Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                        UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                        ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                    };

                    Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectBasicInfo(basicInfoPara);

                    if (userFunctionList.Count == 0)
                    {
                        mongoParaList.Add(new Mongo_BaseAP.RecordUserFunctionPara()
                        {
                            UserID = entityBasicInfo.UserID,
                            UserNM = entityBasicInfo.UserNM,
                            SysID = new DBVarChar(null),
                            SysNM = new DBNVarChar(null),
                            FunControllerID = new DBVarChar(null),
                            FunGroupNM = new DBNVarChar(null),
                            FunActionNM = new DBVarChar(null),
                            FunNM = new DBNVarChar(null),
                            ErpWFNO = new DBVarChar(string.IsNullOrWhiteSpace(erpWFNo) ? null : erpWFNo),
                            Memo = new DBNVarChar(string.IsNullOrWhiteSpace(memo) ? null : memo)
                        });
                    }
                    else
                    {
                        foreach (Entity_BaseAP.UserFunction userFunction in userFunctionList)
                        {
                            mongoParaList.Add(new Mongo_BaseAP.RecordUserFunctionPara()
                            {
                                UserID = entityBasicInfo.UserID,
                                UserNM = entityBasicInfo.UserNM,
                                SysID = userFunction.SysID,
                                SysNM = userFunction.SysNM,
                                FunControllerID = userFunction.FunControllerID,
                                FunGroupNM = userFunction.FunGroupNM,
                                FunActionNM = userFunction.FunActionNM,
                                FunNM = userFunction.FunNM,
                                ErpWFNO = new DBVarChar(string.IsNullOrWhiteSpace(erpWFNo) ? null : erpWFNo),
                                Memo = new DBNVarChar(string.IsNullOrWhiteSpace(memo) ? null : memo)
                            });
                        }
                    }
                }

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_FUN, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, mongoParaList);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetRecordUserAccessResult(string userID, string restrictType, string isLock, string isDisable, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(para);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.RestrictType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> restrictTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                Mongo_BaseAP.RecordUserAccessPara recordPara = new Mongo_BaseAP.RecordUserAccessPara()
                {
                    UserID = entityBasicInfo.UserID,
                    UserNM = entityBasicInfo.UserNM,
                    RestrictType = new DBChar((string.IsNullOrWhiteSpace(restrictType) ? null : restrictType)),
                    RestrictTypeNM = new DBNVarChar(null),
                    IsLock = new DBChar((string.IsNullOrWhiteSpace(isLock) ? null : isLock)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(isDisable) ? null : isDisable)),
                    APINo = new DBChar(null),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (restrictTypeList != null && restrictTypeList.Count > 0 &&
                    !string.IsNullOrWhiteSpace(restrictType))
                {
                    recordPara.RestrictTypeNM = (restrictTypeList.Find(e => e.CodeID.GetValue() == restrictType)).CodeNM;
                }

                // if (new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP)
                //     .RecordUserAccess(recordPara) == Mongo_BaseAP.EnumRecordUserAccessResult.Success)
                // {
                //     return true;
                // }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        protected void RecordLog<TSource>(Mongo_BaseAP.EnumLogDocName docName, Mongo_BaseAP.EnumModifyType modifyType, string updUserID, string ipAddress, EnumCultureID cultureID, TSource source)
        {
            try
            {
                var mongoEntity = new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP);
                var mongoPara = _GetRecordLogPara(docName, modifyType, updUserID, ipAddress, cultureID);
                mongoEntity.RecordLog(docName, mongoPara, source);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        protected void RecordLog<TSource>(Mongo_BaseAP.EnumLogDocName docName, Mongo_BaseAP.EnumModifyType modifyType, string updUserID, string ipAddress, EnumCultureID cultureID, List<TSource> source)
        {
            try
            {
                var mongoEntity = new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP);
                var mongoPara = _GetRecordLogPara(docName, modifyType, updUserID, ipAddress, cultureID);
                mongoEntity.RecordLog(docName, mongoPara, source);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        private Mongo_BaseAP.RecordLogPara _GetRecordLogPara(Mongo_BaseAP.EnumLogDocName docName, Mongo_BaseAP.EnumModifyType modifyType, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            var mongoEntity = new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP);
            var recordPara = (Mongo_BaseAP.RecordLogPara)Activator.CreateInstance(mongoEntity.GetRecordLogParaType(docName));

            try
            {
                List<DBVarChar> methodFullNameList = new List<DBVarChar>();
                for (int methodIndex = 2; methodIndex < 10; methodIndex++)
                {
                    MethodBase methodBase = Common.GetMethodBase(methodIndex);

                    if (methodBase != null)
                    {
                        var methodFullName = string.Concat(methodBase.ReflectedType.FullName, ".", methodBase.Name);
                        methodFullNameList.Add(new DBVarChar(methodFullName));
                        if (methodFullName.StartsWith(string.Format("{0}.Controllers", EnumSystemID.ERPAP)))
                        {
                            break;
                        }
                    }
                }

                Entity_BaseAP.BasicInfoPara para =
                    new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                    {
                        UserID = new DBVarChar(null),
                        UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                        ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                    };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(para);

                Entity_BaseAP.CMCodePara codePara =
                    new Entity_BaseAP.CMCodePara
                    {
                        ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                        CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                        CodeParent = new DBVarChar(null),
                        CultureID = new DBVarChar(cultureID.ToString())
                    };

                List<Entity_BaseAP.CMCode> modifyTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                if (modifyTypeList != null &&
                    modifyTypeList.Any())
                {
                    recordPara.ModifyTypeNM = modifyTypeList.Find(e => e.CodeID.GetValue() == modifyType.ToString()).CodeNM;
                }
                else
                {
                    recordPara.ModifyTypeNM = new DBNVarChar(null);
                }

                recordPara.ModifyType = new DBChar(modifyType.ToString());
                recordPara.UpdUserID = entityBasicInfo.UpdUserID;
                recordPara.UpdUserNM = entityBasicInfo.UpdUserNM;
                recordPara.UpdDT = new DBDateTime(DateTime.Now);
                recordPara.ExecSysID = entityBasicInfo.ExecSysID;
                recordPara.ExecSysNM = entityBasicInfo.ExecSysNM;
                recordPara.ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress));
                recordPara.MethodFullNameList = methodFullNameList;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return recordPara;
        }
        #endregion
    }
}
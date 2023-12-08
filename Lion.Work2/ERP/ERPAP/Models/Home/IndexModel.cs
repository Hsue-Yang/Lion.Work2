using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Home
{
    public enum EnumUserAuthenticationResult
    {
        OtherError,
        Success,
        PasswordExpires,
        PasswordError
    }

    public interface IUserAuthentication
    {
        EnumUserAuthenticationResult Result { get; set; }
    }

    public class ExApiResponse<TModel> : IUserAuthentication
    {
        public EnumUserAuthenticationResult Result { get; set; }
        public string rDesc { get; set; }
        public string rCode { get; set; }
        public string TokenExpires { get; set; }
        public TModel Data { get; set; }
    }

    public class AuthenticationOTPResult
    {
        public EnumAuthenticationOTP authResultCode { get; set; }
        public string authResultMsg { get; set; }
        public bool isVerified { get; set; }
        public int infoCode { get; set; }
        public string message { get; set; }
    }
    
    public enum EnumAuthenticationOTP
    {
        None = 0,

        Success = 1,

        [Description("OTP驗證錯誤")]
        OTPError = 2,

        [Description("OTP系統錯誤")]
        EmpOTPSystemError = 3,

        [Description("OTP驗證時間超過")]
        EmpOTPAuthOvertime = 4,

        [Description("禁用IP")]
        DisableIP = 5,

        [Description("錯誤次數太多")]
        TryPassword = 6,

        [Description("外部IP,但位置選擇公司")]
        LocationeErrorExtraIP = 7,

        [Description("內部IP,但位置選擇非公司")]
        LocationeErroriIntraIP = 8,

        [Description("非公司ip，身分證＆生日異常")]
        ExtraIPBasicDataError = 9,

        [Description("帳號不存在")]
        EmpNotExist = 10,

        [Description("帳號鎖定")]
        Locked = 11,

        [Description("員工離職")]
        EmpTurnover = 12,

        [Description("參數錯誤")]
        ParameterError = 13,

        [Description("OTP密碼不可重複使用，請換一組OTP密碼")]
        OTPCacheError = 14,

        [Description("OTP其他錯誤")]
        EmpOTPOtherError = 1000
    }

    public class IndexModel : HomeModel, IUserAccountInfo
    {
        public class TokenModel
        {
            public TokenDataModel Data { get; set; }
            
            public string rDesc { get; set; }

            /// <summary>
            /// 錯誤代碼
            /// </summary>
            public string rCode { get; set; }

            /// <summary>
            /// Access Token 到期時間，每次成功呼叫會自動延長
            /// </summary>
            public string TokenExpires { get; set; }
        }

        public class TokenDataModel
        {
            /// <summary>
            /// Token
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// 建立時間
            /// </summary>
            public DateTime CreateDateTime { get; set; }

            /// <summary>
            /// 過期時間
            /// </summary>
            public DateTime ExpireDateTime { get; set; }
        }

        public new enum EnumCookieKey 
        {
            LoginType
        }

        public enum EnumLoginType
        {
            AccountPSW,
            QRCode,
            DigipassOTP
        }
        
        public string CultureID { get; set; }

        [AllowHtml]
        [InputType(EnumInputType.TextBoxHidden)]
        public string TargetUrl { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string ProxyLoginSystemID { get; set; }

        public string UserToken { get; set; }

        //[Required]
        public string LoginType { get; set; }

        private string _UserID;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID { 
            get {
                if (string.IsNullOrWhiteSpace(_UserID))
                {
                    return _UserID;
                }
                return _UserID.ToUpper();
            }
            set {
                _UserID = value;
            }
        }

        [Required]
        [StringLength(14)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string UserPassword { get; set; }

        public string UserLocation { get; set; }

        [StringLength(20)]
        [InputType(EnumInputType.TextBox)]
        public string LocationDesc { get; set; }

        private string _IdNo;

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string IdNo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_IdNo))
                {
                    return _IdNo;
                }
                return _IdNo.ToUpper();
            }
            set
            {
                _IdNo = value;
            }
        }

        [StringLength(8)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string Birthday { get; set; }

        public string EnvironmentRemind { get; set; }

        public string ProxyLoginToAp { get; set; }

        public IndexModel()
        {
            UserID = string.Empty;
            UserPassword = string.Empty;
            IdNo = string.Empty;
            Birthday = string.Empty;
            UserLocation = LocationEnum.COMP.ToString();
            EnvironmentRemind = string.Empty;
        }

        public void FormReset()
        {
            UserPassword = string.Empty;
            IdNo = string.Empty;
            Birthday = string.Empty;
        }
        
        public List<Entity_BaseAP.CMCode> EntityBaseCultureIDList { get; private set; }

        public bool GetBaseCultureIDList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.Culture,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                EntityBaseCultureIDList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public string GetSystemName(string sysID, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar(null),
                    ExecSysID = new DBVarChar(sysID)
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(para);

                if (entityBasicInfo != null)
                {
                    return entityBasicInfo.ExecSysNM.GetValue();
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }

        public bool GetValidUserAccountResult()
        {
            try
            {
                Entity_BaseAP.UserAccountPara para = new Entity_BaseAP.UserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(UserPassword) ? null : UserPassword))
                };

                return new Entity_BaseAP(ConnectionStringERP, ProviderNameERP).ValidUserAccount(para);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        
        public bool GetRecordUserLoginResult(IUserAuthentication iUserAuthentication, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string authResultCode;
                string authResultMsg;
                string rCode;
                string rDesc;

                if (iUserAuthentication.GetType().GenericTypeArguments[0] == typeof(AuthenticationOTPResult))
                {
                    var employee = (ExApiResponse<AuthenticationOTPResult>)iUserAuthentication;
                    authResultCode = employee.Data.authResultCode.ToString();
                    authResultMsg = employee.Data.authResultMsg;
                    rCode = employee.rCode;
                    rDesc = string.IsNullOrWhiteSpace(employee.rDesc) ? null : employee.rDesc;
                }
                else
                {
                    var employee = (ExApiResponse<AuthenticationResult>)iUserAuthentication;
                    authResultCode = employee.Data.authResultCode.ToString();
                    authResultMsg = employee.Data.authResultMsg;
                    rCode = employee.rCode;
                    rDesc = string.IsNullOrWhiteSpace(employee.rDesc) ? null : employee.rDesc;
                }

                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(UserID) ? null : UserID),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(para);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.UserLocation,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> userLocationList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                Mongo_BaseAP.RecordUserLoginPara recordPara = new Mongo_BaseAP.RecordUserLoginPara()
                {
                    UserID = entityBasicInfo.UserID,
                    UserNM = entityBasicInfo.UserNM,
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(UserPassword) ? null : Security.Encrypt(UserPassword))),
                    LoginType = new DBVarChar((string.IsNullOrWhiteSpace(LoginType) ? null : LoginType)),
                    Location = new DBVarChar((string.IsNullOrWhiteSpace(UserLocation) ? null : UserLocation)),
                    LocationNM = new DBNVarChar(null),
                    LocationDesc = new DBNVarChar((string.IsNullOrWhiteSpace(LocationDesc) ? null : LocationDesc)),
                    UserIDNo = new DBVarChar((string.IsNullOrWhiteSpace(IdNo) ? null : Security.Encrypt(IdNo))),
                    UserBirthday = new DBVarChar((string.IsNullOrWhiteSpace(Birthday) ? null : Security.Encrypt(Birthday))),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress)),
                    ValidResult = new DBVarChar(authResultCode),
                    ValidResultNM = new DBNVarChar(authResultMsg),
                    ProxyLoginSystemID = new DBVarChar(ProxyLoginSystemID),
                    TargetUrl = new DBNVarChar((string.IsNullOrWhiteSpace(TargetUrl) ? null : TargetUrl)),
                    ExApiErrorCode = new DBVarChar(rCode),
                    ExApiErrorDesc = new DBNVarChar(rDesc),
                    APINo = new DBChar(null),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (userLocationList != null && userLocationList.Count > 0 &&
                    !string.IsNullOrWhiteSpace(UserLocation))
                {
                    recordPara.LocationNM = (userLocationList.Find(e => e.CodeID.GetValue() == UserLocation)).CodeNM;
                }

                if (new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP)
                    .RecordUserLogin(recordPara) == Mongo_BaseAP.EnumRecordUserLoginResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}
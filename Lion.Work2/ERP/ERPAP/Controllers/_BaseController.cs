using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using ERPAP.Models;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ERPAP
{
    public enum EnumAppSettingKey
    {
        EDIServiceDistributor, EDIServiceDistributorURL, EDIServiceDistributorTimeOut, EDIServicePath,
        RootPath, FilePath,
        AllowExternalClientIPAddress, ValidationClientIPAddressString, ValidationServerIPAddressString,
        StateServerMachineName,
        ServiceControllerIPAddress, ServiceControllerPort,
        ASPLoginURL, ASPMisTokenRedirectURL, ASPTokenDelayURL, ASPPUBPub003URL, SysSDMail, SmtpClientIPAddress, LineTo, TeamsTo,
        LineID, IsLoginExApiValidation,
        [Description(@"LionTech:EXAPITokenKey")]
        LionTechEXAPITokenKey,
        IsOpenLoginLink,
        JwtEnable, SSODomain, SSOScopeURL
    }

    public enum EnumRootPathFile
    {
        [Description(@"Log\{0}.err")]
        Exception,
        [Description(@"Distributor\{0}.log")]
        Distributor,
        [Description(@"SSOLogin\{0}.log")]
        SSOLogin
    }

    public enum EnumFilePathFolder
    {
        [Description(@"UserMenu\")]
        UserMenu,
        [Description(@"EDIService\EventPara\{0}\{1}.para")]
        EDIServiceEventPara,
        [Description(@"WorkFlow\Chart\")]
        WorkFlow,
    }

    public enum EnumAppDataFile
    {
        [Description("UserMenuContent.xsl")]
        UserMenuContentXSL
    }
}

namespace ERPAP.Controllers
{
    public class _BaseController : Controller
    {
        LionTech.Entity.ERP.EnumCultureID _cultureID;
        protected LionTech.Entity.ERP.EnumCultureID CultureID { get { return _cultureID; } }

        protected string userMenuRedisKey = "serp:usermenu:{0}";

        public string HostIPAddress()
        {
            return Common.GetHostIPAddress(Request.ServerVariables);
        }

        public string ClientIPAddress()
        {
            return Common.GetClientIPAddress(Request.ServerVariables);
        }

        public bool ValidateIPAddressIsInternalServer()
        {
            string clientIPAddress = Common.GetClientIPAddress(Request.ServerVariables);
            string validationServerIPAddressString = ConfigurationManager.AppSettings[EnumAppSettingKey.ValidationServerIPAddressString.ToString()];

            return Common.ValidateIPAddress(clientIPAddress, validationServerIPAddressString);
        }

        public bool ValidateIPAddressIsInternalClient()
        {
            string clientIPAddress = Common.GetClientIPAddress(Request.ServerVariables);
            string validationClientIPAddressString = ConfigurationManager.AppSettings[EnumAppSettingKey.ValidationClientIPAddressString.ToString()];

            return Common.ValidateIPAddress(clientIPAddress, validationClientIPAddressString);
        }

        public bool ValidateIPAddressIsInternal()
        {
            if (this.ValidateIPAddressIsInternalServer() || this.ValidateIPAddressIsInternalClient())
            {
                return true;
            }
            return false;
        }

        protected bool GetEDIServiceDistributor()
        {
            return bool.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.EDIServiceDistributor.ToString()]);
        }

        protected string GetEDIServiceDistributorPath(string sysID, string eventGroupID, string eventID, string userID, string eventPara)
        {
            return string.Format(
                ConfigurationManager.AppSettings[EnumAppSettingKey.EDIServiceDistributorURL.ToString()],
                new string[]
                    {
                        Common.GetEnumDesc(EnumAPISystemID.ERPAP), 
                        EnumSystemID.ERPAP.ToString(), userID ,sysID, eventGroupID, eventID, userID, eventPara
                    });
        }

        protected int GetEDIServiceDistributorTimeOut()
        {
            return int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.EDIServiceDistributorTimeOut.ToString()]);
        }

        protected string GetRootPathFilePath(EnumRootPathFile enumRootPathFile)
        {
            return Path.Combine(
                new string[]
                    {
                        ConfigurationManager.AppSettings[EnumAppSettingKey.RootPath.ToString()], 
                        string.Format(Common.GetEnumDesc(enumRootPathFile), Common.GetDateString())
                    });
        }

        protected string GetRootPathFilePath(EnumRootPathFile enumRootPathFile, string[] paras)
        {
            return Path.Combine(
                new string[]
                    {
                        ConfigurationManager.AppSettings[EnumAppSettingKey.RootPath.ToString()], 
                        string.Format(Common.GetEnumDesc(enumRootPathFile), paras)
                    });
        }

        protected string GetFilePathFolderPath(EnumFilePathFolder enumFilePathFolder)
        {
            return Path.Combine(
                new string[]
                    {
                        ConfigurationManager.AppSettings[EnumAppSettingKey.FilePath.ToString()], 
                        string.Format(Common.GetEnumDesc(enumFilePathFolder), Common.GetDateString())
                    });
        }

        protected string GetFilePathFolderPath(EnumFilePathFolder enumFilePathFolder, string[] paras)
        {
            return Path.Combine(
                new string[]
                    {
                        ConfigurationManager.AppSettings[EnumAppSettingKey.FilePath.ToString()], 
                        string.Format(Common.GetEnumDesc(enumFilePathFolder), paras)
                    });
        }

        protected string GetUserMenuFilePath(string userID, LionTech.Entity.ERP.EnumCultureID cultureID)
        {
            if (string.IsNullOrWhiteSpace(userID))
                userID = _authState.SessionData.UserID;

            return Path.Combine(
                new string[]
                    {
                        GetFilePathFolderPath(EnumFilePathFolder.UserMenu),
                        userID,
                        string.Format("UserMenu.{0}{1}.xml", userID, cultureID == LionTech.Entity.ERP.EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(cultureID))
                    });
        }

        protected string GetAppDataFilePath(EnumAppDataFile enumAppDataFile)
        {
            return Server.MapPath(string.Concat(new object[] { @"~\App_Data\", Common.GetEnumDesc(enumAppDataFile) }));
        }

        protected string GetAppGlobalResourcesFilePath()
        {
            string dirPath =
                string.Concat(
                    $@"~\App_GlobalResources\{_authState.ControllerName}\",
                    _authState.ControllerName, _authState.ActionName);

            string fileName = string.Concat(dirPath, $".{Common.GetEnumDesc(CultureID)}.resx");

            if (CultureID != LionTech.Entity.ERP.EnumCultureID.zh_TW &&
                System.IO.File.Exists(fileName))
            {
                return fileName;
            }

            return Server.MapPath(string.Concat(dirPath, ".resx"));
        }

        AuthState _authState;
        public AuthState AuthState
        {
            get { return _authState; }
        }

        public string UserSystemFunKey
        {
            get { return string.Format("{0}|{1}|{2}|{3}", _authState.SessionData.UserID, _authState.SystemID.ToString(), _authState.ControllerName, _authState.ActionName); }
        }

        bool _hasFunToolDataList = false;
        public bool HasFunToolDataList
        {
            get { return _hasFunToolDataList; }
        }

        FunToolData _funToolData;
        public FunToolData FunToolData
        {
            get { return _funToolData; }
        }

        List<FunToolData> _funToolDataList;
        public List<FunToolData> FunToolDataList
        {
            get { return _funToolDataList; }
        }

        bool _isPostBack = false;
        public bool IsPostBack
        {
            get { return _isPostBack; }
        }

        bool _isSystemError;
        protected bool IsSystemError { get { return _isSystemError; } }

        int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
        }

        public _BaseController()
        {
            _cultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW;
            if (string.IsNullOrWhiteSpace(System.Threading.Thread.CurrentThread.CurrentCulture.Name) == false)
            {
                _cultureID = LionTech.Entity.ERP.Utility.GetCultureID(System.Threading.Thread.CurrentThread.CurrentCulture.Name.Replace("-", "_"));
            }
        }

        protected virtual void OnActionExecutingSetAuthState()
        {

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _authState = new AuthState(
                EnumSystemID.ERPAP, 
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, 
                filterContext.ActionDescriptor.ActionName, 
                this.Session, Request.Cookies, Response.Cookies);
            _authState.UnAuthorizedActionResult = RedirectToAction("UnAuthorizated", "Home");
            _authState.RejectiveActionResult = RedirectToAction("Rejective", "Home");
            //_authState.PWDExpiredActionResult = RedirectToAction("UserMain", "Sys");
            this.OnActionExecutingSetAuthState();

            if (filterContext.ActionParameters != null && filterContext.ActionParameters.Count > 0)
            {
                object baseModel;
                if (filterContext.ActionParameters.TryGetValue("model", out baseModel))
                {
                    if (((_BaseModel)baseModel).SystemControllerAction ==
                        string.Concat(new string[] { 
                        EnumSystemID.ERPAP.ToString(), 
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, 
                        filterContext.ActionDescriptor.ActionName 
                    }))
                    {
                        _isPostBack = true;
                    }
                }
            }

            SetPageSize(filterContext);

            this.ViewData.Set<string>(EnumViewDataItem.JsMsg, Common.GetJsMsgResources(GetAppGlobalResourcesFilePath(), "JSMSG_"));
            this.ViewData.Set<string>(EnumViewDataItem.EditionNo, Security.Encrypt(AuthState.SessionData.SessionID));
            this.ViewData.Set<string>(EnumViewDataItem.UserID, AuthState.SessionData.UserID);
            this.ViewData.Set<string>(EnumViewDataItem.UserNM, AuthState.SessionData.UserNM);

            if (AuthState.IsAuthorized &&
                AuthState.IsLogined &&
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Home")
            {
                this.SetSysFunToolData();
                this.OnSetERPUserMessage();

                // TODO:使用者系統通知,測試階段
                if (Common.GetEnumDesc(EnumSystemID.Domain) == "liontravel.com.tw")
                {
                    //this.OnSetUserSystemNotifications();
                }
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (_authState.IsRejective == true)
            {
                filterContext.Result = _authState.RejectiveActionResult;
            }
            else if (_authState.IsAuthorized == false)
            {
                filterContext.Result = _authState.UnAuthorizedActionResult;
            }
            //else if (_authState.IsPWDExpired == true)
            //{
            //    filterContext.Result = _authState.PWDExpiredActionResult;
            //}
            else if (_authState.IsLogined == true)
            {
                this.ViewData.Set<string>(EnumViewDataItem.UserMenu, AuthState.SessionData.UserMenu);

                SetModelSystemControllerAction(filterContext);
                SetModelPageSize(filterContext);
                SetModelWorkFlowData(filterContext);
                this.SetSysFunToolData();
                this.SetSysFunToolEnable(filterContext);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), filterContext.Exception);

            filterContext.ExceptionHandled = false;
            //if (filterContext.HttpContext.IsCustomErrorEnabled == true) { filterContext.ExceptionHandled = false; }
        }

        protected void SetUserMenu(EnumUserMenuID focusedUserMenuID, EnumCultureID cultureID)
        {
            string userMenu;

            if (LionTechAppSettings.ServerEnvironment == EnumServerEnvironment.Developing)
            {
                userMenu = PublicFun.GetUserMenu(
                    AuthState.SessionData.UserMenuXSLString,
                    AuthState.SessionData.FilePathUserMenu,
                    AuthState.SessionData.UserID,
                    focusedUserMenuID);
            }
            else
            {
                string redisKey = string.Format(userMenuRedisKey, AuthState.SessionData.UserID);

                if (RedisConnection.RedisCache.HashExists(redisKey, cultureID.ToString()) == false)
                {
                    RedisConnection.RedisCache.KeyDelete(redisKey);
                    var model = new _BaseAPModel();
                    var zh_TW = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.zh_TW);
                    var zh_CN = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.zh_CN);
                    var en_US = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.en_US);
                    var th_TH = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.th_TH);
                    var ja_JP = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.ja_JP);
                    var ko_KR = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.ko_KR);
                    GenerateUserMenu(AuthState.SessionData.UserID, zh_TW, zh_CN, en_US, th_TH, ja_JP, ko_KR);
                }

                var userMenuXml = RedisConnection.RedisCache.HashGet(redisKey, cultureID.ToString());
                XDocument xml = XDocument.Parse(userMenuXml);
                userMenu = PublicFun.GetUserMenu(
                    AuthState.SessionData.UserMenuXSLString,
                    xml,
                    AuthState.SessionData.UserID,
                    focusedUserMenuID);
            }

            if (LionTechAppSettings.ServerEnvironment == EnumServerEnvironment.UplanProduction &&
                string.IsNullOrWhiteSpace(userMenu) == false)
            {
                userMenu = userMenu.Replace("liontravel", "uplantravel");
            }
           
            _authState.SessionData.UserMenu = userMenu;
            
            if (Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.JwtEnable.ToString()]))
            {
                string redisActivityKey = $"serp:activity:{AuthState.SessionData.UserID}:{AuthState.SessionData.SessionID}";
                RedisConnection.RedisCache.KeyDelete(redisActivityKey);
                RedisConnection.RedisCache.HashSet(redisActivityKey, new HashEntry[]
                {
                    new HashEntry("UserMenu", _authState.SessionData.UserMenu),
                    new HashEntry("SessionData", JsonConvert.SerializeObject(new
                    {
                        _authState.SessionData.UserRoleIDs,
                        EditionNo = Security.Encrypt(AuthState.SessionData.SessionID),
                        UserSystemIDs = _authState.SessionData.UserSystemIDs.Select(s => s.ToString()),
                        _authState.SessionData.UserComID,
                        _authState.SessionData.UserUnitID,
                        _authState.SessionData.UserUnitNM,
                        UserWorkDiffHour = _authState.SessionData.APSession(EnumSessionKey.UserWorkDiffHour.ToString())
                    }))
                });
                RedisConnection.RedisCache.KeyExpire(redisActivityKey, TimeSpan.FromHours(1));
            }
        }

        protected void GenerateUserMenu(string userID, string zh_TW, string zh_CN, string en_US, string th_TH, string ja_JP, string ko_KR)
        {
            string redisKey = string.Format(userMenuRedisKey, userID);
            RedisConnection.RedisCache.KeyDelete(redisKey);
            RedisConnection.RedisCache.HashSet(redisKey, new HashEntry[] {
                new HashEntry(LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString(),zh_TW),
                new HashEntry(LionTech.Entity.ERP.EnumCultureID.zh_CN.ToString(),zh_CN),
                new HashEntry(LionTech.Entity.ERP.EnumCultureID.en_US.ToString(),en_US),
                new HashEntry(LionTech.Entity.ERP.EnumCultureID.th_TH.ToString(),th_TH),
                new HashEntry(LionTech.Entity.ERP.EnumCultureID.ja_JP.ToString(),ja_JP),
                new HashEntry(LionTech.Entity.ERP.EnumCultureID.ko_KR.ToString(),ko_KR),
            });
        }

        #region - 驗證指定的模型執行個體 -
        /// <summary>
        /// 驗證指定的模型執行個體
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool TryValidatableObject(object model)
        {
            if (LionTech.Web.ERPHelper.Validator.TryValidatableObject(ModelState, model))
            {
                return true;
            }
            SetSystemAlertMessage(ModelState);
            return false;
        }
        #endregion

        #region - 設定模組系統彈跳訊息 -
        /// <summary>
        /// 設定模組系統彈跳訊息
        /// </summary>
        private void SetSystemAlertMessage(ModelStateDictionary modelState)
        {
            if (IsSystemError)
            {
                return;
            }

            var error = GetSystemAlertMessage(modelState);

            if (error.Any())
            {
                SetSystemAlertMessage(string.Join("<br/>", error.Skip(0).Take(3)));
            }
        }
        #endregion

        #region - 取得模組系統彈跳訊息 -
        /// <summary>
        /// 取得模組系統彈跳訊息
        /// </summary>
        protected List<string> GetSystemAlertMessage(ModelStateDictionary modelState, string resourceActionNm = null)
        {
            resourceActionNm = resourceActionNm ?? ControllerContext.RouteData.Values["action"].ToString();
            return
                (from s in modelState
                 where s.Value.Errors.Any()
                 select s.Value.Errors.Select(s1 =>
                 {
                     if (!string.IsNullOrWhiteSpace(s.Key))
                     {
                         var name = s.Key;
                         if (s.Key.IndexOf('.') > -1)
                         {
                             var spName = s.Key.Split('.');
                             name = spName[spName.Length - 1];
                         }

                         var resourceMsg = (string)HttpContext.GetGlobalResourceObject(AuthState.ControllerName + resourceActionNm, "Model_" + name);

                         if (s1.ErrorMessage.IndexOf(name, StringComparison.Ordinal) > -1)
                         {
                             return string.IsNullOrWhiteSpace(resourceMsg)
                                 ? s1.ErrorMessage
                                 : s1.ErrorMessage.Replace(name, resourceMsg);
                         }
                         return string.Format("{0} {1}", resourceMsg, s1.ErrorMessage);
                     }
                     return s1.ErrorMessage;
                 }).ToList()).ToList().SelectMany(sm => sm).ToList();
        }
        #endregion

        protected void SetSystemErrorMessage(string message)
        {
            if (_isSystemError == false)
            {
                TempData["SystemErrorMessage"] = message;
            }
            _isSystemError = true;
        }

        protected void SetSystemAlertMessage(string message)
        {
            if (_isSystemError == false)
            {
                TempData["SystemAlertMessage"] = message;
            }
            _isSystemError = true;
        }

        protected virtual void OnSetERPUserMessage()
        {
        }

        protected virtual void OnSetUserSystemNotifications()
        {
        }

        protected void SetModelSystemControllerAction(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(ViewResult))
            {
                ViewResultBase ViewResultBase = (ViewResultBase)filterContext.Result;
                if (ViewResultBase.Model != null)
                {
                    _BaseModel BaseModel = (_BaseModel)ViewResultBase.Model;
                    BaseModel.SystemControllerAction = string.Concat(new string[] { 
                        EnumSystemID.ERPAP.ToString(), 
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, 
                        filterContext.ActionDescriptor.ActionName 
                    });
                }
            }
        }

        protected void SetModelPageSize(ActionExecutedContext filterContext)
        {
            int pageSize = 0, tempPageSize;
            string[] pageSizeArray = filterContext.HttpContext.Request.Form.GetValues(Common.GetEnumDesc(EnumCookieName.PageSize));

            if (null != pageSizeArray && int.TryParse(pageSizeArray[0], out tempPageSize))
            {
                pageSize = tempPageSize;
            }
            else
            {
                if (AuthState.CookieData.PageSize != null)
                {
                    pageSize = int.Parse(AuthState.CookieData.PageSize);
                }
            }

            if (filterContext.Result.GetType() == typeof(ViewResult))
            {
                ViewResultBase ViewResultBase = (ViewResultBase)filterContext.Result;
                if (ViewResultBase.Model != null)
                {
                    _BaseModel BaseModel = (_BaseModel)ViewResultBase.Model;
                    BaseModel.PageSize = pageSize;
                }
            }
            AuthState.CookieData.SetPageSize(pageSize);
        }

        protected void SetPageSize(ActionExecutingContext filterContext)
        {
            string[] pageSizeArray = filterContext.HttpContext.Request.Form.GetValues(Common.GetEnumDesc(EnumCookieName.PageSize));
            int tempPageSize;

            if (null != pageSizeArray && int.TryParse(pageSizeArray[0], out tempPageSize))
            {
                _pageSize = tempPageSize;
            }
            else
            {
                if (AuthState.CookieData.PageSize != null)
                {
                    _pageSize = int.Parse(AuthState.CookieData.PageSize);
                }
            }

            _pageSize = _pageSize <= 0 ? 10 : _pageSize;
        }

        protected void SetModelWorkFlowData(ActionExecutedContext filterContext)
        {

        }

        protected void SetSysFunToolData()
        {
            _funToolData = new FunToolData();

            _BaseModel model = new _BaseModel();

            _funToolDataList = model.GetSysFunToolList(AuthState.SessionData.UserID, AuthState.SystemID.ToString(), AuthState.ControllerName, AuthState.ActionName);

            if (_funToolDataList != null && _funToolDataList.Count > 0)
            {
                _hasFunToolDataList = true;

                FunToolData currentlyFunToolData = new FunToolData();

                try
                {
                    currentlyFunToolData = _funToolDataList.Where(e => e.IsCurrently == EnumYN.Y.ToString()).First();
                }
                catch
                {
                    currentlyFunToolData.ToolNo = string.Empty;
                    currentlyFunToolData.ToolNM = string.Empty;
                    currentlyFunToolData.IsCurrently = EnumYN.N.ToString();
                    currentlyFunToolData.ParaDict = new ExtensionDictionary<string, string>();
                }

                _funToolData.ToolNo = currentlyFunToolData.ToolNo;
                _funToolData.ToolNM = currentlyFunToolData.ToolNM;
                _funToolData.IsCurrently = currentlyFunToolData.IsCurrently;
                _funToolData.ParaDict = currentlyFunToolData.ParaDict;
            }
            else
            {
                _hasFunToolDataList = false;

                _funToolData.ToolNo = string.Empty;
                _funToolData.ToolNM = string.Empty;
                _funToolData.IsCurrently = EnumYN.N.ToString();
                _funToolData.ParaDict = new ExtensionDictionary<string, string>();
            }

            this.ViewData.Set<string>(EnumViewDataItem.SysFunToolNo, _funToolData.ToolNo);
            this.ViewData.Set<string>(EnumViewDataItem.SysFunToolNM, _funToolData.ToolNM);
            this.ViewData.Set<object>(EnumViewDataItem.SysFunToolDict, model.GetDictionaryFormSelectItem(_funToolDataList, true));
        }

        protected void SetSysFunToolEnable(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(ViewResult))
            {
                UseFunToolAttribute funToolAttribute = (UseFunToolAttribute)filterContext.ActionDescriptor
                    .GetCustomAttributes(typeof(UseFunToolAttribute), true).FirstOrDefault();

                if (funToolAttribute != null)
                {
                    this.ViewData.Set<string>(EnumViewDataItem.SysFunToolIsEnable, funToolAttribute.GetEnableStatus());
                }
                else
                {
                    this.ViewData.Set<string>(EnumViewDataItem.SysFunToolIsEnable, false);
                }
            }
        }

    }

    public class AuthorizationActionFilter : ActionFilterAttribute
    {
        string _actionName;

        public AuthorizationActionFilter()
        {

        }

        public AuthorizationActionFilter(string actionName)
        {
            _actionName = actionName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string ipAddress = ((_BaseController)filterContext.Controller).ClientIPAddress();

            Entity_Base.EnumValidateIPAddressIsTrustResult trustResult = new _BaseModel().GetValidateIPAddressIsTrust(ipAddress);
            if (trustResult == Entity_Base.EnumValidateIPAddressIsTrustResult.Reject)
            {
                ((_BaseController)filterContext.Controller).AuthState.IsRejective = true;
            }

            ((_BaseController)filterContext.Controller).AuthState.IsAuthorized = false;
            if (string.IsNullOrWhiteSpace(_actionName) == false)
            {
                ((_BaseController)filterContext.Controller).AuthState.SetActionName(_actionName);
            }

            //((_BaseController)filterContext.Controller).AuthState.IsPWDExpired = new _BaseModel()
            //    .GetValidateUserPWDIsExpired(((_BaseController)filterContext.Controller).AuthState.SessionData.UserID);

            Entity_Base.RestrictType validateResult = new Entity_Base.RestrictType();
            if (ipAddress != Common.GetEnumDesc(EnumSystemID.Domain))
            {
                validateResult = new _BaseModel().GetValidateUserIsRestrict(
                    ((_BaseController)filterContext.Controller).AuthState.SessionData.UserID,
                    ipAddress
                );
            }
            bool allowExternalIPAddress = bool.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.AllowExternalClientIPAddress.ToString()]);
            bool isTrustIPAddress = true;
            if (ipAddress != Common.GetEnumDesc(EnumSystemID.Domain))
            {
                isTrustIPAddress = (trustResult == Entity_Base.EnumValidateIPAddressIsTrustResult.Trust ? true : false);
            }

            if ((validateResult != null && validateResult.IsRestrict == false) && (allowExternalIPAddress || isTrustIPAddress))
            {
                HttpSessionStateBase httpSessionStateBase = filterContext.RequestContext.HttpContext.Session;
                if (httpSessionStateBase != null && httpSessionStateBase.Count > 0)
                {
                    EnumSystemID systemID = EnumSystemID.ERPAP; 
                    string controllerName = string.Empty;

                    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == LionTech.Utility.ERP.FunToolService.FunToolController._BaseAP.ToString())
                    {
                        if (filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.FunToolService.FunToolActionName.GetUpdateSysFunToolResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.FunToolService.FunToolActionName.GetCopySysFunToolResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.FunToolService.FunToolActionName.GetDeleteSysFunToolResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.FunToolService.FunToolActionName.GetUpdateSysFunToolNameResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.FunToolService.FunToolActionName.GetSelectSysFunToolNameList.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.FunToolService.FunToolActionName.GetBaseRAWUserList.ToString())
                        {
                            systemID = EnumSystemID.ERPAP;
                            controllerName = LionTech.Utility.ERP.FunToolService.FunToolController.Generic.ToString();
                        }
                    }
                    
                    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == LionTech.Utility.ERP.WorkFlowService.WFServiceController._BaseAP.ToString())
                    {
                        if (filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetWFNextNodeRoleUserList.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetNewFlowResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetNextToNodeResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetBackToNodeResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetTerminateFlowResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetSignatureResult.ToString() ||
                            filterContext.ActionDescriptor.ActionName == LionTech.Utility.ERP.WorkFlowService.WFServiceActionName.GetPickNewUserResult.ToString())
                        {
                            systemID = EnumSystemID.ERPAP;
                            controllerName = LionTech.Utility.ERP.WorkFlowService.WFServiceController.Generic.ToString();
                        }
                    }

                    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "_BaseAP")
                    {
                        // TODO:暫時
                        if (filterContext.ActionDescriptor.ActionName == "GetUserSystemNotificationList")
                        {
                            systemID = EnumSystemID.ERPAP;
                            controllerName = "Generic";
                        }
                    }

                    ((_BaseController)filterContext.Controller).AuthState.IsAuthorized = new _BaseModel().GetValidateUserSystemRoleFun(
                        ((_BaseController)filterContext.Controller).AuthState.SessionData.UserID,
                        systemID,
                        string.IsNullOrWhiteSpace(controllerName) ? filterContext.ActionDescriptor.ControllerDescriptor.ControllerName : controllerName,
                        string.IsNullOrWhiteSpace(_actionName) ? filterContext.ActionDescriptor.ActionName : _actionName,
                        ((_BaseController)filterContext.Controller).AuthState.SessionData.SessionID,
                        ipAddress,
          		        (isTrustIPAddress || validateResult.IsPowerUser) ? EnumYN.N : EnumYN.Y
          	       	);
                }
            }
        }
    }

    public class ExternalMonitoringActionFilter : ActionFilterAttribute
    {
        string _actionName;

        public ExternalMonitoringActionFilter()
        {

        }

        public ExternalMonitoringActionFilter(string actionName)
        {
            _actionName = actionName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ((_BaseController)filterContext.Controller).AuthState.IsAuthorized = true;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UseFunToolAttribute : Attribute
    {
        bool _isEnable { get; set; }

        public UseFunToolAttribute(bool isEnable)
        {
            this._isEnable = isEnable;
        }

        public bool GetEnableStatus()
        {
            return this._isEnable;
        }
    }
}

namespace ERPAP
{
    internal sealed class RedisConnection
    {
        public static IDatabase RedisCache { get; set; }

        public static void Init(string settingOption)
        {
            if (RedisCache == null)
            {
                RedisCache = ConnectionMultiplexer.Connect(settingOption).GetDatabase();
            }
        }
    }
}
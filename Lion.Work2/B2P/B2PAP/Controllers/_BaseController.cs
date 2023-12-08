using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using B2PAP.Models;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.B2P;

namespace B2PAP
{
    public enum EnumAppSettingKey
    {
        RootPath, FilePath,
        AllowExternalClientIPAddress, ValidationClientIPAddressString, ValidationServerIPAddressString
    }

    public enum EnumRootPathFile
    {
        [Description(@"Log\{0}.err")]
        Exception
    }

    public enum EnumFilePathFolder
    {
        [Description(@"UserMenu\")]
        UserMenu
    }

    public enum EnumAppDataFile
    {
        [Description("UserMenuContent.xsl")]
        UserMenuContentXSL
    }
}

namespace B2PAP.Controllers
{
    public class _BaseController : Controller
    {
        LionTech.Entity.B2P.EnumCultureID _cultureID;
        protected LionTech.Entity.B2P.EnumCultureID CultureID { get { return _cultureID; } }

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

        protected string GetUserMenuFilePath(string userID, LionTech.Entity.B2P.EnumCultureID cultureID)
        {
            if (string.IsNullOrWhiteSpace(userID))
                userID = _authState.SessionData.UserID;

            return Path.Combine(
                new string[]
                    {
                        GetFilePathFolderPath(EnumFilePathFolder.UserMenu),
                        userID,
                        string.Format("UserMenu.{0}{1}.xml", userID, cultureID == LionTech.Entity.B2P.EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(cultureID))
                    });
        }

        protected string GetAppDataFilePath(EnumAppDataFile enumAppDataFile)
        {
            return Server.MapPath(string.Concat(new object[] { @"~\App_Data\", Common.GetEnumDesc(enumAppDataFile) }));
        }

        protected string GetAppGlobalResourcesFilePath()
        {
            return
                Server.MapPath(
                    string.Concat(new object[]
                                      {
                                          @"~\App_GlobalResources\", _authState.ControllerName, @"\",
                                          _authState.ControllerName, _authState.ActionName,
                                          CultureID == LionTech.Entity.B2P.EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(CultureID),
                                          ".resx"
                                      }));
        }

        AuthState _authState;
        public AuthState AuthState
        {
            get { return _authState; }
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
            _cultureID = LionTech.Entity.B2P.EnumCultureID.zh_TW;
            if (string.IsNullOrWhiteSpace(System.Threading.Thread.CurrentThread.CurrentCulture.Name) == false)
            {
                _cultureID = LionTech.Entity.B2P.Utility.GetCultureID(System.Threading.Thread.CurrentThread.CurrentCulture.Name.Replace("-", "_"));
            }
        }

        protected virtual void OnActionExecutingSetAuthState()
        {
            
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _authState = new AuthState(
                EnumSystemID.B2PAP, 
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, 
                filterContext.ActionDescriptor.ActionName, 
                this.Session, Request.Cookies, Response.Cookies);
            _authState.UnAuthorizedActionResult = RedirectToAction("UnAuthorizated", "Home");
            this.OnActionExecutingSetAuthState();

            if (filterContext.ActionParameters != null && filterContext.ActionParameters.Count > 0)
            {
                object baseModel;
                if (filterContext.ActionParameters.TryGetValue("model", out baseModel))
                {
                    if (((_BaseModel)baseModel).SystemControllerAction ==
                        string.Concat(new string[] { 
                        EnumSystemID.B2PAP.ToString(), 
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
            this.ViewData.Set<string>(EnumViewDataItem.UserID, AuthState.SessionData.UserID);
            this.ViewData.Set<string>(EnumViewDataItem.UserNM, AuthState.SessionData.UserNM);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            
            if (_authState.IsAuthorized == false)
            {
                filterContext.Result = _authState.UnAuthorizedActionResult;
            }
            else if (_authState.IsLogined == true)
            {
                this.ViewData.Set<string>(EnumViewDataItem.UserMenu, AuthState.SessionData.UserMenu);

                SetModelSystemControllerAction(filterContext);
                SetModelPageSize(filterContext);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), filterContext.Exception);

            filterContext.ExceptionHandled = false;
            //if (filterContext.HttpContext.IsCustomErrorEnabled == true) { filterContext.ExceptionHandled = false; }
        }

        protected void SetUserMenu(EnumUserMenuID focusedUserMenuID)
        {
            string userMenu = PublicFun.GetUserMenu(
                AuthState.SessionData.UserMenuXSLString, 
                AuthState.SessionData.FilePathUserMenu, 
                AuthState.SessionData.UserID, 
                focusedUserMenuID);
            _authState.SessionData.UserMenu = userMenu;
        }

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

        protected void SetModelSystemControllerAction(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(ViewResult))
            {
                ViewResultBase ViewResultBase = (ViewResultBase)filterContext.Result;
                if (ViewResultBase.Model != null)
                {
                    _BaseModel BaseModel = (_BaseModel)ViewResultBase.Model;
                    BaseModel.SystemControllerAction = string.Concat(new string[] { 
                        EnumSystemID.B2PAP.ToString(), 
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

            ((_BaseController)filterContext.Controller).AuthState.IsAuthorized = false;
            if (string.IsNullOrWhiteSpace(_actionName) == false)
            {
                ((_BaseController)filterContext.Controller).AuthState.SetActionName(_actionName);
            }

            bool allowExternalIPAddress = bool.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.AllowExternalClientIPAddress.ToString()]);
            bool isInternalIPAddress = ((_BaseController)filterContext.Controller).ValidateIPAddressIsInternal();

            if (allowExternalIPAddress || isInternalIPAddress)
            {
                HttpSessionStateBase httpSessionStateBase = filterContext.RequestContext.HttpContext.Session;
                if (httpSessionStateBase != null && httpSessionStateBase.Count > 0)
                {
                    ((_BaseController)filterContext.Controller).AuthState.IsAuthorized = new _BaseModel().GetValidateUserSystemRoleFun(
                        ((_BaseController)filterContext.Controller).AuthState.SessionData.UserID,
                        EnumSystemID.B2PAP,
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                        string.IsNullOrWhiteSpace(_actionName) ? filterContext.ActionDescriptor.ActionName : _actionName,
                        ((_BaseController)filterContext.Controller).AuthState.SessionData.SessionID,
                        (isInternalIPAddress) ? EnumYN.N : EnumYN.Y
                    );
                }
            }
        }
    }
}
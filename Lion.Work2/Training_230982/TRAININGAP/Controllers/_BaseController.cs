using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRAININGAP.Models;

namespace TRAININGAP
{
    public enum EnumAppSettingKey
    {
        EDIServiceDistributor, EDIServiceDistributorURL, EDIServiceDistributorTimeOut,
        RootPath, FilePath,
        AllowExternalClientIPAddress, ValidationClientIPAddressString, ValidationServerIPAddressString,
        ASPPUBPub003URL
    }

    public enum EnumRootPathFile
    {
        [Description(@"Log\{0}.err")]
        Exception,
        [Description(@"Distributor\{0}.log")]
        Distributor,
    }

    public enum EnumFilePathFolder
    {
    }

    public enum EnumAppDataFile
    {
    }
}

namespace TRAININGAP.Controllers
{
    public class _BaseController : Controller
    {
        LionTech.Entity.TRAINING.EnumCultureID _cultureID;
        protected LionTech.Entity.TRAINING.EnumCultureID CultureID { get { return _cultureID; } }

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
                        EnumSystemID.TRAININGAP.ToString(), userID, sysID, eventGroupID, eventID, userID, eventPara
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
                                          CultureID == LionTech.Entity.TRAINING.EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(CultureID),
                                          ".resx"
                                      }));
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
            _cultureID = LionTech.Entity.TRAINING.EnumCultureID.zh_TW;
            if (string.IsNullOrWhiteSpace(System.Threading.Thread.CurrentThread.CurrentCulture.Name) == false)
            {
                _cultureID = LionTech.Entity.TRAINING.Utility.GetCultureID(System.Threading.Thread.CurrentThread.CurrentCulture.Name.Replace("-", "_"));
            }
        }

        protected virtual void OnActionExecutingSetAuthState()
        {

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _authState = new AuthState(
                EnumSystemID.TRAININGAP,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName,
                this.Session, Request.Cookies, Response.Cookies);
            _authState.UnAuthorizedActionResult = RedirectToAction("UnAuthorizated", "Home");
            _authState.RejectiveActionResult = RedirectToAction("Rejective", "Home");
            this.OnActionExecutingSetAuthState();

            if (filterContext.ActionParameters != null && filterContext.ActionParameters.Count > 0)
            {
                object baseModel;
                if (filterContext.ActionParameters.TryGetValue("model", out baseModel))
                {
                    if (((_BaseModel)baseModel).SystemControllerAction ==
                        string.Concat(new string[] {
                        EnumSystemID.TRAININGAP.ToString(),
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

            if (AuthState.IsAuthorized && AuthState.IsLogined)
            {
                this.SetSysFunToolData();
                this.OnSetERPUserMessage();
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

        protected void SetModelSystemControllerAction(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(ViewResult))
            {
                ViewResultBase ViewResultBase = (ViewResultBase)filterContext.Result;
                if (ViewResultBase.Model != null)
                {
                    _BaseModel BaseModel = (_BaseModel)ViewResultBase.Model;
                    BaseModel.SystemControllerAction = string.Concat(new string[] {
                        EnumSystemID.TRAININGAP.ToString(),
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
                    EnumSystemID systemID = EnumSystemID.TRAININGAP;
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
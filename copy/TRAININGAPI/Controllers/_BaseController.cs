using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TRAININGAPI.Models;

namespace TRAININGAPI
{
    public enum EnumAppSettingKey
    {
        EDIServiceDistributor, EDIServiceDistributorURL, EDIServiceDistributorTimeOut,
        ERPAPHomeQRCodeAuthorization, 

        RootPath, FilePath,
        AllowExternalClientIPAddress, ValidationClientIPAddressString, ValidationServerIPAddressString,
        FilePathERPAPUserMenu, FilePathB2PAPUserMenu, 


        APITimeOut,
        APIERPAPTokenServiceValidationValidateEventURL, 

        APIERPAPAppTokenServiceGeneratorEventURL,
        SysSDMail,
        SmtpClientIPAddress,
        LineChannel,
        LineTo,
        LineID
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

    public enum EnumRequestContent
    {
        T, K, C, U,
        ClientSysID
    }

    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), actionExecutedContext.Exception);

            //filterContext.ExceptionHandled = false;
            //if (filterContext.HttpContext.IsCustomErrorEnabled == true) { filterContext.ExceptionHandled = false; }
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
    }
}

namespace TRAININGAPI.Controllers
{
    public class _BaseController : ApiController
    {
        protected HttpContextBase MSHttpContext => ((HttpContextBase)Request.Properties["MS_HttpContext"]);

        public string HostUrl()
        {
            return MSHttpContext.Request.Url?.ToString();
        }

        public string HostIPAddress()
        {
            return Common.GetHostIPAddress(MSHttpContext.Request.ServerVariables);
        }

        public string ClientIPAddress()
        {
            return Common.GetClientIPAddress(MSHttpContext.Request.ServerVariables);
        }

        public bool ValidateIPAddressIsInternalServer()
        {
            string clientIPAddress = Common.GetClientIPAddress(MSHttpContext.Request.ServerVariables);
            string validationServerIPAddressString = ConfigurationManager.AppSettings[EnumAppSettingKey.ValidationServerIPAddressString.ToString()];

            return Common.ValidateIPAddress(clientIPAddress, validationServerIPAddressString);
        }

        public bool ValidateIPAddressIsInternalClient()
        {
            string clientIPAddress = Common.GetClientIPAddress(MSHttpContext.Request.ServerVariables);
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
                        EnumSystemID.TRAININGAP.ToString(), sysID, eventGroupID, eventID, userID, eventPara
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

        AuthState _authState;
        public AuthState AuthState
        {
            get { return _authState; }
        }

        public _BaseController()
        {

        }

        protected virtual void OnActionExecutingSetAuthState()
        {

        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _authState = new AuthState(
                EnumSystemID.TRAININGAP,
                controllerContext.ControllerDescriptor.ControllerName,
                string.Empty,
                null, null, null);
            this.OnActionExecutingSetAuthState();
        }

        protected IHttpActionResult Text(string text)
        {
            return new TextResult(text, Request);
        }
    }

    public class TextResult : IHttpActionResult
    {
        private readonly string _value;
        private readonly HttpRequestMessage _request;

        public TextResult(string value, HttpRequestMessage request)
        {
            _value = value;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(_value),
                RequestMessage = _request
            };
            return Task.FromResult(response);
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

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ((_BaseController)filterContext.ControllerContext.Controller).AuthState.IsAuthorized = false;
            if (string.IsNullOrWhiteSpace(_actionName) == false)
            {
                ((_BaseController)filterContext.ControllerContext.Controller).AuthState.SetActionName(_actionName);
            }

            bool isInternalIPAddress = ((_BaseController)filterContext.ControllerContext.Controller).ValidateIPAddressIsInternalServer();

            if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request[EnumRequestContent.T.ToString()]) &&
                !string.IsNullOrWhiteSpace(HttpContext.Current.Request[EnumRequestContent.K.ToString()]) &&
                !string.IsNullOrWhiteSpace(HttpContext.Current.Request[EnumRequestContent.C.ToString()]) &&
                !string.IsNullOrWhiteSpace(HttpContext.Current.Request[EnumRequestContent.U.ToString()]))
            {
                bool hasRole = new _BaseModel().GetValidateSystemAPIRole(
                    EnumSystemID.ERPAP,
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    string.IsNullOrWhiteSpace(_actionName) ? filterContext.ActionDescriptor.ActionName : _actionName,
                    (isInternalIPAddress) ? EnumYN.N : EnumYN.Y,
                    HttpContext.Current.Request[EnumRequestContent.U.ToString()]
                );

                if (hasRole)
                {
                    int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.APITimeOut.ToString()]);
                    string apiURL = string.Format(
                        ConfigurationManager.AppSettings[EnumAppSettingKey.APIERPAPTokenServiceValidationValidateEventURL.ToString()],
                        new string[] {
                            Common.GetEnumDesc(EnumAPISystemID.TKNAP), EnumSystemID.TRAININGAP.ToString(),
                            HttpContext.Current.Request[EnumRequestContent.T.ToString()],
                            HttpContext.Current.Request[EnumRequestContent.K.ToString()],
                            HttpContext.Current.Request[EnumRequestContent.C.ToString()],
                            HttpContext.Current.Request[EnumRequestContent.U.ToString()]
                        }
                    );

                    string responseString = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut);
                    if (string.IsNullOrWhiteSpace(responseString) == false && responseString == bool.TrueString)
                    {
                        ((_BaseController)filterContext.ControllerContext.Controller).AuthState.IsAuthorized = true;
                    }
                }
            }
            else
            {
                if (isInternalIPAddress)
                {
                    ((_BaseController)filterContext.ControllerContext.Controller).AuthState.IsAuthorized = new _BaseModel().GetValidateSystemAPIFun(
                        HttpContext.Current.Request[EnumRequestContent.ClientSysID.ToString()],
                        EnumSystemID.TRAININGAP,
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                        string.IsNullOrWhiteSpace(_actionName) ? filterContext.ActionDescriptor.ActionName : _actionName,
                        ((_BaseController)filterContext.ControllerContext.Controller).ClientIPAddress(),
                        (isInternalIPAddress) ? EnumYN.N : EnumYN.Y
                    );
                }
            }
        }
    }
}
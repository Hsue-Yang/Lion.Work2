using ERPAPI.Models;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;
using StackExchange.Redis;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ERPAPI
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
        [Description(@"Log\{0}.log")]
        UserMessageSelectEvent
    }

    public enum EnumFilePathFolder
    {
        [Description(@"EDIService\EventPara\{0}\{1}.para")]
        EDIServiceEventPara,
        [Description(@"EDIService\FlowPara\{0}\{1}.para")]
        EDIServiceFlowPara
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

    public class PlainTextFormatter : MediaTypeFormatter
    {
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override bool CanReadType(System.Type type)
        {
            return false;
        }

        public override bool CanWriteType(System.Type type)
        {
            return type == typeof(string);
        }

        public override async Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            if (value != null && content.Headers != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain") { CharSet = Encoding.UTF8.WebName };
                byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
                await writeStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}

namespace ERPAPI.Controllers
{
    public class _BaseController : ApiController
    {
        internal HttpContextBase MSHttpContext => ((HttpContextBase)Request.Properties["MS_HttpContext"]);

        public string GetApiNo()
        {
            return Common.GetApiNo();
        }
        
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
                        EnumSystemID.ERPAP.ToString(), sysID, eventGroupID, eventID, userID, eventPara
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
        public virtual AuthState AuthState
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
                EnumSystemID.ERPAP,
                controllerContext.ControllerDescriptor.ControllerName,
                string.Empty,
                null, null, null);
            this.OnActionExecutingSetAuthState();
        }

        protected IHttpActionResult Text(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Ok();
            }
            return Content(HttpStatusCode.OK, text, new PlainTextFormatter());
        }
    }

    public class AuthorizationActionFilter : ActionFilterAttribute
    {
        private readonly string _actionName;

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
                            Common.GetEnumDesc(EnumAPISystemID.TKNAP), EnumSystemID.ERPAP.ToString(),
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
                        EnumSystemID.ERPAP,
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
// 新增日期：2017-01-03
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using ERPAPI.Models.LionGroupAppService;
using LionTech.APIService.LionGroupApp;
using LionTech.Utility;

namespace ERPAPI.Controllers.LionGroupAppService
{
    public partial class LionGroupAppServiceController
    {
        [HttpPost]
        [Route("LionGroupAppService/v1/AppRegister")]
        public HttpResponseMessage AppRegisterForApp(AppRegisterModel model)
        {
            HttpResponseMessage actionResult = Request.CreateResponse();

            try
            {
                bool result = true;

                model.ExecLogAPIClientBegin(HttpUtility.UrlDecode(HostUrl()), Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8), ClientIPAddress());

                model.LionGroupAppRegister = LionGroupAppRegister.Create(MSHttpContext.Request);

                string updUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (string.IsNullOrWhiteSpace(model.LionGroupAppRegister.UserID) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.AppID) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.UUID) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.DeviceToken) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.DeviceTokenType) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.OS) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.MobileType))
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    result = false;
                }

                if (result && model.GetValidUserAccountResult() == false)
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    result = false;
                }

                if (result && model.EditAppRegister(updUser) == false)
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    result = false;
                }

                if (result)
                {
                    model.CreateInstanceIDTopics();
                    actionResult = Request.CreateResponse();
                }

                model.ExecLogAPIClientEnd(string.Empty);
                return actionResult;
            }
            catch (Exception ex)
            {
                OnException(ex);
                actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            model.ExecLogAPIClientEnd(string.Empty);
            return actionResult;
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public HttpResponseMessage AppRegister([FromUri]AppRegisterModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;

            try
            {
                bool result = true;

                model.LionGroupAppRegister = LionGroupAppRegister.Create(MSHttpContext.Request);

                string updUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (string.IsNullOrWhiteSpace(model.LionGroupAppRegister.UserID) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.AppID) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.UUID) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.DeviceToken) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.DeviceTokenType) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.OS) ||
                    string.IsNullOrWhiteSpace(model.LionGroupAppRegister.MobileType))
                {
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = false;
                }

                if (result && model.EditAppRegister(updUser) == false)
                {
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = false;
                }

                if (result)
                {
                    model.CreateInstanceIDTopics();
                    httpStatusCode = HttpStatusCode.OK;
                    return Request.CreateResponse(httpStatusCode);
                }
            }
            catch (Exception ex)
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                OnException(ex);
            }
           
            return Request.CreateResponse(httpStatusCode);
        }
    }
}
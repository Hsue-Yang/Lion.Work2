// 新增日期：2017-01-04
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
        [Route("LionGroupAppService/v1/OpenPush")]
        [TokenAuthorizationActionFilter]
        public HttpResponseMessage OpenPushForApp(OpenPushModel model)
        {
            HttpResponseMessage actionResult = Request.CreateResponse();

            try
            {
                bool result = true;

                model.ExecLogAPIClientBegin(HttpUtility.UrlDecode(HostUrl()), Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8), ClientIPAddress());

                model.LionGroupAppOpenPush = LionGroupAppOpenPush.Create(MSHttpContext.Request);

                string updUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (model.GetValidAppUserMobileResult(model.LionGroupAppOpenPush.UserID, model.LionGroupAppOpenPush.UUID) == false)
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    result = false;
                }

                if (model.EditOpenPush(updUser) == false)
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    result = false;
                }

                if (result && model.GetERPRecordAppUserFunResult(updUser, ClientIPAddress(), model.LionGroupAppOpenPush.UserID, model.LionGroupAppOpenPush.UUID, model.LionGroupAppOpenPush.FunID) == false)
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    result = false;
                }

                if (result)
                {
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
        public HttpResponseMessage OpenPush([FromUri]OpenPushModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            
            try
            {
                bool result = true;

                model.LionGroupAppOpenPush = LionGroupAppOpenPush.Create(MSHttpContext.Request);

                string updUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (model.ValidLionGroupAppFunType(model.LionGroupAppOpenPush.FunID) == false)
                {
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = false;
                }

                if (result && model.GetValidAppUserMobileResult(model.LionGroupAppOpenPush.UserID, model.LionGroupAppOpenPush.UUID) == false)
                {
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = false;
                }

                if (result && model.EditOpenPush(updUser) == false)
                {
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = false;
                }

                if (result && model.GetERPRecordAppUserFunResult(updUser, ClientIPAddress(), model.LionGroupAppOpenPush.UserID, model.LionGroupAppOpenPush.UUID, model.LionGroupAppOpenPush.FunID) == false)
                {
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = false;
                }

                if (result)
                {
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
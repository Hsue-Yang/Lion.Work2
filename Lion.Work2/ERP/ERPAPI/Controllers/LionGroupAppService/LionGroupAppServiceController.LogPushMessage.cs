// 新增日期：2017-02-13
// 新增人員：廖先駿
// 新增內容：查詢推播歷史紀錄
// ---------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using ERPAPI.Models.LionGroupAppService;
using LionTech.Utility;

namespace ERPAPI.Controllers.LionGroupAppService
{
    public partial class LionGroupAppServiceController
    {
        //[HttpGet]
        //[Route("LionGroupAppService/v1/LogPushMessage")]
        //[TokenAuthorizationActionFilter]
        //public HttpResponseMessage LogPushMessageForApp([FromUri]LogPushMessageModel model)
        //{
        //    HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.BadRequest);
            
        //    try
        //    {
        //        model.ExecLogAPIClientBegin(HttpUtility.UrlDecode(HostUrl()), Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8), ClientIPAddress());
                
        //        if (string.IsNullOrWhiteSpace(model.UserID) == false && 
        //            string.IsNullOrWhiteSpace(model.StartDateTime) == false &&
        //            string.IsNullOrWhiteSpace(model.UUID) == false &&
        //            model.GetLogPushMessageList())
        //        {
        //            actionResult = Request.CreateResponse(HttpStatusCode.OK);
        //            actionResult.Content
        //                = new StringContent(Common.GetJsonSerializeObject(
        //                    (from row in model.LogPushMessageList
        //                     select new
        //                     {
        //                         MessageID = (string)row.MessageID,
        //                         Title = (string)row.Title,
        //                         Body = (string)row.Body,
        //                         PushDateTime = row.UpdDT.GetFormattedValue(Common.EnumDateTimeFormatted.FullDateTimeNumber),
        //                         Data = new
        //                         {
        //                             SourceType = row.Data.SourceType.GetValue(),
        //                             SourceID = row.Data.SourceID.GetValue()
        //                         }
        //                     })), Encoding.UTF8);

        //            model.ExecLogAPIClientEnd(string.Empty);
        //            return actionResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OnException(ex);
        //        actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //    }
            
        //    model.ExecLogAPIClientEnd(string.Empty);
        //    return actionResult;
        //}

        //[HttpGet]
        //[AuthorizationActionFilter]
        //public HttpResponseMessage LogPushMessage([FromUri]LogPushMessageModel model)
        //{
        //    if (AuthState.IsAuthorized == false)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //    }

        //    HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;

        //    try
        //    {
        //        bool result = true;

        //        if (string.IsNullOrWhiteSpace(model.UserID) ||
        //            string.IsNullOrWhiteSpace(model.StartDateTime) ||
        //            string.IsNullOrWhiteSpace(model.UUID))
        //        {
        //            httpStatusCode = HttpStatusCode.BadRequest;
        //            result = false;
        //        }

        //        if (result && model.GetLogPushMessageList())
        //        {
        //            string content =
        //                Common.GetJsonSerializeObject(
        //                    (from row in model.LogPushMessageList
        //                     select new
        //                     {
        //                         MessageID = (string)row.MessageID,
        //                         Title = (string)row.Title,
        //                         Body = (string)row.Body,
        //                         PushDateTime = row.UpdDT.GetFormattedValue(Common.EnumDateTimeFormatted.FullDateTimeNumber),
        //                         Data = new
        //                         {
        //                             SourceType = row.Data.SourceType.GetValue(),
        //                             SourceID = row.Data.SourceID.GetValue()
        //                         }
        //                     }));

        //            httpStatusCode = HttpStatusCode.OK;
        //            HttpResponseMessage actionResult = Request.CreateResponse(httpStatusCode);
        //            actionResult.Content = new StringContent(content, Encoding.UTF8);
                    
        //            return actionResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        httpStatusCode = HttpStatusCode.InternalServerError;
        //        OnException(ex);
        //    }
            
        //    return Request.CreateResponse(httpStatusCode);
        //}
    }
}
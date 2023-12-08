// 新增日期：2016-12-28
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
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
        [HttpPost]
        [AuthorizationActionFilter]
        public HttpResponseMessage CancelPushMessage([FromUri]CancelPushMessageModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.BadRequest);
            string apiNo = GetApiNo();

            try
            {
                model.UserIDList = null;
                bool result = true;
                string upUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);
                string json = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(json) == false)
                {
                    var targetPara = new { UserIDList = (List<string>)null };
                    var para = Common.GetJsonDeserializeAnonymousType(json, targetPara);
                    model.UserIDList = para.UserIDList;
                }

                if (model.GetAppUserPushList())
                {
                    if (model.CancelAppPushMessage(upUser, model.ClientSysID, apiNo, ClientIPAddress()) == false)
                    {
                        actionResult.Content = new StringContent("取消推播排程失敗", Encoding.UTF8);
                        result = false;
                    }
                }

                if (result)
                {
                    actionResult = Request.CreateResponse(HttpStatusCode.OK);
                    actionResult.Content = new StringContent(bool.TrueString, Encoding.UTF8);
                }
                
                return actionResult;
            }
            catch (Exception ex)
            {
                OnException(ex);
                actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            
            return actionResult;
        }
    }
}
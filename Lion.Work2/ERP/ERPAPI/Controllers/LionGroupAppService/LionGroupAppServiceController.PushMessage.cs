// 新增日期：2016-12-23
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
        [AuthorizationActionFilter]
        public HttpResponseMessage PushMessage([FromUri]PushMessageModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.BadRequest);
            
            string apiNo = GetApiNo();

            try
            {
                model.LionGroupAppMessage = LionGroupAppMessage.Create(MSHttpContext.Request.InputStream);

                model.LionGroupAppMessage.Data = model.LionGroupAppMessage.Data ?? new LionGroupAppMessageData();

                bool result = true;
                
                if (model.GetPushMessageUserInfoList() == false)
                {
                    actionResult.Content = new StringContent("取得推播訊息發送對象資訊失敗。", Encoding.UTF8);
                    result = false;
                }

                if (result)
                {
                    string messageID = Guid.NewGuid().ToString();
                    string upUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                    if (string.IsNullOrWhiteSpace(model.LionGroupAppMessage.PushDateTime) == false)
                    {
                        if (model.AddUserPushSchedule(upUser, messageID, model.ClientSysID, apiNo, ClientIPAddress()) == false)
                        {
                            actionResult.Content = new StringContent("推播排程失敗。", Encoding.UTF8);
                            result = false;
                        }
                    }
                    else
                    {
                        if (model.LionGroupAppPushMessage(upUser) == false)
                        {
                            actionResult.Content = new StringContent("雄獅集團App服務，推播失敗。", Encoding.UTF8);
                            result = false;
                        }

                        if (result && model.GetERPRecordAppUserPushResult(upUser, model.ClientSysID, apiNo, ClientIPAddress(), messageID) == false)
                        {
                            actionResult.Content = new StringContent("雄獅集團App服務，紀錄推播訊息失敗。", Encoding.UTF8);
                            result = false;
                        }
                    }

                    if (result)
                    {
                        actionResult = Request.CreateResponse(HttpStatusCode.OK);
                        actionResult.Content = new StringContent(messageID, Encoding.UTF8);
                    }
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
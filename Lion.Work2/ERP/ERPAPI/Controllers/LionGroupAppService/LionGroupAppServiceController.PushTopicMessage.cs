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
        public HttpResponseMessage PushTopicMessage([FromUri] PushTopicMessageModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.BadRequest);
            string apiNo = GetApiNo();
            bool isSuccess = false;

            try
            {
                model.TopicMessage = LionGroupAppTopicMessage.Create(MSHttpContext.Request.InputStream);
                string messageID = Guid.NewGuid().ToString();
                string upUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (string.IsNullOrWhiteSpace(model.TopicMessage.PushDateTime) == false)
                {
                    isSuccess = model.AddPushTopicSchedule(upUser, messageID, apiNo, ClientIPAddress());
                    if (isSuccess == false)
                    {
                        actionResult.Content = new StringContent("推播排程失敗。", Encoding.UTF8);
                    }
                }
                else
                {
                    isSuccess = model.AddPushTopicMessage();
                }

                if (isSuccess)
                {
                    actionResult = Request.CreateResponse(HttpStatusCode.OK);
                    actionResult.Content = new StringContent(messageID, Encoding.UTF8);
                }
                else
                {
                    actionResult.Content = new StringContent("主題推播失敗。", Encoding.UTF8);
                }

                model.RecordingTopicPushToLogAppTopicPush(upUser, messageID, apiNo, ClientIPAddress(), isSuccess);
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
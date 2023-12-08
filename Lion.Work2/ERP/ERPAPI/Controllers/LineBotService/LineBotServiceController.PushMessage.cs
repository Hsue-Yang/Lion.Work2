using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ERPAPI.Models.LineBotService;
using LionTech.APIService.LineBot;

namespace ERPAPI.Controllers.LineBotService
{
    public partial class LineBotServiceController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public HttpResponseMessage PushMessage([FromUri]PushMessageModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpResponseMessage actionResult = Request.CreateResponse();
            string response = null;

            try
            {
                LineService lineService = new LineService(MSHttpContext.Request);

                if (model.GetLineClinet(model.SysID, model.LineID) == false)
                {
                    actionResult.Content = new StringContent("請確認LineID是否正確。", Encoding.UTF8);
                }
                else if (model.GetLineReceiverList() == false)
                {
                    actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
                else if (model.LineReceiverList.Any() == false)
                {
                    actionResult.Content = new StringContent("請確認發送對象是否正確。", Encoding.UTF8);
                }
                else
                {
                    try
                    {
                        response = lineService.PushMessage(model.LineClinet.ChannelAccessToken.GetValue(), model.LineReceiverList.Select(s => s.ReceiverID.GetValue()));

                        actionResult.Content = new StringContent(response, Encoding.UTF8);
                    }
                    catch (WebException webException)
                    {
                        HttpWebResponse webResponse = webException.Response as HttpWebResponse;

                        if (webResponse != null)
                        {
                            actionResult.Content = new StringContent(string.Format("Line message:{0}", webResponse.StatusDescription), Encoding.UTF8);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
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

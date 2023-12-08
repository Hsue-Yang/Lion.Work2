// 新增日期：2016-12-15
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.LineBotService;
using LionTech.APIService.LineBot;

namespace ERPAPI.Controllers.LineBotService
{
    public partial class LineBotServiceController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public HttpResponseMessage GetProfile([FromUri]GetProfileModel model)
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
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<GetProfileModel.APIParaData>(model.APIPara);

                if (model.GetLineClinet(model.APIData.SysID, model.APIData.LineID) == false)
                {
                    actionResult.Content = new StringContent("請確認LineID是否正確。", Encoding.UTF8);
                }
                else
                {
                    try
                    {
                        response = lineService.GetProfile(model.LineClinet.ChannelAccessToken.GetValue(), model.APIData.ReceiverID);
                        actionResult.Content = new StringContent(response, Encoding.UTF8);
                    }
                    catch (WebException webException)
                    {
                        HttpWebResponse webResponse = webException.Response as HttpWebResponse;

                        if (webResponse != null)
                        {
                            actionResult.Content = new StringContent(string.Format("Line message:{0}", webResponse.StatusDescription), Encoding.Unicode);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    
                    return actionResult;
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
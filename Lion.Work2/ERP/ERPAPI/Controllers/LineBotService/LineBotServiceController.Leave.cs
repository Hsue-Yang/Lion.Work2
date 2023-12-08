// 新增日期：2016-12-16
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
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LineBotService;
using LionTech.Utility;

namespace ERPAPI.Controllers.LineBotService
{
    public partial class LineBotServiceController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public HttpResponseMessage Leave([FromUri]LeaveModel model)
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
                model.APIData = jsonConvert.Deserialize<LeaveModel.APIParaData>(model.APIPara);

                if (model.GetLineClinet(model.APIData.SysID, model.APIData.LineID) == false)
                {
                    actionResult.Content = new StringContent("請確認LineID是否正確。", Encoding.UTF8);
                }
                else if (model.GetLineReceiverInfoList() == false)
                {
                    actionResult.Content = new StringContent("請確認使用者代碼是否正確。", Encoding.UTF8);
                }
                else
                {
                    try
                    {
                        if (model.LineReceiverInfo.SourceType.GetValue() == Entity_BaseAP.EnumLineSourceType.GROUP.ToString())
                        {
                            response = lineService.LeaveGroup(model.LineClinet.ChannelAccessToken.GetValue(), model.LineReceiverInfo.ReceiverID.GetValue());
                            actionResult.Content = new StringContent(response, Encoding.UTF8);
                        }
                        else if (model.LineReceiverInfo.SourceType.GetValue() == Entity_BaseAP.EnumLineSourceType.ROOM.ToString())
                        {
                            response = lineService.LeaveRoom(model.LineClinet.ChannelAccessToken.GetValue(), model.LineReceiverInfo.ReceiverID.GetValue());
                            actionResult.Content = new StringContent(response, Encoding.UTF8);
                        }

                        if (response == Common.GetEnumDesc(EntityLeave.EnumLineBotLeaveResult.Success))
                        {
                            if (model.UpdateLineBotReceiverDetail() == false)
                            {
                                actionResult.Content = new StringContent("更新LineBot好友清單明細失敗。", Encoding.UTF8);
                                response = bool.FalseString;
                            }
                        }
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
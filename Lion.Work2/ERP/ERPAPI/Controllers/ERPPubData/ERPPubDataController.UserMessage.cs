using ERPAPI.Models.ERPPubData;
using LionTech.APIService.Message;
using LionTech.Log;
using LionTech.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ERPAPI.Controllers
{
    public partial class ERPPubDataController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult UserMessageSelectEvent([FromUri]UserMessageModel model)
        {
            if (AuthState.IsAuthorized == false) return Ok();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<UserMessageModel.APIParaData>(model.APIPara);

                if (model.GetUserMessageReturnList())
                {
                    string userMessageReturnJsonString = model.GetUserMessageReturnJsonString();
                    if (userMessageReturnJsonString != null)
                    {
                        return Text(HttpUtility.UrlEncode(userMessageReturnJsonString));
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.UserMessageSelectEvent),
                    string.Join(Environment.NewLine, Environment.NewLine,
                        "APIPara:" + (model.APIPara ?? string.Empty),
                        "ClientSysID:" + (model.ClientSysID ?? string.Empty),
                        "ClientUserID:" + (model.ClientUserID ?? string.Empty),
                        "ClientIPAddress:" + ClientIPAddress()
                        )
                    );
                FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.UserMessageSelectEvent), ex);
                OnException(ex);
            }

            return Ok();
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<HttpResponseMessage> UserMessageAddEvent([FromUri] UserMessageModel model)
        {
            HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.Unauthorized);

            if (AuthState.IsAuthorized == false)
            {
                throw new HttpResponseException(actionResult);
            }
            
            try
            {
                string json = await Request.Content.ReadAsStringAsync();
                Message message = Common.GetJsonDeserializeObject<Message>(json);

                Validate(message);

                if (ModelState.IsValid)
                {
                    if (model.AddUserMessage(message) == false)
                    {
                        ModelState.AddModelError(string.Empty, "新增留言失敗!");
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }

                actionResult = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(GetModelErrorMessages())
                };
            }
            catch (Exception ex)
            {
                OnException(ex);
                actionResult = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            throw new HttpResponseException(actionResult);
        }
    }
}
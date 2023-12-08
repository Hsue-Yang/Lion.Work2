using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ERPAPI.Models.SMSService;
using LionTech.Utility;

namespace ERPAPI.Controllers.SMSService
{
    public partial class SMSServiceController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public HttpResponseMessage Query([FromUri] QueryModel model)
        {
            HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.Unauthorized);

            if (AuthState.IsAuthorized == false)
            {
                throw new HttpResponseException(actionResult);
            }

            string apiNo = GetApiNo();

            try
            {
                var sms = model.GetSMSMessage();

                if (sms == null)
                {
                    ModelState.AddModelError(string.Empty, "查無該筆簡訊，請檢核!");
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(Common.GetJsonSerializeObject(sms))
                    };
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

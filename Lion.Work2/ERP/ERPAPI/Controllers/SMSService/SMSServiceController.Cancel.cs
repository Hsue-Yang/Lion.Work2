using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ERPAPI.Models.SMSService;
using LionTech.APIService.SMS;
using LionTech.Utility;

namespace ERPAPI.Controllers.SMSService
{
    public partial class SMSServiceController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<HttpResponseMessage> Cancel([FromUri] CancelModel model)
        {
            HttpResponseMessage actionResult = Request.CreateResponse(HttpStatusCode.Unauthorized);

            if (AuthState.IsAuthorized == false)
            {
                throw new HttpResponseException(actionResult);
            }

            string apiNo = GetApiNo();

            try
            {
                string json = await Request.Content.ReadAsStringAsync();
                SMSCancel cancel = Common.GetJsonDeserializeObject<SMSCancel>(json);

                if (model.ValidateSendUser(model.ClientUserID) == false)
                {
                    ModelState.AddModelError(nameof(model.ClientUserID), "操作人員不存在或已離職，請檢核!");
                }

                Validate(cancel);

                if (ModelState.IsValid)
                {
                    var sms = model.GetState(cancel.SMSYear, cancel.SMSSeq ?? 0, cancel.PhoneNumber);

                    if (sms == null)
                    {
                        ModelState.AddModelError(string.Empty, "查無該筆簡訊，請檢核!");
                    }
                    else if (string.IsNullOrWhiteSpace(sms.State.GetValue()) == false)
                    {
                        ModelState.AddModelError(string.Empty, "已發送的簡訊無法取消");
                    }

                    if (ModelState.IsValid)
                    {
                        if (model.Cancel(cancel) == false)
                        {
                            ModelState.AddModelError(string.Empty, "取消簡訊失敗!");
                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.OK);
                        }
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

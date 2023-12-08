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
        public async Task<HttpResponseMessage> Send([FromUri] SendModel model)
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
                SMSMessage message = Common.GetJsonDeserializeObject<SMSMessage>(json);

                Validate(message);

                if (ModelState.IsValid)
                {
                    if ((string.IsNullOrWhiteSpace(message.Country) || message.Country == "886") &&
                        message.PhoneNumber.Length != 10)
                    {
                        ModelState.AddModelError(nameof(message.Country), "手機號碼 必須輸入10碼。");
                    }

                    if (message.BookingDateTime.HasValue &&
                        DateTime.Compare(DateTime.Now.AddMinutes(30), message.BookingDateTime.Value) >= 0)
                    {
                        ModelState.AddModelError(nameof(message.BookingDateTime), "預約發送時間 必須大於現在時間+30分鐘。");
                    }

                    if (model.ValidateSendUser(message.SendUser) == false)
                    {
                        ModelState.AddModelError(nameof(message.SendUser), "發送人有誤，請檢核!");
                    }

                    if (ModelState.IsValid)
                    {
                        if (model.SMSSend(message) == false)
                        {
                            ModelState.AddModelError(string.Empty, "發送失敗!");
                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent(Common.GetJsonSerializeObject(model.SendResult))
                            };
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

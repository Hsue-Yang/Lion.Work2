using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Subscriber;

namespace ERPAPI.Controllers
{
    public partial class SubscriberController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPAPPsppm01EditEvent([FromUri]ERPAPPsppm01Model model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPPsppm01Model.EventParaData>(model.EventPara);

                if (model.EditRawCMUserOrg())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Ok();
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPAPPsppm01DeleteEvent([FromUri]ERPAPPsppm01Model model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPPsppm01Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMUserOrg())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Ok();
        }
    }
}
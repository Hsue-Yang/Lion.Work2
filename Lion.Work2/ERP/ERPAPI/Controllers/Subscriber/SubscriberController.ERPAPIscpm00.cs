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
        public IHttpActionResult ERPAPIscpm00EditEvent([FromUri]ERPAPIscpm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Ok();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPIscpm00Model.EventParaData>(model.EventPara);

                if (model.EditRawCMOrg())
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
        public IHttpActionResult ERPAPIscpm00DeleteEvent([FromUri]ERPAPIscpm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Ok();
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPIscpm00Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMOrg())
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
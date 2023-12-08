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
        public IHttpActionResult ERPAPIscpm20EditEvent([FromUri]ERPAPIscpm20Model model)
        {
            if (AuthState.IsAuthorized == false) return Ok();
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPIscpm20Model.EventParaData>(model.EventPara);

                if (model.EditRawCMCode())
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
        public IHttpActionResult ERPAPIscpm20DeleteEvent([FromUri]ERPAPIscpm20Model model)
        {
            if (AuthState.IsAuthorized == false) return Ok();
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPIscpm20Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMCode())
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
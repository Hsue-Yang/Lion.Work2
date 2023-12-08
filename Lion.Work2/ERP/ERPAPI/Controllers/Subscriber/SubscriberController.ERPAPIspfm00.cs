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
        public IHttpActionResult ERPAPIspfm00EditEvent([FromUri]ERPAPIspfm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Ok();
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPIspfm00Model.EventParaData>(model.EventPara);

                if (model.EditRawCMOrgUnit())
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
        public IHttpActionResult ERPAPIspfm00DeleteEvent([FromUri]ERPAPIspfm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Ok();
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPIspfm00Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMOrgUnit())
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
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
        public IHttpActionResult ERPAPPstbm20EditEvent([FromUri]ERPAPPstbm20Model model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPPstbm20Model.EventParaData>(model.EventPara);

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
        public IHttpActionResult ERPAPPstbm20DeleteEvent([FromUri]ERPAPPstbm20Model model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPPstbm20Model.EventParaData>(model.EventPara);

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
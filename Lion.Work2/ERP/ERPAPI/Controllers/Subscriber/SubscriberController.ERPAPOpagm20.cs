using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Subscriber;
using LionTech.Entity;

namespace ERPAPI.Controllers
{
    public partial class SubscriberController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPAPOpagm20EditEvent([FromUri] ERPAPOpagm20Model model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Ok();
            }

            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPOpagm20Model.EventParaData>(model.EventPara);

                if (string.IsNullOrWhiteSpace(model.EventData.stfn_cname) == false &&
                    model.EditRawCMUser(apiNo, ClientIPAddress(), model.ClientSysID))
                {
                    ExecERPUserAccessLogWrite(model.EventData.stfn_stfn, apiNo, EnumLogWriter.ERPSubscriber, model.ClientSysID,
                        null, null, (model.EventData.stfn_sts.Trim() == "1" ? EnumYN.Y.ToString() : EnumYN.N.ToString()));
                    return Text(apiNo);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Text(string.Empty);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPAPOpagm20DeleteEvent([FromUri] ERPAPOpagm20Model model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            string apiNo = GetApiNo();
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPOpagm20Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMUser())
                {
                    ExecERPUserAccessLogWrite(model.EventData.stfn_stfn, apiNo, EnumLogWriter.ERPSubscriber, model.ClientSysID,
                        null, null, EnumYN.Y.ToString());
                    return Text(apiNo);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Text(string.Empty);
        }
    }
}
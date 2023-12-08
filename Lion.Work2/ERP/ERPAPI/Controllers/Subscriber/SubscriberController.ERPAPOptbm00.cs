// 新增日期：2018-10-25
// 新增人員：方道筌
// 新增內容：
// ---------------------------------------------------

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
        public IHttpActionResult ERPAPOptbm00EditEvent([FromUri]ERPAPOptbm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPOptbm00Model.EventParaData>(model.EventPara);

                if (model.EditRawCMCountry())
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
        public IHttpActionResult ERPAPOptbm00DeleteEvent([FromUri]ERPAPOptbm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPOptbm00Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMCountry())
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
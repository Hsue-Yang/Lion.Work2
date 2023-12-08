using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Subscriber;

namespace ERPAPI.Controllers
{
    public partial class SubscriberController
    {
        /// <summary>
        /// 目前尚未使用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPAPPsppm00EditEvent([FromUri]ERPAPPsppm00Model model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPPsppm00Model.EventParaData>(model.EventPara);

                if (model.EditRawCMUserDetail())
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

        /// <summary>
        /// 目前尚未使用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPAPPsppm00DeleteEvent([FromUri] ERPAPPsppm00Model model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.EventData = jsonConvert.Deserialize<ERPAPPsppm00Model.EventParaData>(model.EventPara);

                if (model.DeleteRawCMUserDetail())
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
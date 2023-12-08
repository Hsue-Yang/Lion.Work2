using ERPAPI.Models.ERPPubData;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ERPAPI.Controllers
{
    public partial class ERPPubDataController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult UserFunLog([FromUri]UserFunLogModel model)
        {
            if (AuthState.IsAuthorized == false) return Ok();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<UserFunLogModel.APIParaData>(model.APIPara);
                model.RecordUserSystemFunLog();
            }
            catch
            {
                // 不記錄錯誤訊息,純記log info
            }

            return Ok();
        }
    }
}
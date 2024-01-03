using LionTech.Utility;
using System.Text;
using System.Web.Http;
using TRAININGAPI.Models.Leave;

namespace TRAININGAPI.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult GetLeaveDetail([FromUri] LeaveDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            var leaveData = model.GetLeaveData();
            string responseString = Common.GetJsonSerializeObject(leaveData);
            return Text(responseString);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult EditLeaveData([FromUri] LeaveDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);

            LeaveModel.Prapsppm96 leaveModel = Common.GetJsonDeserializeObject<LeaveModel.Prapsppm96>(jsonPara);

            if (model.EditLeaveData(leaveModel))
            {
                return Text(bool.TrueString);
            }
            return Text(bool.FalseString);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult DeleteLeaveData([FromUri] LeaveDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            if (model.DeleteLeaveData())
            {
                return Text(bool.TrueString);
            }
            return Text(bool.FalseString);
        }
    }
}
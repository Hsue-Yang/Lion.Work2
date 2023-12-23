using LionTech.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using TRAININGAPI.Models.Leave;
using static TRAININGAPI.Models.Leave.LeaveModel;

namespace TRAININGAPI.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpGet]
        //[AuthorizationActionFilter]
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
        //[AuthorizationActionFilter]
        public IHttpActionResult EditLeaveData([FromUri] LeaveDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);

            Prapsppm96 leaveModel = Common.GetJsonDeserializeObject<Prapsppm96>(jsonPara);

            if (model.EditLeaveData(leaveModel))
            {
                return Text(bool.TrueString);
            }

            return Text(bool.FalseString);
        }


    }
}
using LionTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TRAININGAPI.Models.Leave;

namespace TRAININGAPI.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpGet]
        //[AuthorizationActionFilter]
        public IHttpActionResult GetLeaveDataList([FromUri] LeaveModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            var LeaveList = model.GetLeaveList();
            string responseString = Common.GetJsonSerializeObject(LeaveList);
            return Text(responseString);
        }
        [HttpGet]
        //[AuthorizationActionFilter]
        public IHttpActionResult GetPpm95List([FromUri] LeaveModel model)
        {
            if (AuthState.IsAuthorized == false) { return Unauthorized(); }

            var ppmList = model.GetPpmList();
            string responseString = Common.GetJsonSerializeObject(ppmList);
            return Text(responseString);
        }
        [HttpGet]
        //[AuthorizationActionFilter]
        public IHttpActionResult GetPpd95List([FromUri] LeaveModel model)
        {
            if (AuthState.IsAuthorized == false) { return Unauthorized(); }

            var ppdList = model.GetPpdList();
            string responseString = Common.GetJsonSerializeObject(ppdList);
            return Text(responseString);
        }
    }
}
using LionTech.Utility;
using System;
using System.Web.Http;
using TRAININGAPI.Models.Leave;

namespace TRAININGAPI.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpGet]
        public IHttpActionResult Leave([FromUri] LeaveIndexModel model)
        {
            try
            {
                var leaveList = model.GetLeaveList();
                string responseString = Common.GetJsonSerializeObject(leaveList);
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return InternalServerError();
        }
    }
}
using LionTech.Utility;
using System;
using System.Text;
using System.Web.Http;
using TRAININGAPI.Models.Leave;
using static TRAININGAPI.Models.Leave.LeaveDetailModel;

namespace TRAININGAPI.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpGet]
        public IHttpActionResult LeaveDetail([FromUri] LeaveDetailModel model)
        {
            try
            {
                var leaveList = model.GetLeaveListDetail();
                string responseString = Common.GetJsonSerializeObject(leaveList);
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return InternalServerError();
        }

        [HttpPost]
        public IHttpActionResult LeaveEdit([FromUri] LeaveDetailModel model)
        {
            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            try
            {
                LeaveEdit leave = Common.GetJsonDeserializeObject<LeaveEdit>(jsonPara);
                if (model.EditLeave(leave))
                {
                    return Text(bool.TrueString);
                }
                return Text(bool.FalseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return InternalServerError();
        }

        [HttpGet]
        public IHttpActionResult LeaveMenu([FromUri] LeaveDetailModel model)
        {
            try
            {
                var leaveList = model.GetLeaveListMenu();
                string responseString = Common.GetJsonSerializeObject(leaveList);
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return InternalServerError();
        }

        [HttpGet]
        public IHttpActionResult LeaveMenuChild([FromUri] LeaveDetailModel model)
        {
            try
            {
                var leaveList = model.GetLeaveListMenuChild();
                string responseString = Common.GetJsonSerializeObject(leaveList);
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return InternalServerError();
        }

        [HttpDelete]
        public IHttpActionResult LeaveDelete([FromUri] LeaveDetailModel model)
        {
            try
            {
                if (model.DeleteLeave())
                {
                    return Text(bool.TrueString);
                }
                return Text(bool.FalseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return InternalServerError();
        }
    }
}
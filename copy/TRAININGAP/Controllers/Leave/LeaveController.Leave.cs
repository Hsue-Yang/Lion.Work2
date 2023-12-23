using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;
using TRAININGAP.Models.Leave;

namespace TRAININGAP.Controllers
{
    public partial class LeaveController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> Leave()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            LeaveIndexModel model = new LeaveIndexModel();
            if (await model.GetLeaveList(AuthState.SessionData.UserID) == false)  
            {
                SetSystemErrorMessage(LeaveIndexResource.SystemMsg_UnGetLeaveList);
            }
            return View(model);  
        }
    }
}
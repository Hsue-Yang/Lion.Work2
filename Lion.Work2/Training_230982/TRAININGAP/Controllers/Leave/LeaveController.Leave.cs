using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRAININGAP.Models.Leave;

namespace TRAININGAP.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> Leave(string userID)
        {
           if (AuthState.IsAuthorized == false) { return AuthState.UnAuthorizedActionResult; }

            LeaveListModel model = new LeaveListModel();

            if (await model.GetLeaveList(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(PracticeLeave.LeaveMsg_GetLeaveDataList_Failure);
            }

            return View(model);
        }
    }
}
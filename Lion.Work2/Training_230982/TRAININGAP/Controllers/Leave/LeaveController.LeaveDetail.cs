using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;
using TRAININGAP.Models.Leave;

namespace TRAININGAP.Controllers.Leave
{
    public partial class LeaveController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> LeaveDetail(LeaveDetailModel model)
        {
            if (AuthState.IsAuthorized == false) { return AuthState.UnAuthorizedActionResult; }
            bool result = true;
            if (IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (await model.EditLeaveData(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(LeaveLeave.LeaveMsg_EditLeaveData_Failure);
                    }
                }

                if (result || model.ExecAction == EnumActionType.Select)
                {
                    return RedirectToAction("Leave");
                }
            }

            if (model.ExecAction == EnumActionType.Delete)
            {
                if (await model.DeleteLeaveData(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(LeaveLeave.LeaveMsg_DeleteLeaveData_Failure);
                }

                return RedirectToAction("Leave");
            }

            if (await model.GetPpm95List(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(LeaveLeave.LeaveMsg_GetPpm95List_Failure);
            }

            if (model.ExecAction == EnumActionType.Query && await model.GetLeaveDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(LeaveLeave.LeaveMsg_GetLeaveDetail_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("LeaveDetail")]
        public async Task<ActionResult> GetPpd95List(string ppm95_id)
        {
            if (AuthState.IsAuthorized == false) { return AuthState.UnAuthorizedActionResult; }

            LeaveDetailModel model = new LeaveDetailModel();

            if (await model.GetPpd95List(AuthState.SessionData.UserID, ppm95_id))
            {
                return Content(model.GetJsonFormSelectItem(model.Ppd95_List, true));
            }

            return Json(null);
        }
    }
}
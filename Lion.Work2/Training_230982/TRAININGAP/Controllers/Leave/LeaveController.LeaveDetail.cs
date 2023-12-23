using LionTech.Web.ERPHelper;
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
                    if (await model.SubmitFormData(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(PracticeLeave.LeaveMsg_GetLeaveDataList_Failure);
                    }

                }
                if (result || model.ExecAction == EnumActionType.Select)
                {
                    return RedirectToAction("Leave");
                }
            }
            if (await model.GetPpm95List(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(PracticeLeave.LeaveMsg_GetLeaveDataList_Failure);
            }
            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetLeaveDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(PracticeLeave.LeaveMsg_GetLeaveDataList_Failure);
                }
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
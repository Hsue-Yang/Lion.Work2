using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;
using TRAININGAP.Models.Leave;

namespace TRAININGAP.Controllers
{
    public partial class LeaveController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> LeaveDetail(LeaveDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;  
            bool result = true;
            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetLeaveDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(LeaveDetailResource.SystemMsg_UnGetLeaveDetail);
                    result = false;
                }
                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditLeaveDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(LeaveDetailResource.EditLeaveDetailResult_Failure);
                        result = false;
                    }
                }
                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    if (await model.DeleteLeaveDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(LeaveDetailResource.DeleteLeaveResult_Failure);
                        result = false;
                    }
                }
                if (result)
                {
                    return RedirectToAction("Leave", "Leave");
                }

            }
            if (result)
            {
                model.FormReset();
            }
            if ((model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Select) && await model.GetLeaveDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(LeaveDetailResource.SystemMsg_UnGetLeaveDetail);
            }
            if (await model.GetLeaveCategoryList(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(LeaveDetailResource.GetLeaveCategoryByIdList);
            }
            if (await model.GetLeaveCategoryChildList(model.ppm95_id,AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(LeaveDetailResource.GetLeaveCategoryChildByIdList);
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("LeaveDetail")]
        public async Task<ActionResult> GetLeaveCategoryChildByIdList(string ppm95_id)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            LeaveDetailModel model = new LeaveDetailModel();
            if (await model.GetLeaveCategoryChildList(ppm95_id, AuthState.SessionData.UserID))
            {
                return Content(model.GetJsonFormSelectItem(model.LeaveChildCategoryByIdList, true));
            }
            return Json(null);
        }
    }
}
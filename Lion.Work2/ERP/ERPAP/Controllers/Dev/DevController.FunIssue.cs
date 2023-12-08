using System.Web.Mvc;
using ERPAP.Models.Dev;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class DevController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult FunIssue(FunIssueModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add &&
                    model.GetAddFunIssueResult(AuthState.SessionData.UserID, base.CultureID) == false)
                {
                    SetSystemErrorMessage(DevFunIssue.AddFunIssueResult_Failure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSystemFun(base.CultureID) == false)
            {
                SetSystemErrorMessage(DevFunIssue.SystemMsg_GetSystemFunInfor);
            }

            if (model.GetFunIssueList(base.CultureID) == false)
            {
                SetSystemErrorMessage(DevFunIssue.SystemMsg_GetFunIssueList);
            }

            return View(model);
        }
    }
}
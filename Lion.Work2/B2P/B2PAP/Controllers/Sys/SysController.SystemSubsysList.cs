using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemSubsysList(SystemSubsysListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetValidationSubsystemExist() == false)
                {
                    SetSystemAlertMessage(SysSystemSubsysList.SystemMsg_AddSystemSubsysListExist);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Add &&
                    model.InsertSystemSubsysList(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemSubsysList.InsertSystemSubsysListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Update &&
                    model.UpdateSystemSubsysList(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemSubsysList.UpdateSystemSubsysListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete &&
                    model.DeleteSystemSubsysList() == false)
                {
                    SetSystemErrorMessage(SysSystemSubsysList.DeleteSystemSubsysListFailure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSystemSubsysList() == false)
            {
                SetSystemErrorMessage(SysSystemSubsysList.SystemMsg_GetSystemSubsysList);
            }

            return View(model);
        }
    }
}
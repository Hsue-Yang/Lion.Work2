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
        public ActionResult SystemRoleGroupCollect(SystemRoleGroupCollectModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (model.GetEditSysSystemRoleGroupCollectResult(AuthState.SessionData.UserID, base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleGroupCollect.EditSysSystemRoleGroupCollectResult_Failure);
                        result = false;
                    }

                    if (result)
                    {
                        SetSystemAlertMessage(SysSystemRoleGroupCollect.SystemMsg_SetSysSystemRoleGroupCollectResultWasSuccess);
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemRoleGroup", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysSystemRoleGroup(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleGroupCollect.SystemMsg_GetSysSystemRoleGroup);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSysSystemRoleGroupCollectList(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleGroupCollect.SystemMsg_GetSysSystemRoleGroupCollectList);
                }
            }

            return View(model);
        }
    }
}
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
        public ActionResult UserSystemDetail(UserSystemDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (model.GetEditUserSystemRoleResult(AuthState.SessionData.UserID, base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysUserSystemDetail.EditUserSystemRoleResult_Failure);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("UserSystem", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetUserRawData() == false)
            {
                SetSystemErrorMessage(SysUserSystemDetail.SystemMsg_GetUserRawData);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetUserSystemRoleList(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserSystemDetail.SystemMsg_GetUserSystemRoleList);
                }
            }

            return View(model);
        }
    }
}
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
        public ActionResult SystemRoleUserList(SystemRoleUserListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (model.ExecAction == EnumActionType.Select)
            {
                if (model.GetSystemRoleUserList(base.PageSize) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleUserList.SystemMsg_GetSystemRoleUserList);
                }
            }

            return View(model);
        }
    }
}
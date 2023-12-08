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
        public ActionResult SystemUserList(SystemUserListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (model.ExecAction == EnumActionType.Select)
            {
                if (model.GetSystemUserList(base.PageSize) == false)
                {
                    SetSystemErrorMessage(SysSystemUserList.SystemMsg_GetSystemUserList);
                }
            }

            return View(model);
        }
    }
}
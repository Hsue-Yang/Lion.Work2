using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRoleUserList(SystemRoleUserListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (model.ExecAction == EnumActionType.Select)
            {
                if (await model.GetSystemRoleUserList(base.PageSize) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleUserList.SystemMsg_GetSystemRoleUserList);
                }
            }

            return View(model);
        }
    }
}
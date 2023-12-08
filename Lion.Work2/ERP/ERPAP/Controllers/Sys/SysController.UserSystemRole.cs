using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserSystemRole(UserSystemRoleModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetUserADSTabList(_BaseAPModel.EnumTabAction.SysUserSystemRole);

            if (model.GetUserMainInfo() == false)
            {
                SetSystemErrorMessage(SysUserSystemRole.SystemMsg_GetUserMainInfo);
            }
            else
            {
                model.UserNM = string.Format("{0} {1}", model.EntityUserMainInfo.UserID.GetValue(), model.EntityUserMainInfo.UserNM.GetValue());
                model.RoleGroupID = model.EntityUserMainInfo.RoleGroupID.GetValue();
            }

            if (await model.GetSysSystemRoleGroupList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserSystemRole.SystemMsg_GetSysSystemRoleGroupList);
            }

            if (model.GetUserSystemRoleList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserSystemRole.SystemMsg_GetUserSystemRoleList);
            }

            return View(model);
        }
    }
}
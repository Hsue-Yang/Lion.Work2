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
        public async Task<ActionResult> UserSystemDetail(UserSystemDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetEditUserSystemRoleResult(AuthState.SessionData.UserID, base.CultureID) == false)
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

            if (await model.GetUserRawData() == false)
            {
                SetSystemErrorMessage(SysUserSystemDetail.SystemMsg_GetUserRawData);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetUserSystemRoleList(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserSystemDetail.SystemMsg_GetUserSystemRoleList);
                }
            }

            return View(model);
        }
    }
}
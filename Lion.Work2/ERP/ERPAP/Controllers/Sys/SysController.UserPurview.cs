using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserPurview(UserPurviewModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysUserRoleFunctionTabList(_BaseAPModel.EnumTabAction.SysUserPurview);

            if (await model.GetUserRawData(model.UserID) == false)
            {
                SetSystemErrorMessage(SysUserPurview.SystemMsg_GetUserMainInfo_Failure);
            }
            else
            {
                model.UserNM = model.EntitySysUserRawData.UserNM;
            }

            if (await model.GetSysUserPurviewList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysUserPurview.SystemMsg_GetSysUserPurviewList_Failure);
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                return RedirectToAction("UserRoleFun", "Sys");
            }

            return View(model);
        }
    }
}
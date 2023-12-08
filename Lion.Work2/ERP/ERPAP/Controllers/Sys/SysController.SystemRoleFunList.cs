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
        public async Task<ActionResult> SystemRoleFunList(SystemRoleFunListModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false || model.IsITManager == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
                return RedirectToAction("SystemRole");
            }

            if (model.ExecAction == EnumActionType.Select ||
                model.ExecAction == EnumActionType.Update)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.EditSystemRoleFunList(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_EditSystemRoleFunList_Failure);
                    }
                }

                if (await model.GetSystemRoleFunList(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSystemRoleFunList);
                }

                if (await model.GetSysSystemFunControllerIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSysSystemFunControllerIDList);
                }

                if (await model.GetSystemFunJsonStr(CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSysControllerIDAndSysFunActionNMList_Failure);
                }
            }

            return View(model);
        }
    }
}

using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemIPList(SystemIPListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.SysNM = HttpUtility.UrlDecode(model.SysNM);

            if (IsPostBack)
            {
                if ((model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Delete) &&
                    model.CheckIsITManager(AuthState.SessionData.UserID, EnumSystemID.ERPAP.ToString()) == false &&
                    model.IsITManager == false)
                {
                    SetSystemErrorMessage(SysResource.SystemMsg_NoEditPermissions);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemIP(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemIPList.EditSystemIPListFailure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete &&
                    await model.DeleteSystemIP(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemIPList.DeleteSystemIPListFailure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemSubByIds(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysResource.SysMsg_GetSystemSubsysList);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, EnumSystemID.ERPAP.ToString()) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsSerpITManager_Failure);
            }

            if (await model.GetSystemIPList(model.SysID, AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemIPList.SystemMsg_GetSystemIPList);
            }

            return View(model);
        }
    }
}
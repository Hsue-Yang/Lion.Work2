using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemSubsysList(SystemSubsysListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.SysNM = HttpUtility.UrlDecode(model.SysNM);

            if (IsPostBack)
            {
                if ((model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Delete) &&
                    model.CheckIsITManager(AuthState.SessionData.UserID, EnumSystemID.ERPAP.ToString()) == false && model.IsITManager == false)
                {
                    SetSystemErrorMessage(SysResource.SystemMsg_NoEditPermissions);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) && await model.EditSystemSub(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemSubsysList.EditSystemSub_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete && await model.DeleteSystemSub(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemSubsysList.DeleteSystemSub_Failure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, EnumSystemID.ERPAP.ToString()) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsSerpITManager_Failure);
            }

            if (await model.GetSystemSubList(model.SysID, AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemSubsysList.SystemMsg_GetSystemSubList);
            }

            return View(model);
        }
    }
}
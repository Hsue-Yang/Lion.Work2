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
        public async Task<ActionResult> SystemServiceList(SystemServiceListModel model)
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

                if (result &&
                    (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    await model.EditSystmeService(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemServiceList.EditSystemServiceListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete &&
                    await model.DeleteSystmeService(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemServiceList.DeleteSystemServiceListFailure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemServiceTypeList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemServiceList.SysMsg_GetBaseSystemServiceList);
            }

            if (await model.GetSystemSubByIds(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysResource.SysMsg_GetSystemSubsysList);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, EnumSystemID.ERPAP.ToString()) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsSerpITManager_Failure);
            }

            if (await model.GetSystmeServices(model.SysID, AuthState.SessionData.UserID, CultureID, PageSize) == false)
            {
                SetSystemErrorMessage(SysSystemServiceList.SystemMsg_GetSystemServiceList);
            }

            return View(model);
        }
    }
}
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPIAuthorize(SystemAPIAuthorizeModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add &&
                    await model.EditSystemAPIAuthorize(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemAPIAuthorize.EditSystemAPIAuthorize_Failure);
                    result = false;
                }
                else if (model.ExecAction == EnumActionType.Delete &&
                         await model.DeleteSystemAPIAuthorize(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemAPIAuthorize.DeleteSystemAPIAuthorize_Failure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemAPIFullName = model.GetSysSystemAPIFullName(model.SysID, AuthState.SessionData.UserID, model.APIGroupID, model.APIFunID, CultureID);
            Task<bool> getSystemAPIAuthorizeList = model.GetSystemAPIAuthorizeList(AuthState.SessionData.UserID, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSysSystemAPIFullName, getSystemAPIAuthorizeList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIAuthorize.SystemMsg_UnGetSystemByIdList);
            }

            if (getSysSystemAPIFullName.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIAuthorize.SystemMsg_UnGetSysSystemAPIFullName);
            }

            if (getSystemAPIAuthorizeList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIAuthorize.SystemMsg_UnGetSystemAPIAuthorizeList);
            }

            return View(model);
        }
    }
}
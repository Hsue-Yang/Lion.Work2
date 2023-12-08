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
        public async Task<ActionResult> SystemEventTarget(SystemEventTargetModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.EditSystemEventTarget(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEventTarget.EditSystemEventTarget_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete && await model.DeleteSystemEventTarget(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEventTarget.DeleteSystemEventTarget_Failure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSystemSubByIds = model.GetSystemSubByIds(model.TargetSysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSysSystemEventFullName = model.GetSysSystemEventFullName(model.SysID, AuthState.SessionData.UserID, model.EventGroupID, model.EventID, CultureID);
            Task<bool> getSystemEventTargetList = model.GetSystemEventTargetList(AuthState.SessionData.UserID, CultureID);

            await Task.WhenAll(getUserSystemByIdList, getSystemSubByIds, getSysSystemEventFullName, getSystemEventTargetList);

            if (getUserSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTarget.SystemMsg_UnGetUserSystemByIdList);
            }

            if (getSystemSubByIds.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTarget.SystemMsg_UnGetSystemSubByIdList);
            }

            if (getSysSystemEventFullName.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTarget.SystemMsg_UnGetSysSystemEventFullName);
            }

            if (getSystemEventTargetList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTarget.SystemMsg_UnGetSystemEventTargetList);
            }

            return View(model);
        }
    }
}
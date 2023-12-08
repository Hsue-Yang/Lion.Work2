// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

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
        [AuthorizationActionFilter("SystemLoginEventSetting")]
        public async Task<ActionResult> SystemLoginEventSettingDetail(SystemLoginEventSettingDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
                return RedirectToAction("SystemLoginEventSetting");
            }

            if(model.IsITManager == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_False);
                return RedirectToAction("SystemLoginEventSetting");
            }

            if (await model.GetSystemSubByIds(model.SysID, AuthState.SessionData.UserID, CultureID, false) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSettingDetail.SystemMsg_GetSysSystemSubsysIDList_Failure);
            }

            if (IsPostBack == false)
            {
                if (await model.GetLoginEventSettingDetail(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemLoginEventSettingDetail.SystemMsg_GetLoginEventSettingDetail_Failure);
                }
            }
            else
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Add ||
                    model.ExecAction == EnumActionType.Update ||
                    model.ExecAction == EnumActionType.Delete)
                {
                    if (TryValidatableObject(model))
                    {
                        if (model.ExecAction == EnumActionType.Add ||
                            model.ExecAction == EnumActionType.Update)
                        {
                            if (await model.EditLoginEventSettingDetail(AuthState.SessionData.UserID, CultureID) == false)
                            {
                                SetSystemErrorMessage(SysSystemLoginEventSettingDetail.SystemMsg_EditLoginEventSettingDetail_Failure);
                                result = false;
                            }
                        }

                        if (model.ExecAction == EnumActionType.Delete)
                        {
                            if (result && await model.DeleteLoginEventSettingDetail(AuthState.SessionData.UserID) == false)
                            {
                                SetSystemErrorMessage(SysSystemLoginEventSettingDetail.SystemMsg_DeleteLoginEventSettingDetail_Failure);
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemLoginEventSetting");
                }
            }
            return View(model);
        }
    }
}
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
        [AuthorizationActionFilter("LineBotAccountSetting")]
        public async Task<ActionResult> LineBotAccountSettingDetail(LineBotAccountSettingDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false || model.IsITManager == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
                return RedirectToAction("LineBotAccountSetting");
            }

            if (IsPostBack == false)
            {
                if (await model.GetLineBotAccountSettingDetail(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysLineBotAccountSettingDetail.SystemMsg_GetLineBotAccountSettingDetail_Failure);
                }
            }
            else
            {
                if (model.ExecAction == EnumActionType.Select)
                {
                    return RedirectToAction("LineBotAccountSetting");
                }

                if (TryValidatableObject(model))
                {
                    bool result = true;

                    if (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update)
                    {
                        if (await model.EditLineBotAccountSettingDetail(AuthState.SessionData.UserID) == false)
                        {
                            SetSystemErrorMessage(SysLineBotAccountSettingDetail.SystemMsg_EditLineBotAccountSettingDetail_Failure);
                            result = false;
                        }

                        if (result && model.RecordLogLineBotAccountSetting(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                        {
                            SetSystemErrorMessage(SysLineBotAccountSettingDetail.SystemMsg_RecordLogLineBotAccountSetting_Failure);
                            result = false;
                        }
                    }

                    if (model.ExecAction == EnumActionType.Delete)
                    {
                        if (await model.GetLineBotAccountSettingDetail(AuthState.SessionData.UserID, CultureID) == false)
                        {
                            SetSystemErrorMessage(SysLineBotAccountSettingDetail.SystemMsg_GetLineBotAccountSettingDetail_Failure);
                            result = false;
                        }

                        if (result && await model.DeleteLineBotAccountSettingDetail(AuthState.SessionData.UserID) == false)
                        {
                            SetSystemErrorMessage(SysLineBotAccountSettingDetail.SystemMsg_DeleteLineBotAccountSettingDetail_Failure);
                            result = false;
                        }

                        if (result && model.RecordLogLineBotAccountSetting(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                        {
                            SetSystemErrorMessage(SysLineBotAccountSettingDetail.SystemMsg_RecordLogLineBotAccountSetting_Failure);
                            result = false;
                        }
                    }

                    if (result)
                    {
                        return RedirectToAction("LineBotAccountSetting");
                    }
                }
            }
            return View(model);
        }
    }
}
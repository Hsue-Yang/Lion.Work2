using System.Web.Mvc;
using B2PAP.Models;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIFlowLogSetting(SystemEDIFlowLogSettingModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDIFlowLogSettingTabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlowLogSetting); //TabText

            model.GetStatusIDList(base.CultureID);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLogSetting.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLogSetting.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLogSetting.SystemMsg_GetSystemEDIFlowList);
            }

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add &&
                    model.GetEditEDIFlowLogSettingResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlowLogSetting.EditSystemEDIFlowLogSettingResult_Failure);
                }
                else
                {
                    SetSystemAlertMessage(SysSystemEDIFlowLogSetting.SystemMsg_InsetrtEDIFlowLogWasSuccess);
                    model.SaveType = true;
                }
            }

            model.FormReset();

            return View(model);
        }
    }
}
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
        public async Task<ActionResult> SystemEDIFlowLogSetting(SystemEDIFlowLogSettingModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDIFlowLogSettingTabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlowLogSetting); //TabText

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLogSetting.SystemMsg_GetSystemEDIFlowList);
            }

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add &&
                    await model.GetEditEDIFlowLogSettingResult(AuthState.SessionData.UserID) == false)
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
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
        public async Task<ActionResult> SystemEDIConDetail(SystemEDIConDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add &&
                    await model.GetSystemEDIConDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemEDIConDetail.SystemMsg_AddSystemEDIConDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    await model.GetEditSystemEDIConDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIConDetail.EditSystemEDIConDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIConDetailResult = await model.GetDeleteSystemEDIConDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemEDIConDetailResult == SystemEDIConDetailModel.EnumDeleteSystemEDIConDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemEDIConDetail.DeleteSystemEDIConDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEDIConDetailResult == SystemEDIConDetailModel.EnumDeleteSystemEDIConDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemEDIConDetail.DeleteSystemEDIConDetailResult_DataExist);
                        result = false;
                    }

                }

                if (result)
                {
                    return RedirectToAction("SystemEDICon", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSystemSysIDList);
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            else
            {
                if (!model.SetHasSysID())
                {
                    SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_SetHasSysID);
                    return RedirectToAction("SystemEDICon", "Sys");
                }
            }

            if (await model.GetSystemEDIFlowIDList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSysSystemEDIFlowList);
            }
            if (model.ExecAction == EnumActionType.Add)
            {
                await model.GetSortOrder(AuthState.SessionData.UserID);

            }
            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetSystemEDIConDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSystemEDIConDetail);
                }
                else
                {
                    model.EDIConID = model.EntitySystemEDIConDetail.EDIConID;
                    model.EDIConZHTW = model.EntitySystemEDIConDetail.EDIConZHTW;
                    model.EDIConZHCN = model.EntitySystemEDIConDetail.EDIConZHCN;
                    model.EDIConENUS = model.EntitySystemEDIConDetail.EDIConENUS;
                    model.EDIConTHTH = model.EntitySystemEDIConDetail.EDIConTHTH;
                    model.EDIConJAJP = model.EntitySystemEDIConDetail.EDIConJAJP;
                    model.EDIConKOKR = model.EntitySystemEDIConDetail.EDIConKOKR;
                    model.ProviderName = model.EntitySystemEDIConDetail.ProviderName;
                    model.ConValue = model.EntitySystemEDIConDetail.ConValue;
                    model.SortOrder = model.EntitySystemEDIConDetail.SortOrder;
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIConDetail")]
        public async Task<ActionResult> SystemEDIFlowList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIFlowModel model = new SystemEDIFlowModel();

            if (await model.GetSystemEDIFlowIDList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySystemEDIFlowList, true));
            }

            return Json(null);
        }
    }
}
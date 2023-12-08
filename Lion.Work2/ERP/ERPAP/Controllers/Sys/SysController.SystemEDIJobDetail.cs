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
        public async Task<ActionResult> SystemEDIJobDetail(SystemEDIJobDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && await model.GetSystemEDIJobDetail(AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemEDIJobDetail.SystemMsg_AddSystemEDIJobDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                   await model.GetEditSystemEDIJobDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIJobDetail.EditSystemEDIJobDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIJobDetailResult = await model.GetDeleteSystemEDIJobDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemEDIJobDetailResult == SystemEDIJobDetailModel.EnumDeleteSystemEDIJobDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemEDIJobDetail.DeleteSystemEDIJobDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEDIJobDetailResult == SystemEDIJobDetailModel.EnumDeleteSystemEDIJobDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemEDIJobDetail.DeleteSystemEDIJobDetailResult_DataExist);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemEDIJob", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemSysIDList(true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            else
            {
                if (!model.SetHasSysID())
                {
                    SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_SetHasSysID);
                    return RedirectToAction("SystemEDIJob", "Sys");
                }
            }

            if (await model.GetSystemEDIFlowIDList(model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetEDIJobTypeList(AuthState.SessionData.UserID, "0006", CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetEDIJobTypeList);
            }

            if (await model.GetSysSystemEDIConList(model.EDIFlowID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysSystemEDIConList);
            }

            if (await model.GetSysSystemEDIJobList(AuthState.SessionData.UserID, model.SysID, model.EDIFlowID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysSystemDepEDIJobList);
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, "0007", CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetEDIFileEncodingList);
            }

            if (model.ExecAction == EnumActionType.Add)
            {
                await model.GetSortOrder(AuthState.SessionData.UserID);

            }
            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetSystemEDIJobDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemEDIJobDetail); //無法取得EDIJob清單明細列表
                }
                else
                {
                    model.EDIJobZHTW = model.EDIJobDetail.EDIJobZHTW;
                    model.EDIJobZHCN = model.EDIJobDetail.EDIJobZHCN;
                    model.EDIJobENUS = model.EDIJobDetail.EDIJobENUS;
                    model.EDIJobTHTH = model.EDIJobDetail.EDIJobTHTH;
                    model.EDIJobJAJP = model.EDIJobDetail.EDIJobJAJP;
                    model.EDIJobKOKR = model.EDIJobDetail.EDIJobKOKR;
                    model.EDIJobType = model.EDIJobDetail.EDIJobType;
                    model.EDIConID = model.EDIJobDetail.EDIConID;
                    model.ObjectName = model.EDIJobDetail.ObjectName;
                    model.DepEDIJobID = model.EDIJobDetail.DepEDIJobID;
                    model.IsUseRes = model.EDIJobDetail.IsUseRes;
                    model.IgnoreWarning = model.EDIJobDetail.IgnoreWarning;
                    model.IsDisable = model.EDIJobDetail.IsDisable;
                    model.FileSource = model.EDIJobDetail.FileSource;
                    model.FileEncoding = model.EDIJobDetail.FileEncoding;
                    model.URLPath = model.EDIJobDetail.URLPath;
                    model.SortOrder = model.EDIJobDetail.SortOrder;
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIJobDetail")]
        public async Task<ActionResult> GetSysSystemEDIConList(string SysID, string EDIFlowID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult; //登入驗證

            SystemEDIJobDetailModel model = new SystemEDIJobDetailModel
            {
                SysID = SysID,
                EDIFlowID = EDIFlowID
            };

            if (await model.GetSysSystemEDIConList(model.EDIFlowID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SystemEDIConList, true));
            }

            return Json(null);
        }
    }
}
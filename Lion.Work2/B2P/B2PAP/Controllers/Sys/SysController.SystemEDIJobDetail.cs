using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIJobDetail(SystemEDIJobDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemEDIJobDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemEDIJobDetail.SystemMsg_AddSystemEDIJobDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    model.GetEditSystemEDIJobDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIJobDetail.EditSystemEDIJobDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIJobDetailResult = model.GetDeleteSystemEDIJobDetailResult();
                    if (deleteSystemEDIJobDetailResult == EntitySystemEDIJobDetail.EnumDeleteSystemEDIJobDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemEDIJobDetail.DeleteSystemEDIJobDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEDIJobDetailResult == EntitySystemEDIJobDetail.EnumDeleteSystemEDIJobDetailResult.DataExist)
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

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
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

            if (model.GetSysSystemEDIFlowList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetEDIJobTypeList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetEDIJobTypeList);
            }

            if (model.GetSysSystemEDIConList(model.EDIFlowID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysSystemEDIConList);
            }

            if (model.GetSysSystemDepEDIJobList(model.EDIFlowID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSysSystemDepEDIJobList);
            }

            if (model.GetEDIFileEncodingList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetEDIFileEncodingList);
            }
            if (model.ExecAction == EnumActionType.Add) {
                model.GetSortOrder();

            }
            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemEDIJobDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemEDIJobDetail); //無法取得EDIJob清單明細列表
                }
                else
                {
                    model.EDIJobZHTW = model.EntitySystemEDIJobDetail.EDIJobZHTW.GetValue();
                    model.EDIJobZHCN = model.EntitySystemEDIJobDetail.EDIJobZHCN.GetValue();
                    model.EDIJobENUS = model.EntitySystemEDIJobDetail.EDIJobENUS.GetValue();
                    model.EDIJobTHTH = model.EntitySystemEDIJobDetail.EDIJobTHTH.GetValue();
                    model.EDIJobJAJP = model.EntitySystemEDIJobDetail.EDIJobJAJP.GetValue();
                    model.EDIJobType = model.EntitySystemEDIJobDetail.EDIJobType.GetValue();
                    model.EDIConID = model.EntitySystemEDIJobDetail.EDIConID.GetValue();
                    model.ObjectName = model.EntitySystemEDIJobDetail.ObjectName.GetValue();
                    model.DepEDIJobID = model.EntitySystemEDIJobDetail.DepEDIJobID.GetValue();
                    model.IsUseRes = model.EntitySystemEDIJobDetail.IsUseRes.GetValue();
                    model.IsDisable = model.EntitySystemEDIJobDetail.IsDisable.GetValue();
                    model.FileSource = model.EntitySystemEDIJobDetail.FileSource.GetValue();
                    model.FileEncoding = model.EntitySystemEDIJobDetail.EDIFileEncoding.GetValue();
                    model.URLPath = model.EntitySystemEDIJobDetail.URLPath.GetValue();
                    model.SortOrder = model.EntitySystemEDIJobDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIJobDetail")]
        public ActionResult GetSysSystemEDIConList(string SysID, string EDIFlowID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult; //登入驗證
            
            SystemEDIJobDetailModel model = new SystemEDIJobDetailModel();

            model.SysID = SysID;
            model.EDIFlowID = EDIFlowID;

            if (model.GetSysSystemEDIConList(model.EDIFlowID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemEDIConList, true));
            }

            return Json(null);
        }
    }
}
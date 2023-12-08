using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models;
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
        public ActionResult SystemFunDetail(SystemFunDetailModel model, List<EntitySystemFunDetail.SystemMenuFunValue> systemMenuFunValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.CurrentCultureID = base.CultureID;
            model.GetSysSystemFunTabList(_BaseAPModel.EnumTabAction.SysSystemFunDetail);

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemFunDetail(base.CultureID) == true)
                {
                    SetSystemAlertMessage(SysSystemFunDetail.SystemMsg_AddSystemFunDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.GetEditSystemFunDetailResult(AuthState.SessionData.UserID, systemMenuFunValueList) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunDetail.EditSystemFunDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    EntitySystemFunDetail.EnumDeleteSystemFunDetailResult deleteSystemFunDetailResult = model.GetDeleteSystemFunDetailResult();
                    if (deleteSystemFunDetailResult == EntitySystemFunDetail.EnumDeleteSystemFunDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFunDetail.DeleteSystemFunDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemFunDetailResult == EntitySystemFunDetail.EnumDeleteSystemFunDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemFunDetail.DeleteSystemFunDetailResult_DataExist);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemFun", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSubsysIDList(model.SysID, false, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSysSystemSubsysIDList);
            }

            if (model.GetSysSystemPurviewIDList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSysSystemPurviewIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSysSystemFunTypeList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSysSystemFunTypeList);
            }

            if (model.GetSystemFunRoleList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSystemFunRoleList);
            }

            if (model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Copy)
            {
                if (model.GetSystemFunDetail(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_GetSystemFunDetail);
                }
                else
                {
                    model.SubSysID = model.EntitySystemFunDetail.SubSysID.GetValue();
                    model.PurviewID = model.EntitySystemFunDetail.PurviewID.GetValue();
                    model.FunNMZHTW = model.EntitySystemFunDetail.FunNMZHTW.GetValue();
                    model.FunNMZHCN = model.EntitySystemFunDetail.FunNMZHCN.GetValue();
                    model.FunNMENUS = model.EntitySystemFunDetail.FunNMENUS.GetValue();
                    model.FunNMTHTH = model.EntitySystemFunDetail.FunNMTHTH.GetValue();
                    model.FunNMJAJP = model.EntitySystemFunDetail.FunNMJAJP.GetValue();
                    model.FunType = model.EntitySystemFunDetail.FunType.GetValue();
                    model.IsOutside = model.EntitySystemFunDetail.IsOutside.GetValue();
                    model.IsDisable = model.EntitySystemFunDetail.IsDisable.GetValue();
                    model.SortOrder = model.EntitySystemFunDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }        
    }
}
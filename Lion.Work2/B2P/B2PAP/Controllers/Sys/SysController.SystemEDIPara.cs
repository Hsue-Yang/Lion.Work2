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
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIPara()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIParaModel model = new SystemEDIParaModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIParaModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIParaModel.Field.QueryEDIFlowID.ToString());
                model.QueryEDIJobID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIParaModel.Field.QueryEDIJobID.ToString());
            }
            #endregion

            model.GetSysEDIParaTabList(_BaseAPModel.EnumTabAction.SysSystemEDIPara); //TabText

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetEDIParaTypeList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIPara.SystemMsg_GetEDIParaTypeList);
            }

            if (model.GetSystemEDIParaList( base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIPara.SystemMsg_GetSystemEDIParaList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIPara(SystemEDIParaModel model, List<EntitySystemEDIPara.EDIParaParaValue> EDIParaParaValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDIParaTabList(_BaseAPModel.EnumTabAction.SysSystemEDIPara); //TabText

            bool result = true;

            if (base.IsPostBack)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIParaModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIParaModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                paraDict.Add(SystemEDIParaModel.Field.QueryEDIJobID.ToString(), model.QueryEDIJobID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetEDIParaSettingResult(AuthState.SessionData.UserID, base.CultureID, EDIParaParaValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIPara.SystemMsg_SaveEDIParaSortOrderError);
                }
            }

            model.GetSysEDIParaTabList(_BaseAPModel.EnumTabAction.SysSystemEDIPara);

            if (base.IsPostBack) //新增&刪除
            {
                if (result && (model.ExecAction == EnumActionType.Add ) &&
                    model.GetEditSystemEDIParaDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIPara.EditSystemEDIParaResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIParaDetailResult = model.GetDeleteSystemEDIParaDetailResult();
                    if (deleteSystemEDIParaDetailResult == EntitySystemEDIPara.EnumDeleteSystemEDIParaDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemEDIPara.DeleteSystemEDIParaDetailResult_Failure);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemEDIPara", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSysUserSystemSysIDList);
            }
            else
            {
                if (!model.SetHasSysID())
                {
                    SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_SetHasSysID);
                    return RedirectToAction("SystemEDIFlow", "Sys");
                }
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetEDIParaTypeList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIPara.SystemMsg_GetEDIParaTypeList);
            }

            if (model.GetSystemEDIParaList( base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIPara.SystemMsg_GetSystemEDIParaList);
            }

            return View(model);
        }
    }
}
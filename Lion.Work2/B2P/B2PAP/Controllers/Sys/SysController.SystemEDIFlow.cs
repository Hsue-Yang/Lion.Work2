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
        public ActionResult SystemEDIFlow()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIFlowModel model = new SystemEDIFlowModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowModel.Field.QuerySysID.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlow);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSystemEDIFlowList( base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSystemEDIFlowList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIFlow(SystemEDIFlowModel model, List<EntitySystemEDIFlow.EDIFlowValue> EDIFlowValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetEDIFlowSettingResult(AuthState.SessionData.UserID, base.CultureID, EDIFlowValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_SaveEDIFlowSortOrderError);
                }
            }
            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlow);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSysUserSystemSysIDList); 
            }

            if (model.GetSystemEDIFlowList( base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSystemEDIFlowList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIFlowModel.Field.QuerySysID.ToString(), model.QuerySysID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

            }
            if (base.IsPostBack && model.ExecAction == EnumActionType.Copy)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIFlowModel.Field.QuerySysID.ToString(), model.QuerySysID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                //輸出xml進入點
                if (model.SaveEDIXML(model.QuerySysID, base.CultureID, AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemEDIFlow.SysMsg_OutputXMLOK);
                }else
                {
                    SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_SaveEDIXML);
                }
            }
            return View(model);
        }
    }
}

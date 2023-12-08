using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using static ERPAP.Models.Sys.SystemEDIConModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDICon()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIConModel model = new SystemEDIConModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIConModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIConModel.Field.QueryEDIFlowID.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDICon); //TabText

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (string.IsNullOrWhiteSpace(model.QuerySysID))
            {
                var sysUserSystemSysId = model.UserSystemByIdList.FirstOrDefault();
                if (sysUserSystemSysId != null)
                {
                    model.QuerySysID = sysUserSystemSysId.SysID;
                }
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetSystemEDIConList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSystemEDIConList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDICon(SystemEDIConModel model, List<EDIConValue> EDIConValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetEDIConSettingResult(AuthState.SessionData.UserID, base.CultureID, EDIConValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDICon.SystemMsg_SaveEDIConSortOrderError);
                }
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDICon);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetSystemEDIConList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSystemEDIConList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIConModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIConModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Query == true)
            {
                return RedirectToAction("SystemEDIFlow", "Sys");
            }

            return View(model);
        }
    }
}


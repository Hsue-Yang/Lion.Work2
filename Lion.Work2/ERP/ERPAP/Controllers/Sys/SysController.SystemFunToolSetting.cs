using ERPAP.Models.Sys;
using LionTech.Entity;
using LionTech.Web.ERPHelper;
using Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunToolSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunToolSettingModel model = new SystemFunToolSettingModel();

            model.FormReset();

            GetAuthStateUserRole(model);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunToolSettingModel.Field.SysID.ToString());
                model.FunControllerID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunToolSettingModel.Field.FunControllerID.ToString());
                model.FunActionName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunToolSettingModel.Field.FunActionName.ToString());
            }
            #endregion

            if (await model.GetSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunToolSetting.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSysSystemFunControllerIDList(model.SysID, AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunToolSetting.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (await model.GetSystemFunNameList(model.SysID, model.FunControllerID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunToolSetting.SystemMsg_GetSysSystemFunNameList);
            }

            if (!string.IsNullOrEmpty(model.FunControllerID) && !string.IsNullOrEmpty(model.FunActionName) && await model.GetFunToolSettingList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunToolSetting.SystemMsg_GetFunToolSettingList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunToolSetting(SystemFunToolSettingModel model, List<SystemFunToolSettingModel.FunToolSettingValue> funToolSettingValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            GetIsAdSearch(model);

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if(await model.GetEditFunToolSettingResult(AuthState.SessionData.UserID, base.CultureID, funToolSettingValueList) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunToolSetting.EditFunToolSettingResult_Failure);
                    }
                }

                if (model.ExecAction == EnumActionType.Delete)
                {
                    if (await model.GetDeleteFunToolSettingResult(AuthState.SessionData.UserID, base.CultureID, funToolSettingValueList) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunToolSetting.DeleteFunToolSettingResult_Failure);
                    }
                }

                if (model.ExecAction == EnumActionType.Copy)
                {
                    if (await model.GetCopyFunToolSettingResult(AuthState.SessionData.UserID, base.CultureID, funToolSettingValueList) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunToolSetting.CopyFunToolSettingResult_Failure);
                    }
                }
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemFunToolSettingModel.Field.SysID.ToString(), model.SysID);
                paraDict.Add(SystemFunToolSettingModel.Field.FunControllerID.ToString(), model.FunControllerID);
                paraDict.Add(SystemFunToolSettingModel.Field.FunActionName.ToString(), model.FunActionName);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            return RedirectToAction("SystemFunToolSetting", "Sys");
        }

        private void GetIsAdSearch(SystemFunToolSettingModel model)
        {
            if (model.IsAdSearch == EnumYN.Y.ToString())
            {
                model.FunControllerID = model.FunControllerIDSearch;
                model.FunActionName = model.FunActionNameSearch;
            }
        }

        private void GetAuthStateUserRole(SystemFunToolSettingModel model) 
        {
            model.AuthStateUserRole = false;
            foreach (string data in AuthState.SessionData.UserRoleIDs)
            {
                if (data == SystemFunToolSettingModel.AuthStateField.ERPAPSYS.ToString())
                {
                    model.AuthStateUserRole = true;
                    break;
                }
            }
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunToolSetting")]
        public async Task<ActionResult> GetSystemFunToolControllerIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemFunControllerIDList(sysID, AuthState.SessionData.UserID, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunControllerIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunToolSetting")]
        public async Task<ActionResult> GetSystemFunToolFunNameList(string sysID, string funControllerID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemFunNameList(sysID, funControllerID, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunNameList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunToolSetting")]
        public async Task<ActionResult> GetSearchSystemFunToolControllerIDList(string sysID, string condition)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunToolSettingModel model = new SystemFunToolSettingModel();

            if (await model.GetSearchSystemFunToolControllerIDList(sysID, condition, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SystemFunControllerIDList, true));
            }
            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunToolSetting")]
        public async Task<ActionResult> GetSearchSystemFunToolFunNameList(string sysID, string funControllerID, string condition)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunToolSettingModel model = new SystemFunToolSettingModel();

            if (await model.GetSearchSystemFunToolFunNameList(sysID, funControllerID, condition, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SystemFunToolFunNameList, true));
            }
            return Json(null);
        }
    }
}
// 新增日期：2018-01-09
// 新增人員：廖先駿
// 新增內容：元素權限
// ---------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunElm()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemFunElmModel model = new SystemFunElmModel();

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunElm);
            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.ElmDisplayType);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunElmModel.EnumCookieKey.SysID.ToString());
                model.FunControllerID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunElmModel.EnumCookieKey.FunControllerID.ToString());
                model.FunActionName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunElmModel.EnumCookieKey.FunActionName.ToString());
                model.FunElmID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunElmModel.EnumCookieKey.FunElmID.ToString());
                model.FunElmNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunElmModel.EnumCookieKey.FunElmNM.ToString());
                model.IsDisable = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunElmModel.EnumCookieKey.IsDisable.ToString());
            }
            #endregion

            if (await model.GetSystemInfoList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunElm.SystemMsg_GetSystemInfoList_Failure);
            }

            if (await model.GetSysFunElmList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunElm.SystemMsg_GetSysFunElmList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunElm(SystemFunElmModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunElm);
            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.ElmDisplayType);

            #region - Set Cookie -
            Dictionary<string, string> paraDict =
                new Dictionary<string, string>
                {
                    { SystemFunElmModel.EnumCookieKey.SysID.ToString(), model.SysID },
                    { SystemFunElmModel.EnumCookieKey.FunControllerID.ToString(), model.FunControllerID },
                    { SystemFunElmModel.EnumCookieKey.FunActionName.ToString(), model.FunActionName },
                    { SystemFunElmModel.EnumCookieKey.FunElmID.ToString(), model.FunElmID },
                    { SystemFunElmModel.EnumCookieKey.FunElmNM.ToString(), model.FunElmNM },
                    { SystemFunElmModel.EnumCookieKey.IsDisable.ToString(), model.IsDisable }
                };
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (await model.GetSystemInfoList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunElm.SystemMsg_GetSystemInfoList_Failure);
            }

            if (await model.GetSysFunElmList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunElm.SystemMsg_GetSysFunElmList_Failure);
            }

            return View(model);
        }
    }
}
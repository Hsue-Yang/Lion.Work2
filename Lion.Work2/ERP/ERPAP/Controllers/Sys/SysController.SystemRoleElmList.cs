// 新增日期：2018-01-19
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRoleElmList(SystemRoleElmListModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.ExecAction == EnumActionType.Select ||
                model.ExecAction == EnumActionType.Update)
            {
                model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.ElmDisplayType);

                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.EditSystemRoleElmList(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleElmList.SystemMsg_EditSystemRoleElmList_Failure);
                    }
                }

                if (IsPostBack == false)
                {
                    #region - Get Cookie -
                    if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
                    {
                        model.FunControllerID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleElmListModel.EnumCookieKey.FunControllerID.ToString());
                        model.FunActionName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleElmListModel.EnumCookieKey.FunActionName.ToString());
                    }
                    #endregion
                }
                else
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict =
                        new Dictionary<string, string>
                        {
                            { SystemRoleElmListModel.EnumCookieKey.FunControllerID.ToString(), model.FunControllerID },
                            { SystemRoleElmListModel.EnumCookieKey.FunActionName.ToString(), model.FunActionName }
                        };

                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }

                if (await model.GetSystemRoleElmList(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleElmList.SystemMsg_GetSystemRoleElmList_Failure);
                }

                if (await model.GetSysSystemFunControllerIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSysSystemFunControllerIDList);
                }

                if (await model.GetSystemInfoList(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleElmList.SystemMsg_GetSystemInfoList_Failure);
                }

                model.FormReset();
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRoleElmList")]
        public async Task<ActionResult> GetSystemElmID(string sysID, string controllerID, string actionName)
        {
            if (AuthState.IsAuthorized == false)
                return AuthState.UnAuthorizedActionResult;

            SystemRoleElmListModel model = new SystemRoleElmListModel();

            if (await model.GetSystemElmIDList(AuthState.SessionData.UserID, sysID, controllerID, actionName, CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.SystemElmIDList, true));
            }

            return Json(null);
        }
    }
}
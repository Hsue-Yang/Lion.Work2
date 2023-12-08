using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SystemRoleGroup()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemRoleGroupModel model = new SystemRoleGroupModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                //model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleGroupModel.Field.QuerySysID.ToString());
            }
            #endregion

            //model.GetSystemRoleGroupTabList(_BaseAPModel.EnumTabAction.SysSystemRoleGroup);

            if (model.GetSystemRoleGroupList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleGroup.SystemMsg_GetSystemRoleGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemRoleGroup(SystemRoleGroupModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            //model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemRoleGroup);
            
            if (model.GetSystemRoleGroupList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleGroup.SystemMsg_GetSystemRoleGroupList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                //Dictionary<string, string> paraDict = new Dictionary<string, string>();
                //paraDict.Add(SystemRoleGroupModel.Field.QuerySysID.ToString(), model.QuerySysID);
                //AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }
            
            return View(model);
        }
    }
}
using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController 
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemRoleFunList(SystemRoleFunListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (model.ExecAction == EnumActionType.Select)
            {
                if (model.GetSystemRoleFunList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSystemRoleFunList);
                }

                if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false) 
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSystemSysIDList);
                }

                if(model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSysSystemFunControllerIDList);
                }

                if( model.GetSysSystemRoleIDList(model.QuerySysID, base.CultureID)==false)
                {
                    SetSystemErrorMessage(SysSystemRoleFunList.SystemMsg_GetSysSystemRoleIDList);
                }
            }

            return View(model);
        }
    }
}

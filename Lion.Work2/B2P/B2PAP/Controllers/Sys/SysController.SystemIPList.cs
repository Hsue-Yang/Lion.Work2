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
        public ActionResult SystemIPList(SystemIPListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add &&
                    model.InsertSystemIP(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemIPList.InsertSystemIPListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Update &&
                    model.UpdateSystemIP(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemIPList.UpdateSystemIPListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete &&
                    model.DeleteSystemIP() == false)
                {
                    SetSystemErrorMessage(SysSystemIPList.DeleteSystemIPListFailure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSystemIPList(base.PageSize) == false)
            {
                SetSystemErrorMessage(SysSystemIPList.SystemMsg_GetSystemIPList);
            }

            return View(model);
        }
    }
}
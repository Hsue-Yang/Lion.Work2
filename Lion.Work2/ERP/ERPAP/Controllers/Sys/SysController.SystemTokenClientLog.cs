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
        public ActionResult SystemTokenClientLog()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemTokenClientLogModel model = new SystemTokenClientLogModel();
            
            model.FormReset();

            
            if (model.GetTokenClientLogList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemTokenClientLog.SystemMsg_GetSystemSysIDList);
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemTokenClientLog(SystemTokenClientLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select) {
                
                if (model.GetTokenClientLogList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemTokenClientLog.SystemMsg_GetSystemSysIDList);
                }
            }
            return View(model);
        }
    }
}

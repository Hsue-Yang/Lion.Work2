using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemUserList(SystemUserListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.SysNM = HttpUtility.UrlDecode(model.SysNM);

            if (model.ExecAction == EnumActionType.Select && await model.GetSystemUserList(AuthState.SessionData.UserID, PageSize) == false)
            {
                SetSystemErrorMessage(SysSystemUserList.SystemMsg_GetSystemUserList);
            }

            return View(model);
        }
    }
}
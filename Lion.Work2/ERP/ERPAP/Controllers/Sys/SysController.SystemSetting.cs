using ERPAP.Models;
using ERPAP.Models.Sys;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemSettingModel model = new SystemSettingModel();
            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemSetting);

            if (await model.GetUserSystemsAsync(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemSetting.SystemMsg_GetSystemSettingList);
            }

            return View(model);
        }
    }
}
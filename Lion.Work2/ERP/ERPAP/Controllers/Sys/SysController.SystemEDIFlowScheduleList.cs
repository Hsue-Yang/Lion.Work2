using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter("SystemEDIFlow")]
        public async Task<ActionResult> SystemEDIFlowScheduleList(SystemEDIFlowScheduleListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (await model.GetSystemEDIFlowScheduleList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowScheduleList.SystemMsg_GetSystemEDIFlowScheduleList);
            }

            return View(model);
        }
    }
}
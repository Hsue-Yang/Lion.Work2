using ERPAP.Models.Sys;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPIClientDetail(SystemAPIClientDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (await model.GetSystemAPIClientDetail(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemAPIClientDetail.SystemMsg_UnGetSystemAPIClientDetail);
            }

            return View(model);
        }
    }
}
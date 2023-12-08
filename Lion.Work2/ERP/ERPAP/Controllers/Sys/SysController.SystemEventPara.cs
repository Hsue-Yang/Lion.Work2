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
        public async Task<ActionResult> SystemEventPara(SystemEventParaModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            Task<bool> getSysSystemEventFullName = model.GetSysSystemEventFullName(model.SysID, AuthState.SessionData.UserID, model.EventGroupID, model.EventID, CultureID);
            Task<bool> getSystemEventByPara = model.GetSystemEventByPara(AuthState.SessionData.UserID);

            await Task.WhenAll(getSysSystemEventFullName, getSystemEventByPara);

            if (getSysSystemEventFullName.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventPara.SystemMsg_UnGetSysSystemEventFullName);
            }

            if (getSystemEventByPara.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventPara.SystemMsg_UnGetSystemEventByPara);
            }

            return View(model);
        }
    }
}
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
        public async Task<ActionResult> SystemAPIClient(SystemAPIClientModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (IsPostBack == false &&
                (string.IsNullOrWhiteSpace(model.BeginDate) || string.IsNullOrWhiteSpace(model.EndDate)
                || string.IsNullOrWhiteSpace(model.BeginTime) || string.IsNullOrWhiteSpace(model.EndTime)))
            {
                model.FormReset();
            }

            Task<bool> getSysSystemAPIFullName = model.GetSysSystemAPIFullName(model.SysID, AuthState.SessionData.UserID, model.APIGroupID, model.APIFunID, CultureID);
            Task<bool> getSystemAPIClientList = model.GetSystemAPIClientList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getSysSystemAPIFullName, getSystemAPIClientList);

            if (getSysSystemAPIFullName.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIClient.SystemMsg_UnGetSysSystemAPIFullName);
            }

            if (getSystemAPIClientList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIClient.SystemMsg_UnGetSystemAPIClientList);
            }

            return View(model);
        }
    }
}
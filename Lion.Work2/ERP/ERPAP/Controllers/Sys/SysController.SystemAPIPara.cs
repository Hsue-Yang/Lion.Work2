using ERPAP.Models.Sys;
using LionTech.Utility;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;
using static ERPAP.Models._BaseAPModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPIPara(SystemAPIParaModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            Task<bool> getSysSystemAPIFullName = model.GetSysSystemAPIFullName(model.SysID, AuthState.SessionData.UserID, model.APIGroupID, model.APIFunID, CultureID);
            Task<bool> getSysAPIReturnTypeList = model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.APIReturnType), CultureID);
            Task<bool> getSystemAPIPara = model.GetSystemAPIPara(AuthState.SessionData.UserID);

            await Task.WhenAll(getSysSystemAPIFullName, getSysAPIReturnTypeList, getSystemAPIPara);

            if (getSysSystemAPIFullName.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIPara.SystemMsg_UnGetSysSystemAPIFullName);
            }

            if (getSysAPIReturnTypeList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIPara.SystemMsg_UnGetSysAPIReturnTypeList);
            }

            if (getSystemAPIPara.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIPara.SystemMsg_UnGetSystemAPIPara);
            }

            return View(model);
        }
    }
}
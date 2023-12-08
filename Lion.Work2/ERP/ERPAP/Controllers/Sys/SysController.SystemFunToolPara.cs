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
        public async Task<ActionResult> SystemFunToolPara(SystemFunToolParaModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (await model.GetSystemFunToolParaForm(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunToolPara.SystemMsg_GetSystemFunToolParaForm_Failure);
            }

            if (await model.GetSystemFunToolParaList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunToolPara.SystemMsg_GetSystemFunToolParaList_Failure);
            }

            return View(model);
        }
    }
}
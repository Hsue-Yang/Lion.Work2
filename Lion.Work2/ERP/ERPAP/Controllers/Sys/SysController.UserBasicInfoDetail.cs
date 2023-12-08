using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserBasicInfoDetail(UserBasicInfoDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetUserADSTabList(_BaseAPModel.EnumTabAction.SysUserBasicInfoDetail);

            if (model.GetBaseRestrictTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysUserBasicInfoDetail.SystemMsg_GetBaseRestrictTypeList);
            }

            if (await model.GetUserBasicInfoDetail(CultureID, AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysUserBasicInfoDetail.SystemMsg_GetUserBasicInfoDetail);
            }

            return View(model);
        }
    }
}
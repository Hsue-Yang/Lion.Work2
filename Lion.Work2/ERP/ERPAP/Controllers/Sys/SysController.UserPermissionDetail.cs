using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
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
        public async Task<ActionResult> UserPermissionDetail(UserPermissionDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    UserPermissionDetailModel.EnumModifyResult modifyResult = model
                        .GetEditUserPermissionResult(AuthState.SessionData.UserID, base.ClientIPAddress(), base.CultureID);

                    if (modifyResult != UserPermissionDetailModel.EnumModifyResult.Success)
                    {
                        if (modifyResult == UserPermissionDetailModel.EnumModifyResult.Failure)
                            SetSystemErrorMessage(SysUserPermissionDetail.EditUserPermissionResult_Failure);
                        else if (modifyResult == UserPermissionDetailModel.EnumModifyResult.SyncASPFailure)
                            SetSystemErrorMessage(SysUserPermissionDetail.EditUserPermissionResult_SyncASPFailure);

                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("UserPermission", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.RestrictType), base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserPermissionDetail.SystemMsg_GetBaseRestrictTypeList);
            }

            if (await model.GetUserRawData() == false)
            {
                SetSystemErrorMessage(SysUserPermissionDetail.SystemMsg_GetUserRawData);
            }
            else
            {
                model.UserNM = model.UserRawData.UserNM;
                model.RestrictType = model.UserRawData.RestrictType;
                model.IsLock = model.UserRawData.IsLock;
            }

            return View(model);
        }
    }
}
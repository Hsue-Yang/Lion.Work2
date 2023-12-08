using System;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        public ActionResult UserMain()
        {
            UserMainModel model = new UserMainModel();

            model.FormReset();
            model.IsForced = true;
            model.UserID = AuthState.SessionData.UserID;

            if (model.GetUserMain() == false)
            {
                SetSystemErrorMessage(SysUserMain.SystemMsg_GetUserMainFailure);
            }
            else
            {
                if (model.EntityUserMain.PWDValidDate.GetValue() > DateTime.Now)
                    return AuthState.UnAuthorizedActionResult;

                model.UserNM = model.EntityUserMain.UserNM.GetValue();
                model.UserENM = model.EntityUserMain.UserENM.GetValue();
                model.UserTel = model.EntityUserMain.UserTel.GetValue();
                model.UserExtension = model.EntityUserMain.UserExtension.GetValue();
                model.UserMobile = model.EntityUserMain.UserMobile.GetValue();
                model.UserEMail = model.EntityUserMain.UserEMail.GetValue();
                model.UserGoogleAccount = model.EntityUserMain.UserGoogleAccount.GetValue();
                model.IsGAccEnable = model.EntityUserMain.IsGAccEnable.GetValue();
            }

            SetSystemAlertMessage(SysUserMain.SystemMsg_PWDExpirationNotification);

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserMain(UserMainModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update && model.GetUserMain())
                {
                    if (model.ValidateUserPWD() != UserMainModel.EnumValidateUserPWD.Correctly)
                    {
                        SetSystemErrorMessage(SysUserMain.SystemMsg_UserPWDIsError);
                        result = false;
                    }

                    if (result && model.GetPWDEffectiveResult() == false)
                    {
                        SetSystemErrorMessage(SysUserMain.SystemMsg_UserPWDIsRepeated);
                        result = false;
                    }

                    if (result)
                    {
                        UserMainModel.EnumModifyResult modifyResult = model.GetEditUserMainResult(
                            AuthState.SessionData.UserID, base.ClientIPAddress(), base.CultureID);

                        if (modifyResult != UserMainModel.EnumModifyResult.Success)
                        {
                            if (modifyResult == UserMainModel.EnumModifyResult.Failure)
                                SetSystemErrorMessage(SysUserMain.SystemMsg_EditUserMainFailure);
                            else if (modifyResult == UserMainModel.EnumModifyResult.SyncASPFailure)
                                SetSystemErrorMessage(SysUserMain.SystemMsg_EditUserMainFailure_SyncASPFailure);

                            result = false;
                        }
                    }

                    if (model.IsForced)
                    {
                        if (!string.IsNullOrWhiteSpace(model.PWDValidDate))
                        {
                            AuthState.IsPWDExpired = false;
                            return RedirectToAction("Bulletin", "Pub");
                        }

                        return RedirectToAction("UserMain", "Sys");
                    }
                }

                if (result)
                {
                    SetSystemAlertMessage(SysUserMain.SystemMsg_EditUserMainSuccess);
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.UserID != AuthState.SessionData.UserID)
            {
                model.IsPermissible = false;
                SetSystemAlertMessage(SysUserMain.SystemMsg_PermissionDenied);
            }
            else
            {
                model.IsPermissible = true;
            }

            if (model.GetUserMain() == false)
            {
                SetSystemErrorMessage(SysUserMain.SystemMsg_GetUserMainFailure);
            }
            else
            {
                model.UserNM = model.EntityUserMain.UserNM.GetValue();
                model.UserENM = model.EntityUserMain.UserENM.GetValue();
                model.UserTel = model.EntityUserMain.UserTel.GetValue();
                model.UserExtension = model.EntityUserMain.UserExtension.GetValue();
                model.UserMobile = model.EntityUserMain.UserMobile.GetValue();
                model.UserEMail = model.EntityUserMain.UserEMail.GetValue();
                model.UserGoogleAccount = model.EntityUserMain.UserGoogleAccount.GetValue();
                model.IsGAccEnable = model.EntityUserMain.IsGAccEnable.GetValue();
            }

            return View(model);
        }
    }
}
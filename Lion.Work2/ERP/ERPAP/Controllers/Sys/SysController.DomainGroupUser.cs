using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Utility;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult DomainGroupUser(DomainGroupUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.DomainAccount = model.GetUserEMail(AuthState.SessionData.UserID);

            if (string.IsNullOrWhiteSpace(model.DomainPath) == false &&
                model.GetDomainTypeByLDAPPath(model.DomainPath) == SysModel.EnumDomainType.LionTech)
            {
                model.DomainPWD = AuthState.SessionData.APSession(AuthState.SystemID + AuthState.ControllerName + "DomainGroup");
            }

            if (model.GetDomainGroupUserList(CultureID) == false)
            {
                SetSystemErrorMessage(SysDomainGroupUser.SystemMsg_GetDomainGroupUserList_Failure);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult DomainGroupUser()
        {
            DomainGroupUserModel model = new DomainGroupUserModel();
            model.DomainPath = TempData[nameof(model.DomainPath)]?.ToString();
            model.DomainGroupNM = TempData[nameof(model.DomainGroupNM)]?.ToString();

            if (string.IsNullOrWhiteSpace(AuthState.SessionData.UserID) ||
                string.IsNullOrWhiteSpace(model.DomainPath) ||
                string.IsNullOrWhiteSpace(model.DomainGroupNM))
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.DomainPath = Common.GetEnumDesc((SysModel.EnumDomainType)Enum.Parse(typeof(SysModel.EnumDomainType), model.DomainPath));

            if (model.GetDomainGroupUserList(CultureID) == false)
            {
                SetSystemErrorMessage(SysDomainGroupUser.SystemMsg_GetDomainGroupUserList_Failure);
            }

            if (model.IsUserAuthorized(AuthState.SessionData.UserID) == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult DomainGroupUserProxy(DomainGroupUserModel model)
        {
            if (AuthState.IsLogined == false)
            {
                string targetUrl =
                    string.Join("/",
                        Common.GetEnumDesc(EnumSystemID.ERPAP),
                        AuthState.ControllerName,
                        AuthState.ActionName);

                targetUrl = $"{targetUrl}?{nameof(model.DomainPath)}={model.DomainPath}&{nameof(model.DomainGroupNM)}={HttpUtility.UrlEncode(model.DomainGroupNM)}";

                string redirectUrl =
                    string.Format(ConfigurationManager.AppSettings[EnumAppSettingKey.ASPTokenDelayURL.ToString()]
                        , HttpUtility.UrlEncode(targetUrl));

                return Redirect(redirectUrl);
            }

            TempData[nameof(model.DomainPath)] = model.DomainPath;
            TempData[nameof(model.DomainGroupNM)] = Security.Decrypt(model.DomainGroupNM);

            return RedirectToAction("DomainGroupUser");
        }
    }
}
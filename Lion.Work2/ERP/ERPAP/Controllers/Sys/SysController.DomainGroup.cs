using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Utility;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult DomainGroup()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            DomainGroupModel model = new DomainGroupModel();

            model.GetSysDomainTabList(_BaseAPModel.EnumTabAction.SysDomainGroup);
            model.DomainAccount = model.GetUserEMail(AuthState.SessionData.UserID);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.DomainPath = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, DomainGroupModel.EnumCookieKey.DomainPath.ToString());
                model.DomainSecondLevelPath = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, DomainGroupModel.EnumCookieKey.DomainSecondLevelPath.ToString());
                model.DomainThridLevelPath = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, DomainGroupModel.EnumCookieKey.DomainThridLevelPath.ToString());
            }
            #endregion

            if (string.IsNullOrWhiteSpace(model.DomainPath) == false &&
                model.GetDomainTypeByLDAPPath(model.DomainPath) == SysModel.EnumDomainType.LionTech)
            {
                model.DomainPWD = AuthState.SessionData.APSession(AuthState.SystemFunKey);
            }

            if (string.IsNullOrWhiteSpace(model.DomainPWD))
            {
                model.DomainPath = Common.GetEnumDesc(SysModel.EnumDomainType.LionMail);
            }

            if (model.GetDomainInfoList(PageSize) == false)
            {
                SetSystemErrorMessage(SysDomainGroup.SystemMsg_GetSysDomainGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult DomainGroup(DomainGroupModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysDomainTabList(_BaseAPModel.EnumTabAction.SysDomainGroup);
            model.DomainAccount = model.GetUserEMail(AuthState.SessionData.UserID);

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(DomainGroupModel.EnumCookieKey.DomainPath.ToString(), model.DomainPath);
            paraDict.Add(DomainGroupModel.EnumCookieKey.DomainSecondLevelPath.ToString(), model.DomainSecondLevelPath);
            paraDict.Add(DomainGroupModel.EnumCookieKey.DomainThridLevelPath.ToString(), model.DomainThridLevelPath);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (string.IsNullOrWhiteSpace(model.DomainPath) == false &&
                model.GetDomainTypeByLDAPPath(model.DomainPath) == SysModel.EnumDomainType.LionTech)
            {
                model.DomainPWD = AuthState.SessionData.APSession(AuthState.SystemFunKey);
            }

            if (model.GetDomainInfoList(PageSize) == false)
            {
                SetSystemErrorMessage(SysDomainGroup.SystemMsg_GetSysDomainGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("DomainGroup")]
        public ActionResult GetDomainSubLevelList(DomainGroupModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.DomainAccount = model.GetUserEMail(AuthState.SessionData.UserID);
            
            if (string.IsNullOrWhiteSpace(model.LDAPPath) == false &&
                model.GetDomainTypeByLDAPPath(model.LDAPPath) == SysModel.EnumDomainType.LionTech)
            {
                if (string.IsNullOrWhiteSpace(model.DomainPWD) == false)
                {
                    AuthState.SessionData.SetAPSession(AuthState.SystemFunKey, Security.Encrypt(model.DomainPWD));
                }

                model.DomainPWD = AuthState.SessionData.APSession(AuthState.SystemFunKey);
            }

            return Json(model.GetOneLevelList(model.LDAPPath), JsonRequestBehavior.AllowGet);
        }
    }
}

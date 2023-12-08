using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult UserDomain()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserDomainModel model = new UserDomainModel();
            
            model.GetSysDomainTabList(_BaseAPModel.EnumTabAction.SysUserDomain);

            if (model.GetUserDomainList(CultureID, PageSize) == false)
            {
                SetSystemErrorMessage(SysUserDomain.SystemMsg_GetSysUserDomainList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserDomain(UserDomainModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysDomainTabList(_BaseAPModel.EnumTabAction.SysUserDomain);
            
            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                if (model.GetUserDomainList(CultureID, PageSize) == false)
                {
                    SetSystemErrorMessage(SysUserDomain.SystemMsg_GetSysUserDomainList);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("UserDomain")]
        public ActionResult GetUserDoaminInfoList(UserDomainModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.DomainAccount = model.GetUserEMail(AuthState.SessionData.UserID);
            model.DomainPWD = AuthState.SessionData.APSession(AuthState.SystemFunKey);

            return Json(model.GetUserDoaminInfoList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizationActionFilter("UserDomain")]
        public ActionResult GetDomainLoginResult(string pwd)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SysModel model = new SysModel();

            if (model.GetDomainLoginResult(Common.GetEnumDesc(SysModel.EnumDomainType.LionTech), model.GetUserEMail(AuthState.SessionData.UserID), pwd))
            {
                AuthState.SessionData.SetAPSession(AuthState.SystemFunKey, Security.Encrypt(pwd));
                return Json(new
                {
                    isLogin = true
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                isLogin = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

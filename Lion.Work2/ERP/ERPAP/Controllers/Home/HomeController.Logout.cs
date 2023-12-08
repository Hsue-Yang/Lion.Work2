using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Home;
using LionTech.AspNet.Authentication.Jwt;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            UserLogout();

            return RedirectToAction("Index", "Home");
        }

        private void UserLogout()
        {
            LogoutModel model = new LogoutModel();
            model.ExecUserConnectCustLogout(AuthState.SessionData.UserID, AuthState.SessionData.SessionID);
            
            if (Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.JwtEnable.ToString()]))
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                IJwtFactory jwtFactory = new JwtFactory();
                jwtFactory.ClearJwtWithCookies(Response.Cookies);
                HttpContext.User = null;
            }

            AuthState.SessionData.Clear();
        }
    }
}

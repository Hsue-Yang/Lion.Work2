using System.Configuration;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Home;
using LionTech.Entity.ERP;
using LionTech.Utility;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult TokenRedirect(TokenModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.T) &&
                !string.IsNullOrWhiteSpace(model.K) &&
                !string.IsNullOrWhiteSpace(model.C) &&
                !string.IsNullOrWhiteSpace(model.U) &&
                model.GetSystemMain(EnumSystemID.ERPAP.ToString()) == true)
            {
                string systemKey = model.EntitySystemMain.SysKey.GetValue();
                string paraT = string.Empty;
                string paraK = model.EntitySystemMain.ENSysID.GetValue();
                string paraC = model.C;
                string paraU = string.Empty;
                string isValid = bool.FalseString;
                
                if (string.IsNullOrWhiteSpace(AuthState.SessionData.UserID) == false &&
                    AuthState.SessionData.UserID != model.U)
                {
                    UserLogout();
                }

                if ((string.IsNullOrWhiteSpace(AuthState.SessionData.UserID) && string.IsNullOrWhiteSpace(AuthState.SessionData.UserToken) && paraK == model.K) ||
                    (!string.IsNullOrWhiteSpace(AuthState.SessionData.UserID) && !string.IsNullOrWhiteSpace(AuthState.SessionData.UserToken) && paraK == model.K && AuthState.SessionData.UserID == model.U))
                {
                    paraT = model.T;
                    paraU = Token.Encrypt(model.U, systemKey);

                    int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

                    string apiURL = string.Format(
                        ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITKNAPTokenServiceValidationURL.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.TKNAP), paraT, paraK, paraC, paraU }
                    );

                    isValid = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut);
                }

                if (string.IsNullOrWhiteSpace(isValid) == false && isValid == bool.TrueString)
                {
                    return RedirectToAction("TokenIndex", "Home", new { UserToken = paraT, UserID = model.U, TargetUrl = model.TargetUrl });
                }
                else
                {
                    TempData["ERPLoginModelUserID"] = model.U;
                    TempData["ERPLoginModelTargetUrl"] = model.TargetUrl;
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult TokenDelay(TokenModel model)
        {
            if (string.IsNullOrWhiteSpace(AuthState.SessionData.UserID) == false &&
                model.GetSystemMain(EnumSystemID.ERPAP.ToString()))
            {
                string systemKey = model.EntitySystemMain.SysKey.GetValue();
                string paraT = AuthState.SessionData.UserToken;
                string paraK = model.EntitySystemMain.ENSysID.GetValue();
                string paraC = Token.Encrypt(base.ClientIPAddress(), systemKey);
                string paraU = Token.Encrypt(AuthState.SessionData.UserID, systemKey);
                string targetUrl = HttpUtility.UrlEncode(model.TargetUrl);

                EnumExecTokenDelayResult strTokenDelayValidationResult = base.ExecTokenDelay(paraT, paraK, paraC, paraU);
                if (strTokenDelayValidationResult == EnumExecTokenDelayResult.Success)
                {
                    string url = string.Format(
                        ConfigurationManager.AppSettings[EnumAppSettingKey.ASPMisTokenRedirectURL.ToString()],
                        paraT, paraK, paraC, AuthState.SessionData.UserID, targetUrl
                        );

                    return Redirect(url);
                }
                else if (strTokenDelayValidationResult == EnumExecTokenDelayResult.Failure)
                {
                    return Redirect(ConfigurationManager.AppSettings[EnumAppSettingKey.ASPLoginURL.ToString()]);
                }
                else
                {
                    return RedirectToAction("UnAuthorizated", "Home");
                }
            }

            return RedirectToAction("UnAuthorizated", "Home");
        }
    }
}
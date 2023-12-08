using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Home;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using Newtonsoft.Json;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SSORedirectToAuth()
        {
            string ssoDomain = ConfigurationManager.AppSettings[EnumAppSettingKey.SSODomain.ToString()];
            string scopeURL =  $"{ssoDomain}{ConfigurationManager.AppSettings[EnumAppSettingKey.SSOScopeURL.ToString()]}";
            var dicState = new Dictionary<string, string>();
            
            EnumCultureID finalCultureID = _GetCultureID(AuthState.CookieData.CultureID);
            
            if (TempData["ERPLoginModelUserID"] != null)
            {
                dicState.Add("UserID", TempData["ERPLoginModelUserID"].ToString());
            }
            if (TempData["ERPLoginModelTargetUrl"] != null)
            {
                dicState.Add("TargetUrl", Security.Encrypt(TempData["ERPLoginModelTargetUrl"].ToString()));
            }
            if (TempData["ProxyLoginModelSystemID"] != null)
            {
                dicState.Add("ProxyLoginSystemID", TempData["ProxyLoginModelSystemID"].ToString());
            }
            if (TempData["ProxyLoginToAp"] != null)
            {
                dicState.Add("ProxyLoginToAp", TempData["ProxyLoginToAp"].ToString());
            }

            dicState.Add("EnvironmentRemind", _GetEnvironmentRemind());
            dicState.Add("CultureID", finalCultureID.ToString());

            return Redirect(string.Format(scopeURL, $"{Common.GetEnumDesc(EnumSystemID.ERPAP)}/home/ssocallback",
                Common.GetJsonSerializeObject(dicState)));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SSOCallback(string code, string state)
        {
            Dictionary<string, string> dicState = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(state) == false)
            {
                dicState = Common.GetJsonDeserializeObject<Dictionary<string, string>>(state);
            }
            var token = await GetAccessToken(code);

            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = await GetUserInfo(token);
            return AutoLoginWithSSO(new IndexModel
            {
                UserID = userId,
                TargetUrl = dicState.ContainsKey(nameof(IndexModel.TargetUrl)) ? Security.Decrypt(dicState[nameof(IndexModel.TargetUrl)]) : null,
                ProxyLoginSystemID = dicState.ContainsKey(nameof(IndexModel.ProxyLoginSystemID)) ? dicState[nameof(IndexModel.ProxyLoginSystemID)] : null,
                ProxyLoginToAp = dicState.ContainsKey(nameof(IndexModel.ProxyLoginToAp)) ? dicState[nameof(IndexModel.ProxyLoginToAp)] : null,
                CultureID = dicState.ContainsKey(nameof(IndexModel.CultureID)) ? dicState[nameof(IndexModel.CultureID)] : null
            });
        }
        
        private async Task<AccessTokenModel> GetAccessToken(string code)
        {
            string ssoDomain = ConfigurationManager.AppSettings[EnumAppSettingKey.SSODomain.ToString()];
            using (HttpClient client = new HttpClient())
            {
                var requestUrl = $"{ssoDomain}/connect/token";
                var codeVerifier = Request.Cookies["codeVerifier"];
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", "Serp"),
                    new KeyValuePair<string, string>("client_secret", "serp-secret"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", $"{Common.GetEnumDesc(EnumSystemID.ERPAP)}/home/ssocallback"),
                    new KeyValuePair<string, string>("code_verifier", codeVerifier?.Value)
                });
                var response = await client.PostAsync(requestUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<AccessTokenModel>(responseContent);
                    return model;
                }
                return null;
            }
        }

        private async Task<string> GetUserInfo(AccessTokenModel accessToken)
        {
            string ssoDomain = ConfigurationManager.AppSettings[EnumAppSettingKey.SSODomain.ToString()];
            using (HttpClient client = new HttpClient())
            {
                var requestUrl = $"{ssoDomain}/connect/userinfo";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string userInfoJson = await response.Content.ReadAsStringAsync();
                    var userInfo = Common.GetJsonDeserializeObject<Dictionary<string, string>>(userInfoJson);
                    return userInfo["username"];
                }
                return null;
            }
        }

        private JwtSecurityToken ParseIdToken(string idToken)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(idToken);

                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ID Token 解析失敗: {ex.Message}");
            }

            return null;
        }
        
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SSOLogin(string systemID, string targetPath)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SSOLoginModel model = new SSOLoginModel { SystemID = systemID };
            if (model.GetSystemMain(systemID))
            {
                model.ExecUserConnectSSOValid(AuthState.SessionData.UserID, AuthState.SessionData.SessionID, EnumYN.Y);

                string redirectURL =
                    string.Format(model.EntitySystemMain.SysIndexPath.GetValue(),
                        new object[]
                        {
                            AuthState.SessionData.UserID, systemID,
                            Security.Encrypt(AuthState.SessionData.SessionID)
                        });

                if (string.IsNullOrWhiteSpace(targetPath) == false)
                {
                    redirectURL = string.Concat(redirectURL, "&TargetPath=" + HttpUtility.UrlEncode(targetPath));
                }

                return RedirectPermanent(redirectURL);
            }
            else
            {
                return RedirectToAction("GenericError", "Home");
            }
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult GsaSSOLogin(string systemID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SSOLoginModel model = new SSOLoginModel { SystemID = systemID };
            if (model.GetSystemMain(systemID))
            {
                model.ExecUserConnectSSOValid(AuthState.SessionData.UserID, AuthState.SessionData.SessionID, EnumYN.Y);

                string redirectURL =
                    string.Format(model.EntitySystemMain.SysIndexPath.GetValue(),
                        new object[]
                        {
                            AuthState.SessionData.UserID, systemID,
                            Security.Encrypt(AuthState.SessionData.SessionID)
                        });

                redirectURL = string.Concat(redirectURL, "&Status=Validate");

                return RedirectPermanent(redirectURL);
            }
            else
            {
                return RedirectToAction("GenericError", "Home");
            }
        }

        [HttpGet]
        [AuthorizationActionFilter("SSOLogin")]
        public ActionResult SSOAgent(string TargetURL)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SSOLoginModel model = new SSOLoginModel();
            if (!string.IsNullOrWhiteSpace(TargetURL))
            {
                model.ExecUserConnectSSOValid(AuthState.SessionData.UserID, AuthState.SessionData.SessionID, EnumYN.Y);

                return RedirectPermanent(TargetURL);
            }
            else
            {
                return RedirectToAction("GenericError", "Home");
            }
        }

        [HttpGet]
        public ContentResult SSOLoginAuthorizationResult(SSOLoginModel model)
        {
            if (SSOLoginAuthorization(model))
            {
                if (model.GetUserSessionData())
                {
                    return Content(model.SSOLoginResult.SerializeToJson());
                }
            }
            return Content(model.SSOLoginResult.SerializeToJson());
        }

        [HttpGet]
        public bool SSOLoginAuthorization(SSOLoginModel model)
        {
            string counterName = "State Server Sessions Active";
            string categoryName = "ASP.NET State Service";
            string machineName = ConfigurationManager.AppSettings[EnumAppSettingKey.StateServerMachineName.ToString()];
            bool result = false;

            try
            {
                if (model.ValidateIP(base.ClientIPAddress()))
                {
                    PerformanceCounter performanceCounter;
                    int sessionCount = 0;
                    
                    ImpersonateHelper.Excute(() =>
                    {
                        if (PerformanceCounterCategory.CounterExists(counterName, categoryName, machineName))
                        {
                            performanceCounter = new PerformanceCounter(categoryName, counterName, string.Empty, machineName);
                            sessionCount = int.Parse(performanceCounter.NextValue().ToString());
                        }
                        else
                        {
                            this.FileLogWrite(model.UserID, model.SystemID, model.SessionID, false);
                            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.SSOLogin), "Performance Counter Not Exists");
                        }
                    });

                    if (model.GetUserConnect(sessionCount, -20) == true)
                    {
                        this.FileLogWrite(model.UserID, model.SystemID, model.SessionID, true);
                        result = true;
                    }
                    else
                    {
                        this.FileLogWrite(model.UserID, model.SystemID, model.SessionID, false);
                    }
                }
                else
                {
                    this.FileLogWrite(model.UserID, model.SystemID, model.SessionID, false);
                    FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.SSOLogin), "User Host Address: " + base.ClientIPAddress());
                }
            }
            catch (Exception ex)
            {
                this.FileLogWrite(model.UserID, model.SystemID, model.SessionID, ex);
            }

            model.ExecUserConnectSSOValid(model.UserID, model.SessionID, EnumYN.N);

            return result;
        }

        private void FileLogWrite(string userID, string systemID, string sessionID, bool result)
        {
            MethodBase methodBase1 = Common.GetMethodBase(1);

            string logMessage = string.Concat(new string[]
            {
                EnumRootPathFile.SSOLogin.ToString(), ": ", methodBase1.DeclaringType.FullName, ".", methodBase1.Name,
                " ; UserID: ", userID, 
                " ; SystemID: ", systemID, 
                " ; SessionID: ", sessionID, 
                " ; Return: ", result.ToString()
            });

            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.SSOLogin), logMessage);
        }

        private void FileLogWrite(string userID, string systemID, string sessionID, Exception ex)
        {
            MethodBase methodBase1 = Common.GetMethodBase(1);

            string logMessage = string.Concat(new string[]
            {
                EnumRootPathFile.SSOLogin.ToString(), ": ", methodBase1.DeclaringType.FullName, ".", methodBase1.Name,
                " ; UserID: ", userID, 
                " ; SystemID: ", systemID, 
                " ; SessionID: ", sessionID
            });

            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), logMessage);
            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), ex);
        }
    }
}
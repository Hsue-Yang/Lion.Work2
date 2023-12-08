using ERPAP.Models.Home;
using LionTech.APIService.IdentikeyAuthWrapper;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using LionTech.AspNet.Authentication.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Environment.GetEnvironmentVariable("EnableSerpAuth")?.ToUpper() == bool.TrueString.ToUpper())
            {
                return SSORedirectToAuth();
            }
            //SessionIDManager sessionIDManager = new SessionIDManager();
            //string newSessionID = sessionIDManager.CreateSessionID(HttpContext.ApplicationInstance.Context);
            //bool redirected = false;
            //bool cookieAdded = false;
            //sessionIDManager.SaveSessionID(HttpContext.ApplicationInstance.Context, newSessionID, out redirected, out cookieAdded);

            EnumCultureID finalCultureID = _GetCultureID(AuthState.CookieData.CultureID);

            IndexModel model = new IndexModel();

            if (model.GetValidateIPAddressIsTrust(ClientIPAddress()) == Entity_Base.EnumValidateIPAddressIsTrustResult.Reject)
            {
                return AuthState.RejectiveActionResult;
            }

            if (TempData["ERPLoginModelUserID"] != null)
            {
                model.UserID = TempData["ERPLoginModelUserID"].ToString();
            }
            if (TempData["ERPLoginModelTargetUrl"] != null)
            {
                model.TargetUrl = TempData["ERPLoginModelTargetUrl"].ToString();
            }
            if (TempData["ProxyLoginModelSystemID"] != null)
            {
                model.ProxyLoginSystemID = TempData["ProxyLoginModelSystemID"].ToString();
            }
            if (TempData["ProxyLoginToAp"] != null)
            {
                model.ProxyLoginToAp = TempData["ProxyLoginToAp"].ToString();
            }

            model.EnvironmentRemind = _GetEnvironmentRemind();

            model.CultureID = finalCultureID.ToString();
            if (model.GetBaseCultureIDList(finalCultureID) == false)
            {
                SetSystemErrorMessage(HomeIndex.SystemMsg_GetBaseCultureIDList);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            EnumCultureID finalCultureID = _GetCultureID(model.CultureID);

            Entity_Base.EnumValidateIPAddressIsTrustResult trustResult = model.GetValidateIPAddressIsTrust(ClientIPAddress());
            if (trustResult == Entity_Base.EnumValidateIPAddressIsTrustResult.Reject)
            {
                return AuthState.RejectiveActionResult;
            }

            model.EnvironmentRemind = _GetEnvironmentRemind();

            if (model.GetBaseCultureIDList(finalCultureID) == false)
            {
                SetSystemErrorMessage(HomeIndex.SystemMsg_GetBaseCultureIDList);
            }

            bool isValid = false;
            bool isLoginExApiValidation = Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.IsLoginExApiValidation.ToString()]);

            if (model.ExecAction == EnumActionType.Update)
            {
                isValid = TryValidatableObject(model);
            }

            if (isValid)
            {
                isValid = false;

                try
                {
                    IUserAuthentication employee = null;

                    if (ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain))
                    {
                        var isWriteLog = Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.IsWriteLog.ToString()]);

                        if (isLoginExApiValidation)
                        {
                            employee = _GetAuthenticationOTPResult(model);

                            if (employee.Result == EnumUserAuthenticationResult.Success ||
                                employee.Result == EnumUserAuthenticationResult.PasswordExpires)
                            {
                                isValid = true;
                            }
                        }
                        else
                        {
                            string userID = GetConvertUserIDByOTP(model.UserID);
                            Digipass digipass = new Digipass();
                            AuthUserResults authUserResults = digipass.GetIsAuthorized(userID, model.UserPassword);

                            // 確定log是否寫入
                            isValid = authUserResults.CredentialResults.ResultCodes.ReturnCodeEnum == EnumReturnCode.RET_SUCCESS;

                            if (isValid == false)
                            {
                                string name = $"SystemMsg_OTP_{authUserResults.CredentialResults.ResultCodes.StatusCodeEnum}";
                                System.Resources.ResourceManager resourceMan = new System.Resources.ResourceManager(typeof(HomeIndex).FullName, System.Reflection.Assembly.Load("App_GlobalResources"));
                                string value = resourceMan.GetString(name, System.Threading.Thread.CurrentThread.CurrentCulture) ?? authUserResults.CredentialResults.ResultCodes.StatusCodeEnum.ToString();
                                SetSystemAlertMessage(value.Replace("\n", "<br/>"));
                            }
                        }

                        if (isWriteLog && employee != null)
                        {
                            model.GetRecordUserLoginResult(employee, ClientIPAddress(), CultureID);
                        }
                    }
                    else
                    {
                        #region - Development -
                        isValid = true;
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
                }
            }

            if (isValid && model.ExecAction == EnumActionType.Update)
            {
                if (_GetLoginResult(model))
                {
                    _GetTokenInfo(model);

                    _GetUserMenuContentXSL(model);

                    model.ValidFirstLogin(model.UserID, AuthState.SessionData.UserID);

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.JwtEnable.ToString()]))
                    {
                        _GenerateJwtToken();
                    }

                    if (string.IsNullOrWhiteSpace(model.ProxyLoginSystemID) == false)
                    {
                        return RedirectToAction("SSOLogin", "Home", new { systemID = model.ProxyLoginSystemID });
                    }

                    if (model.TargetUrl != null)
                    {
                        return Redirect(model.TargetUrl);
                    }

                    #region - Login Event -
                    if (ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain))
                    {
                        if (model.GetUserLoginEvent(model.UserID) == false)
                        {
                            SetSystemErrorMessage(HomeIndex.SystemMsg_GetUserLoginEvent_Failure);
                        }
                        else if (model.UserLoginEvent != null)
                        {
                            bool isValidLoginEventResult = true;

                            if (model.UserLoginEvent.ValidPath.IsNull() == false)
                            {
                                isValidLoginEventResult = _GetValidLoginEventResult(model.UserLoginEvent.ValidPath.GetValue(), model.UserID);
                            }

                            if (isValidLoginEventResult == false)
                            {
                                if (model.UserLoginEvent.IsOutSourcing.GetValue() == EnumYN.Y.ToString())
                                {
                                    return RedirectToAction("SSOLogin", "Home", new { systemID = model.UserLoginEvent.SysID.GetValue(), targetPath = model.UserLoginEvent.TargetPath.GetValue() });
                                }
                                return Redirect(model.UserLoginEvent.TargetPath.GetValue());
                            }
                        }
                    }
                    #endregion

                    return RedirectToAction("Bulletin", "Pub");
                }

                isValid = false;
            }

            if (isValid == false)
            {
                model.FormReset();
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AutoLogin(string returnUrl)
        {
            IJwtFactory jwtFactory = new JwtFactory();

            ClaimsPrincipal principal;

            try
            {
                jwtFactory.ValidateToken(jwtFactory.GetToken(Request.Cookies), out principal);

                string userid = (from claim in principal.Claims
                                 where claim.Type == ClaimTypes.Name
                                 select claim.Value).SingleOrDefault();

                var random = new Random();

                IndexModel model = new IndexModel
                {
                    UserID = userid,
                    UserPassword = random.Next(10000000, 99999999).ToString()
                };

                if (_GetLoginResult(model))
                {
                    _GetTokenInfo(model);

                    _GetUserMenuContentXSL(model);

                    model.ValidFirstLogin(model.UserID, AuthState.SessionData.UserID);

                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index");
            }
            catch (SecurityTokenExpiredException e)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                jwtFactory.ClearJwtWithCookies(Response.Cookies);
                HttpContext.User = null;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        private void _GenerateJwtToken()
        {
            IJwtFactory jwtFactory = new JwtFactory();
            jwtFactory.GenerateEncodedTokenByCookies(new[]
            {
                new Claim(ClaimTypes.Name, AuthState.SessionData.UserID),
                new Claim(ClaimTypes.NameIdentifier, AuthState.SessionData.SessionID),
                new Claim("UserNM", AuthState.SessionData.UserNM),
                new Claim("UserComID", AuthState.SessionData.UserComID)
            }, Response.Cookies);
        }

        [HttpGet]
        public ActionResult TokenIndex(IndexModel model)
        {
            _GetCultureID(AuthState.CookieData.CultureID);

            if (model.GetValidateIPAddressIsTrust(ClientIPAddress()) == Entity_Base.EnumValidateIPAddressIsTrustResult.Reject)
            {
                return AuthState.RejectiveActionResult;
            }

            if (_GetLoginResult(model))
            {
                AuthState.SessionData.UserToken = model.UserToken;

                _GetUserMenuContentXSL(model);

                if (Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.JwtEnable.ToString()]))
                {
                    _GenerateJwtToken();
                }
                
                if (model.TargetUrl != null)
                {
                    return Redirect(model.TargetUrl);
                }

                return RedirectToAction("Bulletin", "Pub");
            }

            return RedirectToAction("Index", "Home");
        }

        private ActionResult AutoLoginWithSSO(IndexModel model)
        {
            try
            {
                var random = new Random();
                model.UserPassword = random.Next(10000000, 99999999).ToString();

                if (_GetLoginResult(model))
                {
                    _GetTokenInfo(model);

                    _GetUserMenuContentXSL(model);

                    model.ValidFirstLogin(model.UserID, AuthState.SessionData.UserID);

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.JwtEnable.ToString()]))
                    {
                        _GenerateJwtToken();
                    }

                    if (string.IsNullOrWhiteSpace(model.ProxyLoginSystemID) == false)
                    {
                        return RedirectToAction("SSOLogin", "Home", new { systemID = model.ProxyLoginSystemID });
                    }

                    if (model.TargetUrl != null)
                    {
                        return Redirect(model.TargetUrl);
                    }

                    #region - Login Event -
                    if (ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain))
                    {
                        if (model.GetUserLoginEvent(model.UserID) == false)
                        {
                            SetSystemErrorMessage(HomeIndex.SystemMsg_GetUserLoginEvent_Failure);
                        }
                        else if (model.UserLoginEvent != null)
                        {
                            bool isValidLoginEventResult = true;

                            if (model.UserLoginEvent.ValidPath.IsNull() == false)
                            {
                                isValidLoginEventResult = _GetValidLoginEventResult(model.UserLoginEvent.ValidPath.GetValue(), model.UserID);
                            }

                            if (isValidLoginEventResult == false)
                            {
                                if (model.UserLoginEvent.IsOutSourcing.GetValue() == EnumYN.Y.ToString())
                                {
                                    return RedirectToAction("SSOLogin", "Home", new { systemID = model.UserLoginEvent.SysID.GetValue(), targetPath = model.UserLoginEvent.TargetPath.GetValue() });
                                }
                                return Redirect(model.UserLoginEvent.TargetPath.GetValue());
                            }
                        }
                    }
                    #endregion

                    return RedirectToAction("Bulletin", "Pub");
                }
                return RedirectToAction("SSORedirectToAuth", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SSORedirectToAuth", "Home");
            }
        }
    }
}
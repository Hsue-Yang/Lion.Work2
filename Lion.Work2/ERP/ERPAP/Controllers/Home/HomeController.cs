using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using ERPAP.Models.Home;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Home;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;
using Resources;

namespace ERPAP.Controllers
{
    public partial class HomeController : _BaseAPController
    {
        private const string LOGIN_SYSTEMFUN_KEY = "ERPAPHomeIndex";
        private static readonly object LockToken = new object();
       
        private EnumCultureID _GetCultureID(string cultureID)
        {
            EnumCultureID finalCultureID = EnumCultureID.zh_TW;

            if (string.IsNullOrWhiteSpace(cultureID) == false &&
                Enum.IsDefined(typeof(EnumCultureID), cultureID))
            {
                finalCultureID = Utility.GetCultureID(cultureID);
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));

            AuthState.CookieData.SetCultureID(finalCultureID.ToString());

            return finalCultureID;
        }

        private string _GetEnvironmentRemind()
        {
            if (LionTechAppSettings.ServerEnvironment == EnumServerEnvironment.Testing)
            {
                return HomeIndex.Text_LionTestingRemind;
            }
            return string.Empty;
        }

        internal IUserAuthentication _GetAuthenticationOTPResult(IUserAccountInfo accountInfo)
        {
            ExApiResponse<AuthenticationOTPResult> employee = new ExApiResponse<AuthenticationOTPResult>
            {
                Result = EnumUserAuthenticationResult.OtherError,
                Data = new AuthenticationOTPResult
                {
                    authResultCode = EnumAuthenticationOTP.None
                }
            };

            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>();
                headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");

                int retryCount = 0;

                do
                {
                    try
                    {
                        string accessToken = GetExApiToken.GetToken();

                        if (string.IsNullOrWhiteSpace(accessToken) == true)
                        {
                            return employee;
                        }

                        headers[HttpRequestHeader.Authorization] = accessToken;
                    }
                    catch (Exception ex)
                    {
                        FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
                        return employee;
                    }

                    try
                    {
                        string url = Common.GetEnumDesc(EnumExApiURL.AuthWithOTP);

                        string jsonString = Common.GetJsonSerializeObject(new
                        {
                            Stfn = GetConvertUserIDByOTP(accountInfo.UserID),
                            Pswd = accountInfo.UserPassword,
                            ClientIP = ClientIPAddress()
                        });

                        byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);

                        int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);
                        string response = Common.HttpWebRequestGetResponseString(url, apiTimeOut, byteArray, headers);
                        if (string.IsNullOrWhiteSpace(response) == false)
                        {
                            employee = Common.GetJsonDeserializeObject<ExApiResponse<AuthenticationOTPResult>>(response);
                            if (employee?.Data != null)
                            {
                                if (employee.Data.authResultCode != EnumAuthenticationOTP.Success)
                                {
                                    SetSystemAlertMessage(employee.Data.authResultMsg.Replace("\n", "<br/>"));
                                }
                                else
                                {
                                    employee.Result = EnumUserAuthenticationResult.Success;
                                }
                            }
                            else if (employee?.rCode == "0401")
                            {
                                retryCount++;
                                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), $"{url}, response = {response}");
                                continue;
                            }
                            else
                            {
                                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), $"{url}, response = {response}");
                                employee = employee ?? new ExApiResponse<AuthenticationOTPResult>();
                                employee.Data = employee.Data ?? new AuthenticationOTPResult();
                                SetSystemErrorMessage("ExApi response rDesc=" + employee.rDesc);
                            }
                        }
                        break;
                    }
                    catch (WebException webException)
                        when (webException.Response is HttpWebResponse &&
                              ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        retryCount++;
                    }
                } while (retryCount < 10);
            }
            catch (Exception ex)
            {
                employee = new ExApiResponse<AuthenticationOTPResult>
                {
                    Data = new AuthenticationOTPResult
                    {
                        authResultCode = EnumAuthenticationOTP.None
                    }
                };

                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
            }

            return employee;
        }

        private bool _GetLoginResult(IUserAccountInfo accountInfo)
        {
            bool isValid = true;

            HomeModel homeModel = new HomeModel();

            if (homeModel.GetValidateUserAuthorization(accountInfo.UserID, AuthState.SessionData.SessionID, ClientIPAddress()) &&
                homeModel.GetUserMain(accountInfo.UserID) &&
                homeModel.GetUserSystemList(accountInfo.UserID) &&
                homeModel.GetUserSystemRoleList(accountInfo.UserID) &&
                homeModel.GetUserWorkDiffHour(accountInfo.UserID))
            {
                StringBuilder userSystemIDStringBuilder = new StringBuilder();
                foreach (EntityHome.UserSystem userSystem in homeModel.EntityUserSystemList)
                {
                    userSystemIDStringBuilder.Append(string.Concat(new object[] { userSystem.SystemID.GetValue(), "," }));
                }
                string userSystemID = userSystemIDStringBuilder.ToString();
                userSystemID = userSystemID.Substring(0, userSystemID.Length - 1);

                StringBuilder userRoleIDStringBuilder = new StringBuilder();
                foreach (EntityHome.UserSystemRole userSystemRole in homeModel.EntityUserSystemRoleList)
                {
                    userRoleIDStringBuilder.Append(string.Concat(new object[] { userSystemRole.SystemID.GetValue(), userSystemRole.RoleID.GetValue(), "," }));
                }
                string userRoleID = userRoleIDStringBuilder.ToString();
                userRoleID = userRoleID.Substring(0, userRoleID.Length - 1);

                AuthState.SessionData.SetUser(
                    accountInfo.UserID, homeModel.EntityUserMain.UserNM.GetValue(),
                    homeModel.EntityUserMain.UserComID.GetValue(),
                    homeModel.EntityUserMain.UserUnitID.GetValue(), homeModel.EntityUserMain.UserUnitNM.GetValue(),
                    homeModel.UserWorkDiffHour,
                    userSystemID, userRoleID, homeModel.EntityUserMain.UserIdentity.GetValue());
            }
            else
            {
                isValid = false;
                SetSystemErrorMessage(HomeIndex.SystemMsg_UserInformationError);
            }

            return isValid;
        }

        private void _GetTokenInfo(IUserAccountInfo accountInfo)
        {
            HomeModel homeModel = new HomeModel();

            string systemKey = string.Empty;
            string paraT = string.Empty;
            string paraK = string.Empty;
            string paraC = string.Empty;
            string paraU = string.Empty;
            string paraP = string.Empty;

            try
            {
                if (ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain) &&
                    homeModel.GetSystemMain(EnumSystemID.ERPAP.ToString()))
                {
                    systemKey = homeModel.EntitySystemMain.SysKey.GetValue();
                    paraK = homeModel.EntitySystemMain.ENSysID.GetValue();
                    paraC = Token.Encrypt(ClientIPAddress(), systemKey);
                    paraU = Token.Encrypt(accountInfo.UserID, systemKey);

                    int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

                    string generatorURL = string.Format(
                        ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITKNAPTokenServiceGeneratorURL.ToString()],
                        Common.GetEnumDesc(EnumAPISystemID.TKNAP), paraK, paraC, paraU);

                    paraT = Common.HttpWebRequestGetResponseString(generatorURL, apiTimeOut);
                    if (string.IsNullOrWhiteSpace(paraT) == false &&
                        paraT.Length == 64)
                    {
                        paraP = Token.Encrypt(accountInfo.UserPassword, paraT);

                        string loginURL = string.Format(
                            ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITKNAPTokenServiceLoginURL.ToString()],
                            Common.GetEnumDesc(EnumAPISystemID.TKNAP), paraT, paraK, paraC, paraU, paraP);

                        string isLogin = Common.HttpWebRequestGetResponseString(loginURL, apiTimeOut);
                        if (string.IsNullOrWhiteSpace(isLogin) == false &&
                            isLogin == bool.TrueString)
                        {
                            AuthState.SessionData.UserToken = paraT;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _TokenExceptionLogWrite(accountInfo.UserID, paraT, ex);
            }
        }

        private void _GetUserMenuContentXSL(IUserAccountInfo accountInfo)
        {
            AuthState.SessionData.UserMenuXSLString = Common.FileReadStream(GetAppDataFilePath(EnumAppDataFile.UserMenuContentXSL));

            string cultureID = string.Empty;
            if (CultureID != EnumCultureID.zh_TW)
            {
                cultureID = Common.GetEnumDesc(CultureID);
            }

            string filePathUserMenu =
                Path.Combine(new[]
                {
                    GetFilePathFolderPath(EnumFilePathFolder.UserMenu), accountInfo.UserID,
                    string.Concat(new object[]
                    {
                        EnumUserMenu.UserMenu.ToString(),
                        ".", accountInfo.UserID,
                        (string.IsNullOrWhiteSpace(cultureID) ? string.Empty : "."), cultureID
                        ,
                        ".", EnumFileExtension.xml.ToString()
                    }
                        )
                });

            AuthState.SessionData.FilePathUserMenu = filePathUserMenu;
            AuthState.SessionData.UserMenuID = EnumUserMenuID.MenuID1;
            SetUserMenu(EnumUserMenuID.MenuID1, CultureID);
        }

        private static string GetConvertUserIDByOTP(string userid)
        {
            string result = userid;
            if (string.IsNullOrWhiteSpace(userid) == false &&
                userid.Length == 4)
            {
                if (userid.Substring(0, 1).ToUpper() == "Z")
                {
                    result = "ZZ" + userid;
                }
                else
                {
                    result = "00" + userid;
                }
            }
            return result;
        }

        private bool _GetValidLoginEventResult(string validPath, string userID)
        {
            try
            {
                int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);
                string url = validPath.Split('?')[0];
                string queryString = new Uri(validPath).Query;
                var queryDictionary = HttpUtility.ParseQueryString(queryString);
                queryDictionary.Add("ClientSysID", EnumSystemID.ERPAP.ToString());
                queryDictionary.Add("ClientUserID", userID);

                validPath = string.Concat(url, $"?{queryDictionary}");

                return Common.HttpWebRequestGetResponseString(validPath, apiTimeOut) == bool.TrueString;
            }
            catch (Exception ex)
            {
                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
            }
            return true;
        }

        private void _TokenExceptionLogWrite(string userID, string tokenNo, Exception ex)
        {
            MethodBase methodBase1 = Common.GetMethodBase(1);

            string logMessage = string.Concat(new string[]
            {
                EnumRootPathFile.Exception.ToString(), ": ", methodBase1.DeclaringType.FullName, ".", methodBase1.Name,
                " ; UserID: ", userID,
                " ; TokenNo: ", tokenNo
            });

            FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), logMessage);
            FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
        }
    }
}
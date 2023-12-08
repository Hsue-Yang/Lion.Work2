using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using LionTech.Entity.ERP;

namespace LionTech.Utility.ERP
{
    public enum EnumSessionKey
    {
        WorkFlowNo,
        UserToken, UserID, UserNM, UserComID, UserUnitID, UserUnitNM, UserWorkDiffHour, UserSystemIDs, UserRoleIDs,
        UserMenuXSLString, UserMenu, UserMenuID, UserIdentity,
        FilePathUserMenu
    }

    public enum EnumCookieName
    {
        CultureID, PageSize
    }

    public class AuthState
    {
        EnumSystemID _systemID;
        public EnumSystemID SystemID
        {
            get { return _systemID; }
        }

        string _controllerName;
        public string ControllerName
        {
            get { return _controllerName; }
        }

        string _actionName;
        public string ActionName
        {
            get { return _actionName; }
        }

        public void SetActionName(string actionName)
        {
            _actionName = actionName;
        }

        public string SystemFunKey
        {
            get { return _systemID + _controllerName + _actionName; }
        }

        SessionData _sessionData;
        public SessionData SessionData
        {
            get { return _sessionData; }
        }

        CookieData _cookieData;
        public CookieData CookieData
        {
            get { return _cookieData; }
        }

        bool _isAuthorized = true;
        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { _isAuthorized = value; }
        }

        ActionResult _unAuthorizedActionResult;
        public ActionResult UnAuthorizedActionResult
        {
            get { return _unAuthorizedActionResult; }
            set { _unAuthorizedActionResult = value; }
        }

        bool _isRejective = false;
        public bool IsRejective
        {
            get { return _isRejective; }
            set { _isRejective = value; }
        }

        ActionResult _rejectiveActionResult;
        public ActionResult RejectiveActionResult
        {
            get { return _rejectiveActionResult; }
            set { _rejectiveActionResult = value; }
        }

        public bool IsLogined
        {
            get
            {
                if (this.SessionData.UserID != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        bool _isPWDExpired = false;
        public bool IsPWDExpired
        {
            get { return _isPWDExpired; }
            set { _isPWDExpired = value; }
        }

        ActionResult _pwdExpiredActionResult;
        public ActionResult PWDExpiredActionResult
        {
            get { return _pwdExpiredActionResult; }
            set { _pwdExpiredActionResult = value; }
        }

        public AuthState(
            EnumSystemID systemID, string controllerName, string actionName,
            HttpSessionStateBase httpSessionStateBase,
            HttpCookieCollection requestHttpCookieCollection, HttpCookieCollection responseHttpCookieCollection)
        {
            _systemID = systemID;
            _controllerName = controllerName;
            _actionName = actionName;

            _sessionData = new SessionData(httpSessionStateBase);
            _cookieData = new CookieData(requestHttpCookieCollection, responseHttpCookieCollection);
        }
    }

    public class SessionData
    {
        HttpSessionStateBase _httpSessionStateBase;

        public string APSession(string sessionKey)
        {
            return _httpSessionStateBase.Get(sessionKey);
        }

        public string SessionID
        {
            get { return _httpSessionStateBase.SessionID; }
        }
        
        public string UserToken
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserToken); }
            set { _httpSessionStateBase.Set<string>(EnumSessionKey.UserToken, value); }
        }

        public string UserID
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserID); }
        }

        public string UserNM
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserNM); }
        }

        public string UserComID
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserComID); }
        }

        public string UserUnitID
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserUnitID); }
        }

        public string UserUnitNM
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserUnitNM); }
        }

        public DateTime UserWorkDiffDatetime
        {
            get
            {
                var hour = _httpSessionStateBase.Get<int>(EnumSessionKey.UserWorkDiffHour);
                return DateTime.Now.AddHours(hour);
            }
        }

        public List<EnumSystemID> UserSystemIDs
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_httpSessionStateBase.Get<string>(EnumSessionKey.UserSystemIDs)) == false)
                {
                    List<EnumSystemID> userSystemIDList = new List<EnumSystemID>();
                    foreach (string systemID in _httpSessionStateBase.Get<string>(EnumSessionKey.UserSystemIDs).Split(','))
                    {
                        userSystemIDList.Add(Entity.ERP.Utility.GetSystemID(systemID));
                    }
                    return userSystemIDList;
                }
                return null;
            }
        }

        public List<string> UserRoleIDs
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_httpSessionStateBase.Get<string>(EnumSessionKey.UserRoleIDs)) == false)
                {
                    List<string> userRoleIDList = new List<string>();
                    foreach (string roleID in _httpSessionStateBase.Get<string>(EnumSessionKey.UserRoleIDs).Split(','))
                    {
                        userRoleIDList.Add(roleID);
                    }
                    return userRoleIDList;
                }
                return null;
            }
        }

        public string UserDescription
        {
            get
            {
                return string.Concat(new string[]
                    {
                        _httpSessionStateBase.Get<string>(EnumSessionKey.UserUnitNM) +
                        _httpSessionStateBase.Get<string>(EnumSessionKey.UserNM)
                    });
            }
        }

        public string UserMenuXSLString
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserMenuXSLString); }
            set { _httpSessionStateBase.Set<string>(EnumSessionKey.UserMenuXSLString, value); }
        }

        public string UserMenu
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserMenu); }
            set { _httpSessionStateBase.Set<string>(EnumSessionKey.UserMenu, value); }
        }

        public string UserIdentity
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.UserIdentity); }
            set { _httpSessionStateBase.Set<string>(EnumSessionKey.UserIdentity, value); }
        }

        public EnumUserMenuID UserMenuID
        {
            get
            {
                EnumUserMenuID userMenuID = EnumUserMenuID.MenuID1;

                if (string.IsNullOrWhiteSpace(_httpSessionStateBase.Get<string>(EnumSessionKey.UserMenuID)) == false)
                {
                    if (Enum.IsDefined(typeof(EnumUserMenuID), _httpSessionStateBase.Get<string>(EnumSessionKey.UserMenuID)))
                    {
                        userMenuID = (EnumUserMenuID)Enum.Parse(typeof(EnumUserMenuID), _httpSessionStateBase.Get<string>(EnumSessionKey.UserMenuID));
                    }
                }

                return userMenuID;
            }
            set {
                _httpSessionStateBase.Set<string>(EnumSessionKey.UserMenuID, value.ToString());
            }
        }

        public string FilePathUserMenu
        {
            get { return _httpSessionStateBase.Get<string>(EnumSessionKey.FilePathUserMenu); }
            set { _httpSessionStateBase.Set<string>(EnumSessionKey.FilePathUserMenu, value); }
        }

        public SessionData(HttpSessionStateBase httpSessionStateBase)
        {
            _httpSessionStateBase = httpSessionStateBase;
        }

        public void Clear()
        {
            _httpSessionStateBase.RemoveAll();
            _httpSessionStateBase.Abandon();
        }

        public void SetAPSession(string sessionKey, string value)
        {
            _httpSessionStateBase.Set(sessionKey, value);
        }
        
        public void SetUser(string userID, string userNM, string userComID, string userUnitID, string userUnitNM, int userWorkDiffHour, string userSystemIDs, string userRoleIDs, string userIdentity)
        {
            _httpSessionStateBase.RemoveAll();

            _httpSessionStateBase.Set<string>(EnumSessionKey.UserID, userID);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserNM, userNM);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserComID, userComID);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserUnitID, userUnitID);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserUnitNM, userUnitNM);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserWorkDiffHour, userWorkDiffHour);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserSystemIDs, userSystemIDs);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserRoleIDs, userRoleIDs);
            _httpSessionStateBase.Set<string>(EnumSessionKey.UserIdentity, userIdentity);
        }
    }

    public static class SessionExtensions
    {
        public static T Get<T>(this HttpSessionStateBase httpSessionStateBase, EnumSessionKey sessionKey)
        {
            return (T)httpSessionStateBase[sessionKey.ToString()];
        }

        public static void Set<T>(this HttpSessionStateBase httpSessionStateBase, EnumSessionKey sessionKey, object value)
        {
            httpSessionStateBase[sessionKey.ToString()] = value;
        }

        public static string Get(this HttpSessionStateBase httpSessionStateBase, string sessionKey)
        {
            return httpSessionStateBase[sessionKey] != null ? httpSessionStateBase[sessionKey].ToString() : null;
        }

        public static void Set(this HttpSessionStateBase httpSessionStateBase, string sessionKey, string value)
        {
            httpSessionStateBase[sessionKey] = value;
        }
    }

    public class CookieData
    {
        HttpCookieCollection _requestHttpCookieCollection;
        HttpCookieCollection _responseHttpCookieCollection;

        public string CultureID
        {
            get
            {
                HttpCookie httpCookie = _requestHttpCookieCollection[EnumCookieName.CultureID.ToString()];
                if (httpCookie != null)
                {
                    return httpCookie.Value;
                }
                return null;
            }
        }

        public string PageSize
        {
            get
            {
                HttpCookie httpCookie = _requestHttpCookieCollection[EnumCookieName.PageSize.ToString()];
                if (httpCookie != null)
                {
                    return httpCookie.Value;
                }
                return null;
            }
        }

        public bool HasSystemFunKey(string key)
        {
            HttpCookie httpCookie = _requestHttpCookieCollection[key];
            if (httpCookie != null)
            {
                return true;
            }
            return false;
        }

        public string CookieKeys(string key, string paraName)
        {
            HttpCookie httpCookie = _requestHttpCookieCollection[key];
            if (httpCookie != null)
            {
                return HttpUtility.UrlDecode(httpCookie.Values[paraName]);
            }
            return null;
        }

        public CookieData(HttpCookieCollection requestHttpCookieCollection, HttpCookieCollection responseHttpCookieCollection)
        {
            _requestHttpCookieCollection = requestHttpCookieCollection;
            _responseHttpCookieCollection = responseHttpCookieCollection;
        }

        public void SetCultureID(string cultureID)
        {
            HttpCookie cookie = new HttpCookie(EnumCookieName.CultureID.ToString());
            cookie.Value = cultureID;
            cookie.Expires = DateTime.Today.AddDays(10);
            cookie.Domain = Common.GetEnumDesc(EnumSystemID.Domain);
            cookie.Path = "/";
            _responseHttpCookieCollection.Add(cookie);
        }

        public void SetPageSize(int pageSize)
        {
            HttpCookie cookie = new HttpCookie(EnumCookieName.PageSize.ToString());
            cookie.Value = pageSize.ToString();
            cookie.Expires = DateTime.Today.AddDays(10);
            cookie.Domain = Common.GetEnumDesc(EnumSystemID.Domain);
            cookie.Path = "/";
            _responseHttpCookieCollection.Add(cookie);
        }

        public void SetCookieKeys(string cookieNM, Dictionary<string, string> dict, int expireDays = 10, bool isShared = false)
        {
            SetCookieKeys(cookieNM, dict, DateTime.Today.AddDays(expireDays), isShared);
        }

        public void SetCookieKeys(string cookieNM, Dictionary<string, string> dict, DateTime expireDt, bool isShared = false)
        {
            HttpCookie cookie;
            if (isShared)
            {
                cookie = _requestHttpCookieCollection[cookieNM];
                if (cookie == null)
                {
                    cookie = new HttpCookie(cookieNM);
                }
            }
            else
            {
                cookie = new HttpCookie(cookieNM);
            }

            foreach (var para in dict)
            {
                if (string.IsNullOrWhiteSpace(para.Value))
                {
                    cookie.Values[para.Key] = para.Value;
                }
                else
                {
                    cookie.Values[para.Key] = HttpUtility.UrlEncode(para.Value);
                }
            }
            cookie.Expires = expireDt;
            cookie.Domain = Common.GetEnumDesc(EnumSystemID.Domain);
            cookie.Path = "/";
            _responseHttpCookieCollection.Add(cookie);
        }
    }
}
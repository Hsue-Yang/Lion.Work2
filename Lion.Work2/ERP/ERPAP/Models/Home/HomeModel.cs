using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Home;
using LionTech.Utility;

namespace ERPAP.Models.Home
{
    public interface IUserAccountInfo
    {
        string CultureID { get; set; }
        string UserID { get; set; }
        string UserPassword { get; set; }
        string UserLocation { get; set; }
        string LocationDesc { get; set; }
        string IdNo { get; set; }
        string Birthday { get; set; }
        string LoginType { get; set; }
    }

    public class HomeModel : _BaseAPModel
    {
        EntityHome.SystemMain _entitySystemMain;
        public EntityHome.SystemMain EntitySystemMain { get { return _entitySystemMain; } }

        public bool GetSystemMain(string sysID)
        {
            try
            {
                EntityHome.SystemMainPara systemMainPara = new EntityHome.SystemMainPara
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySystemMain = new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .SelectSystemMain(systemMainPara);

                if (_entitySystemMain != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public List<EntityHome.UserSystem> EntityUserSystemList { get; private set; }

        public bool GetUserSystemList(string userID)
        {
            try
            {
                EntityHome.UserSystemPara userSystemPara = new EntityHome.UserSystemPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                EntityUserSystemList = new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserSystemList(userSystemPara);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public List<EntityHome.UserSystemRole> EntityUserSystemRoleList { get; private set; }

        public bool GetUserSystemRoleList(string userID)
        {
            try
            {
                EntityHome.UserSystemRolePara userSystemRolePara = new EntityHome.UserSystemRolePara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                EntityUserSystemRoleList = new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserSystemRoleList(userSystemRolePara);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public int UserWorkDiffHour { get; private set; }

        public bool GetUserWorkDiffHour(string userID)
        {
            try
            {
                EntityHome.UserWorkDiffHourPara userWorkDiffHourPara = new EntityHome.UserWorkDiffHourPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                UserWorkDiffHour = new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserWorkDiffHour(userWorkDiffHourPara).GetValue();

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public bool GetValidateUserAuthorization(string userID, string sessionID, string ip)
        {
            try
            {
                EntityHome.UserAuthorizationPara userAuthorizationPara = new EntityHome.UserAuthorizationPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    SystemID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                    SessionID = new DBChar((string.IsNullOrWhiteSpace(sessionID) ? null : sessionID)),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ip) ? null : ip))
                };

                if (new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .ValidateUserAuthorization(userAuthorizationPara) == EntityHome.EnumValidateUserAuthorizationResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public EntityHome.UserMain EntityUserMain { get; private set; }

        public bool GetUserMain(string userID)
        {
            try
            {
                EntityHome.UserMainPara userMainPara = new EntityHome.UserMainPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                EntityUserMain = new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserMain(userMainPara);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public void ValidFirstLogin(string userID, string updUserID)
        {
            try
            {
                EntityHome.UserMainPara userMainPara = new EntityHome.UserMainPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    LastLoginDate = new DBChar(Common.GetDateString()),
                    UpdUserID = new DBVarChar(updUserID),
                };

                new EntityHome(ConnectionStringSERP, ProviderNameSERP)
                    .EditUserMain(userMainPara);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        #region - 取得使用者登入事件 -
        public EntityHome.UserLoginEvent UserLoginEvent { get; private set; }

        /// <summary>
        /// 取得使用者登入事件
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool GetUserLoginEvent(string userID)
        {
            try
            {
                EntityHome.UserLoginEventPara para = new EntityHome.UserLoginEventPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                };

                UserLoginEvent = new EntityHome(ConnectionStringSERP, ProviderNameSERP).SelectUserLoginEvent(para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion
    }
}
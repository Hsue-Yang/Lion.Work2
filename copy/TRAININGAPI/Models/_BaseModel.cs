using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace TRAININGAPI
{
    public enum EnumConnectionStringsKey
    {
        LionGroupSERP, LionGroupTRAINING
    }
}

namespace TRAININGAPI.Models
{
    public partial class _BaseModel
    {
        public string ServiceUserID
        {
            get { return "APIService.TRAINING"; }
        }


        protected string GetRootPathFilePath(EnumRootPathFile enumRootPathFile)
        {
            return Path.Combine(
                new string[]
                    {
                        ConfigurationManager.AppSettings[EnumAppSettingKey.RootPath.ToString()],
                        string.Format(Common.GetEnumDesc(enumRootPathFile), Common.GetDateString())
                    });
        }

        string _connectionStringSERP;
        protected string ConnectionStringSERP { get { return _connectionStringSERP; } set { _connectionStringSERP = value; } }

        string _providerNameSERP;
        protected string ProviderNameSERP { get { return _providerNameSERP; } set { _providerNameSERP = value; } }

        string _connectionStringTRAINING;
        protected string ConnectionStringTRAINING { get { return _connectionStringTRAINING; } set { _connectionStringTRAINING = value; } }

        string _providerNameTRAINING;
        protected string ProviderNameTRAINING { get { return _providerNameTRAINING; } set { _providerNameTRAINING = value; } }

        public _BaseModel()
        {
            try
            {
                _connectionStringSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ConnectionString;
                _providerNameSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ProviderName;

                _connectionStringTRAINING = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupTRAINING.ToString()].ConnectionString;
                _providerNameTRAINING = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupTRAINING.ToString()].ProviderName;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }

        protected void OnException(Exception exception)
        {
            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), exception);
        }

        public bool GetValidateSystemAPIRole(EnumSystemID systemID, string controllerID, string actionName, EnumYN isOutside, string userID)
        {
            try
            {
                Entity_Base.SystemAPIRolePara systemAPIRolePara = new Entity_Base.SystemAPIRolePara()
                {
                    SystemID = new DBVarChar(systemID.ToString()),
                    ControllerID = new DBVarChar(controllerID),
                    ActionName = new DBVarChar(actionName),
                    UserID = new DBVarChar(userID)
                };

                if (new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ValidateSystemAPIRole(systemAPIRolePara) == Entity_Base.EnumValidateSystemAPIRoleResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return false;
        }

        public bool GetValidateSystemAPIFun(string clientSysID, EnumSystemID systemID, string controllerID, string actionName, string ip, EnumYN isOutside)
        {
            try
            {
                Entity_Base.SystemAPIFunPara systemAPIFunPara = new Entity_Base.SystemAPIFunPara()
                {
                    ClientSysID = new DBVarChar(clientSysID),
                    SystemID = new DBVarChar(systemID.ToString()),
                    ControllerID = new DBVarChar(controllerID),
                    ActionName = new DBVarChar(actionName),
                    IPAddress = new DBVarChar(ip),
                    IsOutside = new DBChar(isOutside.ToString())
                };

                if (new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ValidateSystemAPIFun(systemAPIFunPara) == Entity_Base.EnumValidateSystemAPIFunResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return false;
        }

        public string ExecAPIClientBegin(string clientSysID, string reqURL, string ipAddress)
        {
            try
            {
                MethodBase methodBase1 = Common.GetMethodBase(1);

                string apiControllerID = methodBase1.DeclaringType.Name.Replace("Controller", string.Empty);
                string apiActionName = methodBase1.Name;

                DateTime dtBegin = DateTime.Now;
                string apiDate = Common.GetDateString(dtBegin);
                string apiTime = Common.GetTimeString(dtBegin);

                string returnValue = null;
                int execTimes = 0;
                Exception catchException = null;

                while (string.IsNullOrWhiteSpace(returnValue) && execTimes < 10)
                {
                    execTimes += 1;
                    try
                    {
                        Entity_Base.APIClientPara apiClientPara = new Entity_Base.APIClientPara()
                        {
                            SysID = new DBVarChar(EnumSystemID.TRAININGAP.ToString()),
                            APIControllerID = new DBVarChar(apiControllerID),
                            APIActionName = new DBVarChar(apiActionName),
                            ClientSysID = new DBVarChar(clientSysID),
                            ClientUserID = new DBVarChar(null),
                            APIDate = new DBChar(apiDate),
                            APITime = new DBChar(apiTime),
                            DTBegin = new DBDateTime(dtBegin),
                            ReqURL = new DBNVarChar((string.IsNullOrWhiteSpace(reqURL) ? null : reqURL)),
                            IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress)),
                            UpdUserID = new DBVarChar(this.ServiceUserID)
                        };

                        returnValue = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                            .ExecAPIClientBegin(apiClientPara);
                    }
                    catch (Exception ex)
                    {
                        catchException = ex;
                    }
                }

                if (string.IsNullOrWhiteSpace(returnValue) && catchException != null)
                {
                    throw catchException;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return null;
        }

        public string ExecAPIClientBegin(string clientSysID, string clientUserID, string reqURL, string ipAddress)
        {
            try
            {
                MethodBase methodBase1 = Common.GetMethodBase(1);

                string apiControllerID = methodBase1.DeclaringType.Name.Replace("Controller", string.Empty);
                string apiActionName = methodBase1.Name;

                DateTime dtBegin = DateTime.Now;
                string apiDate = Common.GetDateString(dtBegin);
                string apiTime = Common.GetTimeString(dtBegin);

                string returnValue = null;
                int execTimes = 0;
                Exception catchException = null;

                while (string.IsNullOrWhiteSpace(returnValue) && execTimes < 10)
                {
                    execTimes += 1;
                    try
                    {
                        Entity_Base.APIClientPara apiClientPara = new Entity_Base.APIClientPara()
                        {
                            SysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                            APIControllerID = new DBVarChar(apiControllerID),
                            APIActionName = new DBVarChar(apiActionName),
                            ClientSysID = new DBVarChar(clientSysID),
                            ClientUserID = new DBVarChar(clientUserID),
                            APIDate = new DBChar(apiDate),
                            APITime = new DBChar(apiTime),
                            DTBegin = new DBDateTime(dtBegin),
                            ReqURL = new DBNVarChar((string.IsNullOrWhiteSpace(reqURL) ? null : reqURL)),
                            IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress)),
                            UpdUserID = new DBVarChar(this.ServiceUserID)
                        };

                        returnValue = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                            .ExecAPIClientBegin(apiClientPara);
                    }
                    catch (Exception ex)
                    {
                        catchException = ex;
                    }
                }

                if (string.IsNullOrWhiteSpace(returnValue) && catchException != null)
                {
                    throw catchException;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return null;
        }

        public void ExecAPIClientEnd(string apiNo, string reqReturn)
        {
            try
            {
                DateTime dtEnd = DateTime.Now;

                Entity_Base.APIClientPara apiClientPara = new Entity_Base.APIClientPara()
                {
                    APINo = new DBChar(apiNo),
                    DTEnd = new DBDateTime(dtEnd),
                    ReqReturn = new DBNVarChar(reqReturn),
                    UpdUserID = new DBVarChar(this.ServiceUserID)
                };

                new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ExecAPIClientEnd(apiClientPara);
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }
    }
}
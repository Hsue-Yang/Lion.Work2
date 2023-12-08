using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Http;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;

namespace ERPAPI
{
    public enum EnumConnectionStringsKey
    {
        LionGroupSERP, LionGroupMSERP, LionGroupB2P,
        LionGroupERP
    }
}

namespace ERPAPI.Models
{
    public partial class _BaseModel
    {
        protected const string ServiceUserID = "APIService.ERP";
        
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

        string _connectionStringMSERP;
        protected string ConnectionStringMSERP { get { return _connectionStringMSERP; } set { _connectionStringMSERP = value; } }

        string _providerNameMSERP;
        protected string ProviderNameMSERP { get { return _providerNameMSERP; } set { _providerNameMSERP = value; } }

        string _connectionStringB2P;
        protected string ConnectionStringB2P { get { return _connectionStringB2P; } set { _connectionStringB2P = value; } }

        string _providerNameB2P;
        protected string ProviderNameB2P { get { return _providerNameB2P; } set { _providerNameB2P = value; } }

        string _connectionStringERP;
        protected string ConnectionStringERP { get { return _connectionStringERP; } set { _connectionStringERP = value; } }

        string _providerNameERP;
        protected string ProviderNameERP { get { return _providerNameERP; } set { _providerNameERP = value; } }

        public _BaseModel()
        {
            try
            {
                _connectionStringSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ConnectionString;
                _providerNameSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ProviderName;

                _connectionStringMSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupMSERP.ToString()].ConnectionString;
                _providerNameMSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupMSERP.ToString()].ProviderName;

                _connectionStringB2P = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupB2P.ToString()].ConnectionString;
                _providerNameB2P = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupB2P.ToString()].ProviderName;

                _connectionStringERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupERP.ToString()].ConnectionString;
                _providerNameERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupERP.ToString()].ProviderName;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }

        internal virtual void OnException(Exception exception)
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

        private Mongo_Base.APIClientPara apiClientPara;

        internal void ExecLogAPIClientBegin(string reqURL, string apiPara, string ipAddress)
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

                apiClientPara = new Mongo_Base.APIClientPara
                {
                    SysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                    APIControllerID = new DBVarChar(apiControllerID),
                    APIActionName = new DBVarChar(apiActionName),
                    APIDate = new DBChar(apiDate),
                    APITime = new DBChar(apiTime),
                    DTBegin = new DBDateTime(dtBegin),
                    ReqURL = new DBNVarChar((string.IsNullOrWhiteSpace(reqURL) ? null : reqURL)),
                    APIPara = new DBNVarChar(apiPara),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress)),
                    UpdUserID = new DBVarChar(ServiceUserID)
                };

                new Mongo_Base(this.ConnectionStringMSERP, this.ProviderNameMSERP).Insert(new MongoEntity.MongoCommand("LOG_API_CLIENT"), apiClientPara);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        internal void ExecLogAPIClientEnd(string reqReturn)
        {
            try
            {
                MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_API_CLIENT");
                DateTime dtEnd = DateTime.Now;

                command.AddUpdateSet("DT_END", new DBDateTime(dtEnd));
                command.AddUpdateSet("REQ_RETURN", new DBNVarChar(reqReturn));

                new Mongo_Base(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                    .Update(command, apiClientPara);
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }
        //
        // internal virtual string ExecAPIClientBegin(string clientSysID, string reqURL, string ipAddress)
        // {
        //     try
        //     {
        //         MethodBase methodBase1 = Common.GetMethodBase(1);
        //
        //         string apiControllerID = methodBase1.DeclaringType.Name.Replace("Controller", string.Empty);
        //         string apiActionName = methodBase1.Name;
        //         ActionNameAttribute actionNameAttr = methodBase1.GetCustomAttribute(typeof(ActionNameAttribute)) as ActionNameAttribute;
        //         if (actionNameAttr != null)
        //         {
        //             apiActionName = actionNameAttr.Name;
        //         }
        //
        //         DateTime dtBegin = DateTime.Now;
        //         string apiDate = Common.GetDateString(dtBegin);
        //         string apiTime = Common.GetTimeString(dtBegin);
        //
        //         string returnValue = null;
        //         int execTimes = 0;
        //         Exception catchException = null;
        //
        //         while (string.IsNullOrWhiteSpace(returnValue) && execTimes < 10)
        //         {
        //             execTimes += 1;
        //             try
        //             {
        //                 Entity_Base.APIClientPara apiClientPara = new Entity_Base.APIClientPara()
        //                 {
        //                     SysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
        //                     APIControllerID = new DBVarChar(apiControllerID),
        //                     APIActionName = new DBVarChar(apiActionName),
        //                     ClientSysID = new DBVarChar(clientSysID),
        //                     ClientUserID = new DBVarChar(null),
        //                     APIDate = new DBChar(apiDate),
        //                     APITime = new DBChar(apiTime),
        //                     DTBegin = new DBDateTime(dtBegin),
        //                     ReqURL = new DBNVarChar((string.IsNullOrWhiteSpace(reqURL) ? null : reqURL)),
        //                     IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress)),
        //                     UpdUserID = new DBVarChar(ServiceUserID)
        //                 };
        //
        //                 returnValue = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
        //                     .ExecAPIClientBegin(apiClientPara);
        //             }
        //             catch (Exception ex)
        //             {
        //                 catchException = ex;
        //             }
        //         }
        //
        //         if (string.IsNullOrWhiteSpace(returnValue) && catchException != null)
        //         {
        //             throw catchException;
        //         }
        //
        //         return returnValue;
        //     }
        //     catch (Exception ex)
        //     {
        //         FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception),
        //             $"{nameof(clientSysID)} {clientSysID}, " +
        //             $"{nameof(reqURL)} {reqURL}, " +
        //             $"{nameof(ipAddress)} {ipAddress}");
        //         this.OnException(ex);
        //     }
        //     return null;
        // }
        //
        // internal virtual string ExecAPIClientBegin(string clientSysID, string clientUserID, string reqURL, string ipAddress)
        // {
        //     int methodIndex = 1;
        //     MethodBase methodBase1;
        //     string apiControllerID = null;
        //     string apiActionName = null;
        //
        //     try
        //     {
        //         do
        //         {
        //             methodBase1 = Common.GetMethodBase(methodIndex);
        //             apiControllerID = methodBase1.DeclaringType.Name.Replace("Controller", string.Empty);
        //             apiActionName = methodBase1.Name;
        //             ActionNameAttribute actionNameAttr = methodBase1.GetCustomAttribute(typeof(ActionNameAttribute)) as ActionNameAttribute;
        //             if (actionNameAttr != null)
        //             {
        //                 apiActionName = actionNameAttr.Name;
        //             }
        //             methodIndex++;
        //         } while (methodBase1.DeclaringType.BaseType.Name != "_BaseAPController");
        //         
        //         DateTime dtBegin = DateTime.Now;
        //         string apiDate = Common.GetDateString(dtBegin);
        //         string apiTime = Common.GetTimeString(dtBegin);
        //
        //         string returnValue = null;
        //         int execTimes = 0;
        //         Exception catchException = null;
        //
        //         while (string.IsNullOrWhiteSpace(returnValue) && execTimes < 10)
        //         {
        //             execTimes += 1;
        //             try
        //             {
        //                 Entity_Base.APIClientPara apiClientPara = new Entity_Base.APIClientPara()
        //                 {
        //                     SysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
        //                     APIControllerID = new DBVarChar(apiControllerID),
        //                     APIActionName = new DBVarChar(apiActionName),
        //                     ClientSysID = new DBVarChar(clientSysID),
        //                     ClientUserID = new DBVarChar(clientUserID),
        //                     APIDate = new DBChar(apiDate),
        //                     APITime = new DBChar(apiTime),
        //                     DTBegin = new DBDateTime(dtBegin),
        //                     ReqURL = new DBNVarChar((string.IsNullOrWhiteSpace(reqURL) ? null : reqURL)),
        //                     IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress)),
        //                     UpdUserID = new DBVarChar(ServiceUserID)
        //                 };
        //
        //                 returnValue = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
        //                     .ExecAPIClientBegin(apiClientPara);
        //             }
        //             catch (Exception ex)
        //             {
        //                 catchException = ex;
        //             }
        //         }
        //
        //         if (string.IsNullOrWhiteSpace(returnValue) && catchException != null)
        //         {
        //             throw catchException;
        //         }
        //
        //         return returnValue;
        //     }
        //     catch (Exception ex)
        //     {
        //         FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception),
        //             $"{nameof(methodIndex)} {methodIndex}, " +
        //             $"{nameof(apiControllerID)} {apiControllerID?? string.Empty}, " +
        //             $"{nameof(apiActionName)} {apiActionName}, " +
        //             $"{nameof(clientSysID)} {clientSysID}, " +
        //             $"{nameof(clientUserID)} {clientUserID??string.Empty}, " +
        //             $"{nameof(reqURL)} {reqURL}, " +
        //             $"{nameof(ipAddress)} {ipAddress}");
        //         this.OnException(ex);
        //     }
        //     return null;
        // }
        //
        // internal virtual void ExecAPIClientEnd(string apiNo, string reqReturn)
        // {
        //     try
        //     {
        //         DateTime dtEnd = DateTime.Now;
        //
        //         Entity_Base.APIClientPara apiClientPara = new Entity_Base.APIClientPara()
        //         {
        //             APINo = new DBChar(apiNo),
        //             DTEnd = new DBDateTime(dtEnd),
        //             ReqReturn = new DBNVarChar(reqReturn ?? string.Empty),
        //             UpdUserID = new DBVarChar(ServiceUserID)
        //         };
        //
        //         new Entity_Base(ConnectionStringSERP, ProviderNameSERP)
        //             .ExecAPIClientEnd(apiClientPara);
        //     }
        //     catch (Exception ex)
        //     {
        //         this.OnException(ex);
        //     }
        // }
    }
}
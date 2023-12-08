using LionTech.APIService.Teams;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Web;

namespace LionTech.EDIService.ERPExternal
{
    internal enum EnumEDIServiceEventGroupID
    {
        SysUserSystemRole,
        SysSystemFunAssign
    }

    internal enum EnumEDIServiceEventID
    {
        Edit
    }

    public enum EnumAppSettingKey
    {
        TeamsTo
    }

    internal static class Utility
    {
        static Utility()
        {
            ERPAPIService = new ERPAPIService();
        }

        internal static ERPAPIService ERPAPIService { get; set; }

        internal static string ExecEDIServiceDistributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString, int timeOut)
        {
            return ERPAPIService.Distributor(eventGroup, eventID, eventParaJsonString, timeOut);
        }

        internal static void SendMessageToTeams(string title, string message, string exceptionPath)
        {
            try
            {
                TeamsClient client = TeamsClient.Create();
                client.ClientSysID = EnumSystemID.ERPAP.ToString();

                string teamsID = ConfigurationManager.AppSettings[EnumAppSettingKey.TeamsTo.ToString()];

                client.SendMessage(teamsID, title, message);
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);
            }
        }

        public static EnumEDISystemID GetEnumEDISystemID(string SysID)
        {
            if (Enum.IsDefined(typeof(EnumEDISystemID), SysID) == false)
            {
                throw new EntityException(EnumEntityMessage.EnumValueError, new string[] { typeof(EnumEDISystemID).Name, SysID });
            }
            return (EnumEDISystemID)Enum.Parse(typeof(EnumEDISystemID), SysID);
        }
    }

    internal class ERPAPIService
    {
        public virtual string Distributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString, int timeOut)
        {
            string ediServiceDistributorPath =
                string.Format(
                    "{0}/EDIService/Distributor?ClientSysID={1}&SysID={2}&EventGroupID={3}&EventID={4}&EventPara={5}",
                    Common.GetEnumDesc(EnumAPISystemID.ERPAP),
                    EnumSystemID.ERPAP,
                    EnumSystemID.ERPAP,
                    eventGroup,
                    eventID,
                    HttpUtility.UrlEncode(eventParaJsonString));

            string responseString = Common.HttpWebRequestGetResponseString(ediServiceDistributorPath, timeOut);
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }
    }

    internal sealed class RedisConnection
    {
        private static Lazy<RedisConnection> lazy = new Lazy<RedisConnection>(() =>
        {
            if (string.IsNullOrEmpty(_settingOption)) throw new InvalidOperationException("Please call Init() first.");
            return new RedisConnection();
        });

        private static string _settingOption;

        public readonly ConnectionMultiplexer ConnectionMultiplexer;

        public static RedisConnection Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private RedisConnection()
        {
            ConnectionMultiplexer = ConnectionMultiplexer.Connect(_settingOption);
        }

        public static void Init(string settingOption)
        {
            _settingOption = settingOption;
        }
    }

}

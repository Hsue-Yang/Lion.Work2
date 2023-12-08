using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace LionTech.Entity.B2P
{
    internal enum EnumEnvironment
    {
        Developing,
        Testing,
        Production,
        AZTesting,
        AZProduction
    }

    internal class APDomainName
    {
        internal static string GetDomainNameUrl(EnumSystemID systemID)
        {
            string result = null;
            EnumEnvironment environment = EnumEnvironment.Developing;
            string appstring = ConfigurationManager.AppSettings["LionTech:ServerEnvironment"];

            if (string.IsNullOrWhiteSpace(appstring) == false)
            {
                Enum.TryParse(appstring, out environment);
            }

            switch (environment)
            {
                case EnumEnvironment.Developing:
                    result = DevelopingDictionary[systemID];
                    break;
                case EnumEnvironment.Testing:
                    result = TestingDictionary[systemID];
                    break;
                case EnumEnvironment.Production:
                    result = ProductionDictionary[systemID];
                    break;
                case EnumEnvironment.AZTesting:
                    result = AZTestingDictionary[systemID];
                    break;
                case EnumEnvironment.AZProduction:
                    result = AZProductionDictionary[systemID];
                    break;
            }

            return result;
        }

        #region - Developing -
        private static readonly Dictionary<EnumSystemID, string> DevelopingDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "127.0.0.1" },
            { EnumSystemID.B2PAP, "http://127.0.0.1:7777" },
            { EnumSystemID.HTLB2P, "http://127.0.0.1:7778" },
            { EnumSystemID.QCB2P, "http://127.0.0.1:7779" },
            { EnumSystemID.SCMB2P, "http://127.0.0.1:7780" },
            { EnumSystemID.RPMB2P, "http://127.0.0.1:7781" },
            { EnumSystemID.BUSB2P, "http://127.0.0.1:7782" },
            { EnumSystemID.TGUB2P, "http://127.0.0.1:7783" }
        };
        #endregion

        #region - Testing -
        private static readonly Dictionary<EnumSystemID, string> TestingDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com.tw" },
            { EnumSystemID.B2PAP, "http://ub2p.liontravel.com.tw" },
            { EnumSystemID.HTLB2P, "http://uhotelb2p.liontravel.com.tw" },
            { EnumSystemID.QCB2P, "http://uqcb2p.liontravel.com.tw" },
            { EnumSystemID.SCMB2P, "http://uscmb2p.liontravel.com.tw" },
            { EnumSystemID.RPMB2P, "http://urpmb2p.liontravel.com.tw" },
            { EnumSystemID.BUSB2P, "http://ubusb2p.liontravel.com.tw" },
            { EnumSystemID.TGUB2P, "http://utgub2p.liontravel.com.tw" }
        };
        #endregion

        #region - Production -
        private static readonly Dictionary<EnumSystemID, string> ProductionDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com" },
            { EnumSystemID.B2PAP, "https://b2p.liontravel.com" },
            { EnumSystemID.HTLB2P, "https://hotelb2p.liontravel.com" },
            { EnumSystemID.QCB2P, "https://qcb2p.liontravel.com" },
            { EnumSystemID.SCMB2P, "https://scmb2p.liontravel.com" },
            { EnumSystemID.RPMB2P, "https://rpmb2p.liontravel.com" },
            { EnumSystemID.BUSB2P, "https://busb2p.liontravel.com" },
            { EnumSystemID.TGUB2P, "https://tgub2p.liontravel.com" }
        };
        #endregion

        #region - AZ Testing -
        private static readonly Dictionary<EnumSystemID, string> AZTestingDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com.tw" },
            { EnumSystemID.B2PAP, "http://tub2p.liontravel.com.tw" },
            { EnumSystemID.HTLB2P, "http://tuhotelb2p.liontravel.com.tw" },
            { EnumSystemID.QCB2P, "http://tuqcb2p.liontravel.com.tw" },
            { EnumSystemID.SCMB2P, "http://tuscmb2p.liontravel.com.tw" },
            { EnumSystemID.RPMB2P, "http://turpmb2p.liontravel.com.tw" },
            { EnumSystemID.BUSB2P, "http://tubusb2p.liontravel.com.tw" },
            { EnumSystemID.TGUB2P, "http://tutgub2p.liontravel.com.tw" }
        };
        #endregion

        #region - AZ Production -
        private static readonly Dictionary<EnumSystemID, string> AZProductionDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com" },
            { EnumSystemID.B2PAP, "https://tb2p.liontravel.com" },
            { EnumSystemID.HTLB2P, "https://thotelb2p.liontravel.com" },
            { EnumSystemID.QCB2P, "https://tqcb2p.liontravel.com" },
            { EnumSystemID.SCMB2P, "https://tscmb2p.liontravel.com" },
            { EnumSystemID.RPMB2P, "https://trpmb2p.liontravel.com" },
            { EnumSystemID.BUSB2P, "https://tbusb2p.liontravel.com" },
            { EnumSystemID.TGUB2P, "https://ttgub2p.liontravel.com" }
        };
        #endregion
    }

    internal class APIDomainName
    {
        internal static string GetDomainNameUrl(EnumAPISystemID systemID)
        {
            string result = null;
            EnumEnvironment environment = EnumEnvironment.Developing;
            string appstring = ConfigurationManager.AppSettings["LionTech:ServerEnvironment"];

            if (string.IsNullOrWhiteSpace(appstring) == false)
            {
                Enum.TryParse(appstring, out environment);
            }

            switch (environment)
            {
                case EnumEnvironment.Developing:
                    result = DevelopingDictionary[systemID];
                    break;
                case EnumEnvironment.Testing:
                    result = TestingDictionary[systemID];
                    break;
                case EnumEnvironment.Production:
                    result = ProductionDictionary[systemID];
                    break;
                case EnumEnvironment.AZTesting:
                    result = AZTestingDictionary[systemID];
                    break;
                case EnumEnvironment.AZProduction:
                    result = AZProductionDictionary[systemID];
                    break;
            }

            return result;
        }

        #region - Developing -
        private static readonly Dictionary<EnumAPISystemID, string> DevelopingDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.B2PAP, "http://127.0.0.1:6692" },
            { EnumAPISystemID.HTLB2P, "http://127.0.0.1:6689" },
            { EnumAPISystemID.QCB2P, "http://127.0.0.1:6692" },
            { EnumAPISystemID.SCMB2P, "http://127.0.0.1:6692" },
            { EnumAPISystemID.RPMB2P, "http://127.0.0.1:6708" },
            { EnumAPISystemID.BUSB2P, "http://127.0.0.1:6709" },
            { EnumAPISystemID.TGUB2P, "http://127.0.0.1:6704" }
        };
        #endregion

        #region - Testing -
        private static readonly Dictionary<EnumAPISystemID, string> TestingDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.B2PAP, "http://uscminapi.liontravel.com.tw" },
            { EnumAPISystemID.HTLB2P, "http://uhotelinapi.liontravel.com.tw" },
            { EnumAPISystemID.QCB2P, "http://uscminapi.liontravel.com.tw" },
            { EnumAPISystemID.SCMB2P, "http://uscminapi.liontravel.com.tw" },
            { EnumAPISystemID.RPMB2P, "http://urpminapi.liontravel.com.tw" },
            { EnumAPISystemID.BUSB2P, "http://ubusinapi.liontravel.com.tw" },
            { EnumAPISystemID.TGUB2P, "http://utguinapi.liontravel.com.tw" }
        };
        #endregion

        #region - Production -
        private static readonly Dictionary<EnumAPISystemID, string> ProductionDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.B2PAP, "https://scminapi.liontravel.com" },
            { EnumAPISystemID.HTLB2P, "https://hotelinapi.liontravel.com" },
            { EnumAPISystemID.QCB2P, "https://scminapi.liontravel.com" },
            { EnumAPISystemID.SCMB2P, "https://scminapi.liontravel.com" },
            { EnumAPISystemID.RPMB2P, "https://rpminapi.liontravel.com" },
            { EnumAPISystemID.BUSB2P, "https://businapi.liontravel.com" },
            { EnumAPISystemID.TGUB2P, "https://tguinapi.liontravel.com" }
        };
        #endregion

        #region - AZ Testing -
        private static readonly Dictionary<EnumAPISystemID, string> AZTestingDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.B2PAP, "http://tuscminapi.liontravel.com.tw" },
            { EnumAPISystemID.HTLB2P, "http://tuhotelinapi.liontravel.com.tw" },
            { EnumAPISystemID.QCB2P, "http://tuscminapi.liontravel.com.tw" },
            { EnumAPISystemID.SCMB2P, "http://tuscminapi.liontravel.com.tw" },
            { EnumAPISystemID.RPMB2P, "http://turpminapi.liontravel.com.tw" },
            { EnumAPISystemID.BUSB2P, "http://tubusinapi.liontravel.com.tw" },
            { EnumAPISystemID.TGUB2P, "http://tutguinapi.liontravel.com.tw" }
        };
        #endregion

        #region - AZ Production -
        private static readonly Dictionary<EnumAPISystemID, string> AZProductionDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.B2PAP, "https://tscminapi.liontravel.com" },
            { EnumAPISystemID.HTLB2P, "https://thotelinapi.liontravel.com" },
            { EnumAPISystemID.QCB2P, "https://tscminapi.liontravel.com" },
            { EnumAPISystemID.SCMB2P, "https://tscminapi.liontravel.com" },
            { EnumAPISystemID.RPMB2P, "https://trpminapi.liontravel.com" },
            { EnumAPISystemID.BUSB2P, "https://tbusinapi.liontravel.com" },
            { EnumAPISystemID.TGUB2P, "https://ttguinapi.liontravel.com" }
        };
        #endregion
    }

    public class DomainNameAttribute : DescriptionAttribute
    {
        public DomainNameAttribute(EnumSystemID systemID)
        {
            DescriptionValue = APDomainName.GetDomainNameUrl(systemID);
        }

        public DomainNameAttribute(EnumAPISystemID systemID)
        {
            DescriptionValue = APIDomainName.GetDomainNameUrl(systemID);
        }
    }
}

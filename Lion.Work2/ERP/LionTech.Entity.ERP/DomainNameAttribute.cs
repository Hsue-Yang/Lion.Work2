using System;
using System.Collections.Generic;
using System.ComponentModel;
using LionTech.Utility;

namespace LionTech.Entity.ERP
{
    internal class APDomainName
    {
        internal static string GetDomainNameUrl(EnumSystemID systemID)
        {
            string result = null;

            switch (LionTechAppSettings.ServerEnvironment)
            {
                case EnumServerEnvironment.Developing:
                    result = DevelopingDictionary[systemID];
                    break;
                case EnumServerEnvironment.Testing:
                    result = TestingDictionary[systemID];
                    break;
                case EnumServerEnvironment.Production:
                    result = ProductionDictionary[systemID];
                    break;
                case EnumServerEnvironment.UplanProduction:
                    result = ProductionDictionary[systemID].Replace("liontravel", "uplantravel");
                    break;
                case EnumServerEnvironment.Daily:
                    result = DailyDictionary[systemID];
                    break;
                case EnumServerEnvironment.Learn:
                    result = TestingDictionary[systemID].Replace("http://u", "http://el");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        internal static string GetDomainNameUrlByDeveloping(EnumSystemID systemID)
        {
            return DevelopingDictionary[systemID];
        }

        #region - Developing -
        private static readonly Dictionary<EnumSystemID, string> DevelopingDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "127.0.0.1" },
            { EnumSystemID.ERPAP, "http://127.0.0.1:8888" },
            { EnumSystemID.LIBAP, "http://127.0.0.1:9999" },
            { EnumSystemID.TKNAP, "http://127.0.0.1:8920" },
            { EnumSystemID.WFAP, "http://127.0.0.1:8919" },
            { EnumSystemID.HTLAP, "http://127.0.0.1:8889" },
            { EnumSystemID.HTLMRP, "http://127.0.0.1:8898" },
            { EnumSystemID.BA2AP, "http://127.0.0.1:8890" },
            { EnumSystemID.BA2PD, "http://127.0.0.1:8927" },
            { EnumSystemID.BA2HC, "http://127.0.0.1:8928" },
            { EnumSystemID.BA2EC, "http://127.0.0.1:8929" },
            { EnumSystemID.BA2IA, "http://127.0.0.1:8930" },
            { EnumSystemID.TKTAP, "http://127.0.0.1:8891" },
            { EnumSystemID.TKTCAP, "http://127.0.0.1:8954" },
            { EnumSystemID.TKTMRP, "http://127.0.0.1:8899" },
            { EnumSystemID.SCMAP, "http://127.0.0.1:8892" },
            { EnumSystemID.AC2AP, "http://127.0.0.1:8893" },
            { EnumSystemID.ETKTAP, "http://127.0.0.1:8894" },
            { EnumSystemID.ETKMRP, "http://127.0.0.1:8900" },
            { EnumSystemID.HCMAP, "http://127.0.0.1:8895" },
            { EnumSystemID.AMAP, "http://127.0.0.1:8921" },
            { EnumSystemID.WCAP, "http://127.0.0.1:8944" },
            { EnumSystemID.DWHAP, "http://127.0.0.1:8896" },
            { EnumSystemID.CRMAP, "http://127.0.0.1:8918" },
            { EnumSystemID.SFAAP, "http://127.0.0.1:8897" },
            { EnumSystemID.KAMAP, "http://127.0.0.1:8941" },
            { EnumSystemID.BUSAP, "http://127.0.0.1:8922" },
            { EnumSystemID.BUSMRP, "http://127.0.0.1:8901" },
            { EnumSystemID.TGUAP, "http://127.0.0.1:8907" },
            { EnumSystemID.TGUMRP, "http://127.0.0.1:8902" },
            { EnumSystemID.TOURAP, "http://127.0.0.1:8903" },
            { EnumSystemID.ACCAP, "http://127.0.0.1:8904" },
            { EnumSystemID.QAMAP, "http://127.0.0.1:8905" },
            { EnumSystemID.PUBAP, "http://127.0.0.1:8906" },
            { EnumSystemID.GITAP, "http://127.0.0.1:8907" },
            { EnumSystemID.GITAAP, "http://127.0.0.1:8908" },
            { EnumSystemID.GITBAP, "http://127.0.0.1:8909" },
            { EnumSystemID.GITZAP, "http://127.0.0.1:8910" },
            { EnumSystemID.GITMRP, "http://127.0.0.1:8935" },
            { EnumSystemID.ORDRAP, "http://127.0.0.1:8911" },
            { EnumSystemID.FNAP, "http://127.0.0.1:8912" },
            { EnumSystemID.FNZZAP, "http://127.0.0.1:8913" },
            { EnumSystemID.FNOTAP, "http://127.0.0.1:8914" },
            { EnumSystemID.FNINAP, "http://127.0.0.1:8915" },
            { EnumSystemID.FNIOAP, "http://127.0.0.1:8916" },
            { EnumSystemID.FNEXAP, "http://127.0.0.1:8917" },
            { EnumSystemID.RPMAP, "http://127.0.0.1:8918" },
            { EnumSystemID.CBMAP, "http://127.0.0.1:8923" },
            { EnumSystemID.CBMCA, "http://127.0.0.1:8936" },
            { EnumSystemID.MCMCA, "http://127.0.0.1:8936" },
            { EnumSystemID.ADSAP, "http://127.0.0.1:8924" },
            { EnumSystemID.MKTAP, "http://127.0.0.1:8925" },
            { EnumSystemID.MKTOUR, "http://127.0.0.1:8926" },
            { EnumSystemID.MELAP, "http://127.0.0.1:8932" },
            { EnumSystemID.MELMRP, "http://127.0.0.1:8933" },
            { EnumSystemID.FITAP, "http://127.0.0.1:8934" },
            { EnumSystemID.LOCAP, "http://127.0.0.1:8937" },
            { EnumSystemID.LOCMRP, "http://127.0.0.1:8938" },
            { EnumSystemID.PDMAP, "http://127.0.0.1:8939" },
            { EnumSystemID.TDPDM, "http://127.0.0.1:8940" },
            { EnumSystemID.POIAP, "http://127.0.0.1:8942" },
            { EnumSystemID.POIMRP, "http://127.0.0.1:8943" },
            { EnumSystemID.UPFIT, "http://127.0.0.1:8945" },
            { EnumSystemID.WMP2AP, "http://127.0.0.1:8946" },
            { EnumSystemID.MMSAP, "http://127.0.0.1:8951" },
            { EnumSystemID.GITPCM, "http://127.0.0.1:8947" },
            { EnumSystemID.CRSAP, "http://127.0.0.1:8948" },
            { EnumSystemID.CRSMRP, "http://127.0.0.1:8949" },
            { EnumSystemID.MTMRP, "http://127.0.0.1:8950" },
            { EnumSystemID.XFLAP, "http://127.0.0.1:8952" },
            { EnumSystemID.CCSAP, "http://127.0.0.1:8953" },
            { EnumSystemID.DTKTAP, "http://127.0.0.1:8954" },
            { EnumSystemID.FINVCN, "http://127.0.0.1:8955" },
            { EnumSystemID.VISAAP, "http://127.0.0.1:8956" },
            { EnumSystemID.VISACNMRP, "http://127.0.0.1:8957" },
            { EnumSystemID.PEUAP, "http://127.0.0.1:8958" },
            { EnumSystemID.MCMAP, "http://127.0.0.1:8959" },
            { EnumSystemID.GENAP, "http://127.0.0.1:8960" },
            { EnumSystemID.MISAP, "http://127.0.0.1:8961" },
            { EnumSystemID.CONSOLEAP, "http://127.0.0.1:8962" },
            { EnumSystemID.CTMAP, "http://127.0.0.1:8963" },
            { EnumSystemID.LXSCNAP, "http://127.0.0.1:8964" },
            { EnumSystemID.PDM2AP, "http://127.0.0.1:8965" },
            { EnumSystemID.PAYROLLAP, "http://127.0.0.1:8966" },
            { EnumSystemID.PEUETKT, "http://127.0.0.1:8967" },
            { EnumSystemID.RWAP, "http://127.0.0.1:8968" },
            { EnumSystemID.TRAININGAP, "http://127.0.0.1:8969" },
            { EnumSystemID.XINTUKUAP, "http://127.0.0.1:8970" },
            { EnumSystemID.COUPONAP, "http://127.0.0.1:8971" },
            { EnumSystemID.CENTCTRLAP, "http://127.0.0.1:8972" },
            { EnumSystemID.ROADAP, "http://127.0.0.1:8973" },
            { EnumSystemID.RAILWAYAP, "http://127.0.0.1:8974" },
            { EnumSystemID.OLCSAP, "http://127.0.0.1:8975" },
            { EnumSystemID.WEBPLATFORM, "http://127.0.0.1:8976" },
            { EnumSystemID.APIMGTAP, "http://127.0.0.1:8977" }
        };
        #endregion

        #region - Lion Testing -
        private static readonly Dictionary<EnumSystemID, string> TestingDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com.tw" },
            { EnumSystemID.ERPAP, "http://userp.liontravel.com.tw" },
            { EnumSystemID.LIBAP, "http://ulib.liontravel.com.tw" },
            { EnumSystemID.TKNAP, "http://utkn.liontravel.com.tw" },
            { EnumSystemID.WFAP, "http://uwf.liontravel.com.tw" },
            { EnumSystemID.HTLAP, "http://uhotelerp.liontravel.com.tw" },
            { EnumSystemID.HTLMRP, "http://uhotelmrp.liontravel.com.tw" },
            { EnumSystemID.BA2AP, "http://uba2.liontravel.com.tw" },
            { EnumSystemID.BA2PD, "http://updba2.liontravel.com.tw" },
            { EnumSystemID.BA2HC, "http://uhcba2.liontravel.com.tw" },
            { EnumSystemID.BA2EC, "http://uecba2.liontravel.com.tw" },
            { EnumSystemID.BA2IA, "http://uiaba2.liontravel.com.tw" },
            { EnumSystemID.TKTAP, "http://utkterp.liontravel.com.tw" },
            { EnumSystemID.TKTCAP, "http://utktcerp.liontravel.com.tw" },
            { EnumSystemID.TKTMRP, "http://utktmrp.liontravel.com.tw" },
            { EnumSystemID.SCMAP, "http://uscm.liontravel.com.tw" },
            { EnumSystemID.AC2AP, "http://uac2.liontravel.com.tw" },
            { EnumSystemID.ETKTAP, "http://uetkterp.liontravel.com.tw" },
            { EnumSystemID.ETKMRP, "http://uetktmrp.liontravel.com.tw" },
            { EnumSystemID.HCMAP, "http://uhcm.liontravel.com.tw" },
            { EnumSystemID.AMAP, "http://uam.liontravel.com.tw" },
            { EnumSystemID.WCAP, "http://uwc.liontravel.com.tw" },
            { EnumSystemID.DWHAP, "http://udwh.liontravel.com.tw" },
            { EnumSystemID.CRMAP, "http://ucrm.liontravel.com.tw" },
            { EnumSystemID.SFAAP, "http://usfa.liontravel.com.tw" },
            { EnumSystemID.KAMAP, "http://ukam.liontravel.com.tw" },
            { EnumSystemID.BUSAP, "http://ubuserp.liontravel.com.tw" },
            { EnumSystemID.BUSMRP, "http://ubusmrp.liontravel.com.tw" },
            { EnumSystemID.TGUAP, "http://utguerp.liontravel.com.tw" },
            { EnumSystemID.TGUMRP, "http://utgumrp.liontravel.com.tw" },
            { EnumSystemID.TOURAP, "http://utourerp.liontravel.com.tw" },
            { EnumSystemID.ACCAP, "http://uacc.liontravel.com.tw" },
            { EnumSystemID.QAMAP, "http://uqam.liontravel.com.tw" },
            { EnumSystemID.PUBAP, "http://upub.liontravel.com.tw" },
            { EnumSystemID.GITAP, "http://ugiterp.liontravel.com.tw" },
            { EnumSystemID.GITAAP, "http://ugitaerp.liontravel.com.tw" },
            { EnumSystemID.GITBAP, "http://ugitberp.liontravel.com.tw" },
            { EnumSystemID.GITZAP, "http://ugitzerp.liontravel.com.tw" },
            { EnumSystemID.GITMRP, "http://ugitmrp.liontravel.com.tw" },
            { EnumSystemID.ORDRAP, "http://uordrerp.liontravel.com.tw" },
            { EnumSystemID.FNAP, "http://ufnerp.liontravel.com.tw" },
            { EnumSystemID.FNZZAP, "http://ufnzzerp.liontravel.com.tw" },
            { EnumSystemID.FNOTAP, "http://ufnoterp.liontravel.com.tw" },
            { EnumSystemID.FNINAP, "http://ufninerp.liontravel.com.tw" },
            { EnumSystemID.FNIOAP, "http://ufnioerp.liontravel.com.tw" },
            { EnumSystemID.FNEXAP, "http://ufnexerp.liontravel.com.tw" },
            { EnumSystemID.RPMAP, "http://urpm.liontravel.com.tw" },
            { EnumSystemID.CBMAP, "http://ucbm.liontravel.com.tw" },
            { EnumSystemID.CBMCA, "http://ucacbm.liontravel.com.tw" },
            { EnumSystemID.MCMCA, "http://umcmca.liontravel.com.tw" },
            { EnumSystemID.ADSAP, "http://uads.liontravel.com.tw" },
            { EnumSystemID.MKTAP, "http://umkt.liontravel.com.tw" },
            { EnumSystemID.MKTOUR, "http://umktour.liontravel.com.tw" },
            { EnumSystemID.MELAP, "http://umelerp.liontravel.com.tw" },
            { EnumSystemID.MELMRP, "http://umelmrp.liontravel.com.tw" },
            { EnumSystemID.FITAP, "http://ufiterp.liontravel.com.tw" },
            { EnumSystemID.LOCAP, "http://ulocerp.liontravel.com.tw" },
            { EnumSystemID.LOCMRP, "http://ulocmrp.liontravel.com.tw" },
            { EnumSystemID.PDMAP, "http://updm.liontravel.com.tw" },
            { EnumSystemID.TDPDM, "http://utdpdm.liontravel.com.tw" },
            { EnumSystemID.POIAP, "http://upoierp.liontravel.com.tw" },
            { EnumSystemID.POIMRP, "http://upoimrp.liontravel.com.tw" },
            { EnumSystemID.UPFIT, "http://uupfiterp.liontravel.com.tw" },
            { EnumSystemID.WMP2AP, "http://uwmp2.liontravel.com.tw" },
            { EnumSystemID.MMSAP, "http://umms.liontravel.com.tw" },
            { EnumSystemID.GITPCM, "http://ugitpcm.liontravel.com.tw" },
            { EnumSystemID.CRSAP, "http://ucrserp.liontravel.com.tw" },
            { EnumSystemID.CRSMRP, "http://ucrsmrp.liontravel.com.tw" },
            { EnumSystemID.MTMRP, "http://umtmrp.liontravel.com.tw" },
            { EnumSystemID.XFLAP, "http://uxfl.liontravel.com.tw" },
            { EnumSystemID.CCSAP, "http://uccserp.liontravel.com.tw" },
            { EnumSystemID.DTKTAP, "http://udtkterp.liontravel.com.tw" },
            { EnumSystemID.FINVCN, "http://ufinvcnerp.liontravel.com.tw" },
            { EnumSystemID.VISAAP, "http://uvisaerp.liontravel.com.tw" },
            { EnumSystemID.VISACNMRP, "http://uvisacnmrp.liontravel.com.tw" },
            { EnumSystemID.PEUAP, "http://upeu.liontravel.com.tw" },
            { EnumSystemID.MCMAP, "http://umcmerp.liontravel.com.tw" },
            { EnumSystemID.GENAP, "http://ugen.liontravel.com.tw" },
            { EnumSystemID.MISAP, "http://umis.liontravel.com.tw" },
            { EnumSystemID.CONSOLEAP, "http://uconsole.liontravel.com.tw" },
            { EnumSystemID.CTMAP, "http://uctm.liontravel.com.tw" },
            { EnumSystemID.LXSCNAP, "http://ulxscnerp.liontravel.com.tw" },
            { EnumSystemID.PDM2AP, "http://updm2.liontravel.com.tw" },
            { EnumSystemID.PAYROLLAP, "http://upayroll.liontravel.com.tw" },
            { EnumSystemID.PEUETKT, "http://upeuetkt.liontravel.com.tw" },
            { EnumSystemID.RWAP, "http://urwerp.liontravel.com.tw" },
            { EnumSystemID.TRAININGAP, "http://utraining.liontravel.com.tw" },
            { EnumSystemID.XINTUKUAP, "http://uxintuku.liontravel.com.tw" },
            { EnumSystemID.COUPONAP, "http://ucoupon.liontravel.com.tw" },
            { EnumSystemID.CENTCTRLAP, "http://ucentctrlerp.liontravel.com.tw" },
            { EnumSystemID.ROADAP, "http://uroaderp.liontravel.com.tw" },
            { EnumSystemID.RAILWAYAP, "http://urailwayerp.liontravel.com.tw" },
            { EnumSystemID.OLCSAP, "http://uolcs.liontravel.com.tw" },
            { EnumSystemID.WEBPLATFORM, "http://uwebplatform.liontravel.com.tw" },
            { EnumSystemID.APIMGTAP, "http://uapimgt.liontravel.com.tw" }
        };
        #endregion

        #region - Lion Production -
        private static readonly Dictionary<EnumSystemID, string> ProductionDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com" },
            { EnumSystemID.ERPAP, "https://serp.liontravel.com" },
            { EnumSystemID.LIBAP, "https://lib.liontravel.com" },
            { EnumSystemID.TKNAP, "https://tkn.liontravel.com" },
            { EnumSystemID.WFAP, "https://wf.liontravel.com" },
            { EnumSystemID.HTLAP, "https://hotelerp.liontravel.com" },
            { EnumSystemID.HTLMRP, "https://hotelmrp.liontravel.com" },
            { EnumSystemID.BA2AP, "https://ba2.liontravel.com" },
            { EnumSystemID.BA2PD, "https://pdba2.liontravel.com" },
            { EnumSystemID.BA2HC, "https://hcba2.liontravel.com" },
            { EnumSystemID.BA2EC, "https://ecba2.liontravel.com" },
            { EnumSystemID.BA2IA, "https://iaba2.liontravel.com" },
            { EnumSystemID.TKTAP, "https://tkterp.liontravel.com" },
            { EnumSystemID.TKTCAP, "https://tktcerp.liontravel.com" },
            { EnumSystemID.TKTMRP, "https://tktmrp.liontravel.com" },
            { EnumSystemID.SCMAP, "https://scm.liontravel.com" },
            { EnumSystemID.AC2AP, "https://ac2.liontravel.com" },
            { EnumSystemID.ETKTAP, "https://etkterp.liontravel.com" },
            { EnumSystemID.ETKMRP, "https://etktmrp.liontravel.com" },
            { EnumSystemID.HCMAP, "https://hcm.liontravel.com" },
            { EnumSystemID.AMAP, "https://am.liontravel.com" },
            { EnumSystemID.WCAP, "https://wc.liontravel.com" },
            { EnumSystemID.DWHAP, "https://dwh.liontravel.com" },
            { EnumSystemID.CRMAP, "https://crm.liontravel.com" },
            { EnumSystemID.SFAAP, "https://sfa.liontravel.com" },
            { EnumSystemID.KAMAP, "https://kam.liontravel.com" },
            { EnumSystemID.BUSAP, "https://buserp.liontravel.com" },
            { EnumSystemID.BUSMRP, "https://busmrp.liontravel.com" },
            { EnumSystemID.TGUAP, "https://tguerp.liontravel.com" },
            { EnumSystemID.TGUMRP, "https://tgumrp.liontravel.com" },
            { EnumSystemID.TOURAP, "https://tourerp.liontravel.com" },
            { EnumSystemID.ACCAP, "https://acc.liontravel.com" },
            { EnumSystemID.QAMAP, "https://qam.liontravel.com" },
            { EnumSystemID.PUBAP, "https://pub.liontravel.com" },
            { EnumSystemID.GITAP, "https://giterp.liontravel.com" },
            { EnumSystemID.GITAAP, "https://gitaerp.liontravel.com" },
            { EnumSystemID.GITBAP, "https://gitberp.liontravel.com" },
            { EnumSystemID.GITZAP, "https://gitzerp.liontravel.com" },
            { EnumSystemID.GITMRP, "https://gitmrp.liontravel.com" },
            { EnumSystemID.ORDRAP, "https://ordrerp.liontravel.com" },
            { EnumSystemID.FNAP, "https://fnerp.liontravel.com" },
            { EnumSystemID.FNZZAP, "https://fnzzerp.liontravel.com" },
            { EnumSystemID.FNOTAP, "https://fnoterp.liontravel.com" },
            { EnumSystemID.FNINAP, "https://fninerp.liontravel.com" },
            { EnumSystemID.FNIOAP, "https://fnioerp.liontravel.com" },
            { EnumSystemID.FNEXAP, "https://fnexerp.liontravel.com" },
            { EnumSystemID.RPMAP, "https://rpm.liontravel.com" },
            { EnumSystemID.CBMAP, "https://cbm.liontravel.com" },
            { EnumSystemID.CBMCA, "https://cacbm.liontravel.com" },
            { EnumSystemID.MCMCA, "https://mcmca.liontravel.com" },
            { EnumSystemID.ADSAP, "https://ads.liontravel.com" },
            { EnumSystemID.MKTAP, "https://mkt.liontravel.com" },
            { EnumSystemID.MKTOUR, "https://mktour.liontravel.com" },
            { EnumSystemID.MELAP, "https://melerp.liontravel.com" },
            { EnumSystemID.MELMRP, "https://melmrp.liontravel.com" },
            { EnumSystemID.FITAP, "https://fiterp.liontravel.com" },
            { EnumSystemID.LOCAP, "https://locerp.liontravel.com" },
            { EnumSystemID.LOCMRP, "https://locmrp.liontravel.com" },
            { EnumSystemID.PDMAP, "https://pdm.liontravel.com" },
            { EnumSystemID.TDPDM, "https://tdpdm.liontravel.com" },
            { EnumSystemID.POIAP, "https://poierp.liontravel.com" },
            { EnumSystemID.POIMRP, "https://poimrp.liontravel.com" },
            { EnumSystemID.UPFIT, "https://upfiterp.liontravel.com" },
            { EnumSystemID.WMP2AP, "https://wmp2.liontravel.com" },
            { EnumSystemID.MMSAP, "https://mms.liontravel.com" },
            { EnumSystemID.GITPCM, "https://gitpcm.liontravel.com" },
            { EnumSystemID.CRSAP, "https://crserp.liontravel.com" },
            { EnumSystemID.CRSMRP, "https://crsmrp.liontravel.com" },
            { EnumSystemID.MTMRP, "https://mtmrp.liontravel.com" },
            { EnumSystemID.XFLAP, "https://xfl.liontravel.com" },
            { EnumSystemID.CCSAP, "https://ccserp.liontravel.com" },
            { EnumSystemID.DTKTAP, "https://dtkterp.liontravel.com" },
            { EnumSystemID.FINVCN, "https://finvcnerp.liontravel.com" },
            { EnumSystemID.VISAAP, "https://visaerp.liontravel.com" },
            { EnumSystemID.VISACNMRP, "https://visacnmrp.liontravel.com" },
            { EnumSystemID.PEUAP, "https://peu.liontravel.com" },
            { EnumSystemID.MCMAP, "https://mcmerp.liontravel.com" },
            { EnumSystemID.GENAP, "https://gen.liontravel.com" },
            { EnumSystemID.MISAP, "https://mis.liontravel.com" },
            { EnumSystemID.CONSOLEAP, "https://console.liontravel.com" },
            { EnumSystemID.CTMAP, "https://ctm.liontravel.com" },
            { EnumSystemID.LXSCNAP, "https://lxscnerp.liontravel.com" },
            { EnumSystemID.PDM2AP, "https://pdm2.liontravel.com" },
            { EnumSystemID.PAYROLLAP, "https://payroll.liontravel.com" },
            { EnumSystemID.PEUETKT, "https://peuetkt.liontravel.com" },
            { EnumSystemID.RWAP, "https://rwerp.liontravel.com" },
            { EnumSystemID.TRAININGAP, "https://training.liontravel.com" },
            { EnumSystemID.XINTUKUAP, "https://xintuku.liontravel.com" },
            { EnumSystemID.COUPONAP, "https://coupon.liontravel.com" },
            { EnumSystemID.CENTCTRLAP, "https://centctrlerp.liontravel.com" },
            { EnumSystemID.ROADAP, "https://roaderp.liontravel.com" },
            { EnumSystemID.RAILWAYAP, "https://railwayerp.liontravel.com" },
            { EnumSystemID.OLCSAP, "https://olcs.liontravel.com" },
            { EnumSystemID.WEBPLATFORM, "https://webplatform.liontravel.com" },
            { EnumSystemID.APIMGTAP, "https://apimgt.liontravel.com" }
        };
        #endregion

        #region - Lion Daily -
        private static readonly Dictionary<EnumSystemID, string> DailyDictionary = new Dictionary<EnumSystemID, string>
        {
            { EnumSystemID.Domain, "liontravel.com" },
            { EnumSystemID.ERPAP, "https://dserp.liontravel.com" },
            { EnumSystemID.LIBAP, "https://dlib.liontravel.com" },
            { EnumSystemID.TKNAP, "https://dtkn.liontravel.com" },
            { EnumSystemID.WFAP, "https://dwf.liontravel.com" },
            { EnumSystemID.HTLAP, "https://dhotelerp.liontravel.com" },
            { EnumSystemID.HTLMRP, "https://dhotelmrp.liontravel.com" },
            { EnumSystemID.BA2AP, "https://dba2.liontravel.com" },
            { EnumSystemID.BA2PD, "https://dpdba2.liontravel.com" },
            { EnumSystemID.BA2HC, "https://dhcba2.liontravel.com" },
            { EnumSystemID.BA2EC, "https://decba2.liontravel.com" },
            { EnumSystemID.BA2IA, "https://diaba2.liontravel.com" },
            { EnumSystemID.TKTAP, "https://dtkterp.liontravel.com" },
            { EnumSystemID.TKTCAP, "https://dtktcerp.liontravel.com" },
            { EnumSystemID.TKTMRP, "https://dtktmrp.liontravel.com" },
            { EnumSystemID.SCMAP, "https://dscm.liontravel.com" },
            { EnumSystemID.AC2AP, "https://dac2.liontravel.com" },
            { EnumSystemID.ETKTAP, "https://detkterp.liontravel.com" },
            { EnumSystemID.ETKMRP, "https://detktmrp.liontravel.com" },
            { EnumSystemID.HCMAP, "https://dhcm.liontravel.com" },
            { EnumSystemID.AMAP, "https://dam.liontravel.com" },
            { EnumSystemID.WCAP, "https://dwc.liontravel.com" },
            { EnumSystemID.DWHAP, "https://ddwh.liontravel.com" },
            { EnumSystemID.CRMAP, "https://dcrm.liontravel.com" },
            { EnumSystemID.SFAAP, "https://dsfa.liontravel.com" },
            { EnumSystemID.KAMAP, "https://dkam.liontravel.com" },
            { EnumSystemID.BUSAP, "https://dbuserp.liontravel.com" },
            { EnumSystemID.BUSMRP, "https://dbusmrp.liontravel.com" },
            { EnumSystemID.TGUAP, "https://dtguerp.liontravel.com" },
            { EnumSystemID.TGUMRP, "https://dtgumrp.liontravel.com" },
            { EnumSystemID.TOURAP, "https://dtourerp.liontravel.com" },
            { EnumSystemID.ACCAP, "https://dacc.liontravel.com" },
            { EnumSystemID.QAMAP, "https://dqam.liontravel.com" },
            { EnumSystemID.PUBAP, "https://dpub.liontravel.com" },
            { EnumSystemID.GITAP, "https://dgiterp.liontravel.com" },
            { EnumSystemID.GITAAP, "https://dgitaerp.liontravel.com" },
            { EnumSystemID.GITBAP, "https://dgitberp.liontravel.com" },
            { EnumSystemID.GITZAP, "https://dgitzerp.liontravel.com" },
            { EnumSystemID.GITMRP, "https://dgitmrp.liontravel.com" },
            { EnumSystemID.ORDRAP, "https://dordrerp.liontravel.com" },
            { EnumSystemID.FNAP, "https://dfnerp.liontravel.com" },
            { EnumSystemID.FNZZAP, "https://dfnzzerp.liontravel.com" },
            { EnumSystemID.FNOTAP, "https://dfnoterp.liontravel.com" },
            { EnumSystemID.FNINAP, "https://dfninerp.liontravel.com" },
            { EnumSystemID.FNIOAP, "https://dfnioerp.liontravel.com" },
            { EnumSystemID.FNEXAP, "https://dfnexerp.liontravel.com" },
            { EnumSystemID.RPMAP, "https://drpm.liontravel.com" },
            { EnumSystemID.CBMAP, "https://dcbm.liontravel.com" },
            { EnumSystemID.CBMCA, "https://dcacbm.liontravel.com" },
            { EnumSystemID.MCMCA, "https://dmcmca.liontravel.com" },
            { EnumSystemID.ADSAP, "https://dads.liontravel.com" },
            { EnumSystemID.MKTAP, "https://dmkt.liontravel.com" },
            { EnumSystemID.MKTOUR, "https://dmktour.liontravel.com" },
            { EnumSystemID.MELAP, "https://dmelerp.liontravel.com" },
            { EnumSystemID.MELMRP, "https://dmelmrp.liontravel.com" },
            { EnumSystemID.FITAP, "https://dfiterp.liontravel.com" },
            { EnumSystemID.LOCAP, "https://dlocerp.liontravel.com" },
            { EnumSystemID.LOCMRP, "https://dlocmrp.liontravel.com" },
            { EnumSystemID.PDMAP, "https://dpdm.liontravel.com" },
            { EnumSystemID.TDPDM, "https://dtdpdm.liontravel.com" },
            { EnumSystemID.POIAP, "https://dpoierp.liontravel.com" },
            { EnumSystemID.POIMRP, "https://dpoimrp.liontravel.com" },
            { EnumSystemID.UPFIT, "https://dupfiterp.liontravel.com" },
            { EnumSystemID.WMP2AP, "https://dwmp2.liontravel.com" },
            { EnumSystemID.MMSAP, "https://dmms.liontravel.com" },
            { EnumSystemID.GITPCM, "https://dgitpcm.liontravel.com" },
            { EnumSystemID.CRSAP, "https://dcrserp.liontravel.com" },
            { EnumSystemID.CRSMRP, "https://dcrsmrp.liontravel.com" },
            { EnumSystemID.MTMRP, "https://dmtmrp.liontravel.com" },
            { EnumSystemID.XFLAP, "https://dxfl.liontravel.com" },
            { EnumSystemID.CCSAP, "https://dccserp.liontravel.com" },
            { EnumSystemID.DTKTAP, "https://ddtkterp.liontravel.com" },
            { EnumSystemID.FINVCN, "https://dfinvcnerp.liontravel.com" },
            { EnumSystemID.VISAAP, "https://dvisaerp.liontravel.com" },
            { EnumSystemID.VISACNMRP, "https://dvisacnmrp.liontravel.com" },
            { EnumSystemID.PEUAP, "https://dpeu.liontravel.com" },
            { EnumSystemID.MCMAP, "https://dmcmerp.liontravel.com" },
            { EnumSystemID.GENAP, "https://dgen.liontravel.com" },
            { EnumSystemID.MISAP, "https://dmis.liontravel.com" },
            { EnumSystemID.CONSOLEAP, "https://dconsole.liontravel.com" },
            { EnumSystemID.CTMAP, "https://dctm.liontravel.com" },
            { EnumSystemID.LXSCNAP, "https://dlxscnerp.liontravel.com" },
            { EnumSystemID.PDM2AP, "https://dpdm2.liontravel.com" },
            { EnumSystemID.PAYROLLAP, "https://dpayroll.liontravel.com" },
            { EnumSystemID.PEUETKT, "https://dpeuetkt.liontravel.com" },
            { EnumSystemID.RWAP, "https://drwerp.liontravel.com" },
            { EnumSystemID.TRAININGAP, "https://dtraining.liontravel.com" }
        };
        #endregion
    }

    internal class APIDomainName
    {
        internal static string GetDomainNameUrl(EnumAPISystemID systemID)
        {
            string result = null;

            switch (LionTechAppSettings.ServerEnvironment)
            {
                case EnumServerEnvironment.Developing:
                    result = DevelopingDictionary[systemID];
                    break;
                case EnumServerEnvironment.Testing:
                    result = TestingDictionary[systemID];
                    break;
                case EnumServerEnvironment.Production:
                case EnumServerEnvironment.UplanProduction:
                    result = ProductionDictionary[systemID];
                    break;
                case EnumServerEnvironment.Daily:
                    result = DailyDictionary[systemID];
                    break;
                case EnumServerEnvironment.Learn:
                    result = TestingDictionary[systemID].Replace("http://u", "http://el");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        #region - Developing -
        private static readonly Dictionary<EnumAPISystemID, string> DevelopingDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.ERPAP, "http://127.0.0.1:6666" },
            { EnumAPISystemID.LIBAP, "" },
            { EnumAPISystemID.TKNAP, "http://127.0.0.1:6698" },
            { EnumAPISystemID.WFAP, "http://127.0.0.1:6699" },
            { EnumAPISystemID.LOGAP, "http://127.0.0.1:6655" },
            { EnumAPISystemID.PUSHAP, "http://127.0.0.1:6734" },
            { EnumAPISystemID.HTLAP, "http://127.0.0.1:6689" },
            { EnumAPISystemID.HTLMRP, "http://127.0.0.1:6720" },
            { EnumAPISystemID.BA2AP, "http://127.0.0.1:6690" },
            { EnumAPISystemID.BA2PD, "" },
            { EnumAPISystemID.BA2HC, "" },
            { EnumAPISystemID.BA2EC, "" },
            { EnumAPISystemID.BA2IA, "" },
            { EnumAPISystemID.TKTAP, "http://127.0.0.1:6691" },
            { EnumAPISystemID.TKTCAP, "http://127.0.0.1:6730" },
            { EnumAPISystemID.TKTMRP, "" },
            { EnumAPISystemID.TKTMGT, "http://127.0.0.1:6698" },
            { EnumAPISystemID.SCMAP, "http://127.0.0.1:6692" },
            { EnumAPISystemID.AC2AP, "http://127.0.0.1:6693" },
            { EnumAPISystemID.ETKTAP, "http://127.0.0.1:6694" },
            { EnumAPISystemID.ETKMRP, "" },
            { EnumAPISystemID.HCMAP, "http://127.0.0.1:6695" },
            { EnumAPISystemID.AMAP, "" },
            { EnumAPISystemID.WCAP, "" },
            { EnumAPISystemID.DWHAP, "http://127.0.0.1:6696" },
            { EnumAPISystemID.CRMAP, "http://127.0.0.1:6697" },
            { EnumAPISystemID.SFAAP, "" },
            { EnumAPISystemID.KAMAP, "" },
            { EnumAPISystemID.BUSAP, "http://127.0.0.1:6709" },
            { EnumAPISystemID.BUSMRP, "" },
            { EnumAPISystemID.TGUAP, "http://127.0.0.1:6704" },
            { EnumAPISystemID.TGUMRP, "" },
            { EnumAPISystemID.TOURAP, "http://127.0.0.1:6700" },
            { EnumAPISystemID.ACCAP, "http://127.0.0.1:6701" },
            { EnumAPISystemID.QAMAP, "http://127.0.0.1:6702" },
            { EnumAPISystemID.PUBAP, "http://127.0.0.1:6703" },
            { EnumAPISystemID.GITAP, "http://127.0.0.1:6705" },
            { EnumAPISystemID.GITAAP, "" },
            { EnumAPISystemID.GITBAP, "" },
            { EnumAPISystemID.GITZAP, "" },
            { EnumAPISystemID.GITMRP, "" },
            { EnumAPISystemID.ORDRAP, "http://127.0.0.1:6706" },
            { EnumAPISystemID.FNAP, "http://127.0.0.1:6707" },
            { EnumAPISystemID.FNZZAP, "" },
            { EnumAPISystemID.FNOTAP, "" },
            { EnumAPISystemID.FNINAP, "" },
            { EnumAPISystemID.FNIOAP, "" },
            { EnumAPISystemID.FNEXAP, "" },
            { EnumAPISystemID.RPMAP, "http://127.0.0.1:6708" },
            { EnumAPISystemID.CBMAP, "http://127.0.0.1:6710" },
            { EnumAPISystemID.CBMCA, "" },
            { EnumAPISystemID.MCMCA, "" },
            { EnumAPISystemID.ADSAP, "http://127.0.0.1:6711" },
            { EnumAPISystemID.MKTAP, "http://127.0.0.1:6712" },
            { EnumAPISystemID.MKTOUR, "" },
            { EnumAPISystemID.MELAP, "http://127.0.0.1:6713" },
            { EnumAPISystemID.MELMRP, "" },
            { EnumAPISystemID.FITAP, "http://127.0.0.1:6714" },
            { EnumAPISystemID.LOCAP, "http://127.0.0.1:6715" },
            { EnumAPISystemID.LOCMRP, "" },
            { EnumAPISystemID.PDMAP, "http://127.0.0.1:6716" },
            { EnumAPISystemID.TDPDM, "" },
            { EnumAPISystemID.POIAP, "http://127.0.0.1:6717" },
            { EnumAPISystemID.POIMRP, "" },
            { EnumAPISystemID.UPFIT, "http://127.0.0.1:6718" },
            { EnumAPISystemID.WMP2AP, "http://127.0.0.1:6719" },
            { EnumAPISystemID.MMSAP, "" },
            { EnumAPISystemID.GITPCM, "http://127.0.0.1:6720" },
            { EnumAPISystemID.CRSAP, "http://127.0.0.1:6721" },
            { EnumAPISystemID.CRSMRP, "" },
            { EnumAPISystemID.MTMRP, "http://127.0.0.1:6722" },
            { EnumAPISystemID.XFLAP, "http://127.0.0.1:6723" },
            { EnumAPISystemID.CCSAP, "http://127.0.0.1:6724" },
            { EnumAPISystemID.DTKTAP, "http://127.0.0.1:6725" },
            { EnumAPISystemID.FINVCN, "http://127.0.0.1:6726" },
            { EnumAPISystemID.VISAAP, "http://127.0.0.1:6738" },
            { EnumAPISystemID.VISACN, "http://127.0.0.1:6727" },
            { EnumAPISystemID.PEUAP, "http://127.0.0.1:6728" },
            { EnumAPISystemID.MCMAP, "http://127.0.0.1:6729" },
            { EnumAPISystemID.GENAP, "http://127.0.0.1:6730" },
            { EnumAPISystemID.MISAP, "http://127.0.0.1:6731" },
            { EnumAPISystemID.CONSOLEAP, "http://127.0.0.1:6732" },
            { EnumAPISystemID.CTMAP, "http://127.0.0.1:6733" },
            { EnumAPISystemID.LXSCNAP, "http://127.0.0.1:6735" },
            { EnumAPISystemID.PDM2AP, "http://127.0.0.1:6736" },
            { EnumAPISystemID.PAYROLLAP, "http://127.0.0.1:6737" },
            { EnumAPISystemID.PEUETKT, "http://127.0.0.1:6739" },
            { EnumAPISystemID.RWAP, "http://127.0.0.1:6740" },
            { EnumAPISystemID.TRAININGAP, "http://127.0.0.1:6741" },
            { EnumAPISystemID.XINTUKUAP, "http://127.0.0.1:6742" },
            { EnumAPISystemID.SYSMGTAP, "http://127.0.0.1:6743" },
            { EnumAPISystemID.COUPONAP, "http://127.0.0.1:6744" },
            { EnumAPISystemID.CENTCTRLAP, "http://127.0.0.1:6745" },
            { EnumAPISystemID.OLCSAP, "http://127.0.0.1:6746" },
            { EnumAPISystemID.WEBPLATFORM, "http://127.0.0.1:6747" },
            { EnumAPISystemID.ESIGNAP, "http://127.0.0.1:5235" },
            { EnumAPISystemID.APIMGTAP, "http://127.0.0.1:6748" }
        };
        #endregion

        #region - Lion Testing -
        private static readonly Dictionary<EnumAPISystemID, string> TestingDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.ERPAP, "http://uinapi.liontravel.com.tw" },
            { EnumAPISystemID.LIBAP, "" },
            { EnumAPISystemID.TKNAP, "http://utoken.liontravel.com.tw" },
            { EnumAPISystemID.WFAP, "http://uworkflow.liontravel.com.tw" },
            { EnumAPISystemID.LOGAP, "http://uloginapi.liontravel.com.tw" },
            { EnumAPISystemID.PUSHAP, "http://upushinapi.liontravel.com.tw" },
            { EnumAPISystemID.HTLAP, "http://uhotelinapi.liontravel.com.tw" },
            { EnumAPISystemID.HTLMRP, "http://uhotelmrpinapi.liontravel.com.tw" },
            { EnumAPISystemID.BA2AP, "http://uba2inapi.liontravel.com.tw" },
            { EnumAPISystemID.BA2PD, "" },
            { EnumAPISystemID.BA2HC, "" },
            { EnumAPISystemID.BA2EC, "" },
            { EnumAPISystemID.BA2IA, "" },
            { EnumAPISystemID.TKTAP, "http://utktinapi.liontravel.com.tw" },
            { EnumAPISystemID.TKTCAP, "http://utktcinapi.liontravel.com.tw" },
            { EnumAPISystemID.TKTMRP, "" },
            { EnumAPISystemID.TKTMGT, "http://utktmgtinapi.liontravel.com.tw" },
            { EnumAPISystemID.SCMAP, "http://uscminapi.liontravel.com.tw" },
            { EnumAPISystemID.AC2AP, "http://uac2inapi.liontravel.com.tw" },
            { EnumAPISystemID.ETKTAP, "http://uetktinapi.liontravel.com.tw" },
            { EnumAPISystemID.ETKMRP, "" },
            { EnumAPISystemID.HCMAP, "http://uhcminapi.liontravel.com.tw" },
            { EnumAPISystemID.AMAP, "" },
            { EnumAPISystemID.WCAP, "" },
            { EnumAPISystemID.DWHAP, "http://udwhinapi.liontravel.com.tw" },
            { EnumAPISystemID.CRMAP, "http://ucrminapi.liontravel.com.tw" },
            { EnumAPISystemID.SFAAP, "" },
            { EnumAPISystemID.KAMAP, "" },
            { EnumAPISystemID.BUSAP, "http://ubusinapi.liontravel.com.tw" },
            { EnumAPISystemID.BUSMRP, "" },
            { EnumAPISystemID.TGUAP, "http://utguinapi.liontravel.com.tw" },
            { EnumAPISystemID.TGUMRP, "" },
            { EnumAPISystemID.TOURAP, "http://utourinapi.liontravel.com.tw" },
            { EnumAPISystemID.ACCAP, "http://uaccinapi.liontravel.com.tw" },
            { EnumAPISystemID.QAMAP, "http://uqaminapi.liontravel.com.tw" },
            { EnumAPISystemID.PUBAP, "http://upubinapi.liontravel.com.tw" },
            { EnumAPISystemID.GITAP, "http://ugitinapi.liontravel.com.tw" },
            { EnumAPISystemID.GITAAP, "" },
            { EnumAPISystemID.GITBAP, "" },
            { EnumAPISystemID.GITZAP, "" },
            { EnumAPISystemID.GITMRP, "" },
            { EnumAPISystemID.ORDRAP, "http://uordrinapi.liontravel.com.tw" },
            { EnumAPISystemID.FNAP, "http://ufninapi.liontravel.com.tw" },
            { EnumAPISystemID.FNZZAP, "" },
            { EnumAPISystemID.FNOTAP, "" },
            { EnumAPISystemID.FNINAP, "" },
            { EnumAPISystemID.FNIOAP, "" },
            { EnumAPISystemID.FNEXAP, "" },
            { EnumAPISystemID.RPMAP, "http://urpminapi.liontravel.com.tw" },
            { EnumAPISystemID.CBMAP, "http://ucbminapi.liontravel.com.tw" },
            { EnumAPISystemID.CBMCA, "" },
            { EnumAPISystemID.MCMCA, "" },
            { EnumAPISystemID.ADSAP, "http://uadsinapi.liontravel.com.tw" },
            { EnumAPISystemID.MKTAP, "http://umktinapi.liontravel.com.tw" },
            { EnumAPISystemID.MKTOUR, "" },
            { EnumAPISystemID.MELAP, "http://umelinapi.liontravel.com.tw" },
            { EnumAPISystemID.MELMRP, "" },
            { EnumAPISystemID.FITAP, "http://ufitinapi.liontravel.com.tw" },
            { EnumAPISystemID.LOCAP, "http://ulocinapi.liontravel.com.tw" },
            { EnumAPISystemID.LOCMRP, "" },
            { EnumAPISystemID.PDMAP, "http://updminapi.liontravel.com.tw" },
            { EnumAPISystemID.TDPDM, "" },
            { EnumAPISystemID.POIAP, "http://upoiinapi.liontravel.com.tw" },
            { EnumAPISystemID.POIMRP, "" },
            { EnumAPISystemID.UPFIT, "http://uupfitinapi.liontravel.com.tw" },
            { EnumAPISystemID.WMP2AP, "http://uwmp2inapi.liontravel.com.tw" },
            { EnumAPISystemID.MMSAP, "" },
            { EnumAPISystemID.GITPCM, "http://ugitpcminapi.liontravel.com.tw" },
            { EnumAPISystemID.CRSAP, "http://ucrsinapi.liontravel.com.tw" },
            { EnumAPISystemID.CRSMRP, "" },
            { EnumAPISystemID.MTMRP, "http://umtmrpinapi.liontravel.com.tw" },
            { EnumAPISystemID.XFLAP, "http://uxflinapi.liontravel.com.tw" },
            { EnumAPISystemID.CCSAP, "http://uccsinapi.liontravel.com.tw" },
            { EnumAPISystemID.DTKTAP, "http://udtktinapi.liontravel.com.tw" },
            { EnumAPISystemID.FINVCN, "http://ufinvcninapi.liontravel.com.tw" },
            { EnumAPISystemID.VISAAP, "http://uvisainapi.liontravel.com.tw" },
            { EnumAPISystemID.VISACN, "http://uvisacninapi.liontravel.com.tw" },
            { EnumAPISystemID.PEUAP, "http://upeuinapi.liontravel.com.tw" },
            { EnumAPISystemID.MCMAP, "http://umcminapi.liontravel.com.tw" },
            { EnumAPISystemID.GENAP, "http://ugeninapi.liontravel.com.tw" },
            { EnumAPISystemID.MISAP, "http://umisinapi.liontravel.com.tw" },
            { EnumAPISystemID.CONSOLEAP, "http://uconsoleinapi.liontravel.com.tw" },
            { EnumAPISystemID.CTMAP, "http://uctminapi.liontravel.com.tw" },
            { EnumAPISystemID.LXSCNAP, "http://ulxscninapi.liontravel.com.tw" },
            { EnumAPISystemID.PDM2AP, "http://updm2inapi.liontravel.com.tw" },
            { EnumAPISystemID.PAYROLLAP, "http://upayrollinapi.liontravel.com.tw" },
            { EnumAPISystemID.PEUETKT, "http://upeuetktinapi.liontravel.com.tw" },
            { EnumAPISystemID.RWAP, "http://urwinapi.liontravel.com.tw" },
            { EnumAPISystemID.TRAININGAP, "http://utraininginapi.liontravel.com.tw" },
            { EnumAPISystemID.XINTUKUAP, "http://uxintukuinapi.liontravel.com.tw" },
            { EnumAPISystemID.SYSMGTAP, "http://usysmgtinapi.liontravel.com.tw" },
            { EnumAPISystemID.COUPONAP, "http://ucouponinapi.liontravel.com.tw" },
            { EnumAPISystemID.CENTCTRLAP, "http://ucentctrlinapi.liontravel.com.tw" },
            { EnumAPISystemID.OLCSAP, "http://uolcsinapi.liontravel.com.tw" },
            { EnumAPISystemID.WEBPLATFORM, "http://uwebplatforminapi.liontravel.com.tw" },
            { EnumAPISystemID.ESIGNAP, "http://uesigninapi.liontravel.com.tw" },
            { EnumAPISystemID.APIMGTAP, "http://uapimgtinapi.liontravel.com.tw" }
        };
        #endregion

        #region - Lion Production -
        private static readonly Dictionary<EnumAPISystemID, string> ProductionDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.ERPAP, "https://inapi.liontravel.com" },
            { EnumAPISystemID.LIBAP, "" },
            { EnumAPISystemID.TKNAP, "https://token.liontravel.com" },
            { EnumAPISystemID.WFAP, "https://workflow.liontravel.com" },
            { EnumAPISystemID.LOGAP, "https://loginapi.liontravel.com" },
            { EnumAPISystemID.PUSHAP, "https://pushinapi.liontravel.com" },
            { EnumAPISystemID.HTLAP, "https://hotelinapi.liontravel.com" },
            { EnumAPISystemID.HTLMRP, "https://hotelmrpinapi.liontravel.com" },
            { EnumAPISystemID.BA2AP, "https://ba2inapi.liontravel.com" },
            { EnumAPISystemID.BA2PD, "" },
            { EnumAPISystemID.BA2HC, "" },
            { EnumAPISystemID.BA2EC, "" },
            { EnumAPISystemID.BA2IA, "" },
            { EnumAPISystemID.TKTAP, "https://tktinapi.liontravel.com" },
            { EnumAPISystemID.TKTCAP, "https://tktcinapi.liontravel.com" },
            { EnumAPISystemID.TKTMRP, "" },
            { EnumAPISystemID.TKTMGT, "https://tktmgtinapi.liontravel.com" },
            { EnumAPISystemID.SCMAP, "https://scminapi.liontravel.com" },
            { EnumAPISystemID.AC2AP, "https://ac2inapi.liontravel.com" },
            { EnumAPISystemID.ETKTAP, "https://etktinapi.liontravel.com" },
            { EnumAPISystemID.ETKMRP, "" },
            { EnumAPISystemID.HCMAP, "https://hcminapi.liontravel.com" },
            { EnumAPISystemID.AMAP, "" },
            { EnumAPISystemID.WCAP, "" },
            { EnumAPISystemID.DWHAP, "https://dwhinapi.liontravel.com" },
            { EnumAPISystemID.CRMAP, "https://crminapi.liontravel.com" },
            { EnumAPISystemID.SFAAP, "" },
            { EnumAPISystemID.KAMAP, "" },
            { EnumAPISystemID.BUSAP, "https://businapi.liontravel.com" },
            { EnumAPISystemID.BUSMRP, "" },
            { EnumAPISystemID.TGUAP, "https://tguinapi.liontravel.com" },
            { EnumAPISystemID.TGUMRP, "" },
            { EnumAPISystemID.TOURAP, "https://tourinapi.liontravel.com" },
            { EnumAPISystemID.ACCAP, "https://accinapi.liontravel.com" },
            { EnumAPISystemID.QAMAP, "https://qaminapi.liontravel.com" },
            { EnumAPISystemID.PUBAP, "https://pubinapi.liontravel.com" },
            { EnumAPISystemID.GITAP, "https://gitinapi.liontravel.com" },
            { EnumAPISystemID.GITAAP, "" },
            { EnumAPISystemID.GITBAP, "" },
            { EnumAPISystemID.GITZAP, "" },
            { EnumAPISystemID.GITMRP, "" },
            { EnumAPISystemID.ORDRAP, "https://ordrinapi.liontravel.com" },
            { EnumAPISystemID.FNAP, "https://fninapi.liontravel.com" },
            { EnumAPISystemID.FNZZAP, "" },
            { EnumAPISystemID.FNOTAP, "" },
            { EnumAPISystemID.FNINAP, "" },
            { EnumAPISystemID.FNIOAP, "" },
            { EnumAPISystemID.FNEXAP, "" },
            { EnumAPISystemID.RPMAP, "https://rpminapi.liontravel.com" },
            { EnumAPISystemID.CBMAP, "https://cbminapi.liontravel.com" },
            { EnumAPISystemID.CBMCA, "" },
            { EnumAPISystemID.MCMCA, "" },
            { EnumAPISystemID.ADSAP, "https://adsinapi.liontravel.com" },
            { EnumAPISystemID.MKTAP, "https://mktinapi.liontravel.com" },
            { EnumAPISystemID.MKTOUR, "" },
            { EnumAPISystemID.MELAP, "https://melinapi.liontravel.com" },
            { EnumAPISystemID.MELMRP, "" },
            { EnumAPISystemID.FITAP, "https://fitinapi.liontravel.com" },
            { EnumAPISystemID.LOCAP, "https://locinapi.liontravel.com" },
            { EnumAPISystemID.LOCMRP, "" },
            { EnumAPISystemID.PDMAP, "https://pdminapi.liontravel.com" },
            { EnumAPISystemID.TDPDM, "" },
            { EnumAPISystemID.POIAP, "https://poiinapi.liontravel.com" },
            { EnumAPISystemID.POIMRP, "" },
            { EnumAPISystemID.UPFIT, "https://upfitinapi.liontravel.com" },
            { EnumAPISystemID.WMP2AP, "https://wmp2inapi.liontravel.com" },
            { EnumAPISystemID.MMSAP, "" },
            { EnumAPISystemID.GITPCM, "https://gitpcminapi.liontravel.com" },
            { EnumAPISystemID.CRSAP, "https://crsinapi.liontravel.com" },
            { EnumAPISystemID.CRSMRP, "" },
            { EnumAPISystemID.MTMRP, "https://mtmrpinapi.liontravel.com" },
            { EnumAPISystemID.XFLAP, "https://xflinapi.liontravel.com" },
            { EnumAPISystemID.CCSAP, "https://ccsinapi.liontravel.com" },
            { EnumAPISystemID.DTKTAP, "https://dtktinapi.liontravel.com" },
            { EnumAPISystemID.FINVCN, "https://finvcninapi.liontravel.com" },
            { EnumAPISystemID.VISAAP, "https://visainapi.liontravel.com" },
            { EnumAPISystemID.VISACN, "https://visacninapi.liontravel.com" },
            { EnumAPISystemID.PEUAP, "https://peuinapi.liontravel.com" },
            { EnumAPISystemID.MCMAP, "https://mcminapi.liontravel.com" },
            { EnumAPISystemID.GENAP, "https://geninapi.liontravel.com" },
            { EnumAPISystemID.MISAP, "https://misinapi.liontravel.com" },
            { EnumAPISystemID.CONSOLEAP, "https://consoleinapi.liontravel.com" },
            { EnumAPISystemID.CTMAP, "https://ctminapi.liontravel.com" },
            { EnumAPISystemID.LXSCNAP, "https://lxscninapi.liontravel.com" },
            { EnumAPISystemID.PDM2AP, "https://pdm2inapi.liontravel.com" },
            { EnumAPISystemID.PAYROLLAP, "https://payrollinapi.liontravel.com" },
            { EnumAPISystemID.PEUETKT, "https://peuetktinapi.liontravel.com" },
            { EnumAPISystemID.RWAP, "https://rwinapi.liontravel.com" },
            { EnumAPISystemID.TRAININGAP, "https://traininginapi.liontravel.com" },
            { EnumAPISystemID.XINTUKUAP, "https://xintukuinapi.liontravel.com" },
            { EnumAPISystemID.SYSMGTAP, "https://sysmgtinapi.liontravel.com" },
            { EnumAPISystemID.COUPONAP, "https://couponinapi.liontravel.com" },
            { EnumAPISystemID.CENTCTRLAP, "https://centctrlinapi.liontravel.com" },
            { EnumAPISystemID.OLCSAP, "https://olcsinapi.liontravel.com" },
            { EnumAPISystemID.WEBPLATFORM, "https://webplatforminapi.liontravel.com" },
            { EnumAPISystemID.ESIGNAP, "https://esigninapi.liontravel.com" },
            { EnumAPISystemID.APIMGTAP, "https://apimgtinapi.liontravel.com" }
        };
        #endregion

        #region - Lion Daily -
        private static readonly Dictionary<EnumAPISystemID, string> DailyDictionary = new Dictionary<EnumAPISystemID, string>
        {
            { EnumAPISystemID.ERPAP, "https://dinapi.liontravel.com" },
            { EnumAPISystemID.LIBAP, "" },
            { EnumAPISystemID.TKNAP, "https://dtoken.liontravel.com" },
            { EnumAPISystemID.WFAP, "https://dworkflow.liontravel.com" },
            { EnumAPISystemID.LOGAP, "https://dloginapi.liontravel.com" },
            { EnumAPISystemID.PUSHAP, "https://dpushinapi.liontravel.com" },
            { EnumAPISystemID.HTLAP, "https://dhotelinapi.liontravel.com" },
            { EnumAPISystemID.HTLMRP, "https://dhotelmrpinapi.liontravel.com" },
            { EnumAPISystemID.BA2AP, "https://dba2inapi.liontravel.com" },
            { EnumAPISystemID.BA2PD, "" },
            { EnumAPISystemID.BA2HC, "" },
            { EnumAPISystemID.BA2EC, "" },
            { EnumAPISystemID.BA2IA, "" },
            { EnumAPISystemID.TKTAP, "https://dtktinapi.liontravel.com" },
            { EnumAPISystemID.TKTCAP, "https://dtktcinapi.liontravel.com" },
            { EnumAPISystemID.TKTMRP, "" },
            { EnumAPISystemID.SCMAP, "https://dscminapi.liontravel.com" },
            { EnumAPISystemID.AC2AP, "https://dac2inapi.liontravel.com" },
            { EnumAPISystemID.ETKTAP, "https://detktinapi.liontravel.com" },
            { EnumAPISystemID.ETKMRP, "" },
            { EnumAPISystemID.HCMAP, "https://dhcminapi.liontravel.com" },
            { EnumAPISystemID.AMAP, "" },
            { EnumAPISystemID.WCAP, "" },
            { EnumAPISystemID.DWHAP, "https://ddwhinapi.liontravel.com" },
            { EnumAPISystemID.CRMAP, "https://dcrminapi.liontravel.com" },
            { EnumAPISystemID.SFAAP, "" },
            { EnumAPISystemID.KAMAP, "" },
            { EnumAPISystemID.BUSAP, "https://dbusinapi.liontravel.com" },
            { EnumAPISystemID.BUSMRP, "" },
            { EnumAPISystemID.TGUAP, "https://dtguinapi.liontravel.com" },
            { EnumAPISystemID.TGUMRP, "" },
            { EnumAPISystemID.TOURAP, "https://dtourinapi.liontravel.com" },
            { EnumAPISystemID.ACCAP, "https://daccinapi.liontravel.com" },
            { EnumAPISystemID.QAMAP, "https://dqaminapi.liontravel.com" },
            { EnumAPISystemID.PUBAP, "https://dpubinapi.liontravel.com" },
            { EnumAPISystemID.GITAP, "https://dgitinapi.liontravel.com" },
            { EnumAPISystemID.GITAAP, "" },
            { EnumAPISystemID.GITBAP, "" },
            { EnumAPISystemID.GITZAP, "" },
            { EnumAPISystemID.GITMRP, "" },
            { EnumAPISystemID.ORDRAP, "https://dordrinapi.liontravel.com" },
            { EnumAPISystemID.FNAP, "https://dfninapi.liontravel.com" },
            { EnumAPISystemID.FNZZAP, "" },
            { EnumAPISystemID.FNOTAP, "" },
            { EnumAPISystemID.FNINAP, "" },
            { EnumAPISystemID.FNIOAP, "" },
            { EnumAPISystemID.FNEXAP, "" },
            { EnumAPISystemID.RPMAP, "https://drpminapi.liontravel.com" },
            { EnumAPISystemID.CBMAP, "https://dcbminapi.liontravel.com" },
            { EnumAPISystemID.CBMCA, "" },
            { EnumAPISystemID.MCMCA, "" },
            { EnumAPISystemID.ADSAP, "https://dadsinapi.liontravel.com" },
            { EnumAPISystemID.MKTAP, "https://dmktinapi.liontravel.com" },
            { EnumAPISystemID.MKTOUR, "" },
            { EnumAPISystemID.MELAP, "https://dmelinapi.liontravel.com" },
            { EnumAPISystemID.MELMRP, "" },
            { EnumAPISystemID.FITAP, "https://dfitinapi.liontravel.com" },
            { EnumAPISystemID.LOCAP, "https://dlocinapi.liontravel.com" },
            { EnumAPISystemID.LOCMRP, "" },
            { EnumAPISystemID.PDMAP, "https://dpdminapi.liontravel.com" },
            { EnumAPISystemID.TDPDM, "" },
            { EnumAPISystemID.POIAP, "https://dpoiinapi.liontravel.com" },
            { EnumAPISystemID.POIMRP, "" },
            { EnumAPISystemID.UPFIT, "https://dupfitinapi.liontravel.com" },
            { EnumAPISystemID.WMP2AP, "https://dwmp2inapi.liontravel.com" },
            { EnumAPISystemID.MMSAP, "" },
            { EnumAPISystemID.GITPCM, "https://dgitpcminapi.liontravel.com" },
            { EnumAPISystemID.CRSAP, "https://dcrsinapi.liontravel.com" },
            { EnumAPISystemID.CRSMRP, "" },
            { EnumAPISystemID.MTMRP, "https://dmtmrpinapi.liontravel.com" },
            { EnumAPISystemID.XFLAP, "https://dxflinapi.liontravel.com" },
            { EnumAPISystemID.CCSAP, "https://dccsinapi.liontravel.com" },
            { EnumAPISystemID.DTKTAP, "https://ddtktinapi.liontravel.com" },
            { EnumAPISystemID.FINVCN, "https://dfinvcninapi.liontravel.com" },
            { EnumAPISystemID.VISAAP, "" },
            { EnumAPISystemID.VISACN, "https://dvisacninapi.liontravel.com" },
            { EnumAPISystemID.PEUAP, "https://dpeuinapi.liontravel.com" },
            { EnumAPISystemID.MCMAP, "https://dmcminapi.liontravel.com" },
            { EnumAPISystemID.GENAP, "https://dgeninapi.liontravel.com" },
            { EnumAPISystemID.MISAP, "https://dmisinapi.liontravel.com" },
            { EnumAPISystemID.CONSOLEAP, "https://dconsoleinapi.liontravel.com" },
            { EnumAPISystemID.CTMAP, "https://dctminapi.liontravel.com" },
            { EnumAPISystemID.LXSCNAP, "https://dlxscninapi.liontravel.com" },
            { EnumAPISystemID.PDM2AP, "https://dpdm2inapi.liontravel.com" },
            { EnumAPISystemID.PAYROLLAP, "https://dpayrollinapi.liontravel.com" },
            { EnumAPISystemID.PEUETKT, "https://dpeuetktinapi.liontravel.com" },
            { EnumAPISystemID.RWAP, "https://drwinapi.liontravel.com" },
            { EnumAPISystemID.TRAININGAP, "https://dtraininginapi.liontravel.com" }
        };
        #endregion
    }

    public class ExApiUrl
    {
        internal static string GetFunctionUrl(EnumExApiURL systemID)
        {
            string result = null;

            switch (LionTechAppSettings.ServerEnvironment)
            {
                case EnumServerEnvironment.Developing:
                    result = DevelopingDictionary[systemID];
                    break;
                case EnumServerEnvironment.Testing:
                    result = TestingDictionary[systemID];
                    break;
                case EnumServerEnvironment.Production:
                case EnumServerEnvironment.UplanProduction:
                    result = ProductionDictionary[systemID];
                    break;
                case EnumServerEnvironment.Daily:
                    result = DailyDictionary[systemID];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        #region - Developing -
        private static readonly Dictionary<EnumExApiURL, string> DevelopingDictionary = new Dictionary<EnumExApiURL, string>
        {
            { EnumExApiURL.AuthWithOTP, "https://uinc.api.liontravel.com/api/V2/OTPLogin" },
            { EnumExApiURL.TokenGenerator, "https://uauth.api.liontravel.com/v2/token/generator" }
        };
        #endregion

        #region - Testing -
        private static readonly Dictionary<EnumExApiURL, string> TestingDictionary = new Dictionary<EnumExApiURL, string>
        {
            { EnumExApiURL.AuthWithOTP, "https://uinc.api.liontravel.com/api/V2/OTPLogin" },
            { EnumExApiURL.TokenGenerator, "https://uauth.api.liontravel.com/v2/token/generator" }
        };
        #endregion

        #region - Production -
        private static readonly Dictionary<EnumExApiURL, string> ProductionDictionary = new Dictionary<EnumExApiURL, string>
        {
            { EnumExApiURL.AuthWithOTP, "https://inc.api.liontravel.com/api/V2/OTPLogin" },
            { EnumExApiURL.TokenGenerator, "https://auth.api.liontravel.com/v2/token/generator" }
        };
        #endregion

        #region - Lion Daily -
        private static readonly Dictionary<EnumExApiURL, string> DailyDictionary = new Dictionary<EnumExApiURL, string>
        {
            { EnumExApiURL.AuthWithOTP, "https://dinc.api.liontravel.com/api/V2/OTPLogin" },
            { EnumExApiURL.TokenGenerator, "https://dauth.api.liontravel.com/v2/token/generator" }
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

    public class ApiUrlAttribute : DescriptionAttribute
    {
        public ApiUrlAttribute(EnumExApiURL exApi)
        {
            DescriptionValue = ExApiUrl.GetFunctionUrl(exApi);
        }
    }
}

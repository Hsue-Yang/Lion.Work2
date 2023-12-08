using System.ComponentModel;

namespace LionTech.Entity.ERP
{
    public enum EnumCultureID
    {
        [Description("zh-TW")]
        zh_TW,
        [Description("zh-CN")]
        zh_CN,
        [Description("en-US")]
        en_US,
        [Description("th-TH")]
        th_TH,
        [Description("ja-JP")]
        ja_JP,
        [Description("ko-KR")]
        ko_KR
    }

    public enum EnumSystemID
    {
        [Category("Developing"), DomainName(Domain)]
        Domain,
        [Category("ERPAP"), DomainName(ERPAP)]
        ERPAP,
        [Category("ERPAP"), DomainName(LIBAP)]
        LIBAP,
        [Category("ERPAP"), DomainName(TKNAP)]
        TKNAP,
        [Category("ERPAP"), DomainName(WFAP)]
        WFAP,
        [Category("HTLAP"), DomainName(HTLAP)]
        HTLAP,
        [Category("HTLMRP"), DomainName(HTLMRP)]
        HTLMRP,
        [Category("BA2AP"), DomainName(BA2AP)]
        BA2AP,
        [Category("BA2AP"), DomainName(BA2PD)]
        BA2PD,
        [Category("BA2AP"), DomainName(BA2HC)]
        BA2HC,
        [Category("BA2AP"), DomainName(BA2EC)]
        BA2EC,
        [Category("BA2AP"), DomainName(BA2IA)]
        BA2IA,
        [Category("TKTAP"), DomainName(TKTAP)]
        TKTAP,
        [Category("TKTAP"), DomainName(TKTCAP)]
        TKTCAP,
        [Category("TKTAP"), DomainName(TKTMRP)]
        TKTMRP,
        [Category("SCMAP"), DomainName(SCMAP)]
        SCMAP,
        [Category("AC2AP"), DomainName(AC2AP)]
        AC2AP,
        [Category("ETKTAP"), DomainName(ETKTAP)]
        ETKTAP,
        [Category("ETKTAP"), DomainName(ETKMRP)]
        ETKMRP,
        [Category("HCMAP"), DomainName(HCMAP)]
        HCMAP,
        [Category("HCMAP"), DomainName(AMAP)]
        AMAP,
        [Category("HCMAP"), DomainName(WCAP)]
        WCAP,
        [Category("DWHAP"), DomainName(DWHAP)]
        DWHAP,
        [Category("CRMAP"), DomainName(CRMAP)]
        CRMAP,
        [Category("CRMAP"), DomainName(SFAAP)]
        SFAAP,
        [Category("CRMAP"), DomainName(KAMAP)]
        KAMAP,
        [Category("BUSAP"), DomainName(BUSAP)]
        BUSAP,
        [Category("BUSAP"), DomainName(BUSMRP)]
        BUSMRP,
        [Category("TGUAP"), DomainName(TGUAP)]
        TGUAP,
        [Category("TGUAP"), DomainName(TGUMRP)]
        TGUMRP,
        [Category("TOURAP"), DomainName(TOURAP)]
        TOURAP,
        [Category("ACCAP"), DomainName(ACCAP)]
        ACCAP,
        [Category("QAMAP"), DomainName(QAMAP)]
        QAMAP,
        [Category("PUBAP"), DomainName(PUBAP)]
        PUBAP,
        [Category("GITAP"), DomainName(GITAP)]
        GITAP,
        [Category("GITAP"), DomainName(GITAAP)]
        GITAAP,
        [Category("GITAP"), DomainName(GITBAP)]
        GITBAP,
        [Category("GITAP"), DomainName(GITZAP)]
        GITZAP,
        [Category("GITAP"), DomainName(GITMRP)]
        GITMRP,
        [Category("ORDRAP"), DomainName(ORDRAP)]
        ORDRAP,
        [Category("FNAP"), DomainName(FNAP)]
        FNAP,
        [Category("FNAP"), DomainName(FNZZAP)]
        FNZZAP,
        [Category("FNAP"), DomainName(FNOTAP)]
        FNOTAP,
        [Category("FNAP"), DomainName(FNINAP)]
        FNINAP,
        [Category("FNAP"), DomainName(FNIOAP)]
        FNIOAP,
        [Category("FNAP"), DomainName(FNEXAP)]
        FNEXAP,
        [Category("RPMAP"), DomainName(RPMAP)]
        RPMAP,
        [Category("CBMAP"), DomainName(CBMAP)]
        CBMAP,
        [Category("CBMAP"), DomainName(CBMCA)]
        CBMCA,
        [Category("MCMAP"), DomainName(MCMCA)]
        MCMCA,
        [Category("ADSAP"), DomainName(ADSAP)]
        ADSAP,
        [Category("MKTAP"), DomainName(MKTAP)]
        MKTAP,
        [Category("MKTAP"), DomainName(MKTOUR)]
        MKTOUR,
        [Category("MELAP"), DomainName(MELAP)]
        MELAP,
        [Category("MELAP"), DomainName(MELMRP)]
        MELMRP,
        [Category("FITAP"), DomainName(FITAP)]
        FITAP,
        [Category("LOCAP"), DomainName(LOCAP)]
        LOCAP,
        [Category("LOCAP"), DomainName(LOCMRP)]
        LOCMRP,
        [Category("PDMAP"), DomainName(PDMAP)]
        PDMAP,
        [Category("PDMAP"), DomainName(TDPDM)]
        TDPDM,
        [Category("POIAP"), DomainName(POIAP)]
        POIAP,
        [Category("POIAP"), DomainName(POIMRP)]
        POIMRP,
        [Category("UPFIT"), DomainName(UPFIT)]
        UPFIT,
        [Category("WMP2AP"), DomainName(WMP2AP)]
        WMP2AP,
        [Category("MMSAP"), DomainName(MMSAP)]
        MMSAP,
        [Category("GITPCM"), DomainName(GITPCM)]
        GITPCM,
        [Category("CRSAP"), DomainName(CRSAP)]
        CRSAP,
        [Category("CRSAP"), DomainName(CRSMRP)]
        CRSMRP,
        [Category("MTMRP"), DomainName(MTMRP)]
        MTMRP,
        [Category("XFLAP"), DomainName(XFLAP)]
        XFLAP,
        [Category("CCSAP"), DomainName(CCSAP)]
        CCSAP,
        [Category("DTKTAP"), DomainName(DTKTAP)]
        DTKTAP,
        [Category("FINVCN"), DomainName(FINVCN)]
        FINVCN,
        [Category("VISAAP"), DomainName(VISAAP)]
        VISAAP,
        [Category("VISAAP"), DomainName(VISACNMRP)]
        VISACNMRP,
        [Category("PEUAP"), DomainName(PEUAP)]
        PEUAP,
        [Category("MCMAP"), DomainName(MCMAP)]
        MCMAP,
        [Category("GENAP"), DomainName(GENAP)]
        GENAP,
        [Category("MISAP"), DomainName(MISAP)]
        MISAP,
        [Category("CONSOLEAP"), DomainName(CONSOLEAP)]
        CONSOLEAP,
        [Category("CTMAP"), DomainName(CTMAP)]
        CTMAP,
        [Category("LXSCNAP"), DomainName(LXSCNAP)]
        LXSCNAP,
        [Category("PDM2AP"), DomainName(PDM2AP)]
        PDM2AP,
        [Category("PAYROLLAP"), DomainName(PAYROLLAP)]
        PAYROLLAP,
        [Category("PEUETKT"), DomainName(PEUETKT)]
        PEUETKT,
        [Category("RWAP"), DomainName(RWAP)]
        RWAP,
        [Category("TRAININGAP"), DomainName(TRAININGAP)]
        TRAININGAP,
        [Category("XINTUKUAP"), DomainName(XINTUKUAP)]
        XINTUKUAP,
        [Category("ERPAP"), DomainName(SYSMGTAP)]
        SYSMGTAP,
        [Category("COUPONAP"), DomainName(COUPONAP)]
        COUPONAP,
        [Category("CENTCTRLAP"), DomainName(CENTCTRLAP)]
        CENTCTRLAP,
        [Category("CENTCTRLAP"), DomainName(ROADAP)]
        ROADAP,
        [Category("CENTCTRLAP"), DomainName(RAILWAYAP)]
        RAILWAYAP,
        [Category("OLCSAP"), DomainName(OLCSAP)]
        OLCSAP,
        [Category("WEBPLATFORM"), DomainName(WEBPLATFORM)]
        WEBPLATFORM,
        [Category("APIMGTAP"), DomainName(APIMGTAP)]
        APIMGTAP,
    }

    public enum EnumAPISystemID
    {
        [Category("ERPAP"), DomainName(ERPAP)]
        ERPAP,
        [Category("ERPAP"), DomainName(LIBAP)]
        LIBAP,
        [Category("ERPAP"), DomainName(TKNAP)]
        TKNAP,
        [Category("ERPAP"), DomainName(WFAP)]
        WFAP,
        [Category("ERPAP"), DomainName(LOGAP)]
        LOGAP,
        [Category("ERPAP"), DomainName(PUSHAP)]
        PUSHAP,
        [Category("HTLAP"), DomainName(HTLAP)]
        HTLAP,
        [Category("HTLMRP"), DomainName(HTLMRP)]
        HTLMRP,
        [Category("BA2AP"), DomainName(BA2AP)]
        BA2AP,
        [Category("BA2AP"), DomainName(BA2PD)]
        BA2PD,
        [Category("BA2AP"), DomainName(BA2HC)]
        BA2HC,
        [Category("BA2AP"), DomainName(BA2EC)]
        BA2EC,
        [Category("BA2AP"), DomainName(BA2IA)]
        BA2IA,
        [Category("TKTAP"), DomainName(TKTAP)]
        TKTAP,
        [Category("TKTAP"), DomainName(TKTCAP)]
        TKTCAP,
        [Category("TKTAP"), DomainName(TKTMRP)]
        TKTMRP,
        [Category("TKTAP"), DomainName(TKTMGT)]
        TKTMGT,
        [Category("SCMAP"), DomainName(SCMAP)]
        SCMAP,
        [Category("AC2AP"), DomainName(AC2AP)]
        AC2AP,
        [Category("ETKTAP"), DomainName(ETKTAP)]
        ETKTAP,
        [Category("ETKTAP"), DomainName(ETKMRP)]
        ETKMRP,
        [Category("HCMAP"), DomainName(HCMAP)]
        HCMAP,
        [Category("HCMAP"), DomainName(AMAP)]
        AMAP,
        [Category("HCMAP"), DomainName(WCAP)]
        WCAP,
        [Category("DWHAP"), DomainName(DWHAP)]
        DWHAP,
        [Category("CRMAP"), DomainName(CRMAP)]
        CRMAP,
        [Category("CRMAP"), DomainName(SFAAP)]
        SFAAP,
        [Category("CRMAP"), DomainName(KAMAP)]
        KAMAP,
        [Category("BUSAP"), DomainName(BUSAP)]
        BUSAP,
        [Category("BUSAP"), DomainName(BUSMRP)]
        BUSMRP,
        [Category("TGUAP"), DomainName(TGUAP)]
        TGUAP,
        [Category("TGUAP"), DomainName(TGUMRP)]
        TGUMRP,
        [Category("TOURAP"), DomainName(TOURAP)]
        TOURAP,
        [Category("ACCAP"), DomainName(ACCAP)]
        ACCAP,
        [Category("QAMAP"), DomainName(QAMAP)]
        QAMAP,
        [Category("PUBAP"), DomainName(PUBAP)]
        PUBAP,
        [Category("GITAP"), DomainName(GITAP)]
        GITAP,
        [Category("GITAP"), DomainName(GITAAP)]
        GITAAP,
        [Category("GITAP"), DomainName(GITBAP)]
        GITBAP,
        [Category("GITAP"), DomainName(GITZAP)]
        GITZAP,
        [Category("GITAP"), DomainName(GITMRP)]
        GITMRP,
        [Category("ORDRAP"), DomainName(ORDRAP)]
        ORDRAP,
        [Category("FNAP"), DomainName(FNAP)]
        FNAP,
        [Category("FNAP"), DomainName(FNZZAP)]
        FNZZAP,
        [Category("FNAP"), DomainName(FNOTAP)]
        FNOTAP,
        [Category("FNAP"), DomainName(FNINAP)]
        FNINAP,
        [Category("FNAP"), DomainName(FNIOAP)]
        FNIOAP,
        [Category("FNAP"), DomainName(FNEXAP)]
        FNEXAP,
        [Category("RPMAP"), DomainName(RPMAP)]
        RPMAP,
        [Category("CBMAP"), DomainName(CBMAP)]
        CBMAP,
        [Category("CBMAP"), DomainName(CBMCA)]
        CBMCA,
        [Category("MCMAP"), DomainName(MCMCA)]
        MCMCA,
        [Category("ADSAP"), DomainName(ADSAP)]
        ADSAP,
        [Category("MKTAP"), DomainName(MKTAP)]
        MKTAP,
        [Category("MKTAP"), DomainName(MKTOUR)]
        MKTOUR,
        [Category("MELAP"), DomainName(MELAP)]
        MELAP,
        [Category("MELAP"), DomainName(MELMRP)]
        MELMRP,
        [Category("FITAP"), DomainName(FITAP)]
        FITAP,
        [Category("LOCAP"), DomainName(LOCAP)]
        LOCAP,
        [Category("LOCAP"), DomainName(LOCMRP)]
        LOCMRP,
        [Category("PDMAP"), DomainName(PDMAP)]
        PDMAP,
        [Category("PDMAP"), DomainName(TDPDM)]
        TDPDM,
        [Category("POIAP"), DomainName(POIAP)]
        POIAP,
        [Category("POIAP"), DomainName(POIMRP)]
        POIMRP,
        [Category("UPFIT"), DomainName(UPFIT)]
        UPFIT,
        [Category("WMP2AP"), DomainName(WMP2AP)]
        WMP2AP,
        [Category("MMSAP"), DomainName(MMSAP)]
        MMSAP,
        [Category("GITPCM"), DomainName(GITPCM)]
        GITPCM,
        [Category("CRSAP"), DomainName(CRSAP)]
        CRSAP,
        [Category("CRSAP"), DomainName(CRSMRP)]
        CRSMRP,
        [Category("MTMRP"), DomainName(MTMRP)]
        MTMRP,
        [Category("XFLAP"), DomainName(XFLAP)]
        XFLAP,
        [Category("CCSAP"), DomainName(CCSAP)]
        CCSAP,
        [Category("DTKTAP"), DomainName(DTKTAP)]
        DTKTAP,
        [Category("FINVCN"), DomainName(FINVCN)]
        FINVCN,
        [Category("VISAAP"), DomainName(VISAAP)]
        VISAAP,
        [Category("VISAAP"), DomainName(VISACN)]
        VISACN,
        [Category("PEUAP"), DomainName(PEUAP)]
        PEUAP,
        [Category("MCMAP"), DomainName(MCMAP)]
        MCMAP,
        [Category("GENAP"), DomainName(GENAP)]
        GENAP,
        [Category("MISAP"), DomainName(MISAP)]
        MISAP,
        [Category("CONSOLEAP"), DomainName(CONSOLEAP)]
        CONSOLEAP,
        [Category("CTMAP"), DomainName(CTMAP)]
        CTMAP,
        [Category("LXSCNAP"), DomainName(LXSCNAP)]
        LXSCNAP,
        [Category("PDM2AP"), DomainName(PDM2AP)]
        PDM2AP,
        [Category("PAYROLLAP"), DomainName(PAYROLLAP)]
        PAYROLLAP,
        [Category("PEUETKT"), DomainName(PEUETKT)]
        PEUETKT,
        [Category("RWAP"), DomainName(RWAP)]
        RWAP,
        [Category("TRAININGAP"), DomainName(TRAININGAP)]
        TRAININGAP,
        [Category("XINTUKUAP"), DomainName(XINTUKUAP)]
        XINTUKUAP,
        [Category("ERPAP"), DomainName(SYSMGTAP)]
        SYSMGTAP,
        [Category("COUPONAP"), DomainName(COUPONAP)]
        COUPONAP,
        [Category("CENTCTRLAP"), DomainName(CENTCTRLAP)]
        CENTCTRLAP,
        [Category("OLCSAP"), DomainName(OLCSAP)]
        OLCSAP,
        [Category("WEBPLATFORM"), DomainName(WEBPLATFORM)]
        WEBPLATFORM,
        [Category("ERPAP"), DomainName(ESIGNAP)]
        ESIGNAP,
        [Category("APIMGTAP"), DomainName(APIMGTAP)]
        APIMGTAP,
    }

    public enum EnumEDISystemID
    {
        [Description("LionTech.EDIService.ERP")]
        ERPAP,
        [Description("LionTech.EDIService.PUB")]
        PUBAP,
        [Description("LionTech.EDIService.BA2")]
        BA2AP,
        [Description("LionTech.EDIService.BUS")]
        BUSAP,
        [Description("LionTech.EDIService.AC2")]
        AC2AP,
        [Description("LionTech.EDIService.DWH")]
        DWHAP,
        [Description("LionTech.EDIService.CRM")]
        CRMAP,
        [Description("LionTech.EDIService.TKT")]
        TKTAP,
        [Description("LionTech.EDIService.HCM")]
        HCMAP,
        [Description("LionTech.EDIService.HTL")]
        HTLAP,
        [Description("LionTech.EDIService.HTLMRP")]
        HTLMRP,
        [Description("LionTech.EDIService.QAM")]
        QAMAP,
        [Description("LionTech.EDIService.TGU")]
        TGUAP,
        [Description("LionTech.EDIService.ADS")]
        ADSAP,
        [Description("LionTech.EDIService.PDM")]
        PDMAP,
        [Description("LionTech.EDIService.POI")]
        POIAP,
        [Description("LionTech.EDIService.MEL")]
        MELAP,
        [Description("LionTech.EDIService.LOC")]
        LOCAP,
        [Description("LionTech.EDIService.WMP2")]
        WMP2AP,
        [Description("LionTech.EDIService.TOUR")]
        TOURAP,
        [Description("LionTech.EDIService.GITPCM")]
        GITPCM,
        [Description("LionTech.EDIService.CRS")]
        CRSAP,
        [Description("LionTech.EDIService.RPM")]
        RPMAP,
        [Description("LionTech.EDIService.XFL")]
        XFLAP,
        [Description("LionTech.EDIService.CCS")]
        CCSAP,
        [Description("LionTech.EDIService.FIT")]
        FITAP,
        [Description("LionTech.EDIService.DTKT")]
        DTKTAP,
        [Description("LionTech.EDIService.FN")]
        FNAP,
        [Description("LionTech.EDIService.PEU")]
        PEUAP,
        [Description("LionTech.EDIService.MCM")]
        MCMAP,
        [Description("LionTech.EDIService.SCM")]
        SCMAP,
        [Description("LionTech.EDIService.GEN")]
        GENAP,
        [Description("LionTech.EDIService.MIS")]
        MISAP,
        [Description("LionTech.EDIService.CTM")]
        CTMAP,
        [Description("LionTech.EDIService.ORDR")]
        ORDRAP,
        [Description("LionTech.EDIService.MTMRP")]
        MTMRP,
        [Description("LionTech.EDIService.CONSOLE")]
        CONSOLEAP,
        [Description("LionTech.EDIService.CBM")]
        CBMAP,
        [Description("LionTech.EDIService.PAYROLL")]
        PAYROLLAP,
        [Description("LionTech.EDIService.ETKT")]
        ETKTAP,
        [Description("LionTech.EDIService.VISA")]
        VISAAP,
        [Description("LionTech.EDIService.PEUETKT")]
        PEUETKT,
        [Description("LionTech.EDIService.RW")]
        RWAP,
        [Description("LionTech.EDIService.TRAINING")]
        TRAININGAP,
        [Description("LionTech.EDIService.XINTUKU")]
        XINTUKUAP,
        [Description("LionTech.EDIService.COUPON")]
        COUPONAP,
        [Description("LionTech.EDIService.CENTCTRL")]
        CENTCTRLAP,
        [Description("LionTech.EDIService.WEBPLATFORM")]
        WEBPLATFORM,
        [Description("LionTech.EDIService.APIMGT")]
        APIMGTAP,
    }

    public enum EnumExApiURL
    {
        [ApiUrl(TokenGenerator)]
        TokenGenerator,
        [ApiUrl(AuthWithOTP)]
        AuthWithOTP
    }

    public enum EnumEDIStatusID
    {
        B, F, W
    }

    public enum EnumEDIResultStatusID
    {
        S, F
    }

    public enum EnumEDIResultID
    {
        Z9999
    }

    public enum EnumEDIFlowID
    {
        Event
    }

    #region - WorkFlow -
    public enum EnumWFNodeTypeID
    {
        S,
        P,
        D,
        E
    }

    public enum EnumWFResultID
    {
        P,
        B,
        C,
        F
    }

    public enum EnumWFNodeResultID
    {
        P,
        N,
        B,
        C,
        F
    }

    public enum EnumWFNodeSignatureResultID
    {
        P,
        A,
        R
    }

    public enum EnumSignatureTypeID
    {
        I,
        C,
        A,
        D,
        R
    }

    public enum EnumWFStsType
    {
        IS_APPLYING,
        IS_SIG_STARTING,
        IS_SIGNATUE,
        IS_SIG_ALL_ACCEPT,
        IS_SIG_REJECT,
        IS_CANCEL_URSELF,
        NONE
    }
    #endregion
}
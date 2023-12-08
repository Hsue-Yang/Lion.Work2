using System.ComponentModel;

namespace LionTech.Entity.B2P
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
    }

    public enum EnumSystemID
    {
        #region Developing
        [Category("Domain"), DomainName(Domain)]
        Domain,
        [Category("B2PAP"), DomainName(B2PAP)]
        B2PAP,
        [Category("HTLB2P"), DomainName(HTLB2P)]
        HTLB2P,
        [Category("QCB2P"), DomainName(QCB2P)]
        QCB2P,
        [Category("SCMB2P"), DomainName(SCMB2P)]
        SCMB2P,
        [Category("RPMB2P"), DomainName(RPMB2P)]
        RPMB2P,
        [Category("BUSB2P"), DomainName(BUSB2P)]
        BUSB2P,
        [Category("TGUB2P"), DomainName(TGUB2P)]
        TGUB2P
        #endregion

        #region Testing
        //[Category("Domain"), Description("liontravel.com.tw")]
        //Domain,
        //[Category("B2PAP"), Description("http://ub2p.liontravel.com.tw")]
        //B2PAP,
        //[Category("HTLB2P"), Description("http://uhotelb2p.liontravel.com.tw")]
        //HTLB2P,
        //[Category("QCB2P"), Description("http://uqcb2p.liontravel.com.tw")]
        //QCB2P,
        //[Category("SCMB2P"), Description("http://uscmb2p.liontravel.com.tw")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("http://urpmb2p.liontravel.com.tw")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("http://ubusb2p.liontravel.com.tw")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("http://utgub2p.liontravel.com.tw")]
        //TGUB2P
        #endregion

        #region Production
        //[Category("Domain"), Description("liontravel.com")]
        //Domain,
        //[Category("B2PAP"), Description("https://b2p.liontravel.com")]
        //B2PAP,
        //[Category("HTLB2P"), Description("https://hotelb2p.liontravel.com")]
        //HTLB2P,
        //[Category("QCB2P"), Description("https://qcb2p.liontravel.com")]
        //QCB2P,
        //[Category("SCMB2P"), Description("https://scmb2p.liontravel.com")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("https://rpmb2p.liontravel.com")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("https://busb2p.liontravel.com")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("https://tgub2p.liontravel.com")]
        //TGUB2P
        #endregion
		
        #region AZ Testing
        //[Category("Domain"), Description("liontravel.com.tw")]
        //Domain,
        //[Category("B2PAP"), Description("http://tub2p.liontravel.com.tw")]
        //B2PAP,
        //[Category("HTLB2P"), Description("http://tuhotelb2p.liontravel.com.tw")]
        //HTLB2P,
        //[Category("QCB2P"), Description("http://tuqcb2p.liontravel.com.tw")]
        //QCB2P,
        //[Category("SCMB2P"), Description("http://tuscmb2p.liontravel.com.tw")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("http://turpmb2p.liontravel.com.tw")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("http://tubusb2p.liontravel.com.tw")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("http://tutgub2p.liontravel.com.tw")]
        //TGUB2P
        #endregion

        #region AZ Production
        //[Category("Domain"), Description("liontravel.com")]
        //Domain,
        //[Category("B2PAP"), Description("https://tb2p.liontravel.com")]
        //B2PAP,
        //[Category("HTLB2P"), Description("https://thotelb2p.liontravel.com")]
        //HTLB2P,
        //[Category("QCB2P"), Description("https://tqcb2p.liontravel.com")]
        //QCB2P,
        //[Category("SCMB2P"), Description("https://tscmb2p.liontravel.com")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("https://trpmb2p.liontravel.com")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("https://tbusb2p.liontravel.com")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("https://ttgub2p.liontravel.com")]
        //TGUB2P
        #endregion
    }

    public enum EnumAPISystemID
    {
        #region Developing
        [Category("B2PAP"), DomainName(B2PAP)]
        B2PAP,
        [Category("HTLB2P"), DomainName(HTLB2P)]
        HTLB2P,
        [Category("QCB2P"), DomainName(QCB2P)]
        QCB2P,
        [Category("SCMB2P"), DomainName(SCMB2P)]
        SCMB2P,
        [Category("RPMB2P"), DomainName(RPMB2P)]
        RPMB2P,
        [Category("BUSB2P"), DomainName(BUSB2P)]
        BUSB2P,
        [Category("TGUB2P"), DomainName(TGUB2P)]
        TGUB2P
        #endregion

        #region Testing
        //[Category("B2PAP"), Description("http://uscminapi.liontravel.com.tw")]
        //B2PAP,
        //[Category("HTLB2P"), Description("http://uhotelinapi.liontravel.com.tw")]
        //HTLB2P,
        //[Category("QCB2P"), Description("http://uscminapi.liontravel.com.tw")]
        //QCB2P,
        //[Category("SCMB2P"), Description("http://uscminapi.liontravel.com.tw")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("http://urpminapi.liontravel.com.tw")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("http://ubusinapi.liontravel.com.tw")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("http://utguinapi.liontravel.com.tw")]
        //TGUB2P
        #endregion

        #region Production
        //[Category("B2PAP"), Description("https://scminapi.liontravel.com")]
        //B2PAP,
        //[Category("HTLB2P"), Description("https://hotelinapi.liontravel.com")]
        //HTLB2P,
        //[Category("QCB2P"), Description("https://scminapi.liontravel.com")]
        //QCB2P,
        //[Category("SCMB2P"), Description("https://scminapi.liontravel.com")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("https://rpminapi.liontravel.com")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("https://businapi.liontravel.com")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("https://tguinapi.liontravel.com")]
        //TGUB2P
        #endregion
		
        #region AZ Testing
        //[Category("B2PAP"), Description("http://tuscminapi.liontravel.com.tw")]
        //B2PAP,
        //[Category("HTLB2P"), Description("http://tuhotelinapi.liontravel.com.tw")]
        //HTLB2P,
        //[Category("QCB2P"), Description("http://tuscminapi.liontravel.com.tw")]
        //QCB2P,
        //[Category("SCMB2P"), Description("http://tuscminapi.liontravel.com.tw")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("http://turpminapi.liontravel.com.tw")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("http://tubusinapi.liontravel.com.tw")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("http://tutguinapi.liontravel.com.tw")]
        //TGUB2P
        #endregion

        #region AZ Production
        //[Category("B2PAP"), Description("https://tscminapi.liontravel.com")]
        //B2PAP,
        //[Category("HTLB2P"), Description("https://thotelinapi.liontravel.com")]
        //HTLB2P,
        //[Category("QCB2P"), Description("https://tscminapi.liontravel.com")]
        //QCB2P,
        //[Category("SCMB2P"), Description("https://tscminapi.liontravel.com")]
        //SCMB2P,
        //[Category("RPMB2P"), Description("https://trpminapi.liontravel.com")]
        //RPMB2P,
        //[Category("BUSB2P"), Description("https://tbusinapi.liontravel.com")]
        //BUSB2P,
        //[Category("TGUB2P"), Description("https://ttguinapi.liontravel.com")]
        //TGUB2P
        #endregion
    }

    public enum EnumEDISystemID
    {
        [Description("LionTech.EDIService.B2P")]
        B2PAP,
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
}
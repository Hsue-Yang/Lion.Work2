using System.ComponentModel;

namespace LionTech.Entity.TRAINING
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
        ja_JP
    }

    public enum EnumDataRowCount
    {
        RowAll = 0,
        Row100 = 100
    }
}
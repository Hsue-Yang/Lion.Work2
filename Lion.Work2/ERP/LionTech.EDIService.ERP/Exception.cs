using System;
using System.ComponentModel;

using LionTech.Utility;

namespace LionTech.EDIService
{
    public enum EnumEDIServiceMessage
    {
        [Description("無法取得設定檔中的根目錄路徑。appSetting key: {0}")]
        RootPathNull,
        [Description("無法取得資料庫連線字串。appSetting key: {0}")]
        ConnectionStringNull,

        [Description("流程已在執行中，或上次為不正常結束（run檔已存在）。Flow ID: {0}")]
        FlowRunFileExist,
        [Description("流程不在執行中（run檔不存在）。Flow ID: {0}")]
        FlowRunFileNotExist,
        [Description("流程執行頻率設定錯誤。Frequency: {0}")]
        ScheduleFrequencyError
    }

    public class EDIServiceException : Exception
    {
        EnumEDIServiceMessage _ediServiceMessage;
        public EnumEDIServiceMessage EDIServiceMessage
        {
            get { return _ediServiceMessage; }
        }

        public EDIServiceException(EnumEDIServiceMessage ediServiceMessage)
            : base(Common.GetEnumDesc(ediServiceMessage))
        {
            _ediServiceMessage = ediServiceMessage;
        }

        public EDIServiceException(EnumEDIServiceMessage ediServiceMessage, string[] messageParameters)
            : base(string.Format(Common.GetEnumDesc(ediServiceMessage), messageParameters))
        {
            _ediServiceMessage = ediServiceMessage;
        }
    }
}

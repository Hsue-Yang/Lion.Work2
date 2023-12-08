using System;
using System.ComponentModel;

namespace LionTech.Utility.B2P
{
    public enum EnumUtilityB2PMessage
    {
        [Description("B2P功能清單檔案不存在。Value: {0}")]
        MenuIsNotExist,
        [Description("B2P功能清單讀取資料錯誤。")]
        MenuContentError
    }

    public class UtilityB2PException : Exception
    {
        EnumUtilityB2PMessage _utilityB2PMessage;
        public EnumUtilityB2PMessage UtilityB2PMessage
        {
            get { return _utilityB2PMessage; }
        }

        public UtilityB2PException(EnumUtilityB2PMessage utilityB2PMessage)
            : base(Common.GetEnumDesc(utilityB2PMessage))
        {
            _utilityB2PMessage = utilityB2PMessage;
        }

        public UtilityB2PException(EnumUtilityB2PMessage utilityB2PMessage, string[] messageParameters)
            : base(string.Format(Common.GetEnumDesc(utilityB2PMessage), messageParameters))
        {
            _utilityB2PMessage = utilityB2PMessage;
        }
    }
}

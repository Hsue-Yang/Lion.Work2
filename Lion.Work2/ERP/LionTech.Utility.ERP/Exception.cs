using System;
using System.ComponentModel;

namespace LionTech.Utility.ERP
{
    public enum EnumUtilityERPMessage
    {
        [Description("ERP功能清單檔案不存在。Value: {0}")]
        MenuIsNotExist,
        [Description("ERP功能清單讀取資料錯誤。")]
        MenuContentError
    }

    public class UtilityERPException : Exception
    {
        EnumUtilityERPMessage _utilityERPMessage;
        public EnumUtilityERPMessage UtilityERPMessage
        {
            get { return _utilityERPMessage; }
        }

        public UtilityERPException(EnumUtilityERPMessage utilityERPMessage)
            : base(Common.GetEnumDesc(utilityERPMessage))
        {
            _utilityERPMessage = utilityERPMessage;
        }

        public UtilityERPException(EnumUtilityERPMessage utilityERPMessage, string[] messageParameters)
            : base(string.Format(Common.GetEnumDesc(utilityERPMessage), messageParameters))
        {
            _utilityERPMessage = utilityERPMessage;
        }
    }
}
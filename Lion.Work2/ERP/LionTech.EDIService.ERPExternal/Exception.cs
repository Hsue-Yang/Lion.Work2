using System;
using System.ComponentModel;

using LionTech.Utility;

namespace LionTech.EDIService.ERPExternal
{
    public enum EnumERPExternalMessage
    {
        [Description("無法取得回傳狀態。response: {0}")]
        HttpWebRequestGetResponseStringError
    }

    public class ERPExternalException : Exception
    {
        EnumERPExternalMessage _erpExternalMessage;
        public EnumERPExternalMessage ERPExternalMessage
        {
            get { return _erpExternalMessage; }
        }

        public ERPExternalException(EnumERPExternalMessage erpExternalMessage)
            : base(Common.GetEnumDesc(erpExternalMessage))
        {
            _erpExternalMessage = erpExternalMessage;
        }

        public ERPExternalException(EnumERPExternalMessage erpExternalMessage, string[] messageParameters)
            : base(string.Format(Common.GetEnumDesc(erpExternalMessage), messageParameters))
        {
            _erpExternalMessage = erpExternalMessage;
        }
    }
}

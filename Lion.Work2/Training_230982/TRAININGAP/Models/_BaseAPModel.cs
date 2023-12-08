using System.ComponentModel;

namespace TRAININGAP.Models
{
    public class _BaseAPModel : _BaseModel
    {
        public enum EnumTabController
        {

        }

        public enum EnumTabAction
        {

        }

        public enum EnumCookieKey
        {

        }

        public enum EnumSysFunToolNo
        {
            [Description("000001")]
            DefaultNo,
            [Description("000002")]
            FirstNo
        }
    }
}
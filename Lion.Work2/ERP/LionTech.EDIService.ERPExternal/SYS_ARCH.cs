using System.ComponentModel;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SYS_ARCH
    {
        private static string UrlFormat = "{0}/Demo/{1}TestAction";
        private static string FileNameFormat = "{0}.{1}.log";
        private static string LogPath = string.Empty;
        private static string ExceptionPath = string.Empty;

        private enum EnumRequestStatus
        {
            [Description("[Application system warm-up complete]")]
            ResponseSuccess,
            [Description("[Application system warm-up failed, response status Abnormal]")]
            ResponseAbnormal,
            [Description("[Application system warm-up failed, page request error]")]
            RequestError
        }
    }
}
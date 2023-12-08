using System.ComponentModel;

namespace ERPAPI.Models.SystemSetting
{
    public class SystemIconModel : SystemSettingModel
    {
        public enum EnumTargetPath
        {
            [Description(@"APIUpload\SystemIcon\SystemIconUploadEvent\")]
            SysIconUploadPath
        }

        #region API Property
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        
        public string APIPara { get; set; }
        #endregion

        public class APIParaData
        {
            public string UsedOnIIS { get; set; }
            public string TargetPath { get; set; }
            public string FileName { get; set; }
            public string FileExtension { get; set; }
            public string TargetFilePath { get; set; }
        }

        public APIParaData APIData { get; set; }
    }
}
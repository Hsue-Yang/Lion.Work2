namespace ERPAP.Models.Sys
{
    public class SystemEventTargetLogModel : SysModel
    {
        #region - Enum -
        public enum EDIFlowID
        {
            SUBS
        }
        #endregion

        public string EDIDate { get; set; }

        public string EDITime { get; set; }

        public string EDIFilePath { get; set; }

        public string EDIFile { get; set; }
    }
}
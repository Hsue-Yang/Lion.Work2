using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SysUserPurview
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string PurviewID { get; set; }
        public string PurviewNM { get; set; }
        public bool HasDataPurAuth { get; set; }
        public bool HasUpdateAuth { get; set; }
    }
    public class SysUserPurviewDetails
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string PurviewNM { get; set; }
        public string PurviewID { get; set; }
        public string CodeType { get; set; }
        public string CodeID { get; set; }
        public string PurviewOP { get; set; }
        public bool? HasDataPur { get; set; }
    }

    public class PurviewName
    {
        public string PurviewID { get; set; }
        public string PurviewNM { get; set; }
    }

    public class UserPurviewPara
    {
        public string SysID { get; set; }
        public string UserID { get; set; }
        public string UpdUserID { get; set; }
        public List<SysUserPurviewDetails> SysUserPurviewDetailList { get; set; }
    }
}
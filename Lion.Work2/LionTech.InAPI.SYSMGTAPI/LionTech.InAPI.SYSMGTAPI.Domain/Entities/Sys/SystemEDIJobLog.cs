using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEDIJobLog
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string EDINO { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
        public string EDIJobID { get; set; }
        public string EDIJobNM { get; set; }
        public string StatusID { get; set; }
        public string EDIJobStatusNM { get; set; }
        public string ResultID { get; set; }
        public string EDIJobResultNM { get; set; }
        public string ResultCode { get; set; }
        public DateTime DTBegin { get; set; }
        public DateTime DTEnd { get; set; }
        public int CountRow { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDt { get; set; }
    }
}

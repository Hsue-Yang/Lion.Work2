using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEDIFlow
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
        public string SCHFrequency { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }

   
    public class SystemEDIFlowSort
    {
        public string SysID { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public string EDIFlowID { get; set; }
    }


    public class SystemEDIFlowDetail
    {
        public SystemEDIFlowDetails SystemEDIFlowDetails { get; set; }

        public List<SystemEDIFixEDTime> SystemEDIFixEDTime { get; set; }
    }

    public class SystemEDIFlowDetails
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }

        public string EDIFlowNM { get; set; }

        public string EDIFlowZHTW { get; set; }

        public string EDIFlowZHCN { get; set; }

        public string EDIFlowENUS { get; set; }

        public string EDIFlowTHTH { get; set; }

        public string EDIFlowJAJP { get; set; }

        public string EDIFlowKOKR { get; set; }

        public string SCHFrequency { get; set; }

        public string SCHStartDate { get; set; }

        public string SCHStartTime { get; set; }

        public Nullable<int> SCHIntervalNum { get; set; }

        public Nullable<int> SCHIntervalTime { get; set; }

        public string SCHEndTime { get; set; }
        public Nullable<int> SCHWeeks { get; set; }
        public string SCHDaysStr { get; set; }
        public string SCHDataDelay { get; set; }
        public string SCHKeepLogDay { get; set; }
        public string PATHSCmd { get; set; }

        public string PATHSDat { get; set; }

        public string PATHSSrc { get; set; }

        public string PATHSRes { get; set; }

        public string PATHSBad { get; set; }

        public string PATHSLog { get; set; }

        public string PATHSFlowXml { get; set; }

        public string PATHSFlowCmd { get; set; }

        public string PATHSZipDat { get; set; }

        public string PATHSException { get; set; }

        public string PATHSSummary { get; set; }

        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public string ExeCuteTIME { get; set; }
        public List<string> ExecuteTimeList { get; set; }
    }


    public class SystemEDIFixEDTime
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }
        public string ExeCuteTIME { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemEDIFlowSchedule
    {
        public string EDIFlowNM { get; set; }
        public string SCHFrequency { get; set; }
        public string SCHStartTime { get; set; }
        public Nullable<int> SCHIntervalNum { get; set; }
        public Nullable<int> SCHIntervalTime { get; set; }
        public string SCHEndTime { get; set; }
        public string ExecuteTime { get; set; }
    }

    public class SystemEDIFlowByIds
    {
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
    }

    public class SystemEDIFlowExecuteTime
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }
        public string ExecuteTime { get; set; }
    }

    public class SystemEDIXML
    {
        public List<SystemEDIFlowDetails> SystemEDIFlowDetails { get; set; }
        public List<SystemEDIJob> SystemEDIJobDetails { get; set; }
        public List<SystemEDICon> SystemEDIConDetails { get; set; }
        public List<SystemEDIPara> SystemEDIParDetails { get; set; }
        public List<SystemEDIFlowExecuteTime> SystemEDIFlowExecuteTimeDetails { get; set; }
    }
}

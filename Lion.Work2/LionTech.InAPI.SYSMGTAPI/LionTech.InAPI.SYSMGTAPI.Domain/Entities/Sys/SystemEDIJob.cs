using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEDIJob
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
        public string EDIJobID { get; set; }
        public string EDIJobNM { get; set; }
        public string EDIJobZHTW { get; set; }
        public string EDIJobZHCN { get; set; }
        public string EDIJobENUS { get; set; }
        public string EDIJobTHTH { get; set; }
        public string EDIJobJAJP { get; set; }
        public string EDIJobKOKR { get; set; }
        public string EDIJobType { get; set; }
        public string EDIConID { get; set; }
        public string ObjectName { get; set; }
        public string DepEDIJobID { get; set; }
        public string IsUseRes { get; set; }
        public string IgnoreWarning { get; set; }
        public string IsDisable { get; set; }
        public string FileSource { get; set; }
        public string FileEncoding { get; set; }
        public string URLPath { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }

    public class SystemDepEDIJobID
    {
        public string EDIJobID { get; set; }
        public string EDIJobNM { get; set; }
    }

    public class EditEDIJobValue
    {
        public string SysID { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public string EDIJobID { get; set; }
        public string EDIFlowID { get; set; }
    }

    public class EdiJobSettingPara
    {
        public List<EditEDIJobDetail> EDIJobDetailEditList { get; set; }
        public List<EditEDIJobPara> EDIJobParaEditList { get; set; }

        public EdiJobSettingPara()
        {
            EDIJobDetailEditList = new List<EditEDIJobDetail>();
            EDIJobParaEditList = new List<EditEDIJobPara>();
        }
    }

    public class EditEDIJobDetail
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIJobID { get; set; }
        public string EDIJobZHTW { get; set; }
        public string EDIJobZHCN { get; set; }
        public string EDIJobENUS { get; set; }
        public string EDIJobTHTH { get; set; }
        public string EDIJobJAJP { get; set; }
        public string EDIJobKOKR { get; set; }
        public string EDIJobType { get; set; }
        public string EDIConID { get; set; }
        public string ObjectName { get; set; }
        public string DepEDIJobID { get; set; }
        public string IsUseRes { get; set; }
        public string IgnoreWarning { get; set; }
        public string IsDisable { get; set; }
        public string FileSource { get; set; }
        public string FileEncoding { get; set; }
        public string URLPath { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }

    public class EditEDIJobPara
    {
        public string SysID { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIJobID { get; set; }
        public string EDIJobParaID { get; set; }
        public string EDIJobParaType { get; set; }
        public string EDIJobParaValue { get; set; }
    }
}

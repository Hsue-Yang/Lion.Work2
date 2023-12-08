using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemEDIJobConveyModel : SysModel
    {
        public enum JobConveyCase
        {
            caseCountEqual0 = 0,
            caseCountOver = 1,
            caseCountRegular = 2
        }

        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        [AllowHtml]
        [Required]
        [InputType(EnumInputType.TextBox)]
        [StringLength(4000)]
        public string Content { get; set; }

        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIJobConvey.TabText_SystemEDIJobConvey,
                ImageURL=string.Empty
            }
        };

        public SystemEDIJobConveyModel()
        {

        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryEDIFlowID = string.Empty;
        }

        public int GetEDIJobSettingResult(string userID, EnumCultureID cultureID)
        {
            Flow flow = new Flow
            {
                id = string.Empty,
                description = string.Empty,
                schedule = new Schedule(),
                paths = new Paths(),
                connections = new Connections(),
                jobs = new Jobs(),
                ediNo = string.Empty,
                ediDate = string.Empty,
                ediTime = string.Empty,
                dataDate = string.Empty,
                result = EnumFlowResult.None
            };
            int jobSortOrder;
            int totalCount = 0;
            Content = RevertString(Content);
            Content = string.Concat("<jobs>", Content,"</jobs>");
            try
            {
                EntitySystemEDIJobConvey.SystemEDIFlowPara para = new EntitySystemEDIJobConvey.SystemEDIFlowPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };
                flow = GetFlowData(para, flow);
                jobSortOrder = GetJobMaxSortOrder(para) + 100;
                flow.jobs = Job.FromString(Content, flow);
                foreach (Job job in flow.jobs)
                {
                    totalCount = totalCount + job.parameters.Count + 1;
                }
                if (totalCount == 0)
                {
                    return Convert.ToInt32(JobConveyCase.caseCountEqual0);
                }
                else if (totalCount > 10)
                {
                    return Convert.ToInt32(JobConveyCase.caseCountOver);
                }
                else if (new EntitySystemEDIJobConvey(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EdiJobSetting(para, flow, jobSortOrder) == EntitySystemEDIJobConvey.EnumEDIJobSettingResult.Success)
                {
                    return Convert.ToInt32(JobConveyCase.caseCountRegular);
                }
                return totalCount;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return totalCount;
        }

        public Flow GetFlowData(EntitySystemEDIJobConvey.SystemEDIFlowPara para, Flow flow)
        {
            try
            {
                flow = new EntitySystemEDIJobConvey(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIConData(flow, para);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
               
            }
            return flow;
        }

        public int GetJobMaxSortOrder(EntitySystemEDIJobConvey.SystemEDIFlowPara para)
        {
            int jobMaxSortOrder = 0;
            try
            {
                jobMaxSortOrder = new EntitySystemEDIJobConvey(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .GetJobMaxSortOrder(para);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
               
            }
            return jobMaxSortOrder;
        }

        public string RevertString(string content)
        {
            content = content.Replace("&LT;", "<");
            content = content.Replace("&RT;", ">");
            return content;
        }
    }
}
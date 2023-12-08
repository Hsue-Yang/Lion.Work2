using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemEDIJobModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID, QueryEDIJobID
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }
        
        public SystemEDIJobModel()
        {

        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryEDIFlowID = string.Empty;
        }

        List<EntitySystemEDIJob.SystemEDIJob> _entitySystemEDIJobList;
        public List<EntitySystemEDIJob.SystemEDIJob> EntitySystemEDIJobList { get { return _entitySystemEDIJobList; } }

        public bool GetSystemEDIJobList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIJob.SystemEDIJobPara para = new EntitySystemEDIJob.SystemEDIJobPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID))
                };

                _entitySystemEDIJobList = new EntitySystemEDIJob(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIJobList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEDIJobSettingResult(string userID, EnumCultureID cultureID, List<EntitySystemEDIJob.EDIJobValue> EDIJobValueList)
        {
            try
            {
                EntitySystemEDIJob.SystemEDIJobPara para = new EntitySystemEDIJob.SystemEDIJobPara(cultureID.ToString())
                {
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID))
                };

                if (new EntitySystemEDIJob(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditEDIJobSetting(para, EDIJobValueList) == EntitySystemEDIJob.EnumEDIJobSettingResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
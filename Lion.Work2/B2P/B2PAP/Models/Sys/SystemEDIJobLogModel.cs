using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class SystemEDIJobLogModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID, QueryEDIJobID, DataDate, EDIDate, EDINO
        }

        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }
        
        public string QueryEDIJobID { get; set; }

        [Required]
        public string DataDate { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string EDIDate { get; set; }

        [StringLength(12, MinimumLength = 0)]
        public string EDINO { get; set; }

        public SystemEDIJobLogModel()
        {

        }

        public void FormReset()
        {
            this.EDIDate = DateTime.Now.ToString("yyyyMMdd");
        }

        List<EntitySystemEDIJobLog.SystemEDIJobLog> _entitySystemEDIJobLogList;
        public List<EntitySystemEDIJobLog.SystemEDIJobLog> EntitySystemEDIJobLogList { get { return _entitySystemEDIJobLogList; } }

        public bool GetSystemEDIJobLogList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIJobLog.SystemEDIJobLogPara para = new EntitySystemEDIJobLog.SystemEDIJobLogPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIJobID) ? null : this.QueryEDIJobID)),
                    EDINO = new DBChar((string.IsNullOrWhiteSpace(this.EDINO) ? null : this.EDINO)),
                    EDIDate = new DBChar((string.IsNullOrWhiteSpace(this.EDIDate) ? null : this.EDIDate))
                };

                _entitySystemEDIJobLogList = new EntitySystemEDIJobLog(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIJobLogList(para);

                if (_entitySystemEDIJobLogList != null)
                {
                    _entitySystemEDIJobLogList = base.GetEntitysByPage(_entitySystemEDIJobLogList, pageSize);
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
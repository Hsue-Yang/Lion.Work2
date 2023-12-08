using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class SystemEDIFlowLogModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID, DataDate, EDIDate, EDINO
        }

        [Required]
        public string QuerySysID { get; set; }
        public string QueryEDIFlowID { get; set; }

        [StringLength(12, MinimumLength = 12)]
        public string EDINO { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string DataDate { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string EDIDate { get; set; }

        List<EntitySystemEDIFlowLog.StatusID> _entityStatusIDList = new List<EntitySystemEDIFlowLog.StatusID>();
        public List<EntitySystemEDIFlowLog.StatusID> EntityStatusIDList { get { return _entityStatusIDList; } }

        public bool GetStatusIDList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIFlowLog.StatusIDPara para =
                    new EntitySystemEDIFlowLog.StatusIDPara(cultureID.ToString());

                _entityStatusIDList = new EntitySystemEDIFlowLog(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectStatusIDList(para);

                if (_entityStatusIDList != null)
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

        public SystemEDIFlowLogModel()
        {

        }

        public void FormReset()
        {
            this.EDIDate = DateTime.Now.ToString("yyyyMMdd");

        }

        List<EntitySystemEDIFlowLog.SystemEDIFlowLog> _entitySystemEDIFlowLogList;
        public List<EntitySystemEDIFlowLog.SystemEDIFlowLog> EntitySystemEDIFlowLogList { get { return _entitySystemEDIFlowLogList; } }

        public bool GetSystemEDIFlowLogList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIFlowLog.SystemEDIFlowLogPara para = new EntitySystemEDIFlowLog.SystemEDIFlowLogPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    EDIDate = new DBChar((string.IsNullOrWhiteSpace(this.EDIDate) ? null : this.EDIDate)),
                    DataDate = new DBChar((string.IsNullOrWhiteSpace(this.DataDate) ? null : this.DataDate)),
                    EDINO = new DBChar((string.IsNullOrWhiteSpace(this.EDINO) ? null : this.EDINO))
                };

                _entitySystemEDIFlowLogList = new EntitySystemEDIFlowLog(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIFlowLogList(para);

                if (_entitySystemEDIFlowLogList != null)
                {
                    _entitySystemEDIFlowLogList = base.GetEntitysByPage(_entitySystemEDIFlowLogList, pageSize);
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
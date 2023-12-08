using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemTokenClientLogModel : SysModel
    {
        public enum Field {
            QueryUserID, GenDateBegin, GenDateEnd, GenTimeBegin, GenTimeEnd, QueryTokenNo, QuerySysNM
        }

        
        public string QueryUserID{get;set;}

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string GenDateBegin { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string GenDateEnd { get; set; }

        [Required]
        public string GenTimeBegin { get; set; }

        [Required]
        public string GenTimeEnd { get; set; }

        public string QueryTokenNo { get; set; }
        public string QueryDeviceIP { get; set; }
        public string QuerySysNM { get; set; }

        public Dictionary<string, string> GenBeginTimeList = new Dictionary<string, string>()
        {
            {"0000000", "00:00"},
            {"0100000", "01:00"},
            {"0200000", "02:00"},
            {"0300000", "03:00"},
            {"0400000", "04:00"},
            {"0500000", "05:00"},
            {"0600000", "06:00"},
            {"0700000", "07:00"},
            {"0800000", "08:00"},
            {"0900000", "09:00"},
            {"1000000", "10:00"},
            {"1100000", "11:00"},
            {"1200000", "12:00"},
            {"1300000", "13:00"},
            {"1400000", "14:00"},
            {"1500000", "15:00"},
            {"1600000", "16:00"},
            {"1700000", "17:00"},
            {"1800000", "18:00"},
            {"1900000", "19:00"},
            {"2000000", "20:00"},
            {"2100000", "21:00"},
            {"2200000", "22:00"},
            {"2300000", "23:00"}
        };

        public Dictionary<string, string> GenEndTimeList = new Dictionary<string, string>()
        {
            {"0059999", "00:59"},
            {"0159999", "01:59"},
            {"0259999", "02:59"},
            {"0359999", "03:59"},
            {"0459999", "04:59"},
            {"0559999", "05:59"},
            {"0659999", "06:59"},
            {"0759999", "07:59"},
            {"0859999", "08:59"},
            {"0959999", "09:59"},
            {"1059999", "10:59"},
            {"1159999", "11:59"},
            {"1259999", "12:59"},
            {"1359999", "13:59"},
            {"1459999", "14:59"},
            {"1559999", "15:59"},
            {"1659999", "16:59"},
            {"1759999", "17:59"},
            {"1859999", "18:59"},
            {"1959999", "19:59"},
            {"2059999", "20:59"},
            {"2159999", "21:59"},
            {"2259999", "22:59"},
            {"2359999", "23:59"}
        };
        
        public void FormReset()
        {
            this.GenDateBegin = Common.GetDateString();
            this.GenTimeBegin = "0000000";
            this.GenDateEnd = Common.GetDateString();
            this.GenTimeEnd = "2359999";
        }
        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemTokenClientLog.TabTokenClientLog,
                ImageURL=string.Empty
            }
        };

        List<EntitySystemTokenClientLog.TokenClientLog> _entitySystemTokenClientLogList;
        public List<EntitySystemTokenClientLog.TokenClientLog> EntitySystemTokenClientLogList { get { return _entitySystemTokenClientLogList; } }
        public bool GetTokenClientLogList(int pageSize, EnumCultureID cultureID)
        {
            try {
                EntitySystemTokenClientLog.TokenClientLogPara para = new EntitySystemTokenClientLog.TokenClientLogPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    GenerateDTBegin = new DBChar(this.GenDateBegin + this.GenTimeBegin),
                    GenerateDTEnd = new DBChar(this.GenDateEnd + this.GenTimeEnd),
                    TokenNo = new DBChar(this.QueryTokenNo),
                    DeviceID = new DBVarChar(this.QueryDeviceIP),
                    SysID = new DBVarChar(this.QuerySysNM)
                };

                _entitySystemTokenClientLogList = new EntitySystemTokenClientLog(this.ConnectionStringSERP, this.ProviderNameSERP).SelectTokenClientLog(para);
                if (_entitySystemTokenClientLogList != null)
                {
                    _entitySystemTokenClientLogList = base.GetEntitysByPage(_entitySystemTokenClientLogList, pageSize);
                }
                return true;
            }
            catch (Exception ex) {
                base.OnException(ex);
            }
            return false;
        }
    }
}
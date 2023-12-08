using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemEDIConModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }

        public SystemEDIConModel()
        {

        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryEDIFlowID = string.Empty;
        }

        List<EntitySystemEDICon.SystemEDICon> _entitySystemEDIConList;
        public List<EntitySystemEDICon.SystemEDICon> EntitySystemEDIConList { get { return _entitySystemEDIConList; } }

        public bool GetSystemEDIConList( EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDICon.SystemEDIConPara para = new EntitySystemEDICon.SystemEDIConPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID))
                };

                _entitySystemEDIConList = new EntitySystemEDICon(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIConList(para);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEDIConSettingResult(string userID, EnumCultureID cultureID, List<EntitySystemEDICon.EDIConValue> EDIConValueList)
        {
            try
            {
                EntitySystemEDICon.SystemEDIConPara para = new EntitySystemEDICon.SystemEDIConPara(cultureID.ToString())
                {
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemEDICon(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditEDIConSetting(para, EDIConValueList) == EntitySystemEDICon.EnumEDIConSettingResult.Success)
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
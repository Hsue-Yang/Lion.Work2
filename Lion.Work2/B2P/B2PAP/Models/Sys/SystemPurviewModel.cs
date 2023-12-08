using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemPurviewModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryPurviewID
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryPurviewID { get; set; }

        public SystemPurviewModel()
        {
        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryPurviewID = string.Empty;
        }

        List<EntitySystemPurview.SystemPurview> _entitySystemPurviewList;
        public List<EntitySystemPurview.SystemPurview> EntitySystemPurviewList { get { return _entitySystemPurviewList; } }

        public bool GetSystemPurviewList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemPurview.SystemPurviewPara para = new EntitySystemPurview.SystemPurviewPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    PurviewID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryPurviewID) ? null : this.QueryPurviewID))
                };

                _entitySystemPurviewList = new EntitySystemPurview(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemPurviewList(para);

                if (_entitySystemPurviewList != null)
                {
                    _entitySystemPurviewList = base.GetEntitysByPage(_entitySystemPurviewList, pageSize);
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
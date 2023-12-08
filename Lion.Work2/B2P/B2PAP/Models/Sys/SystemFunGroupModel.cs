using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemFunGroupModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryFunControllerID
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryFunControllerID { get; set; }

        public SystemFunGroupModel()
        {
        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryFunControllerID = string.Empty;
        }

        List<EntitySystemFunGroup.SystemFunGroup> _entitySystemFunGroupList;
        public List<EntitySystemFunGroup.SystemFunGroup> EntitySystemFunGroupList { get { return _entitySystemFunGroupList; } }

        public bool GetSystemFunGroupList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunGroup.SystemFunGroupPara para = new EntitySystemFunGroup.SystemFunGroupPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunControllerID) ? null : this.QueryFunControllerID))
                };

                _entitySystemFunGroupList = new EntitySystemFunGroup(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunGroupList(para);

                if (_entitySystemFunGroupList != null)
                {
                    _entitySystemFunGroupList = base.GetEntitysByPage(_entitySystemFunGroupList, pageSize);
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
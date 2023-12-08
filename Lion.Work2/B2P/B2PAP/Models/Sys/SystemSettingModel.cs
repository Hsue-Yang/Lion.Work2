using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemSettingModel : SysModel
    {
        public SystemSettingModel()
        {
        }

        List<EntitySystemSetting.SystemSetting> _entitySystemSettingList;
        public List<EntitySystemSetting.SystemSetting> EntitySystemSettingList { get { return _entitySystemSettingList; } }

        public bool GetSystemSettingList(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemSetting.SystemSettingPara para = new EntitySystemSetting.SystemSettingPara(cultureID.ToString())
                {
                    UserID = (string.IsNullOrWhiteSpace(userID) ? null : new DBVarChar(userID))
                };

                _entitySystemSettingList = new EntitySystemSetting(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemSettingList(para);

                if (_entitySystemSettingList != null)
                {
                    List<EntitySystemSetting.Subsystem> userSubsystemList = new EntitySystemSetting(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .SelectUserSubsystemList(para);

                    if (userSubsystemList != null && userSubsystemList.Count > 0)
                    {
                        foreach (EntitySystemSetting.SystemSetting data in _entitySystemSettingList)
                        {
                            List<EntitySystemSetting.Subsystem> subsystemList = userSubsystemList
                                .Where(subsystems => subsystems.ParentSysID.GetValue() == data.SysID.GetValue()).ToList();

                            if (subsystemList != null)
                            {
                                data.SubsystemList = new List<EntitySystemSetting.Subsystem>(subsystemList);
                            }
                        }
                    }
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
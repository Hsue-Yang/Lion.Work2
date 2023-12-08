using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemRoleGroupCollectModel : SysModel
    {
        public string RoleGroupID { get; set; }

        public List<string> HasRole { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleGroupCollect.TabText_SystemRoleGroupCollect,
                ImageURL=string.Empty
            }
        };

        public SystemRoleGroupCollectModel()
        {

        }

        public void FormReset()
        {
            this.HasRole = new List<string>();
        }

        EntitySystemRoleGroupCollect.SysSystemRoleGroup _entitySysSystemRoleGroup;
        public EntitySystemRoleGroupCollect.SysSystemRoleGroup EntitySysSystemRoleGroup { get { return _entitySysSystemRoleGroup; } }

        public bool GetSysSystemRoleGroup(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRoleGroupCollect.SysSystemRoleGroupPara para = new EntitySystemRoleGroupCollect.SysSystemRoleGroupPara(cultureID.ToString())
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : RoleGroupID))
                };

                _entitySysSystemRoleGroup = new EntitySystemRoleGroupCollect(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemRoleGroup(para);

                if (_entitySysSystemRoleGroup != null)
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

        List<EntitySystemRoleGroupCollect.SysSystemRoleGroupCollect> _entitySysSystemRoleGroupCollectList;
        public List<EntitySystemRoleGroupCollect.SysSystemRoleGroupCollect> EntitySysSystemRoleGroupCollectList { get { return _entitySysSystemRoleGroupCollectList; } }

        public bool GetSysSystemRoleGroupCollectList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara para = new EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara(cultureID.ToString())
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : RoleGroupID))
                };

                _entitySysSystemRoleGroupCollectList = new EntitySystemRoleGroupCollect(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemRoleGroupCollectList(para);

                if (_entitySysSystemRoleGroupCollectList != null)
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

        public bool GetEditSysSystemRoleGroupCollectResult(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara para = new EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara(cultureID.ToString())
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                List<EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara> paraList = new List<EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara>();
                if (this.HasRole != null && this.HasRole.Count > 0)
                {
                    foreach (string roleString in this.HasRole)
                    {
                        paraList.Add(new EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara(cultureID.ToString())
                        {
                            SysID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0])),
                            RoleID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1]))
                        });
                    }
                }

                if (new EntitySystemRoleGroupCollect(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditSysSystemRoleGroupCollect(para, paraList) == EntitySystemRoleGroupCollect.EnumEditUserSystemRoleResult.Success)
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
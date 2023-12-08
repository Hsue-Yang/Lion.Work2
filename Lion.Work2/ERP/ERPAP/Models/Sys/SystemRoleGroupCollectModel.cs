using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemRoleGroupCollectModel : SysModel
    {
        public string RoleGroupID { get; set; }

        public string RoleGroupNM { get; set; }

        public string Remark { get; set; }

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

                _entitySysSystemRoleGroup = new EntitySystemRoleGroupCollect(this.ConnectionStringSERP, this.ProviderNameSERP)
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

                _entitySysSystemRoleGroupCollectList = new EntitySystemRoleGroupCollect(this.ConnectionStringSERP, this.ProviderNameSERP)
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

        public bool GetEditSysSystemRoleGroupCollectResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara para = new EntitySystemRoleGroupCollect.SysSystemRoleGroupCollectPara(cultureID.ToString())
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID))
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

                if (new EntitySystemRoleGroupCollect(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditSysSystemRoleGroupCollect(para, paraList) == EntitySystemRoleGroupCollect.EnumEditUserSystemRoleResult.Success)
                {
                    this.GetRecordSysSystemRoleGroupCollectResult(updUserID, ipAddress, cultureID);

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        private bool GetRecordSysSystemRoleGroupCollectResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.SysSystemRoleGroupCollectPara para = new Entity_BaseAP.SysSystemRoleGroupCollectPara(cultureID.ToString())
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                };

                List<Entity_BaseAP.SysSystemRoleGroupCollect> sysSystemRoleGroupCollectList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSysSystemRoleGroupCollectList(para);

                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(basicInfoPara);

                Mongo_BaseAP.RecordSysSystemRoleGroupCollectPara mongoPara = new Mongo_BaseAP.RecordSysSystemRoleGroupCollectPara()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                };

                List<Mongo_BaseAP.RecordSysSystemRoleGroupCollectPara> mongoParaList = new List<Mongo_BaseAP.RecordSysSystemRoleGroupCollectPara>();

                if (sysSystemRoleGroupCollectList == null || sysSystemRoleGroupCollectList.Count == 0)
                {
                    mongoParaList.Add(new Mongo_BaseAP.RecordSysSystemRoleGroupCollectPara()
                    {
                        RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                        RoleGroupNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNM) ? null : this.RoleGroupNM)),
                        SysID = new DBVarChar(null),
                        SysNM = new DBNVarChar(null),
                        RoleID = new DBVarChar(null),
                        RoleNM = new DBNVarChar(null),
                        APINo = new DBChar(null),
                        UpdUserID = entityBasicInfo.UpdUserID,
                        UpdUserNM = entityBasicInfo.UpdUserNM,
                        UpdDT = new DBDateTime(DateTime.Now),
                        ExecSysID = entityBasicInfo.ExecSysID,
                        ExecSysNM = entityBasicInfo.ExecSysNM,
                        ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                    });
                }
                else
                {
                    foreach (Entity_BaseAP.SysSystemRoleGroupCollect sysSystemRoleGroupCollect in sysSystemRoleGroupCollectList)
                    {
                        mongoParaList.Add(new Mongo_BaseAP.RecordSysSystemRoleGroupCollectPara()
                        {
                            RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                            RoleGroupNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNM) ? null : this.RoleGroupNM)),
                            SysID = sysSystemRoleGroupCollect.SysID,
                            SysNM = sysSystemRoleGroupCollect.SysNM,
                            RoleID = sysSystemRoleGroupCollect.RoleID,
                            RoleNM = sysSystemRoleGroupCollect.RoleNM,
                            APINo = new DBChar(null),
                            UpdUserID = entityBasicInfo.UpdUserID,
                            UpdUserNM = entityBasicInfo.UpdUserNM,
                            UpdDT = new DBDateTime(DateTime.Now),
                            ExecSysID = entityBasicInfo.ExecSysID,
                            ExecSysNM = entityBasicInfo.ExecSysNM,
                            ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                        });
                    }
                }

                // if (new Mongo_BaseAP(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                //     .RecordSysSystemRoleGroupCollect(mongoPara, mongoParaList) == Mongo_BaseAP.EnumRecordSysSystemRoleGroupCollectResult.Success)
                // {
                //     return true;
                // }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        #region Event
        public EntityEventPara.SysRoleGroupCollectEdit GetEventParaSysRoleGroupCollectEditEntity()
        {
            try
            {
                EntityEventPara.SysRoleGroupCollectEdit entityEventParaRoleGroupCollectEdit = new EntityEventPara.SysRoleGroupCollectEdit()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    SysRoleIDList = new List<DBVarChar>()
                };

                if (this.HasRole != null && this.HasRole.Count > 0)
                {
                    foreach (string roleString in this.HasRole)
                    {
                        entityEventParaRoleGroupCollectEdit.SysRoleIDList.Add(new DBVarChar((string.IsNullOrWhiteSpace(roleString) ? null : roleString)));
                    }
                }

                return entityEventParaRoleGroupCollectEdit;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
        #endregion
    }
}
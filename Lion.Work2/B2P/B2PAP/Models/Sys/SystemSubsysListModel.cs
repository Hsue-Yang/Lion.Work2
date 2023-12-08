using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemSubsysListModel : SysModel
    {
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        [Required]
        [StringLength(6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string SubSysID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMJAJP { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemSubsysList.TabText_SystemSubsysList,
                ImageURL=string.Empty
            }
        };

        public SystemSubsysListModel()
        {

        }

        public void FormReset()
        {
            this.SubSysID = string.Empty;
            this.SysNMZHTW = string.Empty;
            this.SysNMZHCN = string.Empty;
            this.SysNMENUS = string.Empty;
            this.SysNMTHTH = string.Empty;
            this.SysNMJAJP = string.Empty;
            this.SortOrder = string.Empty;
        }

        List<EntitySystemSubsysList.SystemSubsys> _entitySystemSubsysList;
        public List<EntitySystemSubsysList.SystemSubsys> EntitySystemSubsysList { get { return _entitySystemSubsysList; } }

        public bool GetSystemSubsysList()
        {
            try
            {
                EntitySystemSubsysList.SystemSubsysPara para = new EntitySystemSubsysList.SystemSubsysPara()
                {
                    ParentSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID))
                };

                _entitySystemSubsysList = new EntitySystemSubsysList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemSub(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetValidationSubsystemExist()
        {
            try
            {
                EntitySystemSubsysList.SystemSubsysPara para = new EntitySystemSubsysList.SystemSubsysPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SubSysID) ? null : this.SubSysID))
                };

                if (new EntitySystemSubsysList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .ValidationSubsystemExist(para) == LionTech.Entity.B2P.Sys.EntitySystemSubsysList.EnumValidationSubsystemExistResult.Success)
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

        public bool InsertSystemSubsysList(string userID)
        {
            try
            {
                EntitySystemSubsysList.SystemSubsysPara para = new EntitySystemSubsysList.SystemSubsysPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SubSysID) ? null : this.SubSysID)),
                    ParentSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    SysNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMZHTW) ? null : this.SysNMZHTW)),
                    SysNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMZHCN) ? null : this.SysNMZHCN)),
                    SysNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMENUS) ? null : this.SysNMENUS)),
                    SysNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMTHTH) ? null : this.SysNMTHTH)),
                    SysNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMJAJP) ? null : this.SysNMJAJP)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemSubsysList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .InsertSystemSub(para) == LionTech.Entity.B2P.Sys.EntitySystemSubsysList.EnumInsertSystemSubResult.Success)
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

        public bool UpdateSystemSubsysList(string userID)
        {
            try
            {
                EntitySystemSubsysList.SystemSubsysPara para = new EntitySystemSubsysList.SystemSubsysPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SubSysID) ? null : this.SubSysID)),
                    SysNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMZHTW) ? null : this.SysNMZHTW)),
                    SysNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMZHCN) ? null : this.SysNMZHCN)),
                    SysNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMENUS) ? null : this.SysNMENUS)),
                    SysNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMTHTH) ? null : this.SysNMTHTH)),
                    SysNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.SysNMJAJP) ? null : this.SysNMJAJP)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemSubsysList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .UpdateSystemSub(para) == LionTech.Entity.B2P.Sys.EntitySystemSubsysList.EnumUpdateSystemSubResult.Success)
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

        public bool DeleteSystemSubsysList()
        {
            try
            {
                EntitySystemSubsysList.SystemSubsysPara para = new EntitySystemSubsysList.SystemSubsysPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SubSysID) ? null : this.SubSysID))
                };

                if (new EntitySystemSubsysList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .DeleteSystemSub(para) == LionTech.Entity.B2P.Sys.EntitySystemSubsysList.EnumDeleteSystemSubResult.Success)
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
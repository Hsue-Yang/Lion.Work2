using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.APIService;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemRoleDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string RoleID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleNMJAJP { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleDetail.TabText_SystemRoleDetail,
                ImageURL=string.Empty
            }
        };

        public SystemRoleDetailModel()
        {

        }

        public void FormReset()
        {
            this.RoleNMZHTW = string.Empty;
            this.RoleNMZHCN = string.Empty;
            this.RoleNMENUS = string.Empty;
            this.RoleNMTHTH = string.Empty;
            this.RoleNMJAJP = string.Empty;
        }

        EntitySystemRoleDetail.SystemRoleDetail _entitySystemRoleDetail;
        public EntitySystemRoleDetail.SystemRoleDetail EntitySystemRoleDetail { get { return _entitySystemRoleDetail; } }

        public bool GetSystemRoleDetail()
        {
            try
            {
                EntitySystemRoleDetail.SystemRoleDetailPara para = new EntitySystemRoleDetail.SystemRoleDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleID) ? null : this.RoleID))
                };

                _entitySystemRoleDetail = new EntitySystemRoleDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemRoleDetail(para);

                if (_entitySystemRoleDetail != null)
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

        public bool GetEditSystemRoleDetailResult(string userID)
        {
            try
            {
                EntitySystemRoleDetail.SystemRoleDetailPara para = new EntitySystemRoleDetail.SystemRoleDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleID) ? null : this.RoleID)),
                    RoleNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMZHTW) ? null : this.RoleNMZHTW)),
                    RoleNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMZHCN) ? null : this.RoleNMZHCN)),
                    RoleNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMENUS) ? null : this.RoleNMENUS)),
                    RoleNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMTHTH) ? null : this.RoleNMTHTH)),
                    RoleNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMJAJP) ? null : this.RoleNMJAJP)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemRoleDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemRoleDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemRoleDetail.EnumEditSystemRoleDetailResult.Success)
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

        public bool GetDeleteSystemRoleDetailResult()
        {
            try
            {
                EntitySystemRoleDetail.SystemRoleDetailPara para = new EntitySystemRoleDetail.SystemRoleDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleID) ? null : this.RoleID))
                };

                if (new EntitySystemRoleDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .DeleteSystemRoleDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemRoleDetail.EnumDeleteSystemRoleDetailResult.Success)
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

        public EntityAPIPara.SCMAPB2PSettingB2PSystemRole GetAPIParaSCMAPB2PSettingB2PSystemRoleEntity()
        {
            try
            {
                EntityAPIPara.SCMAPB2PSettingB2PSystemRole entitySCMAPB2PSettingB2PSystemRole = new EntityAPIPara.SCMAPB2PSettingB2PSystemRole()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleID) ? null : this.RoleID)),
                    RoleNMzhTW = new DBNVarChar((string.IsNullOrWhiteSpace(RoleNMZHTW) ? null : RoleNMZHTW)),
                    RoleNMzhCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMZHCN) ? null : this.RoleNMZHCN)),
                    RoleNMenUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMENUS) ? null : this.RoleNMENUS)),
                    RoleNMthTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMTHTH) ? null : this.RoleNMTHTH)),
                    RoleNMjaJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleNMJAJP) ? null : this.RoleNMJAJP)),
                };

                return entitySCMAPB2PSettingB2PSystemRole;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
    }
}
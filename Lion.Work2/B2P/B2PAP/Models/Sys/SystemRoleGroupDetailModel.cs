using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemRoleGroupDetailModel : SysModel
    {
        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMZhTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMZhCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMEnUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMThTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMJaJP { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }
        
        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleGroupDetail.TabText_SystemRoleGroupDetail,
                ImageURL=string.Empty
            }
        };

        public SystemRoleGroupDetailModel()
        {

        }

        public void FormReset()
        {
            this.RoleGroupNMZhTW = string.Empty;
            this.RoleGroupNMZhCN = string.Empty;
            this.RoleGroupNMEnUS = string.Empty;
            this.RoleGroupNMThTH = string.Empty;
            this.RoleGroupNMJaJP = string.Empty;
            this.SortOrder = string.Empty;
            this.Remark = string.Empty;
        }

        EntitySystemRoleGroupDetail.SystemRoleGroupDetail _entitySystemRoleGroupDetail;
        public EntitySystemRoleGroupDetail.SystemRoleGroupDetail EntitySystemRoleGroupDetail { get { return _entitySystemRoleGroupDetail; } }

        public bool GetSystemRoleGroupDetail()
        {
            try
            {
                EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara para = new EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                };

                _entitySystemRoleGroupDetail = new EntitySystemRoleGroupDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemRoleGroupDetail(para);

                if (_entitySystemRoleGroupDetail != null)
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

        public bool GetEditSystemRoleGroupDetailResult(string userID)
        {
            try
            {
                EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara para = new EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    RoleGroupNMZhTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhTW) ? null : this.RoleGroupNMZhTW)),
                    RoleGroupNMZhCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhCN) ? null : this.RoleGroupNMZhCN)),
                    RoleGroupNMEnUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMEnUS) ? null : this.RoleGroupNMEnUS)),
                    RoleGroupNMThTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMThTH) ? null : this.RoleGroupNMThTH)),
                    RoleGroupNMJaJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMJaJP) ? null : this.RoleGroupNMJaJP)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemRoleGroupDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemRoleGroupDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemRoleGroupDetail.EnumEditSystemRoleGroupDetailResult.Success)
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

        public EntitySystemRoleGroupDetail.EnumDeleteSystemRoleGroupDetailResult GetDeleteSystemRoleGroupDetailResult()
        {
            EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara para = new EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara()
            {
                RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
            };

            var result = new EntitySystemRoleGroupDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemRoleGroupDetail(para);

            return result;
        }
    }
}
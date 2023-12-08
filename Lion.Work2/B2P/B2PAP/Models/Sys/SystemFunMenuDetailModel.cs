using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemFunMenuDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunMenu { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMJAJP { get; set; }

        [Required]
        [StringLength(2)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string DefaultMenuID { get; set; }

        [Required]
        public string IsDisable { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemFunMenuDetail.TabText_SystemFunMenuDetail,
                ImageURL=string.Empty
            }
        };

        public SystemFunMenuDetailModel()
        {

        }

        public void FormReset()
        {
            this.FunMenuNMZHTW = string.Empty;
            this.FunMenuNMZHCN = string.Empty;
            this.FunMenuNMENUS = string.Empty;
            this.FunMenuNMTHTH = string.Empty;
            this.FunMenuNMJAJP = string.Empty;
            this.DefaultMenuID = "1";
            this.IsDisable = EnumYN.N.ToString();
            this.SortOrder = string.Empty;
        }

        EntitySystemFunMenuDetail.SystemFunMenuDetail _entitySystemFunMenuDetail;
        public EntitySystemFunMenuDetail.SystemFunMenuDetail EntitySystemFunMenuDetail { get { return _entitySystemFunMenuDetail; } }

        public bool GetSystemFunMenuDetail()
        {
            try
            {
                EntitySystemFunMenuDetail.SystemFunMenuDetailPara para = new EntitySystemFunMenuDetail.SystemFunMenuDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.FunMenu) ? null : this.FunMenu))
                };

                _entitySystemFunMenuDetail = new EntitySystemFunMenuDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunMenuDetail(para);

                if (_entitySystemFunMenuDetail != null)
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

        public bool GetEditSystemFunMenuDetailResult(string userID)
        {
            try
            {
                EntitySystemFunMenuDetail.SystemFunMenuDetailPara para = new EntitySystemFunMenuDetail.SystemFunMenuDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.FunMenu) ? null : this.FunMenu)),
                    FunMenuNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunMenuNMZHTW) ? null : this.FunMenuNMZHTW)),
                    FunMenuNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunMenuNMZHCN) ? null : this.FunMenuNMZHCN)),
                    FunMenuNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunMenuNMENUS) ? null : this.FunMenuNMENUS)),
                    FunMenuNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunMenuNMTHTH) ? null : this.FunMenuNMTHTH)),
                    FunMenuNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunMenuNMJAJP) ? null : this.FunMenuNMJAJP)),
                    DefaultMenuID = new DBVarChar((string.IsNullOrWhiteSpace(this.DefaultMenuID) ? null : this.DefaultMenuID)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemFunMenuDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemFunMenuDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemFunMenuDetail.EnumEditSystemFunMenuDetailResult.Success)
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

        public EntitySystemFunMenuDetail.EnumDeleteSystemFunMenuDetailResult GetDeleteSystemFunMenuDetailResult()
        {
            EntitySystemFunMenuDetail.SystemFunMenuDetailPara para = new EntitySystemFunMenuDetail.SystemFunMenuDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.FunMenu) ? null : this.FunMenu))
            };

            var result = new EntitySystemFunMenuDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemFunMenuDetail(para);

            return result;
        }
    }
}
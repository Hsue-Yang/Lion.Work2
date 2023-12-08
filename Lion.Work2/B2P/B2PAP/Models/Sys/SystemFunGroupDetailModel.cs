using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemFunGroupDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunControllerID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunGroupJAJP { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemFunGroupDetail.TabText_SystemFunGroupDetail,
                ImageURL=string.Empty
            }
        };

        public SystemFunGroupDetailModel()
        {

        }

        public void FormReset()
        {
            this.FunGroupZHTW = string.Empty;
            this.FunGroupZHCN = string.Empty;
            this.FunGroupENUS = string.Empty;
            this.FunGroupTHTH = string.Empty;
            this.FunGroupJAJP = string.Empty;
            this.SortOrder = string.Empty;
        }

        EntitySystemFunGroupDetail.SystemFunGroupDetail _entitySystemFunGroupDetail;
        public EntitySystemFunGroupDetail.SystemFunGroupDetail EntitySystemFunGroupDetail { get { return _entitySystemFunGroupDetail; } }

        public bool GetSystemFunGroupDetail()
        {
            try
            {
                EntitySystemFunGroupDetail.SystemFunGroupDetailPara para = new EntitySystemFunGroupDetail.SystemFunGroupDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID))
                };

                _entitySystemFunGroupDetail = new EntitySystemFunGroupDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunGroupDetail(para);

                if (_entitySystemFunGroupDetail != null)
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

        public bool GetEditSystemFunGroupDetailResult(string userID)
        {
            try
            {
                EntitySystemFunGroupDetail.SystemFunGroupDetailPara para = new EntitySystemFunGroupDetail.SystemFunGroupDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunGroupZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunGroupZHTW) ? null : this.FunGroupZHTW)),
                    FunGroupZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunGroupZHCN) ? null : this.FunGroupZHCN)),
                    FunGroupENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunGroupENUS) ? null : this.FunGroupENUS)),
                    FunGroupTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunGroupTHTH) ? null : this.FunGroupTHTH)),
                    FunGroupJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunGroupJAJP) ? null : this.FunGroupJAJP)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemFunGroupDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemFunGroupDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemFunGroupDetail.EnumEditSystemFunGroupDetailResult.Success)
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

        public EntitySystemFunGroupDetail.EnumDeleteSystemFunGroupDetailResult GetDeleteSystemFunGroupDetailResult()
        {
            EntitySystemFunGroupDetail.SystemFunGroupDetailPara para = new EntitySystemFunGroupDetail.SystemFunGroupDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID))
            };

            var result = new EntitySystemFunGroupDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemFunGroupDetail(para);

            return result;
        }
    }
}
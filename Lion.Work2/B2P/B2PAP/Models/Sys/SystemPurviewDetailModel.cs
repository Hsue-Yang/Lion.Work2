using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemPurviewDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string PurviewID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMJAJP { get; set; }

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
                TabText=SysSystemPurviewDetail.TabText_SystemPurviewDetail,
                ImageURL=string.Empty
            }
        };

        public SystemPurviewDetailModel()
        {

        }

        public void FormReset()
        {
            this.PurviewNMZHTW = string.Empty;
            this.PurviewNMZHCN = string.Empty;
            this.PurviewNMENUS = string.Empty;
            this.PurviewNMTHTH = string.Empty;
            this.PurviewNMJAJP = string.Empty;
            this.SortOrder = string.Empty;
        }

        EntitySystemPurviewDetail.SystemPurviewDetail _entitySystemPurviewDetail;
        public EntitySystemPurviewDetail.SystemPurviewDetail EntitySystemPurviewDetail { get { return _entitySystemPurviewDetail; } }

        public bool GetSystemPurviewDetail()
        {
            try
            {
                EntitySystemPurviewDetail.SystemPurviewDetailPara para = new EntitySystemPurviewDetail.SystemPurviewDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    PurviewID = new DBVarChar((string.IsNullOrWhiteSpace(this.PurviewID) ? null : this.PurviewID))
                };

                _entitySystemPurviewDetail = new EntitySystemPurviewDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemPurviewDetail(para);

                if (_entitySystemPurviewDetail != null)
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

        public bool GetEditSystemPurviewDetailResult(string userID)
        {
            try
            {
                EntitySystemPurviewDetail.SystemPurviewDetailPara para = new EntitySystemPurviewDetail.SystemPurviewDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    PurviewID = new DBVarChar((string.IsNullOrWhiteSpace(this.PurviewID) ? null : this.PurviewID)),
                    PurviewNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.PurviewNMZHTW) ? null : this.PurviewNMZHTW)),
                    PurviewNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.PurviewNMZHCN) ? null : this.PurviewNMZHCN)),
                    PurviewNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.PurviewNMENUS) ? null : this.PurviewNMENUS)),
                    PurviewNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.PurviewNMTHTH) ? null : this.PurviewNMTHTH)),
                    PurviewNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.PurviewNMJAJP) ? null : this.PurviewNMJAJP)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemPurviewDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditSystemPurviewDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemPurviewDetail.EnumEditSystemPurviewDetailResult.Success)
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

        public EntitySystemPurviewDetail.EnumDeleteSystemPurviewDetailResult GetDeleteSystemPurviewDetailResult()
        {
            EntitySystemPurviewDetail.SystemPurviewDetailPara para = new EntitySystemPurviewDetail.SystemPurviewDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                PurviewID = new DBVarChar((string.IsNullOrWhiteSpace(this.PurviewID) ? null : this.PurviewID))
            };

            var result = new EntitySystemPurviewDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemPurviewDetail(para);

            return result;
        }
    }
}
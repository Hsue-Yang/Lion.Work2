using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemEDIConDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        public string EDIFlowID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConJAJP { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string ProviderName { get; set; }

        [Required]
        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string ConValue { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public bool HasSysID { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIConDetail.TabText_SystemEDIConDetail,
                ImageURL=string.Empty
            }
        };

        public SystemEDIConDetailModel()
        {

        }

        public void FormReset()
        {
            this.EDIConZHTW = string.Empty;
            this.EDIConZHCN = string.Empty;
            this.EDIConENUS = string.Empty;
            this.EDIConTHTH = string.Empty;
            this.EDIConJAJP = string.Empty;
            this.ProviderName = string.Empty;
            this.ConValue = string.Empty;
            this.SortOrder = string.Empty;
            this.HasSysID = false;
        }

        public bool SetHasSysID()
        {
            foreach (EntitySys.SysUserSystemSysID systemSysIDList in EntitySysUserSystemSysIDList)
            {
                if (this.SysID == systemSysIDList.SysID.GetValue())
                {
                    this.HasSysID = true;
                    break;
                }
            }

            return this.HasSysID;
        }

        EntitySystemEDIConDetail.SystemEDIConDetail _entitySystemEDIConDetail;
        public EntitySystemEDIConDetail.SystemEDIConDetail EntitySystemEDIConDetail { get { return _entitySystemEDIConDetail; } }

        public bool GetSystemEDIConDetail()
        {
            try
            {
                EntitySystemEDIConDetail.SystemEDIConDetailPara para = new EntitySystemEDIConDetail.SystemEDIConDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                    EDIConID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIConID) ? null : this.EDIConID)),
                };

                _entitySystemEDIConDetail = new EntitySystemEDIConDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIConDetail(para);

                if (_entitySystemEDIConDetail != null)
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

        public bool GetEditSystemEDIConDetailResult(string userID)
        {
            try
            {
                EntitySystemEDIConDetail.SystemEDIConDetailPara para = new EntitySystemEDIConDetail.SystemEDIConDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                    EDIConID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIConID) ? null : this.EDIConID)),
                    EDIConZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIConZHTW) ? null : this.EDIConZHTW)),
                    EDIConZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIConZHCN) ? null : this.EDIConZHCN)),
                    EDIConENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIConENUS) ? null : this.EDIConENUS)),
                    EDIConTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIConTHTH) ? null : this.EDIConTHTH)),
                    EDIConJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIConJAJP) ? null : this.EDIConJAJP)),
                    ProviderName = new DBVarChar((string.IsNullOrWhiteSpace(this.ProviderName) ? null : this.ProviderName)),
                    ConValue = new DBVarChar((string.IsNullOrWhiteSpace(this.ConValue) ? null : this.ConValue)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemEDIConDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemEDIConDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemEDIConDetail.EnumEditSystemEDIConDetailResult.Success)
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

        public bool GetSortOrder()
        {
            try
            {
                EntitySystemEDIConDetail.SystemEDIConDetailPara para = new EntitySystemEDIConDetail.SystemEDIConDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID))
                };

                this.SortOrder = new EntitySystemEDIConDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .GetConNewSortOrder(para);

                if (this.SortOrder != null)
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

        public EntitySystemEDIConDetail.EnumDeleteSystemEDIConDetailResult GetDeleteSystemEDIConDetailResult()
        {
            EntitySystemEDIConDetail.SystemEDIConDetailPara para = new EntitySystemEDIConDetail.SystemEDIConDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                EDIConID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIConID) ? null : this.EDIConID))
            };

            var result = new EntitySystemEDIConDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemEDIConDetail(para);

            return result;
        }
    }
}
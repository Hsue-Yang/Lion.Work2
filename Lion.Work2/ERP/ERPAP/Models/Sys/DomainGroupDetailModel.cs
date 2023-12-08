using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class DomainGroupDetailModel : SysModel
    {
        [Required]
        public string DomainName { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string DomainGroupID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string DomainGroupNM { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string DomainGroupNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string DomainGroupNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string DomainGroupNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string DomainGroupNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string DomainGroupNMJAJP { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysDomainGroupDetail.TabText_DomainGroupDetail,
                ImageURL=string.Empty
            }
        };

        public DomainGroupDetailModel()
        {
        }

        EntityDomainGroupDetail.DomainGroupDetail _entityDomainGroupDetail;
        public EntityDomainGroupDetail.DomainGroupDetail EntityDomainGroupDetail { get { return _entityDomainGroupDetail; } }

        public bool GetDomainGroupDetail(EnumCultureID cultureID)
        {
            try
            {
                EntityDomainGroupDetail.DomainGroupDetailPara para = new EntityDomainGroupDetail.DomainGroupDetailPara(cultureID.ToString())
                    {
                        DomainName = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainName) ? null : this.DomainName)),
                        DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainGroupID) ? null : this.DomainGroupID)),                        
                    };

                _entityDomainGroupDetail = new EntityDomainGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectDomainGroupDetail(para);

                if (_entityDomainGroupDetail != null)
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

        EntityDomainGroupDetail.DomainGroupDetail _entityDomainGroupID;
        public EntityDomainGroupDetail.DomainGroupDetail EntityDomainGroupID { get { return _entityDomainGroupID; } }

        public string SelectDomainGroupID(EnumCultureID cultureID, string domainName, string domainGroupID)
        {
            try
            {
                EntityDomainGroupDetail.DomainGroupDetailPara para = new EntityDomainGroupDetail.DomainGroupDetailPara(cultureID.ToString())
                {
                    DomainName = new DBVarChar((string.IsNullOrWhiteSpace(domainName) ? null : domainName)),
                    DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(domainGroupID) ? null : domainGroupID)),
                   
                };

                _entityDomainGroupID = new EntityDomainGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectDomainGroupDetail(para);

                if (_entityDomainGroupID != null)
                {
                    return _entityDomainGroupID.DomainGroupID.GetValue();
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        public bool EditDomainGroupDetail(EnumCultureID cultureID, string userID)
        {
            try
            {
                EntityDomainGroupDetail.DomainGroupDetailPara para = new EntityDomainGroupDetail.DomainGroupDetailPara(cultureID.ToString())
                {
                    DomainName = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainName) ? null : this.DomainName)),
                    DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainGroupID) ? null : this.DomainGroupID)),
                    DomainGroupNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.DomainGroupNMZHTW) ? null : this.DomainGroupNMZHTW)),
                    DomainGroupNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.DomainGroupNMZHCN) ? null : this.DomainGroupNMZHCN)),
                    DomainGroupNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.DomainGroupNMENUS) ? null : this.DomainGroupNMENUS)),
                    DomainGroupNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.DomainGroupNMTHTH) ? null : this.DomainGroupNMTHTH)),
                    DomainGroupNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.DomainGroupNMJAJP) ? null : this.DomainGroupNMJAJP)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                };

                if (new EntityDomainGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditDomainGroupDetailList(para) == LionTech.Entity.ERP.Sys.EntityDomainGroupDetail.EnumEditDomainGroupDetailListResult.Success)
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

        public EntityDomainGroupDetail.EnumDeleteDomainGroupDetailListResult GetDeleteDomainGroupDetailResult(EnumCultureID cultureID)
        {
            EntityDomainGroupDetail.DomainGroupDetailPara para = new EntityDomainGroupDetail.DomainGroupDetailPara(cultureID.ToString())
            {
                DomainName = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainName) ? null : this.DomainName)),
                DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainGroupID) ? null : this.DomainGroupID)),
            };

            var result = new EntityDomainGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                .DeleteDomainGroupDetailList(para);

            return result;
        }
    }
}
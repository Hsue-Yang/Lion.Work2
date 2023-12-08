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
    public class SRCProjectDetailModel : SysModel
    {
        public enum Field
        {
            IsSVN, IsWrite,
            DomainGroupID
        }

        public List<string> DomainName { get; set; }
        public List<string> DomainGroupID { get; set; }
        public List<string> IsWrite { get; set; }

        public string ProjectParent { get; set; }
        public string IsSVN { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string ProjectID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNM { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ProjectNMKOKR { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSRCProjectDetail.TabText_SRCProjectDetail,
                ImageURL=string.Empty
            }
        };

        public SRCProjectDetailModel()
        {
        }

        List<EntitySRCProjectDetail.SRCProjectDetail> _entityProjectParentList = new List<EntitySRCProjectDetail.SRCProjectDetail>();
        public List<EntitySRCProjectDetail.SRCProjectDetail> EntityProjectParentList { get { return _entityProjectParentList; } }

        public bool GetProjectParentList(EnumCultureID cultureID)
        {
            try
            {
                EntitySRCProjectDetail.SRCProjectDetailPara para = new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString());

                _entityProjectParentList = new EntitySRCProjectDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectProjectParentList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        EntitySRCProjectDetail.SRCProjectDetail _entitySRCProjectDetail;
        public EntitySRCProjectDetail.SRCProjectDetail EntitySRCProjectDetail { get { return _entitySRCProjectDetail; } }

        public bool GetSRCProjectDetailList(EnumCultureID cultureID)
        {
            try
            {
                EntitySRCProjectDetail.SRCProjectDetailPara para = new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString())
                    {
                        ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectID) ? null : this.ProjectID))
                    };

                _entitySRCProjectDetail = new EntitySRCProjectDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSRCProjectDetail(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        EntitySRCProjectDetail.SRCProjectDetail _entityProjectID;
        public EntitySRCProjectDetail.SRCProjectDetail EntityProjectID { get { return _entityProjectID; } }

        public string SelectProjectID(EnumCultureID cultureID, string projectID)
        {
            try
            {
                EntitySRCProjectDetail.SRCProjectDetailPara para = new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString())
                {
                    ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(projectID) ? null : projectID)),
                };

                _entityProjectID = new EntitySRCProjectDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSRCProjectDetail(para);

                if (_entityProjectID != null)
                {
                    return _entityProjectID.ProjectID.GetValue();
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        List<EntitySRCProjectDetail.SRCProjectDetail> _entityDomainGroupList = new List<EntitySRCProjectDetail.SRCProjectDetail>();
        public List<EntitySRCProjectDetail.SRCProjectDetail> EntityDomainGroupList { get { return _entityDomainGroupList; } }

        public bool GetDomainGroupList(EnumCultureID cultureID)
        {
            try
            {
                EntitySRCProjectDetail.SRCProjectDetailPara para = new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString())
                {
                    ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectID) ? null : this.ProjectID))
                };

                _entityDomainGroupList = new EntitySRCProjectDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectDomainGroupList(para);

                if (_entityDomainGroupList != null)
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

        public bool EditSRCProjectDetail(EnumCultureID cultureID, string userID)
        {
            try
            {
                string isSVN = null;
                if (this.IsSVN != null)
                {
                    isSVN = "Y";
                }
                else
                {
                    isSVN = "N";
                }
                EntitySRCProjectDetail.SRCProjectDetailPara para = new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString())
                {
                    ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectID) ? null : this.ProjectID)),
                    IsSVN = new DBChar((string.IsNullOrWhiteSpace(isSVN) ? null : isSVN)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    ProjectPraent = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectParent) ? null : this.ProjectParent)),
                    ProjectNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.ProjectNMZHTW) ? null : this.ProjectNMZHTW)),
                    ProjectNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.ProjectNMZHCN) ? null : this.ProjectNMZHCN)),
                    ProjectNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.ProjectNMENUS) ? null : this.ProjectNMENUS)),
                    ProjectNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.ProjectNMTHTH) ? null : this.ProjectNMTHTH)),
                    ProjectNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.ProjectNMJAJP) ? null : this.ProjectNMJAJP)),
                    ProjectNMKOKR = new DBNVarChar((string.IsNullOrWhiteSpace(this.ProjectNMKOKR) ? null : this.ProjectNMKOKR)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                };

                List<EntitySRCProjectDetail.SRCProjectDetailPara> domainGroupIDPara = new List<EntitySRCProjectDetail.SRCProjectDetailPara>();
                if (DomainGroupID != null)
                {
                    int i = 0;
                    string isWrite = null;
                    foreach (string domainGroupID in DomainGroupID)
                    {
                        if (i  >=  IsWrite.Count)
                        {
                            i = IsWrite.Count - 1;
                        }
                        if (domainGroupID == this.IsWrite[i])
                        {
                            isWrite = "Y";
                            i++;
                        }
                        else
                        {
                            isWrite = "N";
                        }
                        domainGroupIDPara.Add(new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString())
                        {
                            ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectID) ? null : this.ProjectID)),
                            DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(domainGroupID) ? null : domainGroupID)),
                            IsWrite = new DBChar((string.IsNullOrWhiteSpace(isWrite) ? null : isWrite)),
                            UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                        });
                    }
                }

                if (new EntitySRCProjectDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditSRCProjectDetailList(para, domainGroupIDPara) == LionTech.Entity.ERP.Sys.EntitySRCProjectDetail.EnumEditSRCProjectDetailListResult.Success)
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

        public bool DeleteSRCProjectDetail(EnumCultureID cultureID, string userID)
        {
            try
            {
                EntitySRCProjectDetail.SRCProjectDetailPara para = new EntitySRCProjectDetail.SRCProjectDetailPara(cultureID.ToString())
                {
                    ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectID) ? null : this.ProjectID)),
                };

                if (new EntitySRCProjectDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .DeleteSRCProjectDetailList(para) == LionTech.Entity.ERP.Sys.EntitySRCProjectDetail.EnumDeleteSRCProjectDetailListResult.Success)
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;

namespace ERPAP.Models.Sys
{
    public class SRCProjectModel : SysModel
    {
        public enum Field
        {
            ProjectID, DomainName, DomainGroupID
        }

        public enum SVNField
        {
            isSVN, notSVN
        }

        public enum DomainNameField
        { 
            LionTech
        }

        [Required]
        public string DomainName { get; set; }

        public string DomainGroupID { get; set; }

        public string ProjectID { get; set; }

        public SRCProjectModel()
        {
        }

        public void FormReset()
        {
            this.DomainName = DomainNameField.LionTech.ToString();
        }

        List<EntitySRCProject.SRCProject> _entitySRCProjectlMenuList = new List<EntitySRCProject.SRCProject>();
        public List<EntitySRCProject.SRCProject> EntitySRCProjectMenuList { get { return _entitySRCProjectlMenuList; } }

        public bool GetProjectMenuList(EnumCultureID cultureID, string domainGroupID)
        {
            try
            {
                List<EntitySRCProject.SRCProject> entitySRCProjectlMenuList;
                int count = 0;
                int listcount = 0;
                if (domainGroupID != null)
                {
                    this.DomainGroupID =  domainGroupID;
                }
                EntitySRCProject.SRCProjectPara para = new EntitySRCProject.SRCProjectPara(cultureID.ToString())
                    {
                        DomainName = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainName) ? null : this.DomainName)),
                        DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainGroupID) ? null : this.DomainGroupID)),                        
                    };

                EntitySRCProject.SRCProject isSVN = new EntitySRCProject.SRCProject()
                {
                    ProjectNM = new DBVarChar(Resources.SysSRCProject.ComboBox_IsSVN),
                    ProjectID = new DBVarChar(SVNField.isSVN),
                };

                EntitySRCProject.SRCProject notSVN = new EntitySRCProject.SRCProject()
                {
                    ProjectNM = new DBVarChar(Resources.SysSRCProject.ComboBox_NotSVN),
                    ProjectID = new DBVarChar(SVNField.notSVN),
                };
                entitySRCProjectlMenuList = new EntitySRCProject(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectProjectMenuList(para);

                if (entitySRCProjectlMenuList != null)
                {
                    _entitySRCProjectlMenuList.Add(entitySRCProjectlMenuList[count].IsSVN.GetValue() ==EnumYN.Y.ToString()? isSVN: notSVN);

                    foreach (EntitySRCProject.SRCProject data in entitySRCProjectlMenuList)
                    {
                        if (entitySRCProjectlMenuList[count].IsSVN.GetValue() !=entitySRCProjectlMenuList[listcount].IsSVN.GetValue())
                        {
                            _entitySRCProjectlMenuList.Add(notSVN);
                        }
                        _entitySRCProjectlMenuList.Add(data);
                        if (count != 0)
                        {
                            listcount++;
                        }
                        count++;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySRCProject.SRCProject> _entitySRCProjectlList;
        public List<EntitySRCProject.SRCProject> EntitySRCProjectList { get { return _entitySRCProjectlList; } }

        public bool GetProjectList(EnumCultureID cultureID)
        {
            if (this.DomainName == null)
            {
                this.DomainName = EntityBaseDomainNameList[0].CodeID.GetValue();
            }
            try
            {
                EntitySRCProject.SRCProjectPara para = new EntitySRCProject.SRCProjectPara(cultureID.ToString())
                {
                    ProjectID = new DBVarChar((string.IsNullOrWhiteSpace(this.ProjectID) ? null : this.ProjectID)),
                    DomainName = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainName) ? null : this.DomainName)),
                    DomainGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainGroupID) ? null : this.DomainGroupID))
                };

                _entitySRCProjectlList = new EntitySRCProject(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectProjectList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
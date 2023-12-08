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
    public class UserDomainDetailModel : SysModel
    {
        private string _QueryUserID;
        [Required]
        [StringLength(4, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID
        {
            get
            {
                if (string.IsNullOrEmpty(_QueryUserID))
                {
                    return _QueryUserID;
                }
                return _QueryUserID.ToUpper();
            }
            set { _QueryUserID = value; }
        }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string DomainAccount { get; set; }
        
        public List<string> DomainName { get; set; }
        public List<string> DomainGroupID { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,                
                TabText= SysUserDomainDetail.TabText_UserDomainDetail,
                ImageURL=string.Empty
            }
        };

        public UserDomainDetailModel()
        {
        }

        List<EntityUserDomainDetail.UserDomainDetail> _entityDomainGroupList = new List<EntityUserDomainDetail.UserDomainDetail>();
        public List<EntityUserDomainDetail.UserDomainDetail> EntityDomainGroupList { get { return _entityDomainGroupList; } }

        public bool GetDomainGroupList(EnumCultureID cultureID)
        {
            try
            {
                EntityUserDomainDetail.UserDomainDetailPara para = new EntityUserDomainDetail.UserDomainDetailPara(cultureID.ToString())
                {
                    DomainAccount = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainAccount) ? null : this.DomainAccount))
                };

                _entityDomainGroupList = new EntityUserDomainDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
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

        public bool EditUserDomainDetailList(EnumCultureID cultureID, string userID)
        {
            try
            {
                EntityUserDomainDetail.UserDomainDetailPara para = new EntityUserDomainDetail.UserDomainDetailPara(cultureID.ToString())
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                        DomainAccount = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainAccount) ? null : this.DomainAccount)),
                    };

                List<EntityUserDomainDetail.UserDomainDetailPara> domainNamePara = new List<EntityUserDomainDetail.UserDomainDetailPara>();
                if (DomainName != null)
                {
                    foreach (string domainName in DomainName)
                    {
                        domainNamePara.Add(new EntityUserDomainDetail.UserDomainDetailPara(cultureID.ToString())
                        {
                            UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                            DomainName = new DBVarChar((string.IsNullOrWhiteSpace(domainName) ? null : domainName)),
                            DomainAccount = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainAccount) ? null : this.DomainAccount)),
                            UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                        });
                    }
                }

                List<EntityUserDomainDetail.UserDomainDetailPara> domainGroupIDPara = new List<EntityUserDomainDetail.UserDomainDetailPara>();
                if (DomainGroupID != null)
                {
                    foreach (string domainGroupID in DomainGroupID)
                    {
                        domainGroupIDPara.Add(new EntityUserDomainDetail.UserDomainDetailPara(cultureID.ToString())
                        {
                            UserID =new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                            DomainGroupID =new DBVarChar((string.IsNullOrWhiteSpace(domainGroupID) ? null : domainGroupID)),
                            DomainAccount =new DBVarChar((string.IsNullOrWhiteSpace(this.DomainAccount)? null: this.DomainAccount)),
                            UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                        });
                    }
                }

                if (new EntityUserDomainDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditUserDomainDetailList(para, domainNamePara, domainGroupIDPara) == LionTech.Entity.ERP.Sys.EntityUserDomainDetail.EnumEditUserDomainDetailListResult.Success)
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

        public bool DeleteUserDomainDetailList(EnumCultureID cultureID)
        {
            try
            {
                EntityUserDomainDetail.UserDomainDetailPara para = new EntityUserDomainDetail.UserDomainDetailPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    DomainAccount = new DBVarChar((string.IsNullOrWhiteSpace(this.DomainAccount) ? null : this.DomainAccount)),
                };

                if (new EntityUserDomainDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .DeleteUserDomainDetailList(para) == LionTech.Entity.ERP.Sys.EntityUserDomainDetail.EnumDeleteUserDomainDetailListResult.Success)
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
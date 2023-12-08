using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class UserDomainModel : SysModel
    {
        #region - Definitions -
        public class UserDoaminInfo
        {
            public string UserNM { get; set; }
            public string DomainAccount { get; set; }
            public string DomainNM { get; set; }
            public List<string> DomainGroup { get; set; }
        }
        #endregion

        #region - Property -
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID
        {
            get
            {
                if (string.IsNullOrEmpty(_userID))
                {
                    return _userID;
                }
                return _userID.ToUpper();
            }
            set { _userID = value; }
        }

        public string UserNM { get; set; }

        [Required]
        public string DomainPath { get; set; }
        public string UserEMailAccount { get; set; }
        public string DomainAccount { get; set; }
        public string DomainPWD { get; set; }
        public List<ExtendedSelectListItem> DomainSelectListItems =>
            (from s in Enum.GetNames(typeof(EnumDomainType))
             let domainPath = Common.GetEnumDesc((EnumDomainType)Enum.Parse(typeof(EnumDomainType), s))
             select new ExtendedSelectListItem
             {
                 Text = s,
                 Value = domainPath,
                 Selected = domainPath == DomainPath
             }).ToList();
        public List<EntityUserDomain.UserDomain> EntityUserDomainList { get; private set; }
        #endregion

        #region - Private -
        private string _userID;
        #endregion

        public bool GetUserDomainList(EnumCultureID cultureID, int pageSize)
        {
            try
            {
                EntityUserDomain.UserDomainPara para = new EntityUserDomain.UserDomainPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID)),
                    UserNM = new DBNVarChar((string.IsNullOrWhiteSpace(UserNM) ? null : UserNM))
                };

                EntityUserDomainList = GetEntitysByPage(new EntityUserDomain(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserDomainList(para), pageSize);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        #region - 取得使用者網域群組 -
        public UserDoaminInfo GetUserDoaminInfoList()
        {
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry(DomainPath);
                DirectorySearcher searcher = new DirectorySearcher(directoryEntry);
                EnumDomainType domainType = GetDomainTypeByLDAPPath(DomainPath);

                if (domainType == EnumDomainType.LionTech &&
                    string.IsNullOrWhiteSpace(DomainPWD) == false)
                {
                    directoryEntry.Username = DomainAccount.Split('@')[0];
                    directoryEntry.Password = Security.Decrypt(DomainPWD);
                }

                searcher.SearchScope = SearchScope.Subtree;
                searcher.Filter = "(&(objectCategory=person)(samaccountname=" + UserEMailAccount + "))";

                searcher.PropertiesToLoad.Add("samaccountname");
                searcher.PropertiesToLoad.Add("memberOf");

                SearchResult searchResult = searcher.FindOne();

                if (searchResult != null)
                {
                    return new UserDoaminInfo
                    {
                        UserNM = UserNM,
                        DomainAccount = searchResult.Properties["samaccountname"][0].ToString(),
                        DomainNM = domainType.ToString(),
                        DomainGroup = (from string row in searchResult.Properties["memberOf"]
                                       let cn = row.Split(',')[0].Substring(3)
                                       select cn).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return null;
        }
        #endregion
    }
}
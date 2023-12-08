using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class DomainGroupUserModel : SysModel
    {
        #region - Property -
        public string DomainNM { get; set; }
        public string DomainPath { get; set; }
        public string DomainGroupNM { get; set; }
        public string DomainAccount { get; set; }
        public string DomainPWD { get; set; }
        public List<EntityDomainGroupUser.DomainGroupAccount> EntityDomainGroupAccountList { get; private set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>
        {
            new TabStripHelper.Tab
            {
                ControllerName = string.Empty,
                ActionName = string.Empty,
                TabText = SysDomainGroupUser.TabText_DomainGroupUser,
                ImageURL = string.Empty
            }
        };
        #endregion

        #region - 取得網域群組使用者 -
        /// <summary>
        /// 取得網域群組使用者
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public bool GetDomainGroupUserList(EnumCultureID cultureID)
        {
            try
            {
                string propFieldName = "mail";
                EnumDomainType domainType = GetDomainTypeByLDAPPath(DomainPath);
                DomainNM = domainType.ToString();
                DirectoryEntry directoryEntry = new DirectoryEntry(DomainPath);

                DirectorySearcher searcher = new DirectorySearcher(directoryEntry);
                searcher.SearchScope = SearchScope.Subtree;

                if (domainType == EnumDomainType.LionTech &&
                    string.IsNullOrWhiteSpace(DomainPWD) == false)
                {
                    propFieldName = "samaccountname";
                    directoryEntry.Username = DomainAccount.Split('@')[0];
                    directoryEntry.Password = Security.Decrypt(DomainPWD);
                }

                searcher.Filter = $"(&(objectCategory=group)(CN={DomainGroupNM}))";
                searcher.PropertiesToLoad.Add("samaccountname");
                searcher.PropertiesToLoad.Add("name");
                searcher.PropertiesToLoad.Add("mail");

                SearchResult searchResult = searcher.FindOne();

                if (searchResult != null)
                {
                    searcher.Filter = $"(&(objectCategory=person)(memberOf={searchResult.Path.Substring(searchResult.Path.LastIndexOf("/", StringComparison.Ordinal) + 1)}))";

                    var searchResultCollection = searcher.FindAll();
                    EntityDomainGroupUser.DomainGroupAccountPara para = new EntityDomainGroupUser.DomainGroupAccountPara(cultureID.ToString())
                    {
                        UserEmail = (from SearchResult result in searchResultCollection
                                     where result.Properties[propFieldName] != null && result.Properties[propFieldName].Count > 0
                                     select new DBVarChar(result.Properties[propFieldName][0] + (propFieldName == "samaccountname" ? "@liontravel.com" : string.Empty))).ToList()
                    };

                    if (para.UserEmail.Any())
                    {
                        EntityDomainGroupAccountList = new EntityDomainGroupUser(ConnectionStringSERP, ProviderNameSERP)
                            .SelectDomainGroupAccount(para);

                        var accountRepeatList =
                            (from s in EntityDomainGroupAccountList
                             group s by s.DomainAccount.GetValue()
                             into g
                             select new
                             {
                                 domain = g.Key,
                                 name = (from SearchResult result in searchResultCollection
                                         where result.Properties["name"] != null && result.Properties[propFieldName][0].ToString() == g.Key
                                         select result.Properties["name"][0].ToString()).SingleOrDefault(),
                                 count = g.Count()
                             }).Where(w => w.count > 1);

                        foreach (var row in accountRepeatList)
                        {
                            EntityDomainGroupAccountList.RemoveAll(w => w.DomainAccount.GetValue() == row.domain && w.UserNM.GetValue() != row.name);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        internal bool IsUserAuthorized(string userID)
        {
            try
            {
                string userEmail = GetUserEMail(userID);
                if (string.IsNullOrWhiteSpace(userEmail) == false)
                {
                    return EntityDomainGroupAccountList.Any(a => string.Compare(a.DomainAccount.GetValue(), userEmail, StringComparison.OrdinalIgnoreCase) == 0);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion
    }
}
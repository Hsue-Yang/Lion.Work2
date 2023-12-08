using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices;
using System.Linq;
using LionTech.Utility;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class DomainGroupModel : SysModel
    {
        #region - Property -
        public new enum EnumCookieKey
        {
            DomainPath,
            DomainSecondLevelPath,
            DomainThridLevelPath
        }

        [Required]
        public string DomainPath { get; set; }
        public string DomainGroupNM { get; set; }
        public string DomainSecondLevelPath { get; set; }
        public string DomainThridLevelPath { get; set; }
        public string LDAPPath { get; set; }
        public string DomainAccount { get; set; }
        [Required]
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
        public List<ExtendedSelectListItem> DomainSecondLevelSelectListItems { get; private set; }
        public List<ExtendedSelectListItem> DomainThirdLevelSelectListItems { get; private set; }
        public List<string> DomainGroupList { get; private set; }
        #endregion
        
        #region - 取得網域群組 -
        /// <summary>
        /// 取得網域群組
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public bool GetDomainInfoList(int pageSize)
        {
            try
            {
                List<ExtendedSelectListItem> domainGroupList;
                DomainPath = DomainPath ?? Common.GetEnumDesc(EnumDomainType.LionMail);
                DomainSecondLevelSelectListItems = GetOneLevelList(DomainPath);
                DomainSecondLevelSelectListItems.ForEach(row =>
                {
                    if (row.Value == DomainSecondLevelPath)
                    {
                        row.Selected = true;
                    }
                });

                string subLevelPath = DomainSecondLevelSelectListItems.Find(f => f.Selected)?.Value;
                subLevelPath = subLevelPath ?? (DomainSecondLevelSelectListItems.FirstOrDefault()?.Value);

                DomainThirdLevelSelectListItems = GetOneLevelList(subLevelPath);
                
                if (GetDomainTypeByLDAPPath(DomainPath) == EnumDomainType.LionTech)
                {
                    domainGroupList = DomainThirdLevelSelectListItems;
                }
                else
                {
                    DomainThirdLevelSelectListItems.ForEach(row =>
                    {
                        if (row.Value == DomainThridLevelPath)
                        {
                            row.Selected = true;
                        }
                    });

                    subLevelPath = DomainThirdLevelSelectListItems.Find(f => f.Selected)?.Value;
                    subLevelPath = subLevelPath ?? (DomainThirdLevelSelectListItems.FirstOrDefault()?.Value);

                    domainGroupList = GetOneLevelList(subLevelPath);
                }

                DomainGroupList = GetEntitysByPage(domainGroupList.Select(s => s.Text).ToList(), pageSize);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion
        
        #region - 取得網域階層資料 -
        /// <summary>
        /// 取得網域階層資料
        /// </summary>
        /// <param name="ldapPath"></param>
        /// <returns></returns>
        public List<ExtendedSelectListItem> GetOneLevelList(string ldapPath)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry(ldapPath);
            DirectorySearcher searcher = new DirectorySearcher(directoryEntry);
            
            if (GetDomainTypeByLDAPPath(ldapPath) == EnumDomainType.LionTech &&
                string.IsNullOrWhiteSpace(DomainPWD) == false)
            {
                directoryEntry.Username = DomainAccount.Split('@')[0];
                directoryEntry.Password = Security.Decrypt(DomainPWD);
            }

            searcher.PropertiesToLoad.Add("name");
            searcher.SearchScope = SearchScope.OneLevel;
            searcher.PageSize = 500;

            return (from SearchResult search in searcher.FindAll()
                    where search.Properties["name"] != null && search.Properties["name"].Count > 0
                    select new ExtendedSelectListItem
                    {
                        Value = search.Properties["adspath"][0].ToString(),
                        Text = search.Properties["name"][0].ToString()
                    }).ToList();
        }
        #endregion
    }
}
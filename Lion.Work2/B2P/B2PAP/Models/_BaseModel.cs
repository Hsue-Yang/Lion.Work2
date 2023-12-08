using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Web.ERPHelper;

namespace B2PAP
{
    public enum EnumConnectionStringsKey
    {
        LionGroupB2P
    }
}

namespace B2PAP.Models
{
    public partial class _BaseModel
    {
        protected string GetRootPathFilePath(EnumRootPathFile enumRootPathFile)
        {
            return Path.Combine(
                new string[]
                    {
                        ConfigurationManager.AppSettings[EnumAppSettingKey.RootPath.ToString()], 
                        string.Format(Common.GetEnumDesc(enumRootPathFile), Common.GetDateString())
                    });
        }

        string _systemControllerAction;
        public string SystemControllerAction { get { return _systemControllerAction; } set { _systemControllerAction = value; } }

        string _connectionStringB2P;
        protected string ConnectionStringB2P { get { return _connectionStringB2P; } set { _connectionStringB2P = value; } }

        string _providerNameB2P;
        protected string ProviderNameB2P { get { return _providerNameB2P; } set { _providerNameB2P = value; } }

        public int RowCount { get; set; }

        int _pageIndex = 1;
        public int PageIndex { get { return _pageIndex; } set { _pageIndex = value; } }

        int _pageSize = 10;
		public int PageSize { get { return _pageSize == 0 || _pageSize > 100 ? 10 : _pageSize; } set { _pageSize = value; } }

        int _pageCount = 0;
        public int PageCount { get { return _pageCount; } set { _pageCount = value; } }

		public LionTech.Web.ERPHelper.EnumActionType ExecAction { get; set; }

        public _BaseModel()
        {
            try
            {
                _connectionStringB2P = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupB2P.ToString()].ConnectionString;
                _providerNameB2P = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupB2P.ToString()].ProviderName;

                ExecAction = LionTech.Web.ERPHelper.EnumActionType.Select;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }

        protected void OnException(Exception exception)
        {
            FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), exception);
        }

        public bool GetValidateUserSystemRoleFun(string userID, EnumSystemID systemID, string controllerID, string actionName, string sessionID, EnumYN isOutside)
        {
            try
            {
                Entity_Base.UserSystemRoleFunPara userSystemRoleFunPara = new Entity_Base.UserSystemRoleFunPara()
                {
                    UserID = new DBVarChar(userID),
                    SystemID = new DBVarChar(systemID.ToString()),
                    ControllerID = new DBVarChar(controllerID),
                    ActionName = new DBVarChar(actionName),
                    SessionID = new DBChar(sessionID),
                    IsOutside = new DBChar(isOutside.ToString())
                };

                if (new Entity_Base(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .ValidateUserSystemRoleFun(userSystemRoleFunPara) == Entity_Base.EnumValidateUserSystemRoleFunResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return false;
        }
        
        protected List<T> GetEntitysByPage<T>(List<T> entitys, int pageSize)
        {
            if (entitys == null)
                return entitys;

            this.PageSize = pageSize;
            this.RowCount = entitys.Count;
            this.PageCount = (entitys.Count() % this.PageSize) == 0 ? (entitys.Count() / this.PageSize) - 1 : entitys.Count() / this.PageSize;
            if ((this.PageSize * this.PageIndex) >= entitys.Count())
                return entitys.Skip(this.PageSize * this.PageCount).Take(this.PageSize).ToList();
            return entitys.Skip(this.PageSize * (this.PageIndex - 1)).Take(this.PageSize).ToList();
        }

        public Dictionary<string, string> GetDictionaryFormSelectItem<T>(List<T> entityList, bool isBlank = false)
        {
            Dictionary<string, string> listItem = new Dictionary<string, string>();
            if (isBlank)
                listItem.Add(string.Empty, string.Empty);
            if (entityList != null)
            {
                entityList.ForEach(entity =>
                {
                    if (entity.GetType().GetInterface(typeof(DBEntity.ISelectItem).Name) != null)
                        listItem.Add(((DBEntity.ISelectItem)entity).ItemValue(), ((DBEntity.ISelectItem)entity).ItemText());
                });
            }
            return listItem;
        }

        public List<ExtendedSelectListItem> GetEntitySelectList(IEnumerable<DBEntity.IExtendedSelectItem> entityList, bool isBlank, string selectedValue)
        {
            try
            {
                List<ExtendedSelectListItem> items = new List<ExtendedSelectListItem>();
                if (isBlank) items.Add(new ExtendedSelectListItem { Text = string.Empty, Value = string.Empty, Background = string.Empty });

                if (entityList != null)
                {
                    foreach (DBEntity.IExtendedSelectItem baseTable in entityList)
                    {
                        ExtendedSelectListItem item = new ExtendedSelectListItem { Text = baseTable.ItemText(), Value = baseTable.ItemValue(), Background = baseTable.Background() };
                        if (item.Value == selectedValue) item.Selected = true;
                        items.Add(item);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return null;
        }

        public string GetJsonFormSelectItem<T>(List<T> entityList, bool isBlank = false)
        {
            string returnJsonString = string.Empty;
            StringBuilder jsonString = new StringBuilder();

            if (isBlank)
            {
                jsonString.Append("{");
                jsonString.Append(string.Concat(new object[] { "\"Text\"", ":\"\", " }));
                jsonString.Append(string.Concat(new object[] { "\"Value\"", ":\"\" " }));
                jsonString.Append("}");
            }

            if (entityList != null)
            {
                entityList.ForEach(entity =>
                {
                    if (entity.GetType().GetInterface(typeof(DBEntity.ISelectItem).Name) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(jsonString.ToString()))
                            jsonString.Append(",");
                        jsonString.Append("{");
                        jsonString.Append(string.Concat(new object[] { "\"Text\"", ":\"", ((DBEntity.ISelectItem)entity).ItemText(), "\", " }));
                        jsonString.Append(string.Concat(new object[] { "\"Value\"", ":\"", ((DBEntity.ISelectItem)entity).ItemValue(), "\" " }));
                        jsonString.Append("}");
                    }
                });
            }

            returnJsonString = string.Concat(new object[] { " [ ", jsonString.ToString(), " ] " });

            return returnJsonString;
        }
    }
}
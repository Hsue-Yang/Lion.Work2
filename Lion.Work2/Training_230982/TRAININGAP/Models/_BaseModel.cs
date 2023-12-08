using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace TRAININGAP
{
    public enum EnumConnectionStringsKey
    {
        LionGroupSERP, LionGroupTRAINING
    }
}

namespace TRAININGAP.Models
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

        string _connectionStringSERP;
        protected string ConnectionStringSERP { get { return _connectionStringSERP; } set { _connectionStringSERP = value; } }

        string _providerNameSERP;
        protected string ProviderNameSERP { get { return _providerNameSERP; } set { _providerNameSERP = value; } }

        string _connectionStringTRAINING;
        protected string ConnectionStringTRAINING { get { return _connectionStringTRAINING; } set { _connectionStringTRAINING = value; } }

        string _providerNameTRAINING;
        protected string ProviderNameTRAINING { get { return _providerNameTRAINING; } set { _providerNameTRAINING = value; } }

        public int RowCount { get; set; }

        int _pageIndex = 1;
        public int PageIndex { get { return _pageIndex; } set { _pageIndex = value; } }

        int _pageSize = 10;
        public int PageSize { get { return _pageSize == 0 ? 10 : _pageSize; } set { _pageSize = value; } }

        int _pageCount = 0;
        public int PageCount { get { return _pageCount; } set { _pageCount = value; } }

        public LionTech.Web.ERPHelper.EnumActionType ExecAction { get; set; }

        public _BaseModel()
        {
            try
            {
                _connectionStringSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ConnectionString;
                _providerNameSERP = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ProviderName;

                _connectionStringTRAINING = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupTRAINING.ToString()].ConnectionString;
                _providerNameTRAINING = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupTRAINING.ToString()].ProviderName;

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

        public Entity_Base.RestrictType GetValidateUserIsRestrict(string userID, string ip)
        {
            try
            {
                Entity_Base.UserMainPara userMainPara = new Entity_Base.UserMainPara()
                {
                    UserID = new DBVarChar(userID)
                };

                Entity_Base entityBase = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP);

                Entity_Base.EnumSelectUserRestrictTypeResult restrictType = entityBase.SelectUserRestrictType(userMainPara);

                Entity_Base.RestrictType result = new Entity_Base.RestrictType();
                result.IsRestrict = true;
                result.IsPowerUser = false;

                switch (restrictType)
                {
                    case Entity_Base.EnumSelectUserRestrictTypeResult.N:
                        result.IsRestrict = false;
                        break;
                    case Entity_Base.EnumSelectUserRestrictTypeResult.R:
                        result.IsRestrict = true;
                        break;
                    case Entity_Base.EnumSelectUserRestrictTypeResult.I:
                        Entity_Base.TrustIPPara trustIPPara = new Entity_Base.TrustIPPara()
                        {
                            IpAddress = new DBVarChar((string.IsNullOrWhiteSpace(ip) ? null : ip))
                        };

                        result.IsRestrict = (entityBase.ValidateIPAddressIsInternal(trustIPPara) == Entity_Base.EnumValidateIPAddressIsInternalResult.Success ? false : true);
                        break;
                    case Entity_Base.EnumSelectUserRestrictTypeResult.U:
                        result.IsRestrict = false;
                        result.IsPowerUser = true;
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return null;
        }

        public Entity_Base.EnumValidateIPAddressIsTrustResult GetValidateIPAddressIsTrust(string ip)
        {
            try
            {
                Entity_Base.TrustIPPara trustIPPara = new Entity_Base.TrustIPPara()
                {
                    IpAddress = new DBVarChar((string.IsNullOrWhiteSpace(ip) ? null : ip))
                };

                Entity_Base.EnumValidateIPAddressIsTrustResult result = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ValidateIPAddressIsTrust(trustIPPara);

                return result;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return Entity_Base.EnumValidateIPAddressIsTrustResult.UnTrust;
        }

        public bool GetValidateUserSystemRoleFun(string userID, EnumSystemID systemID, string controllerID, string actionName, string sessionID, string ip, EnumYN isOutside)
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
                    IPAddress = new DBVarChar(ip),
                    IsOutside = new DBChar(isOutside.ToString())
                };

                if (new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
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

        List<LionTech.Entity.ERP.Entity_Base.RAWUser> _entityBaseRAWUserList;
        public List<LionTech.Entity.ERP.Entity_Base.RAWUser> EntityBaseRAWUserList { get { return _entityBaseRAWUserList; } }

        public bool GetBaseRAWUserList(string condition)
        {
            try
            {
                if (condition != null && condition != string.Empty)
                {
                    LionTech.Entity.ERP.Entity_Base.RAWUserPara rawUserPara = new LionTech.Entity.ERP.Entity_Base.RAWUserPara()
                    {
                        Condition = new DBObject(condition),
                    };

                    _entityBaseRAWUserList = new LionTech.Entity.ERP.Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectRAWUserList(rawUserPara);

                    if (_entityBaseRAWUserList != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _entityBaseRAWUserList = null;
                this.OnException(ex);
            }
            return false;
        }

        #region - FunTool -

        string _toolNo;
        public string ToolNo { get { return _toolNo; } set { _toolNo = value; } }

        string _toolNM;
        public string ToolNM { get { return _toolNM; } set { _toolNM = value; } }

        LionTech.Utility.ERP.FunTool _funTool;
        public LionTech.Utility.ERP.FunTool FunTool { get { return _funTool; } }

        public List<LionTech.Utility.ERP.FunToolData> GetSysFunToolList(string userID, string sysID, string funControllerID, string funActionName)
        {
            try
            {
                _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);

                return _funTool.GetUserSysFunToolList(userID, sysID, funControllerID, funActionName);
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }

            return new List<LionTech.Utility.ERP.FunToolData>();
        }

        public bool SetSysFunToolPara(string userSystemFunKey, Dictionary<string, string> paraDict)
        {
            return this.SetSysFunToolPara(userSystemFunKey, this.ToolNo, this.ToolNM, paraDict);
        }

        public bool SetSysFunToolPara(string userSystemFunKey, string toolNo, Dictionary<string, string> paraDict)
        {
            return this.SetSysFunToolPara(userSystemFunKey, toolNo, Resources.Resource.Text_FunTool_DefaultNM, paraDict);
        }

        public bool SetSysFunToolPara(string userSystemFunKey, string toolNo, string toolNM, Dictionary<string, string> paraDict)
        {
            try
            {
                string userID = string.Empty;
                string sysID = string.Empty;
                string funControllerID = string.Empty;
                string funActionName = string.Empty;

                if (!string.IsNullOrEmpty(userSystemFunKey))
                {
                    userID = userSystemFunKey.Split('|')[0];
                    sysID = userSystemFunKey.Split('|')[1];
                    funControllerID = userSystemFunKey.Split('|')[2];
                    funActionName = userSystemFunKey.Split('|')[3];
                }

                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(sysID) &&
                    !string.IsNullOrEmpty(funControllerID) && !string.IsNullOrEmpty(funActionName))
                {
                    _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);
                    if (_funTool.SetUserSysFunTool(userID, sysID, funControllerID, funActionName, toolNo, toolNM, paraDict))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return false;
        }

        public bool GetUpdateSysFunTool(string userID, string sysID, string funControllerID, string funActionName, string toolNo)
        {
            try
            {
                _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);
                if (_funTool.GetUpdateUserSysFunTool(userID, sysID, funControllerID, funActionName, toolNo))
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

        public bool GetCopySysFunTool(string userID, string sysID, string funControllerID, string funActionName, string defaultToolNo, string copyToolNo, string copyUserID)
        {
            try
            {
                _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);
                if (_funTool.GetCopyUserSysFunTool(userID, sysID, funControllerID, funActionName, defaultToolNo, copyToolNo, copyUserID))
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

        public bool GetDeleteSysFunTool(string userID, string sysID, string funControllerID, string funActionName, string toolNo)
        {
            try
            {
                _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);
                if (_funTool.GetDeleteUserSysFunTool(userID, sysID, funControllerID, funActionName, toolNo))
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

        public bool GetUpdateSysFunToolName(string userID, string SysID, string funToolControllerID, string funToolActionName, string toolNo, ref string toolNM)
        {
            try
            {
                _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);
                if (_funTool.GetUpdatUserSysFunTooleName(userID, SysID, funToolControllerID, funToolActionName, toolNo, ref toolNM))
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

        public bool GetSelectSysFunToolName(string userID, string SysID, string funToolControllerID, string funToolActionName, string toolNo, ref string toolNM)
        {
            try
            {
                _funTool = new LionTech.Utility.ERP.FunTool(this.ConnectionStringSERP, this.ProviderNameSERP);
                if (_funTool.GetSelectUserSysFunTooleNameList(userID, SysID, funToolControllerID, funToolActionName, toolNo, ref toolNM))
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

        public enum EnumSysFunToolStatus
        {
            None,
            Query,
            Update,
            Create,
            Copy,
            Delete
        }

        public EnumSysFunToolStatus GetSysFunToolStatus()
        {
            if (this.ExecAction == EnumActionType.Add && !string.IsNullOrWhiteSpace(this.ToolNM))
            {
                return EnumSysFunToolStatus.Create;
            }

            if (this.ExecAction == EnumActionType.Update && !string.IsNullOrWhiteSpace(this.ToolNo))
            {
                return EnumSysFunToolStatus.Update;
            }

            if (this.ExecAction == EnumActionType.Query && !string.IsNullOrWhiteSpace(this.ToolNo))
            {
                return EnumSysFunToolStatus.Query;
            }

            return EnumSysFunToolStatus.None;
        }

        #endregion

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
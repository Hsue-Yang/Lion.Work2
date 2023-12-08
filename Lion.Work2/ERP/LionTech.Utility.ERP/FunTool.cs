using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;

namespace LionTech.Utility.ERP
{
    public class FunTool
    {
        string _connectionStringSERP;
        string _providerNameSERP;

        public FunTool(string connectionString, string providerName)
        {
            _connectionStringSERP = connectionString;
            _providerNameSERP = providerName;
        }

        public List<FunToolData> GetUserSysFunToolList(string userID, string sysID, string funControllerID, string funActionName)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName)
            };

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);

            List<FunToolData> funToolDataList;
            List<Entity_Base.UserSysFunTool> entityList = entity_Base.SelectUserSysFunToolList(para);

            if (entityList != null)
            {
                funToolDataList = new List<FunToolData>();

                List<string> logNoList = (from entity in entityList
                                          select entity.ToolNo.GetValue()).Distinct().ToList();

                foreach (string logNo in logNoList)
                {
                    FunToolData funToolData = new FunToolData();

                    List<Entity_Base.UserSysFunTool> tempList = entityList.Where(e => e.ToolNo.GetValue() == logNo).ToList();

                    funToolData.ToolNo = tempList[0].ToolNo.GetValue();
                    funToolData.ToolNM = tempList[0].ToolNM.GetValue();
                    funToolData.IsCurrently = tempList[0].IsCurrently.GetValue();

                    funToolData.ParaDict = new ExtensionDictionary<string, string>();
                    funToolData.ParaDict.Set(tempList.ToDictionary(e => e.ParaID.GetValue(), e => e.ParaValue.GetValue()));

                    funToolDataList.Add(funToolData);
                }

                return funToolDataList;
            }

            return null;
        }

        public bool SetUserSysFunTool(string userID, string sysID, string funControllerID, string funActionName, string toolNo, string toolNM, Dictionary<string, string> paraDict)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                ToolNo = new DBChar(string.IsNullOrWhiteSpace(toolNo) ? null : toolNo),
                ToolNM = new DBNVarChar(string.IsNullOrWhiteSpace(toolNM) ? null : toolNM),
                UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
            };

            List<Entity_Base.UserSysFunToolPara> paraList = new List<Entity_Base.UserSysFunToolPara>();
            if (paraDict != null && paraDict.Count > 0)
            {
                foreach (KeyValuePair<string, string> data in paraDict)
                {
                    paraList.Add(new Entity_Base.UserSysFunToolPara()
                    {
                        UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                        SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                        FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                        FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                        ToolNo = new DBChar(string.IsNullOrWhiteSpace(toolNo) ? null : toolNo),
                        ParaID = new DBVarChar(string.IsNullOrWhiteSpace(data.Key) ? null : data.Key),
                        ParaValue = new DBNVarChar(string.IsNullOrWhiteSpace(data.Value) ? null : data.Value),
                        UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                    });
                }
            }

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);

            if (!string.IsNullOrWhiteSpace(toolNo))
            {
                if (Common.GetTextValidationResult(toolNo, @"^\d{6}$") &&
                    entity_Base.RebuildUserSysFunTool(para, paraList) == Entity_Base.EnumRebuildUserSysFunToolResult.Success)
                {
                    return true;
                }
            }
            else
            {
                if (entity_Base.CreateUserSysFunTool(para, paraList) == Entity_Base.EnumCreateUserSysFunToolResult.Success)
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetUpdateUserSysFunTool(string userID, string sysID, string funControllerID, string funActionName, string toolNo)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                ToolNo = new DBChar(string.IsNullOrWhiteSpace(toolNo) ? null : toolNo),
                UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
            };

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);

            if (entity_Base.UpdateUserSysFunTool(para) == Entity_Base.EnumUpdateUserSysFunToolResult.Success)
            {
                return true;
            }

            return false;
        }

        public bool GetCopyUserSysFunTool(string userID, string sysID, string funControllerID, string funActionName, string defaultToolNo, string copyToolNo, string copyUserID)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                ToolNo = new DBChar(string.IsNullOrWhiteSpace(defaultToolNo) ? null : defaultToolNo),
                CopyToolNo = new DBChar(string.IsNullOrWhiteSpace(copyToolNo) ? null : copyToolNo),
                CopyUserID = new DBVarChar(string.IsNullOrWhiteSpace(copyUserID) ? null : copyUserID),
                UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
            };

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);

            if (entity_Base.CopyUserSysFunTool(para) == Entity_Base.EnumCopyUserSysFunToolResult.Success)
            {
                return true;
            }

            return false;
        }

        public bool GetDeleteUserSysFunTool(string userID, string sysID, string funControllerID, string funActionName, string toolNo)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                ToolNo = new DBChar(string.IsNullOrWhiteSpace(toolNo) ? null : toolNo),
                UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
            };

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);

            if (entity_Base.DeleteUserSysFunTool(para) == Entity_Base.EnumDeleteUserSysFunToolResult.Success)
            {
                return true;
            }

            return false;
        }

        public bool GetUpdatUserSysFunTooleName(string userID, string sysID, string funControllerID, string funActionName, string toolNo, ref string toolNM)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                ToolNo = new DBChar(string.IsNullOrWhiteSpace(toolNo) ? null : toolNo),
                ToolNM = new DBNVarChar(string.IsNullOrWhiteSpace(toolNM) ? null : toolNM),
                UpdUserID = new DBVarChar(userID)
            };

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);

            if (entity_Base.UpdateUserSysFunToolName(para, ref toolNM) == Entity_Base.EnumUpdateNameUserSysFunToolResult.Success)
            {
                return true;
            }
            return false;
        }

        public bool GetSelectUserSysFunTooleNameList(string userID, string sysID, string funControllerID, string funActionName, string toolNo, ref string toolNM)
        {
            Entity_Base.UserSysFunToolPara para = new Entity_Base.UserSysFunToolPara()
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                SysID = new DBVarChar(string.IsNullOrWhiteSpace(sysID) ? null : sysID),
                FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID),
                FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(funActionName) ? null : funActionName),
                ToolNo = new DBChar(string.IsNullOrWhiteSpace(toolNo) ? null : toolNo)
            };

            Entity_Base entity_Base = new Entity_Base(_connectionStringSERP, _providerNameSERP);
            Entity_Base.UserSysFunTool entityList = entity_Base.SelectUserSysFunToolNameList(para);

            if (entityList != null)
            {
                toolNM = entityList.ToolNM.GetValue();

                return true;
            }
            return false;
        }
    }

       

    public class FunToolData : DBEntity.ISelectItem
    {
        public string ToolNo { get; set; }

        public string ToolNM { get; set; }

        public string IsCurrently { get; set; }

        public ExtensionDictionary<string, string> ParaDict { get; set; }

        public string ItemText()
        {
            return string.Format("{0} ({1})", this.ToolNM, this.ToolNo);
        }

        public string ItemValue()
        {
            return this.ToolNo;
        }

        public string ItemValue(string key)
        {
            throw new NotImplementedException();
        }

        public string PictureUrl()
        {
            throw new NotImplementedException();
        }

        public string GroupBy()
        {
            throw new NotImplementedException();
        }

        public bool HasFunToolPara()
        {
            if (this.ParaDict != null && this.ParaDict.Count > 0)
            {
                return true;
            }

            return false;
        }
    }

    public class FunToolService
    {
        public enum FunToolController
        {
            _BaseAP, Generic
        }

        public enum FunToolActionName
        {
            GetUpdateSysFunToolResult,
            GetCopySysFunToolResult,
            GetDeleteSysFunToolResult,
            GetBaseRAWUserList,
            GetUpdateSysFunToolNameResult,
            GetSelectSysFunToolNameList,
        }
    }
}
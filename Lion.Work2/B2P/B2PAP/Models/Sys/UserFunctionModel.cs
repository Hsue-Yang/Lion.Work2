using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class UserFunctionModel : SysModel
    {
        public enum Field
        {
            SysID,
            FunControllerID, FunActionName
        }

        public EnumCultureID CurrentCultureID { get; set; }

        public string UserID { get; set; }

        public string IsDisable { get; set; }

        [Required]
        public string SysID { get; set; }

        [Required]
        public string FunControllerID { get; set; }

        [Required]
        public string FunActionName { get; set; }

        public UserFunctionModel()
        {

        }

        public void FormReset()
        {

        }

        EntityUserFunction.UserRawData _entityUserRawData;
        public EntityUserFunction.UserRawData EntityUserRawData { get { return _entityUserRawData; } }

        public bool GetUserRawData()
        {
            try
            {
                EntityUserFunction.UserRawDataPara para = new EntityUserFunction.UserRawDataPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserRawData = new EntityUserFunction(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserRawData(para);

                if (_entityUserRawData != null)
                {
                    this.IsDisable = _entityUserRawData.IsDisable.GetValue();
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntityUserFunction.UserFunction> _entityUserFunctionList;
        public List<EntityUserFunction.UserFunction> EntityUserFunctionList { get { return _entityUserFunctionList; } }

        public bool GetUserFunctionList(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityUserFunction.UserFunctionPara para = new EntityUserFunction.UserFunctionPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    UpdUserID = new DBVarChar(userID)
                };

                _entityUserFunctionList = new EntityUserFunction(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserFunctionList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEditUserFunctionResult(List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList, string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityUserFunction.UserFunctionPara para = new EntityUserFunction.UserFunctionPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : this.IsDisable)),
                    FunctionList = new List<DBVarChar>(),
                    UpdUserID = new DBVarChar(userID)
                };

                if (systemUserFunctionValueList != null && systemUserFunctionValueList.Count > 0)
                {
                    foreach (EntityUserFunction.SystemUserFunctionValue userFunctionValue in systemUserFunctionValueList)
                    {
                        if (!string.IsNullOrWhiteSpace(userFunctionValue.SysID) &&
                            !string.IsNullOrWhiteSpace(userFunctionValue.FunControllerID) &&
                            !string.IsNullOrWhiteSpace(userFunctionValue.FunActionName))
                        {
                            para.FunctionList.Add(new DBVarChar(string.Format("{0}|{1}|{2}",
                                                                              userFunctionValue.SysID,
                                                                              userFunctionValue.FunControllerID,
                                                                              userFunctionValue.FunActionName)));
                        }
                    }
                }

                if (new EntityUserFunction(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditUserFunctionResult(para) == LionTech.Entity.B2P.Sys.EntityUserFunction.EnumEditUserFunctionResult.Success)
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

        public bool GenerateUserMenuXML(string filePath, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.UserID))
                    return false;

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserMenuFunList(para);

                if (Utility.GenerateUserMenuXML(userFunMenuList, this.UserID, filePath) == Entity_BaseAP.EnumGenerateUserMenuXMLResult.Success)
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
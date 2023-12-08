using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Authorization;
using LionTech.Entity.ERP.Sys;

namespace ERPAPI.Models.Authorization
{
    public class ERPUserFunctionModel : AuthorizationModel
    {
        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string APIPara { get; set; }
        #endregion

        public class APIParaData
        {
            public string UpdUserID { get; set; }
            public string UserID { get; set; }
            public List<string> FunctionList { get; set; }
        }

        public APIParaData APIData { get; set; }
        
        public bool EditUserFunction()
        {
            try
            {
                string isDisable = EnumYN.N.ToString();

                EntityUserFunction.UserRawData userRawData = new EntityUserFunction.UserRawData();
                EntityUserFunction.UserRawDataPara userRawDataPara = new EntityUserFunction.UserRawDataPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID))
                };

                userRawData = new EntityUserFunction(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserRawData(userRawDataPara);

                if (userRawData != null)
                {
                    isDisable = userRawData.IsDisable.GetValue();

                    EntityERPUserFunction.UserSystemPara userSystemPara = new EntityERPUserFunction.UserSystemPara()
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UpdUserID) ? null : APIData.UpdUserID))
                    };

                    List<EntityERPUserFunction.UserSystem> userSystemList = new EntityERPUserFunction(ConnectionStringSERP, ProviderNameSERP)
                        .SelectUserSystemList(userSystemPara);

                    EntityUserFunction.UserFunctionPara userFunctionPara = new EntityUserFunction.UserFunctionPara(string.Empty)
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)),
                        IsDisable = new DBChar((string.IsNullOrWhiteSpace(isDisable) ? EnumYN.N.ToString() : isDisable)),
                        FunctionList = new List<DBVarChar>(),
                        UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UpdUserID) ? null : APIData.UpdUserID))
                    };

                    if ((APIData.FunctionList != null && APIData.FunctionList.Count > 0) &&
                        (userSystemList != null && userSystemList.Count > 0))
                    {
                        EntityUserFunction.UserFunctionPara para = new EntityUserFunction.UserFunctionPara(EnumCultureID.zh_TW.ToString())
                        {
                            UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)),
                            UpdUserID = new DBVarChar(APIData.UserID)
                        };
                        var userFunctionList = new EntityUserFunction(ConnectionStringSERP, ProviderNameSERP)
                            .SelectUserFunctionList(para);

                        userFunctionPara.FunctionList.AddRange((from s in userFunctionList
                                                                select new DBVarChar(string.Format("{0}|{1}|{2}",
                                                                    s.SysID.GetValue(),
                                                                    s.FunControllerID.GetValue(),
                                                                    s.FunActionName.GetValue()))));

                        foreach (string function in APIData.FunctionList)
                        {
                            if (!string.IsNullOrWhiteSpace(function))
                            {
                                EntityERPUserFunction.UserSystem userSystem = userSystemList.Find(e => e.SysID.GetValue().Contains(function.Split('|')[0]));

                                if (userSystem != null &&
                                    userFunctionPara.FunctionList.Exists(e => e.GetValue() == function) == false)
                                {
                                    userFunctionPara.FunctionList.Add(new DBVarChar(function));
                                }
                            }
                        }
                    }

                    if (new EntityUserFunction(ConnectionStringSERP, ProviderNameSERP)
                        .EditUserFunctionResult(userFunctionPara) == EntityUserFunction.EnumEditUserFunctionResult.Success)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}
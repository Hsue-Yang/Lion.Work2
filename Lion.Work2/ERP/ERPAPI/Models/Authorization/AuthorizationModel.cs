using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;

namespace ERPAPI.Models.Authorization
{
    public class AuthorizationModel : _BaseAPModel
    {
        public AuthorizationModel()
        {

        }

        public string GenerateUserMenuXML(string userID, string filePath, EnumCultureID cultureID, bool IsDevEnv = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userID))
                    return null;

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(userID)
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserMenuFunList(para);

                if (userFunMenuList != null)
                {
                    Utility.GenerateUserMenuXML(userFunMenuList, userID, filePath, IsDevEnv);

                    var userMenu = LionTech.Entity.ERP.Utility.GetUserMenu(userFunMenuList, userID, IsDevEnv);
                    var xml = LionTech.Entity.ERP.Utility.StringToXmlDocument(userMenu.SerializeToXml());
                    return xml.OuterXml;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
    }
}
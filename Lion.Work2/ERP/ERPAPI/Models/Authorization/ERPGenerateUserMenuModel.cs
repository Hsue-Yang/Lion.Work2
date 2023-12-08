// 新增日期：2017-10-02
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Authorization;

namespace ERPAPI.Models.Authorization
{
    public class ERPGenerateUserMenuModel : AuthorizationModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string UserID { get; set; }
            public bool IsDevEnv { get; set; }
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string APIPara { get; set; }
        public APIParaData APIData { get; set; }
        #endregion

        /// <summary>
        /// 編輯使用者功能選單資訊
        /// </summary>
        /// <param name="useID"></param>
        /// <returns></returns>
        public bool EditUserFunInfo(string useID)
        {
            try
            {
                EntityERPGenerateUserMenu.EditUserFunInfoPara para = new EntityERPGenerateUserMenu.EditUserFunInfoPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(useID) ? null : useID))
                };

                return new EntityERPGenerateUserMenu(ConnectionStringSERP, ProviderNameSERP).EditUserFunInfo(para)
                    == EntityERPGenerateUserMenu.EnumEditUserFunInfoResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
    }
}
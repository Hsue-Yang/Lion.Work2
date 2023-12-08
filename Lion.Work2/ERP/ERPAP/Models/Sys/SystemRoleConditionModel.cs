using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemRoleConditionModel : SysModel
    {
        public class SystemRoleCondition
        {
            public string SysID { get; set; }
            public string RoleConditionID { get; set; }
            public string RoleConditionNM { get; set; }
            public string SortOrder { get; set; }
            public string Remark { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
            public string SysRole { get; set; }
        }

        public List<SystemRoleCondition> SystemRoleConditions { get; set; }
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID,
            ConditionID,
            IncludeRoleID
        }
        #endregion

        #region - Constructor -
        public SystemRoleConditionModel()
        {
            _entity = new EntitySystemRoleCondition(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string SysID { get; set; }
        public string ConditionID { get; set; }
        public string IncludeRoleID { get; set; }
        public List<EntitySystemRoleCondition.SysSystemRoleCondition> SystemRoleConditionList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemRoleCondition _entity;
        #endregion

        #region - 取得系統角色預設條件清單 -
        /// <summary>
        /// 取得系統角色預設條件清單
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> GetSysSystemRoleConditionList(EnumCultureID cultureId)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                ConditionID = string.IsNullOrWhiteSpace(ConditionID) ? null : ConditionID;
                IncludeRoleID = string.IsNullOrWhiteSpace(IncludeRoleID) ? null : IncludeRoleID;

                string apiUrl = API.SystemRoleCondition.QuerySystemRoleConditions(ConditionID, IncludeRoleID, SysID, cultureId.ToString().ToUpper(), PageIndex, PageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemRoleConditionList = (List<SystemRoleCondition>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                RowCount = responseObj.RowCount;
                SystemRoleConditions = responseObj.SystemRoleConditionList;
                return true;
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
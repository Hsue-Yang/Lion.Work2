using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemRoleConditionDetailModel : SysModel, IValidatableObject
    {
        public class SystemRoleConditionDetail
        {
            public string SysID { get; set; }
            public string RoleConditionID { get; set; }
            public string RoleConditionNMZHTW { get; set; }
            public string RoleConditionNMZHCN { get; set; }
            public string RoleConditionNMENUS { get; set; }
            public string RoleConditionNMTHTH { get; set; }
            public string RoleConditionNMJAJP { get; set; }
            public string RoleConditionNMKOKR { get; set; }
            public string RoleConditionSyntax { get; set; }
            public string SortOrder { get; set; }
            public string Remark { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDT { get; set; }
            public string RoleID { get; set; }
            public string SysRole { get; set; }
            public QueryCondition.GroupRule RoleConditionRules { get; set; }
        }

        public SystemRoleConditionDetail SysRoleConditionDetail { get; set; }

        public class SystemRoleConditionDetailPara
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string RoleConditionID { get; set; }
            public string RoleConditionNMZHTW { get; set; }
            public string RoleConditionNMZHCN { get; set; }
            public string RoleConditionNMENUS { get; set; }
            public string RoleConditionNMTHTH { get; set; }
            public string RoleConditionNMJAJP { get; set; }
            public string RoleConditionNMKOKR { get; set; }
            public string RoleConditionSyntax { get; set; }
            public string SortOrder { get; set; }
            public string Remark { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
            public List<string> RoleList { get; set; }
            public QueryCondition.GroupRule RoleConditionRules { get; set; }
        }

        #region - Definitions -
        public enum EnumConditionType
        {
            AccordingCondition,
            Default
        }
        #endregion

        #region - Constructor -
        public SystemRoleConditionDetailModel()
        {
            _entity = new EntitySystemRoleConditionDetail(ConnectionStringSERP, ProviderNameSERP);
            _mongoEntity = new MongoSystemRoleConditionDetail(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ConditionID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ConditionNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ConditionNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ConditionNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ConditionNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ConditionNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string ConditionNMKOKR { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public string ConditionType { get; set; }

        public List<string> SysRoleList { get; set; }

        private List<TabStripHelper.Tab> _tabList;

        public List<TabStripHelper.Tab> TabList
        {
            get
            {
                return _tabList = _tabList ?? (new List<TabStripHelper.Tab>
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName = string.Empty,
                        ActionName = string.Empty,
                        TabText = SysSystemRoleConditionDetail.TabText_SystemRoleConditionDetail,
                        ImageURL = string.Empty
                    }
                });
            }
        }

        private Dictionary<string, string> _conditionTypeDic;

        public Dictionary<string, string> ConditionTypeDic
        {
            get
            {
                return _conditionTypeDic ?? (_conditionTypeDic = new Dictionary<string, string>
                {
                    { EnumConditionType.AccordingCondition.ToString(), SysSystemRoleConditionDetail.Label_AccordingCondition },
                    { EnumConditionType.Default.ToString(), SysSystemRoleConditionDetail.Label_Default }
                });
            }
        }


        public QueryCondition.GroupRule SystemRoleConditionGroupRule { get; set; }

        public string SystemRoleConditionFilterJsonString { get; private set; }

        public string SystemRoleConditionRulesJsonString { get; private set; }
        #endregion

        #region - Field -
        public string SysNM;
        #endregion

        #region - Private -
        private readonly EntitySystemRoleConditionDetail _entity;
        private readonly MongoSystemRoleConditionDetail _mongoEntity;
        private string _conditionSynTax;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region - 角色資料驗證 -
            if (SysRoleList == null ||
                SysRoleList.Any() == false)
            {
                yield return new ValidationResult(SysSystemRoleConditionDetail.SystemMsg_IsValidSysRoleRequired_Failure);
            }
            #endregion

            #region - 角色預設權限規則至少一筆 -
            if (SystemRoleConditionGroupRule == null)
            {
                yield return new ValidationResult(SysSystemRoleConditionDetail.SystemMsg_IsValidSysRoleRulesRequired_Failure);
            }
            #endregion
        }
        #endregion

        public void FormReset()
        {
            ConditionType = EnumConditionType.AccordingCondition.ToString();
        }

        #region - 系統角色預設條件表單Json資料初始化 -
        /// <summary>
        /// 系統角色預設條件表單Json資料初始化
        /// </summary>
        /// <param name="cultureId"></param>
        public bool SetSystemRoleConditionFilterJsonString(EnumCultureID cultureId)
        {
            try
            {
                SystemRoleConditionFilterJsonString = GetSystemRoleConditionFilterJsonString(cultureId);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得系統角色預設權限主檔資料 -
        /// <summary>
        /// 取得系統角色預設權限主檔資料
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> GetSysSystemRoleConditionDetail(EnumCultureID cultureId)
        {
            try
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                QueryCondition queryCondition = new QueryCondition();

                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                ConditionID = string.IsNullOrWhiteSpace(ConditionID) ? null : ConditionID;

                string apiUrl = API.SystemRoleCondition.QuerySystemRoleConditionDetail(SysID, ConditionID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemRoleConditionDetail = (SystemRoleConditionDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                SysRoleConditionDetail = responseObj.SystemRoleConditionDetail;

                if (SysRoleConditionDetail != null)
                {
                    ConditionNMZHTW = SysRoleConditionDetail.RoleConditionNMZHTW;
                    ConditionNMZHCN = SysRoleConditionDetail.RoleConditionNMZHCN;
                    ConditionNMENUS = SysRoleConditionDetail.RoleConditionNMENUS;
                    ConditionNMJAJP = SysRoleConditionDetail.RoleConditionNMJAJP;
                    ConditionNMTHTH = SysRoleConditionDetail.RoleConditionNMTHTH;
                    ConditionNMKOKR = SysRoleConditionDetail.RoleConditionNMKOKR;
                    Remark = SysRoleConditionDetail.Remark;
                    SortOrder = SysRoleConditionDetail.SortOrder;
                    _conditionSynTax = SysRoleConditionDetail.RoleConditionSyntax;
                    SysRoleList = SysRoleConditionDetail.SysRole.Split('、').ToList();

                    if (SysRoleConditionDetail.RoleConditionRules != null)
                    {
                        var rule = queryCondition.GetGroupRules(new List<QueryCondition.GroupRule> { SysRoleConditionDetail.RoleConditionRules }).SingleOrDefault();
                        SystemRoleConditionRulesJsonString = jsSerializer.Serialize(rule);
                    }
                }

                if (string.IsNullOrWhiteSpace(SystemRoleConditionRulesJsonString))
                {
                    ConditionType = EnumConditionType.Default.ToString();

                    SystemRoleConditionRulesJsonString =
                        jsSerializer.Serialize(
                            new QueryCondition.GroupRule
                            {
                                Condition = QueryCondition.EnumConditionType.AND.ToString(),
                                RuleList = new List<QueryCondition.RoleRule>
                                {
                                    new QueryCondition.RoleRule(),
                                    new QueryCondition.RoleRule()
                                },
                                GroupRuleList = new List<QueryCondition.GroupRule>
                                {
                                    new QueryCondition.GroupRule
                                    {
                                        Condition = QueryCondition.EnumConditionType.AND.ToString(),
                                        GroupRuleList = new List<QueryCondition.GroupRule>(),
                                        RuleList = new List<QueryCondition.RoleRule>
                                        {
                                            new QueryCondition.RoleRule(),
                                            new QueryCondition.RoleRule()
                                        }
                                    }
                                }
                            });
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 編輯系統角色預設權限 -
        /// <summary>
        /// 編輯系統角色預設權限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> EditSysSystemRoleCondition(EnumCultureID cultureId, string userID, string userNM)
        {
            try
            {
                if (ConditionType == EnumConditionType.AccordingCondition.ToString())
                {
                    _conditionSynTax = _CombineRoleConditionSyntax(SystemRoleConditionGroupRule);
                }

                var roleConditionRules = (ConditionType == EnumConditionType.AccordingCondition.ToString())
                        ? SystemRoleConditionToRules(new List<QueryCondition.GroupRule> { SystemRoleConditionGroupRule }).SingleOrDefault()
                        : null;

                var para =
                  new SystemRoleConditionDetailPara()
                  {
                      SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                      SysNM = string.IsNullOrWhiteSpace(SysNM) ? null : SysNM,
                      RoleConditionID = string.IsNullOrWhiteSpace(ConditionID) ? null : ConditionID,
                      RoleConditionNMZHTW = string.IsNullOrWhiteSpace(ConditionNMZHTW) ? null : ConditionNMZHTW,
                      RoleConditionNMZHCN = string.IsNullOrWhiteSpace(ConditionNMZHCN) ? null : ConditionNMZHCN,
                      RoleConditionNMENUS = string.IsNullOrWhiteSpace(ConditionNMENUS) ? null : ConditionNMENUS,
                      RoleConditionNMTHTH = string.IsNullOrWhiteSpace(ConditionNMTHTH) ? null : ConditionNMTHTH,
                      RoleConditionNMJAJP = string.IsNullOrWhiteSpace(ConditionNMJAJP) ? null : ConditionNMJAJP,
                      RoleConditionNMKOKR = string.IsNullOrWhiteSpace(ConditionNMKOKR) ? null : ConditionNMKOKR,
                      RoleConditionSyntax = string.IsNullOrWhiteSpace(_conditionSynTax) ? null : _conditionSynTax,
                      SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                      Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                      UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID,
                      UpdUserNM = string.IsNullOrWhiteSpace(userNM) ? null : userNM,
                      UpdDT = DateTime.Now,
                      RoleList = SysRoleList,
                      RoleConditionRules = roleConditionRules
                  };

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemRoleCondition.EditSystemRoleConditionDetail(userID);
                await PublicFun.HttpPutWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 刪除系統角色預設權限 -
        /// <summary>
        /// 刪除系統角色預設權限
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSystemRoleCondition(EnumCultureID cultureId, string userID)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                ConditionID = string.IsNullOrWhiteSpace(ConditionID) ? null : ConditionID;

                var apiUrl = API.SystemRoleCondition.DeleteSystemRoleConditionDetail(SysID, ConditionID, userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, requestMethod: "DELETE");
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 紀錄系統角色預設權限 -
        /// <summary>
        /// 紀錄系統角色預設權限
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="userID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool RecordLogSysSystemRoleCondition(EnumCultureID cultureID, string userID, string ipAddress)
        {
            try
            {
                Mongo_BaseAP.RecordLogSystemRoleCondotionPara para = new Mongo_BaseAP.RecordLogSystemRoleCondotionPara();
                para.SysID = new DBVarChar(SysID);
                para.SysNM = new DBNVarChar(SysNM);
                para.RoleConditionID = new DBVarChar(ConditionID);
                para.Roles = SysRoleList.Select(s => new DBVarChar(s)).ToList();
                para.RoleConditionNMZHTW = new DBVarChar(ConditionNMZHTW);
                para.RoleConditionNMZHCN = new DBVarChar(ConditionNMZHCN);
                para.RoleConditionNMENUS = new DBVarChar(ConditionNMENUS);
                para.RoleConditionNMTHTH = new DBVarChar(ConditionNMTHTH);
                para.RoleConditionNMJAJP = new DBVarChar(ConditionNMJAJP);
                para.RoleConditionNMKOKR = new DBVarChar(ConditionNMKOKR);
                para.RoleConditionSynTax = new DBNVarChar(_conditionSynTax);
                para.SortOrder = new DBVarChar(SortOrder);
                para.Remark = new DBVarChar(Remark);
                para.RoleConditionRules = _SystemRoleConditionToRules(new List<QueryCondition.GroupRule> { SystemRoleConditionGroupRule }).SingleOrDefault();

                Mongo_BaseAP.EnumModifyType modifyType;

                switch (ExecAction)
                {
                    case EnumActionType.Add:
                        modifyType = Mongo_BaseAP.EnumModifyType.I;
                        break;
                    case EnumActionType.Update:
                        modifyType = Mongo_BaseAP.EnumModifyType.U;
                        break;
                    case EnumActionType.Delete:
                        modifyType = Mongo_BaseAP.EnumModifyType.D;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_ROLE_CONDTION, modifyType, userID, ipAddress, cultureID, para);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 系統角色條件轉換規則 -
        /// <summary>
        /// 系統角色條件轉換規則
        /// </summary>
        /// <param name="systemRoleConGroupRule"></param>
        /// <returns></returns>
        private List<QueryCondition.GroupRule> SystemRoleConditionToRules(List<QueryCondition.GroupRule> systemRoleConGroupRule)
        {
            List<QueryCondition.GroupRule> result = new List<QueryCondition.GroupRule>();
            if (systemRoleConGroupRule != null &&
                systemRoleConGroupRule.Any())
            {
                result.AddRange((from s in systemRoleConGroupRule
                                 let groupRuleList = SystemRoleConditionToRules(s.GroupRuleList)
                                 where ((groupRuleList != null && groupRuleList.Any()) ||
                                        (s.RuleList != null && s.RuleList.Any(w => string.IsNullOrWhiteSpace(w.Operator) == false &&
                                                                                   string.IsNullOrWhiteSpace(w.ID) == false)))
                                 select new QueryCondition.GroupRule
                                 {
                                     Condition = s.Condition,
                                     RuleList =
                                         (s.RuleList ?? new List<QueryCondition.RoleRule>())
                                             .Select(c => new QueryCondition.RoleRule
                                             {
                                                 ID = c.ID,
                                                 Operator = c.Operator,
                                                 Value = c.Value
                                             }).ToList(),
                                     GroupRuleList = groupRuleList
                                 }));
            }
            return result;
        }

        private List<Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule> _SystemRoleConditionToRules(List<QueryCondition.GroupRule> systemRoleConGroupRule)
        {
            List<Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule> result = new List<Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule>();
            if (systemRoleConGroupRule != null &&
                systemRoleConGroupRule.Any())
            {
                result.AddRange((from s in systemRoleConGroupRule
                                 let groupRuleList = _SystemRoleConditionToRules(s.GroupRuleList)
                                 where ((groupRuleList != null && groupRuleList.Any()) ||
                                        (s.RuleList != null && s.RuleList.Any(w => string.IsNullOrWhiteSpace(w.Operator) == false &&
                                                                                   string.IsNullOrWhiteSpace(w.ID) == false)))
                                 select new Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule
                                 {
                                     Condition = new DBVarChar(s.Condition),
                                     RuleList =
                                         (s.RuleList ?? new List<QueryCondition.RoleRule>())
                                             .Select(c => new Mongo_BaseAP.RecordLogSystemRoleConditionRoleRule
                                             {
                                                 ID = new DBVarChar(c.ID),
                                                 Operator = new DBVarChar(c.Operator),
                                                 Value = new DBVarChar(c.Value)
                                             }).ToList(),
                                     GroupRuleList = groupRuleList
                                 }));
            }
            return result;
        }
        #endregion

        #region - 系統角色預設條件語法格式化 -
        public string GetRoleConditionSyntaxFormat(QueryCondition.GroupRule conditionGroup)
        {
            try
            {
                string result = _CombineRoleConditionSyntax(conditionGroup, 0);
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    return result.Substring(Environment.NewLine.Length);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return string.Empty;
        }
        #endregion

        #region - 組合系統角色預設條件語法 -
        private string _CombineRoleConditionSyntax(QueryCondition.GroupRule conditionGroup, int? emptyLength = null)
        {
            var result = new List<string>();

            if (conditionGroup.RuleList != null &&
                conditionGroup.RuleList.Any())
            {
                conditionGroup.RuleList =
                    (from s in conditionGroup.RuleList
                     where string.IsNullOrWhiteSpace(s.Operator) == false &&
                           string.IsNullOrWhiteSpace(s.ID) == false
                     select s).ToList();

                result.AddRange
                (
                    conditionGroup
                        .RuleList
                        .Select(roleRule =>
                        {
                            IDBType dbValue;
                            if (roleRule.FieldType == QueryCondition.EnumFieldType.Integer.ToString())
                            {
                                dbValue = new DBInt(roleRule.Value);
                            }
                            else
                            {
                                dbValue = new DBNVarChar(roleRule.Value);
                            }

                            bool isNullOperator = roleRule.Operator == QueryCondition.EnumOperatorType.IsNull.ToString() || roleRule.Operator == QueryCondition.EnumOperatorType.IsNotNull.ToString();
                            //bool isInOperator = roleRule.Operator == QueryCondition.EnumOperatorType.In.ToString() || roleRule.Operator == QueryCondition.EnumOperatorType.NotIn.ToString();
                            string columnName = Common.GetEnumDesc((EnumSystemRoleConditionColumnType)Enum.Parse(typeof(EnumSystemRoleConditionColumnType), roleRule.ID));
                            string operatorType = Common.GetEnumDesc(Enum.Parse(typeof(QueryCondition.EnumOperatorType), roleRule.Operator));
                            string syntax = $" {columnName} {operatorType} ";

                            if (isNullOperator == false)
                            {
                                string value = DBEntity.GetCommandText(ProviderNameSERP, "{value}", new List<DBParameter> { new DBParameter { Name = "value", Value = dbValue } });
                                syntax += $"{value} ";
                            }

                            if (emptyLength.HasValue)
                            {
                                syntax = Environment.NewLine + string.Empty.PadLeft(emptyLength.Value) + syntax;
                            }

                            return syntax;
                        })
                );
            }

            if (conditionGroup.GroupRuleList != null &&
                conditionGroup.GroupRuleList.Any())
            {
                conditionGroup.GroupRuleList =
                    (from s in conditionGroup.GroupRuleList
                     where ((s.GroupRuleList != null && s.GroupRuleList.Any()) ||
                            (s.RuleList != null && s.RuleList.Any(w => string.IsNullOrWhiteSpace(w.Operator) == false &&
                                                                       string.IsNullOrWhiteSpace(w.ID) == false)))
                     select s).ToList();

                result.AddRange((from groupRule in conditionGroup.GroupRuleList
                                 let syntax = _CombineRoleConditionSyntax(groupRule, emptyLength + 4)
                                 let composingSyntax = string.Format("({0}", syntax)
                                 let rightBrackets = ")"
                                 let composingBrackets = emptyLength.HasValue ? (Environment.NewLine + rightBrackets.PadLeft(emptyLength.Value)) : string.Empty
                                 where string.IsNullOrWhiteSpace(syntax) == false
                                 select emptyLength.HasValue == false
                                     ? composingSyntax + rightBrackets
                                     : string.Format("{0}{1}", Environment.NewLine + string.Empty.PadLeft((int)emptyLength) + composingSyntax, composingBrackets)));
            }

            return string.Join(conditionGroup.Condition, result);
        }
        #endregion

        #region - 取得應用系統名稱 -
        /// <summary>
        /// 取得應用系統名稱
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        internal bool GetSysNM(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar(null),
                    ExecSysID = new DBVarChar(SysID)
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(basicInfoPara);

                SysNM = entityBasicInfo.ExecSysNM.GetValue();

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
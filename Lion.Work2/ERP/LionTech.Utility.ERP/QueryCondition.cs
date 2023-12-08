using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LionTech.Entity.ERP;

namespace LionTech.Utility.ERP
{
    public class QueryCondition
    {
        public enum EnumConditionType
        {
            AND,
            OR
        }

        public enum EnumFieldType
        {
            String,
            Integer
        }

        public enum EnumInputType
        {
            Text,
            Select,
            Radio,
            CheckBox
        }

        public enum EnumOperatorType
        {
            [Description("=")]
            Equal,

            [Description("<>")]
            NotEqual,

            [Description("IN")]
            In,

            [Description("NOT IN")]
            NotIn,

            [Description("<")]
            Less,

            [Description("<=")]
            LessOrEqual,

            [Description(">")]
            Greater,

            [Description(">=")]
            GreaterOrEqual,

            [Description("LIKE")]
            Like,

            [Description("IS NULL")]
            IsNull,

            [Description("IS NOT NULL")]
            IsNotNull
        }

        public class RoleRule
        {
            public string ID { get; set; }

            [EnumDataType(typeof(EnumFieldType))]
            public string FieldType { get; set; }

            [EnumDataType(typeof(EnumInputType))]
            public string Input { get; set; }

            [EnumDataType(typeof(EnumOperatorType))]
            public string Operator { get; set; }

            public string Value { get; set; }
        }

        public class GroupRule
        {
            [EnumDataType(typeof(EnumConditionType))]
            public string Condition { get; set; }

            public List<RoleRule> RuleList { get; set; }
            public List<GroupRule> GroupRuleList { get; set; }
        }

        public class RoleConditionRules
        {
            public string CONDITION { get; set; }

            public List<RoleConditionRoleRule> RULE_LIST { get; set; }

            public List<RoleConditionRules> GROUP_RULE_LIST { get; set; }
        }

        public class RoleConditionRoleRule
        {
            public string ID { get; set; }

            public string FIELD_TYPE { get; set; }

            public string INPUT { get; set; }

            public string OPERATOR { get; set; }

            public string VALUE { get; set; }
        }

        public List<GroupRule> GetGroupRules(List<Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule> systemRoleConGroupRule)
        {
            List<GroupRule> result = new List<GroupRule>();
            if (systemRoleConGroupRule != null &&
                systemRoleConGroupRule.Any())
            {
                result.AddRange(systemRoleConGroupRule.Select(s => new GroupRule
                {
                    Condition = s.Condition.GetValue(),
                    RuleList =
                        s.RuleList
                         .Select(c => new RoleRule
                         {
                             ID = c.ID.GetValue(),
                             Operator = c.Operator.GetValue(),
                             Value = c.Value.GetValue()
                         }).ToList(),
                    GroupRuleList = GetGroupRules(s.GroupRuleList)
                }));
            }
            return result;
        }

        public List<GroupRule> GetGroupRules(List<RoleConditionRules> systemRoleConGroupRule)
        {
            List<GroupRule> result = new List<GroupRule>();
            if (systemRoleConGroupRule != null &&
                systemRoleConGroupRule.Any())
            {
                result.AddRange(systemRoleConGroupRule.Select(s => new GroupRule
                {
                    Condition = s.CONDITION,
                    RuleList =
                        s.RULE_LIST
                         .Select(c => new RoleRule
                         {
                             ID = c.ID,
                             Operator = c.OPERATOR,
                             Value = c.VALUE
                         }).ToList(),
                    GroupRuleList = GetGroupRules(s.GROUP_RULE_LIST)
                }));
            }

            return result;
        }

        public List<GroupRule> GetGroupRules<T>(List<T> systemRoleConGroupRule) where T : GroupRule
        {
            List<GroupRule> result = new List<GroupRule>();
            if (systemRoleConGroupRule != null &&
               systemRoleConGroupRule.Any())
            {
                result.AddRange(systemRoleConGroupRule.Select(s => new GroupRule
                {
                    Condition = s.Condition,
                    RuleList =
                       s.RuleList
                        .Select(c => new RoleRule
                        {
                            ID = c.ID,
                            Operator = c.Operator,
                            Value = c.Value
                        }).ToList(),
                    GroupRuleList = GetGroupRules(s.GroupRuleList)
                }));
            }
            return result;
        }
    }
}

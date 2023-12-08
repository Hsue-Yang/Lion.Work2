namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class ADSReport
    {
        public class SysRoleToFunction
        {
            public string SystemNM { get; set; }
            public string SystemRoleNM { get; set; }
            public string FunctionNM { get; set; }
        }

        public class SysUserToFunction
        {
            public string SystemNM { get; set; }
            public string UserNM { get; set; }
            public string UnitNM { get; set; }
            public string RoleNM { get; set; }
        }

        public class SysSingleFunctionAwarded
        {
            public string SystemNM { get; set; }
            public string FunctionNM { get; set; }
            public string UserNM { get; set; }
        }

        public class SysUserLoginLastTime
        {
            public string UserNM { get; set; }
            public string IsDisable { get; set; }
            public string IsLeft { get; set; }
            public string LastTime { get; set; }
        }

        public class SysReportToPermissions
        {
            public string SystemNM { get; set; }
            public string RoleConditionNMZHTW { get; set; }
            public string SystemRoleNM { get; set; }
            public string RoleConditionSyntax { get; set; }
        }
    }
}

using System.Text;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class ADSReportService : IADSReportService
    {
        private readonly IADSReportRepository _asdreportRepository;

        public ADSReportService(IADSReportRepository asdreportRepository)
        {
            _asdreportRepository = asdreportRepository;
        }

        public async Task<string> GetADSReportsCsv(string reportType, string sysID)
        {
            StringBuilder rows = new StringBuilder();

            switch (reportType)
            {
                case nameof(EnumReportType.SysRoleToFunction):
                    rows.AppendLine("應用系統, 系統角色, 系統功能");
                    foreach (var item in await _asdreportRepository.GetSysRoleToFunction(sysID))
                    {
                        rows.AppendLine($"{item.SystemNM},{item.SystemRoleNM},{item.FunctionNM}");
                    }
                    break;
                case nameof(EnumReportType.SysUserToFunction):
                    rows.AppendLine("應用系統, 人員名稱, 單位, 角色");
                    foreach (var item in await _asdreportRepository.GetSysUserToFunction(sysID))
                    {
                        rows.AppendLine($"{item.SystemNM},{item.UserNM},{item.UnitNM},{item.RoleNM}");
                    }
                    break;
                case nameof(EnumReportType.SysSingleFunctionAwarded):
                    rows.AppendLine("應用系統, 系統功能, 人員名稱");
                    foreach (var item in await _asdreportRepository.GetSysSingleFunctionAwarded(sysID))
                    {
                        rows.AppendLine($"{item.SystemNM},{item.FunctionNM},{item.UserNM}");
                    }
                    break;
                case nameof(EnumReportType.SysUserLoginLastTime):
                    rows.AppendLine("人員名稱, 是否停用, 是否離職, 最後登入時間");
                    foreach (var item in await _asdreportRepository.GetSysUserLoginLastTime())
                    {
                        rows.AppendLine($"{item.UserNM},{item.IsDisable},{item.IsLeft},{item.LastTime}");
                    }
                    break;
                case nameof(EnumReportType.SysReportToPermissions):
                    rows.AppendLine("應用系統, 功能群組, 系統角色, 稽核代碼");
                    foreach (var item in await _asdreportRepository.GetSysReportToPermissions(sysID))
                    {
                        rows.AppendLine($"{item.SystemNM},{item.RoleConditionNMZHTW},{item.SystemRoleNM},{item.RoleConditionSyntax}");
                    }
                    break;
                default:
                    return null;
            }

            return rows.ToString();
        }
    }
}

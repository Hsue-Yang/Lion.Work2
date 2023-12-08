using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IADSReportRepository
    {
        Task<IEnumerable<ADSReport.SysRoleToFunction>> GetSysRoleToFunction(string sysID);
        Task<IEnumerable<ADSReport.SysUserToFunction>> GetSysUserToFunction(string sysID);
        Task<IEnumerable<ADSReport.SysSingleFunctionAwarded>> GetSysSingleFunctionAwarded(string sysID);
        Task<IEnumerable<ADSReport.SysUserLoginLastTime>> GetSysUserLoginLastTime();
        Task<IEnumerable<ADSReport.SysReportToPermissions>> GetSysReportToPermissions(string sysID);
    }
}

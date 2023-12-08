
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISystemLogService
    {
        Task RecordLog(SystemLog log);
    }
}

using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysCultureSettingService
    {
        Task<IEnumerable<SystemCulture>> GetSystemCultureIDs();
        Task<(int rowCount, IEnumerable<SystemCulture> systemCultures)> GetSystemCultures(string cultureID, int pageIndex, int pageSize);
        Task<SystemCulture> GetSystemCultureDetail(string cultureID);
        Task EditSystemCultureDetail(SystemCulture model);
        Task DeleteSystemCultureDetail(string cultureID);
        Task GenerateCultureJsonFile();
    }
}
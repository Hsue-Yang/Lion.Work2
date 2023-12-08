using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysCultureSettingService : ISysCultureSettingService
    {
        private readonly ISysCultureSettingRepository _sysCultureRepository;

        public SysCultureSettingService(ISysCultureSettingRepository sysCultureRepository)
        {
            _sysCultureRepository = sysCultureRepository;
        }

        public Task<IEnumerable<SystemCulture>> GetSystemCultureIDs()
        {
            return _sysCultureRepository.GetSystemCultureIDs();
        }

        public Task<(int rowCount, IEnumerable<SystemCulture> systemCultures)> GetSystemCultures(string cultureID, int pageIndex, int pageSize)
        {
            return _sysCultureRepository.GetSystemCultures(cultureID, pageIndex, pageSize);
        }

        public Task<SystemCulture> GetSystemCultureDetail(string cultureID)
        {
            return _sysCultureRepository.GetSystemCultureDetail(cultureID);
        }

        public Task EditSystemCultureDetail(SystemCulture model)
        {
            return _sysCultureRepository.EditSystemCultureDetail(model);
        }

        public Task DeleteSystemCultureDetail(string cultureID)
        {
            return _sysCultureRepository.DeleteSystemCultureDetail(cultureID);
        }

        public Task GenerateCultureJsonFile()
        {
            return _sysCultureRepository.GenerateCultureJsonFile();
        }
    }
}
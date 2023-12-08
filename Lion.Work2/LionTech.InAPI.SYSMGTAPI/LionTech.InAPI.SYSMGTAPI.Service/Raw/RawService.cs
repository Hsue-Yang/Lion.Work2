using LionTech.InAPI.SYSMGTAPI.DataAccess.Raw.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Raw;
using LionTech.InAPI.SYSMGTAPI.Service.Raw.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Raw
{
    public class RawService : IRawService
    {
        private readonly IRawRepository _rawRepository;

        public RawService(IRawRepository rawRepository)
        {
            _rawRepository = rawRepository;
        }

        public Task<IEnumerable<RawCMOrgCom>> GetRawCMOrgComs()
        {
            return _rawRepository.GetRawCMOrgComs();
        }

        public Task<IEnumerable<RawCMOrgUnit>> GetRawCMOrgUnits()
        {
            return _rawRepository.GetRawCMOrgUnits();
        }

        public Task<IEnumerable<RawCMBusinessUnit>> GetRawCMBusinessUnits(string cultureID)
        {
            return _rawRepository.GetRawCMBusinessUnits(cultureID);
        }

        public Task<IEnumerable<RawCMCountry>> GetRawCMCountries(string cultureID)
        {
            return _rawRepository.GetRawCMCountries(cultureID);
        }

        public async Task<IEnumerable<RawUser>> GetRawUsers(string condition, int limit)
        {
            return await _rawRepository.GetRawUsers(condition, limit);
        }
    }
}
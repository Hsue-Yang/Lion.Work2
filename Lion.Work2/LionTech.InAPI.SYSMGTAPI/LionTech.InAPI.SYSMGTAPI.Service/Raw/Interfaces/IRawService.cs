using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Raw;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Raw.Interfaces
{
    public interface IRawService
    {
        Task<IEnumerable<RawCMOrgCom>> GetRawCMOrgComs();

        Task<IEnumerable<RawCMOrgUnit>> GetRawCMOrgUnits();

        Task<IEnumerable<RawCMBusinessUnit>> GetRawCMBusinessUnits(string cultureID);

        Task<IEnumerable<RawCMCountry>> GetRawCMCountries(string cultureID);

        Task<IEnumerable<RawUser>> GetRawUsers(string condition, int limit);
    }
}
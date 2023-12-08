using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Mis.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Mis.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Mis
{
    public class MisService : IMisService
    {
        private readonly IMisRepository _misRepository;

        public MisService(IMisRepository misRepository)
        {
            _misRepository = misRepository;
        }

        public Task<string> CheckIP(string UserCode, int userIP)
        {
            return _misRepository.CheckIP(UserCode, userIP);
        }
    }
}

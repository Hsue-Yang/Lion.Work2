using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class TrustIPService : ITrustIPService
    {
        private readonly ITrustIPRepository _trustIPRepository;
        
        public TrustIPService(ITrustIPRepository trustIPRepository)
        {
            _trustIPRepository = trustIPRepository;
        }

        public async Task<(int rowCount, IEnumerable<TrustIP> TrustIPList)> GetTrustIPs(TrustIPPara para)
        {
            return await _trustIPRepository.GetTrustIPs(para);
        }

        public async Task<TrustIP> GetTrustIPDetail(string IPBegin, string IPEnd, string cultureID)
        {
            return await _trustIPRepository.GetTrustIPDetail(IPBegin, IPEnd, cultureID);
        }

        public async Task<bool> GetValidTrustIPRepeated(TrustIP trustIP)
        {
            return await _trustIPRepository.GetValidTrustIPRepeated(trustIP);
        }

        public async Task<string> EditTrustIP(TrustIP trustIP)
        {
            if (await _trustIPRepository.EditTrustIP(trustIP) == EnumYN.Y.ToString())
            {
                if (await _trustIPRepository.EditASPMism98(trustIP) == EnumYN.Y.ToString())
                {
                    return EnumModifyResult.Success.ToString();
                }
                else
                {
                    return EnumModifyResult.SyncASPFailure.ToString();
                }
            }

            return EnumModifyResult.Failure.ToString();
        }

        public async Task<string> DeleteTrustIP(string IPBegin, string IPEnd)
        {
            if (await _trustIPRepository.DeleteTrustIP(IPBegin, IPEnd) == EnumYN.Y.ToString())
            {
                if (await _trustIPRepository.DeleteASPMism98(IPBegin, IPEnd) == EnumYN.Y.ToString())
                {
                    return EnumModifyResult.Success.ToString();
                }
                else
                {
                    return EnumModifyResult.SyncASPFailure.ToString();
                }
            }
            return EnumModifyResult.Failure.ToString();
        }

        public async Task InsertTrustIPMongoDB(RecordSysTrustIP para)
        {
            await _trustIPRepository.InsertTrustIPMongoDB(para);
        }
    }
}
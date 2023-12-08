using LionTech.AspNetCore.InApi.HostedService;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysPurviewService : ISysPurviewService
    {
        private readonly ISysPurviewRepository _sysPurviewRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;

        public SysPurviewService(ISysPurviewRepository sysPurviewRepository, IEventDistributorQueues eventDistributorQueues)
        {
            _sysPurviewRepository = sysPurviewRepository;
            _eventDistributorQueues = eventDistributorQueues;
        }

        public Task<IEnumerable<SystemPurview>> GetSystemPurviewByIdList(string sysID, string cultureID)
        {
            return _sysPurviewRepository.GetSystemPurviewByIdList(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemPurview> SystemPurviewList)> GetSystemPurviewList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysPurviewRepository.GetSystemPurviewList(sysID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemPurviewMain> GetSystemPurviewDetail(string sysID, string purviewID)
        {
            return _sysPurviewRepository.GetSystemPurviewDetail(sysID, purviewID);
        }

        public async Task EditSystemPurviewDetail(SystemPurviewMain systemPurview)
        {
            await _sysPurviewRepository.EditSystemPurviewDetail(systemPurview);
            var eventPara = new
            {
                systemPurview.SysID,
                systemPurview.PurviewID,
                systemPurview.PurviewNMZHTW,
                systemPurview.PurviewNMZHCN,
                systemPurview.PurviewNMENUS,
                systemPurview.PurviewNMTHTH,
                systemPurview.PurviewNMJAJP,
                systemPurview.PurviewNMKOKR,
                systemPurview.SortOrder,
                systemPurview.Remark
            };

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemPurview, EnumEDIServiceEventID.Edit, eventPara, systemPurview.SysID);
        }

        public async Task<EnumDeleteSystemPurviewResult> DeleteSystemPurviewDetail(string sysID, string purviewID)
        {
            var deleteSystemPurviewDetailResult = await _sysPurviewRepository.DeleteSystemPurviewDetail(sysID, purviewID);
            var eventPara = new { SysID = sysID, PurviewID = purviewID };

            if (deleteSystemPurviewDetailResult == EnumDeleteSystemPurviewResult.Success)
            {
                await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemPurview, EnumEDIServiceEventID.Delete, eventPara, sysID);
            }

            return deleteSystemPurviewDetailResult;
        }
    }
}
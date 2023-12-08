using LionTech.AspNetCore.InApi.HostedService;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysFunMenuService : ISysFunMenuService
    {
        private readonly ISysFunMenuRepository _sysFunMenuRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;

        public SysFunMenuService(ISysFunMenuRepository sysFunMenuRepository, IEventDistributorQueues eventDistributorQueues)
        {
            _sysFunMenuRepository = sysFunMenuRepository;
            _eventDistributorQueues = eventDistributorQueues;
        }

        public Task<IEnumerable<SystemFunMenu>> GetSystemFunMenuByIdList(string sysID, string cultureID)
        {
            return _sysFunMenuRepository.GetSystemFunMenuByIdList(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemFunMenu> SystemFunMenuList)> GetSystemFunMenuList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysFunMenuRepository.GetSystemFunMenuList(sysID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemFunMenuMain> GetSystemFunMenuDetail(string sysID, string funMenu)
        {
            return _sysFunMenuRepository.GetSystemFunMenuDetail(sysID, funMenu);
        }

        public async Task EditSystemFunMenuDetail(SystemFunMenuMain systemFunMenu)
        {
            await _sysFunMenuRepository.EditSystemFunMenuDetail(systemFunMenu);
            var eventPara = new
            {
                systemFunMenu.SysID,
                systemFunMenu.FunMenu,
                systemFunMenu.FunMenuNMZHTW,
                systemFunMenu.FunMenuNMZHCN,
                systemFunMenu.FunMenuNMENUS,
                systemFunMenu.FunMenuNMTHTH,
                systemFunMenu.FunMenuNMJAJP,
                systemFunMenu.FunMenuNMKOKR,
                systemFunMenu.DefaultMenuID,
                systemFunMenu.IsDisable,
                systemFunMenu.SortOrder,
            };

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemMenu, EnumEDIServiceEventID.Edit, eventPara, systemFunMenu.SysID);
        }

        public async Task<EnumDeleteSystemFunMenuResult> DeleteSystemFunMenuDetail(string sysID, string funMenu)
        {
            var deleteSystemFunMenuDetailResult = await _sysFunMenuRepository.DeleteSystemFunMenuDetail(sysID, funMenu);
            var eventPara = new { SysID = sysID, FunMenu = funMenu };

            if (deleteSystemFunMenuDetailResult == EnumDeleteSystemFunMenuResult.Success)
            {
                await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemMenu, EnumEDIServiceEventID.Delete, eventPara, sysID);
            }

            return deleteSystemFunMenuDetailResult;
        }
    }
}
using LionTech.AspNetCore.InApi.HostedService;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysAPIService : ISysAPIService
    {
        private readonly ISysAPIRepository _sysAPIRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;

        public SysAPIService(ISysAPIRepository sysAPIRepository, IEventDistributorQueues eventDistributorQueues)
        {
            _sysAPIRepository = sysAPIRepository;
            _eventDistributorQueues = eventDistributorQueues;
        }

        public Task<SystemAPI> GetSystemAPIFullName(string sysID, string apiGroupID, string apiFunID, string cultureID)
        {
            return _sysAPIRepository.GetSystemAPIFullName(sysID, apiGroupID, apiFunID, cultureID);
        }

        public Task<IEnumerable<SystemAPI>> GetSystemAPIByIdList(string sysID, string apiGroup, string cultureID)
        {
            return _sysAPIRepository.GetSystemAPIByIdList(sysID, apiGroup, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemAPI> SystemAPIList)> GetSystemAPIList(string sysID, string apiGroupID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysAPIRepository.GetSystemAPIList(sysID, apiGroupID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemAPIMain> GetSystemAPIDetail(string sysID, string apiGroupID, string apiFunID)
        {
            return _sysAPIRepository.GetSystemAPIDetail(sysID, apiGroupID, apiFunID);
        }

        public Task<IEnumerable<SystemAPIRole>> GetSystemAPIRoleList(string sysID, string apiGroupID, string apiFunID, string cultureID)
        {
            return _sysAPIRepository.GetSystemAPIRoleList(sysID, apiGroupID, apiFunID, cultureID);
        }

        public async Task EditSystemAPIDetail(SystemAPIMain systemAPI, DataTable systemRoleAPIs)
        {
            await _sysAPIRepository.EditSystemAPIDetail(systemAPI, systemRoleAPIs);
            var eventParaSystemAPIEdit = new
            {
                systemAPI.SysID,
                APIControllerID = systemAPI.APIGroupID,
                APIActionName = systemAPI.APIFunID,
                APINMzhTW = systemAPI.APINMZHTW,
                APINMzhCN = systemAPI.APINMZHCN,
                APINMenUS = systemAPI.APINMENUS,
                APINMthTH = systemAPI.APINMTHTH,
                APINMjaJP = systemAPI.APINMJAJP,
                APINMkoKR = systemAPI.APINMKOKR,
                systemAPI.APIPara,
                systemAPI.APIReturn,
                systemAPI.APIParaDesc,
                systemAPI.APIReturnContent,
                systemAPI.IsOutside,
                systemAPI.IsDisable,
                systemAPI.SortOrder 
            };

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemAPI, EnumEDIServiceEventID.Edit, eventParaSystemAPIEdit, systemAPI.SysID);
        }

        public async Task<EnumDeleteSystemAPIDetailResult> DeleteSystemAPIDetail(string sysID, string apiGroupID, string apiFunID)
        {
            var deleteSystemAPIDetailResult = await _sysAPIRepository.DeleteSystemAPIDetail(sysID, apiGroupID, apiFunID);
            var eventParaSystemAPIDelete = new { SysID = sysID, APIControllerID = apiGroupID, APIActionName = apiFunID };

            if (deleteSystemAPIDetailResult == EnumDeleteSystemAPIDetailResult.Success)
            {
                await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemAPI, EnumEDIServiceEventID.Delete, eventParaSystemAPIDelete, sysID);
            }

            return deleteSystemAPIDetailResult;
        }

        public Task<IEnumerable<SystemAPIFuntion>> GetSystemAPIFuntionList(string sysID, string apiControllerID, string cultureID)
        {
            return _sysAPIRepository.GetSystemAPIFuntionList(sysID, apiControllerID, cultureID);
        }
    }
}
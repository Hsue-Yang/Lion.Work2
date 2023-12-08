using LionTech.AspNetCore.InApi.HostedService;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysFunGroupService : ISysFunGroupService
    {
        private readonly ISysFunGroupRepository _sysFunGroupRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;
        private readonly ISystemLogger _systemLogger;
        private readonly ISysSettingRepository _sysSettingRepository;

        public SysFunGroupService(ISysFunGroupRepository sysFunGroupRepository, IEventDistributorQueues eventDistributorQueues, ISystemLogger systemLogger, ISysSettingRepository sysSettingRepository)
        {
            _sysFunGroupRepository = sysFunGroupRepository;
            _eventDistributorQueues = eventDistributorQueues;
            _systemLogger = systemLogger;
            _sysSettingRepository = sysSettingRepository;
        }
        public Task<IEnumerable<SystemFunGroup>> GetUserSystemFunGroupList(string userID, string cultureID)
        {
            return _sysFunGroupRepository.GetUserSystemFunGroupList(userID, cultureID);
        }

        public Task<IEnumerable<SystemFunGroup>> GetSystemFunGroupByIdList(string sysID, string cultureID)
        {
            return _sysFunGroupRepository.GetSystemFunGroupByIdList(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemFunGroup> SystemFunGroupList)> GetSystemFunGroupList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysFunGroupRepository.GetSystemFunGroupList(sysID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemFunGroupMain> GetSystemFunGroupDetail(string sysID, string funControllerID)
        {
            return _sysFunGroupRepository.GetSystemFunGroupDetail(sysID, funControllerID);
        }

        public async Task EditSystemFunGroupDetail(SystemFunGroupMain systemFunGroup)
        {
            await _sysFunGroupRepository.EditSystemFunGroupDetail(systemFunGroup);
            var systemMainTask = await _sysSettingRepository.GetSystemMain(systemFunGroup.SysID);
            systemFunGroup.SysNM = systemMainTask?.SysNMZHTW;
            
            var eventPara = new
            {
                systemFunGroup.SysID,
                systemFunGroup.FunControllerID,
                systemFunGroup.FunGroupZHTW,
                systemFunGroup.FunGroupZHCN,
                systemFunGroup.FunGroupENUS,
                systemFunGroup.FunGroupTHTH,
                systemFunGroup.FunGroupJAJP,
                systemFunGroup.FunGroupKOKR,
                systemFunGroup.SortOrder
            };
            
            var eventDistributorTask =  _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemFunGroup, EnumEDIServiceEventID.Edit, eventPara, systemFunGroup.SysID);
            var logTask =  _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_FUN_GROUP, systemFunGroup, model => new { model.SysID, model.FunControllerID },  EnumSystemLogModify.U);
            await Task.WhenAll(logTask, eventDistributorTask);
        }

        public async Task<EnumDeleteSystemFunGroupDetailResult> DeleteSystemFunGroupDetail(string sysID, string funControllerID, string userID, string execSysID, string execIpAddress)
        {
            var model = await _sysFunGroupRepository.GetSystemFunGroupDetail(sysID, funControllerID);
            var deleteSystemFunGroupDetailResult = await _sysFunGroupRepository.DeleteSystemFunGroupDetail(sysID, funControllerID, userID, execSysID, execIpAddress);
            
            if (deleteSystemFunGroupDetailResult == EnumDeleteSystemFunGroupDetailResult.Success)
            {
                var eventPara = new { SysID = sysID, FunControllerID = funControllerID };
                var systemMainTask = await _sysSettingRepository.GetSystemMain(model.SysID);
                
                model.SysID = sysID;
                model.FunControllerID = funControllerID;
                model.SysNM = systemMainTask?.SysNMZHTW;
                
                var logTask = _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_FUN_GROUP, model, model => new { model.SysID, model.FunControllerID }, EnumSystemLogModify.D);
                var eventDistributorTask =  _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemFunGroup, EnumEDIServiceEventID.Delete, eventPara, sysID);
                await Task.WhenAll(logTask, eventDistributorTask);
            }

            return deleteSystemFunGroupDetailResult;
        }
    }
}
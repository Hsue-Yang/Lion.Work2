using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysRoleConditionService : ISysRoleConditionService
    {
        private readonly ISysRoleConditionRepository _sysRoleConditionRepository;
        private readonly ISystemLogger _systemLogger;
        private readonly ISysSettingRepository _sysSettingRepository;
        public SysRoleConditionService(ISysRoleConditionRepository sysRoleConditionRepository, ISystemLogger systemLogger, ISysSettingRepository sysSettingRepository)
        {
            _sysRoleConditionRepository = sysRoleConditionRepository;
            _systemLogger = systemLogger;
            _sysSettingRepository = sysSettingRepository;
        }

        public async Task<(int rowCount, IEnumerable<SystemRoleCondition>)> GetSystemRoleConditions(string roleConditionID, string roleID, string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return await _sysRoleConditionRepository.GetSystemRoleConditions(roleConditionID, roleID, sysID, cultureID, pageIndex, pageSize);
        }

        public async Task<SystemRoleConditionDetail> GetSystemRoleConditionDetail(string sysID, string roleConditionID)
        {
            SystemRoleConditionDetail roleConditionDetail = await _sysRoleConditionRepository.GetSystemRoleConditionDetail(sysID, roleConditionID);
            if (roleConditionDetail != null)
            {
                SystemRoleCondotionMongo roleCondotionMongo = await _sysRoleConditionRepository.GetSystemRoleConditionDetailMongoDB(sysID, roleConditionID);
                roleConditionDetail.SysID = roleCondotionMongo.SysID;
                roleConditionDetail.RoleConditionID = roleCondotionMongo.RoleConditionID;
                roleConditionDetail.RoleConditionRules = roleCondotionMongo.RoleConditionRules;
            }
            return roleConditionDetail;
        }

        public async Task EditSystemRoleConditionDetail(SysRoleConditionDetailPara sysRoleConditionDetailPara)
        {
            SystemRoleConditionDetailPara sqlPara = new SystemRoleConditionDetailPara()
            {
                SysSystemRoleCondition = new SysSystemRoleCondition()
                {
                    SysID = sysRoleConditionDetailPara.SysID,
                    RoleConditionID = sysRoleConditionDetailPara.RoleConditionID,
                    RoleConditionNMZHTW = sysRoleConditionDetailPara.RoleConditionNMZHTW,
                    RoleConditionNMZHCN = sysRoleConditionDetailPara.RoleConditionNMZHCN,
                    RoleConditionNMENUS = sysRoleConditionDetailPara.RoleConditionNMENUS,
                    RoleConditionNMTHTH = sysRoleConditionDetailPara.RoleConditionNMTHTH,
                    RoleConditionNMJAJP = sysRoleConditionDetailPara.RoleConditionNMJAJP,
                    RoleConditionNMKOKR = sysRoleConditionDetailPara.RoleConditionNMKOKR,
                    RoleConditionSyntax = sysRoleConditionDetailPara.RoleConditionSyntax,
                    SortOrder = sysRoleConditionDetailPara.SortOrder,
                    Remark = sysRoleConditionDetailPara.Remark,
                    UpdUserID = sysRoleConditionDetailPara.UpdUserID
                }
            };

            List<SystemRoleConditionCollect> systemRoleConditionCollectList = new List<SystemRoleConditionCollect>();

            foreach (var item in sysRoleConditionDetailPara.RoleList)
            {
                SystemRoleConditionCollect systemRoleConditionCollect = new SystemRoleConditionCollect();
                systemRoleConditionCollect.SysID = sysRoleConditionDetailPara.SysID;
                systemRoleConditionCollect.RoleConditionID = sysRoleConditionDetailPara.RoleConditionID;
                systemRoleConditionCollect.RoleID = item;
                systemRoleConditionCollect.UpdUserID = sysRoleConditionDetailPara.UpdUserID;
                systemRoleConditionCollectList.Add(systemRoleConditionCollect);
            }

            sqlPara.SystemRoleConditionCollect = systemRoleConditionCollectList;
            MongoSystemRoleConditionDetail mongoPara = new MongoSystemRoleConditionDetail()
            {
                SysID = sysRoleConditionDetailPara.SysID,
                SysNM = sysRoleConditionDetailPara.SysNM,
                RoleConditionID = sysRoleConditionDetailPara.RoleConditionID,
                Roles = sysRoleConditionDetailPara.RoleList,
                RoleConditionNMZHTW = sysRoleConditionDetailPara.RoleConditionNMZHTW,
                RoleConditionNMZHCN = sysRoleConditionDetailPara.RoleConditionNMZHCN,
                RoleConditionNMENUS = sysRoleConditionDetailPara.RoleConditionNMENUS,
                RoleConditionNMTHTH = sysRoleConditionDetailPara.RoleConditionNMTHTH,
                RoleConditionNMJAJP = sysRoleConditionDetailPara.RoleConditionNMJAJP,
                RoleConditionNMKOKR = sysRoleConditionDetailPara.RoleConditionNMKOKR,
                RoleConditionSynTax = sysRoleConditionDetailPara.RoleConditionSyntax,
                SortOrder = sysRoleConditionDetailPara.SortOrder,
                Remark = sysRoleConditionDetailPara.Remark,
                UpdUserID = sysRoleConditionDetailPara.UpdUserID,
                UpdUserNM = sysRoleConditionDetailPara.UpdUserNM,
                UpdDT = sysRoleConditionDetailPara.UpdDT,
                RoleConditionRules = sysRoleConditionDetailPara.RoleConditionRules
            };
            await _sysRoleConditionRepository.EditSystemRoleConditionDetail(sqlPara);
            await RecordSystemConditionLog(mongoPara);
            _sysRoleConditionRepository.DeleteSystemRoleCondotionDetailMongoDB(sqlPara.SysSystemRoleCondition.SysID, sqlPara.SysSystemRoleCondition.RoleConditionID);
            _sysRoleConditionRepository.InsertSystemRoleCondotionDetailMongoDB(mongoPara);
        }

        public async Task DeleteSystemRoleConditionDetail(string sysID, string roleConditionID)
        {
            var getSystemRoleConditionTask = _sysRoleConditionRepository.GetSystemRoleConditionDetailMongoDB(sysID, roleConditionID);
            var getSystemRoleConditionDetailTask = _sysRoleConditionRepository.GetSystemRoleConditionDetail(sysID, roleConditionID);
            await Task.WhenAll(getSystemRoleConditionTask, getSystemRoleConditionDetailTask);
            await _sysRoleConditionRepository.DeleteSystemRoleConditionDetail(sysID, roleConditionID);
            _sysRoleConditionRepository.DeleteSystemRoleCondotionDetailMongoDB(sysID, roleConditionID);
            await RecordSystemConditionLog(getSystemRoleConditionTask.Result, getSystemRoleConditionDetailTask.Result);
        }

        private async Task RecordSystemConditionLog(MongoSystemRoleConditionDetail mongoPara)
        {
            var systemMainTask = await _sysSettingRepository.GetSystemMain(mongoPara.SysID);
            var log = new
            {
                SYS_ID = mongoPara.SysID,
                SYS_NM = systemMainTask?.SysNMZHTW,
                ROLE_CONDITION_ID = mongoPara.RoleConditionID,
                ROLES = mongoPara.Roles,
                ROLE_CONDITION_NM_ZH_TW = mongoPara.RoleConditionNMZHTW,
                ROLE_CONDITION_NM_ZH_CN = mongoPara.RoleConditionNMZHCN,
                ROLE_CONDITION_NM_EN_US = mongoPara.RoleConditionNMENUS,
                ROLE_CONDITION_NM_TH_TH = mongoPara.RoleConditionNMTHTH,
                ROLE_CONDITION_NM_JA_JP = mongoPara.RoleConditionNMJAJP,
                ROLE_CONDITION_NM_KO_KR = mongoPara.RoleConditionNMKOKR,
                ROLE_CONDITION_SYNTAX = mongoPara.RoleConditionSynTax,
                SORT_ORDER = mongoPara.SortOrder,
                REMARK = mongoPara.Remark,
                ROLE_CONDITION_RULES = mongoPara.RoleConditionRules
            };

            await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE_CONDTION, log,
                model => new { model.SYS_ID, model.ROLE_CONDITION_ID }, EnumSystemLogModify.U);
        }

        private async Task RecordSystemConditionLog(SystemRoleCondotionMongo systemRoleCondition,
            SystemRoleConditionDetail systemRoleConditionDetail)
        {
            var systemMainTask = await _sysSettingRepository.GetSystemMain(systemRoleCondition.SysID);
            var log = new
            {
                SYS_ID = systemRoleConditionDetail.SysID,
                SYS_NM = systemMainTask.SysNMZHTW,
                ROLE_CONDITION_ID = systemRoleConditionDetail.RoleConditionID,
                ROLES = systemRoleConditionDetail.SysRole.Split('、').ToList(),
                ROLE_CONDITION_NM_ZH_TW = systemRoleConditionDetail.RoleConditionNMZHTW,
                ROLE_CONDITION_NM_ZH_CN = systemRoleConditionDetail.RoleConditionNMZHCN,
                ROLE_CONDITION_NM_EN_US = systemRoleConditionDetail.RoleConditionNMENUS,
                ROLE_CONDITION_NM_TH_TH = systemRoleConditionDetail.RoleConditionNMTHTH,
                ROLE_CONDITION_NM_JA_JP = systemRoleConditionDetail.RoleConditionNMJAJP,
                ROLE_CONDITION_NM_KO_KR = systemRoleConditionDetail.RoleConditionNMKOKR,
                ROLE_CONDITION_SYNTAX = systemRoleConditionDetail.RoleConditionSynTax,
                SORT_ORDER = systemRoleConditionDetail.SortOrder,
                REMARK = systemRoleConditionDetail.Remark,
                ROLE_CONDITION_RULES = systemRoleCondition.RoleConditionRules
            };

            await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE_CONDTION, log,
                model => new { model.SYS_ID, model.ROLE_CONDITION_ID }, EnumSystemLogModify.D);
        }
    }
}

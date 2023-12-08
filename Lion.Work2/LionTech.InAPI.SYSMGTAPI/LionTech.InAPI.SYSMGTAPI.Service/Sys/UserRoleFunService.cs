using LionTech.AspNetCore.InApi.HostedService;
using LionTech.AspNetCore.Utility;
using LionTech.AspNetCore.Utility.SERP;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Raw.Interfaces;
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
    public class UserRoleFunService : IUserRoleFunService
    {
        private readonly IUserRoleFunRepository _userRoleFunRepository;
        private readonly IUserFunctionService _userFunctionService;
        private readonly ISystemLogger _systemLogger;
        private readonly IRawRepository _rawRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;
        private readonly ISysEventRepository _sysEventRepository;

        public UserRoleFunService(
            IUserRoleFunRepository userRoleFunRepository,
            ISystemLogger systemLogger,
            IRawRepository rawRepository,
            IUserFunctionService userFunctionService,
            IEventDistributorQueues eventDistributorQueues,
            ISysEventRepository sysEventRepository)
        {
            _userRoleFunRepository = userRoleFunRepository;
            _systemLogger = systemLogger;
            _rawRepository = rawRepository;
            _userFunctionService = userFunctionService;
            _eventDistributorQueues = eventDistributorQueues;
            _sysEventRepository = sysEventRepository;
        }

        public Task<(int rowCount, IEnumerable<UserRoleFun> userRoleFunList)> GetUserRoleFuns(string userID, string userNM, int pageIndex, int pageSize)
        {
            return _userRoleFunRepository.GetUserRoleFuns(userID, userNM, pageIndex, pageSize);
        }

        public async Task<UserMain> GetUserMainInfo(string userID)
        {
            return await _userRoleFunRepository.GetUserMainInfo(userID);
        }

        public async Task<IEnumerable<SystemRoleGroupCollect>> GetSystemRoleGroupCollects(string roleGroupID)
        {
            return await _userRoleFunRepository.GetSystemRoleGroupCollects(roleGroupID);
        }

        public async Task<IEnumerable<UserSystemRoleData>> GetUserSystemRoles(string userID, string updUserID, string cultureID)
        {
            return await _userRoleFunRepository.GetUserSystemRoles(userID, updUserID, cultureID);
        }

        public async Task EditUserSystemRole(UserRoleFunDetailPara userRoleFunDetailPara, List<SystemRoleMain> userRoleFunDetailParaList)
        {
            IEnumerable<UserSystemRoleData> origin = await GetUserSystemRoles(userRoleFunDetailPara.UserID, userRoleFunDetailPara.UpdUserID, "ZH_TW");
            await _userRoleFunRepository.EditUserSystemRole(userRoleFunDetailPara, userRoleFunDetailParaList);

            var systemEventTargets = await _sysEventRepository.GetSystemEventTargetIDs(EnumSystemID.ERPAP.ToString(), EnumEDIServiceEventGroupID.SysUserSystemRole.ToString(), EnumEDIServiceEventID.Edit.ToString());
            if (systemEventTargets != null && systemEventTargets.Any())
            {
                foreach (SystemEventTarget target in systemEventTargets)
                {
                    var eventParaUserSystemRoleEdit = new
                    {
                        userRoleFunDetailPara.UserID,
                        userRoleFunDetailPara.RoleGroupID,
                        RoleIDList = userRoleFunDetailParaList.Where(w => w.SysID == target.SysID).Select(s => s.RoleID).ToArray()
                    };
                    await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysUserSystemRole, EnumEDIServiceEventID.Edit, eventParaUserSystemRoleEdit, target.SysID);
                }
            }

            var rawUserTask = await _rawRepository.GetRawUsers(userRoleFunDetailPara.UserID, 1);
            var userNM = rawUserTask.SingleOrDefault()?.UserNM;
            Task recordUserSystemRoleApplyLogTask = RecordUserSystemRoleApplyLog(userRoleFunDetailPara, userNM, userRoleFunDetailParaList, origin);
            Task recordUserFunctionTask = _userFunctionService.RecordUserFunction(userRoleFunDetailPara.UserID, userNM, userRoleFunDetailPara.ErpWFNO, userRoleFunDetailPara.Memo);
            await Task.WhenAll(recordUserSystemRoleApplyLogTask, recordUserFunctionTask);
        }

        private async Task RecordUserSystemRoleApplyLog(UserRoleFunDetailPara userRoleFunDetail, string userNM,
            List<SystemRoleMain> sysRoles, IEnumerable<UserSystemRoleData> originSysRoles)
        {
            var unAuthSysIDList =
                (from n in originSysRoles
                 where n.HasAuth != 1 && n.HasRole == AspNetCore.Utility.EnumYN.Y.ToString()
                 select new
                 {
                     n.SysID
                 }).Distinct().ToList();

            var authSysRoleList =
                (from s in sysRoles
                 where unAuthSysIDList.Exists(e => e.SysID == s.SysID) == false
                 select new
                 {
                     s.SysID,
                     s.RoleID
                 }).ToList();

            var modifyList =
                (from userSystemRole in originSysRoles.Where(userSystemRole => userSystemRole.HasAuth == 1)
                 let modifyType =
                     (userSystemRole.HasRole == AspNetCore.Utility.EnumYN.N.ToString() &&
                      authSysRoleList.Exists(u => userSystemRole.SysID == u.SysID && userSystemRole.RoleID == u.RoleID))
                         ? EnumModifyType.I
                         : (userSystemRole.HasRole == AspNetCore.Utility.EnumYN.Y.ToString() &&
                            authSysRoleList.Exists(u => userSystemRole.SysID == u.SysID && userSystemRole.RoleID == u.RoleID) == false)
                             ? EnumModifyType.D
                             : EnumModifyType.U
                 where modifyType is EnumModifyType.I or EnumModifyType.D
                 select new
                 {
                     SYS_ID = userSystemRole.SysID,
                     SYS_NM = userSystemRole.SysNM,
                     ROLE_ID = userSystemRole.RoleID,
                     ROLE_NM = userSystemRole.RoleNM,
                     MOTIFY_TYPE = modifyType.ToString(),
                     MODIFY_TYPE_NM = Common.GetEnumDesc(modifyType)
                 }).ToList();

            if (modifyList.Any())
            {
                var log = new
                {
                    USER_ID = userRoleFunDetail.UserID,
                    USER_NM = userNM,
                    WFNO = userRoleFunDetail.ErpWFNO,
                    MEMO = userRoleFunDetail.Memo,
                    MODIFY_LIST = modifyList
                };
                await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_USER_SYSTEM_ROLE_APPLY, log, model => new { model.USER_ID }, EnumSystemLogModify.U);
            }
        }

        public async Task<IEnumerable<UserMenuFun>> GetUserMenuFuns(string userID, string cultureID)
        {
            return await _userRoleFunRepository.GetUserMenuFuns(userID, cultureID);
        }
    }
}

using LionTech.InAPI.SYSMGTAPI.DataAccess.Log.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SystemLogService : ISystemLogService
    {
        private readonly ILogger<SystemLogService> _logger;
        private readonly ISystemLogRepository _mongoDBRepository;

        public SystemLogService(ILogger<SystemLogService> logger, ISystemLogRepository mongoDBRepository)
        {
            _logger = logger;
            _mongoDBRepository = mongoDBRepository;
        }

        public async Task RecordLog(SystemLog log)
        {
            try
            {
                foreach (BsonDocument document in log.Data)
                {
                    if (log.Modify.ToString().Equals("N") == false)
                    {
                        document.Add("MODIFY_TYPE", log.Modify.ToString());
                        document.Add("MODIFY_TYPE_NM", GetModifyName(log.Modify));
                    }

                    if (string.IsNullOrWhiteSpace(log.UpdUserID) == false)
                    {
                        document.Add("UPD_USER_ID", log.UpdUserID);
                        document.Add("UPD_USER_NM", log.UpdUserNM);
                    }

                    document.Add("UPD_DT", log.UpdDT);
                    document.Add("EXEC_SYS_ID", log.ExecSysID);
                    document.Add("EXEC_SYS_NM", log.ExecSysNM);
                    document.Add("EXEC_IP_ADDRESS", log.ExecIPAddress);
                }

                await _mongoDBRepository.RecordLog(log.MongoDocName.ToString(), log.Data, log.Condition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"\n{ex.GetType()}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private string GetModifyName(EnumSystemLogModify modify)
        {
            switch (modify)
            {
                case EnumSystemLogModify.U:
                    return "更新";
                case EnumSystemLogModify.I:
                    return "新增";
                case EnumSystemLogModify.D:
                    return "刪除";
            }

            return null;
        }
    }
}

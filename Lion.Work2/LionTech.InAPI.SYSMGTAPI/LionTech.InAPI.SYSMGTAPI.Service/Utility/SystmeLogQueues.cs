using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System;
using System.Collections.Concurrent;

namespace LionTech.InAPI.SYSMGTAPI.Service.Utility
{
    public class SystmeLogQueues : ISystmeLogQueues
    {
        private readonly BlockingCollection<SystemLog> _collection = new BlockingCollection<SystemLog>();
        private readonly ISystemLogService _systemLogService;

        public SystmeLogQueues(ISystemLogService systemLogService)
        {
            _systemLogService = systemLogService;
        }

        public void Producer(SystemLog log)
        {
            if (log != null)
                _collection.TryAdd(log, TimeSpan.FromSeconds(10));
        }

        public void Consumer()
        {
            foreach (var log in _collection.GetConsumingEnumerable())
            {
                _systemLogService.RecordLog(log);
            }
        }
    }
}
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces
{
    public interface ISystmeLogQueues
    {
        public void Producer(SystemLog log);
        public void Consumer();
    }
}

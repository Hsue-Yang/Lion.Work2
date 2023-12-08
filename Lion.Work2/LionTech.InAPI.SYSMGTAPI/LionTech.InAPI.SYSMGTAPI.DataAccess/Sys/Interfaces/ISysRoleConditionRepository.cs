using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysRoleConditionRepository
    {
        Task<(int rowCount, IEnumerable<SystemRoleCondition>)> GetSystemRoleConditions(string roleConditionID, string roleID, string sysID, string cultureID, int pageIndex, int pageSize);
        Task<SystemRoleConditionDetail> GetSystemRoleConditionDetail(string sysID, string roleConditionID);
        Task<SystemRoleCondotionMongo> GetSystemRoleConditionDetailMongoDB(string sysID, string roleConditionID);
        Task EditSystemRoleConditionDetail(SystemRoleConditionDetailPara systemRoleConditionDetailPara);
        Task DeleteSystemRoleConditionDetail(string sysID, string roleConditionID);
        void DeleteSystemRoleCondotionDetailMongoDB(string sysID, string roleConditionID);
        void InsertSystemRoleCondotionDetailMongoDB(MongoSystemRoleConditionDetail mongoSystemRoleConditionDetail);
    }
}

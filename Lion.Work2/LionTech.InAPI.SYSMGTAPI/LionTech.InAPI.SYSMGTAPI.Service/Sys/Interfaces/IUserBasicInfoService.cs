using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
   public interface IUserBasicInfoService
    {
        Task<(int rowCount, IEnumerable<UserBasicInfo> userBasicInfoList)> GetUserBasicInfotList(string userID, string userNM, string isDisable, string isLeft, string connectDTBegin, string connectDTEnd, string cultureID, int pageIndex, int pageSize);
        Task<UserBasicInfo> GetUserBasicInfoDetail(string userID, string cultureID);       
    }
}

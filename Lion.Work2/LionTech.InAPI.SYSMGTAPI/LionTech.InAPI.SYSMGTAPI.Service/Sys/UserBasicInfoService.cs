using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class UserBasicInfoService: IUserBasicInfoService
    {
        private readonly IUserBasicInfoRepository _userBasicInfoRepository ;

        public UserBasicInfoService(IUserBasicInfoRepository userBasicInfoRepository)
        {
            _userBasicInfoRepository = userBasicInfoRepository;
        }

        public Task<(int rowCount, IEnumerable<UserBasicInfo> userBasicInfoList)> GetUserBasicInfotList(string userID, string userNM, string isDisable, string isLeft, string connectDTBegin, string connectDTEnd, string cultureID, int pageIndex, int pageSize)
        {
            return _userBasicInfoRepository.GetUserBasicInfotList(userID, userNM, isDisable, isLeft, connectDTBegin, connectDTEnd, cultureID, pageIndex, pageSize);
        }

        public Task<UserBasicInfo> GetUserBasicInfoDetail(string userID, string cultureID)
        {
            return _userBasicInfoRepository.GetUserBasicInfoDetail(userID, cultureID);
        }
    }
}

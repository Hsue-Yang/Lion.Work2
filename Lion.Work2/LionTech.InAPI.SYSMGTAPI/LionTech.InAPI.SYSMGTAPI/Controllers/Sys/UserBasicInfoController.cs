using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserBasicInfo")]
    public partial class UserBasicInfoController: ControllerBase
    {
        private readonly IUserBasicInfoService _service;

        public UserBasicInfoController(IUserBasicInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QueryUserBasicInfotList([FromQuery] string userID, [FromQuery] string userNM
            , [FromQuery] string isDisable, [FromQuery] string isLeft, [FromQuery] string connectDTBegin, [FromQuery] string connectDTEnd
            , [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, userBasicInfoList) = await _service.GetUserBasicInfotList(userID, userNM, isDisable, isLeft, connectDTBegin, connectDTEnd, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                UserBasicInfoList = userBasicInfoList
            });
        }

        [HttpGet]
        [Route("{userID}")]
        public async Task<IActionResult> QueryUserBasicInfoDetail([FromRoute] string userID, [FromQuery] string cultureID)
        {
            var result = await _service.GetUserBasicInfoDetail(userID, cultureID);
            return Ok(result);
        }
    }
}
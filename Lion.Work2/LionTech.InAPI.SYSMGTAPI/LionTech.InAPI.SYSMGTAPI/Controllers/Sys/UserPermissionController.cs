using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserPermission")]
    public partial class UserPermissionController : ControllerBase
    {
        private readonly IUserPermissionService _service;

        public UserPermissionController(IUserPermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QueryUserPermissionList([FromQuery] string userID, [FromQuery] string userNM, [FromQuery] string restrictType
            , [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetUserPermissionList(userID ,userNM, restrictType, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                UserPermissionLists = result.userPermissionsList
            });
        }

        [HttpGet]
        [Route("{userID}")]
        public async Task<IActionResult> QueryUserPermissionDetail([FromRoute] string userID)
        {
            var result = await _service.GetUserPermissionDetail(userID);
            return Ok(new
            {
                UserRawData = result
            });
        }
    }
}
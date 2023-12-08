using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserConnect")]
    public partial class UserConnectController : ControllerBase
    {
        private readonly IUserConnectService _service;

        public UserConnectController(IUserConnectService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QueryUserConnectList([FromQuery] string connectDTBegin, [FromQuery] string connectDTEnd, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetUserConnectList(connectDTBegin, connectDTEnd, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                UserConnectLists = result.userConnectList
            });
        }
    }
}
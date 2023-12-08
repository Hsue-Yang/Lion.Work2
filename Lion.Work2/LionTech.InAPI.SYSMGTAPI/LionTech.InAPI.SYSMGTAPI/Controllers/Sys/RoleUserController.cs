using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/RoleUser")]
    public class RoleUserController : ControllerBase
    {
        private readonly IRoleUserService _service;

        public RoleUserController(IRoleUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QueryRoleUsers([FromQuery] string sysID, [FromQuery] string roleID, [FromQuery] string cultureID)
        {
            var result = await _service.GetRoleUsers(sysID, roleID, cultureID);
            return Ok(new
            {
                RoleUserList = result
            });
        }
    }
}

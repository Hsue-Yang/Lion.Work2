using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Authorization
{
    [Authorize]
    [ApiController]
    [Route("v1/Authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces.IAuthorizationService _service;

        public AuthorizationController(LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces.IAuthorizationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("AllUserSystemRoles")]
        public async Task<IActionResult> QueryAllUserSystemRoles()
        {
            var result = await _service.GetAllUserSystemRolesObj();
            return Ok(result);
        }

        [HttpGet]
        [Route("AllUserAssignSystemFuns")]
        public async Task<IActionResult> QueryAllUserAssignSystemFuns()
        {
            var result = await _service.GetAllUserAssignSystemFunsObj();
            return Ok(result);
        }
    }
}
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Service.Mis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Mis
{
    [Authorize]
    [ApiController]
    [Route("v1/Mis")]
    public class MisController : ControllerBase
    {
        private readonly IMisService _misService;
        public MisController(IMisService misService)
        {
            _misService = misService;
        }

        [HttpGet]
        [Route("CheckIP")]
        public async Task<IActionResult> QueryIP([FromQuery] int IpV, [FromQuery] string UserCode = "")
        {
            var result = await _misService.CheckIP(UserCode, IpV);
            return Ok(result);
        }
    }
}
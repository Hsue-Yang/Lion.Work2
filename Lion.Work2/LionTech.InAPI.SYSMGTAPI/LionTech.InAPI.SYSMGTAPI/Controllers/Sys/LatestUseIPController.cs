using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/LatestUseIP")]
    public partial class LatestUseIPController : ControllerBase
    {
        private readonly ILatestUseIPService _service;

        public LatestUseIPController(ILatestUseIPService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("LatestUseIPView")]
        public async Task<IActionResult> QueryLatestUseIPInfoList([FromQuery] string ipAddress, [FromQuery] string codeNM, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetLatestUseIPInfoList(ipAddress, codeNM, pageIndex, pageSize);
            return Ok( new {
                RowCount = result.rowCount,
                LatestUseIPInfoLists = result.LatestUseIPInfoLists
            });
        }
    }
}
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemPurview")]
    public class SystemPurviewController : ControllerBase
    {
        private readonly ISysPurviewService _service;

        public SystemPurviewController(ISysPurviewService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySystemPurviewByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemPurviewByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemPurviews([FromRoute] string sysID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemPurviewList(sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemPurviews = result.SystemPurviewList
            });
        }

        [HttpGet]
        [Route("{sysID}/{purviewID}")]
        public async Task<IActionResult> QuerySystemPurview([FromRoute] string sysID, [FromRoute] string purviewID)
        {
            var result = await _service.GetSystemPurviewDetail(sysID, purviewID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemPurview([FromBody] SystemPurviewMain systemPurview)
        {
            await _service.EditSystemPurviewDetail(systemPurview);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{purviewID}")]
        public async Task<IActionResult> DeleteSystemFunMenu([FromRoute] string sysID, [FromRoute] string purviewID)
        {
            var result = await _service.DeleteSystemPurviewDetail(sysID, purviewID);
            if (result == EnumDeleteSystemPurviewResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }
    }
}
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
    [Route("v1/SystemEventGroup")]
    public class SystemEventGroupController : ControllerBase
    {
        private readonly ISysEventGroupService _service;

        public SystemEventGroupController(ISysEventGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySystemEventGroupByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEventGroupByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemEventGroups([FromRoute] string sysID, [FromQuery] string eventGroupID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemEventGroupList(sysID, eventGroupID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemEventGroups = result.SystemEventGroupList
            });
        }

        [HttpGet]
        [Route("{sysID}/{eventGroupID}")]
        public async Task<IActionResult> QuerySystemEventGroup([FromRoute] string sysID, [FromRoute] string eventGroupID)
        {
            var result = await _service.GetSystemEventGroupDetail(sysID, eventGroupID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemEventGroup([FromBody] SystemEventGroupMain systemEventGroup)
        {
            await _service.EditSystemEventGroupDetail(systemEventGroup);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{eventGroupID}")]
        public async Task<IActionResult> DeleteSystemEventGroup([FromRoute] string sysID, [FromRoute] string eventGroupID)
        {
            var result = await _service.DeleteSystemEventGroupDetail(sysID, eventGroupID);
            if (result == EnumDeleteResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }
    }
}
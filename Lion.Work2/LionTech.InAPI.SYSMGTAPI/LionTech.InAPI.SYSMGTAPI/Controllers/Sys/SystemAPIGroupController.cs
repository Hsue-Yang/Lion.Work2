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
    [Route("v1/SystemAPIGroup")]
    public class SystemAPIGroupController : ControllerBase
    {
        private readonly ISysAPIGroupService _service;

        public SystemAPIGroupController(ISysAPIGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySystemAPIGroupByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemAPIGroupByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemAPIGroups([FromRoute] string sysID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemAPIGroupList(sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemAPIGroups = result.SystemAPIGroupList
            });
        }

        [HttpGet]
        [Route("{sysID}/{apiGroupID}")]
        public async Task<IActionResult> QuerySystemAPIGroup([FromRoute] string sysID, [FromRoute] string apiGroupID)
        {
            var result = await _service.GetSystemAPIGroupDetail(sysID, apiGroupID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemAPIGroup([FromBody] SystemAPIGroupMain systemAPIGroup)
        {
            await _service.EditSystemAPIGroupDetail(systemAPIGroup);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{apiGroupID}")]
        public async Task<IActionResult> DeleteSystemAPIGroup([FromRoute] string sysID, [FromRoute] string apiGroupID)
        {
            var result = await _service.DeleteSystemAPIGroupDetail(sysID, apiGroupID);

            if (result == EnumDeleteSystemAPIGroupResult.Success)
            {
                return Ok();
            }

            return BadRequest(new { Message = result.ToString() });
        }
    }
}
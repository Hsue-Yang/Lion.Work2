using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Model.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemAPI")]
    public partial class SystemAPIController : ControllerBase
    {
        private readonly ISysAPIService _service;

        public SystemAPIController(ISysAPIService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Names")]
        public async Task<IActionResult> QuerySystemAPIFullName([FromRoute] string sysID, [FromQuery] string apiGroupID, [FromQuery] string apiFunID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemAPIFullName(sysID, apiGroupID, apiFunID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/{apiGroup}/Ids")]
        public async Task<IActionResult> QuerySystemAPIByIds([FromRoute] string sysID, [FromRoute] string apiGroup, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemAPIByIdList(sysID, apiGroup, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemAPIs([FromRoute] string sysID, [FromQuery] string apiGroupID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemAPIList(sysID, apiGroupID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemAPIs = result.SystemAPIList
            });
        }

        [HttpGet]
        [Route("{sysID}/{apiGroupID}/{apiFunID}")]
        public async Task<IActionResult> QuerySystemAPI([FromRoute] string sysID, [FromRoute] string apiGroupID, [FromRoute] string apiFunID)
        {
            var result = await _service.GetSystemAPIDetail(sysID, apiGroupID, apiFunID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/Roles")]
        public async Task<IActionResult> QuerySystemAPIRoles([FromRoute] string sysID, [FromQuery] string apiGroupID, [FromQuery] string apiFunID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemAPIRoleList(sysID, apiGroupID, apiFunID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemAPI([FromBody] SystemAPIDetailModel model)
        {
            await _service.EditSystemAPIDetail(model.GetSystemAPIDetail(), model.GetSystemRoleAPIDataTable());
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{apiGroupID}/{apiFunID}")]
        public async Task<IActionResult> DeleteSystemAPI([FromRoute] string sysID, [FromRoute] string apiGroupID, [FromRoute] string apiFunID)
        {
            var result = await _service.DeleteSystemAPIDetail(sysID, apiGroupID, apiFunID);
            if (result == EnumDeleteSystemAPIDetailResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }

        [HttpGet]
        [Route("Funtions")]
        public async Task<IActionResult> QuerySystemAPIFuntions([FromQuery] string sysID, [FromQuery] string apiControllerID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemAPIFuntionList(sysID, apiControllerID, cultureID);
            return Ok(new
            {
                systemAPIFuntions = result
            });
        }
    }
}
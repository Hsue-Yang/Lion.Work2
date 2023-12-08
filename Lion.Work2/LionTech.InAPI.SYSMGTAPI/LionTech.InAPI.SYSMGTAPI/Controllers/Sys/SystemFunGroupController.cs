using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemFunGroup")]
    public partial class SystemFunGroupController : ControllerBase
    {
        private readonly ISysFunGroupService _service;
        private readonly string ClientUserID;

        public SystemFunGroupController(ISysFunGroupService service, IHttpContextAccessor httpContext)
        {
            ClientUserID = httpContext.HttpContext.Request.Query["ClientUserID"];
            _service = service;
        }

        [HttpGet]
        [Route("UserAuthorization")]
        public async Task<IActionResult> QueryUserSystemFunGroups([FromQuery] string cultureID)
        {
            var result = await _service.GetUserSystemFunGroupList(ClientUserID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Ids")]
        public async Task<IActionResult> QuerySystemFunGroupByIds([FromQuery] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunGroupByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemFunGroups([FromRoute] string sysID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemFunGroupList(sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemFunGroups = result.SystemFunGroupList
            });
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}")]
        public async Task<IActionResult> QuerySystemFunGroup([FromRoute] string sysID, [FromRoute] string funControllerID)
        {
            var result = await _service.GetSystemFunGroupDetail(sysID, funControllerID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemFunGroup([FromBody] SystemFunGroupMain systemFunGroup)
        {
            await _service.EditSystemFunGroupDetail(systemFunGroup);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{funControllerID}")]
        public async Task<IActionResult> DeleteSystemFunGroup([FromRoute] string sysID, [FromRoute] string funControllerID, [FromQuery] string execSysID, [FromQuery] string execIpAddress)
        {
            var result = await _service.DeleteSystemFunGroupDetail(sysID, funControllerID, ClientUserID, execSysID, execIpAddress);
            if (result == EnumDeleteSystemFunGroupDetailResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }
    }
}
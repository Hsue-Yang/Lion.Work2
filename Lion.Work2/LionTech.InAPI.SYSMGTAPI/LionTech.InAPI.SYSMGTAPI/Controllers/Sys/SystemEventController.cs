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
    [Route("v1/SystemEvent")]
    public partial class SystemEventController : ControllerBase
    {
        private readonly ISysEventService _service;
        private readonly string ClientUserID;

        public SystemEventController(ISysEventService service, IHttpContextAccessor httpContext)
        {
            ClientUserID = httpContext.HttpContext.Request.Query["ClientUserID"];
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Names")]
        public async Task<IActionResult> QuerySystemEventFullName([FromRoute] string sysID, [FromQuery] string eventGroupID, [FromQuery] string eventID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEventFullName(sysID, eventGroupID, eventID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/{eventGroupID}/Ids")]
        public async Task<IActionResult> QuerySystemEventByIds([FromRoute] string sysID, [FromRoute] string eventGroupID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEventByIdList(sysID, eventGroupID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemEvents([FromRoute] string sysID, [FromQuery] string eventGroupID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemEventList(sysID, eventGroupID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemEvents = result.SystemEventList
            });
        }

        [HttpGet]
        [Route("{sysID}/{eventGroupID}/{eventID}")]
        public async Task<IActionResult> QuerySystemEvent([FromRoute] string sysID, [FromRoute] string eventGroupID, [FromRoute] string eventID)
        {
            var result = await _service.GetSystemEventDetail(sysID, eventGroupID, eventID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemEvent([FromBody] SystemEventMain systemEvent)
        {
            await _service.EditSystemEventDetail(systemEvent);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{eventGroupID}/{eventID}")]
        public async Task<IActionResult> DeleteSystemEvent([FromRoute] string sysID, [FromRoute] string eventGroupID, [FromRoute] string eventID)
        {
            var result = await _service.DeleteSystemEventDetail(sysID, eventGroupID, eventID);
            if (result == EnumDeleteResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }
    }
}
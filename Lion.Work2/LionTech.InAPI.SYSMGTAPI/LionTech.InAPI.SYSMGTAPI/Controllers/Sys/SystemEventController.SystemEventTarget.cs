using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEventController
    {
        [HttpGet]
        [Route("Target/{sysID}/{eventGroupID}/{eventID}")]
        public async Task<IActionResult> QuerySystemEventTargets([FromRoute] string sysID, [FromRoute] string eventGroupID, [FromRoute] string eventID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEventTargetList(sysID, ClientUserID, eventGroupID, eventID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Target")]
        public async Task<IActionResult> EditSystemEventTarget([FromBody] SystemEventTarget systemEventTarget)
        {
            await _service.EditSystemEventTarget(systemEventTarget);           
            return Ok();
        }

        [HttpDelete]
        [Route("Target/{sysID}/{eventGroupID}/{eventID}/{targetSysID}")]
        public async Task<IActionResult> DeleteSystemEventTarget([FromRoute] string sysID, [FromRoute] string eventGroupID, [FromRoute] string eventID, [FromRoute] string targetSysID)
        {
            await _service.DeleteSystemEventTarget(sysID, eventGroupID, eventID, targetSysID);
            return Ok();
        }

        [HttpGet]
        [Route("Target/IDs/{sysID}/{eventGroupID}/{eventID}")]
        public async Task<IActionResult> QuerySystemEventTargetIDs([FromRoute] string sysID, [FromRoute] string eventGroupID, [FromRoute] string eventID)
        {
            var result = await _service.GetSystemEventTargetIDs(sysID, eventGroupID, eventID);
            return Ok(new
            {
                SystemEventTargetIDs = result
            });
        }
    }
}
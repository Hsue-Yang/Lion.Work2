using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEventController
    {
        [HttpGet]
        [Route("EDI")]
        public async Task<IActionResult> QuerySystemEventEDIs([FromQuery]string sysID, [FromQuery]string targetSysID, [FromQuery]string eventGroupID, [FromQuery]string eventID, [FromQuery]string dtBegin, [FromQuery]string dtEnd, [FromQuery]string isOnlyFail, [FromQuery]string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemEventEDIList(sysID, ClientUserID, targetSysID, eventGroupID, eventID, dtBegin, dtEnd, isOnlyFail, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemEventEDIs = result.SystemEventEDIList
            });
        }

        [HttpGet]
        [Route("TargetEDI/{sysID}/{eventGroupID}/{eventID}")]
        public async Task<IActionResult> QuerySystemEventTargetEDIs([FromRoute] string sysID, [FromQuery]string targetSysID, [FromRoute] string eventGroupID, [FromRoute] string eventID, [FromQuery]string dtBegin, [FromQuery]string dtEnd, [FromQuery]string isOnlyFail, [FromQuery]string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemEventTargetEDIList(sysID, ClientUserID, targetSysID, eventGroupID, eventID, dtBegin, dtEnd, isOnlyFail, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemEventTargetEDIs = result.SystemEventTargetEDIList
            });
        }

        [HttpPost]
        [Route("ExcuteSubscription")]
        public async Task<IActionResult> ExcuteSubscription([FromBody] SystemEDIEvent systemEDIEvent)
        {
            var ediEventNo = await _service.ExcuteSubscription(systemEDIEvent);
            return Ok(new
            {
                EDIEventNo = ediEventNo
            });
        }
    }
}
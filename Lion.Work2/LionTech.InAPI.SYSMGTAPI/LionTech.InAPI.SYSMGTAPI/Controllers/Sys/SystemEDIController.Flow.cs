using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEDIController
    {
        [HttpGet]
        [Route("Flow/{sysID}")]
        public async Task<IActionResult> QuerySystemEDIFlows([FromRoute] string sysID, [FromQuery] string schFrequency, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIFlows(sysID, schFrequency, cultureID);
            return Ok(new
            {
                systemEDIFlowList = result
            });
        }

        [HttpPost]
        [Route("Flow/Sort")]
        public async Task<IActionResult> EditEDIFlowSetting([FromBody] List<SystemEDIFlowSort> systemEDIFlowSort)
        {
            await _service.EditEDIFlowSettingSort(systemEDIFlowSort);
            return Ok();
        }

        [HttpGet]
        [Route("Flow/{sysID}/IP")]
        public async Task<IActionResult> QuerySystemEDIIPAddress([FromRoute] string sysID)
        {
            var result = await _service.GetSystemEDIIPAddress(sysID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Flow/{sysID}/{ediflowID}/Detail")]
        public async Task<IActionResult> QuerySystemEDIFlowDetail([FromRoute] string sysID, [FromRoute] string ediflowID)
        {
            var result = await _service.GetSystemEDIFlowDetail(sysID, ediflowID);
            return Ok(new
            {
                systemEDIFlowDetails = result
            });
        }

        [HttpPost]
        [Route("Flow/{sysID}/{ediflowID}")]
        public async Task<IActionResult> EditSystemEDIFlowDetail([FromRoute] string sysID, [FromRoute] string ediflowID, [FromBody] SystemEDIFlowDetail systemEDIFlowDetail)
        {
            await _service.EditSystemEDIFlowDetail(systemEDIFlowDetail);
            return Ok();
        }

        [HttpDelete]
        [Route("Flow/{sysID}/{ediflowID}")]
        public async Task<IActionResult> DeleteSystemEDIFlowDetail([FromRoute] string sysID, [FromRoute] string ediflowID)
        {
            var result = await _service.DeleteSystemEDIFlowDetail(sysID, ediflowID);
            if (result == EnumDeleteResult.Success || result == EnumDeleteResult.DataExist)
            {
                return Ok(result.ToString());
            }
            return BadRequest(result.ToString());
        }

        [HttpGet]
        [Route("Flow/{sysID}/SortOrder")]
        public async Task<IActionResult> QueryGetSortOrder([FromRoute] string sysID)
        {
            var result = await _service.GetFlowNewSortOrder(sysID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Flow/ScheduleList/{sysID}")]
        public async Task<IActionResult> QuerySystemEDIFlowScheduleList([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIFlowScheduleList(sysID, cultureID);
            return Ok(new
            {
                systemEDIFlowScheduleList = result
            });
        }

        [HttpGet]
        [Route("Flow/Ids/{sysID}")]
        public async Task<IActionResult> QuerySystemEDIFlowByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIFlowByIds(sysID, cultureID);
            return Ok(new
            {
                systemEDIFlowByIds = result
            });
        }

        [HttpGet]
        [Route("Flow/XML/{sysID}")]
        public async Task<IActionResult> QuerySystemEDIFlowXMLDetail([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIFlowXMLDetail(sysID, cultureID);
            return Ok(result);
        }
    }
}
